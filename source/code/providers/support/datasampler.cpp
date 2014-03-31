/*--------------------------------------------------------------------------------
 *        Copyright (c) Microsoft Corporation. All rights reserved. See license.txt for license information.
 *     
 *        */
 /**
      \file        datasampler.cpp

      \brief       Provider time-based computations

      \date        03-04-14
*/
/*----------------------------------------------------------------------------*/

#include <apr_strings.h>

#include "apachebinding.h"
#include "datasampler.h"

DataSampler::DataSampler()
    : m_tid(NULL), m_mutex(NULL), m_cond(NULL), m_fShutdown(false)
{
}

DataSampler::~DataSampler()
{
    if (NULL != m_tid)
    {
        WaitForCompletion();
    }
}

// Thread entry point ("C" style); simply dispatch to the "real" method
void* APR_THREAD_FUNC DataSampler::threadmain(apr_thread_t *tid, void *data)
{
    // Dispatch to class threadmain method
    DataSampler *sample = reinterpret_cast<DataSampler *> (data);
    sample->ThreadMain();

    apr_thread_exit(tid, APR_SUCCESS);
    return NULL;
}

apr_status_t DataSampler::Launch()
{
    apr_pool_t *pool = g_apache.GetPool();
    apr_threadattr_t *attr;
    apr_status_t status;

    // Set up condition variables for thread signalling

    if ((NULL == m_mutex) && APR_SUCCESS != (status = apr_thread_mutex_create(&m_mutex, APR_THREAD_MUTEX_UNNESTED, pool)))
    {
        g_apache.DisplayError(status, "DataSampler::Initialize failed to initialize mutex for condition");
        return status;
    }

    if ((NULL == m_cond) && APR_SUCCESS != (status = apr_thread_cond_create(&m_cond, pool)))
    {
        g_apache.DisplayError(status, "DataSampler::Initialize failed to create/initialize condition");
        return status;
    }

    // If we've already launched, don't do so again
    if (NULL != m_tid)
    {
        return APR_BADARG;
    }

    apr_threadattr_create(&attr, pool);

    if (APR_SUCCESS != (status = apr_thread_create(&m_tid, attr, DataSampler::threadmain, this, pool)))
    {
        g_apache.DisplayError(status, "DataSampler::Launch failed to create sampler thread");
        return status;
    }

    return APR_SUCCESS;
}

apr_status_t DataSampler::WaitForCompletion()
{
    // No need to wait if we were never launched to begin with

    if (NULL != m_tid)
    {
        apr_status_t status, tstatus;

        // Signal for thread to shut down (lock, set condition variable, signal condition, and unlock)

        if (APR_SUCCESS != (status = Lock()))
        {
            return status;
        }

        m_fShutdown = true;

        if (APR_SUCCESS != (status = apr_thread_cond_signal(m_cond)))
        {
            g_apache.DisplayError(status, "DataSampler::WaitForCompletion failed to signal condition");
            Unlock();
            return status;
        }

        if (APR_SUCCESS != (status = Unlock()))
        {
            return status;
        }

        // Now wait for the thread to actually stop

        status = apr_thread_join(&tstatus, m_tid);
        if (APR_SUCCESS != status)
        {
            g_apache.DisplayError(status, "DataSampler::WaitForCompletion failed waiting for worker thread");
            return status;
        }

        m_tid = NULL;
    }

    return APR_SUCCESS;
}

apr_status_t DataSampler::Lock()
{
    apr_status_t status;

    if (APR_SUCCESS != (status = apr_thread_mutex_lock(m_mutex)))
    {
        g_apache.DisplayError(status, "DataSampler::Lock failed to lock condition mutex");
    }

    return status;
}

apr_status_t DataSampler::Unlock()
{
    apr_status_t status;

    if (APR_SUCCESS != (status = apr_thread_mutex_unlock(m_mutex)))
    {
        g_apache.DisplayError(status, "DataSampler::Lock failed to unlock condition mutex");
    }

    return status;
}

void DataSampler::ThreadMain()
{
    apr_status_t status;
    const apr_time_t sleepInterval = apr_time_from_sec(60);
    apr_time_t wakeupTime = apr_time_now() + sleepInterval;

    g_apache.DisplayError(0, "DataSampler::ThreadMain is alive");

    /*
     * Main loop: Wait for condition (shutdown); if none, then wake up one/minute for calculations
     */

    if (APR_SUCCESS != (status = Lock()))
    {
        g_apache.DisplayError(status, "DataSampler::ThreadMain is aborting due to failure of Lock() operation");
        return;
    }

    while (! m_fShutdown)
    {
        /*
         * Schedule such that we run every 'sleepInterval' (converted to seconds).
         * If we get scheduled "late", compensate next time around.
         */

        apr_time_t currentTime = apr_time_now();

        // If it's time to wake up, just ignore the condition entirely
        if (currentTime <= wakeupTime)
        {
            apr_interval_time_t timeout = wakeupTime - currentTime;
            g_apache.DisplayError(0, "DataSampler::ThreadMain Waiting for condition");
            status = apr_thread_cond_timedwait(m_cond, m_mutex, timeout);
        }
        else
        {
            status = APR_TIMEUP;
        }

        if (m_fShutdown)
        {
            break;
        }

        // If something went wrong with condition, throw in the towel

        if (APR_SUCCESS != status && APR_TIMEUP != status)
        {
            g_apache.DisplayError(status, "DataSampler::Threadmain received unexpected error from apr_thread_cond_timedwait");
            break;
        }

        // Our condition (m_fShutdown) isn't set, and we didn't time out - must be a spurious wakeup; sleep again
        if (APR_TIMEUP != status)
        {
            continue;
        }

        // Perform computations now (leave condition unlocked while performing computatations so it can be signalled)

        if (APR_SUCCESS != (status = Unlock()))
        {
            g_apache.DisplayError(status, "DataSampler::Threadmain unable to unlock mutex");
            break;
        }

        PerformComputations();

        if (APR_SUCCESS != (status = Lock()))
        {
            g_apache.DisplayError(status, "DataSampler::Threadmain unable to lock mutex");
            break;
        }

        // Compute next wakeup period

        wakeupTime += sleepInterval;
    }

    g_apache.DisplayError(0, "DataSampler::ThreadMain is shutting down");
    Unlock();
    return;
}

void DataSampler::PerformComputations()
{
    g_apache.DisplayError(0, "DataSampler::PerformComputations executing");

    return;
}
