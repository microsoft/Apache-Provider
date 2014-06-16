/*--------------------------------------------------------------------------------
 *        Copyright (c) Microsoft Corporation. All rights reserved. See license.txt for license information.
 *      
 *           */
 /**
        \file        apachebinding.cpp

        \brief       Provider helper functions for Apache module/provider bindings

        \date        03-04-14
*/
/*----------------------------------------------------------------------------*/

#include <stdio.h>
#include <string.h>
#include <unistd.h>
#include <apr_strings.h>

#include "apachebinding.h"
#include "datasampler.h"

// Define single global copy (intended as a singleton class)
ApacheFactory* g_pFactory = NULL;

bool ApacheInitialization::sm_fAprInitialized = false;



ApacheInitDependencies::~ApacheInitDependencies()
{
}

/*--------------------------------------------------------------*/
/**
   Gets the server configuration file

   \returns        Full path to server configuration file, or NULL

   Apache doesn't dependably return the top level configuration file. Testing has
   indicated that (at least on SLES 11) uid.conf can be returned rather than httpd.conf,
   even though httpd.conf is the top level configuration file.

   This class works with ApacheInitialization::GetServerConfigFile. If we can find the real
   configuration file via #define options (from httpd -V), then we use that. Otherwise,
   we return NULL and let ApacheDataCollector::GetServerConfigFile report what Apache told us
   (which could be wrong, but at least we tried to do better).

   httpd -V returns something like:
        Server version: Apache/2.2.15 (Unix)
        Server built:   Apr  9 2011 08:58:28
        Server's Module Magic Number: 20051115:24
        Server loaded:  APR 1.3.9, APR-Util 1.3.9
        Compiled using: APR 1.3.9, APR-Util 1.3.9
        Architecture:   64-bit
        Server MPM:     Prefork
          threaded:     no
            forked:     yes (variable process count)
        Server compiled with....
         -D APACHE_MPM_DIR="server/mpm/prefork"
         -D APR_HAS_SENDFILE
         -D APR_HAS_MMAP
         -D APR_HAVE_IPV6 (IPv4-mapped addresses enabled)
         -D APR_USE_SYSVSEM_SERIALIZE
         -D APR_USE_PTHREAD_SERIALIZE
         -D SINGLE_LISTEN_UNSERIALIZED_ACCEPT
         -D APR_HAS_OTHER_CHILD
         -D AP_HAVE_RELIABLE_PIPED_LOGS
         -D DYNAMIC_MODULE_LIMIT=128
         -D HTTPD_ROOT="/etc/httpd"
         -D SUEXEC_BIN="/usr/sbin/suexec"
         -D DEFAULT_PIDLOG="run/httpd.pid"
         -D DEFAULT_SCOREBOARD="logs/apache_runtime_status"
         -D DEFAULT_LOCKFILE="logs/accept.lock"
         -D DEFAULT_ERRORLOG="logs/error_log"
         -D AP_TYPES_CONFIG_FILE="conf/mime.types"
         -D SERVER_CONFIG_FILE="conf/httpd.conf"
   In particular, we care about SERVER_CONFIG_FILE and HTTPD_ROOT, if
   SERVER_CONFIG_FILE isn't an absolute path.
*/
const char* ApacheInitDependencies::GetServerConfigFile(apr_pool_t* pool)
{
    if ( !m_configFileAttempted )
    {
        char buffer[128];
        apr_status_t status;
        apr_proc_t proc;
        apr_procattr_t *pattr;
        std::string rootDir, configFile;

        m_configFileAttempted = true;

        // Try to run twice (two possible binary names)
        const int loopLimit = 1;
        for (int i = 0; i <= loopLimit; i++)
        {
            // Initialize the process attribute
            status = apr_procattr_create(&pattr, pool);
            if (status != APR_SUCCESS)
            {
                DisplayError(status, "GetServerConfigFile: error creating child process attributes");
                return NULL;
            }

            // Set up the pipe of stdout from the child to this process' proc.out
            status = apr_procattr_io_set(pattr, APR_NO_PIPE, APR_FULL_BLOCK, APR_NO_PIPE);
            if (status != APR_SUCCESS)
            {
                DisplayError(status, "GetServerConfigFile: error setting child process i/o attributes");
                return NULL;
            }

            // Make the httpd/apache2ctl program be run using the PATH variable
            status = apr_procattr_cmdtype_set(pattr, APR_PROGRAM_PATH);
            if (status != APR_SUCCESS)
            {
                DisplayError(status, "GetServerConfigFile: error setting child process command type");
                return NULL;
            }

            // Run the binary
            const char *progname;
            if (0 == i)
            {
                progname = "apache2ctl";
                const char* prog_args[] =
                {
                    "apache2ctl",
                    "-V",
                    NULL
                };
                char* const* argptr = const_cast<char* const*>(prog_args);
                status = apr_proc_create(&proc, progname, argptr, NULL, (apr_procattr_t*)pattr, pool);
            }
            else if (1 == i)
            {
                progname = "httpd";
                const char* prog_args[] =
                {
                    "httpd",
                    "-V",
                    NULL
                };
                char* const* argptr = const_cast<char* const*>(prog_args);
                status = apr_proc_create(&proc, progname, argptr, NULL, (apr_procattr_t*)pattr, pool);
            }
            else
            {
                DisplayError(status, "GetServerConfigFile: unknown program to run");
                return NULL;
            }

            if (status != APR_SUCCESS)
            {
                char *text = apr_psprintf(pool, "GetServerConfigFile: error creating child process for %s", progname);
                DisplayError(status, text);
                return NULL;
            }

            // Drain the output from the child process, grabbing what we need
            while (APR_SUCCESS == (status = apr_file_gets(buffer, sizeof(buffer), proc.out)))
            {
                char* substrRoot = strstr(buffer, "-D HTTPD_ROOT=\"");
                char* substrConfig = strstr(buffer, "-D SERVER_CONFIG_FILE=\"");

                if (substrRoot != NULL || substrConfig != NULL)
                {
                    // We found something - figure out the value between the quotes
                    // (i.e.  HTTPD_ROOT="/etc/httpd")

                    char *valStart = strstr(buffer, "\"") + 1;
                    char *valEnd = strstr(valStart, "\"");

                    // Null-terminate the value (write null over ending quote)
                    if (valEnd != NULL)
                    {
                        *valEnd = '\0';
                    }

                    // Save the value that we found
                    if (substrRoot)
                    {
                        rootDir = valStart;
                    }
                    else if (substrConfig)
                    {
                        configFile = valStart;
                    }
                }
            }

            if (status != APR_SUCCESS && status != APR_EOF)
            {
                char *text = apr_psprintf(pool, "GetServerConfigFile: error reading process output for %s", progname);
                DisplayError(status, text);
                return NULL;
            }

            // Wait for the child process to finish
            apr_exit_why_e why;
            int exitCode;
            status = apr_proc_wait(&proc, &exitCode, &why, APR_WAIT);
            if (!APR_STATUS_IS_CHILD_DONE(status))
            {
                char *text = apr_psprintf(pool, "GetServerConfigFile: process did not finish successfully for %s", progname);
                DisplayError(status, text);
                return NULL;
            }

            if (exitCode != 0)
            {
                if (i < loopLimit)
                {
                    continue;
                }

                char *text = apr_psprintf(pool, "GetServerConfigFile: process %s failed with status %d", progname, exitCode);
                DisplayError(status, text);
                return NULL;
            }

            // If not absolute path, prepend root directory to configuration file
            if (configFile[0] != '/' && rootDir.length())
            {
                m_configFile = std::string(rootDir) + std::string("/") + configFile;
            }
            else
            {
                m_configFile = configFile;
            }

            break;
        }
    }

    return (m_configFile.length() ? m_configFile.c_str() : NULL);
}



