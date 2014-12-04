/* @migen@ */
#include <stdio.h>
#include <string.h>

#include <apr.h>
#include <apr_strings.h>
#include <apr_errno.h>

#include <string>
#include <vector>

#include <MI.h>
#include <micxx/datetime.h>

#include <mmap_region.h>
#include "apachebinding.h"
#include "cimconstants.h"
#include "utils.h"
#include "Apache_HTTPDVirtualHostCertificate_Class_Provider.h"

static char s_monNames[12][4] =
{
    "Jan",
    "Feb",
    "Mar",
    "Apr",
    "May",
    "Jun",
    "Jul",
    "Aug",
    "Sep",
    "Oct",
    "Nov",
    "Dec"
};

// Get an SSL certificate expiration date and days until it expires
static int GetCertificateExpirationDate(
    const char* file,
    apr_pool_t* pool,
    char* date,
    apr_time_t* expirationAprTime)
{
    char dateString[64];
    char buf[128];
    char monName[8];
    int year, month, day, hour, min, sec;
    struct tm tmBuf;
    apr_status_t status;
    apr_proc_t proc;
    apr_procattr_t *pattr;
    apr_exit_why_e why;
    int exitCode;
    const char* args[] =
    {
        "/usr/bin/openssl",
        "x509",
        "-in",
        file,
        "-enddate",
        NULL
    };
    char* const* argptr =  const_cast<char* const*>(args);

    // Initialize the process attribute
    status = apr_procattr_create(&pattr, pool);
    if (status != APR_SUCCESS)
    {
        DisplayError(status, "GetCertificateExpirationDate: error creating openssl child process attributes");
        return -1;
    }

    // Set up the pipe of stdout from the child to this process' proc.out
    status = apr_procattr_io_set(pattr, APR_NO_PIPE, APR_FULL_BLOCK, APR_NO_PIPE);
    if (status != APR_SUCCESS)
    {
        DisplayError(status, "GetCertificateExpirationDate: error setting openssl child process i/o attributes");
        return -1;
    }

    // Make the openssl program be run using the PATH variable
    status = apr_procattr_cmdtype_set(pattr, APR_PROGRAM_PATH);
    if (status != APR_SUCCESS)
    {
        DisplayError(status, "GetCertificateExpirationDate: error setting openssl child process command type");
        return -1;
    }

    // Run the openssl binary
    status = apr_proc_create(&proc, "openssl", argptr, NULL, (apr_procattr_t*)pattr, pool);
    if (status != APR_SUCCESS)
    {
        DisplayError(status, "GetCertificateExpirationDate: error creating openssl child process");
        return -1;
    }

    // Read the first line from the child process stdout
    status = apr_file_gets(dateString, sizeof(dateString), proc.out);
    if (status != APR_SUCCESS)
    {
        DisplayError(status, "GetCertificateExpirationDate: error reading openssl process output");
        return -1;
    }

    // Drain the output from the child process
    while (apr_file_gets(buf, 128, proc.out) == APR_SUCCESS)
    {
        ;
    }

    // Wait for the child process to finish
    status = apr_proc_wait(&proc, &exitCode, &why, APR_WAIT);
    if (!APR_STATUS_IS_CHILD_DONE(status))
    {
        DisplayError(status, "GetCertificateExpirationDate: openssl process did not finish successfully");
        return -1;
    }

    // check for format: "notAfter=Mmm dd hh:mm:ss yyyy GMT", with Mmm
    // being a three-letter Engish abbreviation for the month name
    if (strncmp(dateString, "notAfter=", 9) != 0)
    {
        DisplayError(apr_get_netos_error(), "GetCertificateExpirationDate: could not read openssl notAfter date");
        return -1;
    }
    if (sscanf(&dateString[9], "%3s%d%d:%d:%d %d", monName, &day, &hour, &min, &sec, &year) < 6)
    {
        DisplayError(apr_get_netos_error(), "GetCertificateExpirationDate: could not read openssl notAfter date");
        return -1;
    }
    for (month = 0; month < 12; month++)
    {
        if (apr_strnatcasecmp(monName, s_monNames[month]) == 0)
            break;
    }
    if (month == 12)
    {
        DisplayError(apr_get_netos_error(), "GetCertificateExpirationDate: could not read openssl notAfter month");
        return -1;
    }

    // Set the expiration date in CIM time
    sprintf(date, "%04d%02d%02d%02d%02d%02d.000000+000", year, month+1, day, hour, min, sec);

    // Compute APR time of expiration
    memset(&tmBuf, 0, sizeof (struct tm));   // set DST and time zone to 0
    tmBuf.tm_year = year - 1900;
    tmBuf.tm_mon = month;
    tmBuf.tm_mday = day;
    tmBuf.tm_hour = hour;
    tmBuf.tm_min = min;
    tmBuf.tm_sec = sec;
    *expirationAprTime = (apr_time_t)mktime(&tmBuf) * 1000000;

    return 0;
}

