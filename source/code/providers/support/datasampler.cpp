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

#include <apr_atomic.h>
#include <apr_strings.h>

#include "apachebinding.h"
#include "datasampler.h"

#include <algorithm>
#include <limits>


struct LinuxProcStat {
    int processId;                           //!< %d  1
    char command[30];                        //!< %s
    char state;                              //!< %c
    int parentProcessId;                     //!< %d
    int processGroupId;                      //!< %d  5
    int sessionId;                           //!< %d
    int controllingTty;                      //!< %d
    int terminalProcessId;                   //!< %d
    unsigned long flags;                     //!< %lu
    unsigned long minorFaults;               //!< %lu 10
    unsigned long childMinorFaults;          //!< %lu
    unsigned long majorFaults;               //!< %lu
    unsigned long childMajorFaults;          //!< %lu
    unsigned long userTime;                  //!< %lu
    unsigned long systemTime;                //!< %lu 15
    long childUserTime;                      //!< %ld
    long childSystemTime;                    //!< %ld
    long priority;                           //!< %ld
    long nice;                               //!< %ld
    // dummy at this position, not read;     //!< %ld 20
    long intervalTimerValue;                 //!< %ld
    unsigned long startTime;                 //!< %lu
    unsigned long virtualMemSizeBytes;       //!< %lu
    long residentSetSize;                    //!< %ld
    unsigned long residentSetSizeLimit;      //!< %lu 25
    unsigned long startAddress;              //!< %lu
    unsigned long endAddress;                //!< %lu
    unsigned long startStackAddress;         //!< %lu
    unsigned long kernelStackPointer;        //!< %lu
    unsigned long kernelInstructionPointer;  //!< %lu 30
    unsigned long signal;                    //!< %lu
    unsigned long blocked;                   //!< %lu
    unsigned long sigignore;                 //!< %lu
    unsigned long sigcatch;                  //!< %lu
    unsigned long waitChannel;               //!< %lu 35
    unsigned long numPagesSwapped;           //!< %lu
    unsigned long cumNumPagesSwapped;        //!< %lu
    int exitSignal;                          //!< %d
    int processorNum;                        //!< %d
    unsigned long realTimePriority;          //!< %lu 40 (Since 2.5.19)
    unsigned long schedulingPolicy;          //!< %lu    (Since 2.5.19)

    static const int procstat_len = 40; //!< Number of fields not counting dummy

    /** The format string that should be supplied to fscanf() to read procstat_fields */
    static const char *scanstring;  /* = 
        "%d %s %c %d %d %d %d %d %lu %lu " // 1 to 10
        "%lu %lu %lu %lu %lu %ld %ld %ld %ld %*ld " // 11 to 20
        "%ld %lu %lu %ld %lu %lu %lu %lu %lu %lu " // 21 to 30
        "%lu %lu %lu %lu %lu %lu %lu %d %d %lu %lu"; // 31 to 41
                                    */
};

const char* LinuxProcStat::scanstring = 
        /* "%d %s " */ "%c %d %d %d %d %d %lu %lu " // 1 to 10 (pid & command read independently)
        "%lu %lu %lu %lu %lu %ld %ld %ld %ld %*ld " // 11 to 20
        "%ld %lu %lu %ld %lu %lu %lu %lu %lu %lu " // 21 to 30
        "%lu %lu %lu %lu %lu %lu %lu %d %d %lu %lu"; // 31 to 41


DataSampler::DataSampler()
    : m_tid(NULL), m_skipValidationCount(0), m_mutex(NULL), m_cond(NULL), m_fShutdown(false)
{
    m_timeLastUpdated = apr_time_now();
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
    apr_pool_t *pool = g_pFactory->GetInit()->GetPool();
    apr_threadattr_t *attr;
    apr_status_t status;

    // Set up condition variables for thread signalling

    if ((NULL == m_mutex) && APR_SUCCESS != (status = apr_thread_mutex_create(&m_mutex, APR_THREAD_MUTEX_UNNESTED, pool)))
    {
        DisplayError(status, "DataSampler::Initialize failed to initialize mutex for condition");
        return status;
    }

    if ((NULL == m_cond) && APR_SUCCESS != (status = apr_thread_cond_create(&m_cond, pool)))
    {
        DisplayError(status, "DataSampler::Initialize failed to create/initialize condition");
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
        DisplayError(status, "DataSampler::Launch failed to create sampler thread");
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
            DisplayError(status, "DataSampler::WaitForCompletion failed to signal condition");
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
            DisplayError(status, "DataSampler::WaitForCompletion failed waiting for worker thread");
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
        DisplayError(status, "DataSampler::Lock failed to lock condition mutex");
    }

    return status;
}

