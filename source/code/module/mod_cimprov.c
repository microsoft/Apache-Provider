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

#if defined(HAVE_OPENSSL)
# include <openssl/x509.h>
# include <openssl/ssl.h>
# if OPENSSL_VERSION_NUMBER < 0x00904000     /* OpenSSL vers. < 0.9d */
#  define mod_PEM_read_bio_X509(b,x,c,a) PEM_read_bio_X509(b,x,c);
# else
#  define mod_PEM_read_bio_X509(b,x,c,a) PEM_read_bio_X509(b,x,c,a);
# endif
#endif

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

/*
 * Global memory definitions
 */

static apr_shm_t *g_mmap_region = NULL;
static mmap_server_data *g_server_data = NULL;
static mmap_vhost_data *g_vhost_data = NULL;

static apr_hash_t *g_vhost_hash = NULL;

static apr_global_mutex_t *mutexMapInit = NULL;
static apr_global_mutex_t *mutexMapRW = NULL;

static int g_enablelogging = 0;
static int g_enablehystericallogging = 0;
static int g_busyrefreshfrequency = 60;

static int g_process_limit = 0, g_thread_limit = 0;

#if defined(HAVE_OPENSSL)
static char g_defaultsslcertificatefile[PATH_MAX] = "";
#endif

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
 * Persistent server configuration data
 */

typedef struct {
    apr_pool_t *pool;
} persist_cfg;

/*
 * Utility functions
 */

apr_status_t display_error(const char *error_text, apr_status_t status, int fatal)
{
    // If logging disabled (and nothing "bad"), just return

    if (!g_enablelogging && !fatal)
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

static const char *set_logging_state(cmd_parms *cmd, void *dummy, int arg)
{
    const char *err = ap_check_cmd_context(cmd, GLOBAL_ONLY);
    if (err != NULL) {
        return err;
    }

    g_enablelogging = arg;
    return NULL;
}

static const char *set_hystericallogging_state(cmd_parms *cmd, void *dummy, int arg)
{
    const char *err = ap_check_cmd_context(cmd, GLOBAL_ONLY);
    if (err != NULL) {
        return err;
    }

    g_enablehystericallogging = arg;
    return NULL;
}

static const char *set_busyrefresh_frequency(cmd_parms *cmd, void *dummy, const char *arg)
{
    const char *err = ap_check_cmd_context(cmd, GLOBAL_ONLY);
    if (err != NULL) {
        return err;
    }

    g_busyrefreshfrequency = atoi(arg);
    return NULL;
}

#if defined(HAVE_OPENSSL)
static const char *set_default_ssl_certificate_file(cmd_parms *cmd, void *dummy, const char *arg)
{
    const char *err = ap_check_cmd_context(cmd, NOT_IN_DIR_LOC_FILE);
    if (err != NULL)
    {
        return err;
    }

    strncpy(g_defaultsslcertificatefile, arg, sizeof g_defaultsslcertificatefile);
    return NULL;
}
#endif

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
#if defined(HAVE_OPENSSL)
    AP_INIT_TAKE1("SSLCertificateFile", set_default_ssl_certificate_file, NULL, RSRC_CONF,
      "Set the name of the SSL certificate file for the default host."),
#endif
    {NULL}
};

/*
 * Mutex initialization and manipulation
 */

static apr_status_t mutex_lock(lock_type type)
{
    apr_status_t status;
    apr_global_mutex_t *mutex;

    if (LOCKTYPE_INIT == type)
    {
        if (g_enablehystericallogging)
        {
            display_error("cimprov: mutex_lock: locking INIT mutex", 0, 0);
        }
        mutex = mutexMapInit;
    }
    else
    {
        if (g_enablehystericallogging)
        {
            display_error("cimprov: mutex_lock: locking RW mutex", 0, 0);
        }
        mutex = mutexMapRW;
    }

    status = apr_global_mutex_lock(mutex);
    if (APR_SUCCESS != status)
    {
        display_error("cimprov: mutex_lock failed to lock mutex", status, 1);
        return status;
    }

    return APR_SUCCESS;
}

static apr_status_t mutex_unlock(lock_type type)
{
    apr_status_t status;
    apr_global_mutex_t *mutex;

    if (LOCKTYPE_INIT == type)
    {
        if (g_enablehystericallogging)
        {
            display_error("cimprov: mutex_unlock: unlocking INIT mutex", 0, 0);
        }
        mutex = mutexMapInit;
    }
    else
    {
        if (g_enablehystericallogging)
        {
            display_error("cimprov: mutex_unlock: unlocking RW mutex", 0, 0);
        }
        mutex = mutexMapRW;
    }

    status = apr_global_mutex_unlock(mutex);
    if (APR_SUCCESS != status)
    {
        display_error("cimprov: mutex_unlock failed to unlock mutex", status, 1);
        return status;
    }

    return APR_SUCCESS;
}

