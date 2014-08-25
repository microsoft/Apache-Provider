/*--------------------------------------------------------------------------------
    Copyright (c) Microsoft Corporation. All rights reserved. See license.txt for license information.
*/
/**
    \file        mod_cimprov.c

    \brief       Apache Module for Apache managmement/monitoring via CIM Provider

    \date        01-21-2013 13:38:00
*/
/*----------------------------------------------------------------------------*/

#if defined(linux)
// The APR doesn't have a way to get the current PID ...
# include <unistd.h>
# include <time.h>
# include <errno.h>
#endif

#include <string.h>

#include <apr_atomic.h>
#include <apr_hash.h>
#include <apr_strings.h>

#define CORE_PRIVATE

#include <httpd.h>
#include <http_config.h>
#include <http_protocol.h>
#include <ap_mpm.h>
#include <scoreboard.h>
#ifdef AP_NEED_SET_MUTEX_PERMS
#include <unixd.h>
#endif // AP_NEED_SET_MUTEX_PERMS
#include "mmap_region.h"

/* The extern for ap_server_root. This is the documented way to access it, instead
 * of including http_main.h
 */
extern const char* ap_server_root;

#ifdef HAVE_TIMES
/* ugh... need to know if we're running with a pthread implementation
 * such as linuxthreads that treats individual threads as distinct
 * processes; that affects how we add up CPU time in a process
 */
static pid_t g_child_pid;
#endif


typedef enum
{
    LOCKTYPE_INIT,
    LOCKTYPE_RW
} lock_type;


module AP_MODULE_DECLARE_DATA cimprov_module;

/*
 * Representation of an ordered array of strings
 */

typedef struct string_array string_array;
struct string_array {
    string_array* next;                 /* Pointer to next string */
    char string[PATH_MAX];              /* This string, either the source or destination of the the path portion of a URL */
};

/*
 * The module-wide list of hosts and some of their per-host configuration information
 */

typedef struct config_hostInfo config_hostInfo;
struct config_hostInfo {
    config_hostInfo* next;              /* Pointer to next host information structure */
    server_rec* srec;                   /* The record of the host to which this applies */
    char transferLogFileName[PATH_MAX]; /* Name of the transfer log file */
    char customLogFileName[PATH_MAX];   /* Name of the custom log file */
    char documentRoot[PATH_MAX];        /* Name of the host's document root directory */
    string_array* aliases;              /* List of aliases for this server */
    string_array* lastAlias;            /* Last item in *aliases, used to avoid walking to the end when adding */
};

typedef enum
{
    DOCUMENT_ROOT,
    TRANSFER_LOG_FILE_NAME,
    CUSTOM_LOG_FILE_NAME
} host_info_type;

/*
 * The module-wide list of server certificate files and the host names and ports that they apply to
 */

typedef struct config_sslCertFile config_sslCertFile;
struct config_sslCertFile {
    config_sslCertFile* next;           /* Pointer to next certificate structure */
    server_rec* srec;                   /* The record of the host to which this applies */
    char certificateFileName[PATH_MAX]; /* Name of the the certificate file */
    char hostName[MAX_HOST_NAME_LEN];   /* Name of host for this certficate (or _Default) */
    apr_uint16_t port;                  /* Port of host that uses this certificate */
};

/*
 * Persistent server configuration data
 */

typedef struct {
    /* The following items are used during configuration, and are deleted when configuration is complete */
    config_hostInfo* hostInfoList;
    config_sslCertFile* certificateFileList;
} config_data;

typedef struct {
    apr_pool_t *pool;                   /* Primary pool given to us by Apache */
    int enablelogging;                  /* Should we log to the Apache error logfile? */
    int enablehystericallogging;        /* Should we log hysterically? */
    int busyrefreshfrequency;           /* How often (at minimum) do we update busy/refresh properties? */

    apr_shm_t *mmap_region;             /* APR's memory mapped region handle */
    mmap_server_data *server_data;      /* Pointer to server data within memory mapped region */
    mmap_vhost_data *vhost_data;        /* Pointer to vhost data within memory mapped region */
    mmap_certificate_data *certificate_data; /* Pointer to certificate file data within memory mapped region */
    mmap_string_table *string_data;     /* Pointer to string table within memory mapped region */
    char *stable;                       /* Convenience pointer to the strings themselves within region */
    apr_hash_t *vhost_hash;             /* APR hash to hosts in memory mapped region */

    apr_global_mutex_t *mutexMapInit;   /* APR handle to Initialization Mutex */
    apr_global_mutex_t *mutexMapRW;     /* APR handle to Read/Write Mutex */

    int process_limit;                  /* Process limit for Apache Process */
    int thread_limit;                   /* Thread limit for Apache Process */

    apr_pool_t *configPool;             /* Temporary pool used during configuration */
    config_data *configData;            /* Temporary configuration data (discarded after configuration) */

} persist_cfg;

static persist_cfg* g_persistConfig = NULL; /* The configuration information, stored in the default server's memory pool */

/*
 * Utility functions
 */

apr_status_t display_error(persist_cfg *cfg, const char *error_text, apr_status_t status, int fatal)
{
    // If logging disabled (and nothing "bad"), just return

    if (!cfg->enablelogging && !fatal)
    {
        return HTTP_INTERNAL_SERVER_ERROR;
    }

    fprintf(stderr, "%s", error_text ? error_text : "cimprov: Unexpected error occurred");
    if (status != APR_SUCCESS)
    {
        char buffer[256];
        char *errString = apr_strerror(status, buffer, sizeof(buffer));

        fprintf(stderr, ", status=%d (%s)", status, (errString != NULL ? errString : "Unknown text"));
    }
    fprintf(stderr, "\n");

    if (fatal)
    {
        fprintf(stderr, "cimprov: fatal error, unable to proceed\n");
    }
    fflush(stderr);

    return HTTP_INTERNAL_SERVER_ERROR;
}

/*
 * Command parsing code.
 *
 * Be sure that cimprov related commands are at global level only.
 */

/* Set the logging state for a single host */
static const char *set_logging_state(cmd_parms *cmd, void *dummy, int arg)
{
    persist_cfg *cfg = (persist_cfg *) ap_get_module_config(cmd->server->module_config, &cimprov_module);
    const char *err = ap_check_cmd_context(cmd, GLOBAL_ONLY);
    if (err != NULL) {
        return err;
    }

    cfg->enablelogging = arg;
    return NULL;
}

/* Set the hyserical logging state for a single host */
static const char *set_hystericallogging_state(cmd_parms *cmd, void *dummy, int arg)
{
    persist_cfg *cfg = (persist_cfg *) ap_get_module_config(cmd->server->module_config, &cimprov_module);
    const char *err = ap_check_cmd_context(cmd, GLOBAL_ONLY);
    if (err != NULL) {
        return err;
    }

    cfg->enablehystericallogging = arg;
    return NULL;
}