apr_status_t DataSampler::Unlock()
{
    apr_status_t status;

    if (APR_SUCCESS != (status = apr_thread_mutex_unlock(m_mutex)))
    {
        DisplayError(status, "DataSampler::Lock failed to unlock condition mutex");
    }

    return status;
}

void DataSampler::ThreadMain()
{
    apr_status_t status;
    const apr_time_t sleepInterval = apr_time_from_sec(60);
    apr_time_t wakeupTime = apr_time_now() + sleepInterval;

    DisplayError(0, "DataSampler::ThreadMain is alive");

    /*
     * Main loop: Wait for condition (shutdown); if none, then wake up one/minute for calculations
     */

    if (APR_SUCCESS != (status = Lock()))
    {
        DisplayError(status, "DataSampler::ThreadMain is aborting due to failure of Lock() operation");
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
            DisplayError(0, "DataSampler::ThreadMain Waiting for condition");
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
            DisplayError(status, "DataSampler::Threadmain received unexpected error from apr_thread_cond_timedwait");
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
            DisplayError(status, "DataSampler::Threadmain unable to unlock mutex");
            break;
        }

        PerformComputations();

        if (APR_SUCCESS != (status = Lock()))
        {
            DisplayError(status, "DataSampler::Threadmain unable to lock mutex");
            break;
        }

        // Compute next wakeup period

        wakeupTime += sleepInterval;
    }

    DisplayError(0, "DataSampler::ThreadMain is shutting down");
    Unlock();
    return;
}

/*----------------------------------------------------------------------------*/
/**
    Helper routine to determine delta updates and accumulate totals.

    The purpose of this function is to accumulate totals and determine the
    delta (change) from the last total to the current total kept by the
    Apache server. This is necessary because the APR only allows for 32-bit
    atomic operations, allowing the possibility of numeric overflow.

    \param      myCurrentTotal          Grand total (64-bit) for the counter (or NULL)
    \param      myPriorTotal            Previous total (last time we ran),
                                        updated to latest total upon exit
    \param      myLatestTotal           Latest total from Apache module (atomic)

    \returns    Difference between myLatestTotal and myPriorTotal, accounting
                for numeric overflows.
*/

static apr_uint32_t DeltaHelper(apr_uint64_t *myCurrentTotal, apr_uint32_t *myPriorTotal, volatile apr_uint32_t *myLatestTotal)
{
    apr_uint32_t currentDelta;
    apr_uint32_t priorTotal = *myPriorTotal;
    apr_uint32_t latestTotal = apr_atomic_read32(myLatestTotal);

    // Did we roll over (APR only keeps 32-bit atomics)
    if (priorTotal > latestTotal)
    {
        currentDelta = (std::numeric_limits<apr_uint32_t>::max() - priorTotal) + latestTotal + 1;
    }
    else
    {
        currentDelta = latestTotal - priorTotal;
    }

    // Now update values and return the delta

    *myPriorTotal = *myLatestTotal;
    if (myCurrentTotal != NULL)
    {
        *myCurrentTotal += currentDelta;
    }

    return currentDelta;
}

