/* @migen@ */
/*
**==============================================================================
**
** WARNING: THIS FILE WAS AUTOMATICALLY GENERATED. PLEASE DO NOT EDIT.
**
**==============================================================================
*/
#ifndef _Apache_HTTPDProcess_h
#define _Apache_HTTPDProcess_h

#include <MI.h>
#include "CIM_Process.h"
#include "CIM_ConcreteJob.h"

/*
**==============================================================================
**
** Apache_HTTPDProcess [Apache_HTTPDProcess]
**
** Keys:
**    CSCreationClassName
**    CSName
**    OSCreationClassName
**    OSName
**    CreationClassName
**    Handle
**
**==============================================================================
*/

typedef struct _Apache_HTTPDProcess /* extends CIM_Process */
{
    MI_Instance __instance;
    /* CIM_ManagedElement properties */
    MI_ConstStringField InstanceID;
    MI_ConstStringField Caption;
    MI_ConstStringField Description;
    MI_ConstStringField ElementName;
    /* CIM_ManagedSystemElement properties */
    MI_ConstDatetimeField InstallDate;
    MI_ConstStringField Name;
    MI_ConstUint16AField OperationalStatus;
    MI_ConstStringAField StatusDescriptions;
    MI_ConstStringField Status;
    MI_ConstUint16Field HealthState;
    MI_ConstUint16Field CommunicationStatus;
    MI_ConstUint16Field DetailedStatus;
    MI_ConstUint16Field OperatingStatus;
    MI_ConstUint16Field PrimaryStatus;
    /* CIM_LogicalElement properties */
    /* CIM_EnabledLogicalElement properties */
    MI_ConstUint16Field EnabledState;
    MI_ConstStringField OtherEnabledState;
    MI_ConstUint16Field RequestedState;
    MI_ConstUint16Field EnabledDefault;
    MI_ConstDatetimeField TimeOfLastStateChange;
    MI_ConstUint16AField AvailableRequestedStates;
    MI_ConstUint16Field TransitioningToState;
    /* CIM_Process properties */
    /*KEY*/ MI_ConstStringField CSCreationClassName;
    /*KEY*/ MI_ConstStringField CSName;
    /*KEY*/ MI_ConstStringField OSCreationClassName;
    /*KEY*/ MI_ConstStringField OSName;
    /*KEY*/ MI_ConstStringField CreationClassName;
    /*KEY*/ MI_ConstStringField Handle;
    MI_ConstUint32Field Priority;
    MI_ConstUint16Field ExecutionState;
    MI_ConstStringField OtherExecutionDescription;
    MI_ConstDatetimeField CreationDate;
    MI_ConstDatetimeField TerminationDate;
    MI_ConstUint64Field KernelModeTime;
    MI_ConstUint64Field UserModeTime;
    MI_ConstUint64Field WorkingSetSize;
    /* Apache_HTTPDProcess properties */
    MI_ConstUint16Field PercentUserTime;
    MI_ConstUint32Field WorkingSetSizeMB;
}
Apache_HTTPDProcess;

