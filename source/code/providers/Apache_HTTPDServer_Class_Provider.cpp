/* @migen@ */
#include <MI.h>
#include "Apache_HTTPDServer_Class_Provider.h"

// Provider include definitions
#include <stdlib.h>
#include <sstream>
#include <vector>
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
        if (APR_SUCCESS != g_apache.Load("Server"))
        {
            context.Post(MI_RESULT_FAILED);
            return;
        }

        // Notify that we don't wish to unload
        MI_Result r = context.RefuseUnload();
        if ( MI_RESULT_OK != r )
        {
            g_apache.DisplayError(g_apache.OMI_Error(r), "Apache_HTTPDServer_Class_Provider refuses to not unload");
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
        if (APR_SUCCESS != g_apache.Unload("VirtualHostStatistics"))
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
    CIM_PEX_BEGIN
    {
        Apache_HTTPDServer_Class inst;
        const char* apacheServerVersion = GetApacheComponentVersion(g_apache.GetServerVersion(), "Apache");

        if (! keysOnly)
        {
            // Build the version string
            std::stringstream ss;
            ss << CIMPROV_BUILDVERSION_MAJOR
               << "." << CIMPROV_BUILDVERSION_MINOR
               << "." << CIMPROV_BUILDVERSION_PATCH
               << "-" << CIMPROV_BUILDVERSION_BUILDNR
               << " (" << CIMPROV_BUILDVERSION_DATE << ")";
            const char* serviceName = "_Unknown";
#if defined(linux)
            int status = system("service httpd status");
            if (status == 0 || status == 3)
            {
                serviceName = "httpd";
            }
            else
            {
                status = system("service apache2 status");
                if (status == 0 || status == 3)
                {
                    serviceName = "apache2";
                }
            }
#endif

            // Insert the values into the instance

            inst.ProductIdentifyingNumber_value("1");   /* serial number */
            inst.ProductName_value(g_apache.GetServerConfigFile());
            inst.ProductVendor_value(APACHE_VENDOR_ID);
            inst.ProductVersion_value(apacheServerVersion);
            inst.SystemID_value(g_apache.GetServerID());
            inst.CollectionID_value(g_apache.GetServerRoot());
            inst.ModuleVersion_value(ss.str().c_str());
            inst.InstanceID_value(g_apache.GetServerConfigFile());
            inst.ConfigurationFile_value(g_apache.GetServerConfigFile());
            inst.ProcessName_value(g_apache.GetServerProcessName());
            inst.ServiceName_value(serviceName);
            inst.OperatingStatus_value(OperatingStatusValues[g_apache.GetOperatingStatus()]);

            std::vector<mi::String> strArrary;
            for (apr_size_t moduleNum = 0; moduleNum < g_apache.GetModuleCount(); moduleNum++)
            {
                strArrary.push_back(g_apache.GetDataString(g_apache.GetServerModules()[moduleNum].moduleNameOffset));
            }
            mi::StringA modules(&strArrary[0], g_apache.GetModuleCount());
            inst.InstalledModules_value(modules);
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
