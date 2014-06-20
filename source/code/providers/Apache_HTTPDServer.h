/* @migen@ */
/*
**==============================================================================
**
** WARNING: THIS FILE WAS AUTOMATICALLY GENERATED. PLEASE DO NOT EDIT.
**
**==============================================================================
*/
#ifndef _Apache_HTTPDServer_h
#define _Apache_HTTPDServer_h

#include <MI.h>
#include "CIM_InstalledProduct.h"

/*
**==============================================================================
**
** Apache_HTTPDServer [Apache_HTTPDServer]
**
** Keys:
**    ProductIdentifyingNumber
**    ProductName
**    ProductVendor
**    ProductVersion
**    SystemID
**    CollectionID
**
**==============================================================================
*/

typedef struct _Apache_HTTPDServer /* extends CIM_InstalledProduct */
{
    MI_Instance __instance;
    /* CIM_ManagedElement properties */
    MI_ConstStringField InstanceID;
    MI_ConstStringField Caption;
    MI_ConstStringField Description;
    MI_ConstStringField ElementName;
    /* CIM_Collection properties */
    /* CIM_InstalledProduct properties */
    /*KEY*/ MI_ConstStringField ProductIdentifyingNumber;
    /*KEY*/ MI_ConstStringField ProductName;
    /*KEY*/ MI_ConstStringField ProductVendor;
    /*KEY*/ MI_ConstStringField ProductVersion;
    /*KEY*/ MI_ConstStringField SystemID;
    /*KEY*/ MI_ConstStringField CollectionID;
    MI_ConstStringField Name;
    /* Apache_HTTPDServer properties */
    MI_ConstStringField ModuleVersion;
    MI_ConstStringField ConfigurationFile;
    MI_ConstStringAField InstalledModules;
    MI_ConstStringField InstalledModulesFormatted;
    MI_ConstStringField ProcessName;
    MI_ConstStringField ServiceName;
    MI_ConstStringField OperatingStatus;
    MI_ConstStringField OperatingStatusDetail;
}
Apache_HTTPDServer;

