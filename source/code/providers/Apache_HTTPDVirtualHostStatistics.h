/* @migen@ */
/*
**==============================================================================
**
** WARNING: THIS FILE WAS AUTOMATICALLY GENERATED. PLEASE DO NOT EDIT.
**
**==============================================================================
*/
#ifndef _Apache_HTTPDVirtualHostStatistics_h
#define _Apache_HTTPDVirtualHostStatistics_h

#include <MI.h>
#include "CIM_StatisticalData.h"

/*
**==============================================================================
**
** Apache_HTTPDVirtualHostStatistics [Apache_HTTPDVirtualHostStatistics]
**
** Keys:
**    InstanceID
**
**==============================================================================
*/

typedef struct _Apache_HTTPDVirtualHostStatistics /* extends CIM_StatisticalData */
{
    MI_Instance __instance;
    /* CIM_ManagedElement properties */
    /*KEY*/ MI_ConstStringField InstanceID;
    MI_ConstStringField Caption;
    MI_ConstStringField Description;
    MI_ConstStringField ElementName;
    /* CIM_StatisticalData properties */
    MI_ConstDatetimeField StartStatisticTime;
    MI_ConstDatetimeField StatisticTime;
    MI_ConstDatetimeField SampleInterval;
    /* Apache_HTTPDVirtualHostStatistics properties */
    MI_ConstStringField ServerName;
    MI_ConstUint64Field RequestsTotal;
    MI_ConstUint64Field RequestsTotalBytes;
    MI_ConstUint32Field RequestsPerSecond;
    MI_ConstUint32Field KBPerRequest;
    MI_ConstUint32Field KBPerSecond;
    MI_ConstUint64Field ErrorCount400;
    MI_ConstUint64Field ErrorCount500;
    MI_ConstUint32Field ErrorsPerMinute400;
    MI_ConstUint32Field ErrorsPerMinute500;
}
Apache_HTTPDVirtualHostStatistics;