static apr_status_t module_mutex_cleanup(void *dummy)
{
    apr_status_t status;

    display_error("cimprov: module_mutex_cleanup invoked ...", 0, 0);

    // Clean up the mutexes
    // (We normally leave INIT mutex locked - so unlock it)
    if (APR_SUCCESS != (status = mutex_unlock(LOCKTYPE_INIT)))
    {
        display_error("cimprov: mutex_mutex_cleanup failed to unlock INIT mutex", status, 0);
        // A failure to unlock shouldn't abort our cleanup efforts ...
    }

    if (APR_SUCCESS != (status = apr_global_mutex_destroy(mutexMapInit)))
    {
        display_error("cimprov: mutex_mutex_cleanup failed to clean up INIT mutex", status, 0);
        // A failure here is abnormal, but shouldn't abort our cleanup efforts ...
    }

    if (APR_SUCCESS != (status = apr_global_mutex_destroy(mutexMapRW)))
    {
        display_error("cimprov: mutex_mutex_cleanup failed to clean up RW mutex", status, 1);
        return status;
    }

    return APR_SUCCESS;
}

static apr_status_t module_mutex_initialize(apr_pool_t *pool)
{
    apr_status_t status;

    // Initialize the mutexes
    display_error("cimprov: module_mutex_initialize: creating Init mutex", 0, 0);

    if (APR_SUCCESS != (status = apr_global_mutex_create(&mutexMapInit, MUTEX_INIT_NAME, APR_LOCK_DEFAULT, pool)))
    {
        display_error("cimprov: mutex_mutex_initialize failed to initialize INIT mutex", status, 1);
        return status;
    }

    display_error("cimprov: mutex_mutex_initialize: creating RW mutex", 0, 0);
    if (APR_SUCCESS != (status = apr_global_mutex_create(&mutexMapRW, MUTEX_RW_NAME, APR_LOCK_DEFAULT, pool)))
    {
        display_error("cimprov: mutex_mutex_initialize failed to initialize RW mutex", status, 1);
        return status;
    }

    // Register a cleanup handler
    apr_pool_cleanup_register(pool, NULL, module_mutex_cleanup, apr_pool_cleanup_null);

    return APR_SUCCESS;
}

#if defined(HAVE_OPENSSL)
/* Get an SSL certificate expiration date */
static apr_status_t get_certificate_expiration_time(const char* file, char* expirationCimTime, time_t* expirationPosixTime)
{
    apr_status_t status;
    X509* pCert = NULL;
    X509* pCertOut;
    int n;

    /* Open the certificate file and load it into an X509 structure */
    BIO* bio = BIO_new_file(file, "r");
    if (bio == NULL)
    {
        status = APR_FROM_OS_ERROR(apr_get_os_error());
        display_error("cimprov: failed to open certificate PEM file", status, 0);
        return status;
    }
     
    pCertOut = mod_PEM_read_bio_X509(bio, &pCert, NULL, NULL);
    BIO_free(bio);
    if (pCertOut == NULL)
    {
        status = APR_EINVAL;
        display_error("cimprov: certificate file not in correct PEM format", status, 0);
        return status;
    }

    /* get not after time */
    ASN1_TIME* notAfterTime = X509_get_notAfter(pCert);
    if (notAfterTime == NULL)
    {
        status = APR_EINVAL;
        display_error("cimprov: X.509 certificate does not contain valid date/time fields", status, 0);
        return status;
    }

    /* get the time field and check for valid length */
    const char* expirationX509Time = (char*)notAfterTime->data;
    if (strlen(expirationX509Time) < 13)
    {
        status = APR_EINVAL;
        display_error("cimprov: X.509 certificate does not contain valid date/time fields", status, 0);
        return status;
    }

    /* Make a CIM time from an ASN-1 time by prepending "20" to make a 4-digit year,
       by appending microseconds and by replacing the trailing "Z" with a CIM UTC
       indicator, "+000".
       Thus, "YYMMDDHHMMSSZ" becomes "20YYMMDDHHMMSS.000000+000". */
    sprintf(expirationCimTime, "20%12.12s.000000+000", expirationX509Time);

    /* get the expiation time in Posix form */
    struct tm expirationTm;
    memset(&expirationTm, 0, sizeof (struct tm));   /* set timezone and DST fields to 0 */
    n = sscanf(expirationX509Time,
               "%2d%2d%2d%2d%2d%2d",
               &expirationTm.tm_year,
               &expirationTm.tm_mon,
               &expirationTm.tm_mday,
               &expirationTm.tm_hour,
               &expirationTm.tm_min,
               &expirationTm.tm_sec);
    if (n != 6)
    {
        status = APR_EINVAL;
        display_error("cimprov: X.509 certificate does not contain valid date/time fields", status, 0);
        return status;
    }    
    expirationTm.tm_year += 100;    /* change year base from 2000 to 1900 */
    expirationTm.tm_mon--;          /* change month base from 1 to 0 */


    *expirationPosixTime = mktime(&expirationTm);

    return APR_SUCCESS;
}
#endif