static const char *set_busyrefresh_frequency(cmd_parms *cmd, void *dummy, const char *arg)
{
    persist_cfg *cfg = (persist_cfg *) ap_get_module_config(cmd->server->module_config, &cimprov_module);
    const char *err = ap_check_cmd_context(cmd, GLOBAL_ONLY);
    if (err != NULL) {
        return err;
    }

    cfg->busyrefreshfrequency = atoi(arg);
    return NULL;
}

/* Find an entry for host information that matches the address of a given server record */
static config_hostInfo* find_host_info(persist_cfg* cfg, const server_rec* srec)
{
    config_hostInfo* s;

    for (s = cfg->configData->hostInfoList; s != NULL; s = s->next)
    {
        if (s->srec == srec)
        {
            return s;
        }
    }

    return NULL;
}

/* Find an entry for a host and port in the list of server certificate files */
static config_sslCertFile* find_certificate_file(persist_cfg* cfg, const char* certificateFile)
{
    config_sslCertFile* certInfo;

    for (certInfo = cfg->configData->certificateFileList; certInfo != NULL; certInfo = certInfo->next)
    {
        if (apr_strnatcmp(certInfo->certificateFileName, certificateFile) == 0)
        {
            return certInfo;
        }
    }
    return NULL;
}

/* Add data to the host information item for a given server record */
static const char *set_host_info(cmd_parms *cmd, void *dummy, host_info_type type, const char* value1, const char* value2)
{
    config_hostInfo* hostInfo;
    server_rec* s = cmd->server;
    const char *err = ap_check_cmd_context(cmd, NOT_IN_DIR_LOC_FILE);

    if (err != NULL)
    {
        return err;
    }

    hostInfo = find_host_info(g_persistConfig, s);
    if (hostInfo == NULL)
    {
        /* if this host has no entry, create one by creating a pool and an item in the pool */
        hostInfo = (config_hostInfo*)apr_pcalloc(g_persistConfig->configPool, sizeof (config_hostInfo));
        if (hostInfo == NULL)
        {
            display_error(g_persistConfig, "cimprov: unable to allocate memory for file name items", APR_ENOMEM, 1);
            return "cimprov: unable to allocate memory for file name items";
        }

        hostInfo->next = g_persistConfig->configData->hostInfoList;
        hostInfo->srec = s;
        g_persistConfig->configData->hostInfoList = hostInfo;
    }

    /* Set the appropriate data element in the list item */
    if (type == DOCUMENT_ROOT)
    {
        apr_cpystrn(hostInfo->documentRoot, value1, sizeof hostInfo->documentRoot);
    }
    else if (type == TRANSFER_LOG_FILE_NAME)
    {
        apr_cpystrn(hostInfo->transferLogFileName, value1, sizeof hostInfo->transferLogFileName);
    }
    else if (type == CUSTOM_LOG_FILE_NAME)
    {
        apr_cpystrn(hostInfo->customLogFileName, value1, sizeof hostInfo->customLogFileName);
    }
    else
    {
        display_error(g_persistConfig, "cimprov: request for unsupported host configuration option", APR_EINVAL, 1);
        return "cimprov: request for unsupported host configuration option";
    }

    return NULL;
}

/* Add the name of the document root directory the list of host information */
static const char *set_document_root(cmd_parms *cmd, void *dummy, const char *arg)
{
    return set_host_info(cmd, dummy, DOCUMENT_ROOT, arg, NULL);
}

/* Add the name of a transfer log file to the list of host information */
static const char *set_transfer_log_file(cmd_parms *cmd, void *dummy, const char *arg)
{
    return set_host_info(cmd, dummy, TRANSFER_LOG_FILE_NAME, arg, NULL);
}

/* Add the name of a custom log file to the list of host information */
static const char *set_custom_log_file(cmd_parms *cmd, void *dummy, const char *arg1, const char *arg2)
{
    return set_host_info(cmd, dummy, CUSTOM_LOG_FILE_NAME, arg1, NULL);
}

/* Take a list of server alias names for the virtual host */
static const char *set_server_alias(cmd_parms *cmd, void *dummy, const char *arg)
{
    config_hostInfo* hostInfo;
    server_rec* s = cmd->server;

    if (!cmd->server->names) {
        return "ServerAlias only used in <VirtualHost>";
    }

    hostInfo = find_host_info(g_persistConfig, s);
    if (hostInfo == NULL)
    {
        /* if this host has no entry, create one by creating a pool and an item in the pool */
        hostInfo = (config_hostInfo*)apr_pcalloc(g_persistConfig->configPool, sizeof (config_hostInfo));
        if (hostInfo == NULL)
        {
            display_error(g_persistConfig, "cimprov: unable to allocate memory for file name items", APR_ENOMEM, 1);
            return "cimprov: unable to allocate memory for file name items";
        }

        hostInfo->next = g_persistConfig->configData->hostInfoList;
        hostInfo->srec = s;
        g_persistConfig->configData->hostInfoList = hostInfo;
    }

    while (*arg) {
        string_array* alias;
        char *name = ap_getword_conf(g_persistConfig->configPool, &arg);

        /* Allocate the string in temporary memory */
        alias = (string_array*) apr_pcalloc(g_persistConfig->configPool, sizeof (string_array));
        if (alias == NULL)
        {
            display_error(g_persistConfig, "cimprov: unable to allocate memory for server alias item", APR_ENOMEM, 1);
            return "cimprov: unable to allocate memory for server alias item";
        }

        if (hostInfo->aliases == NULL)
        {
            hostInfo->aliases = alias;
        }
        else
        {
            hostInfo->lastAlias->next = alias;
        }        
        hostInfo->lastAlias = alias;

        apr_cpystrn(alias->string, name, PATH_MAX);    /* populate the strings */
    }

    return NULL;
}

/* Add the name of a server certificate file for a given host and port to the certificate file names list */
static const char *set_server_certificate_file(cmd_parms *cmd, void *dummy, const char *arg)
{
    config_sslCertFile* certFileInfo;
    server_rec* s = cmd->server;
    char* hostName = s->server_hostname;
    const char* err = ap_check_cmd_context(cmd, NOT_IN_DIR_LOC_FILE);

    if (err != NULL)
    {
        return err;
    }

    if (hostName == NULL)
    {
        hostName = "_Default";
    }

    certFileInfo = find_certificate_file(g_persistConfig, arg);
    if (certFileInfo == NULL)
    {
        /* if this certificate file has no entry, create one by creating a pool and an item in the pool */
        certFileInfo = (config_sslCertFile*)apr_pcalloc(g_persistConfig->configPool, sizeof (config_sslCertFile));
        if (certFileInfo == NULL)
        {
            display_error(g_persistConfig, "cimprov: unable to allocate memory for SSL certificate file name item", APR_ENOMEM, 1);
            return "cimprov: unable to allocate memory for SSL certificate file name item";
        }

        /* populate the item */
        certFileInfo->srec = s;
        certFileInfo->next = g_persistConfig->configData->certificateFileList;
        apr_cpystrn(certFileInfo->certificateFileName, arg, sizeof certFileInfo->certificateFileName);

        /* add the new entry to the linked list */
        g_persistConfig->configData->certificateFileList = certFileInfo;

        /* if this is the first use of this certificate file, add the host and port to the list within the item */
        apr_cpystrn(certFileInfo->hostName, hostName, sizeof certFileInfo->hostName);
        certFileInfo->port = s->port;
    }

    return NULL;
}

