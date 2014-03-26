/* @migen@ */
/*
**==============================================================================
**
** WARNING: THIS FILE WAS AUTOMATICALLY GENERATED. PLEASE DO NOT EDIT.
**
**==============================================================================
*/
#include <MI.h>
#include "module.h"
#include "Apache_HTTPDProcess_Class_Provider.h"
#include "Apache_HTTPDServer_Class_Provider.h"
#include "Apache_HTTPDServerStatistics_Class_Provider.h"
#include "Apache_HTTPDVirtualHost_Class_Provider.h"
#include "Apache_HTTPDVirtualHostCertificate_Class_Provider.h"
#include "Apache_HTTPDVirtualHostStatistics_Class_Provider.h"

using namespace mi;

MI_EXTERN_C void MI_CALL Apache_HTTPDProcess_Load(
    Apache_HTTPDProcess_Self** self,
    MI_Module_Self* selfModule,
    MI_Context* context)
{
    MI_Result r = MI_RESULT_OK;
    Context ctx(context, &r);
    Apache_HTTPDProcess_Class_Provider* prov = new Apache_HTTPDProcess_Class_Provider((Module*)selfModule);

    prov->Load(ctx);
    if (MI_RESULT_OK != r)
    {
        delete prov;
        MI_PostResult(context, r);
        return;
    }
    *self = (Apache_HTTPDProcess_Self*)prov;
    MI_PostResult(context, MI_RESULT_OK);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDProcess_Unload(
    Apache_HTTPDProcess_Self* self,
    MI_Context* context)
{
    MI_Result r = MI_RESULT_OK;
    Context ctx(context, &r);
    Apache_HTTPDProcess_Class_Provider* prov = (Apache_HTTPDProcess_Class_Provider*)self;

    prov->Unload(ctx);
    delete ((Apache_HTTPDProcess_Class_Provider*)self);
    MI_PostResult(context, r);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDProcess_EnumerateInstances(
    Apache_HTTPDProcess_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const MI_PropertySet* propertySet,
    MI_Boolean keysOnly,
    const MI_Filter* filter)
{
    Apache_HTTPDProcess_Class_Provider* cxxSelf =((Apache_HTTPDProcess_Class_Provider*)self);
    Context  cxxContext(context);

    cxxSelf->EnumerateInstances(
        cxxContext,
        nameSpace,
        __PropertySet(propertySet),
        __bool(keysOnly),
        filter);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDProcess_GetInstance(
    Apache_HTTPDProcess_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDProcess* instanceName,
    const MI_PropertySet* propertySet)
{
    Apache_HTTPDProcess_Class_Provider* cxxSelf =((Apache_HTTPDProcess_Class_Provider*)self);
    Context  cxxContext(context);
    Apache_HTTPDProcess_Class cxxInstanceName(instanceName, true);

    cxxSelf->GetInstance(
        cxxContext,
        nameSpace,
        cxxInstanceName,
        __PropertySet(propertySet));
}

MI_EXTERN_C void MI_CALL Apache_HTTPDProcess_CreateInstance(
    Apache_HTTPDProcess_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDProcess* newInstance)
{
    Apache_HTTPDProcess_Class_Provider* cxxSelf =((Apache_HTTPDProcess_Class_Provider*)self);
    Context  cxxContext(context);
    Apache_HTTPDProcess_Class cxxNewInstance(newInstance, false);

    cxxSelf->CreateInstance(cxxContext, nameSpace, cxxNewInstance);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDProcess_ModifyInstance(
    Apache_HTTPDProcess_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDProcess* modifiedInstance,
    const MI_PropertySet* propertySet)
{
    Apache_HTTPDProcess_Class_Provider* cxxSelf =((Apache_HTTPDProcess_Class_Provider*)self);
    Context  cxxContext(context);
    Apache_HTTPDProcess_Class cxxModifiedInstance(modifiedInstance, false);

    cxxSelf->ModifyInstance(
        cxxContext,
        nameSpace,
        cxxModifiedInstance,
        __PropertySet(propertySet));
}

MI_EXTERN_C void MI_CALL Apache_HTTPDProcess_DeleteInstance(
    Apache_HTTPDProcess_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDProcess* instanceName)
{
    Apache_HTTPDProcess_Class_Provider* cxxSelf =((Apache_HTTPDProcess_Class_Provider*)self);
    Context  cxxContext(context);
    Apache_HTTPDProcess_Class cxxInstanceName(instanceName, true);

    cxxSelf->DeleteInstance(cxxContext, nameSpace, cxxInstanceName);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDProcess_Invoke_RequestStateChange(
    Apache_HTTPDProcess_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const MI_Char* methodName,
    const Apache_HTTPDProcess* instanceName,
    const Apache_HTTPDProcess_RequestStateChange* in)
{
    Apache_HTTPDProcess_Class_Provider* cxxSelf =((Apache_HTTPDProcess_Class_Provider*)self);
    Apache_HTTPDProcess_Class instance(instanceName, false);
    Context  cxxContext(context);
    Apache_HTTPDProcess_RequestStateChange_Class param(in, false);

    cxxSelf->Invoke_RequestStateChange(cxxContext, nameSpace, instance, param);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDServer_Load(
    Apache_HTTPDServer_Self** self,
    MI_Module_Self* selfModule,
    MI_Context* context)
{
    MI_Result r = MI_RESULT_OK;
    Context ctx(context, &r);
    Apache_HTTPDServer_Class_Provider* prov = new Apache_HTTPDServer_Class_Provider((Module*)selfModule);

    prov->Load(ctx);
    if (MI_RESULT_OK != r)
    {
        delete prov;
        MI_PostResult(context, r);
        return;
    }
    *self = (Apache_HTTPDServer_Self*)prov;
    MI_PostResult(context, MI_RESULT_OK);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDServer_Unload(
    Apache_HTTPDServer_Self* self,
    MI_Context* context)
{
    MI_Result r = MI_RESULT_OK;
    Context ctx(context, &r);
    Apache_HTTPDServer_Class_Provider* prov = (Apache_HTTPDServer_Class_Provider*)self;

    prov->Unload(ctx);
    delete ((Apache_HTTPDServer_Class_Provider*)self);
    MI_PostResult(context, r);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDServer_EnumerateInstances(
    Apache_HTTPDServer_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const MI_PropertySet* propertySet,
    MI_Boolean keysOnly,
    const MI_Filter* filter)
{
    Apache_HTTPDServer_Class_Provider* cxxSelf =((Apache_HTTPDServer_Class_Provider*)self);
    Context  cxxContext(context);

    cxxSelf->EnumerateInstances(
        cxxContext,
        nameSpace,
        __PropertySet(propertySet),
        __bool(keysOnly),
        filter);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDServer_GetInstance(
    Apache_HTTPDServer_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDServer* instanceName,
    const MI_PropertySet* propertySet)
{
    Apache_HTTPDServer_Class_Provider* cxxSelf =((Apache_HTTPDServer_Class_Provider*)self);
    Context  cxxContext(context);
    Apache_HTTPDServer_Class cxxInstanceName(instanceName, true);

    cxxSelf->GetInstance(
        cxxContext,
        nameSpace,
        cxxInstanceName,
        __PropertySet(propertySet));
}

MI_EXTERN_C void MI_CALL Apache_HTTPDServer_CreateInstance(
    Apache_HTTPDServer_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDServer* newInstance)
{
    Apache_HTTPDServer_Class_Provider* cxxSelf =((Apache_HTTPDServer_Class_Provider*)self);
    Context  cxxContext(context);
    Apache_HTTPDServer_Class cxxNewInstance(newInstance, false);

    cxxSelf->CreateInstance(cxxContext, nameSpace, cxxNewInstance);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDServer_ModifyInstance(
    Apache_HTTPDServer_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDServer* modifiedInstance,
    const MI_PropertySet* propertySet)
{
    Apache_HTTPDServer_Class_Provider* cxxSelf =((Apache_HTTPDServer_Class_Provider*)self);
    Context  cxxContext(context);
    Apache_HTTPDServer_Class cxxModifiedInstance(modifiedInstance, false);

    cxxSelf->ModifyInstance(
        cxxContext,
        nameSpace,
        cxxModifiedInstance,
        __PropertySet(propertySet));
}

MI_EXTERN_C void MI_CALL Apache_HTTPDServer_DeleteInstance(
    Apache_HTTPDServer_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDServer* instanceName)
{
    Apache_HTTPDServer_Class_Provider* cxxSelf =((Apache_HTTPDServer_Class_Provider*)self);
    Context  cxxContext(context);
    Apache_HTTPDServer_Class cxxInstanceName(instanceName, true);

    cxxSelf->DeleteInstance(cxxContext, nameSpace, cxxInstanceName);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDServerStatistics_Load(
    Apache_HTTPDServerStatistics_Self** self,
    MI_Module_Self* selfModule,
    MI_Context* context)
{
    MI_Result r = MI_RESULT_OK;
    Context ctx(context, &r);
    Apache_HTTPDServerStatistics_Class_Provider* prov = new Apache_HTTPDServerStatistics_Class_Provider((Module*)selfModule);

    prov->Load(ctx);
    if (MI_RESULT_OK != r)
    {
        delete prov;
        MI_PostResult(context, r);
        return;
    }
    *self = (Apache_HTTPDServerStatistics_Self*)prov;
    MI_PostResult(context, MI_RESULT_OK);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDServerStatistics_Unload(
    Apache_HTTPDServerStatistics_Self* self,
    MI_Context* context)
{
    MI_Result r = MI_RESULT_OK;
    Context ctx(context, &r);
    Apache_HTTPDServerStatistics_Class_Provider* prov = (Apache_HTTPDServerStatistics_Class_Provider*)self;

    prov->Unload(ctx);
    delete ((Apache_HTTPDServerStatistics_Class_Provider*)self);
    MI_PostResult(context, r);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDServerStatistics_EnumerateInstances(
    Apache_HTTPDServerStatistics_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const MI_PropertySet* propertySet,
    MI_Boolean keysOnly,
    const MI_Filter* filter)
{
    Apache_HTTPDServerStatistics_Class_Provider* cxxSelf =((Apache_HTTPDServerStatistics_Class_Provider*)self);
    Context  cxxContext(context);

    cxxSelf->EnumerateInstances(
        cxxContext,
        nameSpace,
        __PropertySet(propertySet),
        __bool(keysOnly),
        filter);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDServerStatistics_GetInstance(
    Apache_HTTPDServerStatistics_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDServerStatistics* instanceName,
    const MI_PropertySet* propertySet)
{
    Apache_HTTPDServerStatistics_Class_Provider* cxxSelf =((Apache_HTTPDServerStatistics_Class_Provider*)self);
    Context  cxxContext(context);
    Apache_HTTPDServerStatistics_Class cxxInstanceName(instanceName, true);

    cxxSelf->GetInstance(
        cxxContext,
        nameSpace,
        cxxInstanceName,
        __PropertySet(propertySet));
}

MI_EXTERN_C void MI_CALL Apache_HTTPDServerStatistics_CreateInstance(
    Apache_HTTPDServerStatistics_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDServerStatistics* newInstance)
{
    Apache_HTTPDServerStatistics_Class_Provider* cxxSelf =((Apache_HTTPDServerStatistics_Class_Provider*)self);
    Context  cxxContext(context);
    Apache_HTTPDServerStatistics_Class cxxNewInstance(newInstance, false);

    cxxSelf->CreateInstance(cxxContext, nameSpace, cxxNewInstance);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDServerStatistics_ModifyInstance(
    Apache_HTTPDServerStatistics_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDServerStatistics* modifiedInstance,
    const MI_PropertySet* propertySet)
{
    Apache_HTTPDServerStatistics_Class_Provider* cxxSelf =((Apache_HTTPDServerStatistics_Class_Provider*)self);
    Context  cxxContext(context);
    Apache_HTTPDServerStatistics_Class cxxModifiedInstance(modifiedInstance, false);

    cxxSelf->ModifyInstance(
        cxxContext,
        nameSpace,
        cxxModifiedInstance,
        __PropertySet(propertySet));
}

MI_EXTERN_C void MI_CALL Apache_HTTPDServerStatistics_DeleteInstance(
    Apache_HTTPDServerStatistics_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDServerStatistics* instanceName)
{
    Apache_HTTPDServerStatistics_Class_Provider* cxxSelf =((Apache_HTTPDServerStatistics_Class_Provider*)self);
    Context  cxxContext(context);
    Apache_HTTPDServerStatistics_Class cxxInstanceName(instanceName, true);

    cxxSelf->DeleteInstance(cxxContext, nameSpace, cxxInstanceName);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDServerStatistics_Invoke_ResetSelectedStats(
    Apache_HTTPDServerStatistics_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const MI_Char* methodName,
    const Apache_HTTPDServerStatistics* instanceName,
    const Apache_HTTPDServerStatistics_ResetSelectedStats* in)
{
    Apache_HTTPDServerStatistics_Class_Provider* cxxSelf =((Apache_HTTPDServerStatistics_Class_Provider*)self);
    Apache_HTTPDServerStatistics_Class instance(instanceName, false);
    Context  cxxContext(context);
    Apache_HTTPDServerStatistics_ResetSelectedStats_Class param(in, false);

    cxxSelf->Invoke_ResetSelectedStats(cxxContext, nameSpace, instance, param);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHost_Load(
    Apache_HTTPDVirtualHost_Self** self,
    MI_Module_Self* selfModule,
    MI_Context* context)
{
    MI_Result r = MI_RESULT_OK;
    Context ctx(context, &r);
    Apache_HTTPDVirtualHost_Class_Provider* prov = new Apache_HTTPDVirtualHost_Class_Provider((Module*)selfModule);

    prov->Load(ctx);
    if (MI_RESULT_OK != r)
    {
        delete prov;
        MI_PostResult(context, r);
        return;
    }
    *self = (Apache_HTTPDVirtualHost_Self*)prov;
    MI_PostResult(context, MI_RESULT_OK);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHost_Unload(
    Apache_HTTPDVirtualHost_Self* self,
    MI_Context* context)
{
    MI_Result r = MI_RESULT_OK;
    Context ctx(context, &r);
    Apache_HTTPDVirtualHost_Class_Provider* prov = (Apache_HTTPDVirtualHost_Class_Provider*)self;

    prov->Unload(ctx);
    delete ((Apache_HTTPDVirtualHost_Class_Provider*)self);
    MI_PostResult(context, r);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHost_EnumerateInstances(
    Apache_HTTPDVirtualHost_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const MI_PropertySet* propertySet,
    MI_Boolean keysOnly,
    const MI_Filter* filter)
{
    Apache_HTTPDVirtualHost_Class_Provider* cxxSelf =((Apache_HTTPDVirtualHost_Class_Provider*)self);
    Context  cxxContext(context);

    cxxSelf->EnumerateInstances(
        cxxContext,
        nameSpace,
        __PropertySet(propertySet),
        __bool(keysOnly),
        filter);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHost_GetInstance(
    Apache_HTTPDVirtualHost_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDVirtualHost* instanceName,
    const MI_PropertySet* propertySet)
{
    Apache_HTTPDVirtualHost_Class_Provider* cxxSelf =((Apache_HTTPDVirtualHost_Class_Provider*)self);
    Context  cxxContext(context);
    Apache_HTTPDVirtualHost_Class cxxInstanceName(instanceName, true);

    cxxSelf->GetInstance(
        cxxContext,
        nameSpace,
        cxxInstanceName,
        __PropertySet(propertySet));
}

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHost_CreateInstance(
    Apache_HTTPDVirtualHost_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDVirtualHost* newInstance)
{
    Apache_HTTPDVirtualHost_Class_Provider* cxxSelf =((Apache_HTTPDVirtualHost_Class_Provider*)self);
    Context  cxxContext(context);
    Apache_HTTPDVirtualHost_Class cxxNewInstance(newInstance, false);

    cxxSelf->CreateInstance(cxxContext, nameSpace, cxxNewInstance);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHost_ModifyInstance(
    Apache_HTTPDVirtualHost_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDVirtualHost* modifiedInstance,
    const MI_PropertySet* propertySet)
{
    Apache_HTTPDVirtualHost_Class_Provider* cxxSelf =((Apache_HTTPDVirtualHost_Class_Provider*)self);
    Context  cxxContext(context);
    Apache_HTTPDVirtualHost_Class cxxModifiedInstance(modifiedInstance, false);

    cxxSelf->ModifyInstance(
        cxxContext,
        nameSpace,
        cxxModifiedInstance,
        __PropertySet(propertySet));
}

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHost_DeleteInstance(
    Apache_HTTPDVirtualHost_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDVirtualHost* instanceName)
{
    Apache_HTTPDVirtualHost_Class_Provider* cxxSelf =((Apache_HTTPDVirtualHost_Class_Provider*)self);
    Context  cxxContext(context);
    Apache_HTTPDVirtualHost_Class cxxInstanceName(instanceName, true);

    cxxSelf->DeleteInstance(cxxContext, nameSpace, cxxInstanceName);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHostCertificate_Load(
    Apache_HTTPDVirtualHostCertificate_Self** self,
    MI_Module_Self* selfModule,
    MI_Context* context)
{
    MI_Result r = MI_RESULT_OK;
    Context ctx(context, &r);
    Apache_HTTPDVirtualHostCertificate_Class_Provider* prov = new Apache_HTTPDVirtualHostCertificate_Class_Provider((Module*)selfModule);

    prov->Load(ctx);
    if (MI_RESULT_OK != r)
    {
        delete prov;
        MI_PostResult(context, r);
        return;
    }
    *self = (Apache_HTTPDVirtualHostCertificate_Self*)prov;
    MI_PostResult(context, MI_RESULT_OK);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHostCertificate_Unload(
    Apache_HTTPDVirtualHostCertificate_Self* self,
    MI_Context* context)
{
    MI_Result r = MI_RESULT_OK;
    Context ctx(context, &r);
    Apache_HTTPDVirtualHostCertificate_Class_Provider* prov = (Apache_HTTPDVirtualHostCertificate_Class_Provider*)self;

    prov->Unload(ctx);
    delete ((Apache_HTTPDVirtualHostCertificate_Class_Provider*)self);
    MI_PostResult(context, r);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHostCertificate_EnumerateInstances(
    Apache_HTTPDVirtualHostCertificate_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const MI_PropertySet* propertySet,
    MI_Boolean keysOnly,
    const MI_Filter* filter)
{
    Apache_HTTPDVirtualHostCertificate_Class_Provider* cxxSelf =((Apache_HTTPDVirtualHostCertificate_Class_Provider*)self);
    Context  cxxContext(context);

    cxxSelf->EnumerateInstances(
        cxxContext,
        nameSpace,
        __PropertySet(propertySet),
        __bool(keysOnly),
        filter);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHostCertificate_GetInstance(
    Apache_HTTPDVirtualHostCertificate_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDVirtualHostCertificate* instanceName,
    const MI_PropertySet* propertySet)
{
    Apache_HTTPDVirtualHostCertificate_Class_Provider* cxxSelf =((Apache_HTTPDVirtualHostCertificate_Class_Provider*)self);
    Context  cxxContext(context);
    Apache_HTTPDVirtualHostCertificate_Class cxxInstanceName(instanceName, true);

    cxxSelf->GetInstance(
        cxxContext,
        nameSpace,
        cxxInstanceName,
        __PropertySet(propertySet));
}

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHostCertificate_CreateInstance(
    Apache_HTTPDVirtualHostCertificate_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDVirtualHostCertificate* newInstance)
{
    Apache_HTTPDVirtualHostCertificate_Class_Provider* cxxSelf =((Apache_HTTPDVirtualHostCertificate_Class_Provider*)self);
    Context  cxxContext(context);
    Apache_HTTPDVirtualHostCertificate_Class cxxNewInstance(newInstance, false);

    cxxSelf->CreateInstance(cxxContext, nameSpace, cxxNewInstance);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHostCertificate_ModifyInstance(
    Apache_HTTPDVirtualHostCertificate_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDVirtualHostCertificate* modifiedInstance,
    const MI_PropertySet* propertySet)
{
    Apache_HTTPDVirtualHostCertificate_Class_Provider* cxxSelf =((Apache_HTTPDVirtualHostCertificate_Class_Provider*)self);
    Context  cxxContext(context);
    Apache_HTTPDVirtualHostCertificate_Class cxxModifiedInstance(modifiedInstance, false);

    cxxSelf->ModifyInstance(
        cxxContext,
        nameSpace,
        cxxModifiedInstance,
        __PropertySet(propertySet));
}

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHostCertificate_DeleteInstance(
    Apache_HTTPDVirtualHostCertificate_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDVirtualHostCertificate* instanceName)
{
    Apache_HTTPDVirtualHostCertificate_Class_Provider* cxxSelf =((Apache_HTTPDVirtualHostCertificate_Class_Provider*)self);
    Context  cxxContext(context);
    Apache_HTTPDVirtualHostCertificate_Class cxxInstanceName(instanceName, true);

    cxxSelf->DeleteInstance(cxxContext, nameSpace, cxxInstanceName);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHostStatistics_Load(
    Apache_HTTPDVirtualHostStatistics_Self** self,
    MI_Module_Self* selfModule,
    MI_Context* context)
{
    MI_Result r = MI_RESULT_OK;
    Context ctx(context, &r);
    Apache_HTTPDVirtualHostStatistics_Class_Provider* prov = new Apache_HTTPDVirtualHostStatistics_Class_Provider((Module*)selfModule);

    prov->Load(ctx);
    if (MI_RESULT_OK != r)
    {
        delete prov;
        MI_PostResult(context, r);
        return;
    }
    *self = (Apache_HTTPDVirtualHostStatistics_Self*)prov;
    MI_PostResult(context, MI_RESULT_OK);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHostStatistics_Unload(
    Apache_HTTPDVirtualHostStatistics_Self* self,
    MI_Context* context)
{
    MI_Result r = MI_RESULT_OK;
    Context ctx(context, &r);
    Apache_HTTPDVirtualHostStatistics_Class_Provider* prov = (Apache_HTTPDVirtualHostStatistics_Class_Provider*)self;

    prov->Unload(ctx);
    delete ((Apache_HTTPDVirtualHostStatistics_Class_Provider*)self);
    MI_PostResult(context, r);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHostStatistics_EnumerateInstances(
    Apache_HTTPDVirtualHostStatistics_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const MI_PropertySet* propertySet,
    MI_Boolean keysOnly,
    const MI_Filter* filter)
{
    Apache_HTTPDVirtualHostStatistics_Class_Provider* cxxSelf =((Apache_HTTPDVirtualHostStatistics_Class_Provider*)self);
    Context  cxxContext(context);

    cxxSelf->EnumerateInstances(
        cxxContext,
        nameSpace,
        __PropertySet(propertySet),
        __bool(keysOnly),
        filter);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHostStatistics_GetInstance(
    Apache_HTTPDVirtualHostStatistics_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDVirtualHostStatistics* instanceName,
    const MI_PropertySet* propertySet)
{
    Apache_HTTPDVirtualHostStatistics_Class_Provider* cxxSelf =((Apache_HTTPDVirtualHostStatistics_Class_Provider*)self);
    Context  cxxContext(context);
    Apache_HTTPDVirtualHostStatistics_Class cxxInstanceName(instanceName, true);

    cxxSelf->GetInstance(
        cxxContext,
        nameSpace,
        cxxInstanceName,
        __PropertySet(propertySet));
}

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHostStatistics_CreateInstance(
    Apache_HTTPDVirtualHostStatistics_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDVirtualHostStatistics* newInstance)
{
    Apache_HTTPDVirtualHostStatistics_Class_Provider* cxxSelf =((Apache_HTTPDVirtualHostStatistics_Class_Provider*)self);
    Context  cxxContext(context);
    Apache_HTTPDVirtualHostStatistics_Class cxxNewInstance(newInstance, false);

    cxxSelf->CreateInstance(cxxContext, nameSpace, cxxNewInstance);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHostStatistics_ModifyInstance(
    Apache_HTTPDVirtualHostStatistics_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDVirtualHostStatistics* modifiedInstance,
    const MI_PropertySet* propertySet)
{
    Apache_HTTPDVirtualHostStatistics_Class_Provider* cxxSelf =((Apache_HTTPDVirtualHostStatistics_Class_Provider*)self);
    Context  cxxContext(context);
    Apache_HTTPDVirtualHostStatistics_Class cxxModifiedInstance(modifiedInstance, false);

    cxxSelf->ModifyInstance(
        cxxContext,
        nameSpace,
        cxxModifiedInstance,
        __PropertySet(propertySet));
}

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHostStatistics_DeleteInstance(
    Apache_HTTPDVirtualHostStatistics_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDVirtualHostStatistics* instanceName)
{
    Apache_HTTPDVirtualHostStatistics_Class_Provider* cxxSelf =((Apache_HTTPDVirtualHostStatistics_Class_Provider*)self);
    Context  cxxContext(context);
    Apache_HTTPDVirtualHostStatistics_Class cxxInstanceName(instanceName, true);

    cxxSelf->DeleteInstance(cxxContext, nameSpace, cxxInstanceName);
}

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHostStatistics_Invoke_ResetSelectedStats(
    Apache_HTTPDVirtualHostStatistics_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const MI_Char* methodName,
    const Apache_HTTPDVirtualHostStatistics* instanceName,
    const Apache_HTTPDVirtualHostStatistics_ResetSelectedStats* in)
{
    Apache_HTTPDVirtualHostStatistics_Class_Provider* cxxSelf =((Apache_HTTPDVirtualHostStatistics_Class_Provider*)self);
    Apache_HTTPDVirtualHostStatistics_Class instance(instanceName, false);
    Context  cxxContext(context);
    Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_Class param(in, false);

    cxxSelf->Invoke_ResetSelectedStats(cxxContext, nameSpace, instance, param);
}


MI_EXTERN_C MI_SchemaDecl schemaDecl;

void MI_CALL Load(MI_Module_Self** self, struct _MI_Context* context)
{
    *self = (MI_Module_Self*)new Module;
    MI_PostResult(context, MI_RESULT_OK);
}

void MI_CALL Unload(MI_Module_Self* self, struct _MI_Context* context)
{
    Module* module = (Module*)self;
    delete module;
    MI_PostResult(context, MI_RESULT_OK);
}

MI_EXTERN_C MI_EXPORT MI_Module* MI_MAIN_CALL MI_Main(MI_Server* server)
{
    /* WARNING: THIS FUNCTION AUTOMATICALLY GENERATED. PLEASE DO NOT EDIT. */
    extern MI_Server* __mi_server;
    static MI_Module module;
    __mi_server = server;
    module.flags |= MI_MODULE_FLAG_STANDARD_QUALIFIERS;
    module.flags |= MI_MODULE_FLAG_CPLUSPLUS;
    module.charSize = sizeof(MI_Char);
    module.version = MI_VERSION;
    module.generatorVersion = MI_MAKE_VERSION(1,0,0);
    module.schemaDecl = &schemaDecl;
    module.Load = Load;
    module.Unload = Unload;
    return &module;
}

