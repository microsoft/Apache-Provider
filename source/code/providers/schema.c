/* @migen@ */
/*
**==============================================================================
**
** WARNING: THIS FILE WAS AUTOMATICALLY GENERATED. PLEASE DO NOT EDIT.
**
**==============================================================================
*/
#include <ctype.h>
#include <MI.h>
#include "Apache_HTTPDServer.h"
#include "Apache_HTTPDServerStatistics.h"
#include "Apache_HTTPDVirtualHost.h"
#include "Apache_HTTPDVirtualHostCertificate.h"
#include "Apache_HTTPDVirtualHostStatistics.h"

/*
**==============================================================================
**
** Schema Declaration
**
**==============================================================================
*/

extern MI_SchemaDecl schemaDecl;

/*
**==============================================================================
**
** Qualifier declarations
**
**==============================================================================
*/

/*
**==============================================================================
**
** CIM_ManagedElement
**
**==============================================================================
*/

/* property CIM_ManagedElement.InstanceID */
static MI_CONST MI_PropertyDecl CIM_ManagedElement_InstanceID_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0069640A, /* code */
    MI_T("InstanceID"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_ManagedElement, InstanceID), /* offset */
    MI_T("CIM_ManagedElement"), /* origin */
    MI_T("CIM_ManagedElement"), /* propagator */
    NULL,
};

static MI_CONST MI_Uint32 CIM_ManagedElement_Caption_MaxLen_qual_value = 64U;

static MI_CONST MI_Qualifier CIM_ManagedElement_Caption_MaxLen_qual =
{
    MI_T("MaxLen"),
    MI_UINT32,
    0,
    &CIM_ManagedElement_Caption_MaxLen_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_ManagedElement_Caption_quals[] =
{
    &CIM_ManagedElement_Caption_MaxLen_qual,
};

/* property CIM_ManagedElement.Caption */
static MI_CONST MI_PropertyDecl CIM_ManagedElement_Caption_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00636E07, /* code */
    MI_T("Caption"), /* name */
    CIM_ManagedElement_Caption_quals, /* qualifiers */
    MI_COUNT(CIM_ManagedElement_Caption_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_ManagedElement, Caption), /* offset */
    MI_T("CIM_ManagedElement"), /* origin */
    MI_T("CIM_ManagedElement"), /* propagator */
    NULL,
};

/* property CIM_ManagedElement.Description */
static MI_CONST MI_PropertyDecl CIM_ManagedElement_Description_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00646E0B, /* code */
    MI_T("Description"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_ManagedElement, Description), /* offset */
    MI_T("CIM_ManagedElement"), /* origin */
    MI_T("CIM_ManagedElement"), /* propagator */
    NULL,
};

/* property CIM_ManagedElement.ElementName */
static MI_CONST MI_PropertyDecl CIM_ManagedElement_ElementName_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0065650B, /* code */
    MI_T("ElementName"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_ManagedElement, ElementName), /* offset */
    MI_T("CIM_ManagedElement"), /* origin */
    MI_T("CIM_ManagedElement"), /* propagator */
    NULL,
};

static MI_PropertyDecl MI_CONST* MI_CONST CIM_ManagedElement_props[] =
{
    &CIM_ManagedElement_InstanceID_prop,
    &CIM_ManagedElement_Caption_prop,
    &CIM_ManagedElement_Description_prop,
    &CIM_ManagedElement_ElementName_prop,
};

static MI_CONST MI_Char* CIM_ManagedElement_Version_qual_value = MI_T("2.19.0");

static MI_CONST MI_Qualifier CIM_ManagedElement_Version_qual =
{
    MI_T("Version"),
    MI_STRING,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_TRANSLATABLE|MI_FLAG_RESTRICTED,
    &CIM_ManagedElement_Version_qual_value
};

static MI_CONST MI_Char* CIM_ManagedElement_UMLPackagePath_qual_value = MI_T("CIM::Core::CoreElements");