static const command_rec cimprov_module_cmds[] =
{
    AP_INIT_FLAG("CimSetLogging", set_logging_state, NULL, RSRC_CONF,
      "\"On\" to enable logging information, \"Off\" to disable"),
    AP_INIT_FLAG("CimSetHystericalLogging", set_hystericallogging_state, NULL, RSRC_CONF,
      "\"On\" to enable hysterical logging information, \"Off\" to disable. "
      "Note that CimSetLogging must be On for hysterical output to display properly."),
    AP_INIT_TAKE1("CimBusyRefreshFrequency", set_busyrefresh_frequency, NULL, RSRC_CONF,
      "Set the default busy refresh frequency for busy/idle thread counts and CPU load. "
      "Default = 60 seconds, -1 = Disabled, 0 = Update on each request (very high overhead)."),
    AP_INIT_TAKE1("DocumentRoot", set_document_root, NULL, RSRC_CONF,
      "Set the name of the document root directory for the host."),
    AP_INIT_TAKE1("TransferLog", set_transfer_log_file, NULL, RSRC_CONF,
      "Set the name of the transfer log file for the host."),
    AP_INIT_TAKE2("CustomLog", set_custom_log_file, NULL, RSRC_CONF,
      "Set the name of the custom log file for the host host."),
    AP_INIT_RAW_ARGS("ServerAlias", set_server_alias, NULL, RSRC_CONF,
      "A name or names alternately used to access the server"),
    AP_INIT_TAKE1("SSLCertificateFile", set_server_certificate_file, NULL, RSRC_CONF,
      "Set the name of the SSL certificate file for the host."),
    {NULL}
};

/*
 * Mutex initialization and manipulation
 */

static apr_status_t mutex_lock(persist_cfg *cfg, lock_type type)
{
    apr_status_t status;
    apr_global_mutex_t *mutex;

    if (LOCKTYPE_INIT == type)
    {
        if (cfg->enablehystericallogging)
        {
            display_error(cfg, "cimprov: mutex_lock: locking INIT mutex", 0, 0);
        }
        mutex = cfg->mutexMapInit;
    }
    else
    {
        if (cfg->enablehystericallogging)
        {
            display_error(cfg, "cimprov: mutex_lock: locking RW mutex", 0, 0);
        }
        mutex = cfg->mutexMapRW;
    }

    status = apr_global_mutex_lock(mutex);
    if (APR_SUCCESS != status)
    {
        display_error(cfg, "cimprov: mutex_lock failed to lock mutex", status, 1);
        return status;
    }

    return APR_SUCCESS;
}

static apr_status_t mutex_unlock(persist_cfg *cfg, lock_type type)
{
    apr_status_t status;
    apr_global_mutex_t *mutex;

    if (LOCKTYPE_INIT == type)
    {
        if (cfg->enablehystericallogging)
        {
            display_error(cfg, "cimprov: mutex_unlock: unlocking INIT mutex", 0, 0);
        }
        mutex = cfg->mutexMapInit;
    }
    else
    {
        if (cfg->enablehystericallogging)
        {
            display_error(cfg, "cimprov: mutex_unlock: unlocking RW mutex", 0, 0);
        }
        mutex = cfg->mutexMapRW;
    }

    status = apr_global_mutex_unlock(mutex);
    if (APR_SUCCESS != status)
    {
        display_error(cfg, "cimprov: mutex_unlock failed to unlock mutex", status, 1);
        return status;
    }

    return APR_SUCCESS;
}

static apr_status_t module_mutex_cleanup(void *configuration)
{
    persist_cfg *cfg = (persist_cfg *) configuration;
    apr_status_t status;

    display_error(cfg, "cimprov: module_mutex_cleanup invoked ...", 0, 0);

    // Clean up the mutexes
    // (We normally leave INIT mutex locked - so unlock it)
    if (APR_SUCCESS != (status = mutex_unlock(cfg, LOCKTYPE_INIT)))
    {
        display_error(cfg, "cimprov: mutex_mutex_cleanup failed to unlock INIT mutex", status, 0);
        // A failure to unlock shouldn't abort our cleanup efforts ...
    }

    if (APR_SUCCESS != (status = apr_global_mutex_destroy(cfg->mutexMapInit)))
    {
        display_error(cfg, "cimprov: mutex_mutex_cleanup failed to clean up INIT mutex", status, 0);
        // A failure here is abnormal, but shouldn't abort our cleanup efforts ...
    }

    if (APR_SUCCESS != (status = apr_global_mutex_destroy(cfg->mutexMapRW)))
    {
        display_error(cfg, "cimprov: mutex_mutex_cleanup failed to clean up RW mutex", status, 1);
        return status;
    }

    return APR_SUCCESS;
}

static apr_status_t module_mutex_initialize(persist_cfg *cfg, apr_pool_t *pool)
{
    apr_status_t status;

    // Initialize the mutexes
    display_error(cfg, "cimprov: module_mutex_initialize: creating Init mutex", 0, 0);

    if (APR_SUCCESS != (status = apr_global_mutex_create(&cfg->mutexMapInit, MUTEX_INIT_NAME, APR_LOCK_DEFAULT, pool)))
    {
        display_error(cfg, "cimprov: mutex_mutex_initialize failed to initialize INIT mutex", status, 1);
        return status;
    }

    display_error(cfg, "cimprov: mutex_mutex_initialize: creating RW mutex", 0, 0);
    if (APR_SUCCESS != (status = apr_global_mutex_create(&cfg->mutexMapRW, MUTEX_RW_NAME, APR_LOCK_DEFAULT, pool)))
    {
        display_error(cfg, "cimprov: mutex_mutex_initialize failed to initialize RW mutex", status, 1);
        return status;
    }

    // Register a cleanup handler
    apr_pool_cleanup_register(pool, cfg, module_mutex_cleanup, apr_pool_cleanup_null);

    return APR_SUCCESS;
}

static apr_status_t mmap_region_cleanup(void *configuration)
{
    persist_cfg *cfg = (persist_cfg *) configuration;
    apr_status_t status;

    display_error(cfg, "cimprov: mmap_region_cleanup invoked", 0, 0);

    if (APR_SUCCESS != (status = apr_shm_destroy(cfg->mmap_region)))
    {
        display_error(cfg, "cimprov: mmap_region_cleanup failed to destroy shared region", status, 1);
        return status;
    }

    return APR_SUCCESS;
}