typedef struct _Apache_HTTPDVirtualHostStatistics_Ref
{
    Apache_HTTPDVirtualHostStatistics* value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
Apache_HTTPDVirtualHostStatistics_Ref;

typedef struct _Apache_HTTPDVirtualHostStatistics_ConstRef
{
    MI_CONST Apache_HTTPDVirtualHostStatistics* value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
Apache_HTTPDVirtualHostStatistics_ConstRef;

typedef struct _Apache_HTTPDVirtualHostStatistics_Array
{
    struct _Apache_HTTPDVirtualHostStatistics** data;
    MI_Uint32 size;
}
Apache_HTTPDVirtualHostStatistics_Array;

typedef struct _Apache_HTTPDVirtualHostStatistics_ConstArray
{
    struct _Apache_HTTPDVirtualHostStatistics MI_CONST* MI_CONST* data;
    MI_Uint32 size;
}
Apache_HTTPDVirtualHostStatistics_ConstArray;

typedef struct _Apache_HTTPDVirtualHostStatistics_ArrayRef
{
    Apache_HTTPDVirtualHostStatistics_Array value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
Apache_HTTPDVirtualHostStatistics_ArrayRef;

typedef struct _Apache_HTTPDVirtualHostStatistics_ConstArrayRef
{
    Apache_HTTPDVirtualHostStatistics_ConstArray value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
Apache_HTTPDVirtualHostStatistics_ConstArrayRef;

MI_EXTERN_C MI_CONST MI_ClassDecl Apache_HTTPDVirtualHostStatistics_rtti;

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Construct(
    Apache_HTTPDVirtualHostStatistics* self,
    MI_Context* context)
{
    return MI_ConstructInstance(context, &Apache_HTTPDVirtualHostStatistics_rtti,
        (MI_Instance*)&self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Clone(
    const Apache_HTTPDVirtualHostStatistics* self,
    Apache_HTTPDVirtualHostStatistics** newInstance)
{
    return MI_Instance_Clone(
        &self->__instance, (MI_Instance**)newInstance);
}

MI_INLINE MI_Boolean MI_CALL Apache_HTTPDVirtualHostStatistics_IsA(
    const MI_Instance* self)
{
    MI_Boolean res = MI_FALSE;
    return MI_Instance_IsA(self, &Apache_HTTPDVirtualHostStatistics_rtti, &res) == MI_RESULT_OK && res;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Destruct(Apache_HTTPDVirtualHostStatistics* self)
{
    return MI_Instance_Destruct(&self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Delete(Apache_HTTPDVirtualHostStatistics* self)
{
    return MI_Instance_Delete(&self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Post(
    const Apache_HTTPDVirtualHostStatistics* self,
    MI_Context* context)
{
    return MI_PostInstance(context, &self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Set_InstanceID(
    Apache_HTTPDVirtualHostStatistics* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        0,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_SetPtr_InstanceID(
    Apache_HTTPDVirtualHostStatistics* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        0,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Clear_InstanceID(
    Apache_HTTPDVirtualHostStatistics* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Set_Caption(
    Apache_HTTPDVirtualHostStatistics* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        1,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_SetPtr_Caption(
    Apache_HTTPDVirtualHostStatistics* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        1,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Clear_Caption(
    Apache_HTTPDVirtualHostStatistics* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        1);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Set_Description(
    Apache_HTTPDVirtualHostStatistics* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        2,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_SetPtr_Description(
    Apache_HTTPDVirtualHostStatistics* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        2,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Clear_Description(
    Apache_HTTPDVirtualHostStatistics* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        2);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Set_ElementName(
    Apache_HTTPDVirtualHostStatistics* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        3,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_SetPtr_ElementName(
    Apache_HTTPDVirtualHostStatistics* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        3,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Clear_ElementName(
    Apache_HTTPDVirtualHostStatistics* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        3);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Set_StartStatisticTime(
    Apache_HTTPDVirtualHostStatistics* self,
    MI_Datetime x)
{
    ((MI_DatetimeField*)&self->StartStatisticTime)->value = x;
    ((MI_DatetimeField*)&self->StartStatisticTime)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Clear_StartStatisticTime(
    Apache_HTTPDVirtualHostStatistics* self)
{
    memset((void*)&self->StartStatisticTime, 0, sizeof(self->StartStatisticTime));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Set_StatisticTime(
    Apache_HTTPDVirtualHostStatistics* self,
    MI_Datetime x)
{
    ((MI_DatetimeField*)&self->StatisticTime)->value = x;
    ((MI_DatetimeField*)&self->StatisticTime)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Clear_StatisticTime(
    Apache_HTTPDVirtualHostStatistics* self)
{
    memset((void*)&self->StatisticTime, 0, sizeof(self->StatisticTime));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Set_SampleInterval(
    Apache_HTTPDVirtualHostStatistics* self,
    MI_Datetime x)
{
    ((MI_DatetimeField*)&self->SampleInterval)->value = x;
    ((MI_DatetimeField*)&self->SampleInterval)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Clear_SampleInterval(
    Apache_HTTPDVirtualHostStatistics* self)
{
    memset((void*)&self->SampleInterval, 0, sizeof(self->SampleInterval));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Set_ServerName(
    Apache_HTTPDVirtualHostStatistics* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        7,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_SetPtr_ServerName(
    Apache_HTTPDVirtualHostStatistics* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        7,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Clear_ServerName(
    Apache_HTTPDVirtualHostStatistics* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        7);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Set_RequestsTotal(
    Apache_HTTPDVirtualHostStatistics* self,
    MI_Uint64 x)
{
    ((MI_Uint64Field*)&self->RequestsTotal)->value = x;
    ((MI_Uint64Field*)&self->RequestsTotal)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Clear_RequestsTotal(
    Apache_HTTPDVirtualHostStatistics* self)
{
    memset((void*)&self->RequestsTotal, 0, sizeof(self->RequestsTotal));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Set_RequestsTotalBytes(
    Apache_HTTPDVirtualHostStatistics* self,
    MI_Uint64 x)
{
    ((MI_Uint64Field*)&self->RequestsTotalBytes)->value = x;
    ((MI_Uint64Field*)&self->RequestsTotalBytes)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Clear_RequestsTotalBytes(
    Apache_HTTPDVirtualHostStatistics* self)
{
    memset((void*)&self->RequestsTotalBytes, 0, sizeof(self->RequestsTotalBytes));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Set_RequestsPerSecond(
    Apache_HTTPDVirtualHostStatistics* self,
    MI_Uint32 x)
{
    ((MI_Uint32Field*)&self->RequestsPerSecond)->value = x;
    ((MI_Uint32Field*)&self->RequestsPerSecond)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Clear_RequestsPerSecond(
    Apache_HTTPDVirtualHostStatistics* self)
{
    memset((void*)&self->RequestsPerSecond, 0, sizeof(self->RequestsPerSecond));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Set_KBPerRequest(
    Apache_HTTPDVirtualHostStatistics* self,
    MI_Uint32 x)
{
    ((MI_Uint32Field*)&self->KBPerRequest)->value = x;
    ((MI_Uint32Field*)&self->KBPerRequest)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Clear_KBPerRequest(
    Apache_HTTPDVirtualHostStatistics* self)
{
    memset((void*)&self->KBPerRequest, 0, sizeof(self->KBPerRequest));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Set_KBPerSecond(
    Apache_HTTPDVirtualHostStatistics* self,
    MI_Uint32 x)
{
    ((MI_Uint32Field*)&self->KBPerSecond)->value = x;
    ((MI_Uint32Field*)&self->KBPerSecond)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Clear_KBPerSecond(
    Apache_HTTPDVirtualHostStatistics* self)
{
    memset((void*)&self->KBPerSecond, 0, sizeof(self->KBPerSecond));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Set_ErrorCount400(
    Apache_HTTPDVirtualHostStatistics* self,
    MI_Uint64 x)
{
    ((MI_Uint64Field*)&self->ErrorCount400)->value = x;
    ((MI_Uint64Field*)&self->ErrorCount400)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Clear_ErrorCount400(
    Apache_HTTPDVirtualHostStatistics* self)
{
    memset((void*)&self->ErrorCount400, 0, sizeof(self->ErrorCount400));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Set_ErrorCount500(
    Apache_HTTPDVirtualHostStatistics* self,
    MI_Uint64 x)
{
    ((MI_Uint64Field*)&self->ErrorCount500)->value = x;
    ((MI_Uint64Field*)&self->ErrorCount500)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Clear_ErrorCount500(
    Apache_HTTPDVirtualHostStatistics* self)
{
    memset((void*)&self->ErrorCount500, 0, sizeof(self->ErrorCount500));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Set_ErrorsPerMinute400(
    Apache_HTTPDVirtualHostStatistics* self,
    MI_Uint32 x)
{
    ((MI_Uint32Field*)&self->ErrorsPerMinute400)->value = x;
    ((MI_Uint32Field*)&self->ErrorsPerMinute400)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Clear_ErrorsPerMinute400(
    Apache_HTTPDVirtualHostStatistics* self)
{
    memset((void*)&self->ErrorsPerMinute400, 0, sizeof(self->ErrorsPerMinute400));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Set_ErrorsPerMinute500(
    Apache_HTTPDVirtualHostStatistics* self,
    MI_Uint32 x)
{
    ((MI_Uint32Field*)&self->ErrorsPerMinute500)->value = x;
    ((MI_Uint32Field*)&self->ErrorsPerMinute500)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_Clear_ErrorsPerMinute500(
    Apache_HTTPDVirtualHostStatistics* self)
{
    memset((void*)&self->ErrorsPerMinute500, 0, sizeof(self->ErrorsPerMinute500));
    return MI_RESULT_OK;
}

/*
**==============================================================================
**
** Apache_HTTPDVirtualHostStatistics.ResetSelectedStats()
**
**==============================================================================
*/

typedef struct _Apache_HTTPDVirtualHostStatistics_ResetSelectedStats
{
    MI_Instance __instance;
    /*OUT*/ MI_ConstUint32Field MIReturn;
    /*IN*/ MI_ConstStringAField SelectedStatistics;
}
Apache_HTTPDVirtualHostStatistics_ResetSelectedStats;

MI_EXTERN_C MI_CONST MI_MethodDecl Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_rtti;

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_Construct(
    Apache_HTTPDVirtualHostStatistics_ResetSelectedStats* self,
    MI_Context* context)
{
    return MI_ConstructParameters(context, &Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_rtti,
        (MI_Instance*)&self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_Clone(
    const Apache_HTTPDVirtualHostStatistics_ResetSelectedStats* self,
    Apache_HTTPDVirtualHostStatistics_ResetSelectedStats** newInstance)
{
    return MI_Instance_Clone(
        &self->__instance, (MI_Instance**)newInstance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_Destruct(
    Apache_HTTPDVirtualHostStatistics_ResetSelectedStats* self)
{
    return MI_Instance_Destruct(&self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_Delete(
    Apache_HTTPDVirtualHostStatistics_ResetSelectedStats* self)
{
    return MI_Instance_Delete(&self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_Post(
    const Apache_HTTPDVirtualHostStatistics_ResetSelectedStats* self,
    MI_Context* context)
{
    return MI_PostInstance(context, &self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_Set_MIReturn(
    Apache_HTTPDVirtualHostStatistics_ResetSelectedStats* self,
    MI_Uint32 x)
{
    ((MI_Uint32Field*)&self->MIReturn)->value = x;
    ((MI_Uint32Field*)&self->MIReturn)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_Clear_MIReturn(
    Apache_HTTPDVirtualHostStatistics_ResetSelectedStats* self)
{
    memset((void*)&self->MIReturn, 0, sizeof(self->MIReturn));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_Set_SelectedStatistics(
    Apache_HTTPDVirtualHostStatistics_ResetSelectedStats* self,
    const MI_Char** data,
    MI_Uint32 size)
{
    MI_Array arr;
    arr.data = (void*)data;
    arr.size = size;
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        1,
        (MI_Value*)&arr,
        MI_STRINGA,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_SetPtr_SelectedStatistics(
    Apache_HTTPDVirtualHostStatistics_ResetSelectedStats* self,
    const MI_Char** data,
    MI_Uint32 size)
{
    MI_Array arr;
    arr.data = (void*)data;
    arr.size = size;
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        1,
        (MI_Value*)&arr,
        MI_STRINGA,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_Clear_SelectedStatistics(
    Apache_HTTPDVirtualHostStatistics_ResetSelectedStats* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        1);
}

/*
**==============================================================================
**
** Apache_HTTPDVirtualHostStatistics provider function prototypes
**
**==============================================================================
*/

/* The developer may optionally define this structure */
typedef struct _Apache_HTTPDVirtualHostStatistics_Self Apache_HTTPDVirtualHostStatistics_Self;

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHostStatistics_Load(
    Apache_HTTPDVirtualHostStatistics_Self** self,
    MI_Module_Self* selfModule,
    MI_Context* context);

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHostStatistics_Unload(
    Apache_HTTPDVirtualHostStatistics_Self* self,
    MI_Context* context);

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHostStatistics_EnumerateInstances(
    Apache_HTTPDVirtualHostStatistics_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const MI_PropertySet* propertySet,
    MI_Boolean keysOnly,
    const MI_Filter* filter);

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHostStatistics_GetInstance(
    Apache_HTTPDVirtualHostStatistics_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDVirtualHostStatistics* instanceName,
    const MI_PropertySet* propertySet);

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHostStatistics_CreateInstance(
    Apache_HTTPDVirtualHostStatistics_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDVirtualHostStatistics* newInstance);

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHostStatistics_ModifyInstance(
    Apache_HTTPDVirtualHostStatistics_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDVirtualHostStatistics* modifiedInstance,
    const MI_PropertySet* propertySet);

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHostStatistics_DeleteInstance(
    Apache_HTTPDVirtualHostStatistics_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDVirtualHostStatistics* instanceName);

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHostStatistics_Invoke_ResetSelectedStats(
    Apache_HTTPDVirtualHostStatistics_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const MI_Char* methodName,
    const Apache_HTTPDVirtualHostStatistics* instanceName,
    const Apache_HTTPDVirtualHostStatistics_ResetSelectedStats* in);


/*
**==============================================================================
**
** Apache_HTTPDVirtualHostStatistics_Class
**
**==============================================================================
*/

#ifdef __cplusplus
# include <micxx/micxx.h>

MI_BEGIN_NAMESPACE

class Apache_HTTPDVirtualHostStatistics_Class : public CIM_StatisticalData_Class
{
public:
    
    typedef Apache_HTTPDVirtualHostStatistics Self;
    
    Apache_HTTPDVirtualHostStatistics_Class() :
        CIM_StatisticalData_Class(&Apache_HTTPDVirtualHostStatistics_rtti)
    {
    }
    
    Apache_HTTPDVirtualHostStatistics_Class(
        const Apache_HTTPDVirtualHostStatistics* instanceName,
        bool keysOnly) :
        CIM_StatisticalData_Class(
            &Apache_HTTPDVirtualHostStatistics_rtti,
            &instanceName->__instance,
            keysOnly)
    {
    }
    
    Apache_HTTPDVirtualHostStatistics_Class(
        const MI_ClassDecl* clDecl,
        const MI_Instance* instance,
        bool keysOnly) :
        CIM_StatisticalData_Class(clDecl, instance, keysOnly)
    {
    }
    
    Apache_HTTPDVirtualHostStatistics_Class(
        const MI_ClassDecl* clDecl) :
        CIM_StatisticalData_Class(clDecl)
    {
    }
    
    Apache_HTTPDVirtualHostStatistics_Class& operator=(
        const Apache_HTTPDVirtualHostStatistics_Class& x)
    {
        CopyRef(x);
        return *this;
    }
    
    Apache_HTTPDVirtualHostStatistics_Class(
        const Apache_HTTPDVirtualHostStatistics_Class& x) :
        CIM_StatisticalData_Class(x)
    {
    }

    static const MI_ClassDecl* GetClassDecl()
    {
        return &Apache_HTTPDVirtualHostStatistics_rtti;
    }

    //
    // Apache_HTTPDVirtualHostStatistics_Class.ServerName
    //
    
    const Field<String>& ServerName() const
    {
        const size_t n = offsetof(Self, ServerName);
        return GetField<String>(n);
    }
    
    void ServerName(const Field<String>& x)
    {
        const size_t n = offsetof(Self, ServerName);
        GetField<String>(n) = x;
    }
    
    const String& ServerName_value() const
    {
        const size_t n = offsetof(Self, ServerName);
        return GetField<String>(n).value;
    }
    
    void ServerName_value(const String& x)
    {
        const size_t n = offsetof(Self, ServerName);
        GetField<String>(n).Set(x);
    }
    
    bool ServerName_exists() const
    {
        const size_t n = offsetof(Self, ServerName);
        return GetField<String>(n).exists ? true : false;
    }
    
    void ServerName_clear()
    {
        const size_t n = offsetof(Self, ServerName);
        GetField<String>(n).Clear();
    }

    //
    // Apache_HTTPDVirtualHostStatistics_Class.RequestsTotal
    //
    
    const Field<Uint64>& RequestsTotal() const
    {
        const size_t n = offsetof(Self, RequestsTotal);
        return GetField<Uint64>(n);
    }
    
    void RequestsTotal(const Field<Uint64>& x)
    {
        const size_t n = offsetof(Self, RequestsTotal);
        GetField<Uint64>(n) = x;
    }
    
    const Uint64& RequestsTotal_value() const
    {
        const size_t n = offsetof(Self, RequestsTotal);
        return GetField<Uint64>(n).value;
    }
    
    void RequestsTotal_value(const Uint64& x)
    {
        const size_t n = offsetof(Self, RequestsTotal);
        GetField<Uint64>(n).Set(x);
    }
    
    bool RequestsTotal_exists() const
    {
        const size_t n = offsetof(Self, RequestsTotal);
        return GetField<Uint64>(n).exists ? true : false;
    }
    
    void RequestsTotal_clear()
    {
        const size_t n = offsetof(Self, RequestsTotal);
        GetField<Uint64>(n).Clear();
    }

    //
    // Apache_HTTPDVirtualHostStatistics_Class.RequestsTotalBytes
    //
    
    const Field<Uint64>& RequestsTotalBytes() const
    {
        const size_t n = offsetof(Self, RequestsTotalBytes);
        return GetField<Uint64>(n);
    }
    
    void RequestsTotalBytes(const Field<Uint64>& x)
    {
        const size_t n = offsetof(Self, RequestsTotalBytes);
        GetField<Uint64>(n) = x;
    }
    
    const Uint64& RequestsTotalBytes_value() const
    {
        const size_t n = offsetof(Self, RequestsTotalBytes);
        return GetField<Uint64>(n).value;
    }
    
    void RequestsTotalBytes_value(const Uint64& x)
    {
        const size_t n = offsetof(Self, RequestsTotalBytes);
        GetField<Uint64>(n).Set(x);
    }
    
    bool RequestsTotalBytes_exists() const
    {
        const size_t n = offsetof(Self, RequestsTotalBytes);
        return GetField<Uint64>(n).exists ? true : false;
    }
    
    void RequestsTotalBytes_clear()
    {
        const size_t n = offsetof(Self, RequestsTotalBytes);
        GetField<Uint64>(n).Clear();
    }

    //
    // Apache_HTTPDVirtualHostStatistics_Class.RequestsPerSecond
    //
    
    const Field<Uint32>& RequestsPerSecond() const
    {
        const size_t n = offsetof(Self, RequestsPerSecond);
        return GetField<Uint32>(n);
    }
    
    void RequestsPerSecond(const Field<Uint32>& x)
    {
        const size_t n = offsetof(Self, RequestsPerSecond);
        GetField<Uint32>(n) = x;
    }
    
    const Uint32& RequestsPerSecond_value() const
    {
        const size_t n = offsetof(Self, RequestsPerSecond);
        return GetField<Uint32>(n).value;
    }
    
    void RequestsPerSecond_value(const Uint32& x)
    {
        const size_t n = offsetof(Self, RequestsPerSecond);
        GetField<Uint32>(n).Set(x);
    }
    
    bool RequestsPerSecond_exists() const
    {
        const size_t n = offsetof(Self, RequestsPerSecond);
        return GetField<Uint32>(n).exists ? true : false;
    }
    
    void RequestsPerSecond_clear()
    {
        const size_t n = offsetof(Self, RequestsPerSecond);
        GetField<Uint32>(n).Clear();
    }

    //
    // Apache_HTTPDVirtualHostStatistics_Class.KBPerRequest
    //
    
    const Field<Uint32>& KBPerRequest() const
    {
        const size_t n = offsetof(Self, KBPerRequest);
        return GetField<Uint32>(n);
    }
    
    void KBPerRequest(const Field<Uint32>& x)
    {
        const size_t n = offsetof(Self, KBPerRequest);
        GetField<Uint32>(n) = x;
    }
    
    const Uint32& KBPerRequest_value() const
    {
        const size_t n = offsetof(Self, KBPerRequest);
        return GetField<Uint32>(n).value;
    }
    
    void KBPerRequest_value(const Uint32& x)
    {
        const size_t n = offsetof(Self, KBPerRequest);
        GetField<Uint32>(n).Set(x);
    }
    
    bool KBPerRequest_exists() const
    {
        const size_t n = offsetof(Self, KBPerRequest);
        return GetField<Uint32>(n).exists ? true : false;
    }
    
    void KBPerRequest_clear()
    {
        const size_t n = offsetof(Self, KBPerRequest);
        GetField<Uint32>(n).Clear();
    }

    //
    // Apache_HTTPDVirtualHostStatistics_Class.KBPerSecond
    //
    
    const Field<Uint32>& KBPerSecond() const
    {
        const size_t n = offsetof(Self, KBPerSecond);
        return GetField<Uint32>(n);
    }
    
    void KBPerSecond(const Field<Uint32>& x)
    {
        const size_t n = offsetof(Self, KBPerSecond);
        GetField<Uint32>(n) = x;
    }
    
    const Uint32& KBPerSecond_value() const
    {
        const size_t n = offsetof(Self, KBPerSecond);
        return GetField<Uint32>(n).value;
    }
    
    void KBPerSecond_value(const Uint32& x)
    {
        const size_t n = offsetof(Self, KBPerSecond);
        GetField<Uint32>(n).Set(x);
    }
    
    bool KBPerSecond_exists() const
    {
        const size_t n = offsetof(Self, KBPerSecond);
        return GetField<Uint32>(n).exists ? true : false;
    }
    
    void KBPerSecond_clear()
    {
        const size_t n = offsetof(Self, KBPerSecond);
        GetField<Uint32>(n).Clear();
    }

    //
    // Apache_HTTPDVirtualHostStatistics_Class.ErrorCount400
    //
    
    const Field<Uint64>& ErrorCount400() const
    {
        const size_t n = offsetof(Self, ErrorCount400);
        return GetField<Uint64>(n);
    }
    
    void ErrorCount400(const Field<Uint64>& x)
    {
        const size_t n = offsetof(Self, ErrorCount400);
        GetField<Uint64>(n) = x;
    }
    
    const Uint64& ErrorCount400_value() const
    {
        const size_t n = offsetof(Self, ErrorCount400);
        return GetField<Uint64>(n).value;
    }
    
    void ErrorCount400_value(const Uint64& x)
    {
        const size_t n = offsetof(Self, ErrorCount400);
        GetField<Uint64>(n).Set(x);
    }
    
    bool ErrorCount400_exists() const
    {
        const size_t n = offsetof(Self, ErrorCount400);
        return GetField<Uint64>(n).exists ? true : false;
    }
    
    void ErrorCount400_clear()
    {
        const size_t n = offsetof(Self, ErrorCount400);
        GetField<Uint64>(n).Clear();
    }

    //
    // Apache_HTTPDVirtualHostStatistics_Class.ErrorCount500
    //
    
    const Field<Uint64>& ErrorCount500() const
    {
        const size_t n = offsetof(Self, ErrorCount500);
        return GetField<Uint64>(n);
    }
    
    void ErrorCount500(const Field<Uint64>& x)
    {
        const size_t n = offsetof(Self, ErrorCount500);
        GetField<Uint64>(n) = x;
    }
    
    const Uint64& ErrorCount500_value() const
    {
        const size_t n = offsetof(Self, ErrorCount500);
        return GetField<Uint64>(n).value;
    }
    
    void ErrorCount500_value(const Uint64& x)
    {
        const size_t n = offsetof(Self, ErrorCount500);
        GetField<Uint64>(n).Set(x);
    }
    
    bool ErrorCount500_exists() const
    {
        const size_t n = offsetof(Self, ErrorCount500);
        return GetField<Uint64>(n).exists ? true : false;
    }
    
    void ErrorCount500_clear()
    {
        const size_t n = offsetof(Self, ErrorCount500);
        GetField<Uint64>(n).Clear();
    }

    //
    // Apache_HTTPDVirtualHostStatistics_Class.ErrorsPerMinute400
    //
    
    const Field<Uint32>& ErrorsPerMinute400() const
    {
        const size_t n = offsetof(Self, ErrorsPerMinute400);
        return GetField<Uint32>(n);
    }
    
    void ErrorsPerMinute400(const Field<Uint32>& x)
    {
        const size_t n = offsetof(Self, ErrorsPerMinute400);
        GetField<Uint32>(n) = x;
    }
    
    const Uint32& ErrorsPerMinute400_value() const
    {
        const size_t n = offsetof(Self, ErrorsPerMinute400);
        return GetField<Uint32>(n).value;
    }
    
    void ErrorsPerMinute400_value(const Uint32& x)
    {
        const size_t n = offsetof(Self, ErrorsPerMinute400);
        GetField<Uint32>(n).Set(x);
    }
    
    bool ErrorsPerMinute400_exists() const
    {
        const size_t n = offsetof(Self, ErrorsPerMinute400);
        return GetField<Uint32>(n).exists ? true : false;
    }
    
    void ErrorsPerMinute400_clear()
    {
        const size_t n = offsetof(Self, ErrorsPerMinute400);
        GetField<Uint32>(n).Clear();
    }

    //
    // Apache_HTTPDVirtualHostStatistics_Class.ErrorsPerMinute500
    //
    
    const Field<Uint32>& ErrorsPerMinute500() const
    {
        const size_t n = offsetof(Self, ErrorsPerMinute500);
        return GetField<Uint32>(n);
    }
    
    void ErrorsPerMinute500(const Field<Uint32>& x)
    {
        const size_t n = offsetof(Self, ErrorsPerMinute500);
        GetField<Uint32>(n) = x;
    }
    
    const Uint32& ErrorsPerMinute500_value() const
    {
        const size_t n = offsetof(Self, ErrorsPerMinute500);
        return GetField<Uint32>(n).value;
    }
    
    void ErrorsPerMinute500_value(const Uint32& x)
    {
        const size_t n = offsetof(Self, ErrorsPerMinute500);
        GetField<Uint32>(n).Set(x);
    }
    
    bool ErrorsPerMinute500_exists() const
    {
        const size_t n = offsetof(Self, ErrorsPerMinute500);
        return GetField<Uint32>(n).exists ? true : false;
    }
    
    void ErrorsPerMinute500_clear()
    {
        const size_t n = offsetof(Self, ErrorsPerMinute500);
        GetField<Uint32>(n).Clear();
    }
};

typedef Array<Apache_HTTPDVirtualHostStatistics_Class> Apache_HTTPDVirtualHostStatistics_ClassA;

class Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_Class : public Instance
{
public:
    
    typedef Apache_HTTPDVirtualHostStatistics_ResetSelectedStats Self;
    
    Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_Class() :
        Instance(&Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_rtti)
    {
    }
    
    Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_Class(
        const Apache_HTTPDVirtualHostStatistics_ResetSelectedStats* instanceName,
        bool keysOnly) :
        Instance(
            &Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_rtti,
            &instanceName->__instance,
            keysOnly)
    {
    }
    
    Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_Class(
        const MI_ClassDecl* clDecl,
        const MI_Instance* instance,
        bool keysOnly) :
        Instance(clDecl, instance, keysOnly)
    {
    }
    
    Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_Class(
        const MI_ClassDecl* clDecl) :
        Instance(clDecl)
    {
    }
    
    Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_Class& operator=(
        const Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_Class& x)
    {
        CopyRef(x);
        return *this;
    }
    
    Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_Class(
        const Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_Class& x) :
        Instance(x)
    {
    }

    //
    // Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_Class.MIReturn
    //
    
    const Field<Uint32>& MIReturn() const
    {
        const size_t n = offsetof(Self, MIReturn);
        return GetField<Uint32>(n);
    }
    
    void MIReturn(const Field<Uint32>& x)
    {
        const size_t n = offsetof(Self, MIReturn);
        GetField<Uint32>(n) = x;
    }
    
    const Uint32& MIReturn_value() const
    {
        const size_t n = offsetof(Self, MIReturn);
        return GetField<Uint32>(n).value;
    }
    
    void MIReturn_value(const Uint32& x)
    {
        const size_t n = offsetof(Self, MIReturn);
        GetField<Uint32>(n).Set(x);
    }
    
    bool MIReturn_exists() const
    {
        const size_t n = offsetof(Self, MIReturn);
        return GetField<Uint32>(n).exists ? true : false;
    }
    
    void MIReturn_clear()
    {
        const size_t n = offsetof(Self, MIReturn);
        GetField<Uint32>(n).Clear();
    }

    //
    // Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_Class.SelectedStatistics
    //
    
    const Field<StringA>& SelectedStatistics() const
    {
        const size_t n = offsetof(Self, SelectedStatistics);
        return GetField<StringA>(n);
    }
    
    void SelectedStatistics(const Field<StringA>& x)
    {
        const size_t n = offsetof(Self, SelectedStatistics);
        GetField<StringA>(n) = x;
    }
    
    const StringA& SelectedStatistics_value() const
    {
        const size_t n = offsetof(Self, SelectedStatistics);
        return GetField<StringA>(n).value;
    }
    
    void SelectedStatistics_value(const StringA& x)
    {
        const size_t n = offsetof(Self, SelectedStatistics);
        GetField<StringA>(n).Set(x);
    }
    
    bool SelectedStatistics_exists() const
    {
        const size_t n = offsetof(Self, SelectedStatistics);
        return GetField<StringA>(n).exists ? true : false;
    }
    
    void SelectedStatistics_clear()
    {
        const size_t n = offsetof(Self, SelectedStatistics);
        GetField<StringA>(n).Clear();
    }
};

typedef Array<Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_Class> Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_ClassA;

MI_END_NAMESPACE

#endif /* __cplusplus */

#endif /* _Apache_HTTPDVirtualHostStatistics_h */
