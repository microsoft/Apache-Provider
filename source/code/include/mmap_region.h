#ifndef MMAP_REGION_H
#define MMAP_REGION_H

#include <time.h>
#include <stdint.h>
#include <apr.h>
#include <apr_file_info.h>

/*
 * Memory mapped region layout
 *
 * Memory mapped region contains the following:
 *   mmap_server_data:          Server informaion and number of server modules allocated.  This includes:
 *     mmap_server_modules:       Array (size based on Apache Config) for each module loaded in configuration
 *   mmap_vhost_data:           Size marker to indicate number of virtual tables allocated.  This includes:
 *     mmap_vhost_elements:       Array (size based on Apache Config) for each virtual host in configuraiton
 *   mmap_certificate_data:     Size marker to indicate number of certificate file information blocks allocated.  This includes:
 *     mmap_certificate_elements: Array (size based on Apache Config) for each certificate file
 */

// Length of the host name (Wikipedia claims max length=253 for any DNS name)
#define MAX_HOST_NAME_LEN 256

typedef struct
{
    apr_size_t moduleNameOffset;
} mmap_server_modules;

typedef struct
{
    apr_size_t configFileOffset;        // Apache configuration file name
    apr_size_t serverVersionOffset;     // Version of Apache server
    apr_size_t serverRootOffset;        // Root directory of server install
    apr_size_t serverIDOffset;          // Name of computer running Apache server
    pid_t serverPid;                    // PID of the Apache Server

    apr_uint32_t idleApacheWorkers;     // Number of workers that are currently idle (from Apache)
    apr_uint32_t busyApacheWorkers;     // Number of workers that are currently busy (from Apache)
    apr_uint32_t apacheCpuUtilization;  // CPU utilization (from Apache); total ticks consumed
    volatile time_t busyRefreshTime;    // Time of last update for idle/busy workers

    /* The following are from provider worker thread that are updated once/minute */
    apr_uint32_t idleWorkers;           // Number of workers that are currently idle
    apr_uint32_t busyWorkers;           // Number of workers that are currently busy
    apr_uint32_t priorCpuUtilization;   // Prior copy of apacheCpuUtilization for delta computations
    apr_uint32_t percentCPU;            // Percentage of CPU utilization

    apr_size_t moduleCount;             // Number of elements of mmap_server_modules that follow
    mmap_server_modules modules[0];     // Array of Apache modules loaded into the configuraiton
} mmap_server_data;

typedef struct
{
    apr_size_t hostNameOffset;
    apr_size_t documentRootOffset;
    apr_size_t serverAdminOffset;
    apr_size_t instanceIDOffset;
    apr_size_t logErrorOffset;
    apr_size_t logCustomOffset;
    apr_size_t logAccessOffset;

    apr_size_t addressesAndPortsOffset;
    apr_size_t serverAliasesOffset;

    // Counters are kept as 32-bit values, for APR compatibility. Overflow to be
    // recognized by and compensated by the provider. This works because we are
    // primarily interested in rate of change vs. overall totals anyway.

    volatile apr_uint32_t requestsTotal;
    volatile apr_uint32_t requestsBytes;
    volatile apr_uint32_t errorCount400;
    volatile apr_uint32_t errorCount500;

    // Following information kept by provider, not by Apache module. It's here
    // for convenience only. This data must be per-host, and since the above
    // data is per-VHost structure, here is a good a place as any.

    apr_uint32_t requestsTotalPrior;
    apr_uint32_t requestsTotalBytesPrior;
    apr_uint32_t errorCount400TotalPrior;
    apr_uint32_t errorCount500TotalPrior;

    apr_uint64_t requestTotal64;
    apr_uint64_t requestsBytesTotal64;
    apr_uint64_t errorCount400Total64;
    apr_uint64_t errorCount500Total64;

    volatile apr_uint32_t requestsPerSecond;
    volatile apr_uint32_t kbPerRequest;
    volatile apr_uint32_t kbPerSecond;

    volatile apr_uint32_t errorsPerMinute400;
    volatile apr_uint32_t errorsPerMinute500;
} mmap_vhost_elements;

typedef struct
{
    apr_size_t count;                   // Number of elements of mmap_vhost_elements that follow
    mmap_vhost_elements vhosts[0];      // Array of mmap_vhost_elements (host information)
} mmap_vhost_data;

typedef struct
{
    /* SSL certificate information */
    apr_size_t certificateFileNameOffset;       /* name of file containing certificate */
    apr_size_t hostNameOffset;                  /* name of first host that uses this certificate */
    apr_uint16_t port;                          /* port of first host that uses this certificate */
    apr_size_t virtualHostOffset;               /* name of the virtual host using this certificate */
    char certificateExpirationCimTime[32];      /* expiration time in CIM format; filled in by provider */
    apr_time_t certificateExpirationAprTime;    /* expiration time in APR format; filled in by provider */
    apr_time_t certificateFileMtime;            /* last time file was modified; filled in by provider */
} mmap_certificate_elements;

typedef struct
{
    apr_size_t count;                           /* Number of elements of mmap_certificate_elements that follow */
    mmap_certificate_elements certificates[0];  /* Array of mmap_certificate_elements */
} mmap_certificate_data;

typedef struct
{
    apr_size_t total_length;            /* Total length of the string table */
    char data[0];                       /* Pointer to beginning of table */
} mmap_string_table;

#define PROVIDER_MMAP_NAME      "/var/opt/microsoft/apache-cimprov/run/Provider_Region"
#define MUTEX_INIT_NAME         "/var/opt/microsoft/apache-cimprov/run/mutexInit.lock"
#define MUTEX_RW_NAME           "/var/opt/microsoft/apache-cimprov/run/mutexRW.lock"

#endif // #define MMAP_REGION_H