static MI_CONST MI_Qualifier CIM_ManagedElement_UMLPackagePath_qual =
{
    MI_T("UMLPackagePath"),
    MI_STRING,
    0,
    &CIM_ManagedElement_UMLPackagePath_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_ManagedElement_quals[] =
{
    &CIM_ManagedElement_Version_qual,
    &CIM_ManagedElement_UMLPackagePath_qual,
};

/* class CIM_ManagedElement */
MI_CONST MI_ClassDecl CIM_ManagedElement_rtti =
{
    MI_FLAG_CLASS|MI_FLAG_ABSTRACT, /* flags */
    0x00637412, /* code */
    MI_T("CIM_ManagedElement"), /* name */
    CIM_ManagedElement_quals, /* qualifiers */
    MI_COUNT(CIM_ManagedElement_quals), /* numQualifiers */
    CIM_ManagedElement_props, /* properties */
    MI_COUNT(CIM_ManagedElement_props), /* numProperties */
    sizeof(CIM_ManagedElement), /* size */
    NULL, /* superClass */
    NULL, /* superClassDecl */
    NULL, /* methods */
    0, /* numMethods */
    &schemaDecl, /* schema */
    NULL, /* functions */
    NULL, /* owningClass */
};

/*
**==============================================================================
**
** CIM_Collection
**
**==============================================================================
*/

static MI_PropertyDecl MI_CONST* MI_CONST CIM_Collection_props[] =
{
    &CIM_ManagedElement_InstanceID_prop,
    &CIM_ManagedElement_Caption_prop,
    &CIM_ManagedElement_Description_prop,
    &CIM_ManagedElement_ElementName_prop,
};

static MI_CONST MI_Char* CIM_Collection_UMLPackagePath_qual_value = MI_T("CIM::Core::Collection");

static MI_CONST MI_Qualifier CIM_Collection_UMLPackagePath_qual =
{
    MI_T("UMLPackagePath"),
    MI_STRING,
    0,
    &CIM_Collection_UMLPackagePath_qual_value
};

static MI_CONST MI_Char* CIM_Collection_Version_qual_value = MI_T("2.6.0");

static MI_CONST MI_Qualifier CIM_Collection_Version_qual =
{
    MI_T("Version"),
    MI_STRING,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_TRANSLATABLE|MI_FLAG_RESTRICTED,
    &CIM_Collection_Version_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Collection_quals[] =
{
    &CIM_Collection_UMLPackagePath_qual,
    &CIM_Collection_Version_qual,
};

/* class CIM_Collection */
MI_CONST MI_ClassDecl CIM_Collection_rtti =
{
    MI_FLAG_CLASS|MI_FLAG_ABSTRACT, /* flags */
    0x00636E0E, /* code */
    MI_T("CIM_Collection"), /* name */
    CIM_Collection_quals, /* qualifiers */
    MI_COUNT(CIM_Collection_quals), /* numQualifiers */
    CIM_Collection_props, /* properties */
    MI_COUNT(CIM_Collection_props), /* numProperties */
    sizeof(CIM_Collection), /* size */
    MI_T("CIM_ManagedElement"), /* superClass */
    &CIM_ManagedElement_rtti, /* superClassDecl */
    NULL, /* methods */
    0, /* numMethods */
    &schemaDecl, /* schema */
    NULL, /* functions */
    NULL, /* owningClass */
};

/*
**==============================================================================
**
** CIM_InstalledProduct
**
**==============================================================================
*/

static MI_CONST MI_Uint32 CIM_InstalledProduct_ProductIdentifyingNumber_MaxLen_qual_value = 64U;

static MI_CONST MI_Qualifier CIM_InstalledProduct_ProductIdentifyingNumber_MaxLen_qual =
{
    MI_T("MaxLen"),
    MI_UINT32,
    0,
    &CIM_InstalledProduct_ProductIdentifyingNumber_MaxLen_qual_value
};

static MI_CONST MI_Char* CIM_InstalledProduct_ProductIdentifyingNumber_Propagated_qual_value = MI_T("CIM_Product.IdentifyingNumber");

static MI_CONST MI_Qualifier CIM_InstalledProduct_ProductIdentifyingNumber_Propagated_qual =
{
    MI_T("Propagated"),
    MI_STRING,
    MI_FLAG_DISABLEOVERRIDE|MI_FLAG_TOSUBCLASS,
    &CIM_InstalledProduct_ProductIdentifyingNumber_Propagated_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_InstalledProduct_ProductIdentifyingNumber_quals[] =
{
    &CIM_InstalledProduct_ProductIdentifyingNumber_MaxLen_qual,
    &CIM_InstalledProduct_ProductIdentifyingNumber_Propagated_qual,
};

/* property CIM_InstalledProduct.ProductIdentifyingNumber */
static MI_CONST MI_PropertyDecl CIM_InstalledProduct_ProductIdentifyingNumber_prop =
{
    MI_FLAG_PROPERTY|MI_FLAG_KEY, /* flags */
    0x00707218, /* code */
    MI_T("ProductIdentifyingNumber"), /* name */
    CIM_InstalledProduct_ProductIdentifyingNumber_quals, /* qualifiers */
    MI_COUNT(CIM_InstalledProduct_ProductIdentifyingNumber_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_InstalledProduct, ProductIdentifyingNumber), /* offset */
    MI_T("CIM_InstalledProduct"), /* origin */
    MI_T("CIM_InstalledProduct"), /* propagator */
    NULL,
};

static MI_CONST MI_Uint32 CIM_InstalledProduct_ProductName_MaxLen_qual_value = 256U;

static MI_CONST MI_Qualifier CIM_InstalledProduct_ProductName_MaxLen_qual =
{
    MI_T("MaxLen"),
    MI_UINT32,
    0,
    &CIM_InstalledProduct_ProductName_MaxLen_qual_value
};

static MI_CONST MI_Char* CIM_InstalledProduct_ProductName_Propagated_qual_value = MI_T("CIM_Product.Name");

static MI_CONST MI_Qualifier CIM_InstalledProduct_ProductName_Propagated_qual =
{
    MI_T("Propagated"),
    MI_STRING,
    MI_FLAG_DISABLEOVERRIDE|MI_FLAG_TOSUBCLASS,
    &CIM_InstalledProduct_ProductName_Propagated_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_InstalledProduct_ProductName_quals[] =
{
    &CIM_InstalledProduct_ProductName_MaxLen_qual,
    &CIM_InstalledProduct_ProductName_Propagated_qual,
};

/* property CIM_InstalledProduct.ProductName */
static MI_CONST MI_PropertyDecl CIM_InstalledProduct_ProductName_prop =
{
    MI_FLAG_PROPERTY|MI_FLAG_KEY, /* flags */
    0x0070650B, /* code */
    MI_T("ProductName"), /* name */
    CIM_InstalledProduct_ProductName_quals, /* qualifiers */
    MI_COUNT(CIM_InstalledProduct_ProductName_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_InstalledProduct, ProductName), /* offset */
    MI_T("CIM_InstalledProduct"), /* origin */
    MI_T("CIM_InstalledProduct"), /* propagator */
    NULL,
};

static MI_CONST MI_Uint32 CIM_InstalledProduct_ProductVendor_MaxLen_qual_value = 256U;

static MI_CONST MI_Qualifier CIM_InstalledProduct_ProductVendor_MaxLen_qual =
{
    MI_T("MaxLen"),
    MI_UINT32,
    0,
    &CIM_InstalledProduct_ProductVendor_MaxLen_qual_value
};

static MI_CONST MI_Char* CIM_InstalledProduct_ProductVendor_Propagated_qual_value = MI_T("CIM_Product.Vendor");

static MI_CONST MI_Qualifier CIM_InstalledProduct_ProductVendor_Propagated_qual =
{
    MI_T("Propagated"),
    MI_STRING,
    MI_FLAG_DISABLEOVERRIDE|MI_FLAG_TOSUBCLASS,
    &CIM_InstalledProduct_ProductVendor_Propagated_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_InstalledProduct_ProductVendor_quals[] =
{
    &CIM_InstalledProduct_ProductVendor_MaxLen_qual,
    &CIM_InstalledProduct_ProductVendor_Propagated_qual,
};

/* property CIM_InstalledProduct.ProductVendor */
static MI_CONST MI_PropertyDecl CIM_InstalledProduct_ProductVendor_prop =
{
    MI_FLAG_PROPERTY|MI_FLAG_KEY, /* flags */
    0x0070720D, /* code */
    MI_T("ProductVendor"), /* name */
    CIM_InstalledProduct_ProductVendor_quals, /* qualifiers */
    MI_COUNT(CIM_InstalledProduct_ProductVendor_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_InstalledProduct, ProductVendor), /* offset */
    MI_T("CIM_InstalledProduct"), /* origin */
    MI_T("CIM_InstalledProduct"), /* propagator */
    NULL,
};

static MI_CONST MI_Uint32 CIM_InstalledProduct_ProductVersion_MaxLen_qual_value = 64U;

static MI_CONST MI_Qualifier CIM_InstalledProduct_ProductVersion_MaxLen_qual =
{
    MI_T("MaxLen"),
    MI_UINT32,
    0,
    &CIM_InstalledProduct_ProductVersion_MaxLen_qual_value
};

static MI_CONST MI_Char* CIM_InstalledProduct_ProductVersion_Propagated_qual_value = MI_T("CIM_Product.Version");

static MI_CONST MI_Qualifier CIM_InstalledProduct_ProductVersion_Propagated_qual =
{
    MI_T("Propagated"),
    MI_STRING,
    MI_FLAG_DISABLEOVERRIDE|MI_FLAG_TOSUBCLASS,
    &CIM_InstalledProduct_ProductVersion_Propagated_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_InstalledProduct_ProductVersion_quals[] =
{
    &CIM_InstalledProduct_ProductVersion_MaxLen_qual,
    &CIM_InstalledProduct_ProductVersion_Propagated_qual,
};

/* property CIM_InstalledProduct.ProductVersion */
static MI_CONST MI_PropertyDecl CIM_InstalledProduct_ProductVersion_prop =
{
    MI_FLAG_PROPERTY|MI_FLAG_KEY, /* flags */
    0x00706E0E, /* code */
    MI_T("ProductVersion"), /* name */
    CIM_InstalledProduct_ProductVersion_quals, /* qualifiers */
    MI_COUNT(CIM_InstalledProduct_ProductVersion_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_InstalledProduct, ProductVersion), /* offset */
    MI_T("CIM_InstalledProduct"), /* origin */
    MI_T("CIM_InstalledProduct"), /* propagator */
    NULL,
};

static MI_CONST MI_Uint32 CIM_InstalledProduct_SystemID_MaxLen_qual_value = 256U;

static MI_CONST MI_Qualifier CIM_InstalledProduct_SystemID_MaxLen_qual =
{
    MI_T("MaxLen"),
    MI_UINT32,
    0,
    &CIM_InstalledProduct_SystemID_MaxLen_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_InstalledProduct_SystemID_quals[] =
{
    &CIM_InstalledProduct_SystemID_MaxLen_qual,
};

/* property CIM_InstalledProduct.SystemID */
static MI_CONST MI_PropertyDecl CIM_InstalledProduct_SystemID_prop =
{
    MI_FLAG_PROPERTY|MI_FLAG_KEY, /* flags */
    0x00736408, /* code */
    MI_T("SystemID"), /* name */
    CIM_InstalledProduct_SystemID_quals, /* qualifiers */
    MI_COUNT(CIM_InstalledProduct_SystemID_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_InstalledProduct, SystemID), /* offset */
    MI_T("CIM_InstalledProduct"), /* origin */
    MI_T("CIM_InstalledProduct"), /* propagator */
    NULL,
};

static MI_CONST MI_Uint32 CIM_InstalledProduct_CollectionID_MaxLen_qual_value = 256U;

static MI_CONST MI_Qualifier CIM_InstalledProduct_CollectionID_MaxLen_qual =
{
    MI_T("MaxLen"),
    MI_UINT32,
    0,
    &CIM_InstalledProduct_CollectionID_MaxLen_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_InstalledProduct_CollectionID_quals[] =
{
    &CIM_InstalledProduct_CollectionID_MaxLen_qual,
};

/* property CIM_InstalledProduct.CollectionID */
static MI_CONST MI_PropertyDecl CIM_InstalledProduct_CollectionID_prop =
{
    MI_FLAG_PROPERTY|MI_FLAG_KEY, /* flags */
    0x0063640C, /* code */
    MI_T("CollectionID"), /* name */
    CIM_InstalledProduct_CollectionID_quals, /* qualifiers */
    MI_COUNT(CIM_InstalledProduct_CollectionID_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_InstalledProduct, CollectionID), /* offset */
    MI_T("CIM_InstalledProduct"), /* origin */
    MI_T("CIM_InstalledProduct"), /* propagator */
    NULL,
};

static MI_CONST MI_Uint32 CIM_InstalledProduct_Name_MaxLen_qual_value = 256U;

static MI_CONST MI_Qualifier CIM_InstalledProduct_Name_MaxLen_qual =
{
    MI_T("MaxLen"),
    MI_UINT32,
    0,
    &CIM_InstalledProduct_Name_MaxLen_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_InstalledProduct_Name_quals[] =
{
    &CIM_InstalledProduct_Name_MaxLen_qual,
};

/* property CIM_InstalledProduct.Name */
static MI_CONST MI_PropertyDecl CIM_InstalledProduct_Name_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x006E6504, /* code */
    MI_T("Name"), /* name */
    CIM_InstalledProduct_Name_quals, /* qualifiers */
    MI_COUNT(CIM_InstalledProduct_Name_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_InstalledProduct, Name), /* offset */
    MI_T("CIM_InstalledProduct"), /* origin */
    MI_T("CIM_InstalledProduct"), /* propagator */
    NULL,
};

static MI_PropertyDecl MI_CONST* MI_CONST CIM_InstalledProduct_props[] =
{
    &CIM_ManagedElement_InstanceID_prop,
    &CIM_ManagedElement_Caption_prop,
    &CIM_ManagedElement_Description_prop,
    &CIM_ManagedElement_ElementName_prop,
    &CIM_InstalledProduct_ProductIdentifyingNumber_prop,
    &CIM_InstalledProduct_ProductName_prop,
    &CIM_InstalledProduct_ProductVendor_prop,
    &CIM_InstalledProduct_ProductVersion_prop,
    &CIM_InstalledProduct_SystemID_prop,
    &CIM_InstalledProduct_CollectionID_prop,
    &CIM_InstalledProduct_Name_prop,
};

static MI_CONST MI_Char* CIM_InstalledProduct_UMLPackagePath_qual_value = MI_T("CIM::Application::InstalledProduct");

static MI_CONST MI_Qualifier CIM_InstalledProduct_UMLPackagePath_qual =
{
    MI_T("UMLPackagePath"),
    MI_STRING,
    0,
    &CIM_InstalledProduct_UMLPackagePath_qual_value
};

static MI_CONST MI_Char* CIM_InstalledProduct_Version_qual_value = MI_T("2.6.0");

static MI_CONST MI_Qualifier CIM_InstalledProduct_Version_qual =
{
    MI_T("Version"),
    MI_STRING,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_TRANSLATABLE|MI_FLAG_RESTRICTED,
    &CIM_InstalledProduct_Version_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_InstalledProduct_quals[] =
{
    &CIM_InstalledProduct_UMLPackagePath_qual,
    &CIM_InstalledProduct_Version_qual,
};

/* class CIM_InstalledProduct */
MI_CONST MI_ClassDecl CIM_InstalledProduct_rtti =
{
    MI_FLAG_CLASS, /* flags */
    0x00637414, /* code */
    MI_T("CIM_InstalledProduct"), /* name */
    CIM_InstalledProduct_quals, /* qualifiers */
    MI_COUNT(CIM_InstalledProduct_quals), /* numQualifiers */
    CIM_InstalledProduct_props, /* properties */
    MI_COUNT(CIM_InstalledProduct_props), /* numProperties */
    sizeof(CIM_InstalledProduct), /* size */
    MI_T("CIM_Collection"), /* superClass */
    &CIM_Collection_rtti, /* superClassDecl */
    NULL, /* methods */
    0, /* numMethods */
    &schemaDecl, /* schema */
    NULL, /* functions */
    NULL, /* owningClass */
};

/*
**==============================================================================
**
** Apache_HTTPDServer
**
**==============================================================================
*/

/* property Apache_HTTPDServer.ModuleVersion */
static MI_CONST MI_PropertyDecl Apache_HTTPDServer_ModuleVersion_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x006D6E0D, /* code */
    MI_T("ModuleVersion"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDServer, ModuleVersion), /* offset */
    MI_T("Apache_HTTPDServer"), /* origin */
    MI_T("Apache_HTTPDServer"), /* propagator */
    NULL,
};

/* property Apache_HTTPDServer.ConfigurationFile */
static MI_CONST MI_PropertyDecl Apache_HTTPDServer_ConfigurationFile_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00636511, /* code */
    MI_T("ConfigurationFile"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDServer, ConfigurationFile), /* offset */
    MI_T("Apache_HTTPDServer"), /* origin */
    MI_T("Apache_HTTPDServer"), /* propagator */
    NULL,
};

/* property Apache_HTTPDServer.InstalledModules */
static MI_CONST MI_PropertyDecl Apache_HTTPDServer_InstalledModules_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00697310, /* code */
    MI_T("InstalledModules"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_STRINGA, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDServer, InstalledModules), /* offset */
    MI_T("Apache_HTTPDServer"), /* origin */
    MI_T("Apache_HTTPDServer"), /* propagator */
    NULL,
};

/* property Apache_HTTPDServer.ProcessName */
static MI_CONST MI_PropertyDecl Apache_HTTPDServer_ProcessName_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0070650B, /* code */
    MI_T("ProcessName"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDServer, ProcessName), /* offset */
    MI_T("Apache_HTTPDServer"), /* origin */
    MI_T("Apache_HTTPDServer"), /* propagator */
    NULL,
};

/* property Apache_HTTPDServer.ServiceName */
static MI_CONST MI_PropertyDecl Apache_HTTPDServer_ServiceName_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0073650B, /* code */
    MI_T("ServiceName"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDServer, ServiceName), /* offset */
    MI_T("Apache_HTTPDServer"), /* origin */
    MI_T("Apache_HTTPDServer"), /* propagator */
    NULL,
};

/* property Apache_HTTPDServer.OperatingStatus */
static MI_CONST MI_PropertyDecl Apache_HTTPDServer_OperatingStatus_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x006F730F, /* code */
    MI_T("OperatingStatus"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDServer, OperatingStatus), /* offset */
    MI_T("Apache_HTTPDServer"), /* origin */
    MI_T("Apache_HTTPDServer"), /* propagator */
    NULL,
};

static MI_PropertyDecl MI_CONST* MI_CONST Apache_HTTPDServer_props[] =
{
    &CIM_ManagedElement_InstanceID_prop,
    &CIM_ManagedElement_Caption_prop,
    &CIM_ManagedElement_Description_prop,
    &CIM_ManagedElement_ElementName_prop,
    &CIM_InstalledProduct_ProductIdentifyingNumber_prop,
    &CIM_InstalledProduct_ProductName_prop,
    &CIM_InstalledProduct_ProductVendor_prop,
    &CIM_InstalledProduct_ProductVersion_prop,
    &CIM_InstalledProduct_SystemID_prop,
    &CIM_InstalledProduct_CollectionID_prop,
    &CIM_InstalledProduct_Name_prop,
    &Apache_HTTPDServer_ModuleVersion_prop,
    &Apache_HTTPDServer_ConfigurationFile_prop,
    &Apache_HTTPDServer_InstalledModules_prop,
    &Apache_HTTPDServer_ProcessName_prop,
    &Apache_HTTPDServer_ServiceName_prop,
    &Apache_HTTPDServer_OperatingStatus_prop,
};

static MI_CONST MI_ProviderFT Apache_HTTPDServer_funcs =
{
  (MI_ProviderFT_Load)Apache_HTTPDServer_Load,
  (MI_ProviderFT_Unload)Apache_HTTPDServer_Unload,
  (MI_ProviderFT_GetInstance)Apache_HTTPDServer_GetInstance,
  (MI_ProviderFT_EnumerateInstances)Apache_HTTPDServer_EnumerateInstances,
  (MI_ProviderFT_CreateInstance)Apache_HTTPDServer_CreateInstance,
  (MI_ProviderFT_ModifyInstance)Apache_HTTPDServer_ModifyInstance,
  (MI_ProviderFT_DeleteInstance)Apache_HTTPDServer_DeleteInstance,
  (MI_ProviderFT_AssociatorInstances)NULL,
  (MI_ProviderFT_ReferenceInstances)NULL,
  (MI_ProviderFT_EnableIndications)NULL,
  (MI_ProviderFT_DisableIndications)NULL,
  (MI_ProviderFT_Subscribe)NULL,
  (MI_ProviderFT_Unsubscribe)NULL,
  (MI_ProviderFT_Invoke)NULL,
};

static MI_CONST MI_Char* Apache_HTTPDServer_UMLPackagePath_qual_value = MI_T("CIM::Application::InstalledProduct");

static MI_CONST MI_Qualifier Apache_HTTPDServer_UMLPackagePath_qual =
{
    MI_T("UMLPackagePath"),
    MI_STRING,
    0,
    &Apache_HTTPDServer_UMLPackagePath_qual_value
};

static MI_CONST MI_Char* Apache_HTTPDServer_Version_qual_value = MI_T("1.0.0");

static MI_CONST MI_Qualifier Apache_HTTPDServer_Version_qual =
{
    MI_T("Version"),
    MI_STRING,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_TRANSLATABLE|MI_FLAG_RESTRICTED,
    &Apache_HTTPDServer_Version_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST Apache_HTTPDServer_quals[] =
{
    &Apache_HTTPDServer_UMLPackagePath_qual,
    &Apache_HTTPDServer_Version_qual,
};

/* class Apache_HTTPDServer */
MI_CONST MI_ClassDecl Apache_HTTPDServer_rtti =
{
    MI_FLAG_CLASS, /* flags */
    0x00617212, /* code */
    MI_T("Apache_HTTPDServer"), /* name */
    Apache_HTTPDServer_quals, /* qualifiers */
    MI_COUNT(Apache_HTTPDServer_quals), /* numQualifiers */
    Apache_HTTPDServer_props, /* properties */
    MI_COUNT(Apache_HTTPDServer_props), /* numProperties */
    sizeof(Apache_HTTPDServer), /* size */
    MI_T("CIM_InstalledProduct"), /* superClass */
    &CIM_InstalledProduct_rtti, /* superClassDecl */
    NULL, /* methods */
    0, /* numMethods */
    &schemaDecl, /* schema */
    &Apache_HTTPDServer_funcs, /* functions */
    NULL, /* owningClass */
};

/*
**==============================================================================
**
** CIM_StatisticalData
**
**==============================================================================
*/

static MI_CONST MI_Char* CIM_StatisticalData_InstanceID_Override_qual_value = MI_T("InstanceID");

static MI_CONST MI_Qualifier CIM_StatisticalData_InstanceID_Override_qual =
{
    MI_T("Override"),
    MI_STRING,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_RESTRICTED,
    &CIM_StatisticalData_InstanceID_Override_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_StatisticalData_InstanceID_quals[] =
{
    &CIM_StatisticalData_InstanceID_Override_qual,
};

/* property CIM_StatisticalData.InstanceID */
static MI_CONST MI_PropertyDecl CIM_StatisticalData_InstanceID_prop =
{
    MI_FLAG_PROPERTY|MI_FLAG_KEY, /* flags */
    0x0069640A, /* code */
    MI_T("InstanceID"), /* name */
    CIM_StatisticalData_InstanceID_quals, /* qualifiers */
    MI_COUNT(CIM_StatisticalData_InstanceID_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_StatisticalData, InstanceID), /* offset */
    MI_T("CIM_ManagedElement"), /* origin */
    MI_T("CIM_StatisticalData"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_StatisticalData_ElementName_Override_qual_value = MI_T("ElementName");

static MI_CONST MI_Qualifier CIM_StatisticalData_ElementName_Override_qual =
{
    MI_T("Override"),
    MI_STRING,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_RESTRICTED,
    &CIM_StatisticalData_ElementName_Override_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_StatisticalData_ElementName_quals[] =
{
    &CIM_StatisticalData_ElementName_Override_qual,
};

/* property CIM_StatisticalData.ElementName */
static MI_CONST MI_PropertyDecl CIM_StatisticalData_ElementName_prop =
{
    MI_FLAG_PROPERTY|MI_FLAG_REQUIRED, /* flags */
    0x0065650B, /* code */
    MI_T("ElementName"), /* name */
    CIM_StatisticalData_ElementName_quals, /* qualifiers */
    MI_COUNT(CIM_StatisticalData_ElementName_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_StatisticalData, ElementName), /* offset */
    MI_T("CIM_ManagedElement"), /* origin */
    MI_T("CIM_StatisticalData"), /* propagator */
    NULL,
};

/* property CIM_StatisticalData.StartStatisticTime */
static MI_CONST MI_PropertyDecl CIM_StatisticalData_StartStatisticTime_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00736512, /* code */
    MI_T("StartStatisticTime"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_DATETIME, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_StatisticalData, StartStatisticTime), /* offset */
    MI_T("CIM_StatisticalData"), /* origin */
    MI_T("CIM_StatisticalData"), /* propagator */
    NULL,
};

/* property CIM_StatisticalData.StatisticTime */
static MI_CONST MI_PropertyDecl CIM_StatisticalData_StatisticTime_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0073650D, /* code */
    MI_T("StatisticTime"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_DATETIME, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_StatisticalData, StatisticTime), /* offset */
    MI_T("CIM_StatisticalData"), /* origin */
    MI_T("CIM_StatisticalData"), /* propagator */
    NULL,
};

static MI_CONST MI_Datetime CIM_StatisticalData_SampleInterval_value = {0,{{0,0,0,0,0}}};

/* property CIM_StatisticalData.SampleInterval */
static MI_CONST MI_PropertyDecl CIM_StatisticalData_SampleInterval_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00736C0E, /* code */
    MI_T("SampleInterval"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_DATETIME, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_StatisticalData, SampleInterval), /* offset */
    MI_T("CIM_StatisticalData"), /* origin */
    MI_T("CIM_StatisticalData"), /* propagator */
    &CIM_StatisticalData_SampleInterval_value,
};

static MI_PropertyDecl MI_CONST* MI_CONST CIM_StatisticalData_props[] =
{
    &CIM_StatisticalData_InstanceID_prop,
    &CIM_ManagedElement_Caption_prop,
    &CIM_ManagedElement_Description_prop,
    &CIM_StatisticalData_ElementName_prop,
    &CIM_StatisticalData_StartStatisticTime_prop,
    &CIM_StatisticalData_StatisticTime_prop,
    &CIM_StatisticalData_SampleInterval_prop,
};

/* parameter CIM_StatisticalData.ResetSelectedStats(): SelectedStatistics */
static MI_CONST MI_ParameterDecl CIM_StatisticalData_ResetSelectedStats_SelectedStatistics_param =
{
    MI_FLAG_PARAMETER|MI_FLAG_IN, /* flags */
    0x00737312, /* code */
    MI_T("SelectedStatistics"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_STRINGA, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_StatisticalData_ResetSelectedStats, SelectedStatistics), /* offset */
};

/* parameter CIM_StatisticalData.ResetSelectedStats(): MIReturn */
static MI_CONST MI_ParameterDecl CIM_StatisticalData_ResetSelectedStats_MIReturn_param =
{
    MI_FLAG_PARAMETER|MI_FLAG_OUT, /* flags */
    0x006D6E08, /* code */
    MI_T("MIReturn"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT32, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_StatisticalData_ResetSelectedStats, MIReturn), /* offset */
};

static MI_ParameterDecl MI_CONST* MI_CONST CIM_StatisticalData_ResetSelectedStats_params[] =
{
    &CIM_StatisticalData_ResetSelectedStats_MIReturn_param,
    &CIM_StatisticalData_ResetSelectedStats_SelectedStatistics_param,
};

/* method CIM_StatisticalData.ResetSelectedStats() */
MI_CONST MI_MethodDecl CIM_StatisticalData_ResetSelectedStats_rtti =
{
    MI_FLAG_METHOD, /* flags */
    0x00727312, /* code */
    MI_T("ResetSelectedStats"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    CIM_StatisticalData_ResetSelectedStats_params, /* parameters */
    MI_COUNT(CIM_StatisticalData_ResetSelectedStats_params), /* numParameters */
    sizeof(CIM_StatisticalData_ResetSelectedStats), /* size */
    MI_UINT32, /* returnType */
    MI_T("CIM_StatisticalData"), /* origin */
    MI_T("CIM_StatisticalData"), /* propagator */
    &schemaDecl, /* schema */
    NULL, /* method */
};

static MI_MethodDecl MI_CONST* MI_CONST CIM_StatisticalData_meths[] =
{
    &CIM_StatisticalData_ResetSelectedStats_rtti,
};

static MI_CONST MI_Char* CIM_StatisticalData_UMLPackagePath_qual_value = MI_T("CIM::Core::Statistics");

static MI_CONST MI_Qualifier CIM_StatisticalData_UMLPackagePath_qual =
{
    MI_T("UMLPackagePath"),
    MI_STRING,
    0,
    &CIM_StatisticalData_UMLPackagePath_qual_value
};

static MI_CONST MI_Char* CIM_StatisticalData_Version_qual_value = MI_T("2.19.0");

static MI_CONST MI_Qualifier CIM_StatisticalData_Version_qual =
{
    MI_T("Version"),
    MI_STRING,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_TRANSLATABLE|MI_FLAG_RESTRICTED,
    &CIM_StatisticalData_Version_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_StatisticalData_quals[] =
{
    &CIM_StatisticalData_UMLPackagePath_qual,
    &CIM_StatisticalData_Version_qual,
};

/* class CIM_StatisticalData */
MI_CONST MI_ClassDecl CIM_StatisticalData_rtti =
{
    MI_FLAG_CLASS|MI_FLAG_ABSTRACT, /* flags */
    0x00636113, /* code */
    MI_T("CIM_StatisticalData"), /* name */
    CIM_StatisticalData_quals, /* qualifiers */
    MI_COUNT(CIM_StatisticalData_quals), /* numQualifiers */
    CIM_StatisticalData_props, /* properties */
    MI_COUNT(CIM_StatisticalData_props), /* numProperties */
    sizeof(CIM_StatisticalData), /* size */
    MI_T("CIM_ManagedElement"), /* superClass */
    &CIM_ManagedElement_rtti, /* superClassDecl */
    CIM_StatisticalData_meths, /* methods */
    MI_COUNT(CIM_StatisticalData_meths), /* numMethods */
    &schemaDecl, /* schema */
    NULL, /* functions */
    NULL, /* owningClass */
};

/*
**==============================================================================
**
** Apache_HTTPDServerStatistics
**
**==============================================================================
*/

/* property Apache_HTTPDServerStatistics.TotalPctCPU */
static MI_CONST MI_PropertyDecl Apache_HTTPDServerStatistics_TotalPctCPU_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0074750B, /* code */
    MI_T("TotalPctCPU"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT32, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDServerStatistics, TotalPctCPU), /* offset */
    MI_T("Apache_HTTPDServerStatistics"), /* origin */
    MI_T("Apache_HTTPDServerStatistics"), /* propagator */
    NULL,
};

/* property Apache_HTTPDServerStatistics.IdleWorkers */
static MI_CONST MI_PropertyDecl Apache_HTTPDServerStatistics_IdleWorkers_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0069730B, /* code */
    MI_T("IdleWorkers"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT32, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDServerStatistics, IdleWorkers), /* offset */
    MI_T("Apache_HTTPDServerStatistics"), /* origin */
    MI_T("Apache_HTTPDServerStatistics"), /* propagator */
    NULL,
};

/* property Apache_HTTPDServerStatistics.BusyWorkers */
static MI_CONST MI_PropertyDecl Apache_HTTPDServerStatistics_BusyWorkers_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0062730B, /* code */
    MI_T("BusyWorkers"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT32, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDServerStatistics, BusyWorkers), /* offset */
    MI_T("Apache_HTTPDServerStatistics"), /* origin */
    MI_T("Apache_HTTPDServerStatistics"), /* propagator */
    NULL,
};

/* property Apache_HTTPDServerStatistics.PctBusyWorkers */
static MI_CONST MI_PropertyDecl Apache_HTTPDServerStatistics_PctBusyWorkers_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0070730E, /* code */
    MI_T("PctBusyWorkers"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT32, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDServerStatistics, PctBusyWorkers), /* offset */
    MI_T("Apache_HTTPDServerStatistics"), /* origin */
    MI_T("Apache_HTTPDServerStatistics"), /* propagator */
    NULL,
};

/* property Apache_HTTPDServerStatistics.ConfigurationFile */
static MI_CONST MI_PropertyDecl Apache_HTTPDServerStatistics_ConfigurationFile_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00636511, /* code */
    MI_T("ConfigurationFile"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDServerStatistics, ConfigurationFile), /* offset */
    MI_T("Apache_HTTPDServerStatistics"), /* origin */
    MI_T("Apache_HTTPDServerStatistics"), /* propagator */
    NULL,
};

static MI_PropertyDecl MI_CONST* MI_CONST Apache_HTTPDServerStatistics_props[] =
{
    &CIM_StatisticalData_InstanceID_prop,
    &CIM_ManagedElement_Caption_prop,
    &CIM_ManagedElement_Description_prop,
    &CIM_StatisticalData_ElementName_prop,
    &CIM_StatisticalData_StartStatisticTime_prop,
    &CIM_StatisticalData_StatisticTime_prop,
    &CIM_StatisticalData_SampleInterval_prop,
    &Apache_HTTPDServerStatistics_TotalPctCPU_prop,
    &Apache_HTTPDServerStatistics_IdleWorkers_prop,
    &Apache_HTTPDServerStatistics_BusyWorkers_prop,
    &Apache_HTTPDServerStatistics_PctBusyWorkers_prop,
    &Apache_HTTPDServerStatistics_ConfigurationFile_prop,
};

/* parameter Apache_HTTPDServerStatistics.ResetSelectedStats(): SelectedStatistics */
static MI_CONST MI_ParameterDecl Apache_HTTPDServerStatistics_ResetSelectedStats_SelectedStatistics_param =
{
    MI_FLAG_PARAMETER|MI_FLAG_IN, /* flags */
    0x00737312, /* code */
    MI_T("SelectedStatistics"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_STRINGA, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDServerStatistics_ResetSelectedStats, SelectedStatistics), /* offset */
};

/* parameter Apache_HTTPDServerStatistics.ResetSelectedStats(): MIReturn */
static MI_CONST MI_ParameterDecl Apache_HTTPDServerStatistics_ResetSelectedStats_MIReturn_param =
{
    MI_FLAG_PARAMETER|MI_FLAG_OUT, /* flags */
    0x006D6E08, /* code */
    MI_T("MIReturn"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT32, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDServerStatistics_ResetSelectedStats, MIReturn), /* offset */
};

static MI_ParameterDecl MI_CONST* MI_CONST Apache_HTTPDServerStatistics_ResetSelectedStats_params[] =
{
    &Apache_HTTPDServerStatistics_ResetSelectedStats_MIReturn_param,
    &Apache_HTTPDServerStatistics_ResetSelectedStats_SelectedStatistics_param,
};

/* method Apache_HTTPDServerStatistics.ResetSelectedStats() */
MI_CONST MI_MethodDecl Apache_HTTPDServerStatistics_ResetSelectedStats_rtti =
{
    MI_FLAG_METHOD, /* flags */
    0x00727312, /* code */
    MI_T("ResetSelectedStats"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    Apache_HTTPDServerStatistics_ResetSelectedStats_params, /* parameters */
    MI_COUNT(Apache_HTTPDServerStatistics_ResetSelectedStats_params), /* numParameters */
    sizeof(Apache_HTTPDServerStatistics_ResetSelectedStats), /* size */
    MI_UINT32, /* returnType */
    MI_T("CIM_StatisticalData"), /* origin */
    MI_T("CIM_StatisticalData"), /* propagator */
    &schemaDecl, /* schema */
    (MI_ProviderFT_Invoke)Apache_HTTPDServerStatistics_Invoke_ResetSelectedStats, /* method */
};

static MI_MethodDecl MI_CONST* MI_CONST Apache_HTTPDServerStatistics_meths[] =
{
    &Apache_HTTPDServerStatistics_ResetSelectedStats_rtti,
};

static MI_CONST MI_ProviderFT Apache_HTTPDServerStatistics_funcs =
{
  (MI_ProviderFT_Load)Apache_HTTPDServerStatistics_Load,
  (MI_ProviderFT_Unload)Apache_HTTPDServerStatistics_Unload,
  (MI_ProviderFT_GetInstance)Apache_HTTPDServerStatistics_GetInstance,
  (MI_ProviderFT_EnumerateInstances)Apache_HTTPDServerStatistics_EnumerateInstances,
  (MI_ProviderFT_CreateInstance)Apache_HTTPDServerStatistics_CreateInstance,
  (MI_ProviderFT_ModifyInstance)Apache_HTTPDServerStatistics_ModifyInstance,
  (MI_ProviderFT_DeleteInstance)Apache_HTTPDServerStatistics_DeleteInstance,
  (MI_ProviderFT_AssociatorInstances)NULL,
  (MI_ProviderFT_ReferenceInstances)NULL,
  (MI_ProviderFT_EnableIndications)NULL,
  (MI_ProviderFT_DisableIndications)NULL,
  (MI_ProviderFT_Subscribe)NULL,
  (MI_ProviderFT_Unsubscribe)NULL,
  (MI_ProviderFT_Invoke)NULL,
};

static MI_CONST MI_Char* Apache_HTTPDServerStatistics_UMLPackagePath_qual_value = MI_T("CIM::Core::Statistics");

static MI_CONST MI_Qualifier Apache_HTTPDServerStatistics_UMLPackagePath_qual =
{
    MI_T("UMLPackagePath"),
    MI_STRING,
    0,
    &Apache_HTTPDServerStatistics_UMLPackagePath_qual_value
};

static MI_CONST MI_Char* Apache_HTTPDServerStatistics_Version_qual_value = MI_T("1.0.0");

static MI_CONST MI_Qualifier Apache_HTTPDServerStatistics_Version_qual =
{
    MI_T("Version"),
    MI_STRING,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_TRANSLATABLE|MI_FLAG_RESTRICTED,
    &Apache_HTTPDServerStatistics_Version_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST Apache_HTTPDServerStatistics_quals[] =
{
    &Apache_HTTPDServerStatistics_UMLPackagePath_qual,
    &Apache_HTTPDServerStatistics_Version_qual,
};

/* class Apache_HTTPDServerStatistics */
MI_CONST MI_ClassDecl Apache_HTTPDServerStatistics_rtti =
{
    MI_FLAG_CLASS, /* flags */
    0x0061731C, /* code */
    MI_T("Apache_HTTPDServerStatistics"), /* name */
    Apache_HTTPDServerStatistics_quals, /* qualifiers */
    MI_COUNT(Apache_HTTPDServerStatistics_quals), /* numQualifiers */
    Apache_HTTPDServerStatistics_props, /* properties */
    MI_COUNT(Apache_HTTPDServerStatistics_props), /* numProperties */
    sizeof(Apache_HTTPDServerStatistics), /* size */
    MI_T("CIM_StatisticalData"), /* superClass */
    &CIM_StatisticalData_rtti, /* superClassDecl */
    Apache_HTTPDServerStatistics_meths, /* methods */
    MI_COUNT(Apache_HTTPDServerStatistics_meths), /* numMethods */
    &schemaDecl, /* schema */
    &Apache_HTTPDServerStatistics_funcs, /* functions */
    NULL, /* owningClass */
};

/*
**==============================================================================
**
** CIM_ManagedSystemElement
**
**==============================================================================
*/

/* property CIM_ManagedSystemElement.InstallDate */
static MI_CONST MI_PropertyDecl CIM_ManagedSystemElement_InstallDate_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0069650B, /* code */
    MI_T("InstallDate"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_DATETIME, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_ManagedSystemElement, InstallDate), /* offset */
    MI_T("CIM_ManagedSystemElement"), /* origin */
    MI_T("CIM_ManagedSystemElement"), /* propagator */
    NULL,
};

static MI_CONST MI_Uint32 CIM_ManagedSystemElement_Name_MaxLen_qual_value = 1024U;

static MI_CONST MI_Qualifier CIM_ManagedSystemElement_Name_MaxLen_qual =
{
    MI_T("MaxLen"),
    MI_UINT32,
    0,
    &CIM_ManagedSystemElement_Name_MaxLen_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_ManagedSystemElement_Name_quals[] =
{
    &CIM_ManagedSystemElement_Name_MaxLen_qual,
};

/* property CIM_ManagedSystemElement.Name */
static MI_CONST MI_PropertyDecl CIM_ManagedSystemElement_Name_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x006E6504, /* code */
    MI_T("Name"), /* name */
    CIM_ManagedSystemElement_Name_quals, /* qualifiers */
    MI_COUNT(CIM_ManagedSystemElement_Name_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_ManagedSystemElement, Name), /* offset */
    MI_T("CIM_ManagedSystemElement"), /* origin */
    MI_T("CIM_ManagedSystemElement"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_ManagedSystemElement_OperationalStatus_ArrayType_qual_value = MI_T("Indexed");

static MI_CONST MI_Qualifier CIM_ManagedSystemElement_OperationalStatus_ArrayType_qual =
{
    MI_T("ArrayType"),
    MI_STRING,
    MI_FLAG_DISABLEOVERRIDE|MI_FLAG_TOSUBCLASS,
    &CIM_ManagedSystemElement_OperationalStatus_ArrayType_qual_value
};

static MI_CONST MI_Char* CIM_ManagedSystemElement_OperationalStatus_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_ManagedSystemElement.StatusDescriptions"),
};

static MI_CONST MI_ConstStringA CIM_ManagedSystemElement_OperationalStatus_ModelCorrespondence_qual_value =
{
    CIM_ManagedSystemElement_OperationalStatus_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_ManagedSystemElement_OperationalStatus_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_ManagedSystemElement_OperationalStatus_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_ManagedSystemElement_OperationalStatus_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_ManagedSystemElement_OperationalStatus_quals[] =
{
    &CIM_ManagedSystemElement_OperationalStatus_ArrayType_qual,
    &CIM_ManagedSystemElement_OperationalStatus_ModelCorrespondence_qual,
};

/* property CIM_ManagedSystemElement.OperationalStatus */
static MI_CONST MI_PropertyDecl CIM_ManagedSystemElement_OperationalStatus_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x006F7311, /* code */
    MI_T("OperationalStatus"), /* name */
    CIM_ManagedSystemElement_OperationalStatus_quals, /* qualifiers */
    MI_COUNT(CIM_ManagedSystemElement_OperationalStatus_quals), /* numQualifiers */
    MI_UINT16A, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_ManagedSystemElement, OperationalStatus), /* offset */
    MI_T("CIM_ManagedSystemElement"), /* origin */
    MI_T("CIM_ManagedSystemElement"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_ManagedSystemElement_StatusDescriptions_ArrayType_qual_value = MI_T("Indexed");

static MI_CONST MI_Qualifier CIM_ManagedSystemElement_StatusDescriptions_ArrayType_qual =
{
    MI_T("ArrayType"),
    MI_STRING,
    MI_FLAG_DISABLEOVERRIDE|MI_FLAG_TOSUBCLASS,
    &CIM_ManagedSystemElement_StatusDescriptions_ArrayType_qual_value
};

static MI_CONST MI_Char* CIM_ManagedSystemElement_StatusDescriptions_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_ManagedSystemElement.OperationalStatus"),
};

static MI_CONST MI_ConstStringA CIM_ManagedSystemElement_StatusDescriptions_ModelCorrespondence_qual_value =
{
    CIM_ManagedSystemElement_StatusDescriptions_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_ManagedSystemElement_StatusDescriptions_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_ManagedSystemElement_StatusDescriptions_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_ManagedSystemElement_StatusDescriptions_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_ManagedSystemElement_StatusDescriptions_quals[] =
{
    &CIM_ManagedSystemElement_StatusDescriptions_ArrayType_qual,
    &CIM_ManagedSystemElement_StatusDescriptions_ModelCorrespondence_qual,
};

/* property CIM_ManagedSystemElement.StatusDescriptions */
static MI_CONST MI_PropertyDecl CIM_ManagedSystemElement_StatusDescriptions_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00737312, /* code */
    MI_T("StatusDescriptions"), /* name */
    CIM_ManagedSystemElement_StatusDescriptions_quals, /* qualifiers */
    MI_COUNT(CIM_ManagedSystemElement_StatusDescriptions_quals), /* numQualifiers */
    MI_STRINGA, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_ManagedSystemElement, StatusDescriptions), /* offset */
    MI_T("CIM_ManagedSystemElement"), /* origin */
    MI_T("CIM_ManagedSystemElement"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_ManagedSystemElement_Status_Deprecated_qual_data_value[] =
{
    MI_T("CIM_ManagedSystemElement.OperationalStatus"),
};

static MI_CONST MI_ConstStringA CIM_ManagedSystemElement_Status_Deprecated_qual_value =
{
    CIM_ManagedSystemElement_Status_Deprecated_qual_data_value,
    MI_COUNT(CIM_ManagedSystemElement_Status_Deprecated_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_ManagedSystemElement_Status_Deprecated_qual =
{
    MI_T("Deprecated"),
    MI_STRINGA,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_RESTRICTED,
    &CIM_ManagedSystemElement_Status_Deprecated_qual_value
};

static MI_CONST MI_Uint32 CIM_ManagedSystemElement_Status_MaxLen_qual_value = 10U;

static MI_CONST MI_Qualifier CIM_ManagedSystemElement_Status_MaxLen_qual =
{
    MI_T("MaxLen"),
    MI_UINT32,
    0,
    &CIM_ManagedSystemElement_Status_MaxLen_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_ManagedSystemElement_Status_quals[] =
{
    &CIM_ManagedSystemElement_Status_Deprecated_qual,
    &CIM_ManagedSystemElement_Status_MaxLen_qual,
};

/* property CIM_ManagedSystemElement.Status */
static MI_CONST MI_PropertyDecl CIM_ManagedSystemElement_Status_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00737306, /* code */
    MI_T("Status"), /* name */
    CIM_ManagedSystemElement_Status_quals, /* qualifiers */
    MI_COUNT(CIM_ManagedSystemElement_Status_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_ManagedSystemElement, Status), /* offset */
    MI_T("CIM_ManagedSystemElement"), /* origin */
    MI_T("CIM_ManagedSystemElement"), /* propagator */
    NULL,
};

/* property CIM_ManagedSystemElement.HealthState */
static MI_CONST MI_PropertyDecl CIM_ManagedSystemElement_HealthState_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0068650B, /* code */
    MI_T("HealthState"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT16, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_ManagedSystemElement, HealthState), /* offset */
    MI_T("CIM_ManagedSystemElement"), /* origin */
    MI_T("CIM_ManagedSystemElement"), /* propagator */
    NULL,
};

/* property CIM_ManagedSystemElement.CommunicationStatus */
static MI_CONST MI_PropertyDecl CIM_ManagedSystemElement_CommunicationStatus_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00637313, /* code */
    MI_T("CommunicationStatus"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT16, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_ManagedSystemElement, CommunicationStatus), /* offset */
    MI_T("CIM_ManagedSystemElement"), /* origin */
    MI_T("CIM_ManagedSystemElement"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_ManagedSystemElement_DetailedStatus_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_EnabledLogicalElement.PrimaryStatus"),
    MI_T("CIM_ManagedSystemElement.HealthState"),
};

static MI_CONST MI_ConstStringA CIM_ManagedSystemElement_DetailedStatus_ModelCorrespondence_qual_value =
{
    CIM_ManagedSystemElement_DetailedStatus_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_ManagedSystemElement_DetailedStatus_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_ManagedSystemElement_DetailedStatus_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_ManagedSystemElement_DetailedStatus_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_ManagedSystemElement_DetailedStatus_quals[] =
{
    &CIM_ManagedSystemElement_DetailedStatus_ModelCorrespondence_qual,
};

/* property CIM_ManagedSystemElement.DetailedStatus */
static MI_CONST MI_PropertyDecl CIM_ManagedSystemElement_DetailedStatus_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0064730E, /* code */
    MI_T("DetailedStatus"), /* name */
    CIM_ManagedSystemElement_DetailedStatus_quals, /* qualifiers */
    MI_COUNT(CIM_ManagedSystemElement_DetailedStatus_quals), /* numQualifiers */
    MI_UINT16, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_ManagedSystemElement, DetailedStatus), /* offset */
    MI_T("CIM_ManagedSystemElement"), /* origin */
    MI_T("CIM_ManagedSystemElement"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_ManagedSystemElement_OperatingStatus_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_EnabledLogicalElement.EnabledState"),
};

static MI_CONST MI_ConstStringA CIM_ManagedSystemElement_OperatingStatus_ModelCorrespondence_qual_value =
{
    CIM_ManagedSystemElement_OperatingStatus_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_ManagedSystemElement_OperatingStatus_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_ManagedSystemElement_OperatingStatus_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_ManagedSystemElement_OperatingStatus_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_ManagedSystemElement_OperatingStatus_quals[] =
{
    &CIM_ManagedSystemElement_OperatingStatus_ModelCorrespondence_qual,
};

/* property CIM_ManagedSystemElement.OperatingStatus */
static MI_CONST MI_PropertyDecl CIM_ManagedSystemElement_OperatingStatus_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x006F730F, /* code */
    MI_T("OperatingStatus"), /* name */
    CIM_ManagedSystemElement_OperatingStatus_quals, /* qualifiers */
    MI_COUNT(CIM_ManagedSystemElement_OperatingStatus_quals), /* numQualifiers */
    MI_UINT16, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_ManagedSystemElement, OperatingStatus), /* offset */
    MI_T("CIM_ManagedSystemElement"), /* origin */
    MI_T("CIM_ManagedSystemElement"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_ManagedSystemElement_PrimaryStatus_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_ManagedSystemElement.DetailedStatus"),
    MI_T("CIM_ManagedSystemElement.HealthState"),
};

static MI_CONST MI_ConstStringA CIM_ManagedSystemElement_PrimaryStatus_ModelCorrespondence_qual_value =
{
    CIM_ManagedSystemElement_PrimaryStatus_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_ManagedSystemElement_PrimaryStatus_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_ManagedSystemElement_PrimaryStatus_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_ManagedSystemElement_PrimaryStatus_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_ManagedSystemElement_PrimaryStatus_quals[] =
{
    &CIM_ManagedSystemElement_PrimaryStatus_ModelCorrespondence_qual,
};

/* property CIM_ManagedSystemElement.PrimaryStatus */
static MI_CONST MI_PropertyDecl CIM_ManagedSystemElement_PrimaryStatus_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0070730D, /* code */
    MI_T("PrimaryStatus"), /* name */
    CIM_ManagedSystemElement_PrimaryStatus_quals, /* qualifiers */
    MI_COUNT(CIM_ManagedSystemElement_PrimaryStatus_quals), /* numQualifiers */
    MI_UINT16, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_ManagedSystemElement, PrimaryStatus), /* offset */
    MI_T("CIM_ManagedSystemElement"), /* origin */
    MI_T("CIM_ManagedSystemElement"), /* propagator */
    NULL,
};

static MI_PropertyDecl MI_CONST* MI_CONST CIM_ManagedSystemElement_props[] =
{
    &CIM_ManagedElement_InstanceID_prop,
    &CIM_ManagedElement_Caption_prop,
    &CIM_ManagedElement_Description_prop,
    &CIM_ManagedElement_ElementName_prop,
    &CIM_ManagedSystemElement_InstallDate_prop,
    &CIM_ManagedSystemElement_Name_prop,
    &CIM_ManagedSystemElement_OperationalStatus_prop,
    &CIM_ManagedSystemElement_StatusDescriptions_prop,
    &CIM_ManagedSystemElement_Status_prop,
    &CIM_ManagedSystemElement_HealthState_prop,
    &CIM_ManagedSystemElement_CommunicationStatus_prop,
    &CIM_ManagedSystemElement_DetailedStatus_prop,
    &CIM_ManagedSystemElement_OperatingStatus_prop,
    &CIM_ManagedSystemElement_PrimaryStatus_prop,
};

static MI_CONST MI_Char* CIM_ManagedSystemElement_UMLPackagePath_qual_value = MI_T("CIM::Core::CoreElements");

static MI_CONST MI_Qualifier CIM_ManagedSystemElement_UMLPackagePath_qual =
{
    MI_T("UMLPackagePath"),
    MI_STRING,
    0,
    &CIM_ManagedSystemElement_UMLPackagePath_qual_value
};

static MI_CONST MI_Char* CIM_ManagedSystemElement_Version_qual_value = MI_T("2.28.0");

static MI_CONST MI_Qualifier CIM_ManagedSystemElement_Version_qual =
{
    MI_T("Version"),
    MI_STRING,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_TRANSLATABLE|MI_FLAG_RESTRICTED,
    &CIM_ManagedSystemElement_Version_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_ManagedSystemElement_quals[] =
{
    &CIM_ManagedSystemElement_UMLPackagePath_qual,
    &CIM_ManagedSystemElement_Version_qual,
};

/* class CIM_ManagedSystemElement */
MI_CONST MI_ClassDecl CIM_ManagedSystemElement_rtti =
{
    MI_FLAG_CLASS|MI_FLAG_ABSTRACT, /* flags */
    0x00637418, /* code */
    MI_T("CIM_ManagedSystemElement"), /* name */
    CIM_ManagedSystemElement_quals, /* qualifiers */
    MI_COUNT(CIM_ManagedSystemElement_quals), /* numQualifiers */
    CIM_ManagedSystemElement_props, /* properties */
    MI_COUNT(CIM_ManagedSystemElement_props), /* numProperties */
    sizeof(CIM_ManagedSystemElement), /* size */
    MI_T("CIM_ManagedElement"), /* superClass */
    &CIM_ManagedElement_rtti, /* superClassDecl */
    NULL, /* methods */
    0, /* numMethods */
    &schemaDecl, /* schema */
    NULL, /* functions */
    NULL, /* owningClass */
};

/*
**==============================================================================
**
** CIM_LogicalElement
**
**==============================================================================
*/

static MI_PropertyDecl MI_CONST* MI_CONST CIM_LogicalElement_props[] =
{
    &CIM_ManagedElement_InstanceID_prop,
    &CIM_ManagedElement_Caption_prop,
    &CIM_ManagedElement_Description_prop,
    &CIM_ManagedElement_ElementName_prop,
    &CIM_ManagedSystemElement_InstallDate_prop,
    &CIM_ManagedSystemElement_Name_prop,
    &CIM_ManagedSystemElement_OperationalStatus_prop,
    &CIM_ManagedSystemElement_StatusDescriptions_prop,
    &CIM_ManagedSystemElement_Status_prop,
    &CIM_ManagedSystemElement_HealthState_prop,
    &CIM_ManagedSystemElement_CommunicationStatus_prop,
    &CIM_ManagedSystemElement_DetailedStatus_prop,
    &CIM_ManagedSystemElement_OperatingStatus_prop,
    &CIM_ManagedSystemElement_PrimaryStatus_prop,
};

static MI_CONST MI_Char* CIM_LogicalElement_UMLPackagePath_qual_value = MI_T("CIM::Core::CoreElements");

static MI_CONST MI_Qualifier CIM_LogicalElement_UMLPackagePath_qual =
{
    MI_T("UMLPackagePath"),
    MI_STRING,
    0,
    &CIM_LogicalElement_UMLPackagePath_qual_value
};

static MI_CONST MI_Char* CIM_LogicalElement_Version_qual_value = MI_T("2.6.0");

static MI_CONST MI_Qualifier CIM_LogicalElement_Version_qual =
{
    MI_T("Version"),
    MI_STRING,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_TRANSLATABLE|MI_FLAG_RESTRICTED,
    &CIM_LogicalElement_Version_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_LogicalElement_quals[] =
{
    &CIM_LogicalElement_UMLPackagePath_qual,
    &CIM_LogicalElement_Version_qual,
};

/* class CIM_LogicalElement */
MI_CONST MI_ClassDecl CIM_LogicalElement_rtti =
{
    MI_FLAG_CLASS|MI_FLAG_ABSTRACT, /* flags */
    0x00637412, /* code */
    MI_T("CIM_LogicalElement"), /* name */
    CIM_LogicalElement_quals, /* qualifiers */
    MI_COUNT(CIM_LogicalElement_quals), /* numQualifiers */
    CIM_LogicalElement_props, /* properties */
    MI_COUNT(CIM_LogicalElement_props), /* numProperties */
    sizeof(CIM_LogicalElement), /* size */
    MI_T("CIM_ManagedSystemElement"), /* superClass */
    &CIM_ManagedSystemElement_rtti, /* superClassDecl */
    NULL, /* methods */
    0, /* numMethods */
    &schemaDecl, /* schema */
    NULL, /* functions */
    NULL, /* owningClass */
};

/*
**==============================================================================
**
** CIM_SoftwareElement
**
**==============================================================================
*/

static MI_CONST MI_Uint32 CIM_SoftwareElement_Name_MaxLen_qual_value = 256U;

static MI_CONST MI_Qualifier CIM_SoftwareElement_Name_MaxLen_qual =
{
    MI_T("MaxLen"),
    MI_UINT32,
    0,
    &CIM_SoftwareElement_Name_MaxLen_qual_value
};

static MI_CONST MI_Char* CIM_SoftwareElement_Name_Override_qual_value = MI_T("Name");

static MI_CONST MI_Qualifier CIM_SoftwareElement_Name_Override_qual =
{
    MI_T("Override"),
    MI_STRING,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_RESTRICTED,
    &CIM_SoftwareElement_Name_Override_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_SoftwareElement_Name_quals[] =
{
    &CIM_SoftwareElement_Name_MaxLen_qual,
    &CIM_SoftwareElement_Name_Override_qual,
};

/* property CIM_SoftwareElement.Name */
static MI_CONST MI_PropertyDecl CIM_SoftwareElement_Name_prop =
{
    MI_FLAG_PROPERTY|MI_FLAG_KEY, /* flags */
    0x006E6504, /* code */
    MI_T("Name"), /* name */
    CIM_SoftwareElement_Name_quals, /* qualifiers */
    MI_COUNT(CIM_SoftwareElement_Name_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_SoftwareElement, Name), /* offset */
    MI_T("CIM_ManagedSystemElement"), /* origin */
    MI_T("CIM_SoftwareElement"), /* propagator */
    NULL,
};

static MI_CONST MI_Uint32 CIM_SoftwareElement_Version_MaxLen_qual_value = 64U;

static MI_CONST MI_Qualifier CIM_SoftwareElement_Version_MaxLen_qual =
{
    MI_T("MaxLen"),
    MI_UINT32,
    0,
    &CIM_SoftwareElement_Version_MaxLen_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_SoftwareElement_Version_quals[] =
{
    &CIM_SoftwareElement_Version_MaxLen_qual,
};

/* property CIM_SoftwareElement.Version */
static MI_CONST MI_PropertyDecl CIM_SoftwareElement_Version_prop =
{
    MI_FLAG_PROPERTY|MI_FLAG_KEY, /* flags */
    0x00766E07, /* code */
    MI_T("Version"), /* name */
    CIM_SoftwareElement_Version_quals, /* qualifiers */
    MI_COUNT(CIM_SoftwareElement_Version_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_SoftwareElement, Version), /* offset */
    MI_T("CIM_SoftwareElement"), /* origin */
    MI_T("CIM_SoftwareElement"), /* propagator */
    NULL,
};

/* property CIM_SoftwareElement.SoftwareElementState */
static MI_CONST MI_PropertyDecl CIM_SoftwareElement_SoftwareElementState_prop =
{
    MI_FLAG_PROPERTY|MI_FLAG_KEY, /* flags */
    0x00736514, /* code */
    MI_T("SoftwareElementState"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT16, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_SoftwareElement, SoftwareElementState), /* offset */
    MI_T("CIM_SoftwareElement"), /* origin */
    MI_T("CIM_SoftwareElement"), /* propagator */
    NULL,
};

static MI_CONST MI_Uint32 CIM_SoftwareElement_SoftwareElementID_MaxLen_qual_value = 256U;

static MI_CONST MI_Qualifier CIM_SoftwareElement_SoftwareElementID_MaxLen_qual =
{
    MI_T("MaxLen"),
    MI_UINT32,
    0,
    &CIM_SoftwareElement_SoftwareElementID_MaxLen_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_SoftwareElement_SoftwareElementID_quals[] =
{
    &CIM_SoftwareElement_SoftwareElementID_MaxLen_qual,
};

/* property CIM_SoftwareElement.SoftwareElementID */
static MI_CONST MI_PropertyDecl CIM_SoftwareElement_SoftwareElementID_prop =
{
    MI_FLAG_PROPERTY|MI_FLAG_KEY, /* flags */
    0x00736411, /* code */
    MI_T("SoftwareElementID"), /* name */
    CIM_SoftwareElement_SoftwareElementID_quals, /* qualifiers */
    MI_COUNT(CIM_SoftwareElement_SoftwareElementID_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_SoftwareElement, SoftwareElementID), /* offset */
    MI_T("CIM_SoftwareElement"), /* origin */
    MI_T("CIM_SoftwareElement"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_SoftwareElement_TargetOperatingSystem_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_OperatingSystem.OSType"),
};

static MI_CONST MI_ConstStringA CIM_SoftwareElement_TargetOperatingSystem_ModelCorrespondence_qual_value =
{
    CIM_SoftwareElement_TargetOperatingSystem_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_SoftwareElement_TargetOperatingSystem_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_SoftwareElement_TargetOperatingSystem_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_SoftwareElement_TargetOperatingSystem_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_SoftwareElement_TargetOperatingSystem_quals[] =
{
    &CIM_SoftwareElement_TargetOperatingSystem_ModelCorrespondence_qual,
};

/* property CIM_SoftwareElement.TargetOperatingSystem */
static MI_CONST MI_PropertyDecl CIM_SoftwareElement_TargetOperatingSystem_prop =
{
    MI_FLAG_PROPERTY|MI_FLAG_KEY, /* flags */
    0x00746D15, /* code */
    MI_T("TargetOperatingSystem"), /* name */
    CIM_SoftwareElement_TargetOperatingSystem_quals, /* qualifiers */
    MI_COUNT(CIM_SoftwareElement_TargetOperatingSystem_quals), /* numQualifiers */
    MI_UINT16, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_SoftwareElement, TargetOperatingSystem), /* offset */
    MI_T("CIM_SoftwareElement"), /* origin */
    MI_T("CIM_SoftwareElement"), /* propagator */
    NULL,
};

static MI_CONST MI_Uint32 CIM_SoftwareElement_OtherTargetOS_MaxLen_qual_value = 64U;

static MI_CONST MI_Qualifier CIM_SoftwareElement_OtherTargetOS_MaxLen_qual =
{
    MI_T("MaxLen"),
    MI_UINT32,
    0,
    &CIM_SoftwareElement_OtherTargetOS_MaxLen_qual_value
};

static MI_CONST MI_Char* CIM_SoftwareElement_OtherTargetOS_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_OperatingSystem.OtherTypeDescription"),
};

static MI_CONST MI_ConstStringA CIM_SoftwareElement_OtherTargetOS_ModelCorrespondence_qual_value =
{
    CIM_SoftwareElement_OtherTargetOS_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_SoftwareElement_OtherTargetOS_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_SoftwareElement_OtherTargetOS_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_SoftwareElement_OtherTargetOS_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_SoftwareElement_OtherTargetOS_quals[] =
{
    &CIM_SoftwareElement_OtherTargetOS_MaxLen_qual,
    &CIM_SoftwareElement_OtherTargetOS_ModelCorrespondence_qual,
};

/* property CIM_SoftwareElement.OtherTargetOS */
static MI_CONST MI_PropertyDecl CIM_SoftwareElement_OtherTargetOS_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x006F730D, /* code */
    MI_T("OtherTargetOS"), /* name */
    CIM_SoftwareElement_OtherTargetOS_quals, /* qualifiers */
    MI_COUNT(CIM_SoftwareElement_OtherTargetOS_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_SoftwareElement, OtherTargetOS), /* offset */
    MI_T("CIM_SoftwareElement"), /* origin */
    MI_T("CIM_SoftwareElement"), /* propagator */
    NULL,
};

static MI_CONST MI_Uint32 CIM_SoftwareElement_Manufacturer_MaxLen_qual_value = 256U;

static MI_CONST MI_Qualifier CIM_SoftwareElement_Manufacturer_MaxLen_qual =
{
    MI_T("MaxLen"),
    MI_UINT32,
    0,
    &CIM_SoftwareElement_Manufacturer_MaxLen_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_SoftwareElement_Manufacturer_quals[] =
{
    &CIM_SoftwareElement_Manufacturer_MaxLen_qual,
};

/* property CIM_SoftwareElement.Manufacturer */
static MI_CONST MI_PropertyDecl CIM_SoftwareElement_Manufacturer_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x006D720C, /* code */
    MI_T("Manufacturer"), /* name */
    CIM_SoftwareElement_Manufacturer_quals, /* qualifiers */
    MI_COUNT(CIM_SoftwareElement_Manufacturer_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_SoftwareElement, Manufacturer), /* offset */
    MI_T("CIM_SoftwareElement"), /* origin */
    MI_T("CIM_SoftwareElement"), /* propagator */
    NULL,
};

static MI_CONST MI_Uint32 CIM_SoftwareElement_BuildNumber_MaxLen_qual_value = 64U;

static MI_CONST MI_Qualifier CIM_SoftwareElement_BuildNumber_MaxLen_qual =
{
    MI_T("MaxLen"),
    MI_UINT32,
    0,
    &CIM_SoftwareElement_BuildNumber_MaxLen_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_SoftwareElement_BuildNumber_quals[] =
{
    &CIM_SoftwareElement_BuildNumber_MaxLen_qual,
};

/* property CIM_SoftwareElement.BuildNumber */
static MI_CONST MI_PropertyDecl CIM_SoftwareElement_BuildNumber_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0062720B, /* code */
    MI_T("BuildNumber"), /* name */
    CIM_SoftwareElement_BuildNumber_quals, /* qualifiers */
    MI_COUNT(CIM_SoftwareElement_BuildNumber_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_SoftwareElement, BuildNumber), /* offset */
    MI_T("CIM_SoftwareElement"), /* origin */
    MI_T("CIM_SoftwareElement"), /* propagator */
    NULL,
};

static MI_CONST MI_Uint32 CIM_SoftwareElement_SerialNumber_MaxLen_qual_value = 64U;

static MI_CONST MI_Qualifier CIM_SoftwareElement_SerialNumber_MaxLen_qual =
{
    MI_T("MaxLen"),
    MI_UINT32,
    0,
    &CIM_SoftwareElement_SerialNumber_MaxLen_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_SoftwareElement_SerialNumber_quals[] =
{
    &CIM_SoftwareElement_SerialNumber_MaxLen_qual,
};

/* property CIM_SoftwareElement.SerialNumber */
static MI_CONST MI_PropertyDecl CIM_SoftwareElement_SerialNumber_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0073720C, /* code */
    MI_T("SerialNumber"), /* name */
    CIM_SoftwareElement_SerialNumber_quals, /* qualifiers */
    MI_COUNT(CIM_SoftwareElement_SerialNumber_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_SoftwareElement, SerialNumber), /* offset */
    MI_T("CIM_SoftwareElement"), /* origin */
    MI_T("CIM_SoftwareElement"), /* propagator */
    NULL,
};

static MI_CONST MI_Uint32 CIM_SoftwareElement_CodeSet_MaxLen_qual_value = 64U;

static MI_CONST MI_Qualifier CIM_SoftwareElement_CodeSet_MaxLen_qual =
{
    MI_T("MaxLen"),
    MI_UINT32,
    0,
    &CIM_SoftwareElement_CodeSet_MaxLen_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_SoftwareElement_CodeSet_quals[] =
{
    &CIM_SoftwareElement_CodeSet_MaxLen_qual,
};

/* property CIM_SoftwareElement.CodeSet */
static MI_CONST MI_PropertyDecl CIM_SoftwareElement_CodeSet_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00637407, /* code */
    MI_T("CodeSet"), /* name */
    CIM_SoftwareElement_CodeSet_quals, /* qualifiers */
    MI_COUNT(CIM_SoftwareElement_CodeSet_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_SoftwareElement, CodeSet), /* offset */
    MI_T("CIM_SoftwareElement"), /* origin */
    MI_T("CIM_SoftwareElement"), /* propagator */
    NULL,
};

static MI_CONST MI_Uint32 CIM_SoftwareElement_IdentificationCode_MaxLen_qual_value = 64U;

static MI_CONST MI_Qualifier CIM_SoftwareElement_IdentificationCode_MaxLen_qual =
{
    MI_T("MaxLen"),
    MI_UINT32,
    0,
    &CIM_SoftwareElement_IdentificationCode_MaxLen_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_SoftwareElement_IdentificationCode_quals[] =
{
    &CIM_SoftwareElement_IdentificationCode_MaxLen_qual,
};

/* property CIM_SoftwareElement.IdentificationCode */
static MI_CONST MI_PropertyDecl CIM_SoftwareElement_IdentificationCode_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00696512, /* code */
    MI_T("IdentificationCode"), /* name */
    CIM_SoftwareElement_IdentificationCode_quals, /* qualifiers */
    MI_COUNT(CIM_SoftwareElement_IdentificationCode_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_SoftwareElement, IdentificationCode), /* offset */
    MI_T("CIM_SoftwareElement"), /* origin */
    MI_T("CIM_SoftwareElement"), /* propagator */
    NULL,
};

static MI_CONST MI_Uint32 CIM_SoftwareElement_LanguageEdition_MaxLen_qual_value = 32U;

static MI_CONST MI_Qualifier CIM_SoftwareElement_LanguageEdition_MaxLen_qual =
{
    MI_T("MaxLen"),
    MI_UINT32,
    0,
    &CIM_SoftwareElement_LanguageEdition_MaxLen_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_SoftwareElement_LanguageEdition_quals[] =
{
    &CIM_SoftwareElement_LanguageEdition_MaxLen_qual,
};

/* property CIM_SoftwareElement.LanguageEdition */
static MI_CONST MI_PropertyDecl CIM_SoftwareElement_LanguageEdition_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x006C6E0F, /* code */
    MI_T("LanguageEdition"), /* name */
    CIM_SoftwareElement_LanguageEdition_quals, /* qualifiers */
    MI_COUNT(CIM_SoftwareElement_LanguageEdition_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_SoftwareElement, LanguageEdition), /* offset */
    MI_T("CIM_SoftwareElement"), /* origin */
    MI_T("CIM_SoftwareElement"), /* propagator */
    NULL,
};

static MI_PropertyDecl MI_CONST* MI_CONST CIM_SoftwareElement_props[] =
{
    &CIM_ManagedElement_InstanceID_prop,
    &CIM_ManagedElement_Caption_prop,
    &CIM_ManagedElement_Description_prop,
    &CIM_ManagedElement_ElementName_prop,
    &CIM_ManagedSystemElement_InstallDate_prop,
    &CIM_SoftwareElement_Name_prop,
    &CIM_ManagedSystemElement_OperationalStatus_prop,
    &CIM_ManagedSystemElement_StatusDescriptions_prop,
    &CIM_ManagedSystemElement_Status_prop,
    &CIM_ManagedSystemElement_HealthState_prop,
    &CIM_ManagedSystemElement_CommunicationStatus_prop,
    &CIM_ManagedSystemElement_DetailedStatus_prop,
    &CIM_ManagedSystemElement_OperatingStatus_prop,
    &CIM_ManagedSystemElement_PrimaryStatus_prop,
    &CIM_SoftwareElement_Version_prop,
    &CIM_SoftwareElement_SoftwareElementState_prop,
    &CIM_SoftwareElement_SoftwareElementID_prop,
    &CIM_SoftwareElement_TargetOperatingSystem_prop,
    &CIM_SoftwareElement_OtherTargetOS_prop,
    &CIM_SoftwareElement_Manufacturer_prop,
    &CIM_SoftwareElement_BuildNumber_prop,
    &CIM_SoftwareElement_SerialNumber_prop,
    &CIM_SoftwareElement_CodeSet_prop,
    &CIM_SoftwareElement_IdentificationCode_prop,
    &CIM_SoftwareElement_LanguageEdition_prop,
};

static MI_CONST MI_Char* CIM_SoftwareElement_UMLPackagePath_qual_value = MI_T("CIM::Application::DeploymentModel");

static MI_CONST MI_Qualifier CIM_SoftwareElement_UMLPackagePath_qual =
{
    MI_T("UMLPackagePath"),
    MI_STRING,
    0,
    &CIM_SoftwareElement_UMLPackagePath_qual_value
};

static MI_CONST MI_Char* CIM_SoftwareElement_Version_qual_value = MI_T("2.23.0");

static MI_CONST MI_Qualifier CIM_SoftwareElement_Version_qual =
{
    MI_T("Version"),
    MI_STRING,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_TRANSLATABLE|MI_FLAG_RESTRICTED,
    &CIM_SoftwareElement_Version_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_SoftwareElement_quals[] =
{
    &CIM_SoftwareElement_UMLPackagePath_qual,
    &CIM_SoftwareElement_Version_qual,
};

/* class CIM_SoftwareElement */
MI_CONST MI_ClassDecl CIM_SoftwareElement_rtti =
{
    MI_FLAG_CLASS, /* flags */
    0x00637413, /* code */
    MI_T("CIM_SoftwareElement"), /* name */
    CIM_SoftwareElement_quals, /* qualifiers */
    MI_COUNT(CIM_SoftwareElement_quals), /* numQualifiers */
    CIM_SoftwareElement_props, /* properties */
    MI_COUNT(CIM_SoftwareElement_props), /* numProperties */
    sizeof(CIM_SoftwareElement), /* size */
    MI_T("CIM_LogicalElement"), /* superClass */
    &CIM_LogicalElement_rtti, /* superClassDecl */
    NULL, /* methods */
    0, /* numMethods */
    &schemaDecl, /* schema */
    NULL, /* functions */
    NULL, /* owningClass */
};

/*
**==============================================================================
**
** Apache_HTTPDVirtualHost
**
**==============================================================================
*/

/* property Apache_HTTPDVirtualHost.IPAddresses */
static MI_CONST MI_PropertyDecl Apache_HTTPDVirtualHost_IPAddresses_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0069730B, /* code */
    MI_T("IPAddresses"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_STRINGA, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDVirtualHost, IPAddresses), /* offset */
    MI_T("Apache_HTTPDVirtualHost"), /* origin */
    MI_T("Apache_HTTPDVirtualHost"), /* propagator */
    NULL,
};

/* property Apache_HTTPDVirtualHost.Ports */
static MI_CONST MI_PropertyDecl Apache_HTTPDVirtualHost_Ports_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00707305, /* code */
    MI_T("Ports"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT16A, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDVirtualHost, Ports), /* offset */
    MI_T("Apache_HTTPDVirtualHost"), /* origin */
    MI_T("Apache_HTTPDVirtualHost"), /* propagator */
    NULL,
};

/* property Apache_HTTPDVirtualHost.ServerName */
static MI_CONST MI_PropertyDecl Apache_HTTPDVirtualHost_ServerName_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0073650A, /* code */
    MI_T("ServerName"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDVirtualHost, ServerName), /* offset */
    MI_T("Apache_HTTPDVirtualHost"), /* origin */
    MI_T("Apache_HTTPDVirtualHost"), /* propagator */
    NULL,
};

/* property Apache_HTTPDVirtualHost.ServerAlias */
static MI_CONST MI_PropertyDecl Apache_HTTPDVirtualHost_ServerAlias_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0073730B, /* code */
    MI_T("ServerAlias"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_STRINGA, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDVirtualHost, ServerAlias), /* offset */
    MI_T("Apache_HTTPDVirtualHost"), /* origin */
    MI_T("Apache_HTTPDVirtualHost"), /* propagator */
    NULL,
};

/* property Apache_HTTPDVirtualHost.DocumentRoot */
static MI_CONST MI_PropertyDecl Apache_HTTPDVirtualHost_DocumentRoot_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0064740C, /* code */
    MI_T("DocumentRoot"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDVirtualHost, DocumentRoot), /* offset */
    MI_T("Apache_HTTPDVirtualHost"), /* origin */
    MI_T("Apache_HTTPDVirtualHost"), /* propagator */
    NULL,
};

/* property Apache_HTTPDVirtualHost.ServerAdmin */
static MI_CONST MI_PropertyDecl Apache_HTTPDVirtualHost_ServerAdmin_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00736E0B, /* code */
    MI_T("ServerAdmin"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDVirtualHost, ServerAdmin), /* offset */
    MI_T("Apache_HTTPDVirtualHost"), /* origin */
    MI_T("Apache_HTTPDVirtualHost"), /* propagator */
    NULL,
};

/* property Apache_HTTPDVirtualHost.ErrorLog */
static MI_CONST MI_PropertyDecl Apache_HTTPDVirtualHost_ErrorLog_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00656708, /* code */
    MI_T("ErrorLog"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDVirtualHost, ErrorLog), /* offset */
    MI_T("Apache_HTTPDVirtualHost"), /* origin */
    MI_T("Apache_HTTPDVirtualHost"), /* propagator */
    NULL,
};

/* property Apache_HTTPDVirtualHost.CustomLog */
static MI_CONST MI_PropertyDecl Apache_HTTPDVirtualHost_CustomLog_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00636709, /* code */
    MI_T("CustomLog"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDVirtualHost, CustomLog), /* offset */
    MI_T("Apache_HTTPDVirtualHost"), /* origin */
    MI_T("Apache_HTTPDVirtualHost"), /* propagator */
    NULL,
};

/* property Apache_HTTPDVirtualHost.AccessLog */
static MI_CONST MI_PropertyDecl Apache_HTTPDVirtualHost_AccessLog_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00616709, /* code */
    MI_T("AccessLog"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDVirtualHost, AccessLog), /* offset */
    MI_T("Apache_HTTPDVirtualHost"), /* origin */
    MI_T("Apache_HTTPDVirtualHost"), /* propagator */
    NULL,
};

static MI_PropertyDecl MI_CONST* MI_CONST Apache_HTTPDVirtualHost_props[] =
{
    &CIM_ManagedElement_InstanceID_prop,
    &CIM_ManagedElement_Caption_prop,
    &CIM_ManagedElement_Description_prop,
    &CIM_ManagedElement_ElementName_prop,
    &CIM_ManagedSystemElement_InstallDate_prop,
    &CIM_SoftwareElement_Name_prop,
    &CIM_ManagedSystemElement_OperationalStatus_prop,
    &CIM_ManagedSystemElement_StatusDescriptions_prop,
    &CIM_ManagedSystemElement_Status_prop,
    &CIM_ManagedSystemElement_HealthState_prop,
    &CIM_ManagedSystemElement_CommunicationStatus_prop,
    &CIM_ManagedSystemElement_DetailedStatus_prop,
    &CIM_ManagedSystemElement_OperatingStatus_prop,
    &CIM_ManagedSystemElement_PrimaryStatus_prop,
    &CIM_SoftwareElement_Version_prop,
    &CIM_SoftwareElement_SoftwareElementState_prop,
    &CIM_SoftwareElement_SoftwareElementID_prop,
    &CIM_SoftwareElement_TargetOperatingSystem_prop,
    &CIM_SoftwareElement_OtherTargetOS_prop,
    &CIM_SoftwareElement_Manufacturer_prop,
    &CIM_SoftwareElement_BuildNumber_prop,
    &CIM_SoftwareElement_SerialNumber_prop,
    &CIM_SoftwareElement_CodeSet_prop,
    &CIM_SoftwareElement_IdentificationCode_prop,
    &CIM_SoftwareElement_LanguageEdition_prop,
    &Apache_HTTPDVirtualHost_IPAddresses_prop,
    &Apache_HTTPDVirtualHost_Ports_prop,
    &Apache_HTTPDVirtualHost_ServerName_prop,
    &Apache_HTTPDVirtualHost_ServerAlias_prop,
    &Apache_HTTPDVirtualHost_DocumentRoot_prop,
    &Apache_HTTPDVirtualHost_ServerAdmin_prop,
    &Apache_HTTPDVirtualHost_ErrorLog_prop,
    &Apache_HTTPDVirtualHost_CustomLog_prop,
    &Apache_HTTPDVirtualHost_AccessLog_prop,
};

static MI_CONST MI_ProviderFT Apache_HTTPDVirtualHost_funcs =
{
  (MI_ProviderFT_Load)Apache_HTTPDVirtualHost_Load,
  (MI_ProviderFT_Unload)Apache_HTTPDVirtualHost_Unload,
  (MI_ProviderFT_GetInstance)Apache_HTTPDVirtualHost_GetInstance,
  (MI_ProviderFT_EnumerateInstances)Apache_HTTPDVirtualHost_EnumerateInstances,
  (MI_ProviderFT_CreateInstance)Apache_HTTPDVirtualHost_CreateInstance,
  (MI_ProviderFT_ModifyInstance)Apache_HTTPDVirtualHost_ModifyInstance,
  (MI_ProviderFT_DeleteInstance)Apache_HTTPDVirtualHost_DeleteInstance,
  (MI_ProviderFT_AssociatorInstances)NULL,
  (MI_ProviderFT_ReferenceInstances)NULL,
  (MI_ProviderFT_EnableIndications)NULL,
  (MI_ProviderFT_DisableIndications)NULL,
  (MI_ProviderFT_Subscribe)NULL,
  (MI_ProviderFT_Unsubscribe)NULL,
  (MI_ProviderFT_Invoke)NULL,
};

static MI_CONST MI_Char* Apache_HTTPDVirtualHost_UMLPackagePath_qual_value = MI_T("CIM::Application::DeploymentModel");

static MI_CONST MI_Qualifier Apache_HTTPDVirtualHost_UMLPackagePath_qual =
{
    MI_T("UMLPackagePath"),
    MI_STRING,
    0,
    &Apache_HTTPDVirtualHost_UMLPackagePath_qual_value
};

static MI_CONST MI_Char* Apache_HTTPDVirtualHost_Version_qual_value = MI_T("1.0.0");

static MI_CONST MI_Qualifier Apache_HTTPDVirtualHost_Version_qual =
{
    MI_T("Version"),
    MI_STRING,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_TRANSLATABLE|MI_FLAG_RESTRICTED,
    &Apache_HTTPDVirtualHost_Version_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST Apache_HTTPDVirtualHost_quals[] =
{
    &Apache_HTTPDVirtualHost_UMLPackagePath_qual,
    &Apache_HTTPDVirtualHost_Version_qual,
};

/* class Apache_HTTPDVirtualHost */
MI_CONST MI_ClassDecl Apache_HTTPDVirtualHost_rtti =
{
    MI_FLAG_CLASS, /* flags */
    0x00617417, /* code */
    MI_T("Apache_HTTPDVirtualHost"), /* name */
    Apache_HTTPDVirtualHost_quals, /* qualifiers */
    MI_COUNT(Apache_HTTPDVirtualHost_quals), /* numQualifiers */
    Apache_HTTPDVirtualHost_props, /* properties */
    MI_COUNT(Apache_HTTPDVirtualHost_props), /* numProperties */
    sizeof(Apache_HTTPDVirtualHost), /* size */
    MI_T("CIM_SoftwareElement"), /* superClass */
    &CIM_SoftwareElement_rtti, /* superClassDecl */
    NULL, /* methods */
    0, /* numMethods */
    &schemaDecl, /* schema */
    &Apache_HTTPDVirtualHost_funcs, /* functions */
    NULL, /* owningClass */
};

/*
**==============================================================================
**
** Apache_HTTPDVirtualHostCertificate
**
**==============================================================================
*/

/* property Apache_HTTPDVirtualHostCertificate.ServerName */
static MI_CONST MI_PropertyDecl Apache_HTTPDVirtualHostCertificate_ServerName_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0073650A, /* code */
    MI_T("ServerName"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDVirtualHostCertificate, ServerName), /* offset */
    MI_T("Apache_HTTPDVirtualHostCertificate"), /* origin */
    MI_T("Apache_HTTPDVirtualHostCertificate"), /* propagator */
    NULL,
};

/* property Apache_HTTPDVirtualHostCertificate.ExpirationDate */
static MI_CONST MI_PropertyDecl Apache_HTTPDVirtualHostCertificate_ExpirationDate_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0065650E, /* code */
    MI_T("ExpirationDate"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_DATETIME, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDVirtualHostCertificate, ExpirationDate), /* offset */
    MI_T("Apache_HTTPDVirtualHostCertificate"), /* origin */
    MI_T("Apache_HTTPDVirtualHostCertificate"), /* propagator */
    NULL,
};

/* property Apache_HTTPDVirtualHostCertificate.DaysUntilExpiration */
static MI_CONST MI_PropertyDecl Apache_HTTPDVirtualHostCertificate_DaysUntilExpiration_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00646E13, /* code */
    MI_T("DaysUntilExpiration"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT16, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDVirtualHostCertificate, DaysUntilExpiration), /* offset */
    MI_T("Apache_HTTPDVirtualHostCertificate"), /* origin */
    MI_T("Apache_HTTPDVirtualHostCertificate"), /* propagator */
    NULL,
};

