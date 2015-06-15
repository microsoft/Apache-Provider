/* @migen@ */

//
//--------------------------------- START OF LICENSE ----------------------------
//
// Apache Cimprov ver. 1.0
//
// Copyright (c) Microsoft Corporation
//
// All rights reserved. 
//
// Licensed under the Apache License, Version 2.0 (the License); you may not use
// this file except in compliance with the license. You may obtain a copy of the
// License at http://www.apache.org/licenses/LICENSE-2.0 
//
// THIS CODE IS PROVIDED ON AN *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED
// WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE,
// MERCHANTABLITY OR NON-INFRINGEMENT.
//
// See the Apache Version 2.0 License for specific language governing permissions
// and limitations under the License.
//
//---------------------------------- END OF LICENSE -----------------------------
//

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
    CIM_PEX_BEGIN
    {
        if (NULL == g_pFactory)
        {
            g_pFactory = new ApacheFactory();
        }

        if (APR_SUCCESS != g_pFactory->GetInit()->Load("ServerStatistics"))
        {
            context.Post(MI_RESULT_FAILED);
            return;
        }

        // Notify that we don't wish to unload
        MI_Result r = context.RefuseUnload();
        if ( MI_RESULT_OK != r )
        {
            DisplayError(OMI_Error(r), "Apache_HTTPDServerStatistics_Class_Provider refuses to not unload");
        }

        context.Post(MI_RESULT_OK);
    }
    CIM_PEX_END( "Apache_HTTPDServerStatistics_Class_Provider::Load" );
}

void Apache_HTTPDServerStatistics_Class_Provider::Unload(
        Context& context)
{
    CIM_PEX_BEGIN
    {
        if (APR_SUCCESS != g_pFactory->GetInit()->Unload("ServerStatistics"))
        {
            context.Post(MI_RESULT_FAILED);
            return;
        }

        context.Post(MI_RESULT_OK);
    }
    CIM_PEX_END( "Apache_HTTPDServerStatistics_Class_Provider::Unload" );
}

void Apache_HTTPDServerStatistics_Class_Provider::EnumerateInstances(
    Context& context,
    const String& nameSpace,
    const PropertySet& propertySet,
    bool keysOnly,
    const MI_Filter* filter)
{
    CIM_PEX_BEGIN
    {
        ApacheDataCollector data = g_pFactory->DataCollectorFactory();
        Apache_HTTPDServerStatistics_Class inst;

        if (APR_SUCCESS != data.Attach("Apache_HTTPDServerStatistics_Class_Provider::EnumerateInstances"))
        {
            context.Post(MI_RESULT_FAILED);
            return;
        }

        // Insert the key into the instance

        inst.InstanceID_value(data.GetServerConfigFile());

        if (! keysOnly)
        {
            // Insert the values into the instance

            inst.ConfigurationFile_value(data.GetServerConfigFile());

            // Insert time-based values into the instance

            inst.TotalPctCPU_value(data.GetCPUUtilization());

            apr_uint32_t idleWorkers = data.GetWorkerCountIdle();
            apr_uint32_t busyWorkers = data.GetWorkerCountBusy();
            apr_uint32_t totalWorkers = idleWorkers + busyWorkers;

            inst.IdleWorkers_value(idleWorkers);
            inst.BusyWorkers_value(busyWorkers);
            inst.PctBusyWorkers_value(totalWorkers ? (busyWorkers * 100) / totalWorkers : 0);
        }

        context.Post(inst);
        context.Post(MI_RESULT_OK);
    }
    CIM_PEX_END( "Apache_HTTPDServerStatistics_Class_Provider::EnumerateInstances" );
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
