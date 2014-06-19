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

#include <string>


// Forward definitions
class ApacheDataCollector;

/*------------------------------------------------------------------------------*/
/**
 *   ApacheBinding 
 *   Helper class to help with Apache module <-> Provider bindings
 */

class ApacheInitDependencies
{
public:
    ApacheInitDependencies()
    : m_configFileAttempted(false), m_bIsRegionValid(false)
    {}
    virtual ~ApacheInitDependencies();

    virtual bool AllowStatusOutput() { return true; }

    virtual apr_status_t LaunchDataCollector() { return m_sampler.Launch(); }
    virtual apr_status_t ShutdownDataCollector() { return m_sampler.WaitForCompletion(); }

    virtual const char* GetServerConfigFile(apr_pool_t* pool);
    virtual apr_status_t ValidateSharedMemory(ApacheDataCollector& data);
    virtual bool IsSharedMemoryValid() { return m_bIsRegionValid; }
    virtual void GetApacheProcessName(std::string& processName);

private:
    DataSampler m_sampler;
    bool m_configFileAttempted;
    std::string m_configFile;

    // Support for validating shared memory segment
    bool m_bIsRegionValid;
    std::string m_processName;
};

class ApacheDataCollectorDependencies
{
public:
    ApacheDataCollectorDependencies()
    : m_mutexMapRW(NULL),
      m_mmap_region(NULL),
      m_apr_attach_pool(NULL)
    {}
    virtual ~ApacheDataCollectorDependencies() { Detach("Destructor"); }
    virtual apr_status_t Attach(const char *text, apr_pool_t* pool,
            mmap_server_data** svr, mmap_vhost_data** vhost,
            mmap_certificate_data** cert, mmap_string_table** str);
    virtual apr_status_t Detach(const char *text);

    virtual apr_status_t Lock() { return apr_global_mutex_lock(m_mutexMapRW); }
    virtual apr_status_t Unlock() { return apr_global_mutex_unlock(m_mutexMapRW); }

private:
    virtual apr_status_t InitializeMutex();
    virtual apr_status_t DestroyMutex();

    virtual apr_status_t LoadMemoryMap( mmap_server_data** svr,
                                       mmap_vhost_data** vhost,
                                       mmap_certificate_data** cert,
                                       mmap_string_table** str);
    virtual apr_status_t UnloadMemoryMap();

    apr_global_mutex_t *m_mutexMapRW;
    apr_shm_t *m_mmap_region;

    apr_pool_t *m_apr_attach_pool; // Owned by ApacheDataCollector; do not clean up!
};



//
// Basic Apache Initialization - only done once by the provider(s)
//

class ApacheInitialization
{
public:
    explicit ApacheInitialization(ApacheInitDependencies* deps)
    : m_pDeps(deps), m_apr_pool(NULL), m_loadCount(0)
    {}
    ~ApacheInitialization() { delete m_pDeps; }

    static apr_status_t OMI_Error(int err) { return APR_OS_START_USERERR + err; }
    void DisplayError(apr_status_t status, const char *text);
    apr_status_t Load(const char *text);
    apr_status_t Unload(const char *text);

    // Inline helpers to call dependency methods
    const char* GetServerConfigFile(apr_pool_t* pool)
        { return m_pDeps->GetServerConfigFile(pool); }
    apr_status_t ValidateSharedMemory(ApacheDataCollector& data)
        { return m_pDeps->ValidateSharedMemory(data); }
    bool IsSharedMemoryValid()
        { return m_pDeps->IsSharedMemoryValid(); }
    void GetApacheProcessName(std::string& processName)
        { m_pDeps->GetApacheProcessName(processName); }

    apr_pool_t *GetPool() { return m_apr_pool; }

protected:
    apr_status_t Initialize(const char *text);

    ApacheInitDependencies* m_pDeps;

private:
    static bool sm_fAprInitialized;

    apr_pool_t *m_apr_pool;
    int m_loadCount;

    friend class DataSampler;
};


//
// Apache Data Collector - Attaches to shared memory segment for Apache data
//
// The intent is that this is generated on the stack, and when it falls out of
// scope, the destructor will clean everything up. You may also call Detach()
// explicitly to detach from the memory pool.
//
// Note: The Attach() method MUST be called, and MUST be checked for a valid
// return code. If Attach() is not called, or if it fails and data accessors
// are called anyway, then segmentation faults are likely to follow.  Consider
// yourself warned!
//