/* A simple base-64 encoding of a 2-byte integer into 3 printable characters.
 * This is not the base-64 encoding used in MIME, because MIME uses a table of
 * legal output characters to avoid most punctuation; that is not needed here.
 */

static char s_encode_buf[4];

static char* encode64(apr_uint16_t value)
{
    s_encode_buf[0] = (char)(((value >> 12) & 0x000F) + 0x0030);
    s_encode_buf[1] = (char)(((value >>  6) & 0x003F) + 0x0030);
    s_encode_buf[2] = (char)(( value        & 0x003F) + 0x0030);
    s_encode_buf[3] = '\0';
    return s_encode_buf;
}

/* Terminate a data string array in the shared memory string table */

static void terminate_data_string_array(
    char* const string_table,
    apr_size_t* pos)
{
    if (string_table != NULL)
    {
        *(string_table + *pos) = '\0';
    }

    *pos += 1;

    return;
}

/* Add a string to the shared memory string table */

static void add_data_string(
    char* const string_table,
    apr_size_t* pos,
    const char* string,
    apr_size_t* offset)
{
    if (string != NULL && *string != '\0')
    {
        size_t len = strlen(string) + 1;

        if (len > PATH_MAX)
        {
            len = PATH_MAX;
        }
        if (string_table != NULL)
        {
            apr_cpystrn(string_table + *pos, string, len);
        }

        if (offset != NULL)
        {
            *offset = (apr_size_t)*pos;
        }

        *pos += len;
    }
    else
    {
        if (offset != NULL)
        {
            *offset = 0;
        }
    }
    return;
}

/* Get the data for the server */
static apr_status_t collect_server_data(
    mmap_server_data* server_data,
    char* const string_table,
    apr_size_t* string_table_len,
    module* top_module,
    apr_size_t module_count,
    const char* config_file,
    const char* server_hostname)
{
    apr_size_t index;
    module* modp;

    // Pull the server version formatted like: Apache/2.0.41
#if AP_SERVER_MAJORVERSION_NUMBER == 2 && AP_SERVER_MINORVERSION_NUMBER == 2
    const char* version = ap_get_server_version();
#elif AP_SERVER_MAJORVERSION_NUMBER == 2 && AP_SERVER_MINORVERSION_NUMBER >= 4
    const char* version = ap_get_server_banner();
#endif

    const char* server_root = ap_server_root;

    /* Add the name of the configuration file */
    add_data_string(string_table,
               string_table_len,
               config_file,
               server_data != NULL ? &server_data->configFileOffset : NULL);

    /* Add the server root directory */
    add_data_string(string_table,
               string_table_len,
               server_root,
               server_data != NULL ? &server_data->serverRootOffset : NULL);

    /* Add the Apache server version */
    add_data_string(string_table,
                    string_table_len,
                    version,
                    server_data != NULL ? &server_data->serverVersionOffset : NULL);

    /* Add the host computer's ID */
    add_data_string(string_table,
                    string_table_len,
                    server_hostname,
                    server_data != NULL ? &server_data->serverIDOffset : NULL);

    if (server_data != NULL)
    {
        server_data->moduleCount = module_count;
        server_data->serverPid = getpid();
    }

    /* Add each of the loaded modules */
    index = 0;
    for (modp = top_module; modp != NULL; modp = modp->next)
    {
        if (index >= module_count)
        {
            return APR_EMISMATCH;
        }
        add_data_string(string_table,
                   string_table_len,
                   modp->name,
                   server_data != NULL ? &server_data->modules[index].moduleNameOffset : NULL);
        index++;
    }

    return APR_SUCCESS;
}

/* Merge two strings by using the "from" string's offset if the "to" string's offset is zero */
static void merge_data_strings(apr_size_t* toOffset, apr_size_t fromOffset)
{
    if (*toOffset == 0)
    {
        *toOffset = fromOffset;
    }
    return;
}

