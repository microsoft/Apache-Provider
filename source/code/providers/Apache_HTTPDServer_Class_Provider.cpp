/* @migen@ */
#include <MI.h>
#include "Apache_HTTPDServer_Class_Provider.h"

// Provider include definitions
#include <stdlib.h>
#include <sstream>
#include <string.h>
#include <vector>

#include <apr_file_io.h>

#include "apachebinding.h"
#include "cimconstants.h"
#include "utils.h"
#include "buildversion.h"

// From MOF file:
const char *OperatingStatusValues[] =
    {
        /*  0 */ "Unknown",                     "Other",                "OK",           "Degraded",
        /*  4 */ "Stressed",                    "Predictive Failure",   "Error",        "Non-Recoverable Error",
        /*  8 */ "Starting",                    "Stopping",             "Stopped",      "In Service",
        /* 12 */ "No Contact",                  "Lost Communication",   "Aborted",      "Dormant",
        /* 16 */ "Supporting Entity in Error",  "Completed",            "Power Mode"
    };


MI_BEGIN_NAMESPACE

/* Check if native systemd services are supported, and if service exists under systemd */
static int CheckServiceSystemd(apr_pool_t* pool, const char* serviceName)
{
    // Return codes:
    //    0 : systemd supported, service exists
    //    1 : systemd supported, services does not exist
    //    2 : systemd supported, but an internal error occurred
    //   -1 : systemd not supported

#if defined(linux)
    int exitStatus;

    // The new systemd capability (via systemctl) will be automatically used if
    // /etc/init.d script doesn't exist. However, systemctl will return error 3
    // if the service is down OR if the service doesn't exist. This is problematic
    // for us to determine the service name, since init.d-style scripts return
    // error code 3 if the service exists but is down.
    //
    // Check if /bin/systemctl exists and, if so, only use that. This has the
    // restriction where, on new systems, we'll only get the service name if the
    // service is defined in systemd.

    apr_status_t status;
    apr_finfo_t fileinfo;
    if (APR_SUCCESS != apr_stat(&fileinfo, "/bin/systemctl", 0, pool))
    {
        return -1;
    }

    // We don't have SCXProcess::Run to run things and capture the output, and error
    // codes from systemctl aren't good enough for us - we need to see the output.
    // The easiest way, without a ton of code, is to send systemctl output to a file,
    // read that, and then delete it. Ugh.

    // First let's get a temporary filename that we can use
    const char* tempDir;
    pid_t pid = getpid();

    if ( APR_SUCCESS != (status = apr_temp_dir_get(&tempDir, pool)) )
    {
        DisplayError(status, "Apache_HTTPDServer_Class_Provider::CheckSystemServiceD apr_temp_dir_get failed");
        return 2;
    }

    char *filename = apr_psprintf(pool, "%s/apache_cimprov_%d", tempDir, pid);

    // Now run the program and generate the result file
    // (don't care about result; we'll just look for file)
    char *command = apr_pstrcat(pool, "/bin/systemctl status ", serviceName, " > ", filename, NULL);

    system(command);

    /* When run, systemctl will return something like this:
     *
     *   httpd.service - The Apache HTTP Server
     *      Loaded: loaded (/usr/lib/systemd/system/httpd.service; enabled)
     *      Active: active (running) since Thu 2015-01-08 07:46:43 PST; 3h 18min ago
     *      <more stuff we don't care about>
     *
     * We only care about the second line, looking for "Loaded: loaded"
     *
     * Note: Once file is open, we must continue to close the file ...
     */
    apr_file_t* fh;
    char buffer[256];
    exitStatus = -1;

    if ( APR_SUCCESS != (status = apr_file_open(&fh, filename, APR_FOPEN_READ, 0, pool)) )
    {
        char *errorText = apr_pstrcat(pool, "Apache_HTTPDServer_Class_Provider::CheckSystemServiceD"
                                      "apr_file_open failed with filename: '", filename, "'", NULL);
        DisplayError(status, errorText);
        return 2;
    }

    if ( APR_SUCCESS != (status = apr_file_remove(filename, pool)) )
    {
        DisplayError(status, "Apache_HTTPDServer_Class_Provider::CheckSystemServiceD apr_file_remove failed");
        /* Unfortunate, but continue processing ... */
    }

    // We want the second line, so read twice
    for ( size_t i = 1; i <= 2; ++i )
    {
        if ( APR_SUCCESS != (status = apr_file_gets(buffer, sizeof(buffer), fh)) )
        {
            DisplayError(status, "Apache_HTTPDServer_Class_Provider::CheckSystemServiceD apr_file_gets failed");
            exitStatus = 2;
        }
    }

    // Okay, see if second line contains "<whitespace>Loaded: loaded"
    if ( NULL != strstr(buffer, "Loaded: loaded") )
    {
        /* Only set exitStatus if some failure did not occur already */
        if ( exitStatus < 0 )
            exitStatus = 0;
    }

    if ( APR_SUCCESS != (status = apr_file_close(fh)) )
    {
        DisplayError(status, "Apache_HTTPDServer_Class_Provider::CheckSystemServiceD apr_file_close failed");
        return 2;
    }

    return exitStatus;

#else

    return -1;

#endif // defined(linux)
}

