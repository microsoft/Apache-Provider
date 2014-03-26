/*--------------------------------------------------------------------------------
 *        Copyright (c) Microsoft Corporation. All rights reserved. See license.txt for license information.
 *     
 *        */
 /**
      \file        apachebinding.h

      \brief       Provider helper functions for Apache module/provider bindings

      \date        03-04-14
*/
/*----------------------------------------------------------------------------*/

#ifndef APACHEBINDING_H
#define APACHEBINDING_H

// Apache Portable Runtime definitions
#include "apr.h"
#include "apr_errno.h"
#include "apr_global_mutex.h"
#include "apr_shm.h"

#include "mmap_region.h"


/*------------------------------------------------------------------------------*/
/**
 *   ApacheBinding 
 *   Helper class to help with Apache module <-> Provider bindings
 */
class ApacheBinding
{
public:
    ApacheBinding() {};
    ~ApacheBinding() {};

    static apr_status_t OMI_Error(int err) { return APR_OS_START_USERERR + err; }
    static void DisplayError(apr_status_t status, const char *text);
    static apr_status_t Load(const char *text);
    static apr_status_t Unload(const char *text);

    static const char *GetServerConfigFile() { return ms_server_data->configFile; }
    static const char *GetServerProcessName() { return ms_server_data->processName; }
    static int GetOperatingStatus() { return ms_server_data->operatingStatus; }
    static apr_size_t GetModuleCount() { return ms_server_data->moduleCount; }
    static mmap_server_modules *GetServerModules() { return ms_server_data->modules; }
    static apr_uint32_t GetWorkerCountIdle() { return ms_server_data->idleWorkers; }
    static apr_uint32_t GetWorkerCountBusy() { return ms_server_data->busyWorkers; }

    static apr_size_t GetVHostCount() { return ms_vhost_data->count; }
    static mmap_vhost_elements *GetVHostElements() { return ms_vhost_data->vhosts; }

    static apr_status_t LockMutex() { return apr_global_mutex_lock(ms_mutexMapRW); }
    static apr_status_t UnlockMutex() { return apr_global_mutex_unlock(ms_mutexMapRW); }

private:
    static apr_pool_t *ms_apr_pool;
    static apr_shm_t *ms_mmap_region;
    static mmap_server_data *ms_server_data;
    static mmap_vhost_data *ms_vhost_data;

    static apr_global_mutex_t *ms_mutexMapRW;

    static int ms_loadCount;
};

extern ApacheBinding g_apache;

#endif /* APACHEBINDING_H */

/*----------------------------E-N-D---O-F---F-I-L-E---------------------------*/