apr_status_t ApacheDataCollectorDependencies::Attach(
    const char *text, apr_pool_t* pool,
    mmap_server_data** p_svr, mmap_vhost_data** p_vhost,
    mmap_certificate_data** p_cert, mmap_string_table** p_str)
{
    apr_status_t status;

    // Save the pool that we used for the attach (used for destruction)
    m_apr_attach_pool = pool;

    DisplayError(0, apr_psprintf(pool, "ApacheDataCollectorDependencies::Attach (%s): Attaching", text));

    // Initialize the mutex that we need
    if (APR_SUCCESS != (status = InitializeMutex()))
    {
        DisplayError(status, apr_psprintf(pool, "ApacheDataCollectorDependencies::Attach (%s): failed to initialize the RW mutex", text));
        return status;
    }

    // Map the shared memory region
    status = LoadMemoryMap(p_svr, p_vhost, p_cert, p_str);
    if (APR_SUCCESS != status)
    {
        DisplayError(0, apr_psprintf(pool, "ApacheDataCollector::Attach (%s): failed to map shared memory", text));
        return status;
    }

    return APR_SUCCESS;
}

apr_status_t ApacheDataCollectorDependencies::Detach(const char *text)
{
    apr_status_t statusMap, statusMutex;

    DisplayError(0, apr_psprintf(m_apr_attach_pool, "ApacheDataCollector::Detach (%s): Detaching", text));

    // Unmap the shared memory region and clean up resources
    if (APR_SUCCESS != (statusMap = UnloadMemoryMap()))
    {
        DisplayError(statusMap, apr_psprintf(m_apr_attach_pool, "ApacheDataCollector::Detach (%s): failed to unmap shared memory", text));
        // Shouldn't terminate shutdown for this - fall through
    }

    if (APR_SUCCESS != (statusMutex = DestroyMutex()))
    {
        DisplayError(statusMutex, apr_psprintf(m_apr_attach_pool, "ApacheDataCollector::Detach (%s): failed to clean up mutex", text));
        // Shouldn't terminate shutdown for this - fall through
    }

    // Return "must important" error codes first
    if (APR_SUCCESS != statusMap)
        return statusMap;
    else if (APR_SUCCESS != statusMutex)
        return statusMutex;

    return APR_SUCCESS;
}