#if !defined(_WIN32)

static apr_uint32_t QuickHash(const char* str)
{
    apr_uint32_t hash = 0x4F3AB917;
    apr_uint32_t c;

    while ((c = (apr_uint32_t)(unsigned char)*str++) != '\0')
    {
        hash *= 37;
        hash += (c + 19);
    }
    return hash;
}

#endif

MI_BEGIN_NAMESPACE

static void EnumerateOneInstance(
    Context& context,
    bool keysOnly,
    apr_size_t item,
    ApacheDataCollector& data)
{
    Apache_HTTPDVirtualHostCertificate_Class inst;
    mmap_certificate_elements* certs = data.GetCertificateElements();
    const char* certificateFileName = data.GetDataString(certs[item].certificateFileNameOffset);
    const char* openSslVersion = GetApacheComponentVersion(data.GetServerVersion(), "OpenSSL");

#if defined(_WIN32)
    // Since Windows file names are case-insensitive, just use the certificate file name
    // as the instance ID
    const mi::String idMiString(certificateFileName);
#else
    // For case-sensitive file systems, make the instance ID from the base certificate file name +
    // a hash of the entire path
    apr_uint32_t hash = QuickHash(certificateFileName);
    const char* ptr = strrchr(certificateFileName, '/');

    if (ptr == NULL)
    {
        ptr = certificateFileName;
    }
    else
    {
        ptr++;
    }
    const char* idStr = apr_psprintf(data.GetPool(), "%s*%08x", ptr, (unsigned int)hash);

    const mi::String idMiString(idStr);
#endif

    inst.Name_value(idMiString);
    inst.Version_value(openSslVersion);
    inst.SoftwareElementID_value(idMiString);
    inst.TargetOperatingSystem_value(CIM_TARGET_OPERATING_SYSTEM);
    inst.SoftwareElementState_value(CIM_SOFTWARE_ELEMENT_STATE_RUNNING);
    inst.InstanceID_value(idMiString);

    if (!keysOnly)
    {
        apr_finfo_t fileInfo;
        apr_time_t timeNow;
        apr_status_t status;

        // Insert the appropriate value to map the certficate to the virtual host
        inst.VirtualHost_value(data.GetDataString(certs[item].virtualHostOffset));
        inst.FileName_value(certificateFileName);

        // Insert the certificate file dates
        timeNow = apr_time_now();

        // See if the file is newer that the stored dates
        status = apr_stat(&fileInfo, certificateFileName, APR_FINFO_MTIME, data.GetPool());
        if (status != APR_SUCCESS || fileInfo.mtime != certs[item].certificateFileMtime)
        {
            // Get an updated copy of the certificate date information
            certs[item].certificateFileMtime = fileInfo.mtime;
            if (GetCertificateExpirationDate(certificateFileName,
                                             data.GetPool(),
                                             certs[item].certificateExpirationCimTime,
                                             &certs[item].certificateExpirationAprTime) < 0)
            {
                // Problem getting certificate expiration information; try later
                certs[item].certificateFileMtime = 0;
            }
        }

        // Supply expiration information if we were able to compute it
        if (certs[item].certificateFileMtime)
        {
            // Convert the the certificate information to MI types and put in into the instance
            mi::Datetime certificateExpirationCimTime;
            certificateExpirationCimTime.Set(certs[item].certificateExpirationCimTime);

            mi::Uint16 certificateDaysUntilExpiration(
                ( certs[item].certificateExpirationAprTime >= timeNow
                  ? (mi::Uint16)((certs[item].certificateExpirationAprTime - timeNow) /
                                     ((apr_int64_t)1000000 * 60 * 60 * 24))
                  : 0 ));
            inst.ExpirationDate_value(certificateExpirationCimTime);
            inst.DaysUntilExpiration_value(certificateDaysUntilExpiration);
        }
    }

    context.Post(inst);

    return;
}