Apache_HTTPDServer_Class_Provider::Apache_HTTPDServer_Class_Provider(
    Module* module) :
    m_Module(module)
{
}

Apache_HTTPDServer_Class_Provider::~Apache_HTTPDServer_Class_Provider()
{
}

void Apache_HTTPDServer_Class_Provider::Load(
        Context& context)
{
    CIM_PEX_BEGIN
    {
        if (NULL == g_pFactory)
        {
            g_pFactory = new ApacheFactory();
        }

        if (APR_SUCCESS != g_pFactory->GetInit()->Load("Server"))
        {
            context.Post(MI_RESULT_FAILED);
            return;
        }

        // Notify that we don't wish to unload
        MI_Result r = context.RefuseUnload();
        if ( MI_RESULT_OK != r )
        {
            DisplayError(OMI_Error(r), "Apache_HTTPDServer_Class_Provider refuses to not unload");
        }

        context.Post(MI_RESULT_OK);
    }
    CIM_PEX_END( "Apache_HTTPDServer_Class_Provider::Load" );
}

void Apache_HTTPDServer_Class_Provider::Unload(
        Context& context)
{
    CIM_PEX_BEGIN
    {
        if (APR_SUCCESS != g_pFactory->GetInit()->Unload("Server"))
        {
            context.Post(MI_RESULT_FAILED);
            return;
        }

        context.Post(MI_RESULT_OK);
    }
    CIM_PEX_END( "Apache_HTTPDServer_Class_Provider::Unload" );
}

