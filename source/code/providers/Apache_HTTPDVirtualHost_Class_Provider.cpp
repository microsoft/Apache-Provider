/* @migen@ */
#include <MI.h>
#include "Apache_HTTPDVirtualHost_Class_Provider.h"

#include <string>
#include <vector>

// Apache Portable Runtime definitions
#include "apr.h"
#include "apr_errno.h"
#include "apr_global_mutex.h"
#include "apr_shm.h"

// Virtual host memory mapped file definitions
#include "apachebinding.h"
#include "cimconstants.h"
#include "utils.h"

// Convert a string encoded in this project's base-64 encoding into a 16-bit integer
static apr_uint16_t decode64(const char* str)
{
    return (((apr_uint16_t)(unsigned char)*str       - 0x0030) << 12) |
           (((apr_uint16_t)(unsigned char)*(str + 1) - 0x0030) <<  6) |
            ((apr_uint16_t)(unsigned char)*(str + 2) - 0x0030);
}

MI_BEGIN_NAMESPACE

static void EnumerateOneInstance(Context& context,
        bool keysOnly,
        apr_size_t item,
        ApacheDataCollector& data)
{
    Apache_HTTPDVirtualHost_Class inst;
    mmap_vhost_elements *vhosts = data.GetVHostElements();
    const char* apacheServerVersion = GetApacheComponentVersion(data.GetServerVersion(), "Apache");

    // Insert the key properties into the instance
    inst.Name_value(data.GetDataString(vhosts[item].instanceIDOffset));
    inst.Version_value(apacheServerVersion);
    inst.SoftwareElementID_value(data.GetDataString(vhosts[item].instanceIDOffset));
    inst.TargetOperatingSystem_value(CIM_TARGET_OPERATING_SYSTEM);
    inst.SoftwareElementState_value(CIM_SOFTWARE_ELEMENT_STATE_RUNNING);

    // Insert the instance ID into the instance

    inst.InstanceID_value(data.GetDataString(vhosts[item].instanceIDOffset));

    if (! keysOnly)
    {
        // Get the IP addresses and ports from the array of (IP address, port) items
        // corresponding to this host

        std::vector<mi::String> ipAddressesArray;
        std::vector<mi::Uint16> portsArray;
        std::vector<mi::String> aliasesArray;

        std::string ipAddressesFormatted;
        std::string portsFormatted;
        std::string aliasesFormatted;

        const char* ptr = data.GetDataString(vhosts[item].addressesAndPortsOffset);
        while (*ptr != '\0')
        {
            ipAddressesArray.push_back(ptr);
            if (ipAddressesFormatted.size())
            {
                ipAddressesFormatted += ", ";
            }
            ipAddressesFormatted += ptr;
            ptr += strlen(ptr) + 1;

            portsArray.push_back(decode64(ptr));
            if (portsFormatted.size())
            {
                portsFormatted += ", ";
            }
            portsFormatted += apr_itoa(data.GetPool(), decode64(ptr));
            ptr += strlen(ptr) + 1;
        }

        ptr = data.GetDataString(vhosts[item].serverAliasesOffset);
        while (*ptr != '\0')
        {
            aliasesArray.push_back(ptr);
            if (aliasesFormatted.size())
            {
                aliasesFormatted += ", ";
            }
            aliasesFormatted += ptr;
            ptr += strlen(ptr) + 1;
        }

        // Insert the values into the instance

        inst.ServerName_value(data.GetDataString(vhosts[item].hostNameOffset));
        inst.SoftwareElementState_value(CIM_SOFTWARE_ELEMENT_STATE_RUNNING);
        inst.SoftwareElementID_value(data.GetDataString(vhosts[item].instanceIDOffset));
        inst.TargetOperatingSystem_value(CIM_TARGET_OPERATING_SYSTEM);

        inst.DocumentRoot_value(data.GetDataString(vhosts[item].documentRootOffset));
        inst.ServerAdmin_value(data.GetDataString(vhosts[item].serverAdminOffset));
        inst.ErrorLog_value(data.GetDataString(vhosts[item].logErrorOffset));
        inst.CustomLog_value(data.GetDataString(vhosts[item].logCustomOffset));
        inst.AccessLog_value(data.GetDataString(vhosts[item].logAccessOffset));

        mi::StringA ipAddressesA(&ipAddressesArray[0], (MI_Uint32)ipAddressesArray.size());
        inst.IPAddresses_value(ipAddressesA);
        inst.IPAddressesFormatted_value(ipAddressesFormatted.c_str());

        mi::Uint16A portsA(&portsArray[0], (MI_Uint32)portsArray.size());
        inst.Ports_value(portsA);
        inst.PortsFormatted_value(portsFormatted.c_str());

        mi::StringA aliasesA(&aliasesArray[0], (MI_Uint32)aliasesArray.size());
        inst.ServerAlias_value(aliasesA);
        inst.ServerAliasFormatted_value(aliasesFormatted.c_str());
    }

    context.Post(inst);
}

