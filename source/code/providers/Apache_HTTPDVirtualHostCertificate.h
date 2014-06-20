/* @migen@ */
/*
**==============================================================================
**
** WARNING: THIS FILE WAS AUTOMATICALLY GENERATED. PLEASE DO NOT EDIT.
**
**==============================================================================
*/
#ifndef _Apache_HTTPDVirtualHostCertificate_h
#define _Apache_HTTPDVirtualHostCertificate_h

#include <MI.h>
#include "CIM_SoftwareElement.h"

/*
**==============================================================================
**
** Apache_HTTPDVirtualHostCertificate [Apache_HTTPDVirtualHostCertificate]
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

typedef struct _Apache_HTTPDVirtualHostCertificate /* extends CIM_SoftwareElement */
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
    /* Apache_HTTPDVirtualHostCertificate properties */
    MI_ConstStringField VirtualHost;
    MI_ConstDatetimeField ExpirationDate;
    MI_ConstUint16Field DaysUntilExpiration;
    MI_ConstStringField FileName;
}
Apache_HTTPDVirtualHostCertificate;

typedef struct _Apache_HTTPDVirtualHostCertificate_Ref
{
    Apache_HTTPDVirtualHostCertificate* value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
Apache_HTTPDVirtualHostCertificate_Ref;

typedef struct _Apache_HTTPDVirtualHostCertificate_ConstRef
{
    MI_CONST Apache_HTTPDVirtualHostCertificate* value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
Apache_HTTPDVirtualHostCertificate_ConstRef;

typedef struct _Apache_HTTPDVirtualHostCertificate_Array
{
    struct _Apache_HTTPDVirtualHostCertificate** data;
    MI_Uint32 size;
}
Apache_HTTPDVirtualHostCertificate_Array;

typedef struct _Apache_HTTPDVirtualHostCertificate_ConstArray
{
    struct _Apache_HTTPDVirtualHostCertificate MI_CONST* MI_CONST* data;
    MI_Uint32 size;
}
Apache_HTTPDVirtualHostCertificate_ConstArray;

typedef struct _Apache_HTTPDVirtualHostCertificate_ArrayRef
{
    Apache_HTTPDVirtualHostCertificate_Array value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
Apache_HTTPDVirtualHostCertificate_ArrayRef;

typedef struct _Apache_HTTPDVirtualHostCertificate_ConstArrayRef
{
    Apache_HTTPDVirtualHostCertificate_ConstArray value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
Apache_HTTPDVirtualHostCertificate_ConstArrayRef;

MI_EXTERN_C MI_CONST MI_ClassDecl Apache_HTTPDVirtualHostCertificate_rtti;

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Construct(
    Apache_HTTPDVirtualHostCertificate* self,
    MI_Context* context)
{
    return MI_ConstructInstance(context, &Apache_HTTPDVirtualHostCertificate_rtti,
        (MI_Instance*)&self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Clone(
    const Apache_HTTPDVirtualHostCertificate* self,
    Apache_HTTPDVirtualHostCertificate** newInstance)
{
    return MI_Instance_Clone(
        &self->__instance, (MI_Instance**)newInstance);
}

MI_INLINE MI_Boolean MI_CALL Apache_HTTPDVirtualHostCertificate_IsA(
    const MI_Instance* self)
{
    MI_Boolean res = MI_FALSE;
    return MI_Instance_IsA(self, &Apache_HTTPDVirtualHostCertificate_rtti, &res) == MI_RESULT_OK && res;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Destruct(Apache_HTTPDVirtualHostCertificate* self)
{
    return MI_Instance_Destruct(&self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Delete(Apache_HTTPDVirtualHostCertificate* self)
{
    return MI_Instance_Delete(&self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Post(
    const Apache_HTTPDVirtualHostCertificate* self,
    MI_Context* context)
{
    return MI_PostInstance(context, &self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Set_InstanceID(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        0,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_SetPtr_InstanceID(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        0,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Clear_InstanceID(
    Apache_HTTPDVirtualHostCertificate* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Set_Caption(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        1,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_SetPtr_Caption(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        1,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Clear_Caption(
    Apache_HTTPDVirtualHostCertificate* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        1);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Set_Description(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        2,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_SetPtr_Description(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        2,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Clear_Description(
    Apache_HTTPDVirtualHostCertificate* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        2);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Set_ElementName(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        3,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_SetPtr_ElementName(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        3,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Clear_ElementName(
    Apache_HTTPDVirtualHostCertificate* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        3);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Set_InstallDate(
    Apache_HTTPDVirtualHostCertificate* self,
    MI_Datetime x)
{
    ((MI_DatetimeField*)&self->InstallDate)->value = x;
    ((MI_DatetimeField*)&self->InstallDate)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Clear_InstallDate(
    Apache_HTTPDVirtualHostCertificate* self)
{
    memset((void*)&self->InstallDate, 0, sizeof(self->InstallDate));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Set_Name(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        5,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_SetPtr_Name(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        5,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Clear_Name(
    Apache_HTTPDVirtualHostCertificate* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        5);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Set_OperationalStatus(
    Apache_HTTPDVirtualHostCertificate* self,
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

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_SetPtr_OperationalStatus(
    Apache_HTTPDVirtualHostCertificate* self,
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

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Clear_OperationalStatus(
    Apache_HTTPDVirtualHostCertificate* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        6);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Set_StatusDescriptions(
    Apache_HTTPDVirtualHostCertificate* self,
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

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_SetPtr_StatusDescriptions(
    Apache_HTTPDVirtualHostCertificate* self,
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

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Clear_StatusDescriptions(
    Apache_HTTPDVirtualHostCertificate* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        7);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Set_Status(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        8,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_SetPtr_Status(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        8,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Clear_Status(
    Apache_HTTPDVirtualHostCertificate* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        8);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Set_HealthState(
    Apache_HTTPDVirtualHostCertificate* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->HealthState)->value = x;
    ((MI_Uint16Field*)&self->HealthState)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Clear_HealthState(
    Apache_HTTPDVirtualHostCertificate* self)
{
    memset((void*)&self->HealthState, 0, sizeof(self->HealthState));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Set_CommunicationStatus(
    Apache_HTTPDVirtualHostCertificate* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->CommunicationStatus)->value = x;
    ((MI_Uint16Field*)&self->CommunicationStatus)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Clear_CommunicationStatus(
    Apache_HTTPDVirtualHostCertificate* self)
{
    memset((void*)&self->CommunicationStatus, 0, sizeof(self->CommunicationStatus));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Set_DetailedStatus(
    Apache_HTTPDVirtualHostCertificate* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->DetailedStatus)->value = x;
    ((MI_Uint16Field*)&self->DetailedStatus)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Clear_DetailedStatus(
    Apache_HTTPDVirtualHostCertificate* self)
{
    memset((void*)&self->DetailedStatus, 0, sizeof(self->DetailedStatus));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Set_OperatingStatus(
    Apache_HTTPDVirtualHostCertificate* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->OperatingStatus)->value = x;
    ((MI_Uint16Field*)&self->OperatingStatus)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Clear_OperatingStatus(
    Apache_HTTPDVirtualHostCertificate* self)
{
    memset((void*)&self->OperatingStatus, 0, sizeof(self->OperatingStatus));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Set_PrimaryStatus(
    Apache_HTTPDVirtualHostCertificate* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->PrimaryStatus)->value = x;
    ((MI_Uint16Field*)&self->PrimaryStatus)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Clear_PrimaryStatus(
    Apache_HTTPDVirtualHostCertificate* self)
{
    memset((void*)&self->PrimaryStatus, 0, sizeof(self->PrimaryStatus));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Set_Version(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        14,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_SetPtr_Version(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        14,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Clear_Version(
    Apache_HTTPDVirtualHostCertificate* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        14);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Set_SoftwareElementState(
    Apache_HTTPDVirtualHostCertificate* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->SoftwareElementState)->value = x;
    ((MI_Uint16Field*)&self->SoftwareElementState)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Clear_SoftwareElementState(
    Apache_HTTPDVirtualHostCertificate* self)
{
    memset((void*)&self->SoftwareElementState, 0, sizeof(self->SoftwareElementState));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Set_SoftwareElementID(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        16,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_SetPtr_SoftwareElementID(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        16,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Clear_SoftwareElementID(
    Apache_HTTPDVirtualHostCertificate* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        16);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Set_TargetOperatingSystem(
    Apache_HTTPDVirtualHostCertificate* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->TargetOperatingSystem)->value = x;
    ((MI_Uint16Field*)&self->TargetOperatingSystem)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Clear_TargetOperatingSystem(
    Apache_HTTPDVirtualHostCertificate* self)
{
    memset((void*)&self->TargetOperatingSystem, 0, sizeof(self->TargetOperatingSystem));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Set_OtherTargetOS(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        18,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_SetPtr_OtherTargetOS(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        18,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Clear_OtherTargetOS(
    Apache_HTTPDVirtualHostCertificate* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        18);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Set_Manufacturer(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        19,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_SetPtr_Manufacturer(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        19,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Clear_Manufacturer(
    Apache_HTTPDVirtualHostCertificate* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        19);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Set_BuildNumber(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        20,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_SetPtr_BuildNumber(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        20,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Clear_BuildNumber(
    Apache_HTTPDVirtualHostCertificate* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        20);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Set_SerialNumber(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        21,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_SetPtr_SerialNumber(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        21,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Clear_SerialNumber(
    Apache_HTTPDVirtualHostCertificate* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        21);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Set_CodeSet(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        22,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_SetPtr_CodeSet(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        22,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Clear_CodeSet(
    Apache_HTTPDVirtualHostCertificate* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        22);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Set_IdentificationCode(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        23,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_SetPtr_IdentificationCode(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        23,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Clear_IdentificationCode(
    Apache_HTTPDVirtualHostCertificate* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        23);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Set_LanguageEdition(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        24,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_SetPtr_LanguageEdition(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        24,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Clear_LanguageEdition(
    Apache_HTTPDVirtualHostCertificate* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        24);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Set_VirtualHost(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        25,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_SetPtr_VirtualHost(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        25,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Clear_VirtualHost(
    Apache_HTTPDVirtualHostCertificate* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        25);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Set_ExpirationDate(
    Apache_HTTPDVirtualHostCertificate* self,
    MI_Datetime x)
{
    ((MI_DatetimeField*)&self->ExpirationDate)->value = x;
    ((MI_DatetimeField*)&self->ExpirationDate)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Clear_ExpirationDate(
    Apache_HTTPDVirtualHostCertificate* self)
{
    memset((void*)&self->ExpirationDate, 0, sizeof(self->ExpirationDate));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Set_DaysUntilExpiration(
    Apache_HTTPDVirtualHostCertificate* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->DaysUntilExpiration)->value = x;
    ((MI_Uint16Field*)&self->DaysUntilExpiration)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Clear_DaysUntilExpiration(
    Apache_HTTPDVirtualHostCertificate* self)
{
    memset((void*)&self->DaysUntilExpiration, 0, sizeof(self->DaysUntilExpiration));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Set_FileName(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        28,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_SetPtr_FileName(
    Apache_HTTPDVirtualHostCertificate* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        28,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDVirtualHostCertificate_Clear_FileName(
    Apache_HTTPDVirtualHostCertificate* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        28);
}

/*
**==============================================================================
**
** Apache_HTTPDVirtualHostCertificate provider function prototypes
**
**==============================================================================
*/

/* The developer may optionally define this structure */
typedef struct _Apache_HTTPDVirtualHostCertificate_Self Apache_HTTPDVirtualHostCertificate_Self;

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHostCertificate_Load(
    Apache_HTTPDVirtualHostCertificate_Self** self,
    MI_Module_Self* selfModule,
    MI_Context* context);

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHostCertificate_Unload(
    Apache_HTTPDVirtualHostCertificate_Self* self,
    MI_Context* context);

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHostCertificate_EnumerateInstances(
    Apache_HTTPDVirtualHostCertificate_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const MI_PropertySet* propertySet,
    MI_Boolean keysOnly,
    const MI_Filter* filter);

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHostCertificate_GetInstance(
    Apache_HTTPDVirtualHostCertificate_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDVirtualHostCertificate* instanceName,
    const MI_PropertySet* propertySet);

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHostCertificate_CreateInstance(
    Apache_HTTPDVirtualHostCertificate_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDVirtualHostCertificate* newInstance);

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHostCertificate_ModifyInstance(
    Apache_HTTPDVirtualHostCertificate_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDVirtualHostCertificate* modifiedInstance,
    const MI_PropertySet* propertySet);

MI_EXTERN_C void MI_CALL Apache_HTTPDVirtualHostCertificate_DeleteInstance(
    Apache_HTTPDVirtualHostCertificate_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDVirtualHostCertificate* instanceName);


/*
**==============================================================================
**
** Apache_HTTPDVirtualHostCertificate_Class
**
**==============================================================================
*/

#ifdef __cplusplus
# include <micxx/micxx.h>

MI_BEGIN_NAMESPACE

class Apache_HTTPDVirtualHostCertificate_Class : public CIM_SoftwareElement_Class
{
public:
    
    typedef Apache_HTTPDVirtualHostCertificate Self;
    
    Apache_HTTPDVirtualHostCertificate_Class() :
        CIM_SoftwareElement_Class(&Apache_HTTPDVirtualHostCertificate_rtti)
    {
    }
    
    Apache_HTTPDVirtualHostCertificate_Class(
        const Apache_HTTPDVirtualHostCertificate* instanceName,
        bool keysOnly) :
        CIM_SoftwareElement_Class(
            &Apache_HTTPDVirtualHostCertificate_rtti,
            &instanceName->__instance,
            keysOnly)
    {
    }
    
    Apache_HTTPDVirtualHostCertificate_Class(
        const MI_ClassDecl* clDecl,
        const MI_Instance* instance,
        bool keysOnly) :
        CIM_SoftwareElement_Class(clDecl, instance, keysOnly)
    {
    }
    
    Apache_HTTPDVirtualHostCertificate_Class(
        const MI_ClassDecl* clDecl) :
        CIM_SoftwareElement_Class(clDecl)
    {
    }
    
    Apache_HTTPDVirtualHostCertificate_Class& operator=(
        const Apache_HTTPDVirtualHostCertificate_Class& x)
    {
        CopyRef(x);
        return *this;
    }
    
    Apache_HTTPDVirtualHostCertificate_Class(
        const Apache_HTTPDVirtualHostCertificate_Class& x) :
        CIM_SoftwareElement_Class(x)
    {
    }

    static const MI_ClassDecl* GetClassDecl()
    {
        return &Apache_HTTPDVirtualHostCertificate_rtti;
    }

    //
    // Apache_HTTPDVirtualHostCertificate_Class.VirtualHost
    //
    
    const Field<String>& VirtualHost() const
    {
        const size_t n = offsetof(Self, VirtualHost);
        return GetField<String>(n);
    }
    
    void VirtualHost(const Field<String>& x)
    {
        const size_t n = offsetof(Self, VirtualHost);
        GetField<String>(n) = x;
    }
    
    const String& VirtualHost_value() const
    {
        const size_t n = offsetof(Self, VirtualHost);
        return GetField<String>(n).value;
    }
    
    void VirtualHost_value(const String& x)
    {
        const size_t n = offsetof(Self, VirtualHost);
        GetField<String>(n).Set(x);
    }
    
    bool VirtualHost_exists() const
    {
        const size_t n = offsetof(Self, VirtualHost);
        return GetField<String>(n).exists ? true : false;
    }
    
    void VirtualHost_clear()
    {
        const size_t n = offsetof(Self, VirtualHost);
        GetField<String>(n).Clear();
    }

    //
    // Apache_HTTPDVirtualHostCertificate_Class.ExpirationDate
    //
    
    const Field<Datetime>& ExpirationDate() const
    {
        const size_t n = offsetof(Self, ExpirationDate);
        return GetField<Datetime>(n);
    }
    
    void ExpirationDate(const Field<Datetime>& x)
    {
        const size_t n = offsetof(Self, ExpirationDate);
        GetField<Datetime>(n) = x;
    }
    
    const Datetime& ExpirationDate_value() const
    {
        const size_t n = offsetof(Self, ExpirationDate);
        return GetField<Datetime>(n).value;
    }
    
    void ExpirationDate_value(const Datetime& x)
    {
        const size_t n = offsetof(Self, ExpirationDate);
        GetField<Datetime>(n).Set(x);
    }
    
    bool ExpirationDate_exists() const
    {
        const size_t n = offsetof(Self, ExpirationDate);
        return GetField<Datetime>(n).exists ? true : false;
    }
    
    void ExpirationDate_clear()
    {
        const size_t n = offsetof(Self, ExpirationDate);
        GetField<Datetime>(n).Clear();
    }

    //
    // Apache_HTTPDVirtualHostCertificate_Class.DaysUntilExpiration
    //
    
    const Field<Uint16>& DaysUntilExpiration() const
    {
        const size_t n = offsetof(Self, DaysUntilExpiration);
        return GetField<Uint16>(n);
    }
    
    void DaysUntilExpiration(const Field<Uint16>& x)
    {
        const size_t n = offsetof(Self, DaysUntilExpiration);
        GetField<Uint16>(n) = x;
    }
    
    const Uint16& DaysUntilExpiration_value() const
    {
        const size_t n = offsetof(Self, DaysUntilExpiration);
        return GetField<Uint16>(n).value;
    }
    
    void DaysUntilExpiration_value(const Uint16& x)
    {
        const size_t n = offsetof(Self, DaysUntilExpiration);
        GetField<Uint16>(n).Set(x);
    }
    
    bool DaysUntilExpiration_exists() const
    {
        const size_t n = offsetof(Self, DaysUntilExpiration);
        return GetField<Uint16>(n).exists ? true : false;
    }
    
    void DaysUntilExpiration_clear()
    {
        const size_t n = offsetof(Self, DaysUntilExpiration);
        GetField<Uint16>(n).Clear();
    }

    //
    // Apache_HTTPDVirtualHostCertificate_Class.FileName
    //
    
    const Field<String>& FileName() const
    {
        const size_t n = offsetof(Self, FileName);
        return GetField<String>(n);
    }
    
    void FileName(const Field<String>& x)
    {
        const size_t n = offsetof(Self, FileName);
        GetField<String>(n) = x;
    }
    
    const String& FileName_value() const
    {
        const size_t n = offsetof(Self, FileName);
        return GetField<String>(n).value;
    }
    
    void FileName_value(const String& x)
    {
        const size_t n = offsetof(Self, FileName);
        GetField<String>(n).Set(x);
    }
    
    bool FileName_exists() const
    {
        const size_t n = offsetof(Self, FileName);
        return GetField<String>(n).exists ? true : false;
    }
    
    void FileName_clear()
    {
        const size_t n = offsetof(Self, FileName);
        GetField<String>(n).Clear();
    }
};

typedef Array<Apache_HTTPDVirtualHostCertificate_Class> Apache_HTTPDVirtualHostCertificate_ClassA;

MI_END_NAMESPACE

#endif /* __cplusplus */

#endif /* _Apache_HTTPDVirtualHostCertificate_h */
