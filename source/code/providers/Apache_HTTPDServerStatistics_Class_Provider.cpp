/* @migen@ */
#include <MI.h>
#include "Apache_HTTPDServerStatistics_Class_Provider.h"

// Provider include definitions
#include <apr_atomic.h>
#include "apachebinding.h"

#include <unistd.h>

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

    // Insert the key into the instance

    inst.InstanceID_value(g_apache.GetServerConfigFile());

    if (! keysOnly)
    {
        // Insert the values into the instance

        inst.ConfigurationFile_value(g_apache.GetServerConfigFile());

        // Insert time-based values into the instance

        inst.TotalPctCPU_value(g_apache.GetCPUUtilization());

        apr_uint32_t idleWorkers = g_apache.GetWorkerCountIdle();
        apr_uint32_t busyWorkers = g_apache.GetWorkerCountBusy();
        apr_uint32_t totalWorkers = idleWorkers + busyWorkers;

        inst.IdleWorkers_value(idleWorkers);
        inst.BusyWorkers_value(busyWorkers);
        inst.PctBusyWorkers_value(totalWorkers ? (busyWorkers * 100) / totalWorkers : 0);
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