Apache_HTTPDVirtualHost_Class_Provider::Apache_HTTPDVirtualHost_Class_Provider(
    Module* module) :
    m_Module(module)
{
}

Apache_HTTPDVirtualHost_Class_Provider::~Apache_HTTPDVirtualHost_Class_Provider()
{
}

void Apache_HTTPDVirtualHost_Class_Provider::Load(
        Context& context)
{
    CIM_PEX_BEGIN
    {
        if (NULL == g_pFactory)
        {
            g_pFactory = new ApacheFactory();
        }

        if (APR_SUCCESS != g_pFactory->GetInit()->Load("VirtualHost"))
        {
            context.Post(MI_RESULT_FAILED);
            return;
        }

        // Notify that we don't wish to unload
        MI_Result r = context.RefuseUnload();
        if ( MI_RESULT_OK != r )
        {
            DisplayError(OMI_Error(r), "Apache_HTTPDVirtualHost_Class_Provider refuses to not unload");
        }

        context.Post(MI_RESULT_OK);
    }
    CIM_PEX_END( "Apache_HTTPDVirtualHost_Class_Provider::Load" );
}

void Apache_HTTPDVirtualHost_Class_Provider::Unload(
        Context& context)
{
    CIM_PEX_BEGIN
    {
        if (APR_SUCCESS != g_pFactory->GetInit()->Unload("VirtualHost"))
        {
            context.Post(MI_RESULT_FAILED);
            return;
        }

        context.Post(MI_RESULT_OK);
    }
    CIM_PEX_END( "Apache_HTTPDVirtualHost_Class_Provider::Load" );
}

void Apache_HTTPDVirtualHost_Class_Provider::EnumerateInstances(
    Context& context,
    const String& nameSpace,
    const PropertySet& propertySet,
    bool keysOnly,
    const MI_Filter* filter)
{
    ApacheDataCollector data = g_pFactory->DataCollectorFactory();

    CIM_PEX_BEGIN
    {
        Apache_HTTPDVirtualHost_Class inst;
        apr_status_t status;

        if (APR_SUCCESS != data.Attach("Apache_HTTPDVirtualHost_Class_Provider::EnumerateInstances"))
        {
            context.Post(MI_RESULT_FAILED);
            return;
        }

        /* Lock the mutex to walk the list */
        if (APR_SUCCESS != (status = data.LockMutex()))
        {
            DisplayError(status, "VirtualHost::EnumerateInstances: failed to lock mutex");
            context.Post(MI_RESULT_FAILED);
            return;
        }

        for (apr_size_t i = 2; i <= data.GetVHostCount() - 1; i++)
        {
            EnumerateOneInstance(context, keysOnly, i, data);
        }

        // Only display _Unknown if data is saved to it
        if (data.GetVHostElements()[1].requestsTotal)
        {
            EnumerateOneInstance(context, keysOnly, 1, data);
        }

        context.Post(MI_RESULT_OK);
    }
    CIM_PEX_END( "Apache_HTTPDVirtualHost_Class_Provider::EnumerateInstances" );

    // Be sure mutex gets unlocked, regardless if an exception occurs
    data.UnlockMutex();
}

void Apache_HTTPDVirtualHost_Class_Provider::GetInstance(
    Context& context,
    const String& nameSpace,
    const Apache_HTTPDVirtualHost_Class& instanceName,
    const PropertySet& propertySet)
{
    context.Post(MI_RESULT_NOT_SUPPORTED);
}

void Apache_HTTPDVirtualHost_Class_Provider::CreateInstance(
    Context& context,
    const String& nameSpace,
    const Apache_HTTPDVirtualHost_Class& newInstance)
{
    context.Post(MI_RESULT_NOT_SUPPORTED);
}

void Apache_HTTPDVirtualHost_Class_Provider::ModifyInstance(
    Context& context,
    const String& nameSpace,
    const Apache_HTTPDVirtualHost_Class& modifiedInstance,
    const PropertySet& propertySet)
{
    context.Post(MI_RESULT_NOT_SUPPORTED);
}

void Apache_HTTPDVirtualHost_Class_Provider::DeleteInstance(
    Context& context,
    const String& nameSpace,
    const Apache_HTTPDVirtualHost_Class& instanceName)
{
    context.Post(MI_RESULT_NOT_SUPPORTED);
}


MI_END_NAMESPACE
