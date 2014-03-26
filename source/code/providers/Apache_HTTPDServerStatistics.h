/* @migen@ */
/*
**==============================================================================
**
** WARNING: THIS FILE WAS AUTOMATICALLY GENERATED. PLEASE DO NOT EDIT.
**
**==============================================================================
*/
#ifndef _Apache_HTTPDServerStatistics_h
#define _Apache_HTTPDServerStatistics_h

#include <MI.h>
#include "CIM_StatisticalData.h"

/*
**==============================================================================
**
** Apache_HTTPDServerStatistics [Apache_HTTPDServerStatistics]
**
** Keys:
**    InstanceID
**
**==============================================================================
*/

typedef struct _Apache_HTTPDServerStatistics /* extends CIM_StatisticalData */
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
    /* Apache_HTTPDServerStatistics properties */
    MI_ConstUint64Field RequestsTotal;
    MI_ConstUint32Field RequestsPerSecond;
    MI_ConstUint32Field KBPerRequest;
    MI_ConstUint32Field KBPerSecond;
    MI_ConstUint32Field TotalPctCPU;
    MI_ConstUint32Field IdleWorkers;
    MI_ConstUint32Field BusyWorkers;
    MI_ConstUint32Field PctBusyWorkers;
    MI_ConstStringField ConfigurationFile;
}
Apache_HTTPDServerStatistics;