/* Get the data for the hosts */
static apr_status_t collect_vhost_data(
    mmap_vhost_data* vhost_data,
    char* const string_table,
    apr_size_t* string_table_len,
    persist_cfg* cfg,
    apr_pool_t* ptemp,
    apr_hash_t* vhost_hash,             /* APR hash to hosts in memory mapped region */
    const server_rec* head,
    apr_size_t vhost_count)
{
    const server_rec* srec;
    apr_size_t vhost_element;
    apr_size_t physical_host_element;
    int first;

    if (vhost_data != NULL)
    {
        vhost_data->count = vhost_count;
    }

    /*
     * VHost [0] reserved for _Total
     * VHost [1] reserved for _Unknown
     */

    add_data_string(string_table,
                    string_table_len,
                    "_Total",
                    vhost_data != NULL ? &vhost_data->vhosts[0].hostNameOffset : NULL);

    add_data_string(string_table,
                    string_table_len,
                    "_Unknown",
                    vhost_data != NULL ? &vhost_data->vhosts[1].hostNameOffset : NULL);

    if (vhost_data != NULL)
    {
        /* Match instanceID with hostName for _Total and _Unknown */
        vhost_data->vhosts[0].instanceIDOffset = vhost_data->vhosts[0].hostNameOffset;
        vhost_data->vhosts[1].instanceIDOffset = vhost_data->vhosts[1].hostNameOffset;
    }

    vhost_element = 2;
    physical_host_element = vhost_count;    /* initialize to an invalid value */
    for (srec = head; srec != NULL; srec = srec->next)
    {
        char* instanceHost;
        server_addr_rec* addrs;
        config_hostInfo* hostInfo;

        /* Create the InstanceID for uniquely tracking this host */
        instanceHost = apr_pstrdup(ptemp, srec->server_hostname != NULL ? srec->server_hostname : "_default_");
        for (addrs = srec->addrs; addrs != NULL; addrs = addrs->next)
        {
            char *nextEntry = apr_psprintf(ptemp,
                                           ",%s:%d",
                                           addrs->host_addr->hostname != NULL ? addrs->host_addr->hostname : "_default_",
                                           (unsigned int)addrs->host_addr->port);
            instanceHost = apr_pstrcat(ptemp, instanceHost, nextEntry, NULL);
        }
        add_data_string(string_table,
                        string_table_len,
                        instanceHost,
                        vhost_data != NULL ? &vhost_data->vhosts[vhost_element].instanceIDOffset : NULL);

        /* Create the hash table based on the server record address */
        /* Note: It would be nice to use server_rec pointer, but that didn't work properly */
        if (vhost_data != NULL)
        {
            apr_hash_set(vhost_hash, apr_psprintf(ptemp, "%pp", srec), APR_HASH_KEY_STRING, (void*)vhost_element);
        }

        /* Populate the remainder of the host */

        add_data_string(string_table,
                        string_table_len,
                        srec->server_hostname,
                        vhost_data != NULL ? &vhost_data->vhosts[vhost_element].hostNameOffset : NULL);

        if (srec->server_admin != NULL)
        {
            add_data_string(string_table,
                            string_table_len,
                            srec->server_admin,
                            vhost_data != NULL ? &vhost_data->vhosts[vhost_element].serverAdminOffset : NULL);
        }

        if (srec->error_fname != NULL)
        {
            add_data_string(string_table,
                            string_table_len,
                            srec->error_fname,
                            vhost_data != NULL ? &vhost_data->vhosts[vhost_element].logErrorOffset : NULL);
        }

        hostInfo = find_host_info(cfg, srec);
        if (hostInfo != NULL)
        {
            string_array* alias;        /* List of aliases for this server */

            add_data_string(string_table,
                            string_table_len,
                            hostInfo->transferLogFileName,
                            vhost_data != NULL ? &vhost_data->vhosts[vhost_element].logAccessOffset : NULL);
            add_data_string(string_table,
                            string_table_len,
                            hostInfo->customLogFileName,
                            vhost_data != NULL ? &vhost_data->vhosts[vhost_element].logCustomOffset : NULL);
            add_data_string(string_table,
                            string_table_len,
                            hostInfo->documentRoot,
                            vhost_data != NULL ? &vhost_data->vhosts[vhost_element].documentRootOffset : NULL);
            first = 1;
            for (alias = hostInfo->aliases; alias != NULL; alias = alias->next)
            {
                add_data_string(string_table,
                                string_table_len,
                                alias->string,
                                first && vhost_data != NULL ? &vhost_data->vhosts[vhost_element].serverAliasesOffset : NULL);
                first = 0;
            }
            /* Terminate the array of IP addresses and ports */
            terminate_data_string_array(string_table, string_table_len);
        }

        /* Populate the array of (IP address, port) */
        first = 1;
        for (addrs = srec->addrs; addrs != NULL; addrs = addrs->next)
        {
            add_data_string(string_table,
                            string_table_len,
                            addrs->host_addr->hostname != NULL ? addrs->host_addr->hostname : "_default_",
                            first && vhost_data != NULL ? &vhost_data->vhosts[vhost_element].addressesAndPortsOffset : NULL);
            add_data_string(string_table,
                            string_table_len,
                            encode64(addrs->host_addr->port),
                            NULL);
            first = 0;
        }

        if (!srec->is_virtual)
        {
            physical_host_element = vhost_element;
        }

        /* Terminate the array of IP addresses and ports */
        terminate_data_string_array(string_table, string_table_len);

        vhost_element++;
    }

    /* if there is a physical host, merge empty mergable properties of virtual hosts
     * with physical host properties */
    if (vhost_data != NULL && physical_host_element < vhost_count)
    {
        for (vhost_element = 2; vhost_element < vhost_count; vhost_element++)
        {
            if (vhost_element != physical_host_element)
            {
                merge_data_strings(&vhost_data->vhosts[vhost_element].logErrorOffset,
                                   vhost_data->vhosts[physical_host_element].logErrorOffset);
                merge_data_strings(&vhost_data->vhosts[vhost_element].logAccessOffset,
                                   vhost_data->vhosts[physical_host_element].logAccessOffset);
                merge_data_strings(&vhost_data->vhosts[vhost_element].logCustomOffset,
                                   vhost_data->vhosts[physical_host_element].logCustomOffset);
                merge_data_strings(&vhost_data->vhosts[vhost_element].documentRootOffset,
                                    vhost_data->vhosts[physical_host_element].documentRootOffset);
            }
        }
    }

    return APR_SUCCESS;
}

/* Get the data for the SSL certificates */
static apr_status_t collect_certificate_data(
    mmap_certificate_data* certificate_data,
    char* const string_table,
    apr_size_t* string_table_len,
    apr_pool_t* ptemp,
    config_sslCertFile* head,
    apr_size_t certificate_count)
{
    apr_size_t certificate_element;
    config_sslCertFile* cert_file_info;

    if (certificate_data != NULL)
    {
        certificate_data->count = certificate_count;
    }

    certificate_element = certificate_count - 1;
    for (cert_file_info = head; cert_file_info != NULL; cert_file_info = cert_file_info->next)
    {
        char* instanceHost;
        server_rec* s;
        server_addr_rec* addrs;

        /* save first host and port that use this certificate file */
        add_data_string(string_table,
                   string_table_len,
                   cert_file_info->certificateFileName,
                   certificate_data != NULL ? &certificate_data->certificates[certificate_element].certificateFileNameOffset : NULL);
        add_data_string(string_table,
                   string_table_len,
                   cert_file_info->hostName,
                   certificate_data != NULL ? &certificate_data->certificates[certificate_element].hostNameOffset : NULL);
        /* populate the virtual host identifier for this certificate as well */
        /* Note: This can't be done when reading configuration - server_hostname isn't initialized yet */
        s = cert_file_info->srec;
        instanceHost = apr_pstrdup(ptemp, s->server_hostname != NULL ? s->server_hostname : "_default_");
        for (addrs = s->addrs; addrs != NULL; addrs = addrs->next)
        {
            char *nextEntry = apr_psprintf(ptemp,
                                           ",%s:%d",
                                           addrs->host_addr->hostname != NULL ? addrs->host_addr->hostname : "_default_",
                                           (unsigned int)addrs->host_addr->port);
            instanceHost = apr_pstrcat(ptemp, instanceHost, nextEntry, NULL);
        }

        add_data_string(string_table,
                   string_table_len,
                   instanceHost,
                   certificate_data != NULL ? &certificate_data->certificates[certificate_element].virtualHostOffset : NULL);
        if (certificate_data != NULL)
        {
            certificate_data->certificates[certificate_element].port = cert_file_info->port;
        }
        certificate_element--;
    }
    return APR_SUCCESS;
}

/* Create and populate the shared memory region */

