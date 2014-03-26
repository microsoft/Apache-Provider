
#ifndef MMAP_REGION_H
#define MMAP_REGION_H

#include <stdint.h>
#include "apr.h"

/*
 * Memory mapped region layout
 *
 * Memory mapped region contains the following:
 *   mmap_server_data:          Server informaion and nuber of server modules allocated.  This includes:
 *     mmap_server_modules:       Array (size based on Apache Config) for each module loaded in configuration
 *   mmap_vhost_data:           Size marker to indicate number of virtual tables allocated.  This includes:
 *     mmap_vhost_elements:       Array (size based on Apache Config) for each virtual host in configuraiton
 */

// Length of the virtual host name (Wikipedia claims max length=253 for any DNS name)
#define MAX_VIRTUALHOST_NAME_LEN 256

typedef struct
{
    char moduleName[NAME_MAX+1];
} mmap_server_modules;

typedef struct
{
    char configFile[PATH_MAX];          // Apache configuration file name
    char processName[NAME_MAX+1];       // Process name
    int operatingStatus;                // Operating status

    volatile apr_uint32_t idleWorkers;  // Number of workers that are currently idle
    volatile apr_uint32_t busyWorkers;  // Number of workers that are currently busy
    volatile time_t busyRefreshTime;    // Time of last update for idle/busy workers

    apr_size_t moduleCount;             // Number of elements of mmap_server_modules that follow
    mmap_server_modules modules[0];     // Array of Apache modules loaded into the configuraiton
} mmap_server_data;

typedef struct
{
    char name[MAX_VIRTUALHOST_NAME_LEN];
    char documentRoot[PATH_MAX];
    char serverAdmin[MAX_VIRTUALHOST_NAME_LEN+32];
    char logError[PATH_MAX];
    char logCustom[PATH_MAX];
    char logAccess[PATH_MAX];

    // Need: IPAddresses[], ServerAliases[] in some way to avoid maximum lengths.
    // Perhaps variable length ending in \0\0 like "val1\0val2\0val3\0\0" ?
    // It needs to fit in the memory segment.  Mulling on this.
    //
    // Another option is to impose a maximum length.  Discussing w/Kris.

    apr_uint16_t port_http;
    apr_uint16_t port_https;

    // Counters are kept as 32-bit values, for APR compatibility. Overflow to be
    // recognized by and compensated by the provider. This works because we are
    // primarily interested in rate of change vs. overall totals anyway.

    volatile apr_uint32_t requestsTotal;
    volatile apr_uint32_t requestsBytes;
    volatile apr_uint32_t errorCount400;
    volatile apr_uint32_t errorCount500;

    // Following information kept by provider, not by Apache module. It's here
    // for convenience only. This data must be per-VirtualHost, and since the
    // above data is per-VirtualHost structure, here is a good a place as any.

    apr_uint32_t requestsTotalPrior;
    apr_uint32_t requestsBytesPrior;

    apr_uint64_t requestsTotalHistory[5];       // 5 minutes worth of history
    apr_uint64_t requestsBytesHistory[5];
    apr_uint64_t errorCount400History[5];
    apr_uint64_t errorCount500History[5];

    volatile apr_uint32_t requestsCurrent;
    volatile apr_uint32_t requestsPerSecond;

    volatile apr_uint32_t msPerRequest;
    volatile apr_uint32_t kbPerRequest;
    volatile apr_uint32_t kbPerSecond;

    volatile apr_uint32_t errorsPerMinute400;
    volatile apr_uint32_t errorsPerMinute500;
} mmap_vhost_elements;

typedef struct
{
    apr_size_t count;                   // Number of elements of mmap_vhost_elements that follow
    mmap_vhost_elements vhosts[0];      // Array of mmap_vhost_elements (virtual host information)
} mmap_vhost_data;

#define PROVIDER_MMAP_NAME      "/var/www/mod_cimprov/Provider_Region"
#define MUTEX_INIT_NAME         "/var/www/mod_cimprov/mutexInit.lock"
#define MUTEX_RW_NAME           "/var/www/mod_cimprov/mutexRW.lock"

#endif // #define MMAP_REGION_H