/* property Apache_HTTPDVirtualHostCertificate.FileName */
static MI_CONST MI_PropertyDecl Apache_HTTPDVirtualHostCertificate_FileName_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00666508, /* code */
    MI_T("FileName"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDVirtualHostCertificate, FileName), /* offset */
    MI_T("Apache_HTTPDVirtualHostCertificate"), /* origin */
    MI_T("Apache_HTTPDVirtualHostCertificate"), /* propagator */
    NULL,
};

static MI_PropertyDecl MI_CONST* MI_CONST Apache_HTTPDVirtualHostCertificate_props[] =
{
    &CIM_ManagedElement_InstanceID_prop,
    &CIM_ManagedElement_Caption_prop,
    &CIM_ManagedElement_Description_prop,
    &CIM_ManagedElement_ElementName_prop,
    &CIM_ManagedSystemElement_InstallDate_prop,
    &CIM_SoftwareElement_Name_prop,
    &CIM_ManagedSystemElement_OperationalStatus_prop,
    &CIM_ManagedSystemElement_StatusDescriptions_prop,
    &CIM_ManagedSystemElement_Status_prop,
    &CIM_ManagedSystemElement_HealthState_prop,
    &CIM_ManagedSystemElement_CommunicationStatus_prop,
    &CIM_ManagedSystemElement_DetailedStatus_prop,
    &CIM_ManagedSystemElement_OperatingStatus_prop,
    &CIM_ManagedSystemElement_PrimaryStatus_prop,
    &CIM_SoftwareElement_Version_prop,
    &CIM_SoftwareElement_SoftwareElementState_prop,
    &CIM_SoftwareElement_SoftwareElementID_prop,
    &CIM_SoftwareElement_TargetOperatingSystem_prop,
    &CIM_SoftwareElement_OtherTargetOS_prop,
    &CIM_SoftwareElement_Manufacturer_prop,
    &CIM_SoftwareElement_BuildNumber_prop,
    &CIM_SoftwareElement_SerialNumber_prop,
    &CIM_SoftwareElement_CodeSet_prop,
    &CIM_SoftwareElement_IdentificationCode_prop,
    &CIM_SoftwareElement_LanguageEdition_prop,
    &Apache_HTTPDVirtualHostCertificate_ServerName_prop,
    &Apache_HTTPDVirtualHostCertificate_ExpirationDate_prop,
    &Apache_HTTPDVirtualHostCertificate_DaysUntilExpiration_prop,
    &Apache_HTTPDVirtualHostCertificate_FileName_prop,
};