typedef struct _Apache_HTTPDServer_Ref
{
    Apache_HTTPDServer* value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
Apache_HTTPDServer_Ref;

typedef struct _Apache_HTTPDServer_ConstRef
{
    MI_CONST Apache_HTTPDServer* value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
Apache_HTTPDServer_ConstRef;

typedef struct _Apache_HTTPDServer_Array
{
    struct _Apache_HTTPDServer** data;
    MI_Uint32 size;
}
Apache_HTTPDServer_Array;

typedef struct _Apache_HTTPDServer_ConstArray
{
    struct _Apache_HTTPDServer MI_CONST* MI_CONST* data;
    MI_Uint32 size;
}
Apache_HTTPDServer_ConstArray;

typedef struct _Apache_HTTPDServer_ArrayRef
{
    Apache_HTTPDServer_Array value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
Apache_HTTPDServer_ArrayRef;

typedef struct _Apache_HTTPDServer_ConstArrayRef
{
    Apache_HTTPDServer_ConstArray value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
Apache_HTTPDServer_ConstArrayRef;

MI_EXTERN_C MI_CONST MI_ClassDecl Apache_HTTPDServer_rtti;

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Construct(
    Apache_HTTPDServer* self,
    MI_Context* context)
{
    return MI_ConstructInstance(context, &Apache_HTTPDServer_rtti,
        (MI_Instance*)&self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Clone(
    const Apache_HTTPDServer* self,
    Apache_HTTPDServer** newInstance)
{
    return MI_Instance_Clone(
        &self->__instance, (MI_Instance**)newInstance);
}

MI_INLINE MI_Boolean MI_CALL Apache_HTTPDServer_IsA(
    const MI_Instance* self)
{
    MI_Boolean res = MI_FALSE;
    return MI_Instance_IsA(self, &Apache_HTTPDServer_rtti, &res) == MI_RESULT_OK && res;
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Destruct(Apache_HTTPDServer* self)
{
    return MI_Instance_Destruct(&self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Delete(Apache_HTTPDServer* self)
{
    return MI_Instance_Delete(&self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Post(
    const Apache_HTTPDServer* self,
    MI_Context* context)
{
    return MI_PostInstance(context, &self->__instance);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Set_InstanceID(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        0,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_SetPtr_InstanceID(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        0,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Clear_InstanceID(
    Apache_HTTPDServer* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Set_Caption(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        1,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_SetPtr_Caption(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        1,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Clear_Caption(
    Apache_HTTPDServer* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        1);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Set_Description(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        2,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_SetPtr_Description(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        2,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Clear_Description(
    Apache_HTTPDServer* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        2);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Set_ElementName(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        3,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_SetPtr_ElementName(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        3,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Clear_ElementName(
    Apache_HTTPDServer* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        3);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Set_ProductIdentifyingNumber(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        4,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_SetPtr_ProductIdentifyingNumber(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        4,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Clear_ProductIdentifyingNumber(
    Apache_HTTPDServer* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        4);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Set_ProductName(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        5,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_SetPtr_ProductName(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        5,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Clear_ProductName(
    Apache_HTTPDServer* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        5);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Set_ProductVendor(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        6,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_SetPtr_ProductVendor(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        6,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Clear_ProductVendor(
    Apache_HTTPDServer* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        6);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Set_ProductVersion(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        7,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_SetPtr_ProductVersion(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        7,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Clear_ProductVersion(
    Apache_HTTPDServer* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        7);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Set_SystemID(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        8,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_SetPtr_SystemID(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        8,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Clear_SystemID(
    Apache_HTTPDServer* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        8);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Set_CollectionID(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        9,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_SetPtr_CollectionID(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        9,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Clear_CollectionID(
    Apache_HTTPDServer* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        9);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Set_Name(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        10,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_SetPtr_Name(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        10,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Clear_Name(
    Apache_HTTPDServer* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        10);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Set_ModuleVersion(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        11,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_SetPtr_ModuleVersion(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        11,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Clear_ModuleVersion(
    Apache_HTTPDServer* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        11);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Set_ConfigurationFile(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        12,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_SetPtr_ConfigurationFile(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        12,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Clear_ConfigurationFile(
    Apache_HTTPDServer* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        12);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Set_InstalledModules(
    Apache_HTTPDServer* self,
    const MI_Char** data,
    MI_Uint32 size)
{
    MI_Array arr;
    arr.data = (void*)data;
    arr.size = size;
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        13,
        (MI_Value*)&arr,
        MI_STRINGA,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_SetPtr_InstalledModules(
    Apache_HTTPDServer* self,
    const MI_Char** data,
    MI_Uint32 size)
{
    MI_Array arr;
    arr.data = (void*)data;
    arr.size = size;
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        13,
        (MI_Value*)&arr,
        MI_STRINGA,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Clear_InstalledModules(
    Apache_HTTPDServer* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        13);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Set_InstalledModulesFormatted(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        14,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_SetPtr_InstalledModulesFormatted(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        14,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Clear_InstalledModulesFormatted(
    Apache_HTTPDServer* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        14);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Set_ProcessName(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        15,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_SetPtr_ProcessName(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        15,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Clear_ProcessName(
    Apache_HTTPDServer* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        15);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Set_ServiceName(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        16,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_SetPtr_ServiceName(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        16,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Clear_ServiceName(
    Apache_HTTPDServer* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        16);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Set_OperatingStatus(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        17,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_SetPtr_OperatingStatus(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        17,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Clear_OperatingStatus(
    Apache_HTTPDServer* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        17);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Set_OperatingStatusDetail(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        18,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_SetPtr_OperatingStatusDetail(
    Apache_HTTPDServer* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        18,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL Apache_HTTPDServer_Clear_OperatingStatusDetail(
    Apache_HTTPDServer* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        18);
}

/*
**==============================================================================
**
** Apache_HTTPDServer provider function prototypes
**
**==============================================================================
*/

/* The developer may optionally define this structure */
typedef struct _Apache_HTTPDServer_Self Apache_HTTPDServer_Self;

MI_EXTERN_C void MI_CALL Apache_HTTPDServer_Load(
    Apache_HTTPDServer_Self** self,
    MI_Module_Self* selfModule,
    MI_Context* context);

MI_EXTERN_C void MI_CALL Apache_HTTPDServer_Unload(
    Apache_HTTPDServer_Self* self,
    MI_Context* context);

MI_EXTERN_C void MI_CALL Apache_HTTPDServer_EnumerateInstances(
    Apache_HTTPDServer_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const MI_PropertySet* propertySet,
    MI_Boolean keysOnly,
    const MI_Filter* filter);

MI_EXTERN_C void MI_CALL Apache_HTTPDServer_GetInstance(
    Apache_HTTPDServer_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDServer* instanceName,
    const MI_PropertySet* propertySet);

MI_EXTERN_C void MI_CALL Apache_HTTPDServer_CreateInstance(
    Apache_HTTPDServer_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDServer* newInstance);

MI_EXTERN_C void MI_CALL Apache_HTTPDServer_ModifyInstance(
    Apache_HTTPDServer_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDServer* modifiedInstance,
    const MI_PropertySet* propertySet);

MI_EXTERN_C void MI_CALL Apache_HTTPDServer_DeleteInstance(
    Apache_HTTPDServer_Self* self,
    MI_Context* context,
    const MI_Char* nameSpace,
    const MI_Char* className,
    const Apache_HTTPDServer* instanceName);


/*
**==============================================================================
**
** Apache_HTTPDServer_Class
**
**==============================================================================
*/

#ifdef __cplusplus
# include <micxx/micxx.h>

MI_BEGIN_NAMESPACE

class Apache_HTTPDServer_Class : public CIM_InstalledProduct_Class
{
public:
    
    typedef Apache_HTTPDServer Self;
    
    Apache_HTTPDServer_Class() :
        CIM_InstalledProduct_Class(&Apache_HTTPDServer_rtti)
    {
    }
    
    Apache_HTTPDServer_Class(
        const Apache_HTTPDServer* instanceName,
        bool keysOnly) :
        CIM_InstalledProduct_Class(
            &Apache_HTTPDServer_rtti,
            &instanceName->__instance,
            keysOnly)
    {
    }
    
    Apache_HTTPDServer_Class(
        const MI_ClassDecl* clDecl,
        const MI_Instance* instance,
        bool keysOnly) :
        CIM_InstalledProduct_Class(clDecl, instance, keysOnly)
    {
    }
    
    Apache_HTTPDServer_Class(
        const MI_ClassDecl* clDecl) :
        CIM_InstalledProduct_Class(clDecl)
    {
    }
    
    Apache_HTTPDServer_Class& operator=(
        const Apache_HTTPDServer_Class& x)
    {
        CopyRef(x);
        return *this;
    }
    
    Apache_HTTPDServer_Class(
        const Apache_HTTPDServer_Class& x) :
        CIM_InstalledProduct_Class(x)
    {
    }

    static const MI_ClassDecl* GetClassDecl()
    {
        return &Apache_HTTPDServer_rtti;
    }

    //
    // Apache_HTTPDServer_Class.ModuleVersion
    //
    
    const Field<String>& ModuleVersion() const
    {
        const size_t n = offsetof(Self, ModuleVersion);
        return GetField<String>(n);
    }
    
    void ModuleVersion(const Field<String>& x)
    {
        const size_t n = offsetof(Self, ModuleVersion);
        GetField<String>(n) = x;
    }
    
    const String& ModuleVersion_value() const
    {
        const size_t n = offsetof(Self, ModuleVersion);
        return GetField<String>(n).value;
    }
    
    void ModuleVersion_value(const String& x)
    {
        const size_t n = offsetof(Self, ModuleVersion);
        GetField<String>(n).Set(x);
    }
    
    bool ModuleVersion_exists() const
    {
        const size_t n = offsetof(Self, ModuleVersion);
        return GetField<String>(n).exists ? true : false;
    }
    
    void ModuleVersion_clear()
    {
        const size_t n = offsetof(Self, ModuleVersion);
        GetField<String>(n).Clear();
    }

    //
    // Apache_HTTPDServer_Class.ConfigurationFile
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

    //
    // Apache_HTTPDServer_Class.InstalledModules
    //
    
    const Field<StringA>& InstalledModules() const
    {
        const size_t n = offsetof(Self, InstalledModules);
        return GetField<StringA>(n);
    }
    
    void InstalledModules(const Field<StringA>& x)
    {
        const size_t n = offsetof(Self, InstalledModules);
        GetField<StringA>(n) = x;
    }
    
    const StringA& InstalledModules_value() const
    {
        const size_t n = offsetof(Self, InstalledModules);
        return GetField<StringA>(n).value;
    }
    
    void InstalledModules_value(const StringA& x)
    {
        const size_t n = offsetof(Self, InstalledModules);
        GetField<StringA>(n).Set(x);
    }
    
    bool InstalledModules_exists() const
    {
        const size_t n = offsetof(Self, InstalledModules);
        return GetField<StringA>(n).exists ? true : false;
    }
    
    void InstalledModules_clear()
    {
        const size_t n = offsetof(Self, InstalledModules);
        GetField<StringA>(n).Clear();
    }

    //
    // Apache_HTTPDServer_Class.InstalledModulesFormatted
    //
    
    const Field<String>& InstalledModulesFormatted() const
    {
        const size_t n = offsetof(Self, InstalledModulesFormatted);
        return GetField<String>(n);
    }
    
    void InstalledModulesFormatted(const Field<String>& x)
    {
        const size_t n = offsetof(Self, InstalledModulesFormatted);
        GetField<String>(n) = x;
    }
    
    const String& InstalledModulesFormatted_value() const
    {
        const size_t n = offsetof(Self, InstalledModulesFormatted);
        return GetField<String>(n).value;
    }
    
    void InstalledModulesFormatted_value(const String& x)
    {
        const size_t n = offsetof(Self, InstalledModulesFormatted);
        GetField<String>(n).Set(x);
    }
    
    bool InstalledModulesFormatted_exists() const
    {
        const size_t n = offsetof(Self, InstalledModulesFormatted);
        return GetField<String>(n).exists ? true : false;
    }
    
    void InstalledModulesFormatted_clear()
    {
        const size_t n = offsetof(Self, InstalledModulesFormatted);
        GetField<String>(n).Clear();
    }

    //
    // Apache_HTTPDServer_Class.ProcessName
    //
    
    const Field<String>& ProcessName() const
    {
        const size_t n = offsetof(Self, ProcessName);
        return GetField<String>(n);
    }
    
    void ProcessName(const Field<String>& x)
    {
        const size_t n = offsetof(Self, ProcessName);
        GetField<String>(n) = x;
    }
    
    const String& ProcessName_value() const
    {
        const size_t n = offsetof(Self, ProcessName);
        return GetField<String>(n).value;
    }
    
    void ProcessName_value(const String& x)
    {
        const size_t n = offsetof(Self, ProcessName);
        GetField<String>(n).Set(x);
    }
    
    bool ProcessName_exists() const
    {
        const size_t n = offsetof(Self, ProcessName);
        return GetField<String>(n).exists ? true : false;
    }
    
    void ProcessName_clear()
    {
        const size_t n = offsetof(Self, ProcessName);
        GetField<String>(n).Clear();
    }

    //
    // Apache_HTTPDServer_Class.ServiceName
    //
    
    const Field<String>& ServiceName() const
    {
        const size_t n = offsetof(Self, ServiceName);
        return GetField<String>(n);
    }
    
    void ServiceName(const Field<String>& x)
    {
        const size_t n = offsetof(Self, ServiceName);
        GetField<String>(n) = x;
    }
    
    const String& ServiceName_value() const
    {
        const size_t n = offsetof(Self, ServiceName);
        return GetField<String>(n).value;
    }
    
    void ServiceName_value(const String& x)
    {
        const size_t n = offsetof(Self, ServiceName);
        GetField<String>(n).Set(x);
    }
    
    bool ServiceName_exists() const
    {
        const size_t n = offsetof(Self, ServiceName);
        return GetField<String>(n).exists ? true : false;
    }
    
    void ServiceName_clear()
    {
        const size_t n = offsetof(Self, ServiceName);
        GetField<String>(n).Clear();
    }

    //
    // Apache_HTTPDServer_Class.OperatingStatus
    //
    
    const Field<String>& OperatingStatus() const
    {
        const size_t n = offsetof(Self, OperatingStatus);
        return GetField<String>(n);
    }
    
    void OperatingStatus(const Field<String>& x)
    {
        const size_t n = offsetof(Self, OperatingStatus);
        GetField<String>(n) = x;
    }
    
    const String& OperatingStatus_value() const
    {
        const size_t n = offsetof(Self, OperatingStatus);
        return GetField<String>(n).value;
    }
    
    void OperatingStatus_value(const String& x)
    {
        const size_t n = offsetof(Self, OperatingStatus);
        GetField<String>(n).Set(x);
    }
    
    bool OperatingStatus_exists() const
    {
        const size_t n = offsetof(Self, OperatingStatus);
        return GetField<String>(n).exists ? true : false;
    }
    
    void OperatingStatus_clear()
    {
        const size_t n = offsetof(Self, OperatingStatus);
        GetField<String>(n).Clear();
    }

    //
    // Apache_HTTPDServer_Class.OperatingStatusDetail
    //
    
    const Field<String>& OperatingStatusDetail() const
    {
        const size_t n = offsetof(Self, OperatingStatusDetail);
        return GetField<String>(n);
    }
    
    void OperatingStatusDetail(const Field<String>& x)
    {
        const size_t n = offsetof(Self, OperatingStatusDetail);
        GetField<String>(n) = x;
    }
    
    const String& OperatingStatusDetail_value() const
    {
        const size_t n = offsetof(Self, OperatingStatusDetail);
        return GetField<String>(n).value;
    }
    
    void OperatingStatusDetail_value(const String& x)
    {
        const size_t n = offsetof(Self, OperatingStatusDetail);
        GetField<String>(n).Set(x);
    }
    
    bool OperatingStatusDetail_exists() const
    {
        const size_t n = offsetof(Self, OperatingStatusDetail);
        return GetField<String>(n).exists ? true : false;
    }
    
    void OperatingStatusDetail_clear()
    {
        const size_t n = offsetof(Self, OperatingStatusDetail);
        GetField<String>(n).Clear();
    }
};

typedef Array<Apache_HTTPDServer_Class> Apache_HTTPDServer_ClassA;

MI_END_NAMESPACE

#endif /* __cplusplus */

#endif /* _Apache_HTTPDServer_h */