static apr_status_t mmap_region_create(persist_cfg *cfg, apr_pool_t *pool, apr_pool_t *ptemp, server_rec *head)
{
    apr_size_t module_count;            /* Number of modules loaded into server */
    apr_size_t vhost_count;             /* Save space for _Total and _Unknown */
    apr_size_t certificate_count;       /* Number of certificate information blocks */
    apr_status_t status;
    config_sslCertFile* cert_file_info; /* Ptr. to information about a certificate file */
    size_t stable_length;               /* Enough space for two null bytes as terminator */
    char* text;
    const char* server_hostname = NULL;
    server_rec* srec;
    module *modp;

    /* Walk the list of loaded modules to determine the count */

    module_count = 0;
    for (modp = ap_top_module; modp != NULL; modp = modp->next) {
        module_count++;
    }

    /* Walk the list of server_rec structures to determine the count of hosts */

    vhost_count = 2;
    for (srec = head; srec != NULL; srec = srec->next)
    {
        if (!srec->is_virtual)
        {
            server_hostname = srec->server_hostname;
        }
        text = apr_psprintf(ptemp, "cimprov: Server Vhost Name=%s, port=%d, is_virtual=%d",
                                   srec->server_hostname ? srec->server_hostname : "NULL",
                                   srec->port, srec->is_virtual);
        display_error(cfg, text, 0, 0);
        vhost_count++;
    }
    if (server_hostname == NULL)
    {
        server_hostname = "_Unknown";
    }

    /* Walk the list of certificate files to determine the count */

    certificate_count = 0;
    for (cert_file_info = cfg->configData->certificateFileList; cert_file_info != NULL; cert_file_info = cert_file_info->next)
    {
        certificate_count++;
    }

    text = apr_psprintf(ptemp, "cimprov: Count of hosts: %pS (including _Unknown & _Total)",
                        &vhost_count);
    display_error(cfg, text, 0, 0);
    text = apr_psprintf(ptemp, "cimprov: Count of certificates: %pS", &certificate_count);
    display_error(cfg, text, 0, 0);

    stable_length = 1;                  /* reserve space for a single empty string at the beginning of the string table */

    status = collect_server_data(NULL,
                                 NULL,
                                 &stable_length,
                                 ap_top_module,
                                 module_count,
                                 ap_conftree->filename,
                                 server_hostname);
    if (status != APR_SUCCESS)
    {
        display_error(cfg, "cimprov: collection of server data failed", status, 0);
        return status;
    }

    status = collect_vhost_data(NULL,
                                NULL,
                                &stable_length,
                                cfg,
                                ptemp,
                                NULL,
                                head,
                                vhost_count);
    if (status != APR_SUCCESS)
    {
        display_error(cfg, "cimprov: collection of vhost data failed", status, 0);
        return status;
    }

    status = collect_certificate_data(NULL,
                                      NULL,
                                      &stable_length,
                                      ptemp,
                                      cfg->configData->certificateFileList,
                                      certificate_count);
    if (status != APR_SUCCESS)
    {
        display_error(cfg, "cimprov: collection of certificate data failed", status, 0);
        return status;
    }

    /*
     * Create the memory mapped region
     */

    apr_size_t mapSize = sizeof(mmap_server_data) + (sizeof(mmap_server_modules) * module_count)
                       + sizeof(mmap_vhost_data) + (sizeof(mmap_vhost_elements) * vhost_count)
                       + sizeof(mmap_certificate_data) + (sizeof(mmap_certificate_elements) * certificate_count)
                       + sizeof(mmap_string_table) + stable_length;

    /* Region may already be mapped (due to a crash or something); try removing it just in case */
    /* (If successful, indicates improper shutdown, so log informationally; otherwise ignore error) */
    status = apr_shm_remove(PROVIDER_MMAP_NAME, pool);
    if (APR_SUCCESS == status)
    {
        display_error(cfg, "cimprov: mmap_region_create: delete success", status, 0);
    }

    text = apr_psprintf(ptemp, "cimprov: mmap_region_create: creating memory map %s with size %pS",
                        PROVIDER_MMAP_NAME, &mapSize);
    display_error(cfg, text, 0, 0);

    status = apr_shm_create(&cfg->mmap_region, mapSize, PROVIDER_MMAP_NAME, pool);
    if (APR_SUCCESS != status)
    {
        display_error(cfg, "cimprov: mmap_region_create failed to create shared region", status, 1);
        return status;
    }

    /*
     * Populate memory map.
     */

    /* Assign global pointers */
    cfg->server_data = (mmap_server_data*)apr_shm_baseaddr_get(cfg->mmap_region);
    cfg->vhost_data = (mmap_vhost_data*)(cfg->server_data->modules + module_count);
    cfg->certificate_data = (mmap_certificate_data*)(cfg->vhost_data->vhosts + vhost_count);
    cfg->string_data = (mmap_string_table*)(cfg->certificate_data->certificates + certificate_count);
    memset(cfg->server_data, 0, mapSize);

    /* Assign some other values */
    cfg->vhost_hash = apr_hash_make(pool);
    cfg->stable = cfg->string_data->data;
    *cfg->stable = '\0';                /* reserve a single empty string at the beginning of the string table */
    stable_length = 1;                  /* so that zero offsets result in empty strings */

    /* Initialize the memory mapped region for server data */
    status = collect_server_data(cfg->server_data,
                                 cfg->stable,
                                 &stable_length,
                                 ap_top_module,
                                 module_count,
                                 ap_conftree->filename,
                                 server_hostname);
    if (status != APR_SUCCESS)
    {
        display_error(cfg, "cimprov: collection of server data failed", status, 0);
        return status;
    }

    /* Initialize the memory mapped region for vhost data */

    status = collect_vhost_data(cfg->vhost_data,
                                cfg->stable,
                                &stable_length,
                                cfg,
                                ptemp,
                                cfg->vhost_hash,
                                head,
                                vhost_count);
    if (status != APR_SUCCESS)
    {
        display_error(cfg, "cimprov: collection of vhost data failed", status, 0);
        return status;
    }

    /* Initialize the memory mapped region for certificate file data */

    status = collect_certificate_data(cfg->certificate_data,
                                      cfg->stable,
                                      &stable_length,
                                      ptemp,
                                      cfg->configData->certificateFileList,
                                      certificate_count);
    if (status != APR_SUCCESS)
    {
        display_error(cfg, "cimprov: collection of certificate data failed", status, 0);
        return status;
    }

    /* Final initialization for string table */

    cfg->string_data->total_length = stable_length;

    display_error(cfg, "cimprov: mmap_region_create says buh bye", 0, 0);

    /* Register a cleanup handler */
    apr_pool_cleanup_register(pool, cfg, mmap_region_cleanup, apr_pool_cleanup_null);

    return APR_SUCCESS;
}

/*
 * Handlers
 */

/* Create the persist_config memory for a single host */
static void *create_config(apr_pool_t *pool, server_rec *s)
{
    apr_status_t status;

    /*
     * Initialize persistent storage
     *
     * To fetch this configuration for the pertinent host within handlers, use something like:
     *   persist_cfg *cfg = ap_get_module_config(r->server->module_config, &cimprov_module);
     */

    /* Create ONE configuration structure for all virtual and physical hosts */
    if (g_persistConfig == NULL)
    {
        persist_cfg *cfg = apr_pcalloc(pool, sizeof(persist_cfg));

        cfg->pool = pool;
        cfg->busyrefreshfrequency = 60; /* Update busy/refresh statistics every 60 seconds */

        /* Create sub-pool for configuration purposes and initialize configuration structure */
        status = apr_pool_create(&cfg->configPool, pool);
        if (APR_SUCCESS != status) {
            display_error(cfg, "create_config unable to create configuration pool", status, 1);
            return NULL;
        }
        cfg->configData = apr_pcalloc(cfg->configPool, sizeof(config_data));
        g_persistConfig = cfg;
    }

    return g_persistConfig;
}

