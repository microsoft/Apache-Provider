/* @migen@ */
/*
**==============================================================================
**
** WARNING: THIS FILE WAS AUTOMATICALLY GENERATED. PLEASE DO NOT EDIT.
**
**==============================================================================
*/
#ifndef _Apache_HTTPDVirtualHost_h
#define _Apache_HTTPDVirtualHost_h

#include <MI.h>
#include "CIM_SoftwareElement.h"

/*
**==============================================================================
**
** Apache_HTTPDVirtualHost [Apache_HTTPDVirtualHost]
**
** Keys:
**    Name
**    Version
**    SoftwareElementState
**    SoftwareElementID
**    TargetOperatingSystem
**
**==============================================================================
*/

typedef struct _Apache_HTTPDVirtualHost /* extends CIM_SoftwareElement */
{
    MI_Instance __instance;
    /* CIM_ManagedElement properties */
    MI_ConstStringField InstanceID;
    MI_ConstStringField Caption;
    MI_ConstStringField Description;
    MI_ConstStringField ElementName;
    /* CIM_ManagedSystemElement properties */
    MI_ConstDatetimeField InstallDate;
    /*KEY*/ MI_ConstStringField Name;
    MI_ConstUint16AField OperationalStatus;
    MI_ConstStringAField StatusDescriptions;
    MI_ConstStringField Status;
    MI_ConstUint16Field HealthState;
    MI_ConstUint16Field CommunicationStatus;
    MI_ConstUint16Field DetailedStatus;
    MI_ConstUint16Field OperatingStatus;
    MI_ConstUint16Field PrimaryStatus;
    /* CIM_LogicalElement properties */
    /* CIM_SoftwareElement properties */
    /*KEY*/ MI_ConstStringField Version;
    /*KEY*/ MI_ConstUint16Field SoftwareElementState;
    /*KEY*/ MI_ConstStringField SoftwareElementID;
    /*KEY*/ MI_ConstUint16Field TargetOperatingSystem;
    MI_ConstStringField OtherTargetOS;
    MI_ConstStringField Manufacturer;
    MI_ConstStringField BuildNumber;
    MI_ConstStringField SerialNumber;
    MI_ConstStringField CodeSet;
    MI_ConstStringField IdentificationCode;
    MI_ConstStringField LanguageEdition;
    /* Apache_HTTPDVirtualHost properties */
    MI_ConstStringAField IPAddresses;
    MI_ConstUint16Field HTTPPort;
    MI_ConstUint16Field HTTPSPort;
    MI_ConstStringField ServerName;
    MI_ConstStringAField ServerAlias;
    MI_ConstStringField DocumentRoot;
    MI_ConstStringField ServerAdmin;
    MI_ConstStringField ErrorLog;
    MI_ConstStringField CustomLog;
    MI_ConstStringField AccessLog;
}
Apache_HTTPDVirtualHost;

