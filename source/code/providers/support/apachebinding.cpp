/*
 *--------------------------------- START OF LICENSE ----------------------------
 *
 * Apache Cimprov ver. 1.0
 *
 * Copyright (c) Microsoft Corporation
 *
 * All rights reserved. 
 *
 * Licensed under the Apache License, Version 2.0 (the License); you may not use
 * this file except in compliance with the license. You may obtain a copy of the
 * License at http://www.apache.org/licenses/LICENSE-2.0 
 *
 * THIS CODE IS PROVIDED ON AN *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED
 * WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE,
 * MERCHANTABLITY OR NON-INFRINGEMENT.
 *
 * See the Apache Version 2.0 License for specific language governing permissions
 * and limitations under the License.
 *
 *---------------------------------- END OF LICENSE -----------------------------
 */
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
#include "utils.h"

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

/*--------------------------------------------------------------*/
/**
   Checks and validates if the shared memory region is valid or not

   \returns     APR_SUCCESS if no errors occured, error code otherwise

   Note that error is logged normally prior to return from this function,
   so error code is for informaitonal purposes only.

   This method should NOT be used to check if shared memory segment is valid;
   this method will actually perform (costly) checks to find out.


   Note: It is assumed that ApacheDataCollector is already attached to the
   shared memory segment. If this is not the case, we are likely to segfault
   since ApacheDataCollector is defined to segfault if used when Attach()
   fails.
*/
apr_status_t ApacheInitDependencies::ValidateSharedMemory(ApacheDataCollector& data)
{
    /*
      Our algorithm is as follows:

      Get the server PID (for Apache), and read Linux process stat file to get
      the process name.

      If the process name doesn't contain "http" or "apache", then assume that
      the process is re-used for something else.

      If process name contains "http" or "apache", we're good!
     */

    class DisplayInvalid
    {
    public:
        DisplayInvalid() : m_isValid(false)
            { DisplayError(0, "ValidateSharedMemory: Checking region ..."); }
        ~DisplayInvalid()
            { if (!m_isValid) DisplayError(0, "ValidateSharedMemory: Apache server appears dead!"); }
        void MarkValid() { m_isValid = true; }

    private:
        bool m_isValid;
    } invalidDisplay;

    apr_status_t status;
    apr_pool_t* pool = data.GetPool();
    pid_t serverPID = data.GetServerPID();
    char *text;

    const char *fname = apr_psprintf(pool, "/proc/%d/stat", serverPID);
    apr_file_t *fHandle;
    if (APR_SUCCESS != (status = apr_file_open(&fHandle, fname, APR_FOPEN_READ, APR_FPROT_UREAD, pool)))
    {
        // APR_ENOENT is expected if the process no longer exists
        if (APR_ENOENT != status)
        {
            text = apr_psprintf(pool, "ValidateSharedMemory: Error opening Apache process stat file: %s", fname);
            DisplayError(status, text);
        }

        m_bIsRegionValid = false;
        return status;
    }

    // The process is still alive; validate the process name to make sure it's still Apache!

    char statBuffer[256];
    apr_size_t bytes = sizeof(statBuffer) - 1;
    if (APR_SUCCESS != (status = apr_file_read(fHandle, statBuffer, &bytes)))
    {
        text = apr_psprintf(pool, "ValidateSharedMemory: Error reading Apache process stat file: %s", fname);
        DisplayError(status, text);

        m_bIsRegionValid = false;
        return status;
    }
    statBuffer[bytes] = '\0';

    // The stat file is something like:
    //
    // 22646 (httpd) S 1 22646 22646 0 -1 4202560 1166 ...
    //
    // We only care about the second field (process name)

    char *last;
    apr_strtok(statBuffer, " ", &last);
    char *processName = apr_strtok(NULL, " ", &last);
    if ('(' == processName[0])
    {
        // Get rid of trailing ")" byte
        processName[strlen(processName)-1] = '\0';
        m_processName = &processName[1];
    }
    else
    {
        m_processName = processName;
    }

    if (APR_SUCCESS != (status = apr_file_close(fHandle)))
    {
        DisplayError(status, "ValidateSharedMemory: Error closing Apache process stat file");
        // Fall through - don't abort for this, just report the error
    }

    // Validate if the process name is what we expect
    // If not, we don't log an error here; but we do report failure

    std::string procNameLower = StrToLower(m_processName);
    if (procNameLower.find("apache") == std::string::npos
        && procNameLower.find("http") == std::string::npos)
    {
        status = APR_EINVAL;
        text = apr_psprintf(pool, "ValidateSharedMemory: Process stat file %s does not appear to belong to Apache", fname);
        DisplayError(status, text);

        m_bIsRegionValid = false;
        return status;
    }

    invalidDisplay.MarkValid();
    m_bIsRegionValid = true;
    return APR_SUCCESS;
}

/*--------------------------------------------------------------*/
/**
   Returns process name of the Apache process

   \param       processName     Name of the Apache process (for reporting)
*/
void ApacheInitDependencies::GetApacheProcessName(std::string& processName)
{
    if ( IsSharedMemoryValid() )
    {
        processName = m_processName;
    }
    else
    {
        processName.erase();
    }
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
    apr_status_t status;

    // Map to the shared memory region
    if (APR_SUCCESS != (status = m_pDeps->Attach(text, m_apr_pool, &m_server_data, &m_vhost_data, &m_certificate_data, &m_string_data)))
    {
        return status;
    }

    // We are mapped to a region; verify if it's actually valid
    //
    // Note: If it's not valid, then immediately validate again (since we are loaded)
    // In this way, if it wasn't valid, but is now, we don't give false negatives.
    // Depend on data collector thread to insure actual problems are caught in a
    // timely manner.

    if (g_pFactory->GetInit()->IsSharedMemoryValid())
    {
        return APR_SUCCESS;
    }

    if (APR_SUCCESS != (status = g_pFactory->GetInit()->ValidateSharedMemory(*this)))
    {
        return status;
    }

    return APR_SUCCESS;
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