static MI_CONST MI_ProviderFT Apache_HTTPDVirtualHostCertificate_funcs =
{
  (MI_ProviderFT_Load)Apache_HTTPDVirtualHostCertificate_Load,
  (MI_ProviderFT_Unload)Apache_HTTPDVirtualHostCertificate_Unload,
  (MI_ProviderFT_GetInstance)Apache_HTTPDVirtualHostCertificate_GetInstance,
  (MI_ProviderFT_EnumerateInstances)Apache_HTTPDVirtualHostCertificate_EnumerateInstances,
  (MI_ProviderFT_CreateInstance)Apache_HTTPDVirtualHostCertificate_CreateInstance,
  (MI_ProviderFT_ModifyInstance)Apache_HTTPDVirtualHostCertificate_ModifyInstance,
  (MI_ProviderFT_DeleteInstance)Apache_HTTPDVirtualHostCertificate_DeleteInstance,
  (MI_ProviderFT_AssociatorInstances)NULL,
  (MI_ProviderFT_ReferenceInstances)NULL,
  (MI_ProviderFT_EnableIndications)NULL,
  (MI_ProviderFT_DisableIndications)NULL,
  (MI_ProviderFT_Subscribe)NULL,
  (MI_ProviderFT_Unsubscribe)NULL,
  (MI_ProviderFT_Invoke)NULL,
};

