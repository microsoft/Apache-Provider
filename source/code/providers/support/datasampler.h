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
      \file        datasampler.h

      \brief       Header file for Provider time-based computations

      \date        03-04-14
*/
/*----------------------------------------------------------------------------*/

#ifndef DATASAMPLER_APACHE_H
#define DATASAMPLER_APACHE_H

// Apache Portable Runtime definitions
#include <apr.h>
#include <apr_thread_cond.h>
#include <apr_thread_mutex.h>
#include <apr_thread_proc.h>

class ApacheDataCollector;

/*------------------------------------------------------------------------------*/
/**
 *   DataSampler
 *   Helper class to manage thread and perform time-based computations
 */

class DataSampler
{
public:
    DataSampler();
    ~DataSampler();

    apr_status_t Launch();
    apr_status_t WaitForCompletion();

private:
    static void* APR_THREAD_FUNC threadmain(apr_thread_t *tid, void *data);
    apr_status_t Lock();
    apr_status_t Unlock();
    void ThreadMain();
    bool GetApacheTickCount(ApacheDataCollector& data);
    void PerformComputations();

    apr_thread_t *m_tid;
    apr_time_t m_timeLastUpdated;

    // Support for shared memory validation
    int m_skipValidationCount;

    // Support for condition (to control thread shutdown)
    apr_thread_mutex_t *m_mutex;
    apr_thread_cond_t *m_cond;
    bool m_fShutdown;
};

#endif /* DATASAMPLER_APACHE_H */

/*----------------------------E-N-D---O-F---F-I-L-E---------------------------*/