bool DataSampler::GetApacheTickCount(ApacheDataCollector& data)
{
    LinuxProcStat ps;
    char procStatName[64];

    apr_file_t* fp;
    apr_pool_t *pool = data.GetPool();
    apr_status_t status;

    apr_snprintf(procStatName, sizeof(procStatName), "/proc/%d/stat", data.m_server_data->serverPid);
    if ( APR_SUCCESS != (status = apr_file_open(&fp, procStatName, APR_FOPEN_READ, 0, pool)) )
    {
        char *text = apr_psprintf(pool,
                                  "DataSampler::GetApacheTickCount skipping execution due to file open error on %s",
                                  procStatName);
        DisplayError(status, text);

        data.m_server_data->currentCpuUtilization = 0;
        return false;
    }

    // We used to parse by reading bytes, but this can be problematic if
    // process names contain "(" or ")" (which was found on SuSE 12).
    //
    // Read the entire thing into a buffer and use sscanf to parse it.
    // This allows us to search for "(" (start of process name), and
    // reverse search for ")" (end of process name), and not care what
    // is in the middle.

    char buffer[1024];
    apr_size_t bufferSize = sizeof(buffer) - 1;

    if ( APR_SUCCESS != (status = apr_file_read(fp, buffer, &bufferSize)) )
    {
        DisplayError(status, "DataSampler::GetApacheTickCount encountered error reading status file");
        data.m_server_data->currentCpuUtilization = 0;
        return false;
    }

    // Less than 32 bytes read; that's not possible unless something is really wrong
    if (bufferSize < 32) {
        char *text = apr_psprintf(pool,
                                  "DataSampler::GetApacheTickCount Getting very short contents reading %s file. "
                                  "Expecing minimum of 32 bytes but got %pS bytes.",
                                  procStatName, &bufferSize);
        DisplayError(0, text);

        data.m_server_data->currentCpuUtilization = 0;
        return false;
    }

    // We have some data, so parse it
    buffer[bufferSize] = '\0';
    apr_file_close(fp);

    int nscanned = sscanf(buffer, "%d", &ps.processId);
    if (nscanned != 1) {
        char *text = apr_psprintf(pool,
                                  "DataSampler::GetApacheTickCount Getting wrong number of parameters from %s file. "
                                  "Expecing 1 but getting %d.",
                                  procStatName, nscanned);
        DisplayError(0, text);

        data.m_server_data->currentCpuUtilization = 0;
        return false;
    }

    // Now go for the process name "(processname)", but search for starting
    // "(" and last ")" to handle processes that contain "(" or ")" bytes.
    const char* procStart = strchr(buffer, '(');
    const char* procEnd = strrchr(buffer, ')');
    if (NULL == procStart || NULL == procEnd || procStart > procEnd || (procEnd - procStart) > 28)
    {
        char *text = apr_psprintf(pool,
                                  "DataSampler::GetApacheTickCount Unexpended error parsing  %s file, file contents: %s",
                                  procStatName, buffer);
        DisplayError(0, text);

        data.m_server_data->currentCpuUtilization = 0;
        return false;
    }

    size_t endByte = procEnd - procStart - 1;
    const char* remainingPtr = procEnd + 2;
    memcpy(ps.command, procStart + 1, endByte);
    ps.command[endByte] = '\0';

    nscanned = sscanf(remainingPtr, ps.scanstring,
                      &ps.state, &ps.parentProcessId, &ps.processGroupId,
                      &ps.sessionId, &ps.controllingTty, &ps.terminalProcessId, &ps.flags, &ps.minorFaults,
                      &ps.childMinorFaults, &ps.majorFaults, &ps.childMajorFaults, &ps.userTime, &ps.systemTime,
                      &ps.childUserTime, &ps.childSystemTime, &ps.priority, &ps.nice, &ps.intervalTimerValue,
                      &ps.startTime, &ps.virtualMemSizeBytes, &ps.residentSetSize, &ps.residentSetSizeLimit,
                      &ps.startAddress, &ps.endAddress, &ps.startStackAddress, &ps.kernelStackPointer, 
                      &ps.kernelInstructionPointer, &ps.signal, &ps.blocked, &ps.sigignore, &ps.sigcatch, 
                      &ps.waitChannel, &ps.numPagesSwapped, &ps.cumNumPagesSwapped, &ps.exitSignal, 
                      &ps.processorNum, &ps.realTimePriority, &ps.schedulingPolicy);

    // -2 since we read pid and name separatly
    if (nscanned != ps.procstat_len-2) {
        char *text = apr_psprintf(pool,
                                  "DataSampler::GetApacheTickCount Getting wrong number of parameters from %s file. "
                                  "Expecting %d, but getting %d.",
                                  procStatName, ps.procstat_len - 2, nscanned);
        DisplayError(0, text);

        data.m_server_data->currentCpuUtilization = 0;
        return false;
    }

    data.m_server_data->currentCpuUtilization = ps.userTime + ps.systemTime + ps.childUserTime + ps.childSystemTime;
    return true;
}