apr_status_t ApacheDataCollectorDependencies::InitializeMutex()
{
    return apr_global_mutex_create(&m_mutexMapRW, MUTEX_INIT_NAME, APR_LOCK_DEFAULT, m_apr_attach_pool);
}

apr_status_t ApacheDataCollectorDependencies::DestroyMutex()
{
    if (NULL != m_mutexMapRW)
    {
        apr_status_t status = apr_global_mutex_destroy(m_mutexMapRW);
        m_mutexMapRW = NULL;
        return status;
    }

    return APR_SUCCESS;
}

apr_status_t ApacheDataCollectorDependencies::LoadMemoryMap(
    mmap_server_data** p_svr,
    mmap_vhost_data** p_vhost,
    mmap_certificate_data** p_cert,
    mmap_string_table** p_str)
{
    apr_status_t status;

    // Attach to the memory mapped file
    if (APR_SUCCESS != (status = apr_shm_attach(&m_mmap_region, PROVIDER_MMAP_NAME, m_apr_attach_pool)))
    {
        return status;
    }

    // Assign global pointers
    mmap_server_data*      svr   = reinterpret_cast<mmap_server_data*> (apr_shm_baseaddr_get(m_mmap_region));
    mmap_vhost_data*       vhost = reinterpret_cast<mmap_vhost_data*> (svr->modules + svr->moduleCount);
    mmap_certificate_data* cert  = reinterpret_cast<mmap_certificate_data*> (vhost->vhosts + vhost->count);
    mmap_string_table*     str   = reinterpret_cast<mmap_string_table*> (cert->certificates + cert->count);

    // Return pointers to the caller
    *p_svr = svr;
    *p_vhost = vhost;
    *p_cert = cert;
    *p_str = str;

    return APR_SUCCESS;
}

apr_status_t ApacheDataCollectorDependencies::UnloadMemoryMap()
{
    if (NULL != m_mmap_region)
    {
        apr_status_t status = apr_shm_detach(m_mmap_region);
        m_mmap_region = NULL;

        return status;
    }

    return APR_SUCCESS;
}



void ApacheInitialization::DisplayError(apr_status_t status, const char *text)
{
    char dateStr[APR_CTIME_LEN];
    apr_ctime(dateStr, apr_time_now());

    /* APR_OS_START_USERERR used for OMI errors; anything below is APR error */

    if (0 == status)
    {
        if ( m_pDeps->AllowStatusOutput() )
        {
            fprintf(stderr, "[%s] %s\n",
                    dateStr, (text != NULL ? text : "Unexpected error occurred"));
        }
    }
    else if (status < APR_OS_START_USERERR)
    {
        char buffer[256];
        char *errString = apr_strerror(status, buffer, sizeof(buffer));
        fprintf(stderr, "[%s] %s, status=%d (%s)\n",
                dateStr, (text != NULL ? text : "Unexpected error occurred"),
                status, (errString != NULL ? errString : "Unknown error text"));
    }
    else
    {
        /* TODO: OMI 1.0.8 allows you to convert OMI errors;
         * we should use that once we bind to OMI 1.0.8
         * For now, just use APR (which will just give numeric error) */

        char buffer[256];
        char *errString = apr_strerror(status, buffer, sizeof(buffer));
        fprintf(stderr, "[%s] %s, status=%d (%s)\n",
                dateStr, (text != NULL ? text : "Unexpected error occurred"),
                status, (errString != NULL ? errString : "Unknown error text"));
    }

    fflush(stderr);

    return;
}