void Apache_HTTPDServer_Class_Provider::EnumerateInstances(
    Context& context,
    const String& nameSpace,
    const PropertySet& propertySet,
    bool keysOnly,
    const MI_Filter* filter)
{
    static char s_ServerConfigFile[PATH_MAX];
    static char s_ServerVersion[96];

    CIM_PEX_BEGIN
    {
        ApacheDataCollector data = g_pFactory->DataCollectorFactory();
        apr_pool_t* pool = data.GetPool();
        Apache_HTTPDServer_Class inst;

        std::stringstream ss;
        const char* serviceName = "_Unknown";

        // Common code (regardless of if we attach to shared memory segment or not)

        if (! keysOnly)
        {
            // Build the version string
            ss << CIMPROV_BUILDVERSION_MAJOR
               << "." << CIMPROV_BUILDVERSION_MINOR
               << "." << CIMPROV_BUILDVERSION_PATCH
               << "-" << CIMPROV_BUILDVERSION_BUILDNR
               << " (" << CIMPROV_BUILDVERSION_DATE << ")";

#if defined(linux)
            int status;
            status = CheckServiceSystemd(pool, "httpd");
            if ( status >= 0 )
            {
                if ( 0 == status )
                {
                    serviceName = "httpd";
                }
                else
                {
                    if ( 0 == CheckServiceSystemd(pool, "apache2") )
                    {
                        serviceName = "apache2";
                    }
                }
            }
            else
            {
                status = system("service httpd status > /dev/null 2>&1");
                status = WEXITSTATUS( status );
                if (status == 0 || status == 3)
                {
                    serviceName = "httpd";
                }
                else
                {
                    status = system("service apache2 status > /dev/null 2>&1");
                    status = WEXITSTATUS( status );
                    if (status == 0 || status == 3)
                    {
                        serviceName = "apache2";
                    }
                }
            }
#endif
        }

        if (APR_SUCCESS == data.Attach("Apache_HTTPDServer_Class_Provider::EnumerateInstances"))
        {
            const char* apacheServerVersion = GetApacheComponentVersion(data.GetServerVersion(), "Apache");

            // Save values for reporting if unable to attach next time 'round
            // (WI 693191: Make Apache_HTTPDSeerver properties sticky)

            strncpy(s_ServerConfigFile, data.GetServerConfigFile(), sizeof(s_ServerConfigFile));
            strncpy(s_ServerVersion, apacheServerVersion, sizeof(s_ServerVersion));

            // Successfully attached to memory segment; provide normal results

            inst.ProductIdentifyingNumber_value("1");   /* serial number */
            inst.ProductName_value(data.GetServerConfigFile());
            inst.ProductVendor_value(APACHE_VENDOR_ID);
            inst.ProductVersion_value(apacheServerVersion);
            inst.SystemID_value(data.GetServerID());
            inst.CollectionID_value(data.GetServerRoot());

            if (! keysOnly)
            {
                std::string processName;
                g_pFactory->GetInit()->GetApacheProcessName(processName);

                // Insert the values into the instance

                inst.ModuleVersion_value(ss.str().c_str());
                inst.InstanceID_value(data.GetServerConfigFile());
                inst.ConfigurationFile_value(data.GetServerConfigFile());
                inst.ProcessName_value(processName.c_str());
                inst.ServiceName_value(serviceName);
                inst.OperatingStatus_value(OperatingStatusValues[2]); // Server us up

                std::vector<mi::String> strArrary;
                std::string modulesFormatted;
                for (apr_size_t moduleNum = 0; moduleNum < data.GetModuleCount(); moduleNum++)
                {
                    const char *moduleName = data.GetDataString(data.GetServerModules()[moduleNum].moduleNameOffset);
                    strArrary.push_back(moduleName);
                    if (modulesFormatted.size())
                    {
                        modulesFormatted += ", ";
                    }
                    modulesFormatted += moduleName;
                }
                mi::StringA modules(&strArrary[0], data.GetModuleCount());
                inst.InstalledModules_value(modules);
                inst.InstalledModulesFormatted_value(modulesFormatted.c_str());
            }
        }
        else
        {
            // We can't attach, so provide a minimal response indicating the server is down

            inst.ProductIdentifyingNumber_value("1");   /* serial number */
            inst.ProductName_value(s_ServerConfigFile[0] ? s_ServerConfigFile : "Unknown");
            inst.ProductVendor_value(APACHE_VENDOR_ID);
            inst.ProductVersion_value(s_ServerVersion[0] ? s_ServerVersion : "Unknown");
            inst.SystemID_value("Unknown");
            inst.CollectionID_value("Unknown");

            if (! keysOnly)
            {
                inst.ModuleVersion_value(ss.str().c_str());
                inst.ServiceName_value(serviceName);
                inst.OperatingStatus_value(OperatingStatusValues[6]); // Server state: Error

                inst.InstanceID_value(s_ServerConfigFile[0] ? s_ServerConfigFile : "Unknown");
            }
        }

        context.Post(inst);
        context.Post(MI_RESULT_OK);
    }
    CIM_PEX_END( "Apache_HTTPDServer_Class_Provider::EnumerateInstances" );
}

void Apache_HTTPDServer_Class_Provider::GetInstance(
    Context& context,
    const String& nameSpace,
    const Apache_HTTPDServer_Class& instanceName,
    const PropertySet& propertySet)
{
    context.Post(MI_RESULT_NOT_SUPPORTED);
}

void Apache_HTTPDServer_Class_Provider::CreateInstance(
    Context& context,
    const String& nameSpace,
    const Apache_HTTPDServer_Class& newInstance)
{
    context.Post(MI_RESULT_NOT_SUPPORTED);
}

void Apache_HTTPDServer_Class_Provider::ModifyInstance(
    Context& context,
    const String& nameSpace,
    const Apache_HTTPDServer_Class& modifiedInstance,
    const PropertySet& propertySet)
{
    context.Post(MI_RESULT_NOT_SUPPORTED);
}

void Apache_HTTPDServer_Class_Provider::DeleteInstance(
    Context& context,
    const String& nameSpace,
    const Apache_HTTPDServer_Class& instanceName)
{
    context.Post(MI_RESULT_NOT_SUPPORTED);
}


MI_END_NAMESPACE