static MI_CONST MI_Char* Apache_HTTPDVirtualHostCertificate_UMLPackagePath_qual_value = MI_T("CIM::Application::DeploymentModel");

static MI_CONST MI_Qualifier Apache_HTTPDVirtualHostCertificate_UMLPackagePath_qual =
{
    MI_T("UMLPackagePath"),
    MI_STRING,
    0,
    &Apache_HTTPDVirtualHostCertificate_UMLPackagePath_qual_value
};

static MI_CONST MI_Char* Apache_HTTPDVirtualHostCertificate_Version_qual_value = MI_T("1.0.0");

static MI_CONST MI_Qualifier Apache_HTTPDVirtualHostCertificate_Version_qual =
{
    MI_T("Version"),
    MI_STRING,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_TRANSLATABLE|MI_FLAG_RESTRICTED,
    &Apache_HTTPDVirtualHostCertificate_Version_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST Apache_HTTPDVirtualHostCertificate_quals[] =
{
    &Apache_HTTPDVirtualHostCertificate_UMLPackagePath_qual,
    &Apache_HTTPDVirtualHostCertificate_Version_qual,
};

/* class Apache_HTTPDVirtualHostCertificate */
MI_CONST MI_ClassDecl Apache_HTTPDVirtualHostCertificate_rtti =
{
    MI_FLAG_CLASS, /* flags */
    0x00616522, /* code */
    MI_T("Apache_HTTPDVirtualHostCertificate"), /* name */
    Apache_HTTPDVirtualHostCertificate_quals, /* qualifiers */
    MI_COUNT(Apache_HTTPDVirtualHostCertificate_quals), /* numQualifiers */
    Apache_HTTPDVirtualHostCertificate_props, /* properties */
    MI_COUNT(Apache_HTTPDVirtualHostCertificate_props), /* numProperties */
    sizeof(Apache_HTTPDVirtualHostCertificate), /* size */
    MI_T("CIM_SoftwareElement"), /* superClass */
    &CIM_SoftwareElement_rtti, /* superClassDecl */
    NULL, /* methods */
    0, /* numMethods */
    &schemaDecl, /* schema */
    &Apache_HTTPDVirtualHostCertificate_funcs, /* functions */
    NULL, /* owningClass */
};

/*
**==============================================================================
**
** Apache_HTTPDVirtualHostStatistics
**
**==============================================================================
*/

/* property Apache_HTTPDVirtualHostStatistics.ServerName */
static MI_CONST MI_PropertyDecl Apache_HTTPDVirtualHostStatistics_ServerName_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0073650A, /* code */
    MI_T("ServerName"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDVirtualHostStatistics, ServerName), /* offset */
    MI_T("Apache_HTTPDVirtualHostStatistics"), /* origin */
    MI_T("Apache_HTTPDVirtualHostStatistics"), /* propagator */
    NULL,
};

