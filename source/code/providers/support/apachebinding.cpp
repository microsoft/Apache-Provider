/*--------------------------------------------------------------------------------
 *        Copyright (c) Microsoft Corporation. All rights reserved. See license.txt for license information.
 *      
 *           */
 /**
        \file        apache_interface.cpp

        \brief       Provider helper functions for Apache module/provider bindings

        \date        03-04-14
*/
/*----------------------------------------------------------------------------*/

#include <stdio.h>
#include "apachebinding.h"

// Static member initialization
apr_pool_t *ApacheBinding::ms_apr_pool = NULL;
apr_shm_t *ApacheBinding::ms_mmap_region = NULL;
mmap_server_data *ApacheBinding::ms_server_data = NULL;
mmap_vhost_data *ApacheBinding::ms_vhost_data = NULL;
apr_global_mutex_t *ApacheBinding::ms_mutexMapRW = NULL;
int ApacheBinding::ms_loadCount = 0;

void ApacheBinding::DisplayError(apr_status_t status, const char *text)
{
    char buffer[256];
    char *errString = apr_strerror(status, buffer, sizeof(buffer));
    fprintf(stderr, "%s, status=%d (%s)\n",
            (text != NULL ? text : "Unexpected error occurred"),
            status, (errString != NULL ? errString : "Unknown error text"));
    return;
}

apr_status_t ApacheBinding::Load(const char *text)
{
    if ( 1 == ++ms_loadCount )
    {
        apr_status_t status;

        // Initialize the APR (Apache Portable Runtime)
        fprintf(stdout, "%s: Initializing the APR ...\n", text ? text : "Unspecified");
        status = apr_initialize();
        if (APR_SUCCESS != status)
        {
            DisplayError(status, "failed to initialize APR");
            return status;
        }

        fprintf(stdout, "%s: Creating memory pool ...\n", text ? text : "Unspecified");
        status = apr_pool_create(&ms_apr_pool, NULL);
        if (APR_SUCCESS != status)
        {
            DisplayError(status, "failed to create memory pool");
            return status;
        }

        // Initialize the mutex that we need
        // FAIL???  We can lock the INIT lock when Apache should be holding, investigate!
        fprintf(stdout, "%s: Creating mutex ...\n", text ? text : "Unspecified");
        status = apr_global_mutex_create(&ms_mutexMapRW, MUTEX_INIT_NAME, APR_LOCK_DEFAULT, ms_apr_pool);
        if (APR_SUCCESS != status)
        {
            DisplayError(status, "failed to initialize the RW mutex");
            return status;
        }

        /*
          fprintf(stdout, "ApacheHTTPD: Locking as a test ...\n");
          status = apr_global_mutex_lock(mutexMapRW);
          fprintf(stdout, "!!!FAIL!!! ApacheHTTPD: Lock worked???\n");
        */

        // Map the shared memory region
        fprintf(stdout, "%s: Mapping region ...\n", text ? text : "Unspecified");
        status = apr_shm_attach(&ms_mmap_region, PROVIDER_MMAP_NAME, ms_apr_pool);
        if (APR_SUCCESS != status)
        {
            DisplayError(status, "failed to map shared memory");
            return status;
        }

        /* Assign global pointers */
        ms_server_data = reinterpret_cast<mmap_server_data*> (apr_shm_baseaddr_get(ms_mmap_region));
        ms_vhost_data = reinterpret_cast<mmap_vhost_data*> (ms_server_data->modules + ms_server_data->moduleCount);

        /* TODO: Launch thread to count time-based statistics */

    }

    return APR_SUCCESS;
}

int ApacheBinding::Unload(const char *text)
{
    if (0 == --ms_loadCount)
    {
        apr_status_t status;

        // Unmap the shared memory region and clean up resources
        status = apr_shm_detach(ms_mmap_region);
        if (APR_SUCCESS != status)
        {
            DisplayError(status, "failed to unmap shared memory");
            return status;
        }

        status = apr_global_mutex_destroy(ms_mutexMapRW);
        if (APR_SUCCESS != status)
        {
            DisplayError(status, "failed to clean up mutex");
            return status;
        }

        apr_pool_clear(ms_apr_pool);
        apr_terminate();
    }

    return APR_SUCCESS;
}

// Only construct class once - no need to load shared memory multime times
ApacheBinding g_apache;

/*----------------------------E-N-D---O-F---F-I-L-E---------------------------*/