/* Create memory mapped file for provider information */

static apr_status_t mmap_region_cleanup(void *dummy)
{
    apr_status_t status;

    display_error("cimprov: mmap_region_cleanup invoked", 0, 0);

    if (APR_SUCCESS != (status = apr_shm_destroy(g_mmap_region)))
    {
        display_error("cimprov: mmap_region_cleanup failed to destroy shared region", status, 1);
        return status;
    }

    return APR_SUCCESS;
}

static apr_status_t mmap_region_create(apr_pool_t *pool, apr_pool_t *ptemp, server_rec *head)
{
    int module_count = 0;               /* Number of modules loaded into server */
    int vhost_count = 2;                /* Save space for _Total and _Default */
    char *text;

    /* Walk the list of loaded modules to determine the count */

    module *modp = NULL;
    for (modp = ap_top_module; modp; modp = modp->next) {
        module_count++;
    }

    /* Walk the list of server_rec structures to determine the count of virtual hosts */

    server_rec *s = head;
    apr_port_t default_port = 0;
    while (s != NULL)
    {
        if (default_port == 0 && s->port != 0 && s->is_virtual == 0)
        {
            default_port = s->port;
            text = apr_psprintf(ptemp, "cimprov: *** Default port has been set to %d ***",
                                default_port);
            display_error(text, 0, 0);
        }

        text = apr_psprintf(ptemp, "cimprov: Server Vhost Name=%s, port=%d, is_virtual=%d",
                                  s->server_hostname ? s->server_hostname : "NULL",
                                  s->port, s->is_virtual);
        display_error(text, 0, 0);

        if (s->is_virtual && s->server_hostname != NULL)
            vhost_count++;

        s = s->next;
    }

    text = apr_psprintf(ptemp, "cimprov: Count of virtual hosts: %d (including _Default & _Total)",
                        vhost_count);
    display_error(text, 0, 0);

    /*
     * Create the memory mapped region
     */

    apr_status_t status;
    apr_size_t mapSize = sizeof(mmap_server_data) + (sizeof(mmap_server_modules) * module_count)
                       + sizeof(mmap_vhost_data) + (sizeof(mmap_vhost_elements) * vhost_count);

    /* Region may already be mapped (due to a crash or something); try removing it just in case */
    status = apr_shm_remove(PROVIDER_MMAP_NAME, pool);
    if (APR_SUCCESS != status)
    {
        display_error("cimprov: mmap_region_create: delete failed", status, 0);
    }
    else
    {
        display_error("cimprov: mmap_region_create: delete success", status, 0);
    }

    text = apr_psprintf(ptemp, "cimprov: mmap_region_create: creating memory map %s with size %lu",
                        PROVIDER_MMAP_NAME, mapSize);
    display_error(text, 0, 0);

    status = apr_shm_create(&g_mmap_region, mapSize, PROVIDER_MMAP_NAME, pool);
    if (APR_SUCCESS != status)
    {
        display_error("cimprov: mmap_region_create failed to create shared region", status, 1);
        return status;
    }

    g_vhost_hash = apr_hash_make(pool);

    /* Assign global pointers */
    g_server_data = apr_shm_baseaddr_get(g_mmap_region);
    g_vhost_data = (void *) (g_server_data->modules + module_count);
    memset(g_server_data, 0, mapSize);

    /*
     * Initialize the memory mapped region for server data
     *
     * Process name is the second part of configuration filename:
     *   If configuration file is: /etc/httpd/conf/httpd.conf, we pick: httpd
     *   If configuration file is: /etc/apache2/apache2.conf, we pick: apache2
     *   If configuration file is: /etc/apache2/httpd.conf, we pick: apache2
     * So just pick the second directory in the configuration filename.
     *
     *   If the configuration file begins with "/usr/local", treat the "/usr/local"
     *   just like the "/etc", above.
     *
     * Note: If config filename isn't one of those, keep blank (unknown installation)
     */

    g_server_data->moduleCount = module_count;
    apr_cpystrn(g_server_data->configFile, ap_conftree->filename, sizeof(g_server_data->configFile));

    char *tok_last;
    char *processName = apr_pstrdup(ptemp, ap_conftree->filename);
    char *token = apr_strtok(processName, "/", &tok_last);

    /* Skip /usr/local for install by source compiles */
    if (0 == apr_strnatcasecmp(token, "local"))
    {
        token = apr_strtok(NULL, "/", &tok_last);
    }

    if (0 == apr_strnatcasecmp(token, "apache2") || 0 == apr_strnatcasecmp(token, "httpd"))
    {
        apr_cpystrn(g_server_data->processName, token, sizeof(g_server_data->processName));
    }

    g_server_data->operatingStatus = 2 /* From MOF file: OPERATING_STATUS_OK */;

    /* Add each of the loaded modules */

    int index = 0;
    for (modp = ap_top_module; modp; modp = modp->next) {
        if (index >= module_count)
        {
            status = APR_EMISMATCH;
            display_error("cimprov: mmap_region_create failed to allocate module storage properly", status, 0);
            return status;
        }

        apr_cpystrn(g_server_data->modules[index++].moduleName, modp->name, sizeof(g_server_data->modules[0].moduleName));

    }

    /* Initialize the memory mapped region for virtual host data */

    g_vhost_data->count = vhost_count;

    /*
     * VirtualHost [0] reserved for _Total
     * VirtualHost [1] reserved for _Default
     */

    apr_cpystrn(g_vhost_data->vhosts[0].name, "_Total", MAX_VIRTUALHOST_NAME_LEN);
    apr_cpystrn(g_vhost_data->vhosts[1].name, "_Default", MAX_VIRTUALHOST_NAME_LEN);

#if defined(HAVE_OPENSSL)
    if (ap_find_linked_module("mod_ssl.c") != NULL)
    {
        apr_cpystrn(g_vhost_data->vhosts[1].certificateFile, g_defaultsslcertificatefile, PATH_MAX);
        get_certificate_expiration_time(g_defaultsslcertificatefile,
                                        g_vhost_data->vhosts[1].certificateExpirationCimTime,
                                        &g_vhost_data->vhosts[1].certificateExpirationPosixTime);
    }
#endif

    /* Set up virtual host map in reverse order since that's how server_rc comes to us */

    apr_size_t vhost_element = vhost_count - 1; /* Since count is 1-based, reference the last element */
    s = head;
    while (s != NULL)
    {
        if (s->is_virtual)
        {
            apr_hash_set(g_vhost_hash, s->server_hostname, APR_HASH_KEY_STRING, (void *) vhost_element);

            apr_cpystrn(g_vhost_data->vhosts[vhost_element].name, s->server_hostname, MAX_VIRTUALHOST_NAME_LEN);
            // TODO: Populate documentRoot, but not obvious in server_rec (it's not s->path)
            //if (s->path)
            //{
            //    apr_cpystrn(g_vhost_data->vhosts[vhost_element].documentRoot,s->path, sizeof(g_vhost_entries->documentRoot));
            //}
            if (s->server_admin)
            {
                apr_cpystrn(g_vhost_data->vhosts[vhost_element].serverAdmin, s->server_admin, sizeof(g_vhost_data->vhosts[0].serverAdmin));
            }
            if (s->error_fname)
            {
                apr_cpystrn(g_vhost_data->vhosts[vhost_element].logError, s->error_fname, sizeof(g_vhost_data->vhosts[0].logError));
            }
            // TODO: Populate logCustom, but not obvious in server_rec
            // TODO: Populate logAccess, but not obvious in server_rec
            g_vhost_data->vhosts[vhost_element].port_http = (s->port ? s->port : default_port);
            vhost_element--;
        }

        s = s->next;
    }

    display_error("cimprov: mmap_region_create says buh bye", 0, 0);

    // Register a cleanup handler
    apr_pool_cleanup_register(pool, NULL, mmap_region_cleanup, apr_pool_cleanup_null);

    return APR_SUCCESS;
}

