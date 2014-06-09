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


class TestableApacheDependencies : public ApacheDependencies
{
    virtual bool AllowStatusOutput() { return false; }
    virtual apr_status_t LoadMemoryMap(apr_pool_t* pool,
                                       mmap_server_data** svr,
                                       mmap_vhost_data** vhost,
                                       mmap_certificate_data** cert,
                                       mmap_string_table** str) { return APR_SUCCESS; }
    virtual apr_status_t UnloadMemoryMap() { return APR_SUCCESS; }

    virtual apr_status_t InitializeMutex(apr_pool_t* pool) { return APR_SUCCESS; }
    virtual apr_status_t DestroyMutex() { return APR_SUCCESS; }
    virtual apr_status_t Lock() { return APR_SUCCESS; }
    virtual apr_status_t Unlock() { return APR_SUCCESS; }

    virtual apr_status_t LaunchDataCollector() { return APR_SUCCESS; }
    virtual apr_status_t ShutdownDataCollector() { return APR_SUCCESS; }
};

class TestableApacheBinding : public ApacheBinding
{
public:
    explicit TestableApacheBinding(TestableApacheDependencies* deps = new TestableApacheDependencies())
        : ApacheBinding(deps)
    {}
    ApacheDependencies* GetDependencies() { return static_cast<ApacheDependencies*>(m_pDeps); }
    void SetMemoryMap(mmap_server_data* svr, mmap_string_table* str)
    {
        m_server_data = svr;
        m_string_data = str;
    }
};

#endif /* TESTABLEAPACHE_H */

/*----------------------------E-N-D---O-F---F-I-L-E---------------------------*/
