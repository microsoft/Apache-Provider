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
#include <apr.h>
#include <apr_atomic.h>
#include <apr_errno.h>
#include <apr_global_mutex.h>
#include <apr_shm.h>
#include <apr_strings.h>

#include "mmap_region.h"
#include "datasampler.h"
#include "temppool.h"


/*------------------------------------------------------------------------------*/
/**
 *   ApacheBinding 
 *   Helper class to help with Apache module <-> Provider bindings
 */
class ApacheBinding
{
public:
    ApacheBinding()
        : m_fStatusOutput(true),
          m_apr_pool(NULL), m_mmap_region(NULL),
          m_server_data(NULL), m_vhost_data(NULL),
          m_certificate_data(NULL), m_string_data(NULL),
          m_mutexMapRW(NULL), m_loadCount(0)
        {};
    ~ApacheBinding() {};

    apr_status_t OMI_Error(int err) { return APR_OS_START_USERERR + err; }
    void DisplayError(apr_status_t status, const char *text);
    apr_status_t Load(const char *text);
    apr_status_t Unload(const char *text);

    const char* GetDataString(apr_size_t offset);

    const char *GetServerConfigFile() { return GetDataString(m_server_data->configFileOffset); }
    const char *GetServerProcessName() { return GetDataString(m_server_data->processNameOffset); }
    const char *GetServerVersion() { return GetDataString(m_server_data->serverVersionOffset); }
    int GetOperatingStatus() { return m_server_data->operatingStatus; }
    apr_size_t GetModuleCount() { return m_server_data->moduleCount; }
    mmap_server_modules *GetServerModules() { return m_server_data->modules; }
    apr_uint32_t GetWorkerCountIdle() { return apr_atomic_read32(&m_server_data->idleWorkers); }
    apr_uint32_t GetWorkerCountBusy() { return apr_atomic_read32(&m_server_data->busyWorkers); }
    apr_uint32_t GetCPUUtilization() { return apr_atomic_read32(&m_server_data->percentCPU); }

    apr_size_t GetVHostCount() { return m_vhost_data->count; }
    mmap_vhost_elements *GetVHostElements() { return m_vhost_data->vhosts; }

    apr_size_t GetCertificateCount() { return m_certificate_data->count; }
    mmap_certificate_elements *GetCertificateElements() { return m_certificate_data->certificates; }

    apr_status_t LockMutex() { return apr_global_mutex_lock(m_mutexMapRW); }
    apr_status_t UnlockMutex() { return apr_global_mutex_unlock(m_mutexMapRW); }

    apr_pool_t *GetPool() { return m_apr_pool; }

protected:
    void InhibitStatusOutput() { m_fStatusOutput = false; }
    apr_status_t Initialize();

private:
    static bool sm_fAprInitialized;
    bool m_fStatusOutput;

    apr_pool_t *m_apr_pool;
    apr_shm_t *m_mmap_region;
    mmap_server_data *m_server_data;
    mmap_vhost_data *m_vhost_data;
    mmap_certificate_data *m_certificate_data;
    mmap_string_table *m_string_data;

    apr_global_mutex_t *m_mutexMapRW;

    DataSampler m_sampler;
    int m_loadCount;

    friend class DataSampler;
};

// Only construct class once - no need to load shared memory multiple times
extern ApacheBinding g_apache;


//
// The providers should have an exception handler wrapping all activity.  This
// helps guarantee that the agent won't crash if there's an unhandled exception.
// In the Pegasus model, this was done in the base class.  Since we don't have
// that luxury here, we'll have macros to make it easier.
//
// PEX = Provider Exception
//
// There's an assumption here that, since this is used in the OMI-generated code,
// "context" always exists for posting the result to.
//

#define PEX_ERROR_CODE APR_EGENERAL

#define CIM_PEX_BEGIN \
    try

#define CIM_PEX_END(module) \
    catch (std::exception &e) { \
        TemporaryPool ptemp( g_apache.GetPool() ); \
        char *etext = apr_psprintf(ptemp.Get(), "%s - Exception occurred! Exception %s", module, e.what()); \
        g_apache.DisplayError( PEX_ERROR_CODE, etext ); \
        context.Post(MI_RESULT_FAILED); \
    } \
    catch (...) \
    { \
        TemporaryPool ptemp( g_apache.GetPool() ); \
        char *etext = apr_psprintf(ptemp.Get(), "%s - Unknown exception occurred!", module); \
        g_apache.DisplayError( PEX_ERROR_CODE, etext ); \
        context.Post(MI_RESULT_FAILED); \
    }

//
// Have a little function to make it easy to break into a provider (as a debugging aid)
//
// The idea here is that we sleep indefinitely; if you break in with a debugger, then
// you can set f_break to true and then step through the code.
//

#define CIM_PROVIDER_WAIT_FOR_ATTACH         \
    {                                        \
        volatile bool f_break = false;       \
        while ( !f_break )                   \
            sleep(1);                        \
    }

#endif /* APACHEBINDING_H */

/*----------------------------E-N-D---O-F---F-I-L-E---------------------------*/