void DataSampler::PerformComputations()
{
    apr_time_t currentTime = apr_time_now();
    apr_interval_time_t deltaTime = currentTime - m_timeLastUpdated;

    // Scheduling oddity - just update time and return
    if (apr_time_sec(deltaTime) < 1)
    {
        DisplayError(0, "DataSampler::PerformComputations skipping execution due to thread scheduling issue");
        m_timeLastUpdated = currentTime;
        return;
    }

    DisplayError(0, "DataSampler::PerformComputations executing");

    ApacheDataCollector data = g_pFactory->DataCollectorFactory();
    if (APR_SUCCESS != data.Attach("DataSampler::PerformComputations"))
    {
        // Apache must not be running; pick it up next time 'round
        return;
    }

    // Compute the CPU time utilized for Apache server
    //
    // Apache made the computation like this:
    //   ap_rprintf(r, "CPULoad: %g\n", (tu + ts + tcu + tcs) / tick / up_time * 100.);
    //
    // mod_cimprov.c stores (tu + ts + tcu + tcs) in apacheCpuUtilization.  We take that,
    // massage it, and store the percentCPU in the memory map for provider to access.

    int ticks;

    if ( GetApacheTickCount(data) )
    {
#ifdef _SC_CLK_TCK
        ticks = sysconf(_SC_CLK_TCK);
#else
        ticks = HZ;
#endif

        apr_uint32_t deltaTicks = DeltaHelper(NULL, &data.m_server_data->priorCpuUtilization, &data.m_server_data->currentCpuUtilization);
        int logicalProcs = sysconf(_SC_NPROCESSORS_ONLN);
        int percentBusy = ((deltaTicks + (ticks/2)) * 100) / (logicalProcs * ticks * 60);
        apr_atomic_set32(&data.m_server_data->percentCPU, std::min(percentBusy, 100));
    }
    else
    {
        // Whoops, some sort of error ...
        apr_atomic_set32(&data.m_server_data->percentCPU, 0);
    }

    // Copy from "volatile" Apache idle/busy counters to values that are updated once/minute

    apr_atomic_set32(&data.m_server_data->idleWorkers, apr_atomic_read32(&data.m_server_data->idleApacheWorkers));
    apr_atomic_set32(&data.m_server_data->busyWorkers, apr_atomic_read32(&data.m_server_data->busyApacheWorkers));

    //
    // Walk the virtual hosts and update the virtual host statistics
    //

    // TODO: Lock the process mutex (thread mutex?  Don't think we need process mutex for virtual hosts)

    mmap_vhost_elements *vhosts = data.GetVHostElements();
    for (apr_size_t i = 0; i < data.GetVHostCount(); i++)
    {
        apr_uint32_t deltaRequests, deltaBytes, delta400, delta500;

        // Determine deltas for each of RequestsTotal, RequestsBytesTotal, errorCount400Total, and ErrorCount500Total

        deltaRequests = DeltaHelper(&vhosts[i].requestTotal64, &vhosts[i].requestsTotalPrior, &vhosts[i].requestsTotal);
        deltaBytes = DeltaHelper(&vhosts[i].requestsBytesTotal64, &vhosts[i].requestsTotalBytesPrior, &vhosts[i].requestsBytes);
        delta400 = DeltaHelper(&vhosts[i].errorCount400Total64, &vhosts[i].errorCount400TotalPrior, &vhosts[i].errorCount400);
        delta500 = DeltaHelper(&vhosts[i].errorCount500Total64, &vhosts[i].errorCount500TotalPrior, &vhosts[i].errorCount500);

        // RequestsPerSecond: Delta # of requests / # of seconds since last run
        apr_atomic_set32(&vhosts[i].requestsPerSecond, deltaRequests / apr_time_sec(deltaTime));
        // kbPerRequest: Total KB (delta) / # of requests (delta); KB = Total bytes (delta) / 1024
        apr_atomic_set32(&vhosts[i].kbPerRequest, ( deltaRequests ? (deltaBytes / 1024) / deltaRequests : 0));
        // kbPerSecond: Total KB (delta) / # of seconds since last run
        apr_atomic_set32(&vhosts[i].kbPerSecond, (deltaBytes / 1024) / apr_time_sec(deltaTime));

        // errorsPerMinute* = (errorDelta / (# of seconds since last run)) * 60. (the idea is to normalize to a per-minute rate)
        apr_atomic_set32(&vhosts[i].errorsPerMinute400, (delta400 / apr_time_sec(deltaTime)) * 60);
        apr_atomic_set32(&vhosts[i].errorsPerMinute500, (delta500 / apr_time_sec(deltaTime)) * 60);
    }

    // TODO: Unlock the process mutex

    // Check if the shared memory region could possibly be "stale".
    //
    // This can occur if Apache croaks (without doing normal cleanup), thus
    // leaving our shared memory segment around. To guard against this, we
    // check if the region is valid from time to time and, if not, we mark
    // it invalid. That keeps anyone from using it until Apache restarts.
    //
    // Note that the region is always checked if we failed to attach. This
    // allows immediate recovery of our provider when Apache is restarted.

    if ( ++m_skipValidationCount >= 5 )
    {
        g_pFactory->GetInit()->ValidateSharedMemory(data);
        m_skipValidationCount = 0;
    }

    m_timeLastUpdated = currentTime;
    return;
}