/* property Apache_HTTPDVirtualHostStatistics.RequestsTotal */
static MI_CONST MI_PropertyDecl Apache_HTTPDVirtualHostStatistics_RequestsTotal_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00726C0D, /* code */
    MI_T("RequestsTotal"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT64, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDVirtualHostStatistics, RequestsTotal), /* offset */
    MI_T("Apache_HTTPDVirtualHostStatistics"), /* origin */
    MI_T("Apache_HTTPDVirtualHostStatistics"), /* propagator */
    NULL,
};

/* property Apache_HTTPDVirtualHostStatistics.RequestsTotalBytes */
static MI_CONST MI_PropertyDecl Apache_HTTPDVirtualHostStatistics_RequestsTotalBytes_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00727312, /* code */
    MI_T("RequestsTotalBytes"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT64, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDVirtualHostStatistics, RequestsTotalBytes), /* offset */
    MI_T("Apache_HTTPDVirtualHostStatistics"), /* origin */
    MI_T("Apache_HTTPDVirtualHostStatistics"), /* propagator */
    NULL,
};

/* property Apache_HTTPDVirtualHostStatistics.RequestsPerSecond */
static MI_CONST MI_PropertyDecl Apache_HTTPDVirtualHostStatistics_RequestsPerSecond_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00726411, /* code */
    MI_T("RequestsPerSecond"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT32, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDVirtualHostStatistics, RequestsPerSecond), /* offset */
    MI_T("Apache_HTTPDVirtualHostStatistics"), /* origin */
    MI_T("Apache_HTTPDVirtualHostStatistics"), /* propagator */
    NULL,
};