class ApacheDataCollector
{
public:
    explicit ApacheDataCollector(ApacheDataCollectorDependencies* deps);
    ~ApacheDataCollector();
    apr_status_t Attach(const char *text);
    apr_status_t Detach(const char *text);

    const char* GetDataString(apr_size_t offset);

    const char *GetServerConfigFile();
    const char *GetServerVersion() { return GetDataString(m_server_data->serverVersionOffset); }
    const char *GetServerRoot() { return GetDataString(m_server_data->serverRootOffset); }
    const char *GetServerID() { return GetDataString(m_server_data->serverIDOffset); }
    pid_t GetServerPID() { return m_server_data->serverPid; }
    apr_size_t GetModuleCount() { return m_server_data->moduleCount; }
    mmap_server_modules *GetServerModules() { return m_server_data->modules; }
    apr_uint32_t GetWorkerCountIdle() { return apr_atomic_read32(&m_server_data->idleWorkers); }
    apr_uint32_t GetWorkerCountBusy() { return apr_atomic_read32(&m_server_data->busyWorkers); }
    apr_uint32_t GetCPUUtilization() { return apr_atomic_read32(&m_server_data->percentCPU); }

    apr_size_t GetVHostCount() { return m_vhost_data->count; }
    mmap_vhost_elements *GetVHostElements() { return m_vhost_data->vhosts; }

    apr_size_t GetCertificateCount() { return m_certificate_data->count; }
    mmap_certificate_elements *GetCertificateElements() { return m_certificate_data->certificates; }

    apr_status_t LockMutex() { return m_pDeps->Lock(); }
    apr_status_t UnlockMutex() { return m_pDeps->Unlock(); }

    apr_pool_t *GetPool() { return m_apr_pool; }

protected:
    mmap_server_data *m_server_data;
    mmap_vhost_data *m_vhost_data;
    mmap_certificate_data *m_certificate_data;
    mmap_string_table *m_string_data;

    ApacheDataCollectorDependencies* m_pDeps;

private:
    apr_pool_t *m_apr_pool;

    friend class DataSampler;
};


//
// Apache Class Factory
//
// Allows contruction of ApacheInitialization and ApacheDataCollector classes.
// This allows us to replace the factory for dependency injection purposes.
//

class ApacheFactory
{
public:
    ApacheFactory() : m_pInit(NULL) {}
    virtual ~ApacheFactory() { delete m_pInit; }
    virtual ApacheInitialization* GetInit()
    {
        if (NULL == m_pInit)
        {
            m_pInit = InitializationFactory();
        }

        return m_pInit;
    }
    virtual ApacheDataCollector DataCollectorFactory()
    { return ApacheDataCollector( new ApacheDataCollectorDependencies() ); }

protected:
    virtual ApacheInitialization* InitializationFactory()
    { return new ApacheInitialization( new ApacheInitDependencies()) ; }

private:
    ApacheInitialization* m_pInit;
};

// Define single global copy (intended as a singleton class)
extern ApacheFactory* g_pFactory;



// Ease-of-use functions
//
// Since g_pFactory is pretty much guarenteed to be set up "out of the gate",
// and the initialization class is a singleton, set up some easy functions do
// do very common things with it (error logging for now).

inline apr_status_t OMI_Error(int err)
{
    return ApacheInitialization::OMI_Error(err);
}

inline void DisplayError(apr_status_t status, const char *text)
{
    g_pFactory->GetInit()->DisplayError(status, text);
}



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
        ApacheInitialization* pInit = g_pFactory->GetInit(); \
        TemporaryPool ptemp( pInit->GetPool() ); \
        char *etext = apr_psprintf(ptemp.Get(), "%s - Exception occurred! Exception %s", module, e.what()); \
        pInit->DisplayError( PEX_ERROR_CODE, etext ); \
        context.Post(MI_RESULT_FAILED); \
    } \
    catch (...) \
    { \
        ApacheInitialization* pInit = g_pFactory->GetInit(); \
        TemporaryPool ptemp( pInit->GetPool() ); \
        char *etext = apr_psprintf(ptemp.Get(), "%s - Unknown exception occurred!", module); \
        pInit->DisplayError( PEX_ERROR_CODE, etext );   \
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