/*
 * Handlers
 */

static void *create_config(apr_pool_t *pool, server_rec *s)
{
    /*
     * Initialize persistent storage
     *
     * To fetch this configuration within handlers, use something like:
     *   persist_cfg *cfg = ap_get_module_config(r->server->module_config, &cimprov_module);
     */

    persist_cfg *cfg = apr_palloc(pool, sizeof(persist_cfg));

    cfg->pool = pool;
    return cfg;
}

static apr_status_t post_config_handler(apr_pool_t *pconf, apr_pool_t *plog, apr_pool_t *ptemp, server_rec *head)
{
    // Prevent double initialization of the module during Apache startup
    const char *errorText = "cimprov: post_config_handler";
    const char *key = "MSFT_cimprov_post_config";
    void *data = NULL;

    apr_pool_userdata_get(&data, key, head->process->pool);
    if ( data == NULL )
    {
        apr_pool_userdata_set((const void *)1, key, apr_pool_cleanup_null, head->process->pool);
        return OK;
    }

    display_error("cimprov: post_config_handler invocation ...", 0, 0);

    apr_status_t status;

    if (APR_SUCCESS != (status = module_mutex_initialize(pconf)))
    {
        return display_error(errorText, status, 1);
    }

#ifdef AP_NEED_SET_MUTEX_PERMS
    if (APR_SUCCESS != (status = unixd_set_global_mutex_perms(mutexMapRW)))
    {
        return display_error("cimprov: post_config_handler could not set permissions on global mutex", status, 1);
    }
#endif // AP_NEED_SET_MUTEX_PERMS

    if (APR_SUCCESS != (status = mutex_lock(LOCKTYPE_RW)))
    {
        return display_error(errorText, status, 1);
    }

    if (APR_SUCCESS != (status = mutex_lock(LOCKTYPE_INIT)))
    {
        return display_error(errorText, status, 1);
    }

    display_error("cimprov: post_config_handler creating memory mapped region", 0, 0);
    if (APR_SUCCESS != (status = mmap_region_create(pconf, ptemp, head)))
    {
        return display_error(errorText, status, 1);
    }

    /* Unlock */
    if (APR_SUCCESS != (status = mutex_unlock(LOCKTYPE_RW)))
    {
        return display_error(errorText, status, 1);
    }

    // Get the thread and process limits
    ap_mpm_query(AP_MPMQ_HARD_LIMIT_THREADS, &g_thread_limit);
    ap_mpm_query(AP_MPMQ_HARD_LIMIT_DAEMONS, &g_process_limit);

    return OK;
}

