/* @migen@ */
/*
**==============================================================================
**
** WARNING: THIS FILE WAS AUTOMATICALLY GENERATED. PLEASE DO NOT EDIT.
**
**==============================================================================
*/
#ifndef _CIM_SoftwareElement_h
#define _CIM_SoftwareElement_h

#include <MI.h>
#include "CIM_LogicalElement.h"

/*
**==============================================================================
**
** CIM_SoftwareElement [CIM_SoftwareElement]
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

typedef struct _CIM_SoftwareElement /* extends CIM_LogicalElement */
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
}
CIM_SoftwareElement;

typedef struct _CIM_SoftwareElement_Ref
{
    CIM_SoftwareElement* value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
CIM_SoftwareElement_Ref;

typedef struct _CIM_SoftwareElement_ConstRef
{
    MI_CONST CIM_SoftwareElement* value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
CIM_SoftwareElement_ConstRef;

typedef struct _CIM_SoftwareElement_Array
{
    struct _CIM_SoftwareElement** data;
    MI_Uint32 size;
}
CIM_SoftwareElement_Array;

typedef struct _CIM_SoftwareElement_ConstArray
{
    struct _CIM_SoftwareElement MI_CONST* MI_CONST* data;
    MI_Uint32 size;
}
CIM_SoftwareElement_ConstArray;

typedef struct _CIM_SoftwareElement_ArrayRef
{
    CIM_SoftwareElement_Array value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
CIM_SoftwareElement_ArrayRef;

typedef struct _CIM_SoftwareElement_ConstArrayRef
{
    CIM_SoftwareElement_ConstArray value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
CIM_SoftwareElement_ConstArrayRef;

MI_EXTERN_C MI_CONST MI_ClassDecl CIM_SoftwareElement_rtti;

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Construct(
    CIM_SoftwareElement* self,
    MI_Context* context)
{
    return MI_ConstructInstance(context, &CIM_SoftwareElement_rtti,
        (MI_Instance*)&self->__instance);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Clone(
    const CIM_SoftwareElement* self,
    CIM_SoftwareElement** newInstance)
{
    return MI_Instance_Clone(
        &self->__instance, (MI_Instance**)newInstance);
}

MI_INLINE MI_Boolean MI_CALL CIM_SoftwareElement_IsA(
    const MI_Instance* self)
{
    MI_Boolean res = MI_FALSE;
    return MI_Instance_IsA(self, &CIM_SoftwareElement_rtti, &res) == MI_RESULT_OK && res;
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Destruct(CIM_SoftwareElement* self)
{
    return MI_Instance_Destruct(&self->__instance);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Delete(CIM_SoftwareElement* self)
{
    return MI_Instance_Delete(&self->__instance);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Post(
    const CIM_SoftwareElement* self,
    MI_Context* context)
{
    return MI_PostInstance(context, &self->__instance);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Set_InstanceID(
    CIM_SoftwareElement* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        0,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_SetPtr_InstanceID(
    CIM_SoftwareElement* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        0,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Clear_InstanceID(
    CIM_SoftwareElement* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        0);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Set_Caption(
    CIM_SoftwareElement* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        1,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_SetPtr_Caption(
    CIM_SoftwareElement* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        1,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Clear_Caption(
    CIM_SoftwareElement* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        1);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Set_Description(
    CIM_SoftwareElement* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        2,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_SetPtr_Description(
    CIM_SoftwareElement* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        2,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Clear_Description(
    CIM_SoftwareElement* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        2);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Set_ElementName(
    CIM_SoftwareElement* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        3,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_SetPtr_ElementName(
    CIM_SoftwareElement* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        3,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Clear_ElementName(
    CIM_SoftwareElement* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        3);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Set_InstallDate(
    CIM_SoftwareElement* self,
    MI_Datetime x)
{
    ((MI_DatetimeField*)&self->InstallDate)->value = x;
    ((MI_DatetimeField*)&self->InstallDate)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Clear_InstallDate(
    CIM_SoftwareElement* self)
{
    memset((void*)&self->InstallDate, 0, sizeof(self->InstallDate));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Set_Name(
    CIM_SoftwareElement* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        5,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_SetPtr_Name(
    CIM_SoftwareElement* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        5,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Clear_Name(
    CIM_SoftwareElement* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        5);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Set_OperationalStatus(
    CIM_SoftwareElement* self,
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

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_SetPtr_OperationalStatus(
    CIM_SoftwareElement* self,
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

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Clear_OperationalStatus(
    CIM_SoftwareElement* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        6);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Set_StatusDescriptions(
    CIM_SoftwareElement* self,
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

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_SetPtr_StatusDescriptions(
    CIM_SoftwareElement* self,
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

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Clear_StatusDescriptions(
    CIM_SoftwareElement* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        7);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Set_Status(
    CIM_SoftwareElement* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        8,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_SetPtr_Status(
    CIM_SoftwareElement* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        8,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Clear_Status(
    CIM_SoftwareElement* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        8);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Set_HealthState(
    CIM_SoftwareElement* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->HealthState)->value = x;
    ((MI_Uint16Field*)&self->HealthState)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Clear_HealthState(
    CIM_SoftwareElement* self)
{
    memset((void*)&self->HealthState, 0, sizeof(self->HealthState));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Set_CommunicationStatus(
    CIM_SoftwareElement* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->CommunicationStatus)->value = x;
    ((MI_Uint16Field*)&self->CommunicationStatus)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Clear_CommunicationStatus(
    CIM_SoftwareElement* self)
{
    memset((void*)&self->CommunicationStatus, 0, sizeof(self->CommunicationStatus));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Set_DetailedStatus(
    CIM_SoftwareElement* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->DetailedStatus)->value = x;
    ((MI_Uint16Field*)&self->DetailedStatus)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Clear_DetailedStatus(
    CIM_SoftwareElement* self)
{
    memset((void*)&self->DetailedStatus, 0, sizeof(self->DetailedStatus));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Set_OperatingStatus(
    CIM_SoftwareElement* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->OperatingStatus)->value = x;
    ((MI_Uint16Field*)&self->OperatingStatus)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Clear_OperatingStatus(
    CIM_SoftwareElement* self)
{
    memset((void*)&self->OperatingStatus, 0, sizeof(self->OperatingStatus));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Set_PrimaryStatus(
    CIM_SoftwareElement* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->PrimaryStatus)->value = x;
    ((MI_Uint16Field*)&self->PrimaryStatus)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Clear_PrimaryStatus(
    CIM_SoftwareElement* self)
{
    memset((void*)&self->PrimaryStatus, 0, sizeof(self->PrimaryStatus));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Set_Version(
    CIM_SoftwareElement* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        14,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_SetPtr_Version(
    CIM_SoftwareElement* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        14,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Clear_Version(
    CIM_SoftwareElement* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        14);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Set_SoftwareElementState(
    CIM_SoftwareElement* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->SoftwareElementState)->value = x;
    ((MI_Uint16Field*)&self->SoftwareElementState)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Clear_SoftwareElementState(
    CIM_SoftwareElement* self)
{
    memset((void*)&self->SoftwareElementState, 0, sizeof(self->SoftwareElementState));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Set_SoftwareElementID(
    CIM_SoftwareElement* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        16,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_SetPtr_SoftwareElementID(
    CIM_SoftwareElement* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        16,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Clear_SoftwareElementID(
    CIM_SoftwareElement* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        16);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Set_TargetOperatingSystem(
    CIM_SoftwareElement* self,
    MI_Uint16 x)
{
    ((MI_Uint16Field*)&self->TargetOperatingSystem)->value = x;
    ((MI_Uint16Field*)&self->TargetOperatingSystem)->exists = 1;
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Clear_TargetOperatingSystem(
    CIM_SoftwareElement* self)
{
    memset((void*)&self->TargetOperatingSystem, 0, sizeof(self->TargetOperatingSystem));
    return MI_RESULT_OK;
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Set_OtherTargetOS(
    CIM_SoftwareElement* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        18,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_SetPtr_OtherTargetOS(
    CIM_SoftwareElement* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        18,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Clear_OtherTargetOS(
    CIM_SoftwareElement* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        18);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Set_Manufacturer(
    CIM_SoftwareElement* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        19,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_SetPtr_Manufacturer(
    CIM_SoftwareElement* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        19,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Clear_Manufacturer(
    CIM_SoftwareElement* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        19);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Set_BuildNumber(
    CIM_SoftwareElement* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        20,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_SetPtr_BuildNumber(
    CIM_SoftwareElement* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        20,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Clear_BuildNumber(
    CIM_SoftwareElement* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        20);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Set_SerialNumber(
    CIM_SoftwareElement* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        21,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_SetPtr_SerialNumber(
    CIM_SoftwareElement* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        21,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Clear_SerialNumber(
    CIM_SoftwareElement* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        21);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Set_CodeSet(
    CIM_SoftwareElement* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        22,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_SetPtr_CodeSet(
    CIM_SoftwareElement* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        22,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Clear_CodeSet(
    CIM_SoftwareElement* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        22);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Set_IdentificationCode(
    CIM_SoftwareElement* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        23,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_SetPtr_IdentificationCode(
    CIM_SoftwareElement* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        23,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Clear_IdentificationCode(
    CIM_SoftwareElement* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        23);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Set_LanguageEdition(
    CIM_SoftwareElement* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        24,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_SetPtr_LanguageEdition(
    CIM_SoftwareElement* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        24,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL CIM_SoftwareElement_Clear_LanguageEdition(
    CIM_SoftwareElement* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        24);
}


/*
**==============================================================================
**
** CIM_SoftwareElement_Class
**
**==============================================================================
*/

#ifdef __cplusplus
# include <micxx/micxx.h>

MI_BEGIN_NAMESPACE

class CIM_SoftwareElement_Class : public CIM_LogicalElement_Class
{
public:
    
    typedef CIM_SoftwareElement Self;
    
    CIM_SoftwareElement_Class() :
        CIM_LogicalElement_Class(&CIM_SoftwareElement_rtti)
    {
    }
    
    CIM_SoftwareElement_Class(
        const CIM_SoftwareElement* instanceName,
        bool keysOnly) :
        CIM_LogicalElement_Class(
            &CIM_SoftwareElement_rtti,
            &instanceName->__instance,
            keysOnly)
    {
    }
    
    CIM_SoftwareElement_Class(
        const MI_ClassDecl* clDecl,
        const MI_Instance* instance,
        bool keysOnly) :
        CIM_LogicalElement_Class(clDecl, instance, keysOnly)
    {
    }
    
    CIM_SoftwareElement_Class(
        const MI_ClassDecl* clDecl) :
        CIM_LogicalElement_Class(clDecl)
    {
    }
    
    CIM_SoftwareElement_Class& operator=(
        const CIM_SoftwareElement_Class& x)
    {
        CopyRef(x);
        return *this;
    }
    
    CIM_SoftwareElement_Class(
        const CIM_SoftwareElement_Class& x) :
        CIM_LogicalElement_Class(x)
    {
    }

    static const MI_ClassDecl* GetClassDecl()
    {
        return &CIM_SoftwareElement_rtti;
    }

    //
    // CIM_SoftwareElement_Class.Version
    //
    
    const Field<String>& Version() const
    {
        const size_t n = offsetof(Self, Version);
        return GetField<String>(n);
    }
    
    void Version(const Field<String>& x)
    {
        const size_t n = offsetof(Self, Version);
        GetField<String>(n) = x;
    }
    
    const String& Version_value() const
    {
        const size_t n = offsetof(Self, Version);
        return GetField<String>(n).value;
    }
    
    void Version_value(const String& x)
    {
        const size_t n = offsetof(Self, Version);
        GetField<String>(n).Set(x);
    }
    
    bool Version_exists() const
    {
        const size_t n = offsetof(Self, Version);
        return GetField<String>(n).exists ? true : false;
    }
    
    void Version_clear()
    {
        const size_t n = offsetof(Self, Version);
        GetField<String>(n).Clear();
    }

    //
    // CIM_SoftwareElement_Class.SoftwareElementState
    //
    
    const Field<Uint16>& SoftwareElementState() const
    {
        const size_t n = offsetof(Self, SoftwareElementState);
        return GetField<Uint16>(n);
    }
    
    void SoftwareElementState(const Field<Uint16>& x)
    {
        const size_t n = offsetof(Self, SoftwareElementState);
        GetField<Uint16>(n) = x;
    }
    
    const Uint16& SoftwareElementState_value() const
    {
        const size_t n = offsetof(Self, SoftwareElementState);
        return GetField<Uint16>(n).value;
    }
    
    void SoftwareElementState_value(const Uint16& x)
    {
        const size_t n = offsetof(Self, SoftwareElementState);
        GetField<Uint16>(n).Set(x);
    }
    
    bool SoftwareElementState_exists() const
    {
        const size_t n = offsetof(Self, SoftwareElementState);
        return GetField<Uint16>(n).exists ? true : false;
    }
    
    void SoftwareElementState_clear()
    {
        const size_t n = offsetof(Self, SoftwareElementState);
        GetField<Uint16>(n).Clear();
    }

    //
    // CIM_SoftwareElement_Class.SoftwareElementID
    //
    
    const Field<String>& SoftwareElementID() const
    {
        const size_t n = offsetof(Self, SoftwareElementID);
        return GetField<String>(n);
    }
    
    void SoftwareElementID(const Field<String>& x)
    {
        const size_t n = offsetof(Self, SoftwareElementID);
        GetField<String>(n) = x;
    }
    
    const String& SoftwareElementID_value() const
    {
        const size_t n = offsetof(Self, SoftwareElementID);
        return GetField<String>(n).value;
    }
    
    void SoftwareElementID_value(const String& x)
    {
        const size_t n = offsetof(Self, SoftwareElementID);
        GetField<String>(n).Set(x);
    }
    
    bool SoftwareElementID_exists() const
    {
        const size_t n = offsetof(Self, SoftwareElementID);
        return GetField<String>(n).exists ? true : false;
    }
    
    void SoftwareElementID_clear()
    {
        const size_t n = offsetof(Self, SoftwareElementID);
        GetField<String>(n).Clear();
    }

    //
    // CIM_SoftwareElement_Class.TargetOperatingSystem
    //
    
    const Field<Uint16>& TargetOperatingSystem() const
    {
        const size_t n = offsetof(Self, TargetOperatingSystem);
        return GetField<Uint16>(n);
    }
    
    void TargetOperatingSystem(const Field<Uint16>& x)
    {
        const size_t n = offsetof(Self, TargetOperatingSystem);
        GetField<Uint16>(n) = x;
    }
    
    const Uint16& TargetOperatingSystem_value() const
    {
        const size_t n = offsetof(Self, TargetOperatingSystem);
        return GetField<Uint16>(n).value;
    }
    
    void TargetOperatingSystem_value(const Uint16& x)
    {
        const size_t n = offsetof(Self, TargetOperatingSystem);
        GetField<Uint16>(n).Set(x);
    }
    
    bool TargetOperatingSystem_exists() const
    {
        const size_t n = offsetof(Self, TargetOperatingSystem);
        return GetField<Uint16>(n).exists ? true : false;
    }
    
    void TargetOperatingSystem_clear()
    {
        const size_t n = offsetof(Self, TargetOperatingSystem);
        GetField<Uint16>(n).Clear();
    }

    //
    // CIM_SoftwareElement_Class.OtherTargetOS
    //
    
    const Field<String>& OtherTargetOS() const
    {
        const size_t n = offsetof(Self, OtherTargetOS);
        return GetField<String>(n);
    }
    
    void OtherTargetOS(const Field<String>& x)
    {
        const size_t n = offsetof(Self, OtherTargetOS);
        GetField<String>(n) = x;
    }
    
    const String& OtherTargetOS_value() const
    {
        const size_t n = offsetof(Self, OtherTargetOS);
        return GetField<String>(n).value;
    }
    
    void OtherTargetOS_value(const String& x)
    {
        const size_t n = offsetof(Self, OtherTargetOS);
        GetField<String>(n).Set(x);
    }
    
    bool OtherTargetOS_exists() const
    {
        const size_t n = offsetof(Self, OtherTargetOS);
        return GetField<String>(n).exists ? true : false;
    }
    
    void OtherTargetOS_clear()
    {
        const size_t n = offsetof(Self, OtherTargetOS);
        GetField<String>(n).Clear();
    }

    //
    // CIM_SoftwareElement_Class.Manufacturer
    //
    
    const Field<String>& Manufacturer() const
    {
        const size_t n = offsetof(Self, Manufacturer);
        return GetField<String>(n);
    }
    
    void Manufacturer(const Field<String>& x)
    {
        const size_t n = offsetof(Self, Manufacturer);
        GetField<String>(n) = x;
    }
    
    const String& Manufacturer_value() const
    {
        const size_t n = offsetof(Self, Manufacturer);
        return GetField<String>(n).value;
    }
    
    void Manufacturer_value(const String& x)
    {
        const size_t n = offsetof(Self, Manufacturer);
        GetField<String>(n).Set(x);
    }
    
    bool Manufacturer_exists() const
    {
        const size_t n = offsetof(Self, Manufacturer);
        return GetField<String>(n).exists ? true : false;
    }
    
    void Manufacturer_clear()
    {
        const size_t n = offsetof(Self, Manufacturer);
        GetField<String>(n).Clear();
    }

    //
    // CIM_SoftwareElement_Class.BuildNumber
    //
    
    const Field<String>& BuildNumber() const
    {
        const size_t n = offsetof(Self, BuildNumber);
        return GetField<String>(n);
    }
    
    void BuildNumber(const Field<String>& x)
    {
        const size_t n = offsetof(Self, BuildNumber);
        GetField<String>(n) = x;
    }
    
    const String& BuildNumber_value() const
    {
        const size_t n = offsetof(Self, BuildNumber);
        return GetField<String>(n).value;
    }
    
    void BuildNumber_value(const String& x)
    {
        const size_t n = offsetof(Self, BuildNumber);
        GetField<String>(n).Set(x);
    }
    
    bool BuildNumber_exists() const
    {
        const size_t n = offsetof(Self, BuildNumber);
        return GetField<String>(n).exists ? true : false;
    }
    
    void BuildNumber_clear()
    {
        const size_t n = offsetof(Self, BuildNumber);
        GetField<String>(n).Clear();
    }

    //
    // CIM_SoftwareElement_Class.SerialNumber
    //
    
    const Field<String>& SerialNumber() const
    {
        const size_t n = offsetof(Self, SerialNumber);
        return GetField<String>(n);
    }
    
    void SerialNumber(const Field<String>& x)
    {
        const size_t n = offsetof(Self, SerialNumber);
        GetField<String>(n) = x;
    }
    
    const String& SerialNumber_value() const
    {
        const size_t n = offsetof(Self, SerialNumber);
        return GetField<String>(n).value;
    }
    
    void SerialNumber_value(const String& x)
    {
        const size_t n = offsetof(Self, SerialNumber);
        GetField<String>(n).Set(x);
    }
    
    bool SerialNumber_exists() const
    {
        const size_t n = offsetof(Self, SerialNumber);
        return GetField<String>(n).exists ? true : false;
    }
    
    void SerialNumber_clear()
    {
        const size_t n = offsetof(Self, SerialNumber);
        GetField<String>(n).Clear();
    }

    //
    // CIM_SoftwareElement_Class.CodeSet
    //
    
    const Field<String>& CodeSet() const
    {
        const size_t n = offsetof(Self, CodeSet);
        return GetField<String>(n);
    }
    
    void CodeSet(const Field<String>& x)
    {
        const size_t n = offsetof(Self, CodeSet);
        GetField<String>(n) = x;
    }
    
    const String& CodeSet_value() const
    {
        const size_t n = offsetof(Self, CodeSet);
        return GetField<String>(n).value;
    }
    
    void CodeSet_value(const String& x)
    {
        const size_t n = offsetof(Self, CodeSet);
        GetField<String>(n).Set(x);
    }
    
    bool CodeSet_exists() const
    {
        const size_t n = offsetof(Self, CodeSet);
        return GetField<String>(n).exists ? true : false;
    }
    
    void CodeSet_clear()
    {
        const size_t n = offsetof(Self, CodeSet);
        GetField<String>(n).Clear();
    }

    //
    // CIM_SoftwareElement_Class.IdentificationCode
    //
    
    const Field<String>& IdentificationCode() const
    {
        const size_t n = offsetof(Self, IdentificationCode);
        return GetField<String>(n);
    }
    
    void IdentificationCode(const Field<String>& x)
    {
        const size_t n = offsetof(Self, IdentificationCode);
        GetField<String>(n) = x;
    }
    
    const String& IdentificationCode_value() const
    {
        const size_t n = offsetof(Self, IdentificationCode);
        return GetField<String>(n).value;
    }
    
    void IdentificationCode_value(const String& x)
    {
        const size_t n = offsetof(Self, IdentificationCode);
        GetField<String>(n).Set(x);
    }
    
    bool IdentificationCode_exists() const
    {
        const size_t n = offsetof(Self, IdentificationCode);
        return GetField<String>(n).exists ? true : false;
    }
    
    void IdentificationCode_clear()
    {
        const size_t n = offsetof(Self, IdentificationCode);
        GetField<String>(n).Clear();
    }

    //
    // CIM_SoftwareElement_Class.LanguageEdition
    //
    
    const Field<String>& LanguageEdition() const
    {
        const size_t n = offsetof(Self, LanguageEdition);
        return GetField<String>(n);
    }
    
    void LanguageEdition(const Field<String>& x)
    {
        const size_t n = offsetof(Self, LanguageEdition);
        GetField<String>(n) = x;
    }
    
    const String& LanguageEdition_value() const
    {
        const size_t n = offsetof(Self, LanguageEdition);
        return GetField<String>(n).value;
    }
    
    void LanguageEdition_value(const String& x)
    {
        const size_t n = offsetof(Self, LanguageEdition);
        GetField<String>(n).Set(x);
    }
    
    bool LanguageEdition_exists() const
    {
        const size_t n = offsetof(Self, LanguageEdition);
        return GetField<String>(n).exists ? true : false;
    }
    
    void LanguageEdition_clear()
    {
        const size_t n = offsetof(Self, LanguageEdition);
        GetField<String>(n).Clear();
    }
};

typedef Array<CIM_SoftwareElement_Class> CIM_SoftwareElement_ClassA;

MI_END_NAMESPACE

#endif /* __cplusplus */

#endif /* _CIM_SoftwareElement_h */
