/*--------------------------------------------------------------------------------
    Copyright (c) Microsoft Corporation. All rights reserved. See license.txt for license information.
*/
/**
    \file        testableapache.h

    \brief       Test classes of ApacheBinding and TestableApacheDependencies for unit test purposes

    \date        2014-06-06 16:04:00 AM
*/
/*----------------------------------------------------------------------------*/

#ifndef TESTABLEAPACHE_H
#define TESTABLEAPACHE_H

#include "apachebinding.h"


class TestableApacheInitDependencies : public ApacheInitDependencies
{
public:
    virtual ~TestableApacheInitDependencies() {}

    virtual bool AllowStatusOutput() { return false; }

    virtual apr_status_t LaunchDataCollector() { return APR_SUCCESS; }
    virtual apr_status_t ShutdownDataCollector() { return APR_SUCCESS; }

    virtual const char* GetServerConfigFile(apr_pool_t* pool) { return NULL; }
    virtual apr_status_t ValidateSharedMemory(ApacheDataCollector& data) { return APR_SUCCESS; }
    virtual bool IsSharedMemoryValid() { return true; }
    virtual void GetApacheProcessName(std::string& processName) { processName = "httpd-fake"; }
};

class TestableApacheDataCollectorDependencies : public ApacheDataCollectorDependencies
{
public:
    TestableApacheDataCollectorDependencies()
    : m_server_data(NULL), m_vhost_data(NULL), m_certificate_data(NULL), m_string_data(NULL)
    { }

    virtual ~TestableApacheDataCollectorDependencies() {}

    virtual apr_status_t Lock() { return APR_SUCCESS; }
    virtual apr_status_t Unlock() { return APR_SUCCESS; }

    virtual apr_status_t LaunchDataCollector() { return APR_SUCCESS; }
    virtual apr_status_t ShutdownDataCollector() { return APR_SUCCESS; }

    virtual apr_status_t InitializeMutex() { return APR_SUCCESS; }
    virtual apr_status_t DestroyMutex() { return APR_SUCCESS; }

    virtual apr_status_t LoadMemoryMap(mmap_server_data** svr,
                                       mmap_vhost_data** vhost,
                                       mmap_certificate_data** cert,
                                       mmap_string_table** str)
    {
        *svr = m_server_data;
        *vhost = m_vhost_data;
        *cert = m_certificate_data;
        *str = m_string_data;

        return APR_SUCCESS;
    }
    virtual apr_status_t UnloadMemoryMap() { return APR_SUCCESS; }

    void SetMemoryMap(mmap_server_data* svr, mmap_string_table* str)
    {
        m_server_data = svr;
        m_string_data = str;
    }

private:
    mmap_server_data *m_server_data;
    mmap_vhost_data *m_vhost_data;
    mmap_certificate_data *m_certificate_data;
    mmap_string_table *m_string_data;
};

class TestableApacheFactory : public ApacheFactory
{
public:
    TestableApacheFactory()
    : m_server_data(NULL), m_vhost_data(NULL), m_certificate_data(NULL), m_string_data(NULL)
    { }

    virtual ApacheDataCollector DataCollectorFactory()
    {
        TestableApacheDataCollectorDependencies* pDeps = new TestableApacheDataCollectorDependencies();
        pDeps->SetMemoryMap(m_server_data, m_string_data);
        return ApacheDataCollector( pDeps );
    }

    virtual ApacheInitialization* InitializationFactory()
    { return new ApacheInitialization( new TestableApacheInitDependencies() ); }

    void SetMemoryMap(mmap_server_data* svr, mmap_string_table* str)
    {
        m_server_data = svr;
        m_string_data = str;
    }

private:
    mmap_server_data *m_server_data;
    mmap_vhost_data *m_vhost_data;
    mmap_certificate_data *m_certificate_data;
    mmap_string_table *m_string_data;
};

#endif /* TESTABLEAPACHE_H */

/*----------------------------E-N-D---O-F---F-I-L-E---------------------------*/