static apr_status_t handle_VirtualHostStatistics(const request_rec *r)
{
    const char *serverName = "_Default";
    if (r->server)
        serverName = r->server->server_hostname;

    int http_status = r->status;

    /* Find the hash value of the server name */
    apr_size_t element = (apr_size_t) apr_hash_get(g_vhost_hash, serverName, APR_HASH_KEY_STRING);

    if ( element < 2 || element >= g_vhost_data->count )
    {
        /* Umm, somehow we got a virtual host that wasn't in the hash; use _Default */
        element = 1;
    }

    /* Log our access for debugging purposes, if logging is enabled */

    if (g_enablelogging)
    {
        char *text = apr_psprintf(r->pool,
                                  "cimprov: Access result: Name=%s, status=%d, index=%lu",
                                  serverName ? serverName : "NULL",
                                  http_status, element);
        display_error(text, 0, 0);
    }

    /* Count the statistics - and include in _Total */
    apr_atomic_inc32(&g_vhost_data->vhosts[element].requestsTotal);
    apr_atomic_add32(&g_vhost_data->vhosts[element].requestsBytes, r->bytes_sent);
    if (http_status >= 400 && http_status <= 499)
    {
        apr_atomic_inc32(&g_vhost_data->vhosts[element].errorCount400);
    }
    if (http_status >= 500 && http_status <= 599)
    {
        apr_atomic_inc32(&g_vhost_data->vhosts[element].errorCount500);
    }

    apr_atomic_inc32(&g_vhost_data->vhosts[0].requestsTotal);
    apr_atomic_add32(&g_vhost_data->vhosts[0].requestsBytes, r->bytes_sent);
    if (http_status >= 400 && http_status <= 499)
    {
        apr_atomic_inc32(&g_vhost_data->vhosts[0].errorCount400);
    }
    if (http_status >= 500 && http_status <= 599)
    {
        apr_atomic_inc32(&g_vhost_data->vhosts[0].errorCount500);
    }

    return APR_SUCCESS;
}