apr_status_t ApacheInitialization::Load(const char *text)
{
    if ( 1 == ++m_loadCount )
    {
        apr_status_t status;

        // One time initialization ...
        if ( APR_SUCCESS != (status = Initialize(text)) )
        {
            return status;
        }
    }

    return APR_SUCCESS;
}

int ApacheInitialization::Unload(const char *text)
{
    /* Only deal with memory map if we're not in test mode */
    if (0 == --m_loadCount)
    {
        apr_pool_clear(m_apr_pool);

        // Don't bother terminating the APR - makes unit tests easier,
        // and no real point if we're just going to exit the process anyway
    }

    return APR_SUCCESS;
}

apr_status_t ApacheInitialization::Initialize(const char *text)
{
    apr_status_t status;
    char buffer[256];

    // Initialize the APR (Apache Portable Runtime)
    if (! sm_fAprInitialized)
    {
        snprintf(buffer, sizeof(buffer), "ApacheInitialization::Initialize: Initializing the APR");
        DisplayError(0, buffer);

        if (APR_SUCCESS != (status = apr_initialize()))
        {
            snprintf(buffer, sizeof(buffer), "ApacheInitialization::Initialize: Failed to initialize APR");
            DisplayError(status, buffer);
            return status;
        }

        sm_fAprInitialized = true;
    }

    if (NULL == m_apr_pool)
    {
        snprintf(buffer, sizeof(buffer), "ApacheInitialization::Initialize (%s): Creating memory pool", text ? text : "Unspecified");
        DisplayError(0, buffer);

        if (APR_SUCCESS != (status = apr_pool_create(&m_apr_pool, NULL)))
        {
            snprintf(buffer, sizeof(buffer), "ApacheInitialization::Initialize (%s): Failed to create memory pool", text ? text : "Unspecified");
            DisplayError(status, buffer);
            return status;
        }
    }

    // Launch the data collector
    if (APR_SUCCESS != (status = m_pDeps->LaunchDataCollector()))
    {
        return status;
    }

    return APR_SUCCESS;
}



ApacheDataCollector::ApacheDataCollector(ApacheDataCollectorDependencies* deps)
    : m_server_data(NULL), m_vhost_data(NULL),
      m_certificate_data(NULL), m_string_data(NULL),
      m_pDeps(deps), m_apr_pool(NULL)
{
    apr_status_t status;

    if (APR_SUCCESS != (status = apr_pool_create(&m_apr_pool, g_pFactory->GetInit()->GetPool())))
    {
        // TODO: Throw some kind of exception or something!
    }
}

ApacheDataCollector::~ApacheDataCollector()
{
    Detach("Destructor");
    apr_pool_clear(m_apr_pool);
}

apr_status_t ApacheDataCollector::Attach(const char *text)
{
    return m_pDeps->Attach(text, m_apr_pool, &m_server_data, &m_vhost_data, &m_certificate_data, &m_string_data);
}

apr_status_t ApacheDataCollector::Detach(const char *text)
{
    apr_status_t status = m_pDeps->Detach(text);

    m_server_data = NULL;
    m_vhost_data = NULL;
    m_certificate_data = NULL;
    m_string_data = NULL;

    return status;
}

const char* ApacheDataCollector::GetDataString(apr_size_t offset)
{
    if (offset == 0 || offset >= m_string_data->total_length)
    {
        return "";
    }

    return m_string_data->data + offset;
}

const char* ApacheDataCollector::GetServerConfigFile()
{
    const char* configFile = g_pFactory->GetInit()->GetServerConfigFile(m_apr_pool);

    return (configFile ? configFile : GetDataString(m_server_data->configFileOffset));
}

/*----------------------------E-N-D---O-F---F-I-L-E---------------------------*/