static apr_status_t post_config_handler(apr_pool_t *pconf, apr_pool_t *plog, apr_pool_t *ptemp, server_rec *head)
{
    // Prevent double initialization of the module during Apache startup
    const char *errorText = "cimprov: post_config_handler";
    const char *key = "MSFT_cimprov_post_config";
    void *data = NULL;
    persist_cfg *cfg = (persist_cfg *) ap_get_module_config(head->module_config, &cimprov_module);

    apr_pool_userdata_get(&data, key, head->process->pool);
    if ( data == NULL )
    {
        apr_pool_userdata_set((const void *)1, key, apr_pool_cleanup_null, head->process->pool);
        return OK;
    }

    display_error(cfg, "cimprov: post_config_handler invocation ...", 0, 0);

    apr_status_t status;

    if (APR_SUCCESS != (status = module_mutex_initialize(cfg, pconf)))
    {
        return display_error(cfg, errorText, status, 1);
    }

#ifdef AP_NEED_SET_MUTEX_PERMS
#if AP_SERVER_MAJORVERSION_NUMBER == 2 && AP_SERVER_MINORVERSION_NUMBER == 2
    if (APR_SUCCESS != (status = unixd_set_global_mutex_perms(cfg->mutexMapRW)))
#elif AP_SERVER_MAJORVERSION_NUMBER == 2 && AP_SERVER_MINORVERSION_NUMBER >= 4
    if (APR_SUCCESS != (status = ap_unixd_set_global_mutex_perms(cfg->mutexMapRW)))
#endif
    {
        return display_error(cfg, "cimprov: post_config_handler could not set permissions on global mutex", status, 1);
    }
#endif // AP_NEED_SET_MUTEX_PERMS

    if (APR_SUCCESS != (status = mutex_lock(cfg, LOCKTYPE_RW)))
    {
        return display_error(cfg, errorText, status, 1);
    }

    if (APR_SUCCESS != (status = mutex_lock(cfg, LOCKTYPE_INIT)))
    {
        return display_error(cfg, errorText, status, 1);
    }

    display_error(cfg, "cimprov: post_config_handler creating memory mapped region", 0, 0);
    if (APR_SUCCESS != (status = mmap_region_create(cfg, pconf, ptemp, head)))
    {
        return display_error(cfg, errorText, status, 1);
    }

    /* Unlock */
    if (APR_SUCCESS != (status = mutex_unlock(cfg, LOCKTYPE_RW)))
    {
        return display_error(cfg, errorText, status, 1);
    }

    // Get the thread and process limits
    ap_mpm_query(AP_MPMQ_HARD_LIMIT_THREADS, &cfg->thread_limit);
    ap_mpm_query(AP_MPMQ_HARD_LIMIT_DAEMONS, &cfg->process_limit);

    /* We're completely initialized, so we don't need temporary configuration data anymore */
    apr_pool_destroy(cfg->configPool);
    cfg->configPool = NULL;
    cfg->configData = NULL;

    return OK;
}

static apr_status_t handle_VHostStatistics(const request_rec *r)
{
    persist_cfg *cfg = ap_get_module_config(r->server->module_config, &cimprov_module);
    const char *serverName = "_Default";
    if (r->server)
        serverName = r->server->server_hostname;

    int http_status = r->status;

    /* Find the hash value of the server record address */
    apr_size_t element = (apr_size_t)apr_hash_get(cfg->vhost_hash, apr_psprintf(r->pool, "%pp", r->server), APR_HASH_KEY_STRING);

    if ( element < 2 || element >= cfg->vhost_data->count )
    {
        /* Umm, somehow we got a host that wasn't in the hash; use _Unknown */
        element = 1;
    }

    /* Log our access for debugging purposes, if logging is enabled */

    if (cfg->enablelogging)
    {
        char *text = apr_psprintf(r->pool,
                                  "cimprov: Access result: Name=%s, status=%d, index=%pS",
                                  serverName ? serverName : "NULL",
                                  http_status, &element);
        display_error(cfg, text, 0, 0);
    }

    /* Count the statistics - and include in _Total */
    apr_atomic_inc32(&cfg->vhost_data->vhosts[element].requestsTotal);
    apr_atomic_add32(&cfg->vhost_data->vhosts[element].requestsBytes, r->bytes_sent);
    if (http_status >= 400 && http_status <= 499)
    {
        apr_atomic_inc32(&cfg->vhost_data->vhosts[element].errorCount400);
    }
    if (http_status >= 500 && http_status <= 599)
    {
        apr_atomic_inc32(&cfg->vhost_data->vhosts[element].errorCount500);
    }

    apr_atomic_inc32(&cfg->vhost_data->vhosts[0].requestsTotal);
    apr_atomic_add32(&cfg->vhost_data->vhosts[0].requestsBytes, r->bytes_sent);
    if (http_status >= 400 && http_status <= 499)
    {
        apr_atomic_inc32(&cfg->vhost_data->vhosts[0].errorCount400);
    }
    if (http_status >= 500 && http_status <= 599)
    {
        apr_atomic_inc32(&cfg->vhost_data->vhosts[0].errorCount500);
    }

    return APR_SUCCESS;
}