typedef struct _Apache_HTTPDServerStatistics_Ref
{
    Apache_HTTPDServerStatistics* value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
Apache_HTTPDServerStatistics_Ref;

typedef struct _Apache_HTTPDServerStatistics_ConstRef
{
    MI_CONST Apache_HTTPDServerStatistics* value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
Apache_HTTPDServerStatistics_ConstRef;

typedef struct _Apache_HTTPDServerStatistics_Array
{
    struct _Apache_HTTPDServerStatistics** data;
    MI_Uint32 size;
}
Apache_HTTPDServerStatistics_Array;

typedef struct _Apache_HTTPDServerStatistics_ConstArray
{
    struct _Apache_HTTPDServerStatistics MI_CONST* MI_CONST* data;
    MI_Uint32 size;
}
Apache_HTTPDServerStatistics_ConstArray;

typedef struct _Apache_HTTPDServerStatistics_ArrayRef
{
    Apache_HTTPDServerStatistics_Array value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
Apache_HTTPDServerStatistics_ArrayRef;

typedef struct _Apache_HTTPDServerStatistics_ConstArrayRef
{
    Apache_HTTPDServerStatistics_ConstArray value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
Apache_HTTPDServerStatistics_ConstArrayRef;

MI_EXTERN_C MI_CONST MI_ClassDecl Apache_HTTPDServerStatistics_rtti;

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Construct(
    Apache_HTTPDServerStatistics* self,
    MI_Context* context)
{
    return MI_ConstructInstance(context, &Apache_HTTPDServerStatistics_rtti,
        (MI_Instance*)&self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Clone(
    const Apache_HTTPDServerStatistics* self,
    Apache_HTTPDServerStatistics** newInstance)
{
    return MI_Instance_Clone(
        &self->__instance, (MI_Instance**)newInstance);
}

MI_INLINE MI_Boolean MI_CALL Apache_HTTPDServerStatistics_IsA(
    const MI_Instance* self)
{
    MI_Boolean res = MI_FALSE;
    return MI_Instance_IsA(self, &Apache_HTTPDServerStatistics_rtti, &res) == MI_RESULT_OK && res;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Destruct(Apache_HTTPDServerStatistics* self)
{
    return MI_Instance_Destruct(&self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Delete(Apache_HTTPDServerStatistics* self)
{
    return MI_Instance_Delete(&self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Post(
    const Apache_HTTPDServerStatistics* self,
    MI_Context* context)
{
    return MI_PostInstance(context, &self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Set_InstanceID(
    Apache_HTTPDServerStatistics* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        0,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_SetPtr_InstanceID(
    Apache_HTTPDServerStatistics* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        0,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Clear_InstanceID(
    Apache_HTTPDServerStatistics* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Set_Caption(
    Apache_HTTPDServerStatistics* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        1,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_SetPtr_Caption(
    Apache_HTTPDServerStatistics* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        1,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Clear_Caption(
    Apache_HTTPDServerStatistics* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        1);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Set_Description(
    Apache_HTTPDServerStatistics* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        2,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_SetPtr_Description(
    Apache_HTTPDServerStatistics* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        2,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Clear_Description(
    Apache_HTTPDServerStatistics* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        2);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Set_ElementName(
    Apache_HTTPDServerStatistics* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        3,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_SetPtr_ElementName(
    Apache_HTTPDServerStatistics* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        3,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Clear_ElementName(
    Apache_HTTPDServerStatistics* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        3);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Set_StartStatisticTime(
    Apache_HTTPDServerStatistics* self,
    MI_Datetime x)
{
    ((MI_DatetimeField*)&self->StartStatisticTime)->value = x;
    ((MI_DatetimeField*)&self->StartStatisticTime)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Clear_StartStatisticTime(
    Apache_HTTPDServerStatistics* self)
{
    memset((void*)&self->StartStatisticTime, 0, sizeof(self->StartStatisticTime));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Set_StatisticTime(
    Apache_HTTPDServerStatistics* self,
    MI_Datetime x)
{
    ((MI_DatetimeField*)&self->StatisticTime)->value = x;
    ((MI_DatetimeField*)&self->StatisticTime)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Clear_StatisticTime(
    Apache_HTTPDServerStatistics* self)
{
    memset((void*)&self->StatisticTime, 0, sizeof(self->StatisticTime));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Set_SampleInterval(
    Apache_HTTPDServerStatistics* self,
    MI_Datetime x)
{
    ((MI_DatetimeField*)&self->SampleInterval)->value = x;
    ((MI_DatetimeField*)&self->SampleInterval)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Clear_SampleInterval(
    Apache_HTTPDServerStatistics* self)
{
    memset((void*)&self->SampleInterval, 0, sizeof(self->SampleInterval));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Set_RequestsTotal(
    Apache_HTTPDServerStatistics* self,
    MI_Uint64 x)
{
    ((MI_Uint64Field*)&self->RequestsTotal)->value = x;
    ((MI_Uint64Field*)&self->RequestsTotal)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Clear_RequestsTotal(
    Apache_HTTPDServerStatistics* self)
{
    memset((void*)&self->RequestsTotal, 0, sizeof(self->RequestsTotal));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Set_RequestsPerSecond(
    Apache_HTTPDServerStatistics* self,
    MI_Uint32 x)
{
    ((MI_Uint32Field*)&self->RequestsPerSecond)->value = x;
    ((MI_Uint32Field*)&self->RequestsPerSecond)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Clear_RequestsPerSecond(
    Apache_HTTPDServerStatistics* self)
{
    memset((void*)&self->RequestsPerSecond, 0, sizeof(self->RequestsPerSecond));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Set_KBPerRequest(
    Apache_HTTPDServerStatistics* self,
    MI_Uint32 x)
{
    ((MI_Uint32Field*)&self->KBPerRequest)->value = x;
    ((MI_Uint32Field*)&self->KBPerRequest)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Clear_KBPerRequest(
    Apache_HTTPDServerStatistics* self)
{
    memset((void*)&self->KBPerRequest, 0, sizeof(self->KBPerRequest));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Set_KBPerSecond(
    Apache_HTTPDServerStatistics* self,
    MI_Uint32 x)
{
    ((MI_Uint32Field*)&self->KBPerSecond)->value = x;
    ((MI_Uint32Field*)&self->KBPerSecond)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Clear_KBPerSecond(
    Apache_HTTPDServerStatistics* self)
{
    memset((void*)&self->KBPerSecond, 0, sizeof(self->KBPerSecond));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Set_TotalPctCPU(
    Apache_HTTPDServerStatistics* self,
    MI_Uint32 x)
{
    ((MI_Uint32Field*)&self->TotalPctCPU)->value = x;
    ((MI_Uint32Field*)&self->TotalPctCPU)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Clear_TotalPctCPU(
    Apache_HTTPDServerStatistics* self)
{
    memset((void*)&self->TotalPctCPU, 0, sizeof(self->TotalPctCPU));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Set_IdleWorkers(
    Apache_HTTPDServerStatistics* self,
    MI_Uint32 x)
{
    ((MI_Uint32Field*)&self->IdleWorkers)->value = x;
    ((MI_Uint32Field*)&self->IdleWorkers)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Clear_IdleWorkers(
    Apache_HTTPDServerStatistics* self)
{
    memset((void*)&self->IdleWorkers, 0, sizeof(self->IdleWorkers));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Set_BusyWorkers(
    Apache_HTTPDServerStatistics* self,
    MI_Uint32 x)
{
    ((MI_Uint32Field*)&self->BusyWorkers)->value = x;
    ((MI_Uint32Field*)&self->BusyWorkers)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Clear_BusyWorkers(
    Apache_HTTPDServerStatistics* self)
{
    memset((void*)&self->BusyWorkers, 0, sizeof(self->BusyWorkers));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Set_PctBusyWorkers(
    Apache_HTTPDServerStatistics* self,
    MI_Uint32 x)
{
    ((MI_Uint32Field*)&self->PctBusyWorkers)->value = x;
    ((MI_Uint32Field*)&self->PctBusyWorkers)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Clear_PctBusyWorkers(
    Apache_HTTPDServerStatistics* self)
{
    memset((void*)&self->PctBusyWorkers, 0, sizeof(self->PctBusyWorkers));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Set_ConfigurationFile(
    Apache_HTTPDServerStatistics* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        15,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_SetPtr_ConfigurationFile(
    Apache_HTTPDServerStatistics* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        15,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_Clear_ConfigurationFile(
    Apache_HTTPDServerStatistics* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        15);
}

/*
**==============================================================================
**
** Apache_HTTPDServerStatistics.ResetSelectedStats()
**
**==============================================================================
*/

typedef struct _Apache_HTTPDServerStatistics_ResetSelectedStats
{
    MI_Instance __instance;
    /*OUT*/ MI_ConstUint32Field MIReturn;
    /*IN*/ MI_ConstStringAField SelectedStatistics;
}
Apache_HTTPDServerStatistics_ResetSelectedStats;

MI_EXTERN_C MI_CONST MI_MethodDecl Apache_HTTPDServerStatistics_ResetSelectedStats_rtti;

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_ResetSelectedStats_Construct(
    Apache_HTTPDServerStatistics_ResetSelectedStats* self,
    MI_Context* context)
{
    return MI_ConstructParameters(context, &Apache_HTTPDServerStatistics_ResetSelectedStats_rtti,
        (MI_Instance*)&self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_ResetSelectedStats_Clone(
    const Apache_HTTPDServerStatistics_ResetSelectedStats* self,
    Apache_HTTPDServerStatistics_ResetSelectedStats** newInstance)
{
    return MI_Instance_Clone(
        &self->__instance, (MI_Instance**)newInstance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_ResetSelectedStats_Destruct(
    Apache_HTTPDServerStatistics_ResetSelectedStats* self)
{
    return MI_Instance_Destruct(&self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_ResetSelectedStats_Delete(
    Apache_HTTPDServerStatistics_ResetSelectedStats* self)
{
    return MI_Instance_Delete(&self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_ResetSelectedStats_Post(
    const Apache_HTTPDServerStatistics_ResetSelectedStats* self,
    MI_Context* context)
{
    return MI_PostInstance(context, &self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_ResetSelectedStats_Set_MIReturn(
    Apache_HTTPDServerStatistics_ResetSelectedStats* self,
    MI_Uint32 x)
{
    ((MI_Uint32Field*)&self->MIReturn)->value = x;
    ((MI_Uint32Field*)&self->MIReturn)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_ResetSelectedStats_Clear_MIReturn(
    Apache_HTTPDServerStatistics_ResetSelectedStats* self)
{
    memset((void*)&self->MIReturn, 0, sizeof(self->MIReturn));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_ResetSelectedStats_Set_SelectedStatistics(
    Apache_HTTPDServerStatistics_ResetSelectedStats* self,
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

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_ResetSelectedStats_SetPtr_SelectedStatistics(
    Apache_HTTPDServerStatistics_ResetSelectedStats* self,
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

MI_INLINE MI_Result MI_CALL Apache_HTTPDServerStatistics_ResetSelectedStats_Clear_SelectedStatistics(
    Apache_HTTPDServerStatistics_ResetSelectedStats* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        1);
}

/*
**==============================================================================
**
** Apache_HTTPDServerStatistics provider function prototypes
**
**==============================================================================
*/

/* The developer may optionally define this structure */
typedef struct _Apache_HTTPDServerStatistics_Self Apache_HTTPDServerStatistics_Self;

MI_EXTERN_C void MI_CALL Apache_HTTPDServerStatistics_Load(
    Apache_HTTPDServerStatistics_Self** self,
    MI_Module_Self* selfModule,
    MI_Context* context);

MI_EXTERN_C void MI_CALL Apache_HTTPDServerStatistics_Unload(
    Apache_HTTPDServerStatistics_Self* self,
    MI_Context* context);

MI_EXTERN_C void MI_CALL Apache_HTTPDServerStatistics_EnumerateInstances(
    Apache_HTTPDServerStatistics_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const MI_PropertySet* propertySet,
    MI_Boolean keysOnly,
    const MI_Filter* filter);

MI_EXTERN_C void MI_CALL Apache_HTTPDServerStatistics_GetInstance(
    Apache_HTTPDServerStatistics_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDServerStatistics* instanceName,
    const MI_PropertySet* propertySet);

MI_EXTERN_C void MI_CALL Apache_HTTPDServerStatistics_CreateInstance(
    Apache_HTTPDServerStatistics_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDServerStatistics* newInstance);

MI_EXTERN_C void MI_CALL Apache_HTTPDServerStatistics_ModifyInstance(
    Apache_HTTPDServerStatistics_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDServerStatistics* modifiedInstance,
    const MI_PropertySet* propertySet);

MI_EXTERN_C void MI_CALL Apache_HTTPDServerStatistics_DeleteInstance(
    Apache_HTTPDServerStatistics_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDServerStatistics* instanceName);

MI_EXTERN_C void MI_CALL Apache_HTTPDServerStatistics_Invoke_ResetSelectedStats(
    Apache_HTTPDServerStatistics_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const MI_Char* methodName,
    const Apache_HTTPDServerStatistics* instanceName,
    const Apache_HTTPDServerStatistics_ResetSelectedStats* in);


/*
**==============================================================================
**
** Apache_HTTPDServerStatistics_Class
**
**==============================================================================
*/

#ifdef __cplusplus
# include <micxx/micxx.h>

MI_BEGIN_NAMESPACE

class Apache_HTTPDServerStatistics_Class : public CIM_StatisticalData_Class
{
public:
    
    typedef Apache_HTTPDServerStatistics Self;
    
    Apache_HTTPDServerStatistics_Class() :
        CIM_StatisticalData_Class(&Apache_HTTPDServerStatistics_rtti)
    {
    }
    
    Apache_HTTPDServerStatistics_Class(
        const Apache_HTTPDServerStatistics* instanceName,
        bool keysOnly) :
        CIM_StatisticalData_Class(
            &Apache_HTTPDServerStatistics_rtti,
            &instanceName->__instance,
            keysOnly)
    {
    }
    
    Apache_HTTPDServerStatistics_Class(
        const MI_ClassDecl* clDecl,
        const MI_Instance* instance,
        bool keysOnly) :
        CIM_StatisticalData_Class(clDecl, instance, keysOnly)
    {
    }
    
    Apache_HTTPDServerStatistics_Class(
        const MI_ClassDecl* clDecl) :
        CIM_StatisticalData_Class(clDecl)
    {
    }
    
    Apache_HTTPDServerStatistics_Class& operator=(
        const Apache_HTTPDServerStatistics_Class& x)
    {
        CopyRef(x);
        return *this;
    }
    
    Apache_HTTPDServerStatistics_Class(
        const Apache_HTTPDServerStatistics_Class& x) :
        CIM_StatisticalData_Class(x)
    {
    }

    static const MI_ClassDecl* GetClassDecl()
    {
        return &Apache_HTTPDServerStatistics_rtti;
    }

    //
    // Apache_HTTPDServerStatistics_Class.RequestsTotal
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
    // Apache_HTTPDServerStatistics_Class.RequestsPerSecond
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
    // Apache_HTTPDServerStatistics_Class.KBPerRequest
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
    // Apache_HTTPDServerStatistics_Class.KBPerSecond
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
    // Apache_HTTPDServerStatistics_Class.TotalPctCPU
    //
    
    const Field<Uint32>& TotalPctCPU() const
    {
        const size_t n = offsetof(Self, TotalPctCPU);
        return GetField<Uint32>(n);
    }
    
    void TotalPctCPU(const Field<Uint32>& x)
    {
        const size_t n = offsetof(Self, TotalPctCPU);
        GetField<Uint32>(n) = x;
    }
    
    const Uint32& TotalPctCPU_value() const
    {
        const size_t n = offsetof(Self, TotalPctCPU);
        return GetField<Uint32>(n).value;
    }
    
    void TotalPctCPU_value(const Uint32& x)
    {
        const size_t n = offsetof(Self, TotalPctCPU);
        GetField<Uint32>(n).Set(x);
    }
    
    bool TotalPctCPU_exists() const
    {
        const size_t n = offsetof(Self, TotalPctCPU);
        return GetField<Uint32>(n).exists ? true : false;
    }
    
    void TotalPctCPU_clear()
    {
        const size_t n = offsetof(Self, TotalPctCPU);
        GetField<Uint32>(n).Clear();
    }

    //
    // Apache_HTTPDServerStatistics_Class.IdleWorkers
    //
    
    const Field<Uint32>& IdleWorkers() const
    {
        const size_t n = offsetof(Self, IdleWorkers);
        return GetField<Uint32>(n);
    }
    
    void IdleWorkers(const Field<Uint32>& x)
    {
        const size_t n = offsetof(Self, IdleWorkers);
        GetField<Uint32>(n) = x;
    }
    
    const Uint32& IdleWorkers_value() const
    {
        const size_t n = offsetof(Self, IdleWorkers);
        return GetField<Uint32>(n).value;
    }
    
    void IdleWorkers_value(const Uint32& x)
    {
        const size_t n = offsetof(Self, IdleWorkers);
        GetField<Uint32>(n).Set(x);
    }
    
    bool IdleWorkers_exists() const
    {
        const size_t n = offsetof(Self, IdleWorkers);
        return GetField<Uint32>(n).exists ? true : false;
    }
    
    void IdleWorkers_clear()
    {
        const size_t n = offsetof(Self, IdleWorkers);
        GetField<Uint32>(n).Clear();
    }

    //
    // Apache_HTTPDServerStatistics_Class.BusyWorkers
    //
    
    const Field<Uint32>& BusyWorkers() const
    {
        const size_t n = offsetof(Self, BusyWorkers);
        return GetField<Uint32>(n);
    }
    
    void BusyWorkers(const Field<Uint32>& x)
    {
        const size_t n = offsetof(Self, BusyWorkers);
        GetField<Uint32>(n) = x;
    }
    
    const Uint32& BusyWorkers_value() const
    {
        const size_t n = offsetof(Self, BusyWorkers);
        return GetField<Uint32>(n).value;
    }
    
    void BusyWorkers_value(const Uint32& x)
    {
        const size_t n = offsetof(Self, BusyWorkers);
        GetField<Uint32>(n).Set(x);
    }
    
    bool BusyWorkers_exists() const
    {
        const size_t n = offsetof(Self, BusyWorkers);
        return GetField<Uint32>(n).exists ? true : false;
    }
    
    void BusyWorkers_clear()
    {
        const size_t n = offsetof(Self, BusyWorkers);
        GetField<Uint32>(n).Clear();
    }

    //
    // Apache_HTTPDServerStatistics_Class.PctBusyWorkers
    //
    
    const Field<Uint32>& PctBusyWorkers() const
    {
        const size_t n = offsetof(Self, PctBusyWorkers);
        return GetField<Uint32>(n);
    }
    
    void PctBusyWorkers(const Field<Uint32>& x)
    {
        const size_t n = offsetof(Self, PctBusyWorkers);
        GetField<Uint32>(n) = x;
    }
    
    const Uint32& PctBusyWorkers_value() const
    {
        const size_t n = offsetof(Self, PctBusyWorkers);
        return GetField<Uint32>(n).value;
    }
    
    void PctBusyWorkers_value(const Uint32& x)
    {
        const size_t n = offsetof(Self, PctBusyWorkers);
        GetField<Uint32>(n).Set(x);
    }
    
    bool PctBusyWorkers_exists() const
    {
        const size_t n = offsetof(Self, PctBusyWorkers);
        return GetField<Uint32>(n).exists ? true : false;
    }
    
    void PctBusyWorkers_clear()
    {
        const size_t n = offsetof(Self, PctBusyWorkers);
        GetField<Uint32>(n).Clear();
    }

    //
    // Apache_HTTPDServerStatistics_Class.ConfigurationFile
    //
    
    const Field<String>& ConfigurationFile() const
    {
        const size_t n = offsetof(Self, ConfigurationFile);
        return GetField<String>(n);
    }
    
    void ConfigurationFile(const Field<String>& x)
    {
        const size_t n = offsetof(Self, ConfigurationFile);
        GetField<String>(n) = x;
    }
    
    const String& ConfigurationFile_value() const
    {
        const size_t n = offsetof(Self, ConfigurationFile);
        return GetField<String>(n).value;
    }
    
    void ConfigurationFile_value(const String& x)
    {
        const size_t n = offsetof(Self, ConfigurationFile);
        GetField<String>(n).Set(x);
    }
    
    bool ConfigurationFile_exists() const
    {
        const size_t n = offsetof(Self, ConfigurationFile);
        return GetField<String>(n).exists ? true : false;
    }
    
    void ConfigurationFile_clear()
    {
        const size_t n = offsetof(Self, ConfigurationFile);
        GetField<String>(n).Clear();
    }
};

typedef Array<Apache_HTTPDServerStatistics_Class> Apache_HTTPDServerStatistics_ClassA;

class Apache_HTTPDServerStatistics_ResetSelectedStats_Class : public Instance
{
public:
    
    typedef Apache_HTTPDServerStatistics_ResetSelectedStats Self;
    
    Apache_HTTPDServerStatistics_ResetSelectedStats_Class() :
        Instance(&Apache_HTTPDServerStatistics_ResetSelectedStats_rtti)
    {
    }
    
    Apache_HTTPDServerStatistics_ResetSelectedStats_Class(
        const Apache_HTTPDServerStatistics_ResetSelectedStats* instanceName,
        bool keysOnly) :
        Instance(
            &Apache_HTTPDServerStatistics_ResetSelectedStats_rtti,
            &instanceName->__instance,
            keysOnly)
    {
    }
    
    Apache_HTTPDServerStatistics_ResetSelectedStats_Class(
        const MI_ClassDecl* clDecl,
        const MI_Instance* instance,
        bool keysOnly) :
        Instance(clDecl, instance, keysOnly)
    {
    }
    
    Apache_HTTPDServerStatistics_ResetSelectedStats_Class(
        const MI_ClassDecl* clDecl) :
        Instance(clDecl)
    {
    }
    
    Apache_HTTPDServerStatistics_ResetSelectedStats_Class& operator=(
        const Apache_HTTPDServerStatistics_ResetSelectedStats_Class& x)
    {
        CopyRef(x);
        return *this;
    }
    
    Apache_HTTPDServerStatistics_ResetSelectedStats_Class(
        const Apache_HTTPDServerStatistics_ResetSelectedStats_Class& x) :
        Instance(x)
    {
    }

    //
    // Apache_HTTPDServerStatistics_ResetSelectedStats_Class.MIReturn
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
    // Apache_HTTPDServerStatistics_ResetSelectedStats_Class.SelectedStatistics
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

typedef Array<Apache_HTTPDServerStatistics_ResetSelectedStats_Class> Apache_HTTPDServerStatistics_ResetSelectedStats_ClassA;

MI_END_NAMESPACE

#endif /* __cplusplus */

#endif /* _Apache_HTTPDServerStatistics_h */