typedef struct _Apache_HTTPDProcess_Ref
{
    Apache_HTTPDProcess* value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
Apache_HTTPDProcess_Ref;

typedef struct _Apache_HTTPDProcess_ConstRef
{
    MI_CONST Apache_HTTPDProcess* value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
Apache_HTTPDProcess_ConstRef;

typedef struct _Apache_HTTPDProcess_Array
{
    struct _Apache_HTTPDProcess** data;
    MI_Uint32 size;
}
Apache_HTTPDProcess_Array;

typedef struct _Apache_HTTPDProcess_ConstArray
{
    struct _Apache_HTTPDProcess MI_CONST* MI_CONST* data;
    MI_Uint32 size;
}
Apache_HTTPDProcess_ConstArray;

typedef struct _Apache_HTTPDProcess_ArrayRef
{
    Apache_HTTPDProcess_Array value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
Apache_HTTPDProcess_ArrayRef;

typedef struct _Apache_HTTPDProcess_ConstArrayRef
{
    Apache_HTTPDProcess_ConstArray value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
Apache_HTTPDProcess_ConstArrayRef;

MI_EXTERN_C MI_CONST MI_ClassDecl Apache_HTTPDProcess_rtti;

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Construct(
    Apache_HTTPDProcess* self,
    MI_Context* context)
{
    return MI_ConstructInstance(context, &Apache_HTTPDProcess_rtti,
        (MI_Instance*)&self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clone(
    const Apache_HTTPDProcess* self,
    Apache_HTTPDProcess** newInstance)
{
    return MI_Instance_Clone(
        &self->__instance, (MI_Instance**)newInstance);
}

MI_INLINE MI_Boolean MI_CALL Apache_HTTPDProcess_IsA(
    const MI_Instance* self)
{
    MI_Boolean res = MI_FALSE;
    return MI_Instance_IsA(self, &Apache_HTTPDProcess_rtti, &res) == MI_RESULT_OK && res;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Destruct(Apache_HTTPDProcess* self)
{
    return MI_Instance_Destruct(&self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Delete(Apache_HTTPDProcess* self)
{
    return MI_Instance_Delete(&self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Post(
    const Apache_HTTPDProcess* self,
    MI_Context* context)
{
    return MI_PostInstance(context, &self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_InstanceID(
    Apache_HTTPDProcess* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        0,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_SetPtr_InstanceID(
    Apache_HTTPDProcess* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        0,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_InstanceID(
    Apache_HTTPDProcess* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_Caption(
    Apache_HTTPDProcess* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        1,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_SetPtr_Caption(
    Apache_HTTPDProcess* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        1,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_Caption(
    Apache_HTTPDProcess* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        1);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_Description(
    Apache_HTTPDProcess* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        2,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_SetPtr_Description(
    Apache_HTTPDProcess* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        2,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_Description(
    Apache_HTTPDProcess* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        2);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_ElementName(
    Apache_HTTPDProcess* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        3,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_SetPtr_ElementName(
    Apache_HTTPDProcess* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        3,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_ElementName(
    Apache_HTTPDProcess* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        3);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_InstallDate(
    Apache_HTTPDProcess* self,
    MI_Datetime x)
{
    ((MI_DatetimeField*)&self->InstallDate)->value = x;
    ((MI_DatetimeField*)&self->InstallDate)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_InstallDate(
    Apache_HTTPDProcess* self)
{
    memset((void*)&self->InstallDate, 0, sizeof(self->InstallDate));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_Name(
    Apache_HTTPDProcess* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        5,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_SetPtr_Name(
    Apache_HTTPDProcess* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        5,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_Name(
    Apache_HTTPDProcess* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        5);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_OperationalStatus(
    Apache_HTTPDProcess* self,
    const MI_Uint16* data,
    MI_Uint32 size)
{
    MI_Array arr;
    arr.data = (void*)data;
    arr.size = size;
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        6,
        (MI_Value*)&arr,
        MI_UINT16A,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_SetPtr_OperationalStatus(
    Apache_HTTPDProcess* self,
    const MI_Uint16* data,
    MI_Uint32 size)
{
    MI_Array arr;
    arr.data = (void*)data;
    arr.size = size;
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        6,
        (MI_Value*)&arr,
        MI_UINT16A,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_OperationalStatus(
    Apache_HTTPDProcess* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        6);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_StatusDescriptions(
    Apache_HTTPDProcess* self,
    const MI_Char** data,
    MI_Uint32 size)
{
    MI_Array arr;
    arr.data = (void*)data;
    arr.size = size;
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        7,
        (MI_Value*)&arr,
        MI_STRINGA,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_SetPtr_StatusDescriptions(
    Apache_HTTPDProcess* self,
    const MI_Char** data,
    MI_Uint32 size)
{
    MI_Array arr;
    arr.data = (void*)data;
    arr.size = size;
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        7,
        (MI_Value*)&arr,
        MI_STRINGA,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_StatusDescriptions(
    Apache_HTTPDProcess* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        7);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_Status(
    Apache_HTTPDProcess* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        8,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_SetPtr_Status(
    Apache_HTTPDProcess* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        8,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_Status(
    Apache_HTTPDProcess* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        8);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_HealthState(
    Apache_HTTPDProcess* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->HealthState)->value = x;
    ((MI_Uint16Field*)&self->HealthState)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_HealthState(
    Apache_HTTPDProcess* self)
{
    memset((void*)&self->HealthState, 0, sizeof(self->HealthState));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_CommunicationStatus(
    Apache_HTTPDProcess* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->CommunicationStatus)->value = x;
    ((MI_Uint16Field*)&self->CommunicationStatus)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_CommunicationStatus(
    Apache_HTTPDProcess* self)
{
    memset((void*)&self->CommunicationStatus, 0, sizeof(self->CommunicationStatus));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_DetailedStatus(
    Apache_HTTPDProcess* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->DetailedStatus)->value = x;
    ((MI_Uint16Field*)&self->DetailedStatus)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_DetailedStatus(
    Apache_HTTPDProcess* self)
{
    memset((void*)&self->DetailedStatus, 0, sizeof(self->DetailedStatus));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_OperatingStatus(
    Apache_HTTPDProcess* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->OperatingStatus)->value = x;
    ((MI_Uint16Field*)&self->OperatingStatus)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_OperatingStatus(
    Apache_HTTPDProcess* self)
{
    memset((void*)&self->OperatingStatus, 0, sizeof(self->OperatingStatus));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_PrimaryStatus(
    Apache_HTTPDProcess* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->PrimaryStatus)->value = x;
    ((MI_Uint16Field*)&self->PrimaryStatus)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_PrimaryStatus(
    Apache_HTTPDProcess* self)
{
    memset((void*)&self->PrimaryStatus, 0, sizeof(self->PrimaryStatus));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_EnabledState(
    Apache_HTTPDProcess* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->EnabledState)->value = x;
    ((MI_Uint16Field*)&self->EnabledState)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_EnabledState(
    Apache_HTTPDProcess* self)
{
    memset((void*)&self->EnabledState, 0, sizeof(self->EnabledState));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_OtherEnabledState(
    Apache_HTTPDProcess* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        15,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_SetPtr_OtherEnabledState(
    Apache_HTTPDProcess* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        15,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_OtherEnabledState(
    Apache_HTTPDProcess* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        15);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_RequestedState(
    Apache_HTTPDProcess* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->RequestedState)->value = x;
    ((MI_Uint16Field*)&self->RequestedState)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_RequestedState(
    Apache_HTTPDProcess* self)
{
    memset((void*)&self->RequestedState, 0, sizeof(self->RequestedState));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_EnabledDefault(
    Apache_HTTPDProcess* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->EnabledDefault)->value = x;
    ((MI_Uint16Field*)&self->EnabledDefault)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_EnabledDefault(
    Apache_HTTPDProcess* self)
{
    memset((void*)&self->EnabledDefault, 0, sizeof(self->EnabledDefault));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_TimeOfLastStateChange(
    Apache_HTTPDProcess* self,
    MI_Datetime x)
{
    ((MI_DatetimeField*)&self->TimeOfLastStateChange)->value = x;
    ((MI_DatetimeField*)&self->TimeOfLastStateChange)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_TimeOfLastStateChange(
    Apache_HTTPDProcess* self)
{
    memset((void*)&self->TimeOfLastStateChange, 0, sizeof(self->TimeOfLastStateChange));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_AvailableRequestedStates(
    Apache_HTTPDProcess* self,
    const MI_Uint16* data,
    MI_Uint32 size)
{
    MI_Array arr;
    arr.data = (void*)data;
    arr.size = size;
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        19,
        (MI_Value*)&arr,
        MI_UINT16A,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_SetPtr_AvailableRequestedStates(
    Apache_HTTPDProcess* self,
    const MI_Uint16* data,
    MI_Uint32 size)
{
    MI_Array arr;
    arr.data = (void*)data;
    arr.size = size;
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        19,
        (MI_Value*)&arr,
        MI_UINT16A,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_AvailableRequestedStates(
    Apache_HTTPDProcess* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        19);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_TransitioningToState(
    Apache_HTTPDProcess* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->TransitioningToState)->value = x;
    ((MI_Uint16Field*)&self->TransitioningToState)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_TransitioningToState(
    Apache_HTTPDProcess* self)
{
    memset((void*)&self->TransitioningToState, 0, sizeof(self->TransitioningToState));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_CSCreationClassName(
    Apache_HTTPDProcess* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        21,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_SetPtr_CSCreationClassName(
    Apache_HTTPDProcess* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        21,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_CSCreationClassName(
    Apache_HTTPDProcess* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        21);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_CSName(
    Apache_HTTPDProcess* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        22,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_SetPtr_CSName(
    Apache_HTTPDProcess* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        22,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_CSName(
    Apache_HTTPDProcess* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        22);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_OSCreationClassName(
    Apache_HTTPDProcess* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        23,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_SetPtr_OSCreationClassName(
    Apache_HTTPDProcess* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        23,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_OSCreationClassName(
    Apache_HTTPDProcess* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        23);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_OSName(
    Apache_HTTPDProcess* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        24,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_SetPtr_OSName(
    Apache_HTTPDProcess* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        24,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_OSName(
    Apache_HTTPDProcess* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        24);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_CreationClassName(
    Apache_HTTPDProcess* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        25,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_SetPtr_CreationClassName(
    Apache_HTTPDProcess* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        25,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_CreationClassName(
    Apache_HTTPDProcess* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        25);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_Handle(
    Apache_HTTPDProcess* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        26,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_SetPtr_Handle(
    Apache_HTTPDProcess* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        26,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_Handle(
    Apache_HTTPDProcess* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        26);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_Priority(
    Apache_HTTPDProcess* self,
    MI_Uint32 x)
{
    ((MI_Uint32Field*)&self->Priority)->value = x;
    ((MI_Uint32Field*)&self->Priority)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_Priority(
    Apache_HTTPDProcess* self)
{
    memset((void*)&self->Priority, 0, sizeof(self->Priority));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_ExecutionState(
    Apache_HTTPDProcess* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->ExecutionState)->value = x;
    ((MI_Uint16Field*)&self->ExecutionState)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_ExecutionState(
    Apache_HTTPDProcess* self)
{
    memset((void*)&self->ExecutionState, 0, sizeof(self->ExecutionState));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_OtherExecutionDescription(
    Apache_HTTPDProcess* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        29,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_SetPtr_OtherExecutionDescription(
    Apache_HTTPDProcess* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        29,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_OtherExecutionDescription(
    Apache_HTTPDProcess* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        29);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_CreationDate(
    Apache_HTTPDProcess* self,
    MI_Datetime x)
{
    ((MI_DatetimeField*)&self->CreationDate)->value = x;
    ((MI_DatetimeField*)&self->CreationDate)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_CreationDate(
    Apache_HTTPDProcess* self)
{
    memset((void*)&self->CreationDate, 0, sizeof(self->CreationDate));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_TerminationDate(
    Apache_HTTPDProcess* self,
    MI_Datetime x)
{
    ((MI_DatetimeField*)&self->TerminationDate)->value = x;
    ((MI_DatetimeField*)&self->TerminationDate)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_TerminationDate(
    Apache_HTTPDProcess* self)
{
    memset((void*)&self->TerminationDate, 0, sizeof(self->TerminationDate));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_KernelModeTime(
    Apache_HTTPDProcess* self,
    MI_Uint64 x)
{
    ((MI_Uint64Field*)&self->KernelModeTime)->value = x;
    ((MI_Uint64Field*)&self->KernelModeTime)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_KernelModeTime(
    Apache_HTTPDProcess* self)
{
    memset((void*)&self->KernelModeTime, 0, sizeof(self->KernelModeTime));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_UserModeTime(
    Apache_HTTPDProcess* self,
    MI_Uint64 x)
{
    ((MI_Uint64Field*)&self->UserModeTime)->value = x;
    ((MI_Uint64Field*)&self->UserModeTime)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_UserModeTime(
    Apache_HTTPDProcess* self)
{
    memset((void*)&self->UserModeTime, 0, sizeof(self->UserModeTime));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_WorkingSetSize(
    Apache_HTTPDProcess* self,
    MI_Uint64 x)
{
    ((MI_Uint64Field*)&self->WorkingSetSize)->value = x;
    ((MI_Uint64Field*)&self->WorkingSetSize)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_WorkingSetSize(
    Apache_HTTPDProcess* self)
{
    memset((void*)&self->WorkingSetSize, 0, sizeof(self->WorkingSetSize));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_PercentUserTime(
    Apache_HTTPDProcess* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->PercentUserTime)->value = x;
    ((MI_Uint16Field*)&self->PercentUserTime)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_PercentUserTime(
    Apache_HTTPDProcess* self)
{
    memset((void*)&self->PercentUserTime, 0, sizeof(self->PercentUserTime));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Set_WorkingSetSizeMB(
    Apache_HTTPDProcess* self,
    MI_Uint32 x)
{
    ((MI_Uint32Field*)&self->WorkingSetSizeMB)->value = x;
    ((MI_Uint32Field*)&self->WorkingSetSizeMB)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_Clear_WorkingSetSizeMB(
    Apache_HTTPDProcess* self)
{
    memset((void*)&self->WorkingSetSizeMB, 0, sizeof(self->WorkingSetSizeMB));
    return MI_RESULT_OK;
}

/*
**==============================================================================
**
** Apache_HTTPDProcess.RequestStateChange()
**
**==============================================================================
*/

typedef struct _Apache_HTTPDProcess_RequestStateChange
{
    MI_Instance __instance;
    /*OUT*/ MI_ConstUint32Field MIReturn;
    /*IN*/ MI_ConstUint16Field RequestedState;
    /*OUT*/ CIM_ConcreteJob_ConstRef Job;
    /*IN*/ MI_ConstDatetimeField TimeoutPeriod;
}
Apache_HTTPDProcess_RequestStateChange;

MI_EXTERN_C MI_CONST MI_MethodDecl Apache_HTTPDProcess_RequestStateChange_rtti;

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_RequestStateChange_Construct(
    Apache_HTTPDProcess_RequestStateChange* self,
    MI_Context* context)
{
    return MI_ConstructParameters(context, &Apache_HTTPDProcess_RequestStateChange_rtti,
        (MI_Instance*)&self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_RequestStateChange_Clone(
    const Apache_HTTPDProcess_RequestStateChange* self,
    Apache_HTTPDProcess_RequestStateChange** newInstance)
{
    return MI_Instance_Clone(
        &self->__instance, (MI_Instance**)newInstance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_RequestStateChange_Destruct(
    Apache_HTTPDProcess_RequestStateChange* self)
{
    return MI_Instance_Destruct(&self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_RequestStateChange_Delete(
    Apache_HTTPDProcess_RequestStateChange* self)
{
    return MI_Instance_Delete(&self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_RequestStateChange_Post(
    const Apache_HTTPDProcess_RequestStateChange* self,
    MI_Context* context)
{
    return MI_PostInstance(context, &self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_RequestStateChange_Set_MIReturn(
    Apache_HTTPDProcess_RequestStateChange* self,
    MI_Uint32 x)
{
    ((MI_Uint32Field*)&self->MIReturn)->value = x;
    ((MI_Uint32Field*)&self->MIReturn)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_RequestStateChange_Clear_MIReturn(
    Apache_HTTPDProcess_RequestStateChange* self)
{
    memset((void*)&self->MIReturn, 0, sizeof(self->MIReturn));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_RequestStateChange_Set_RequestedState(
    Apache_HTTPDProcess_RequestStateChange* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->RequestedState)->value = x;
    ((MI_Uint16Field*)&self->RequestedState)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_RequestStateChange_Clear_RequestedState(
    Apache_HTTPDProcess_RequestStateChange* self)
{
    memset((void*)&self->RequestedState, 0, sizeof(self->RequestedState));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_RequestStateChange_Set_Job(
    Apache_HTTPDProcess_RequestStateChange* self,
    const CIM_ConcreteJob* x)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        2,
        (MI_Value*)&x,
        MI_REFERENCE,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_RequestStateChange_SetPtr_Job(
    Apache_HTTPDProcess_RequestStateChange* self,
    const CIM_ConcreteJob* x)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        2,
        (MI_Value*)&x,
        MI_REFERENCE,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_RequestStateChange_Clear_Job(
    Apache_HTTPDProcess_RequestStateChange* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        2);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_RequestStateChange_Set_TimeoutPeriod(
    Apache_HTTPDProcess_RequestStateChange* self,
    MI_Datetime x)
{
    ((MI_DatetimeField*)&self->TimeoutPeriod)->value = x;
    ((MI_DatetimeField*)&self->TimeoutPeriod)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDProcess_RequestStateChange_Clear_TimeoutPeriod(
    Apache_HTTPDProcess_RequestStateChange* self)
{
    memset((void*)&self->TimeoutPeriod, 0, sizeof(self->TimeoutPeriod));
    return MI_RESULT_OK;
}

/*
**==============================================================================
**
** Apache_HTTPDProcess provider function prototypes
**
**==============================================================================
*/

/* The developer may optionally define this structure */
typedef struct _Apache_HTTPDProcess_Self Apache_HTTPDProcess_Self;

MI_EXTERN_C void MI_CALL Apache_HTTPDProcess_Load(
    Apache_HTTPDProcess_Self** self,
    MI_Module_Self* selfModule,
    MI_Context* context);

MI_EXTERN_C void MI_CALL Apache_HTTPDProcess_Unload(
    Apache_HTTPDProcess_Self* self,
    MI_Context* context);

MI_EXTERN_C void MI_CALL Apache_HTTPDProcess_EnumerateInstances(
    Apache_HTTPDProcess_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const MI_PropertySet* propertySet,
    MI_Boolean keysOnly,
    const MI_Filter* filter);

MI_EXTERN_C void MI_CALL Apache_HTTPDProcess_GetInstance(
    Apache_HTTPDProcess_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDProcess* instanceName,
    const MI_PropertySet* propertySet);

MI_EXTERN_C void MI_CALL Apache_HTTPDProcess_CreateInstance(
    Apache_HTTPDProcess_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDProcess* newInstance);

MI_EXTERN_C void MI_CALL Apache_HTTPDProcess_ModifyInstance(
    Apache_HTTPDProcess_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDProcess* modifiedInstance,
    const MI_PropertySet* propertySet);

MI_EXTERN_C void MI_CALL Apache_HTTPDProcess_DeleteInstance(
    Apache_HTTPDProcess_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDProcess* instanceName);

MI_EXTERN_C void MI_CALL Apache_HTTPDProcess_Invoke_RequestStateChange(
    Apache_HTTPDProcess_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const MI_Char* methodName,
    const Apache_HTTPDProcess* instanceName,
    const Apache_HTTPDProcess_RequestStateChange* in);


/*
**==============================================================================
**
** Apache_HTTPDProcess_Class
**
**==============================================================================
*/

#ifdef __cplusplus
# include <micxx/micxx.h>

MI_BEGIN_NAMESPACE

class Apache_HTTPDProcess_Class : public CIM_Process_Class
{
public:
    
    typedef Apache_HTTPDProcess Self;
    
    Apache_HTTPDProcess_Class() :
        CIM_Process_Class(&Apache_HTTPDProcess_rtti)
    {
    }
    
    Apache_HTTPDProcess_Class(
        const Apache_HTTPDProcess* instanceName,
        bool keysOnly) :
        CIM_Process_Class(
            &Apache_HTTPDProcess_rtti,
            &instanceName->__instance,
            keysOnly)
    {
    }
    
    Apache_HTTPDProcess_Class(
        const MI_ClassDecl* clDecl,
        const MI_Instance* instance,
        bool keysOnly) :
        CIM_Process_Class(clDecl, instance, keysOnly)
    {
    }
    
    Apache_HTTPDProcess_Class(
        const MI_ClassDecl* clDecl) :
        CIM_Process_Class(clDecl)
    {
    }
    
    Apache_HTTPDProcess_Class& operator=(
        const Apache_HTTPDProcess_Class& x)
    {
        CopyRef(x);
        return *this;
    }
    
    Apache_HTTPDProcess_Class(
        const Apache_HTTPDProcess_Class& x) :
        CIM_Process_Class(x)
    {
    }

    static const MI_ClassDecl* GetClassDecl()
    {
        return &Apache_HTTPDProcess_rtti;
    }

    //
    // Apache_HTTPDProcess_Class.PercentUserTime
    //
    
    const Field<Uint16>& PercentUserTime() const
    {
        const size_t n = offsetof(Self, PercentUserTime);
        return GetField<Uint16>(n);
    }
    
    void PercentUserTime(const Field<Uint16>& x)
    {
        const size_t n = offsetof(Self, PercentUserTime);
        GetField<Uint16>(n) = x;
    }
    
    const Uint16& PercentUserTime_value() const
    {
        const size_t n = offsetof(Self, PercentUserTime);
        return GetField<Uint16>(n).value;
    }
    
    void PercentUserTime_value(const Uint16& x)
    {
        const size_t n = offsetof(Self, PercentUserTime);
        GetField<Uint16>(n).Set(x);
    }
    
    bool PercentUserTime_exists() const
    {
        const size_t n = offsetof(Self, PercentUserTime);
        return GetField<Uint16>(n).exists ? true : false;
    }
    
    void PercentUserTime_clear()
    {
        const size_t n = offsetof(Self, PercentUserTime);
        GetField<Uint16>(n).Clear();
    }

    //
    // Apache_HTTPDProcess_Class.WorkingSetSizeMB
    //
    
    const Field<Uint32>& WorkingSetSizeMB() const
    {
        const size_t n = offsetof(Self, WorkingSetSizeMB);
        return GetField<Uint32>(n);
    }
    
    void WorkingSetSizeMB(const Field<Uint32>& x)
    {
        const size_t n = offsetof(Self, WorkingSetSizeMB);
        GetField<Uint32>(n) = x;
    }
    
    const Uint32& WorkingSetSizeMB_value() const
    {
        const size_t n = offsetof(Self, WorkingSetSizeMB);
        return GetField<Uint32>(n).value;
    }
    
    void WorkingSetSizeMB_value(const Uint32& x)
    {
        const size_t n = offsetof(Self, WorkingSetSizeMB);
        GetField<Uint32>(n).Set(x);
    }
    
    bool WorkingSetSizeMB_exists() const
    {
        const size_t n = offsetof(Self, WorkingSetSizeMB);
        return GetField<Uint32>(n).exists ? true : false;
    }
    
    void WorkingSetSizeMB_clear()
    {
        const size_t n = offsetof(Self, WorkingSetSizeMB);
        GetField<Uint32>(n).Clear();
    }
};

typedef Array<Apache_HTTPDProcess_Class> Apache_HTTPDProcess_ClassA;

class Apache_HTTPDProcess_RequestStateChange_Class : public Instance
{
public:
    
    typedef Apache_HTTPDProcess_RequestStateChange Self;
    
    Apache_HTTPDProcess_RequestStateChange_Class() :
        Instance(&Apache_HTTPDProcess_RequestStateChange_rtti)
    {
    }
    
    Apache_HTTPDProcess_RequestStateChange_Class(
        const Apache_HTTPDProcess_RequestStateChange* instanceName,
        bool keysOnly) :
        Instance(
            &Apache_HTTPDProcess_RequestStateChange_rtti,
            &instanceName->__instance,
            keysOnly)
    {
    }
    
    Apache_HTTPDProcess_RequestStateChange_Class(
        const MI_ClassDecl* clDecl,
        const MI_Instance* instance,
        bool keysOnly) :
        Instance(clDecl, instance, keysOnly)
    {
    }
    
    Apache_HTTPDProcess_RequestStateChange_Class(
        const MI_ClassDecl* clDecl) :
        Instance(clDecl)
    {
    }
    
    Apache_HTTPDProcess_RequestStateChange_Class& operator=(
        const Apache_HTTPDProcess_RequestStateChange_Class& x)
    {
        CopyRef(x);
        return *this;
    }
    
    Apache_HTTPDProcess_RequestStateChange_Class(
        const Apache_HTTPDProcess_RequestStateChange_Class& x) :
        Instance(x)
    {
    }

    //
    // Apache_HTTPDProcess_RequestStateChange_Class.MIReturn
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
    // Apache_HTTPDProcess_RequestStateChange_Class.RequestedState
    //
    
    const Field<Uint16>& RequestedState() const
    {
        const size_t n = offsetof(Self, RequestedState);
        return GetField<Uint16>(n);
    }
    
    void RequestedState(const Field<Uint16>& x)
    {
        const size_t n = offsetof(Self, RequestedState);
        GetField<Uint16>(n) = x;
    }
    
    const Uint16& RequestedState_value() const
    {
        const size_t n = offsetof(Self, RequestedState);
        return GetField<Uint16>(n).value;
    }
    
    void RequestedState_value(const Uint16& x)
    {
        const size_t n = offsetof(Self, RequestedState);
        GetField<Uint16>(n).Set(x);
    }
    
    bool RequestedState_exists() const
    {
        const size_t n = offsetof(Self, RequestedState);
        return GetField<Uint16>(n).exists ? true : false;
    }
    
    void RequestedState_clear()
    {
        const size_t n = offsetof(Self, RequestedState);
        GetField<Uint16>(n).Clear();
    }

    //
    // Apache_HTTPDProcess_RequestStateChange_Class.Job
    //
    
    const Field<CIM_ConcreteJob_Class>& Job() const
    {
        const size_t n = offsetof(Self, Job);
        return GetField<CIM_ConcreteJob_Class>(n);
    }
    
    void Job(const Field<CIM_ConcreteJob_Class>& x)
    {
        const size_t n = offsetof(Self, Job);
        GetField<CIM_ConcreteJob_Class>(n) = x;
    }
    
    const CIM_ConcreteJob_Class& Job_value() const
    {
        const size_t n = offsetof(Self, Job);
        return GetField<CIM_ConcreteJob_Class>(n).value;
    }
    
    void Job_value(const CIM_ConcreteJob_Class& x)
    {
        const size_t n = offsetof(Self, Job);
        GetField<CIM_ConcreteJob_Class>(n).Set(x);
    }
    
    bool Job_exists() const
    {
        const size_t n = offsetof(Self, Job);
        return GetField<CIM_ConcreteJob_Class>(n).exists ? true : false;
    }
    
    void Job_clear()
    {
        const size_t n = offsetof(Self, Job);
        GetField<CIM_ConcreteJob_Class>(n).Clear();
    }

    //
    // Apache_HTTPDProcess_RequestStateChange_Class.TimeoutPeriod
    //
    
    const Field<Datetime>& TimeoutPeriod() const
    {
        const size_t n = offsetof(Self, TimeoutPeriod);
        return GetField<Datetime>(n);
    }
    
    void TimeoutPeriod(const Field<Datetime>& x)
    {
        const size_t n = offsetof(Self, TimeoutPeriod);
        GetField<Datetime>(n) = x;
    }
    
    const Datetime& TimeoutPeriod_value() const
    {
        const size_t n = offsetof(Self, TimeoutPeriod);
        return GetField<Datetime>(n).value;
    }
    
    void TimeoutPeriod_value(const Datetime& x)
    {
        const size_t n = offsetof(Self, TimeoutPeriod);
        GetField<Datetime>(n).Set(x);
    }
    
    bool TimeoutPeriod_exists() const
    {
        const size_t n = offsetof(Self, TimeoutPeriod);
        return GetField<Datetime>(n).exists ? true : false;
    }
    
    void TimeoutPeriod_clear()
    {
        const size_t n = offsetof(Self, TimeoutPeriod);
        GetField<Datetime>(n).Clear();
    }
};

typedef Array<Apache_HTTPDProcess_RequestStateChange_Class> Apache_HTTPDProcess_RequestStateChange_ClassA;

MI_END_NAMESPACE

#endif /* __cplusplus */

#endif /* _Apache_HTTPDProcess_h */