static apr_status_t handle_WorkerStatistics(const request_rec *r)
{
    persist_cfg *cfg = ap_get_module_config(r->server->module_config, &cimprov_module);
    apr_status_t status;

    /* If idle/busy lookup is diabled, return */
    if (-1 == cfg->busyrefreshfrequency)
    {
        return APR_SUCCESS;
    }

    /* See if it's time to determine idle/busy lookup */
    if (0 != cfg->busyrefreshfrequency)
    {
        apr_time_t currentTime = apr_time_now();
        apr_time_t lastUpdateTime;

        apr_uint32_t ansiUpdateTime = apr_atomic_read32((apr_uint32_t *) &cfg->server_data->busyRefreshTime);
        if (APR_SUCCESS != (status = apr_time_ansi_put(&lastUpdateTime, ansiUpdateTime)))
        {
            display_error(cfg, "cimprov: Error converting from time_t, forcing update", status, 0);
            lastUpdateTime = 0;
        }

        if (cfg->enablehystericallogging)
        {
            char *text = apr_psprintf(r->pool, "DEBUG: lastUpdateTime=%lu, currentTime=%lu, freq=%d",
                                      (unsigned long)lastUpdateTime, (unsigned long)currentTime, cfg->busyrefreshfrequency);
            display_error(cfg, text, 0, 0);
        }

        // Just bag if it's not time to do the refresh
        if (0 != lastUpdateTime && (lastUpdateTime + apr_time_from_sec(cfg->busyrefreshfrequency)) > currentTime)
        {
            return APR_SUCCESS;
        }

        // It's time to refresh - fall through, but take care to only run once in case of thread collision
        apr_uint32_t newUpdateTime = apr_time_sec(currentTime);
        apr_uint32_t originalTime = apr_atomic_cas32((apr_uint32_t *) &cfg->server_data->busyRefreshTime, newUpdateTime, ansiUpdateTime);

        if (cfg->enablehystericallogging)
        {
            char *text = apr_psprintf(r->pool, "DEBUG: originalTime=%d, ansiUpdateTime=%d, freq=%d",
                                      originalTime, ansiUpdateTime, cfg->busyrefreshfrequency);
            display_error(cfg, text, 0, 0);
        }

        if (originalTime != ansiUpdateTime)
        {
            // Some other thread is handling this, so we don't need to
            return APR_SUCCESS;
        }
    }

#if AP_SERVER_MAJORVERSION_NUMBER == 2 && AP_SERVER_MINORVERSION_NUMBER >= 4
    ap_generation_t mpm_generation;
#endif
    apr_uint32_t ready = 0, busy = 0;
    clock_t tu, ts, tcu, tcs;
    int i, j;
#ifdef HAVE_TIMES
    float tick;
    int times_per_thread = getpid() != g_child_pid;

#ifdef _SC_CLK_TCK
    tick = sysconf(_SC_CLK_TCK);
#else
    tick = HZ;
#endif
#endif

    tu = ts = tcu = tcs = 0;

#if defined(linux)
    char *text = apr_psprintf(r->pool, "cimprov: Computing Apache idle/busy thread/process counts in PID %d",
                              getpid());
    display_error(cfg, text, 0, 0);
#else
    display_error(cfg, "cimprov: Computing Apache idle/busy thread/process counts", 0, 0);
#endif // defined(linux)

#if AP_SERVER_MAJORVERSION_NUMBER == 2 && AP_SERVER_MINORVERSION_NUMBER >= 4
    ap_mpm_query(AP_MPMQ_GENERATION, &mpm_generation);
#endif

    for (i = 0; i < cfg->process_limit; ++i) {
        process_score *score_process;
        worker_score *score_worker;
        int state;
#ifdef HAVE_TIMES
        clock_t proc_tu = 0, proc_ts = 0, proc_tcu = 0, proc_tcs = 0;
        clock_t tmp_tu, tmp_ts, tmp_tcu, tmp_tcs;
#endif

        score_process = ap_get_scoreboard_process(i);
        for (j = 0; j < cfg->thread_limit; ++j) {
#if AP_SERVER_MAJORVERSION_NUMBER == 2 && AP_SERVER_MINORVERSION_NUMBER == 2
            score_worker = ap_get_scoreboard_worker(i, j);
#elif AP_SERVER_MAJORVERSION_NUMBER == 2 && AP_SERVER_MINORVERSION_NUMBER == 4
            score_worker = ap_get_scoreboard_worker_from_indexes(i, j);
#else
#error HTTPD Major/Minor Version not recognized
#endif
            state = score_worker->status;

            if (!score_process->quiescing && score_process->pid) {
#if AP_SERVER_MAJORVERSION_NUMBER == 2 && AP_SERVER_MINORVERSION_NUMBER == 2
                if (state == SERVER_READY && score_process->generation == ap_my_generation)
#elif AP_SERVER_MAJORVERSION_NUMBER == 2 && AP_SERVER_MINORVERSION_NUMBER >= 4
                if (state == SERVER_READY && score_process->generation == mpm_generation)
#endif
                {
                    ready++;
                }
                else if (state != SERVER_DEAD && state != SERVER_STARTING && state != SERVER_IDLE_KILL)
                {
                    busy++;
                }
            }

#ifdef HAVE_TIMES
            unsigned long lres = score_worker->access_count;

            if (lres != 0 || (state != SERVER_READY && state != SERVER_DEAD)) {
                tmp_tu = score_worker->times.tms_utime;
                tmp_ts = score_worker->times.tms_stime;
                tmp_tcu = score_worker->times.tms_cutime;
                tmp_tcs = score_worker->times.tms_cstime;

                if (times_per_thread) {
                    proc_tu += tmp_tu;
                    proc_ts += tmp_ts;
                    proc_tcu += tmp_tcu;
                    proc_tcs += proc_tcs;
                }
                else {
                    if (tmp_tu > proc_tu || tmp_ts > proc_ts || tmp_tcu > proc_tcu || tmp_tcs > proc_tcs) {
                        proc_tu = tmp_tu;
                        proc_ts = tmp_ts;
                        proc_tcu = tmp_tcu;
                        proc_tcs = proc_tcs;
                    }
                }
            }
#endif /* HAVE_TIMES */
        }

#ifdef HAVE_TIMES
        tu += proc_tu;
        ts += proc_ts;
        tcu += proc_tcu;
        tcs += proc_tcs;
#endif
    }

    // Because clock_t data structure can be 64-bit (on 64-bit platforms), and
    // because the APR doesn't support 64-bit atomics, we use a mutex here ...

    if (APR_SUCCESS != (status = mutex_lock(cfg, LOCKTYPE_RW)))
    {
        return display_error(cfg, "handle_WorkerStatistics: Error locking RW mutex", status, 1);
    }

#ifdef HAVE_TIMES
    if (ts || tu || tcu || tcs)
    {
        cfg->server_data->apacheCpuUtilization = (tu + ts + tcu + tcs) / tick;
    }
#endif

    // These could be updated atomically, but since we needed the mutex anyway ...
    cfg->server_data->idleApacheWorkers = ready;
    cfg->server_data->busyApacheWorkers = busy;

    if (APR_SUCCESS != (status = mutex_unlock(cfg, LOCKTYPE_RW)))
    {
        return display_error(cfg, "handle_WorkerStatistics: Error unlocking RW mutex", status, 1);
    }

    return APR_SUCCESS;
}

static int log_request_handler(request_rec *r)
{
    /* Handle the request here */
    handle_VHostStatistics(r);
    handle_WorkerStatistics(r);

    return DECLINED;
}

#ifdef HAVE_TIMES
static void child_init_handler(apr_pool_t *pool, server_rec *server)
{
    persist_cfg *cfg = ap_get_module_config(server->module_config, &cimprov_module);
    apr_status_t status;

    if (APR_SUCCESS != (status = apr_global_mutex_child_init(&cfg->mutexMapRW, MUTEX_RW_NAME, pool)))
    {
        display_error(cfg, "child_init_handler: failed to initialize child mutex", status, 1);
    }
    g_child_pid = getpid();
}
#endif

/*
*    Define the hooks and the functions registered to those hooks
*/
static void register_hooks(apr_pool_t *pool)
{
    ap_hook_log_transaction(log_request_handler, NULL, NULL, APR_HOOK_MIDDLE);
    ap_hook_post_config(post_config_handler, NULL, NULL, APR_HOOK_MIDDLE);
    ap_hook_child_init(child_init_handler, NULL, NULL, APR_HOOK_MIDDLE);
}

module AP_MODULE_DECLARE_DATA cimprov_module =
{
    STANDARD20_MODULE_STUFF,
    NULL,                       /* create per-dir    config structures */
    NULL,                       /* merge  per-dir    config structures */
    create_config,              /* create per-server config structures */
    NULL,                       /* merge  per-server config structures */
    cimprov_module_cmds,        /* table of config file commands       */
    register_hooks
};
