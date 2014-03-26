/* @migen@ */
#include <MI.h>
#include "Apache_HTTPDServerStatistics_Class_Provider.h"

// Provider include definitions
#include "apachebinding.h"


MI_BEGIN_NAMESPACE

Apache_HTTPDServerStatistics_Class_Provider::Apache_HTTPDServerStatistics_Class_Provider(
    Module* module) :
    m_Module(module)
{
}

Apache_HTTPDServerStatistics_Class_Provider::~Apache_HTTPDServerStatistics_Class_Provider()
{
}

void Apache_HTTPDServerStatistics_Class_Provider::Load(
        Context& context)
{
    if (APR_SUCCESS != g_apache.Load("ServerStatistics"))
    {
        context.Post(MI_RESULT_FAILED);
        return;
    }

    // Notify that we don't wish to unload
    MI_Result r = context.RefuseUnload();
    if ( MI_RESULT_OK != r )
    {
        g_apache.DisplayError(g_apache.OMI_Error(r), "Apache_HTTPDServerStatistics_Class_Provider refuses to not unload");
    }

    context.Post(MI_RESULT_OK);
}

void Apache_HTTPDServerStatistics_Class_Provider::Unload(
        Context& context)
{
    if (APR_SUCCESS != g_apache.Unload("VirtualHostStatistics"))
    {
        context.Post(MI_RESULT_FAILED);
        return;
    }

    context.Post(MI_RESULT_OK);
}

void Apache_HTTPDServerStatistics_Class_Provider::EnumerateInstances(
    Context& context,
    const String& nameSpace,
    const PropertySet& propertySet,
    bool keysOnly,
    const MI_Filter* filter)
{
    Apache_HTTPDServerStatistics_Class inst;
    mmap_vhost_elements *vhosts = g_apache.GetVHostElements();

    // Insert the key into the instance

    inst.InstanceID_value(g_apache.GetServerConfigFile());

    if (! keysOnly)
    {
        // Insert the values into the instance

        inst.RequestsTotal_value(vhosts[0].requestsTotal);
        inst.IdleWorkers_value(g_apache.GetWorkerCountIdle());
        inst.BusyWorkers_value(g_apache.GetWorkerCountBusy());
        inst.ConfigurationFile_value(g_apache.GetServerConfigFile());
    }

    context.Post(inst);
    context.Post(MI_RESULT_OK);
}

void Apache_HTTPDServerStatistics_Class_Provider::GetInstance(
    Context& context,
    const String& nameSpace,
    const Apache_HTTPDServerStatistics_Class& instanceName,
    const PropertySet& propertySet)
{
    context.Post(MI_RESULT_NOT_SUPPORTED);
}

void Apache_HTTPDServerStatistics_Class_Provider::CreateInstance(
    Context& context,
    const String& nameSpace,
    const Apache_HTTPDServerStatistics_Class& newInstance)
{
    context.Post(MI_RESULT_NOT_SUPPORTED);
}

void Apache_HTTPDServerStatistics_Class_Provider::ModifyInstance(
    Context& context,
    const String& nameSpace,
    const Apache_HTTPDServerStatistics_Class& modifiedInstance,
    const PropertySet& propertySet)
{
    context.Post(MI_RESULT_NOT_SUPPORTED);
}

void Apache_HTTPDServerStatistics_Class_Provider::DeleteInstance(
    Context& context,
    const String& nameSpace,
    const Apache_HTTPDServerStatistics_Class& instanceName)
{
    context.Post(MI_RESULT_NOT_SUPPORTED);
}

void Apache_HTTPDServerStatistics_Class_Provider::Invoke_ResetSelectedStats(
    Context& context,
    const String& nameSpace,
    const Apache_HTTPDServerStatistics_Class& instanceName,
    const Apache_HTTPDServerStatistics_ResetSelectedStats_Class& in)
{
    context.Post(MI_RESULT_NOT_SUPPORTED);
}


MI_END_NAMESPACE
