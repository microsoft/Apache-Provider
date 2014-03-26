/* @migen@ */
/*
**==============================================================================
**
** WARNING: THIS FILE WAS AUTOMATICALLY GENERATED. PLEASE DO NOT EDIT.
**
**==============================================================================
*/
#ifndef _CIM_InstalledProduct_h
#define _CIM_InstalledProduct_h

#include <MI.h>
#include "CIM_Collection.h"

/*
**==============================================================================
**
** CIM_InstalledProduct [CIM_InstalledProduct]
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

typedef struct _CIM_InstalledProduct /* extends CIM_Collection */
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
}
CIM_InstalledProduct;

typedef struct _CIM_InstalledProduct_Ref
{
    CIM_InstalledProduct* value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
CIM_InstalledProduct_Ref;

typedef struct _CIM_InstalledProduct_ConstRef
{
    MI_CONST CIM_InstalledProduct* value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
CIM_InstalledProduct_ConstRef;

typedef struct _CIM_InstalledProduct_Array
{
    struct _CIM_InstalledProduct** data;
    MI_Uint32 size;
}
CIM_InstalledProduct_Array;

typedef struct _CIM_InstalledProduct_ConstArray
{
    struct _CIM_InstalledProduct MI_CONST* MI_CONST* data;
    MI_Uint32 size;
}
CIM_InstalledProduct_ConstArray;

typedef struct _CIM_InstalledProduct_ArrayRef
{
    CIM_InstalledProduct_Array value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
CIM_InstalledProduct_ArrayRef;

typedef struct _CIM_InstalledProduct_ConstArrayRef
{
    CIM_InstalledProduct_ConstArray value;
    MI_Boolean exists;
    MI_Uint8 flags;
}
CIM_InstalledProduct_ConstArrayRef;

MI_EXTERN_C MI_CONST MI_ClassDecl CIM_InstalledProduct_rtti;

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_Construct(
    CIM_InstalledProduct* self,
    MI_Context* context)
{
    return MI_ConstructInstance(context, &CIM_InstalledProduct_rtti,
        (MI_Instance*)&self->__instance);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_Clone(
    const CIM_InstalledProduct* self,
    CIM_InstalledProduct** newInstance)
{
    return MI_Instance_Clone(
        &self->__instance, (MI_Instance**)newInstance);
}

MI_INLINE MI_Boolean MI_CALL CIM_InstalledProduct_IsA(
    const MI_Instance* self)
{
    MI_Boolean res = MI_FALSE;
    return MI_Instance_IsA(self, &CIM_InstalledProduct_rtti, &res) == MI_RESULT_OK && res;
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_Destruct(CIM_InstalledProduct* self)
{
    return MI_Instance_Destruct(&self->__instance);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_Delete(CIM_InstalledProduct* self)
{
    return MI_Instance_Delete(&self->__instance);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_Post(
    const CIM_InstalledProduct* self,
    MI_Context* context)
{
    return MI_PostInstance(context, &self->__instance);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_Set_InstanceID(
    CIM_InstalledProduct* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        0,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_SetPtr_InstanceID(
    CIM_InstalledProduct* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        0,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_Clear_InstanceID(
    CIM_InstalledProduct* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        0);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_Set_Caption(
    CIM_InstalledProduct* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        1,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_SetPtr_Caption(
    CIM_InstalledProduct* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        1,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_Clear_Caption(
    CIM_InstalledProduct* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        1);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_Set_Description(
    CIM_InstalledProduct* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        2,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_SetPtr_Description(
    CIM_InstalledProduct* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        2,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_Clear_Description(
    CIM_InstalledProduct* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        2);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_Set_ElementName(
    CIM_InstalledProduct* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        3,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_SetPtr_ElementName(
    CIM_InstalledProduct* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        3,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_Clear_ElementName(
    CIM_InstalledProduct* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        3);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_Set_ProductIdentifyingNumber(
    CIM_InstalledProduct* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        4,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_SetPtr_ProductIdentifyingNumber(
    CIM_InstalledProduct* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        4,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_Clear_ProductIdentifyingNumber(
    CIM_InstalledProduct* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        4);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_Set_ProductName(
    CIM_InstalledProduct* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        5,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_SetPtr_ProductName(
    CIM_InstalledProduct* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        5,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_Clear_ProductName(
    CIM_InstalledProduct* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        5);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_Set_ProductVendor(
    CIM_InstalledProduct* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        6,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_SetPtr_ProductVendor(
    CIM_InstalledProduct* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        6,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_Clear_ProductVendor(
    CIM_InstalledProduct* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        6);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_Set_ProductVersion(
    CIM_InstalledProduct* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        7,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_SetPtr_ProductVersion(
    CIM_InstalledProduct* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        7,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_Clear_ProductVersion(
    CIM_InstalledProduct* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        7);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_Set_SystemID(
    CIM_InstalledProduct* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        8,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_SetPtr_SystemID(
    CIM_InstalledProduct* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        8,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_Clear_SystemID(
    CIM_InstalledProduct* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        8);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_Set_CollectionID(
    CIM_InstalledProduct* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        9,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_SetPtr_CollectionID(
    CIM_InstalledProduct* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        9,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_Clear_CollectionID(
    CIM_InstalledProduct* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        9);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_Set_Name(
    CIM_InstalledProduct* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        10,
        (MI_Value*)&str,
        MI_STRING,
        0);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_SetPtr_Name(
    CIM_InstalledProduct* self,
    const MI_Char* str)
{
    return self->__instance.ft->SetElementAt(
        (MI_Instance*)&self->__instance,
        10,
        (MI_Value*)&str,
        MI_STRING,
        MI_FLAG_BORROW);
}

MI_INLINE MI_Result MI_CALL CIM_InstalledProduct_Clear_Name(
    CIM_InstalledProduct* self)
{
    return self->__instance.ft->ClearElementAt(
        (MI_Instance*)&self->__instance,
        10);
}


/*
**==============================================================================
**
** CIM_InstalledProduct_Class
**
**==============================================================================
*/

#ifdef __cplusplus
# include <micxx/micxx.h>

MI_BEGIN_NAMESPACE

class CIM_InstalledProduct_Class : public CIM_Collection_Class
{
public:
    
    typedef CIM_InstalledProduct Self;
    
    CIM_InstalledProduct_Class() :
        CIM_Collection_Class(&CIM_InstalledProduct_rtti)
    {
    }
    
    CIM_InstalledProduct_Class(
        const CIM_InstalledProduct* instanceName,
        bool keysOnly) :
        CIM_Collection_Class(
            &CIM_InstalledProduct_rtti,
            &instanceName->__instance,
            keysOnly)
    {
    }
    
    CIM_InstalledProduct_Class(
        const MI_ClassDecl* clDecl,
        const MI_Instance* instance,
        bool keysOnly) :
        CIM_Collection_Class(clDecl, instance, keysOnly)
    {
    }
    
    CIM_InstalledProduct_Class(
        const MI_ClassDecl* clDecl) :
        CIM_Collection_Class(clDecl)
    {
    }
    
    CIM_InstalledProduct_Class& operator=(
        const CIM_InstalledProduct_Class& x)
    {
        CopyRef(x);
        return *this;
    }
    
    CIM_InstalledProduct_Class(
        const CIM_InstalledProduct_Class& x) :
        CIM_Collection_Class(x)
    {
    }

    static const MI_ClassDecl* GetClassDecl()
    {
        return &CIM_InstalledProduct_rtti;
    }

    //
    // CIM_InstalledProduct_Class.ProductIdentifyingNumber
    //
    
    const Field<String>& ProductIdentifyingNumber() const
    {
        const size_t n = offsetof(Self, ProductIdentifyingNumber);
        return GetField<String>(n);
    }
    
    void ProductIdentifyingNumber(const Field<String>& x)
    {
        const size_t n = offsetof(Self, ProductIdentifyingNumber);
        GetField<String>(n) = x;
    }
    
    const String& ProductIdentifyingNumber_value() const
    {
        const size_t n = offsetof(Self, ProductIdentifyingNumber);
        return GetField<String>(n).value;
    }
    
    void ProductIdentifyingNumber_value(const String& x)
    {
        const size_t n = offsetof(Self, ProductIdentifyingNumber);
        GetField<String>(n).Set(x);
    }
    
    bool ProductIdentifyingNumber_exists() const
    {
        const size_t n = offsetof(Self, ProductIdentifyingNumber);
        return GetField<String>(n).exists ? true : false;
    }
    
    void ProductIdentifyingNumber_clear()
    {
        const size_t n = offsetof(Self, ProductIdentifyingNumber);
        GetField<String>(n).Clear();
    }

    //
    // CIM_InstalledProduct_Class.ProductName
    //
    
    const Field<String>& ProductName() const
    {
        const size_t n = offsetof(Self, ProductName);
        return GetField<String>(n);
    }
    
    void ProductName(const Field<String>& x)
    {
        const size_t n = offsetof(Self, ProductName);
        GetField<String>(n) = x;
    }
    
    const String& ProductName_value() const
    {
        const size_t n = offsetof(Self, ProductName);
        return GetField<String>(n).value;
    }
    
    void ProductName_value(const String& x)
    {
        const size_t n = offsetof(Self, ProductName);
        GetField<String>(n).Set(x);
    }
    
    bool ProductName_exists() const
    {
        const size_t n = offsetof(Self, ProductName);
        return GetField<String>(n).exists ? true : false;
    }
    
    void ProductName_clear()
    {
        const size_t n = offsetof(Self, ProductName);
        GetField<String>(n).Clear();
    }

    //
    // CIM_InstalledProduct_Class.ProductVendor
    //
    
    const Field<String>& ProductVendor() const
    {
        const size_t n = offsetof(Self, ProductVendor);
        return GetField<String>(n);
    }
    
    void ProductVendor(const Field<String>& x)
    {
        const size_t n = offsetof(Self, ProductVendor);
        GetField<String>(n) = x;
    }
    
    const String& ProductVendor_value() const
    {
        const size_t n = offsetof(Self, ProductVendor);
        return GetField<String>(n).value;
    }
    
    void ProductVendor_value(const String& x)
    {
        const size_t n = offsetof(Self, ProductVendor);
        GetField<String>(n).Set(x);
    }
    
    bool ProductVendor_exists() const
    {
        const size_t n = offsetof(Self, ProductVendor);
        return GetField<String>(n).exists ? true : false;
    }
    
    void ProductVendor_clear()
    {
        const size_t n = offsetof(Self, ProductVendor);
        GetField<String>(n).Clear();
    }

    //
    // CIM_InstalledProduct_Class.ProductVersion
    //
    
    const Field<String>& ProductVersion() const
    {
        const size_t n = offsetof(Self, ProductVersion);
        return GetField<String>(n);
    }
    
    void ProductVersion(const Field<String>& x)
    {
        const size_t n = offsetof(Self, ProductVersion);
        GetField<String>(n) = x;
    }
    
    const String& ProductVersion_value() const
    {
        const size_t n = offsetof(Self, ProductVersion);
        return GetField<String>(n).value;
    }
    
    void ProductVersion_value(const String& x)
    {
        const size_t n = offsetof(Self, ProductVersion);
        GetField<String>(n).Set(x);
    }
    
    bool ProductVersion_exists() const
    {
        const size_t n = offsetof(Self, ProductVersion);
        return GetField<String>(n).exists ? true : false;
    }
    
    void ProductVersion_clear()
    {
        const size_t n = offsetof(Self, ProductVersion);
        GetField<String>(n).Clear();
    }

    //
    // CIM_InstalledProduct_Class.SystemID
    //
    
    const Field<String>& SystemID() const
    {
        const size_t n = offsetof(Self, SystemID);
        return GetField<String>(n);
    }
    
    void SystemID(const Field<String>& x)
    {
        const size_t n = offsetof(Self, SystemID);
        GetField<String>(n) = x;
    }
    
    const String& SystemID_value() const
    {
        const size_t n = offsetof(Self, SystemID);
        return GetField<String>(n).value;
    }
    
    void SystemID_value(const String& x)
    {
        const size_t n = offsetof(Self, SystemID);
        GetField<String>(n).Set(x);
    }
    
    bool SystemID_exists() const
    {
        const size_t n = offsetof(Self, SystemID);
        return GetField<String>(n).exists ? true : false;
    }
    
    void SystemID_clear()
    {
        const size_t n = offsetof(Self, SystemID);
        GetField<String>(n).Clear();
    }

    //
    // CIM_InstalledProduct_Class.CollectionID
    //
    
    const Field<String>& CollectionID() const
    {
        const size_t n = offsetof(Self, CollectionID);
        return GetField<String>(n);
    }
    
    void CollectionID(const Field<String>& x)
    {
        const size_t n = offsetof(Self, CollectionID);
        GetField<String>(n) = x;
    }
    
    const String& CollectionID_value() const
    {
        const size_t n = offsetof(Self, CollectionID);
        return GetField<String>(n).value;
    }
    
    void CollectionID_value(const String& x)
    {
        const size_t n = offsetof(Self, CollectionID);
        GetField<String>(n).Set(x);
    }
    
    bool CollectionID_exists() const
    {
        const size_t n = offsetof(Self, CollectionID);
        return GetField<String>(n).exists ? true : false;
    }
    
    void CollectionID_clear()
    {
        const size_t n = offsetof(Self, CollectionID);
        GetField<String>(n).Clear();
    }

    //
    // CIM_InstalledProduct_Class.Name
    //
    
    const Field<String>& Name() const
    {
        const size_t n = offsetof(Self, Name);
        return GetField<String>(n);
    }
    
    void Name(const Field<String>& x)
    {
        const size_t n = offsetof(Self, Name);
        GetField<String>(n) = x;
    }
    
    const String& Name_value() const
    {
        const size_t n = offsetof(Self, Name);
        return GetField<String>(n).value;
    }
    
    void Name_value(const String& x)
    {
        const size_t n = offsetof(Self, Name);
        GetField<String>(n).Set(x);
    }
    
    bool Name_exists() const
    {
        const size_t n = offsetof(Self, Name);
        return GetField<String>(n).exists ? true : false;
    }
    
    void Name_clear()
    {
        const size_t n = offsetof(Self, Name);
        GetField<String>(n).Clear();
    }
};

typedef Array<CIM_InstalledProduct_Class> CIM_InstalledProduct_ClassA;

MI_END_NAMESPACE

#endif /* __cplusplus */

#endif /* _CIM_InstalledProduct_h */