typedef struct _Apache_HTTPDVirtualHost_Ref
{
    Apache_HTTPDVirtualHost* value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
Apache_HTTPDVirtualHost_Ref;

typedef struct _Apache_HTTPDVirtualHost_ConstRef
{
    MI_CONST Apache_HTTPDVirtualHost* value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
Apache_HTTPDVirtualHost_ConstRef;

typedef struct _Apache_HTTPDVirtualHost_Array
{
    struct _Apache_HTTPDVirtualHost** data;
    MI_Uint32 size;
}
Apache_HTTPDVirtualHost_Array;

typedef struct _Apache_HTTPDVirtualHost_ConstArray
{
    struct _Apache_HTTPDVirtualHost MI_CONST* MI_CONST* data;
    MI_Uint32 size;
}
Apache_HTTPDVirtualHost_ConstArray;

typedef struct _Apache_HTTPDVirtualHost_ArrayRef
{
    Apache_HTTPDVirtualHost_Array value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
Apache_HTTPDVirtualHost_ArrayRef;

typedef struct _Apache_HTTPDVirtualHost_ConstArrayRef
{
    Apache_HTTPDVirtualHost_ConstArray value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
Apache_HTTPDVirtualHost_ConstArrayRef;

MI_EXTERN_C MI_CONST MI_ClassDecl Apache_HTTPDVirtualHost_rtti;

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Construct(
    Apache_HTTPDVirtualHost* self,
    MI_Context* context)
{
    return MI_ConstructInstance(context, &Apache_HTTPDVirtualHost_rtti,
        (MI_Instance*)&self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clone(
    const Apache_HTTPDVirtualHost* self,
    Apache_HTTPDVirtualHost** newInstance)
{
    return MI_Instance_Clone(
        &self->__instance, (MI_Instance**)newInstance);
}

MI_INLINE MI_Boolean MI_CALL Apache_HTTPDVirtualHost_IsA(
    const MI_Instance* self)
{
    MI_Boolean res = MI_FALSE;
    return MI_Instance_IsA(self, &Apache_HTTPDVirtualHost_rtti, &res) == MI_RESULT_OK && res;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Destruct(Apache_HTTPDVirtualHost* self)
{
    return MI_Instance_Destruct(&self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Delete(Apache_HTTPDVirtualHost* self)
{
    return MI_Instance_Delete(&self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Post(
    const Apache_HTTPDVirtualHost* self,
    MI_Context* context)
{
    return MI_PostInstance(context, &self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_InstanceID(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        0,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_SetPtr_InstanceID(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        0,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_InstanceID(
    Apache_HTTPDVirtualHost* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_Caption(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        1,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_SetPtr_Caption(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        1,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_Caption(
    Apache_HTTPDVirtualHost* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        1);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_Description(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        2,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_SetPtr_Description(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        2,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_Description(
    Apache_HTTPDVirtualHost* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        2);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_ElementName(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        3,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_SetPtr_ElementName(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        3,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_ElementName(
    Apache_HTTPDVirtualHost* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        3);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_InstallDate(
    Apache_HTTPDVirtualHost* self,
    MI_Datetime x)
{
    ((MI_DatetimeField*)&self->InstallDate)->value = x;
    ((MI_DatetimeField*)&self->InstallDate)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_InstallDate(
    Apache_HTTPDVirtualHost* self)
{
    memset((void*)&self->InstallDate, 0, sizeof(self->InstallDate));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_Name(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        5,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_SetPtr_Name(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        5,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_Name(
    Apache_HTTPDVirtualHost* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        5);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_OperationalStatus(
    Apache_HTTPDVirtualHost* self,
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

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_SetPtr_OperationalStatus(
    Apache_HTTPDVirtualHost* self,
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

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_OperationalStatus(
    Apache_HTTPDVirtualHost* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        6);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_StatusDescriptions(
    Apache_HTTPDVirtualHost* self,
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

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_SetPtr_StatusDescriptions(
    Apache_HTTPDVirtualHost* self,
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

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_StatusDescriptions(
    Apache_HTTPDVirtualHost* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        7);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_Status(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        8,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_SetPtr_Status(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        8,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_Status(
    Apache_HTTPDVirtualHost* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        8);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_HealthState(
    Apache_HTTPDVirtualHost* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->HealthState)->value = x;
    ((MI_Uint16Field*)&self->HealthState)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_HealthState(
    Apache_HTTPDVirtualHost* self)
{
    memset((void*)&self->HealthState, 0, sizeof(self->HealthState));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_CommunicationStatus(
    Apache_HTTPDVirtualHost* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->CommunicationStatus)->value = x;
    ((MI_Uint16Field*)&self->CommunicationStatus)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_CommunicationStatus(
    Apache_HTTPDVirtualHost* self)
{
    memset((void*)&self->CommunicationStatus, 0, sizeof(self->CommunicationStatus));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_DetailedStatus(
    Apache_HTTPDVirtualHost* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->DetailedStatus)->value = x;
    ((MI_Uint16Field*)&self->DetailedStatus)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_DetailedStatus(
    Apache_HTTPDVirtualHost* self)
{
    memset((void*)&self->DetailedStatus, 0, sizeof(self->DetailedStatus));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_OperatingStatus(
    Apache_HTTPDVirtualHost* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->OperatingStatus)->value = x;
    ((MI_Uint16Field*)&self->OperatingStatus)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_OperatingStatus(
    Apache_HTTPDVirtualHost* self)
{
    memset((void*)&self->OperatingStatus, 0, sizeof(self->OperatingStatus));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_PrimaryStatus(
    Apache_HTTPDVirtualHost* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->PrimaryStatus)->value = x;
    ((MI_Uint16Field*)&self->PrimaryStatus)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_PrimaryStatus(
    Apache_HTTPDVirtualHost* self)
{
    memset((void*)&self->PrimaryStatus, 0, sizeof(self->PrimaryStatus));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_Version(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        14,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_SetPtr_Version(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        14,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_Version(
    Apache_HTTPDVirtualHost* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        14);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_SoftwareElementState(
    Apache_HTTPDVirtualHost* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->SoftwareElementState)->value = x;
    ((MI_Uint16Field*)&self->SoftwareElementState)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_SoftwareElementState(
    Apache_HTTPDVirtualHost* self)
{
    memset((void*)&self->SoftwareElementState, 0, sizeof(self->SoftwareElementState));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_SoftwareElementID(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        16,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_SetPtr_SoftwareElementID(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        16,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_SoftwareElementID(
    Apache_HTTPDVirtualHost* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        16);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_TargetOperatingSystem(
    Apache_HTTPDVirtualHost* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->TargetOperatingSystem)->value = x;
    ((MI_Uint16Field*)&self->TargetOperatingSystem)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_TargetOperatingSystem(
    Apache_HTTPDVirtualHost* self)
{
    memset((void*)&self->TargetOperatingSystem, 0, sizeof(self->TargetOperatingSystem));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_OtherTargetOS(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        18,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_SetPtr_OtherTargetOS(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        18,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_OtherTargetOS(
    Apache_HTTPDVirtualHost* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        18);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_Manufacturer(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        19,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_SetPtr_Manufacturer(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        19,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_Manufacturer(
    Apache_HTTPDVirtualHost* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        19);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_BuildNumber(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        20,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_SetPtr_BuildNumber(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        20,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_BuildNumber(
    Apache_HTTPDVirtualHost* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        20);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_SerialNumber(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        21,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_SetPtr_SerialNumber(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        21,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_SerialNumber(
    Apache_HTTPDVirtualHost* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        21);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_CodeSet(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        22,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_SetPtr_CodeSet(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        22,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_CodeSet(
    Apache_HTTPDVirtualHost* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        22);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_IdentificationCode(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        23,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_SetPtr_IdentificationCode(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        23,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_IdentificationCode(
    Apache_HTTPDVirtualHost* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        23);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_LanguageEdition(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        24,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_SetPtr_LanguageEdition(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        24,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_LanguageEdition(
    Apache_HTTPDVirtualHost* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        24);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_IPAddresses(
    Apache_HTTPDVirtualHost* self,
    const MI_Char** data,
    MI_Uint32 size)
{
    MI_Array arr;
    arr.data = (void*)data;
    arr.size = size;
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        25,
        (MI_Value*)&arr,
        MI_STRINGA,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_SetPtr_IPAddresses(
    Apache_HTTPDVirtualHost* self,
    const MI_Char** data,
    MI_Uint32 size)
{
    MI_Array arr;
    arr.data = (void*)data;
    arr.size = size;
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        25,
        (MI_Value*)&arr,
        MI_STRINGA,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_IPAddresses(
    Apache_HTTPDVirtualHost* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        25);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_HTTPPort(
    Apache_HTTPDVirtualHost* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->HTTPPort)->value = x;
    ((MI_Uint16Field*)&self->HTTPPort)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_HTTPPort(
    Apache_HTTPDVirtualHost* self)
{
    memset((void*)&self->HTTPPort, 0, sizeof(self->HTTPPort));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_HTTPSPort(
    Apache_HTTPDVirtualHost* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->HTTPSPort)->value = x;
    ((MI_Uint16Field*)&self->HTTPSPort)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_HTTPSPort(
    Apache_HTTPDVirtualHost* self)
{
    memset((void*)&self->HTTPSPort, 0, sizeof(self->HTTPSPort));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_ServerName(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        28,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_SetPtr_ServerName(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        28,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_ServerName(
    Apache_HTTPDVirtualHost* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        28);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_ServerAlias(
    Apache_HTTPDVirtualHost* self,
    const MI_Char** data,
    MI_Uint32 size)
{
    MI_Array arr;
    arr.data = (void*)data;
    arr.size = size;
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        29,
        (MI_Value*)&arr,
        MI_STRINGA,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_SetPtr_ServerAlias(
    Apache_HTTPDVirtualHost* self,
    const MI_Char** data,
    MI_Uint32 size)
{
    MI_Array arr;
    arr.data = (void*)data;
    arr.size = size;
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        29,
        (MI_Value*)&arr,
        MI_STRINGA,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_ServerAlias(
    Apache_HTTPDVirtualHost* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        29);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_DocumentRoot(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        30,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_SetPtr_DocumentRoot(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        30,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_DocumentRoot(
    Apache_HTTPDVirtualHost* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        30);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_ServerAdmin(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        31,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_SetPtr_ServerAdmin(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        31,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_ServerAdmin(
    Apache_HTTPDVirtualHost* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        31);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_ErrorLog(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        32,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_SetPtr_ErrorLog(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        32,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_ErrorLog(
    Apache_HTTPDVirtualHost* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        32);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_CustomLog(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        33,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_SetPtr_CustomLog(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        33,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_CustomLog(
    Apache_HTTPDVirtualHost* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        33);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Set_AccessLog(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        34,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_SetPtr_AccessLog(
    Apache_HTTPDVirtualHost* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        34,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHost_Clear_AccessLog(
    Apache_HTTPDVirtualHost* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        34);
}

/*
**==============================================================================
**
** Apache_HTTPDVirtualHost provider function prototypes
**
**==============================================================================
*/

/* The developer may optionally define this structure */
typedef struct _Apache_HTTPDVirtualHost_Self Apache_HTTPDVirtualHost_Self;

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHost_Load(
    Apache_HTTPDVirtualHost_Self** self,
    MI_Module_Self* selfModule,
    MI_Context* context);

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHost_Unload(
    Apache_HTTPDVirtualHost_Self* self,
    MI_Context* context);

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHost_EnumerateInstances(
    Apache_HTTPDVirtualHost_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const MI_PropertySet* propertySet,
    MI_Boolean keysOnly,
    const MI_Filter* filter);

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHost_GetInstance(
    Apache_HTTPDVirtualHost_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDVirtualHost* instanceName,
    const MI_PropertySet* propertySet);

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHost_CreateInstance(
    Apache_HTTPDVirtualHost_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDVirtualHost* newInstance);

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHost_ModifyInstance(
    Apache_HTTPDVirtualHost_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDVirtualHost* modifiedInstance,
    const MI_PropertySet* propertySet);

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHost_DeleteInstance(
    Apache_HTTPDVirtualHost_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDVirtualHost* instanceName);


/*
**==============================================================================
**
** Apache_HTTPDVirtualHost_Class
**
**==============================================================================
*/

#ifdef __cplusplus
# include <micxx/micxx.h>

MI_BEGIN_NAMESPACE

class Apache_HTTPDVirtualHost_Class : public CIM_SoftwareElement_Class
{
public:
    
    typedef Apache_HTTPDVirtualHost Self;
    
    Apache_HTTPDVirtualHost_Class() :
        CIM_SoftwareElement_Class(&Apache_HTTPDVirtualHost_rtti)
    {
    }
    
    Apache_HTTPDVirtualHost_Class(
        const Apache_HTTPDVirtualHost* instanceName,
        bool keysOnly) :
        CIM_SoftwareElement_Class(
            &Apache_HTTPDVirtualHost_rtti,
            &instanceName->__instance,
            keysOnly)
    {
    }
    
    Apache_HTTPDVirtualHost_Class(
        const MI_ClassDecl* clDecl,
        const MI_Instance* instance,
        bool keysOnly) :
        CIM_SoftwareElement_Class(clDecl, instance, keysOnly)
    {
    }
    
    Apache_HTTPDVirtualHost_Class(
        const MI_ClassDecl* clDecl) :
        CIM_SoftwareElement_Class(clDecl)
    {
    }
    
    Apache_HTTPDVirtualHost_Class& operator=(
        const Apache_HTTPDVirtualHost_Class& x)
    {
        CopyRef(x);
        return *this;
    }
    
    Apache_HTTPDVirtualHost_Class(
        const Apache_HTTPDVirtualHost_Class& x) :
        CIM_SoftwareElement_Class(x)
    {
    }

    static const MI_ClassDecl* GetClassDecl()
    {
        return &Apache_HTTPDVirtualHost_rtti;
    }

    //
    // Apache_HTTPDVirtualHost_Class.IPAddresses
    //
    
    const Field<StringA>& IPAddresses() const
    {
        const size_t n = offsetof(Self, IPAddresses);
        return GetField<StringA>(n);
    }
    
    void IPAddresses(const Field<StringA>& x)
    {
        const size_t n = offsetof(Self, IPAddresses);
        GetField<StringA>(n) = x;
    }
    
    const StringA& IPAddresses_value() const
    {
        const size_t n = offsetof(Self, IPAddresses);
        return GetField<StringA>(n).value;
    }
    
    void IPAddresses_value(const StringA& x)
    {
        const size_t n = offsetof(Self, IPAddresses);
        GetField<StringA>(n).Set(x);
    }
    
    bool IPAddresses_exists() const
    {
        const size_t n = offsetof(Self, IPAddresses);
        return GetField<StringA>(n).exists ? true : false;
    }
    
    void IPAddresses_clear()
    {
        const size_t n = offsetof(Self, IPAddresses);
        GetField<StringA>(n).Clear();
    }

    //
    // Apache_HTTPDVirtualHost_Class.HTTPPort
    //
    
    const Field<Uint16>& HTTPPort() const
    {
        const size_t n = offsetof(Self, HTTPPort);
        return GetField<Uint16>(n);
    }
    
    void HTTPPort(const Field<Uint16>& x)
    {
        const size_t n = offsetof(Self, HTTPPort);
        GetField<Uint16>(n) = x;
    }
    
    const Uint16& HTTPPort_value() const
    {
        const size_t n = offsetof(Self, HTTPPort);
        return GetField<Uint16>(n).value;
    }
    
    void HTTPPort_value(const Uint16& x)
    {
        const size_t n = offsetof(Self, HTTPPort);
        GetField<Uint16>(n).Set(x);
    }
    
    bool HTTPPort_exists() const
    {
        const size_t n = offsetof(Self, HTTPPort);
        return GetField<Uint16>(n).exists ? true : false;
    }
    
    void HTTPPort_clear()
    {
        const size_t n = offsetof(Self, HTTPPort);
        GetField<Uint16>(n).Clear();
    }

    //
    // Apache_HTTPDVirtualHost_Class.HTTPSPort
    //
    
    const Field<Uint16>& HTTPSPort() const
    {
        const size_t n = offsetof(Self, HTTPSPort);
        return GetField<Uint16>(n);
    }
    
    void HTTPSPort(const Field<Uint16>& x)
    {
        const size_t n = offsetof(Self, HTTPSPort);
        GetField<Uint16>(n) = x;
    }
    
    const Uint16& HTTPSPort_value() const
    {
        const size_t n = offsetof(Self, HTTPSPort);
        return GetField<Uint16>(n).value;
    }
    
    void HTTPSPort_value(const Uint16& x)
    {
        const size_t n = offsetof(Self, HTTPSPort);
        GetField<Uint16>(n).Set(x);
    }
    
    bool HTTPSPort_exists() const
    {
        const size_t n = offsetof(Self, HTTPSPort);
        return GetField<Uint16>(n).exists ? true : false;
    }
    
    void HTTPSPort_clear()
    {
        const size_t n = offsetof(Self, HTTPSPort);
        GetField<Uint16>(n).Clear();
    }

    //
    // Apache_HTTPDVirtualHost_Class.ServerName
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
    // Apache_HTTPDVirtualHost_Class.ServerAlias
    //
    
    const Field<StringA>& ServerAlias() const
    {
        const size_t n = offsetof(Self, ServerAlias);
        return GetField<StringA>(n);
    }
    
    void ServerAlias(const Field<StringA>& x)
    {
        const size_t n = offsetof(Self, ServerAlias);
        GetField<StringA>(n) = x;
    }
    
    const StringA& ServerAlias_value() const
    {
        const size_t n = offsetof(Self, ServerAlias);
        return GetField<StringA>(n).value;
    }
    
    void ServerAlias_value(const StringA& x)
    {
        const size_t n = offsetof(Self, ServerAlias);
        GetField<StringA>(n).Set(x);
    }
    
    bool ServerAlias_exists() const
    {
        const size_t n = offsetof(Self, ServerAlias);
        return GetField<StringA>(n).exists ? true : false;
    }
    
    void ServerAlias_clear()
    {
        const size_t n = offsetof(Self, ServerAlias);
        GetField<StringA>(n).Clear();
    }

    //
    // Apache_HTTPDVirtualHost_Class.DocumentRoot
    //
    
    const Field<String>& DocumentRoot() const
    {
        const size_t n = offsetof(Self, DocumentRoot);
        return GetField<String>(n);
    }
    
    void DocumentRoot(const Field<String>& x)
    {
        const size_t n = offsetof(Self, DocumentRoot);
        GetField<String>(n) = x;
    }
    
    const String& DocumentRoot_value() const
    {
        const size_t n = offsetof(Self, DocumentRoot);
        return GetField<String>(n).value;
    }
    
    void DocumentRoot_value(const String& x)
    {
        const size_t n = offsetof(Self, DocumentRoot);
        GetField<String>(n).Set(x);
    }
    
    bool DocumentRoot_exists() const
    {
        const size_t n = offsetof(Self, DocumentRoot);
        return GetField<String>(n).exists ? true : false;
    }
    
    void DocumentRoot_clear()
    {
        const size_t n = offsetof(Self, DocumentRoot);
        GetField<String>(n).Clear();
    }

    //
    // Apache_HTTPDVirtualHost_Class.ServerAdmin
    //
    
    const Field<String>& ServerAdmin() const
    {
        const size_t n = offsetof(Self, ServerAdmin);
        return GetField<String>(n);
    }
    
    void ServerAdmin(const Field<String>& x)
    {
        const size_t n = offsetof(Self, ServerAdmin);
        GetField<String>(n) = x;
    }
    
    const String& ServerAdmin_value() const
    {
        const size_t n = offsetof(Self, ServerAdmin);
        return GetField<String>(n).value;
    }
    
    void ServerAdmin_value(const String& x)
    {
        const size_t n = offsetof(Self, ServerAdmin);
        GetField<String>(n).Set(x);
    }
    
    bool ServerAdmin_exists() const
    {
        const size_t n = offsetof(Self, ServerAdmin);
        return GetField<String>(n).exists ? true : false;
    }
    
    void ServerAdmin_clear()
    {
        const size_t n = offsetof(Self, ServerAdmin);
        GetField<String>(n).Clear();
    }

    //
    // Apache_HTTPDVirtualHost_Class.ErrorLog
    //
    
    const Field<String>& ErrorLog() const
    {
        const size_t n = offsetof(Self, ErrorLog);
        return GetField<String>(n);
    }
    
    void ErrorLog(const Field<String>& x)
    {
        const size_t n = offsetof(Self, ErrorLog);
        GetField<String>(n) = x;
    }
    
    const String& ErrorLog_value() const
    {
        const size_t n = offsetof(Self, ErrorLog);
        return GetField<String>(n).value;
    }
    
    void ErrorLog_value(const String& x)
    {
        const size_t n = offsetof(Self, ErrorLog);
        GetField<String>(n).Set(x);
    }
    
    bool ErrorLog_exists() const
    {
        const size_t n = offsetof(Self, ErrorLog);
        return GetField<String>(n).exists ? true : false;
    }
    
    void ErrorLog_clear()
    {
        const size_t n = offsetof(Self, ErrorLog);
        GetField<String>(n).Clear();
    }

    //
    // Apache_HTTPDVirtualHost_Class.CustomLog
    //
    
    const Field<String>& CustomLog() const
    {
        const size_t n = offsetof(Self, CustomLog);
        return GetField<String>(n);
    }
    
    void CustomLog(const Field<String>& x)
    {
        const size_t n = offsetof(Self, CustomLog);
        GetField<String>(n) = x;
    }
    
    const String& CustomLog_value() const
    {
        const size_t n = offsetof(Self, CustomLog);
        return GetField<String>(n).value;
    }
    
    void CustomLog_value(const String& x)
    {
        const size_t n = offsetof(Self, CustomLog);
        GetField<String>(n).Set(x);
    }
    
    bool CustomLog_exists() const
    {
        const size_t n = offsetof(Self, CustomLog);
        return GetField<String>(n).exists ? true : false;
    }
    
    void CustomLog_clear()
    {
        const size_t n = offsetof(Self, CustomLog);
        GetField<String>(n).Clear();
    }

    //
    // Apache_HTTPDVirtualHost_Class.AccessLog
    //
    
    const Field<String>& AccessLog() const
    {
        const size_t n = offsetof(Self, AccessLog);
        return GetField<String>(n);
    }
    
    void AccessLog(const Field<String>& x)
    {
        const size_t n = offsetof(Self, AccessLog);
        GetField<String>(n) = x;
    }
    
    const String& AccessLog_value() const
    {
        const size_t n = offsetof(Self, AccessLog);
        return GetField<String>(n).value;
    }
    
    void AccessLog_value(const String& x)
    {
        const size_t n = offsetof(Self, AccessLog);
        GetField<String>(n).Set(x);
    }
    
    bool AccessLog_exists() const
    {
        const size_t n = offsetof(Self, AccessLog);
        return GetField<String>(n).exists ? true : false;
    }
    
    void AccessLog_clear()
    {
        const size_t n = offsetof(Self, AccessLog);
        GetField<String>(n).Clear();
    }
};

typedef Array<Apache_HTTPDVirtualHost_Class> Apache_HTTPDVirtualHost_ClassA;

MI_END_NAMESPACE

#endif /* __cplusplus */

#endif /* _Apache_HTTPDVirtualHost_h */