/* property Apache_HTTPDVirtualHostStatistics.KBPerRequest */
static MI_CONST MI_PropertyDecl Apache_HTTPDVirtualHostStatistics_KBPerRequest_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x006B740C, /* code */
    MI_T("KBPerRequest"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT32, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDVirtualHostStatistics, KBPerRequest), /* offset */
    MI_T("Apache_HTTPDVirtualHostStatistics"), /* origin */
    MI_T("Apache_HTTPDVirtualHostStatistics"), /* propagator */
    NULL,
};

/* property Apache_HTTPDVirtualHostStatistics.KBPerSecond */
static MI_CONST MI_PropertyDecl Apache_HTTPDVirtualHostStatistics_KBPerSecond_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x006B640B, /* code */
    MI_T("KBPerSecond"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT32, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDVirtualHostStatistics, KBPerSecond), /* offset */
    MI_T("Apache_HTTPDVirtualHostStatistics"), /* origin */
    MI_T("Apache_HTTPDVirtualHostStatistics"), /* propagator */
    NULL,
};

/* property Apache_HTTPDVirtualHostStatistics.ErrorCount400 */
static MI_CONST MI_PropertyDecl Apache_HTTPDVirtualHostStatistics_ErrorCount400_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0065300D, /* code */
    MI_T("ErrorCount400"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT64, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDVirtualHostStatistics, ErrorCount400), /* offset */
    MI_T("Apache_HTTPDVirtualHostStatistics"), /* origin */
    MI_T("Apache_HTTPDVirtualHostStatistics"), /* propagator */
    NULL,
};