static apr_status_t handle_WorkerStatistics(const request_rec *r)
{
    apr_status_t status;

    /* If idle/busy lookup is diabled, return */
    if (-1 == g_busyrefreshfrequency)
    {
        return APR_SUCCESS;
    }

    /* See if it's time to determine idle/busy lookup */
    if (0 != g_busyrefreshfrequency)
    {
        apr_time_t currentTime = apr_time_now();
        apr_time_t lastUpdateTime;

        apr_uint32_t ansiUpdateTime = apr_atomic_read32((apr_uint32_t *) &g_server_data->busyRefreshTime);
        if (APR_SUCCESS != (status = apr_time_ansi_put(&lastUpdateTime, ansiUpdateTime)))
        {
            display_error("cimprov: Error converting from time_t, forcing update", status, 0);
            lastUpdateTime = 0;
        }

        if (g_enablehystericallogging)
        {
            char *text = apr_psprintf(r->pool, "DEBUG: lastUpdateTime=%lu, currentTime=%lu, freq=%d",
                                      lastUpdateTime, currentTime, g_busyrefreshfrequency);
            display_error(text, 0, 0);
        }

        // Just bag if it's not time to do the refresh
        if (0 != lastUpdateTime && (lastUpdateTime + apr_time_from_sec(g_busyrefreshfrequency)) > currentTime)
        {
            return APR_SUCCESS;
        }

        // It's time to refresh - fall through, but take care to only run once in case of thread collision
        apr_uint32_t newUpdateTime = apr_time_sec(currentTime);
        apr_uint32_t originalTime = apr_atomic_cas32((apr_uint32_t *) &g_server_data->busyRefreshTime, newUpdateTime, ansiUpdateTime);

        if (g_enablehystericallogging)
        {
            char *text = apr_psprintf(r->pool, "DEBUG: originalTime=%d, ansiUpdateTime=%d, freq=%d",
                                      originalTime, ansiUpdateTime, g_busyrefreshfrequency);
            display_error(text, 0, 0);
        }

        if (originalTime != ansiUpdateTime)
        {
            // Some other thread is handling this, so we don't need to
            return APR_SUCCESS;
        }
    }

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
    display_error(text, 0, 0);
#else
    display_error("cimprov: Computing Apache idle/busy thread/process counts", 0, 0);
#endif // defined(linux)

    for (i = 0; i < g_process_limit; ++i) {
        process_score *score_process;
        worker_score *score_worker;
        int state;
#ifdef HAVE_TIMES
        clock_t proc_tu = 0, proc_ts = 0, proc_tcu = 0, proc_tcs = 0;
        clock_t tmp_tu, tmp_ts, tmp_tcu, tmp_tcs;
#endif

        score_process = ap_get_scoreboard_process(i);
        for (j = 0; j < g_thread_limit; ++j) {
            score_worker = ap_get_scoreboard_worker(i, j);
            state = score_worker->status;

            if (!score_process->quiescing && score_process->pid) {
                if (state == SERVER_READY && score_process->generation == ap_my_generation)
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

    if (APR_SUCCESS != (status = mutex_lock(LOCKTYPE_RW)))
    {
        return display_error("handle_WorkerStatistics: Error locking RW mutex", status, 1);
    }

#ifdef HAVE_TIMES
    if (ts || tu || tcu || tcs)
    {
        g_server_data->apacheCpuUtilization = (tu + ts + tcu + tcs) / tick;
    }
#endif

    // These could be updated atomically, but since we needed the mutex anyway ...
    g_server_data->idleApacheWorkers = ready;
    g_server_data->busyApacheWorkers = busy;

    if (APR_SUCCESS != (status = mutex_unlock(LOCKTYPE_RW)))
    {
        return display_error("handle_WorkerStatistics: Error unlocking RW mutex", status, 1);
    }

    return APR_SUCCESS;
}

static int log_request_handler(request_rec *r)
{
    /* Handle the request here */
    handle_VirtualHostStatistics(r);
    handle_WorkerStatistics(r);

    return DECLINED;
}

#ifdef HAVE_TIMES
static void child_init_handler(apr_pool_t *pool, server_rec *server)
{
    apr_status_t status;
    if (APR_SUCCESS != (status = apr_global_mutex_child_init(&mutexMapRW, MUTEX_RW_NAME, pool)))
    {
        display_error("child_init_handler: failed to initialize child mutex", status, 1);
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
