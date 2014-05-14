/* @migen@ */
#include <MI.h>
#include "Apache_HTTPDVirtualHostStatistics_Class_Provider.h"

// Provider include definitions
#include <apr_atomic.h>
#include "apachebinding.h"


MI_BEGIN_NAMESPACE

static void EnumerateOneInstance(Context& context,
        bool keysOnly,
        apr_size_t item)
{
    Apache_HTTPDVirtualHostStatistics_Class inst;
    mmap_vhost_elements *vhosts = g_apache.GetVHostElements();

    // Insert the key into the instance
    inst.InstanceID_value(vhosts[item].instanceID);

    if (! keysOnly)
    {
        // Insert the values into the instance

        inst.ServerName_value(vhosts[item].name);
        inst.RequestsTotal_value(vhosts[item].requestsTotal);
        inst.RequestsTotalBytes_value(vhosts[item].requestsBytes);
        inst.ErrorCount400_value(vhosts[item].errorCount400);
        inst.ErrorCount500_value(vhosts[item].errorCount500);

        // Insert the time-based values into the instance

        inst.RequestsPerSecond_value(apr_atomic_read32(&vhosts[item].requestsPerSecond));
        inst.KBPerRequest_value(apr_atomic_read32(&vhosts[item].kbPerRequest));
        inst.KBPerSecond_value(apr_atomic_read32(&vhosts[item].kbPerSecond));
        inst.ErrorsPerMinute400_value(apr_atomic_read32(&vhosts[item].errorsPerMinute400));
        inst.ErrorsPerMinute500_value(apr_atomic_read32(&vhosts[item].errorsPerMinute500));
    }

    context.Post(inst);
}

Apache_HTTPDVirtualHostStatistics_Class_Provider::Apache_HTTPDVirtualHostStatistics_Class_Provider(
    Module* module) :
    m_Module(module)
{
}

Apache_HTTPDVirtualHostStatistics_Class_Provider::~Apache_HTTPDVirtualHostStatistics_Class_Provider()
{
}

void Apache_HTTPDVirtualHostStatistics_Class_Provider::Load(
        Context& context)
{
    if (APR_SUCCESS != g_apache.Load("VirtualHostStatistics"))
    {
        context.Post(MI_RESULT_FAILED);
        return;
    }

    // Notify that we don't wish to unload
    MI_Result r = context.RefuseUnload();
    if ( MI_RESULT_OK != r )
    {
        g_apache.DisplayError(g_apache.OMI_Error(r), "Apache_HTTPDVirtualHostStatistics_Class_Provider refuses to not unload");
    }

    context.Post(MI_RESULT_OK);
}

void Apache_HTTPDVirtualHostStatistics_Class_Provider::Unload(
        Context& context)
{
    if (APR_SUCCESS != g_apache.Unload("VirtualHostStatistics"))
    {
        context.Post(MI_RESULT_FAILED);
        return;
    }

    context.Post(MI_RESULT_OK);
}

void Apache_HTTPDVirtualHostStatistics_Class_Provider::EnumerateInstances(
    Context& context,
    const String& nameSpace,
    const PropertySet& propertySet,
    bool keysOnly,
    const MI_Filter* filter)
{
    apr_status_t status;
    apr_size_t i;

    /* Lock the mutex to walk the list */
    if (APR_SUCCESS != (status = g_apache.LockMutex()))
    {
        g_apache.DisplayError(status, "VirtualHostStatistics::EnumerateInstances: failed to lock mutex");
        context.Post(MI_RESULT_FAILED);
        return;
    }

    for (i = 2; i <= g_apache.GetVHostCount() - 1; i++)
    {
        EnumerateOneInstance(context, keysOnly, i);
    }

    // Support _Default and _Total
    EnumerateOneInstance(context, keysOnly, 1);
    EnumerateOneInstance(context, keysOnly, 0);

    g_apache.UnlockMutex();
    context.Post(MI_RESULT_OK);
}

void Apache_HTTPDVirtualHostStatistics_Class_Provider::GetInstance(
    Context& context,
    const String& nameSpace,
    const Apache_HTTPDVirtualHostStatistics_Class& instanceName,
    const PropertySet& propertySet)
{
    context.Post(MI_RESULT_NOT_SUPPORTED);
}

void Apache_HTTPDVirtualHostStatistics_Class_Provider::CreateInstance(
    Context& context,
    const String& nameSpace,
    const Apache_HTTPDVirtualHostStatistics_Class& newInstance)
{
    context.Post(MI_RESULT_NOT_SUPPORTED);
}

void Apache_HTTPDVirtualHostStatistics_Class_Provider::ModifyInstance(
    Context& context,
    const String& nameSpace,
    const Apache_HTTPDVirtualHostStatistics_Class& modifiedInstance,
    const PropertySet& propertySet)
{
    context.Post(MI_RESULT_NOT_SUPPORTED);
}

void Apache_HTTPDVirtualHostStatistics_Class_Provider::DeleteInstance(
    Context& context,
    const String& nameSpace,
    const Apache_HTTPDVirtualHostStatistics_Class& instanceName)
{
    context.Post(MI_RESULT_NOT_SUPPORTED);
}

void Apache_HTTPDVirtualHostStatistics_Class_Provider::Invoke_ResetSelectedStats(
    Context& context,
    const String& nameSpace,
    const Apache_HTTPDVirtualHostStatistics_Class& instanceName,
    const Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_Class& in)
{
    context.Post(MI_RESULT_NOT_SUPPORTED);
}


MI_END_NAMESPACE