/* property Apache_HTTPDVirtualHostStatistics.ErrorCount500 */
static MI_CONST MI_PropertyDecl Apache_HTTPDVirtualHostStatistics_ErrorCount500_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0065300D, /* code */
    MI_T("ErrorCount500"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT64, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDVirtualHostStatistics, ErrorCount500), /* offset */
    MI_T("Apache_HTTPDVirtualHostStatistics"), /* origin */
    MI_T("Apache_HTTPDVirtualHostStatistics"), /* propagator */
    NULL,
};

/* property Apache_HTTPDVirtualHostStatistics.ErrorsPerMinute400 */
static MI_CONST MI_PropertyDecl Apache_HTTPDVirtualHostStatistics_ErrorsPerMinute400_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00653012, /* code */
    MI_T("ErrorsPerMinute400"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT32, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDVirtualHostStatistics, ErrorsPerMinute400), /* offset */
    MI_T("Apache_HTTPDVirtualHostStatistics"), /* origin */
    MI_T("Apache_HTTPDVirtualHostStatistics"), /* propagator */
    NULL,
};

/* property Apache_HTTPDVirtualHostStatistics.ErrorsPerMinute500 */
static MI_CONST MI_PropertyDecl Apache_HTTPDVirtualHostStatistics_ErrorsPerMinute500_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00653012, /* code */
    MI_T("ErrorsPerMinute500"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT32, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDVirtualHostStatistics, ErrorsPerMinute500), /* offset */
    MI_T("Apache_HTTPDVirtualHostStatistics"), /* origin */
    MI_T("Apache_HTTPDVirtualHostStatistics"), /* propagator */
    NULL,
};

static MI_PropertyDecl MI_CONST* MI_CONST Apache_HTTPDVirtualHostStatistics_props[] =
{
    &CIM_StatisticalData_InstanceID_prop,
    &CIM_ManagedElement_Caption_prop,
    &CIM_ManagedElement_Description_prop,
    &CIM_StatisticalData_ElementName_prop,
    &CIM_StatisticalData_StartStatisticTime_prop,
    &CIM_StatisticalData_StatisticTime_prop,
    &CIM_StatisticalData_SampleInterval_prop,
    &Apache_HTTPDVirtualHostStatistics_ServerName_prop,
    &Apache_HTTPDVirtualHostStatistics_RequestsTotal_prop,
    &Apache_HTTPDVirtualHostStatistics_RequestsTotalBytes_prop,
    &Apache_HTTPDVirtualHostStatistics_RequestsPerSecond_prop,
    &Apache_HTTPDVirtualHostStatistics_KBPerRequest_prop,
    &Apache_HTTPDVirtualHostStatistics_KBPerSecond_prop,
    &Apache_HTTPDVirtualHostStatistics_ErrorCount400_prop,
    &Apache_HTTPDVirtualHostStatistics_ErrorCount500_prop,
    &Apache_HTTPDVirtualHostStatistics_ErrorsPerMinute400_prop,
    &Apache_HTTPDVirtualHostStatistics_ErrorsPerMinute500_prop,
};

/* parameter Apache_HTTPDVirtualHostStatistics.ResetSelectedStats(): SelectedStatistics */
static MI_CONST MI_ParameterDecl Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_SelectedStatistics_param =
{
    MI_FLAG_PARAMETER|MI_FLAG_IN, /* flags */
    0x00737312, /* code */
    MI_T("SelectedStatistics"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_STRINGA, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDVirtualHostStatistics_ResetSelectedStats, SelectedStatistics), /* offset */
};

/* parameter Apache_HTTPDVirtualHostStatistics.ResetSelectedStats(): MIReturn */
static MI_CONST MI_ParameterDecl Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_MIReturn_param =
{
    MI_FLAG_PARAMETER|MI_FLAG_OUT, /* flags */
    0x006D6E08, /* code */
    MI_T("MIReturn"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT32, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDVirtualHostStatistics_ResetSelectedStats, MIReturn), /* offset */
};

static MI_ParameterDecl MI_CONST* MI_CONST Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_params[] =
{
    &Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_MIReturn_param,
    &Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_SelectedStatistics_param,
};

/* method Apache_HTTPDVirtualHostStatistics.ResetSelectedStats() */
MI_CONST MI_MethodDecl Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_rtti =
{
    MI_FLAG_METHOD, /* flags */
    0x00727312, /* code */
    MI_T("ResetSelectedStats"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_params, /* parameters */
    MI_COUNT(Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_params), /* numParameters */
    sizeof(Apache_HTTPDVirtualHostStatistics_ResetSelectedStats), /* size */
    MI_UINT32, /* returnType */
    MI_T("CIM_StatisticalData"), /* origin */
    MI_T("CIM_StatisticalData"), /* propagator */
    &schemaDecl, /* schema */
    (MI_ProviderFT_Invoke)Apache_HTTPDVirtualHostStatistics_Invoke_ResetSelectedStats, /* method */
};

static MI_MethodDecl MI_CONST* MI_CONST Apache_HTTPDVirtualHostStatistics_meths[] =
{
    &Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_rtti,
};

static MI_CONST MI_ProviderFT Apache_HTTPDVirtualHostStatistics_funcs =
{
  (MI_ProviderFT_Load)Apache_HTTPDVirtualHostStatistics_Load,
  (MI_ProviderFT_Unload)Apache_HTTPDVirtualHostStatistics_Unload,
  (MI_ProviderFT_GetInstance)Apache_HTTPDVirtualHostStatistics_GetInstance,
  (MI_ProviderFT_EnumerateInstances)Apache_HTTPDVirtualHostStatistics_EnumerateInstances,
  (MI_ProviderFT_CreateInstance)Apache_HTTPDVirtualHostStatistics_CreateInstance,
  (MI_ProviderFT_ModifyInstance)Apache_HTTPDVirtualHostStatistics_ModifyInstance,
  (MI_ProviderFT_DeleteInstance)Apache_HTTPDVirtualHostStatistics_DeleteInstance,
  (MI_ProviderFT_AssociatorInstances)NULL,
  (MI_ProviderFT_ReferenceInstances)NULL,
  (MI_ProviderFT_EnableIndications)NULL,
  (MI_ProviderFT_DisableIndications)NULL,
  (MI_ProviderFT_Subscribe)NULL,
  (MI_ProviderFT_Unsubscribe)NULL,
  (MI_ProviderFT_Invoke)NULL,
};

static MI_CONST MI_Char* Apache_HTTPDVirtualHostStatistics_UMLPackagePath_qual_value = MI_T("CIM::Core::Statistics");

static MI_CONST MI_Qualifier Apache_HTTPDVirtualHostStatistics_UMLPackagePath_qual =
{
    MI_T("UMLPackagePath"),
    MI_STRING,
    0,
    &Apache_HTTPDVirtualHostStatistics_UMLPackagePath_qual_value
};

static MI_CONST MI_Char* Apache_HTTPDVirtualHostStatistics_Version_qual_value = MI_T("1.0.0");

static MI_CONST MI_Qualifier Apache_HTTPDVirtualHostStatistics_Version_qual =
{
    MI_T("Version"),
    MI_STRING,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_TRANSLATABLE|MI_FLAG_RESTRICTED,
    &Apache_HTTPDVirtualHostStatistics_Version_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST Apache_HTTPDVirtualHostStatistics_quals[] =
{
    &Apache_HTTPDVirtualHostStatistics_UMLPackagePath_qual,
    &Apache_HTTPDVirtualHostStatistics_Version_qual,
};

/* class Apache_HTTPDVirtualHostStatistics */
MI_CONST MI_ClassDecl Apache_HTTPDVirtualHostStatistics_rtti =
{
    MI_FLAG_CLASS, /* flags */
    0x00617321, /* code */
    MI_T("Apache_HTTPDVirtualHostStatistics"), /* name */
    Apache_HTTPDVirtualHostStatistics_quals, /* qualifiers */
    MI_COUNT(Apache_HTTPDVirtualHostStatistics_quals), /* numQualifiers */
    Apache_HTTPDVirtualHostStatistics_props, /* properties */
    MI_COUNT(Apache_HTTPDVirtualHostStatistics_props), /* numProperties */
    sizeof(Apache_HTTPDVirtualHostStatistics), /* size */
    MI_T("CIM_StatisticalData"), /* superClass */
    &CIM_StatisticalData_rtti, /* superClassDecl */
    Apache_HTTPDVirtualHostStatistics_meths, /* methods */
    MI_COUNT(Apache_HTTPDVirtualHostStatistics_meths), /* numMethods */
    &schemaDecl, /* schema */
    &Apache_HTTPDVirtualHostStatistics_funcs, /* functions */
    NULL, /* owningClass */
};

/*
**==============================================================================
**
** __mi_server
**
**==============================================================================
*/

MI_Server* __mi_server;
/*
**==============================================================================
**
** Schema
**
**==============================================================================
*/

static MI_ClassDecl MI_CONST* MI_CONST classes[] =
{
    &Apache_HTTPDServer_rtti,
    &Apache_HTTPDServerStatistics_rtti,
    &Apache_HTTPDVirtualHost_rtti,
    &Apache_HTTPDVirtualHostCertificate_rtti,
    &Apache_HTTPDVirtualHostStatistics_rtti,
    &CIM_Collection_rtti,
    &CIM_InstalledProduct_rtti,
    &CIM_LogicalElement_rtti,
    &CIM_ManagedElement_rtti,
    &CIM_ManagedSystemElement_rtti,
    &CIM_SoftwareElement_rtti,
    &CIM_StatisticalData_rtti,
};

MI_SchemaDecl schemaDecl =
{
    NULL, /* qualifierDecls */
    0, /* numQualifierDecls */
    classes, /* classDecls */
    MI_COUNT(classes), /* classDecls */
};

/*
**==============================================================================
**
** MI_Server Methods
**
**==============================================================================
*/

MI_Result MI_CALL MI_Server_GetVersion(
    MI_Uint32* version){
    return __mi_server->serverFT->GetVersion(version);
}

MI_Result MI_CALL MI_Server_GetSystemName(
    const MI_Char** systemName)
{
    return __mi_server->serverFT->GetSystemName(systemName);
}