Apache_HTTPDVirtualHostCertificate_Class_Provider::Apache_HTTPDVirtualHostCertificate_Class_Provider(
    Module* module) :
    m_Module(module)
{
}

Apache_HTTPDVirtualHostCertificate_Class_Provider::~Apache_HTTPDVirtualHostCertificate_Class_Provider()
{
}

void Apache_HTTPDVirtualHostCertificate_Class_Provider::Load(
        Context& context)
{
    CIM_PEX_BEGIN
    {
        if (NULL == g_pFactory)
        {
            g_pFactory = new ApacheFactory();
        }

        if (APR_SUCCESS != g_pFactory->GetInit()->Load("VirtualHostCertificate"))
        {
            context.Post(MI_RESULT_FAILED);
            return;
        }

        // Notify that we don't wish to unload
        MI_Result r = context.RefuseUnload();
        if (r != MI_RESULT_OK)
        {
            DisplayError(OMI_Error(r), "Apache_HTTPDVirtualHostCertificate_Class_Provider refuses to not unload");
        }

        context.Post(MI_RESULT_OK);
    }
    CIM_PEX_END( "Apache_HTTPDVirtualHostCertificate_Class_Provider::Load" );
}

void Apache_HTTPDVirtualHostCertificate_Class_Provider::Unload(
        Context& context)
{
    CIM_PEX_BEGIN
    {
        if (APR_SUCCESS != g_pFactory->GetInit()->Unload("VirtualHostCertificate"))
        {
            context.Post(MI_RESULT_FAILED);
            return;
        }

        context.Post(MI_RESULT_OK);
    }
    CIM_PEX_END( "Apache_HTTPDVirtualHostCertificate_Class_Provider::Unload" );
}

void Apache_HTTPDVirtualHostCertificate_Class_Provider::EnumerateInstances(
    Context& context,
    const String& nameSpace,
    const PropertySet& propertySet,
    bool keysOnly,
    const MI_Filter* filter)
{
    ApacheDataCollector data = g_pFactory->DataCollectorFactory();

    CIM_PEX_BEGIN
    {
        apr_status_t status;

        if (APR_SUCCESS != data.Attach("Apache_HTTPDVirtualHostCertificate_Class_Provider::EnumerateInstances"))
        {
            context.Post(MI_RESULT_FAILED);
            return;
        }

        // Lock the mutex to walk the list
        if (APR_SUCCESS != (status = data.LockMutex()))
        {
            DisplayError(status, "VirtualHostCertificate::EnumerateInstances: failed to lock mutex");
            context.Post(MI_RESULT_FAILED);
            return;
        }

        for (apr_size_t item = 0; item < data.GetCertificateCount(); item++)
        {
            EnumerateOneInstance(context, keysOnly, item, data);
        }

        context.Post(MI_RESULT_OK);
    }
    CIM_PEX_END( "Apache_HTTPDVirtualHostCertificate_Class_Provider::EnumerateInstances" );

    // Be sure mutex gets unlocked, regardless if an exception occurs
    data.UnlockMutex();
}

void Apache_HTTPDVirtualHostCertificate_Class_Provider::GetInstance(
    Context& context,
    const String& nameSpace,
    const Apache_HTTPDVirtualHostCertificate_Class& instanceName,
    const PropertySet& propertySet)
{
    context.Post(MI_RESULT_NOT_SUPPORTED);
}

void Apache_HTTPDVirtualHostCertificate_Class_Provider::CreateInstance(
    Context& context,
    const String& nameSpace,
    const Apache_HTTPDVirtualHostCertificate_Class& newInstance)
{
    context.Post(MI_RESULT_NOT_SUPPORTED);
}

void Apache_HTTPDVirtualHostCertificate_Class_Provider::ModifyInstance(
    Context& context,
    const String& nameSpace,
    const Apache_HTTPDVirtualHostCertificate_Class& modifiedInstance,
    const PropertySet& propertySet)
{
    context.Post(MI_RESULT_NOT_SUPPORTED);
}

void Apache_HTTPDVirtualHostCertificate_Class_Provider::DeleteInstance(
    Context& context,
    const String& nameSpace,
    const Apache_HTTPDVirtualHostCertificate_Class& instanceName)
{
    context.Post(MI_RESULT_NOT_SUPPORTED);
}

MI_END_NAMESPACE
