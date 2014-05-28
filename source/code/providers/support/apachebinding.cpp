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
#include <unistd.h>
#include <apr_strings.h>

#include "apachebinding.h"
#include "datasampler.h"

// Define single global copy - no need to load shared memory multiple times
ApacheBinding g_apache;

bool ApacheBinding::sm_fAprInitialized = false;


void ApacheBinding::DisplayError(apr_status_t status, const char *text)
{
    char dateStr[APR_CTIME_LEN];
    apr_ctime(dateStr, apr_time_now());

    /* APR_OS_START_USERERR used for OMI errors; anything below is APR error */

    if (0 == status)
    {
        if ( m_fStatusOutput )
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

apr_status_t ApacheBinding::Load(const char *text)
{
    if ( 1 == ++m_loadCount )
    {
        apr_status_t status;
        char buffer[256];

        // One time initialization ...
        if ( APR_SUCCESS != (status = Initialize()) )
        {
            return status;
        }

        snprintf(buffer, sizeof(buffer), "ApacheBinding::Load (%s): Creating memory pool", text ? text : "Unspecified");
        DisplayError(0, buffer);
        if (APR_SUCCESS != (status = apr_pool_create(&m_apr_pool, NULL)))
        {
            snprintf(buffer, sizeof(buffer), "ApacheBinding::Load (%s): Failed to create memory pool", text ? text : "Unspecified");
            DisplayError(status, buffer);
            return status;
        }

        /*
         * Now we have the APR tool - slightly easier to deal with output using it
         */

        // Initialize the mutex that we need
        // FAIL???  We can lock the INIT lock when Apache should be holding, investigate!
        DisplayError(0, apr_psprintf(m_apr_pool, "ApacheBinding::Load (%s): Creating mutex", text ? text : "Unspecified"));
        if (APR_SUCCESS != (status = apr_global_mutex_create(&m_mutexMapRW, MUTEX_INIT_NAME, APR_LOCK_DEFAULT, m_apr_pool)))
        {
            DisplayError(status, apr_psprintf(m_apr_pool, "ApacheBinding::Load (%s): failed to initialize the RW mutex", text ? text : "Unspecified"));
            return status;
        }

        /*
          fprintf(stdout, "ApacheHTTPD: Locking as a test ...\n");
          status = apr_global_mutex_lock(mutexMapRW);
          fprintf(stdout, "!!!FAIL!!! ApacheHTTPD: Lock worked???\n");
        */

        // Map the shared memory region
        DisplayError(0, apr_psprintf(m_apr_pool, "ApacheBinding::Load (%s): Mapping region", text ? text : "Unspecified"));
        if (APR_SUCCESS != (status = apr_shm_attach(&m_mmap_region, PROVIDER_MMAP_NAME, m_apr_pool)))
        {
            DisplayError(0, apr_psprintf(m_apr_pool, "ApacheBinding::Load (%s): failed to map shared memory", text ? text : "Unspecified"));
            return status;
        }

        /* Assign global pointers */
        m_server_data = reinterpret_cast<mmap_server_data*> (apr_shm_baseaddr_get(m_mmap_region));
        m_vhost_data = reinterpret_cast<mmap_vhost_data*> (m_server_data->modules + m_server_data->moduleCount);
        m_certificate_data = reinterpret_cast<mmap_certificate_data*> (m_vhost_data->vhosts + m_vhost_data->count);
        m_string_data = reinterpret_cast<mmap_string_table*> (m_certificate_data->certificates + m_certificate_data->count);

        /* Launch thread to count time-based statistics */
        DisplayError(0, apr_psprintf(m_apr_pool, "ApacheBinding::Load (%s): Launching worker thread", text ? text : "Unspecified"));
        if (APR_SUCCESS != (status = m_sampler.Launch()))
        {
            return status;
        }
    }

    return APR_SUCCESS;
}

int ApacheBinding::Unload(const char *text)
{
    if (0 == --m_loadCount)
    {
        apr_status_t status;

        // Shut down the sampler thread
        if (APR_SUCCESS != (status = m_sampler.WaitForCompletion()))
        {
            DisplayError(status, apr_psprintf(m_apr_pool, "ApacheBinding::Unload (%s): failed to terminate worker thread", text ? text : "Unspecified"));
            // Shouldn't terminate shutdown for this - fall through
        }

        // Unmap the shared memory region and clean up resources
        if (APR_SUCCESS != (status = apr_shm_detach(m_mmap_region)))
        {
            DisplayError(status, apr_psprintf(m_apr_pool, "ApacheBinding::Unload (%s): failed to unmap shared memory", text ? text : "Unspecified"));
            // Shouldn't terminate shutdown for this - fall through
        }

        if (APR_SUCCESS != (status = apr_global_mutex_destroy(m_mutexMapRW)))
        {
            DisplayError(status, apr_psprintf(m_apr_pool, "ApacheBinding::Unload (%s): failed to clean up mutex", text ? text : "Unspecified"));
            // Shouldn't terminate shutdown for this - fall through
        }

        apr_pool_clear(m_apr_pool);

        // Don't bother terminating the APR - makes unit tests easier,
        // and no real point if we're just going to exit the process anyway
    }

    return APR_SUCCESS;
}

const char* ApacheBinding::GetDataString(apr_size_t offset)
{
    if (offset == 0 || (apr_size_t)offset >= m_string_data->total_length)
    {
        return "";
    }
    return m_string_data->data + offset;
}

apr_status_t ApacheBinding::Initialize()
{
    // Initialize the APR (Apache Portable Runtime)
    if (! sm_fAprInitialized)
    {
        apr_status_t status;
        char buffer[256];

        sm_fAprInitialized = true;

        snprintf(buffer, sizeof(buffer), "ApacheBinding::Load: Initializing the APR");
        DisplayError(0, buffer);

        if (APR_SUCCESS != (status = apr_initialize()))
        {
            snprintf(buffer, sizeof(buffer), "ApacheBinding::Load: Failed to initialize APR");
            DisplayError(status, buffer);
            return status;
        }
    }

    return APR_SUCCESS;
}

/*----------------------------E-N-D---O-F---F-I-L-E---------------------------*/
