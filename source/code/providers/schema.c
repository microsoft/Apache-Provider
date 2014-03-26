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
#include "Apache_HTTPDProcess.h"
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
** CIM_Job
**
**==============================================================================
*/

static MI_CONST MI_Char* CIM_Job_JobStatus_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_ManagedSystemElement.OperationalStatus"),
};

static MI_CONST MI_ConstStringA CIM_Job_JobStatus_ModelCorrespondence_qual_value =
{
    CIM_Job_JobStatus_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_Job_JobStatus_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_Job_JobStatus_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_Job_JobStatus_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Job_JobStatus_quals[] =
{
    &CIM_Job_JobStatus_ModelCorrespondence_qual,
};

/* property CIM_Job.JobStatus */
static MI_CONST MI_PropertyDecl CIM_Job_JobStatus_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x006A7309, /* code */
    MI_T("JobStatus"), /* name */
    CIM_Job_JobStatus_quals, /* qualifiers */
    MI_COUNT(CIM_Job_JobStatus_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Job, JobStatus), /* offset */
    MI_T("CIM_Job"), /* origin */
    MI_T("CIM_Job"), /* propagator */
    NULL,
};

/* property CIM_Job.TimeSubmitted */
static MI_CONST MI_PropertyDecl CIM_Job_TimeSubmitted_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0074640D, /* code */
    MI_T("TimeSubmitted"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_DATETIME, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Job, TimeSubmitted), /* offset */
    MI_T("CIM_Job"), /* origin */
    MI_T("CIM_Job"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_Job_ScheduledStartTime_Deprecated_qual_data_value[] =
{
    MI_T("CIM_Job.RunMonth"),
    MI_T("CIM_Job.RunDay"),
    MI_T("CIM_Job.RunDayOfWeek"),
    MI_T("CIM_Job.RunStartInterval"),
};

static MI_CONST MI_ConstStringA CIM_Job_ScheduledStartTime_Deprecated_qual_value =
{
    CIM_Job_ScheduledStartTime_Deprecated_qual_data_value,
    MI_COUNT(CIM_Job_ScheduledStartTime_Deprecated_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_Job_ScheduledStartTime_Deprecated_qual =
{
    MI_T("Deprecated"),
    MI_STRINGA,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_RESTRICTED,
    &CIM_Job_ScheduledStartTime_Deprecated_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Job_ScheduledStartTime_quals[] =
{
    &CIM_Job_ScheduledStartTime_Deprecated_qual,
};

/* property CIM_Job.ScheduledStartTime */
static MI_CONST MI_PropertyDecl CIM_Job_ScheduledStartTime_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00736512, /* code */
    MI_T("ScheduledStartTime"), /* name */
    CIM_Job_ScheduledStartTime_quals, /* qualifiers */
    MI_COUNT(CIM_Job_ScheduledStartTime_quals), /* numQualifiers */
    MI_DATETIME, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Job, ScheduledStartTime), /* offset */
    MI_T("CIM_Job"), /* origin */
    MI_T("CIM_Job"), /* propagator */
    NULL,
};

/* property CIM_Job.StartTime */
static MI_CONST MI_PropertyDecl CIM_Job_StartTime_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00736509, /* code */
    MI_T("StartTime"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_DATETIME, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Job, StartTime), /* offset */
    MI_T("CIM_Job"), /* origin */
    MI_T("CIM_Job"), /* propagator */
    NULL,
};

/* property CIM_Job.ElapsedTime */
static MI_CONST MI_PropertyDecl CIM_Job_ElapsedTime_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0065650B, /* code */
    MI_T("ElapsedTime"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_DATETIME, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Job, ElapsedTime), /* offset */
    MI_T("CIM_Job"), /* origin */
    MI_T("CIM_Job"), /* propagator */
    NULL,
};

static MI_CONST MI_Uint32 CIM_Job_JobRunTimes_value = 1U;

/* property CIM_Job.JobRunTimes */
static MI_CONST MI_PropertyDecl CIM_Job_JobRunTimes_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x006A730B, /* code */
    MI_T("JobRunTimes"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT32, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Job, JobRunTimes), /* offset */
    MI_T("CIM_Job"), /* origin */
    MI_T("CIM_Job"), /* propagator */
    &CIM_Job_JobRunTimes_value,
};

static MI_CONST MI_Char* CIM_Job_RunMonth_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_Job.RunDay"),
    MI_T("CIM_Job.RunDayOfWeek"),
    MI_T("CIM_Job.RunStartInterval"),
};

static MI_CONST MI_ConstStringA CIM_Job_RunMonth_ModelCorrespondence_qual_value =
{
    CIM_Job_RunMonth_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_Job_RunMonth_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_Job_RunMonth_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_Job_RunMonth_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Job_RunMonth_quals[] =
{
    &CIM_Job_RunMonth_ModelCorrespondence_qual,
};

/* property CIM_Job.RunMonth */
static MI_CONST MI_PropertyDecl CIM_Job_RunMonth_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00726808, /* code */
    MI_T("RunMonth"), /* name */
    CIM_Job_RunMonth_quals, /* qualifiers */
    MI_COUNT(CIM_Job_RunMonth_quals), /* numQualifiers */
    MI_UINT8, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Job, RunMonth), /* offset */
    MI_T("CIM_Job"), /* origin */
    MI_T("CIM_Job"), /* propagator */
    NULL,
};

static MI_CONST MI_Sint64 CIM_Job_RunDay_MinValue_qual_value = -MI_LL(31);

static MI_CONST MI_Qualifier CIM_Job_RunDay_MinValue_qual =
{
    MI_T("MinValue"),
    MI_SINT64,
    0,
    &CIM_Job_RunDay_MinValue_qual_value
};

static MI_CONST MI_Sint64 CIM_Job_RunDay_MaxValue_qual_value = MI_LL(31);

static MI_CONST MI_Qualifier CIM_Job_RunDay_MaxValue_qual =
{
    MI_T("MaxValue"),
    MI_SINT64,
    0,
    &CIM_Job_RunDay_MaxValue_qual_value
};

static MI_CONST MI_Char* CIM_Job_RunDay_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_Job.RunMonth"),
    MI_T("CIM_Job.RunDayOfWeek"),
    MI_T("CIM_Job.RunStartInterval"),
};

static MI_CONST MI_ConstStringA CIM_Job_RunDay_ModelCorrespondence_qual_value =
{
    CIM_Job_RunDay_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_Job_RunDay_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_Job_RunDay_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_Job_RunDay_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Job_RunDay_quals[] =
{
    &CIM_Job_RunDay_MinValue_qual,
    &CIM_Job_RunDay_MaxValue_qual,
    &CIM_Job_RunDay_ModelCorrespondence_qual,
};

/* property CIM_Job.RunDay */
static MI_CONST MI_PropertyDecl CIM_Job_RunDay_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00727906, /* code */
    MI_T("RunDay"), /* name */
    CIM_Job_RunDay_quals, /* qualifiers */
    MI_COUNT(CIM_Job_RunDay_quals), /* numQualifiers */
    MI_SINT8, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Job, RunDay), /* offset */
    MI_T("CIM_Job"), /* origin */
    MI_T("CIM_Job"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_Job_RunDayOfWeek_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_Job.RunMonth"),
    MI_T("CIM_Job.RunDay"),
    MI_T("CIM_Job.RunStartInterval"),
};

static MI_CONST MI_ConstStringA CIM_Job_RunDayOfWeek_ModelCorrespondence_qual_value =
{
    CIM_Job_RunDayOfWeek_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_Job_RunDayOfWeek_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_Job_RunDayOfWeek_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_Job_RunDayOfWeek_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Job_RunDayOfWeek_quals[] =
{
    &CIM_Job_RunDayOfWeek_ModelCorrespondence_qual,
};

/* property CIM_Job.RunDayOfWeek */
static MI_CONST MI_PropertyDecl CIM_Job_RunDayOfWeek_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00726B0C, /* code */
    MI_T("RunDayOfWeek"), /* name */
    CIM_Job_RunDayOfWeek_quals, /* qualifiers */
    MI_COUNT(CIM_Job_RunDayOfWeek_quals), /* numQualifiers */
    MI_SINT8, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Job, RunDayOfWeek), /* offset */
    MI_T("CIM_Job"), /* origin */
    MI_T("CIM_Job"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_Job_RunStartInterval_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_Job.RunMonth"),
    MI_T("CIM_Job.RunDay"),
    MI_T("CIM_Job.RunDayOfWeek"),
    MI_T("CIM_Job.RunStartInterval"),
};

static MI_CONST MI_ConstStringA CIM_Job_RunStartInterval_ModelCorrespondence_qual_value =
{
    CIM_Job_RunStartInterval_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_Job_RunStartInterval_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_Job_RunStartInterval_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_Job_RunStartInterval_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Job_RunStartInterval_quals[] =
{
    &CIM_Job_RunStartInterval_ModelCorrespondence_qual,
};

/* property CIM_Job.RunStartInterval */
static MI_CONST MI_PropertyDecl CIM_Job_RunStartInterval_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00726C10, /* code */
    MI_T("RunStartInterval"), /* name */
    CIM_Job_RunStartInterval_quals, /* qualifiers */
    MI_COUNT(CIM_Job_RunStartInterval_quals), /* numQualifiers */
    MI_DATETIME, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Job, RunStartInterval), /* offset */
    MI_T("CIM_Job"), /* origin */
    MI_T("CIM_Job"), /* propagator */
    NULL,
};

/* property CIM_Job.LocalOrUtcTime */
static MI_CONST MI_PropertyDecl CIM_Job_LocalOrUtcTime_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x006C650E, /* code */
    MI_T("LocalOrUtcTime"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT16, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Job, LocalOrUtcTime), /* offset */
    MI_T("CIM_Job"), /* origin */
    MI_T("CIM_Job"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_Job_UntilTime_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_Job.LocalOrUtcTime"),
};

static MI_CONST MI_ConstStringA CIM_Job_UntilTime_ModelCorrespondence_qual_value =
{
    CIM_Job_UntilTime_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_Job_UntilTime_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_Job_UntilTime_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_Job_UntilTime_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Job_UntilTime_quals[] =
{
    &CIM_Job_UntilTime_ModelCorrespondence_qual,
};

/* property CIM_Job.UntilTime */
static MI_CONST MI_PropertyDecl CIM_Job_UntilTime_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00756509, /* code */
    MI_T("UntilTime"), /* name */
    CIM_Job_UntilTime_quals, /* qualifiers */
    MI_COUNT(CIM_Job_UntilTime_quals), /* numQualifiers */
    MI_DATETIME, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Job, UntilTime), /* offset */
    MI_T("CIM_Job"), /* origin */
    MI_T("CIM_Job"), /* propagator */
    NULL,
};

/* property CIM_Job.Notify */
static MI_CONST MI_PropertyDecl CIM_Job_Notify_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x006E7906, /* code */
    MI_T("Notify"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Job, Notify), /* offset */
    MI_T("CIM_Job"), /* origin */
    MI_T("CIM_Job"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_Job_Owner_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_OwningJobElement"),
};

static MI_CONST MI_ConstStringA CIM_Job_Owner_ModelCorrespondence_qual_value =
{
    CIM_Job_Owner_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_Job_Owner_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_Job_Owner_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_Job_Owner_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Job_Owner_quals[] =
{
    &CIM_Job_Owner_ModelCorrespondence_qual,
};

/* property CIM_Job.Owner */
static MI_CONST MI_PropertyDecl CIM_Job_Owner_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x006F7205, /* code */
    MI_T("Owner"), /* name */
    CIM_Job_Owner_quals, /* qualifiers */
    MI_COUNT(CIM_Job_Owner_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Job, Owner), /* offset */
    MI_T("CIM_Job"), /* origin */
    MI_T("CIM_Job"), /* propagator */
    NULL,
};

/* property CIM_Job.Priority */
static MI_CONST MI_PropertyDecl CIM_Job_Priority_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00707908, /* code */
    MI_T("Priority"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT32, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Job, Priority), /* offset */
    MI_T("CIM_Job"), /* origin */
    MI_T("CIM_Job"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_Job_PercentComplete_Units_qual_value = MI_T("Percent");

static MI_CONST MI_Qualifier CIM_Job_PercentComplete_Units_qual =
{
    MI_T("Units"),
    MI_STRING,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_TOSUBCLASS|MI_FLAG_TRANSLATABLE,
    &CIM_Job_PercentComplete_Units_qual_value
};

static MI_CONST MI_Sint64 CIM_Job_PercentComplete_MinValue_qual_value = MI_LL(0);

static MI_CONST MI_Qualifier CIM_Job_PercentComplete_MinValue_qual =
{
    MI_T("MinValue"),
    MI_SINT64,
    0,
    &CIM_Job_PercentComplete_MinValue_qual_value
};

static MI_CONST MI_Sint64 CIM_Job_PercentComplete_MaxValue_qual_value = MI_LL(101);

static MI_CONST MI_Qualifier CIM_Job_PercentComplete_MaxValue_qual =
{
    MI_T("MaxValue"),
    MI_SINT64,
    0,
    &CIM_Job_PercentComplete_MaxValue_qual_value
};

static MI_CONST MI_Char* CIM_Job_PercentComplete_PUnit_qual_value = MI_T("percent");

static MI_CONST MI_Qualifier CIM_Job_PercentComplete_PUnit_qual =
{
    MI_T("PUnit"),
    MI_STRING,
    0,
    &CIM_Job_PercentComplete_PUnit_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Job_PercentComplete_quals[] =
{
    &CIM_Job_PercentComplete_Units_qual,
    &CIM_Job_PercentComplete_MinValue_qual,
    &CIM_Job_PercentComplete_MaxValue_qual,
    &CIM_Job_PercentComplete_PUnit_qual,
};

/* property CIM_Job.PercentComplete */
static MI_CONST MI_PropertyDecl CIM_Job_PercentComplete_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0070650F, /* code */
    MI_T("PercentComplete"), /* name */
    CIM_Job_PercentComplete_quals, /* qualifiers */
    MI_COUNT(CIM_Job_PercentComplete_quals), /* numQualifiers */
    MI_UINT16, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Job, PercentComplete), /* offset */
    MI_T("CIM_Job"), /* origin */
    MI_T("CIM_Job"), /* propagator */
    NULL,
};

/* property CIM_Job.DeleteOnCompletion */
static MI_CONST MI_PropertyDecl CIM_Job_DeleteOnCompletion_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00646E12, /* code */
    MI_T("DeleteOnCompletion"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_BOOLEAN, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Job, DeleteOnCompletion), /* offset */
    MI_T("CIM_Job"), /* origin */
    MI_T("CIM_Job"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_Job_ErrorCode_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_Job.ErrorDescription"),
};

static MI_CONST MI_ConstStringA CIM_Job_ErrorCode_ModelCorrespondence_qual_value =
{
    CIM_Job_ErrorCode_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_Job_ErrorCode_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_Job_ErrorCode_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_Job_ErrorCode_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Job_ErrorCode_quals[] =
{
    &CIM_Job_ErrorCode_ModelCorrespondence_qual,
};

/* property CIM_Job.ErrorCode */
static MI_CONST MI_PropertyDecl CIM_Job_ErrorCode_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00656509, /* code */
    MI_T("ErrorCode"), /* name */
    CIM_Job_ErrorCode_quals, /* qualifiers */
    MI_COUNT(CIM_Job_ErrorCode_quals), /* numQualifiers */
    MI_UINT16, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Job, ErrorCode), /* offset */
    MI_T("CIM_Job"), /* origin */
    MI_T("CIM_Job"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_Job_ErrorDescription_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_Job.ErrorCode"),
};

static MI_CONST MI_ConstStringA CIM_Job_ErrorDescription_ModelCorrespondence_qual_value =
{
    CIM_Job_ErrorDescription_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_Job_ErrorDescription_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_Job_ErrorDescription_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_Job_ErrorDescription_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Job_ErrorDescription_quals[] =
{
    &CIM_Job_ErrorDescription_ModelCorrespondence_qual,
};

/* property CIM_Job.ErrorDescription */
static MI_CONST MI_PropertyDecl CIM_Job_ErrorDescription_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00656E10, /* code */
    MI_T("ErrorDescription"), /* name */
    CIM_Job_ErrorDescription_quals, /* qualifiers */
    MI_COUNT(CIM_Job_ErrorDescription_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Job, ErrorDescription), /* offset */
    MI_T("CIM_Job"), /* origin */
    MI_T("CIM_Job"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_Job_RecoveryAction_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_Job.OtherRecoveryAction"),
};

static MI_CONST MI_ConstStringA CIM_Job_RecoveryAction_ModelCorrespondence_qual_value =
{
    CIM_Job_RecoveryAction_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_Job_RecoveryAction_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_Job_RecoveryAction_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_Job_RecoveryAction_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Job_RecoveryAction_quals[] =
{
    &CIM_Job_RecoveryAction_ModelCorrespondence_qual,
};

/* property CIM_Job.RecoveryAction */
static MI_CONST MI_PropertyDecl CIM_Job_RecoveryAction_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00726E0E, /* code */
    MI_T("RecoveryAction"), /* name */
    CIM_Job_RecoveryAction_quals, /* qualifiers */
    MI_COUNT(CIM_Job_RecoveryAction_quals), /* numQualifiers */
    MI_UINT16, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Job, RecoveryAction), /* offset */
    MI_T("CIM_Job"), /* origin */
    MI_T("CIM_Job"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_Job_OtherRecoveryAction_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_Job.RecoveryAction"),
};

static MI_CONST MI_ConstStringA CIM_Job_OtherRecoveryAction_ModelCorrespondence_qual_value =
{
    CIM_Job_OtherRecoveryAction_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_Job_OtherRecoveryAction_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_Job_OtherRecoveryAction_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_Job_OtherRecoveryAction_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Job_OtherRecoveryAction_quals[] =
{
    &CIM_Job_OtherRecoveryAction_ModelCorrespondence_qual,
};

/* property CIM_Job.OtherRecoveryAction */
static MI_CONST MI_PropertyDecl CIM_Job_OtherRecoveryAction_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x006F6E13, /* code */
    MI_T("OtherRecoveryAction"), /* name */
    CIM_Job_OtherRecoveryAction_quals, /* qualifiers */
    MI_COUNT(CIM_Job_OtherRecoveryAction_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Job, OtherRecoveryAction), /* offset */
    MI_T("CIM_Job"), /* origin */
    MI_T("CIM_Job"), /* propagator */
    NULL,
};

static MI_PropertyDecl MI_CONST* MI_CONST CIM_Job_props[] =
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
    &CIM_Job_JobStatus_prop,
    &CIM_Job_TimeSubmitted_prop,
    &CIM_Job_ScheduledStartTime_prop,
    &CIM_Job_StartTime_prop,
    &CIM_Job_ElapsedTime_prop,
    &CIM_Job_JobRunTimes_prop,
    &CIM_Job_RunMonth_prop,
    &CIM_Job_RunDay_prop,
    &CIM_Job_RunDayOfWeek_prop,
    &CIM_Job_RunStartInterval_prop,
    &CIM_Job_LocalOrUtcTime_prop,
    &CIM_Job_UntilTime_prop,
    &CIM_Job_Notify_prop,
    &CIM_Job_Owner_prop,
    &CIM_Job_Priority_prop,
    &CIM_Job_PercentComplete_prop,
    &CIM_Job_DeleteOnCompletion_prop,
    &CIM_Job_ErrorCode_prop,
    &CIM_Job_ErrorDescription_prop,
    &CIM_Job_RecoveryAction_prop,
    &CIM_Job_OtherRecoveryAction_prop,
};

static MI_CONST MI_Char* CIM_Job_KillJob_Deprecated_qual_data_value[] =
{
    MI_T("CIM_ConcreteJob.RequestStateChange()"),
};

static MI_CONST MI_ConstStringA CIM_Job_KillJob_Deprecated_qual_value =
{
    CIM_Job_KillJob_Deprecated_qual_data_value,
    MI_COUNT(CIM_Job_KillJob_Deprecated_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_Job_KillJob_Deprecated_qual =
{
    MI_T("Deprecated"),
    MI_STRINGA,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_RESTRICTED,
    &CIM_Job_KillJob_Deprecated_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Job_KillJob_quals[] =
{
    &CIM_Job_KillJob_Deprecated_qual,
};

/* parameter CIM_Job.KillJob(): DeleteOnKill */
static MI_CONST MI_ParameterDecl CIM_Job_KillJob_DeleteOnKill_param =
{
    MI_FLAG_PARAMETER|MI_FLAG_IN, /* flags */
    0x00646C0C, /* code */
    MI_T("DeleteOnKill"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_BOOLEAN, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Job_KillJob, DeleteOnKill), /* offset */
};

static MI_CONST MI_Char* CIM_Job_KillJob_MIReturn_Deprecated_qual_data_value[] =
{
    MI_T("CIM_ConcreteJob.RequestStateChange()"),
};

static MI_CONST MI_ConstStringA CIM_Job_KillJob_MIReturn_Deprecated_qual_value =
{
    CIM_Job_KillJob_MIReturn_Deprecated_qual_data_value,
    MI_COUNT(CIM_Job_KillJob_MIReturn_Deprecated_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_Job_KillJob_MIReturn_Deprecated_qual =
{
    MI_T("Deprecated"),
    MI_STRINGA,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_RESTRICTED,
    &CIM_Job_KillJob_MIReturn_Deprecated_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Job_KillJob_MIReturn_quals[] =
{
    &CIM_Job_KillJob_MIReturn_Deprecated_qual,
};

/* parameter CIM_Job.KillJob(): MIReturn */
static MI_CONST MI_ParameterDecl CIM_Job_KillJob_MIReturn_param =
{
    MI_FLAG_PARAMETER|MI_FLAG_OUT, /* flags */
    0x006D6E08, /* code */
    MI_T("MIReturn"), /* name */
    CIM_Job_KillJob_MIReturn_quals, /* qualifiers */
    MI_COUNT(CIM_Job_KillJob_MIReturn_quals), /* numQualifiers */
    MI_UINT32, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Job_KillJob, MIReturn), /* offset */
};

static MI_ParameterDecl MI_CONST* MI_CONST CIM_Job_KillJob_params[] =
{
    &CIM_Job_KillJob_MIReturn_param,
    &CIM_Job_KillJob_DeleteOnKill_param,
};

/* method CIM_Job.KillJob() */
MI_CONST MI_MethodDecl CIM_Job_KillJob_rtti =
{
    MI_FLAG_METHOD, /* flags */
    0x006B6207, /* code */
    MI_T("KillJob"), /* name */
    CIM_Job_KillJob_quals, /* qualifiers */
    MI_COUNT(CIM_Job_KillJob_quals), /* numQualifiers */
    CIM_Job_KillJob_params, /* parameters */
    MI_COUNT(CIM_Job_KillJob_params), /* numParameters */
    sizeof(CIM_Job_KillJob), /* size */
    MI_UINT32, /* returnType */
    MI_T("CIM_Job"), /* origin */
    MI_T("CIM_Job"), /* propagator */
    &schemaDecl, /* schema */
    NULL, /* method */
};

static MI_MethodDecl MI_CONST* MI_CONST CIM_Job_meths[] =
{
    &CIM_Job_KillJob_rtti,
};

static MI_CONST MI_Char* CIM_Job_UMLPackagePath_qual_value = MI_T("CIM::Core::CoreElements");

static MI_CONST MI_Qualifier CIM_Job_UMLPackagePath_qual =
{
    MI_T("UMLPackagePath"),
    MI_STRING,
    0,
    &CIM_Job_UMLPackagePath_qual_value
};

static MI_CONST MI_Char* CIM_Job_Version_qual_value = MI_T("2.10.0");

static MI_CONST MI_Qualifier CIM_Job_Version_qual =
{
    MI_T("Version"),
    MI_STRING,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_TRANSLATABLE|MI_FLAG_RESTRICTED,
    &CIM_Job_Version_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Job_quals[] =
{
    &CIM_Job_UMLPackagePath_qual,
    &CIM_Job_Version_qual,
};

/* class CIM_Job */
MI_CONST MI_ClassDecl CIM_Job_rtti =
{
    MI_FLAG_CLASS|MI_FLAG_ABSTRACT, /* flags */
    0x00636207, /* code */
    MI_T("CIM_Job"), /* name */
    CIM_Job_quals, /* qualifiers */
    MI_COUNT(CIM_Job_quals), /* numQualifiers */
    CIM_Job_props, /* properties */
    MI_COUNT(CIM_Job_props), /* numProperties */
    sizeof(CIM_Job), /* size */
    MI_T("CIM_LogicalElement"), /* superClass */
    &CIM_LogicalElement_rtti, /* superClassDecl */
    CIM_Job_meths, /* methods */
    MI_COUNT(CIM_Job_meths), /* numMethods */
    &schemaDecl, /* schema */
    NULL, /* functions */
    NULL, /* owningClass */
};

/*
**==============================================================================
**
** CIM_Error
**
**==============================================================================
*/

static MI_CONST MI_Char* CIM_Error_ErrorType_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_Error.OtherErrorType"),
};

static MI_CONST MI_ConstStringA CIM_Error_ErrorType_ModelCorrespondence_qual_value =
{
    CIM_Error_ErrorType_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_Error_ErrorType_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_Error_ErrorType_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_Error_ErrorType_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Error_ErrorType_quals[] =
{
    &CIM_Error_ErrorType_ModelCorrespondence_qual,
};

/* property CIM_Error.ErrorType */
static MI_CONST MI_PropertyDecl CIM_Error_ErrorType_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00656509, /* code */
    MI_T("ErrorType"), /* name */
    CIM_Error_ErrorType_quals, /* qualifiers */
    MI_COUNT(CIM_Error_ErrorType_quals), /* numQualifiers */
    MI_UINT16, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Error, ErrorType), /* offset */
    MI_T("CIM_Error"), /* origin */
    MI_T("CIM_Error"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_Error_OtherErrorType_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_Error.ErrorType"),
};

static MI_CONST MI_ConstStringA CIM_Error_OtherErrorType_ModelCorrespondence_qual_value =
{
    CIM_Error_OtherErrorType_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_Error_OtherErrorType_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_Error_OtherErrorType_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_Error_OtherErrorType_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Error_OtherErrorType_quals[] =
{
    &CIM_Error_OtherErrorType_ModelCorrespondence_qual,
};

/* property CIM_Error.OtherErrorType */
static MI_CONST MI_PropertyDecl CIM_Error_OtherErrorType_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x006F650E, /* code */
    MI_T("OtherErrorType"), /* name */
    CIM_Error_OtherErrorType_quals, /* qualifiers */
    MI_COUNT(CIM_Error_OtherErrorType_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Error, OtherErrorType), /* offset */
    MI_T("CIM_Error"), /* origin */
    MI_T("CIM_Error"), /* propagator */
    NULL,
};

/* property CIM_Error.OwningEntity */
static MI_CONST MI_PropertyDecl CIM_Error_OwningEntity_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x006F790C, /* code */
    MI_T("OwningEntity"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Error, OwningEntity), /* offset */
    MI_T("CIM_Error"), /* origin */
    MI_T("CIM_Error"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_Error_MessageID_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_Error.Message"),
    MI_T("CIM_Error.MessageArguments"),
};

static MI_CONST MI_ConstStringA CIM_Error_MessageID_ModelCorrespondence_qual_value =
{
    CIM_Error_MessageID_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_Error_MessageID_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_Error_MessageID_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_Error_MessageID_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Error_MessageID_quals[] =
{
    &CIM_Error_MessageID_ModelCorrespondence_qual,
};

/* property CIM_Error.MessageID */
static MI_CONST MI_PropertyDecl CIM_Error_MessageID_prop =
{
    MI_FLAG_PROPERTY|MI_FLAG_REQUIRED, /* flags */
    0x006D6409, /* code */
    MI_T("MessageID"), /* name */
    CIM_Error_MessageID_quals, /* qualifiers */
    MI_COUNT(CIM_Error_MessageID_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Error, MessageID), /* offset */
    MI_T("CIM_Error"), /* origin */
    MI_T("CIM_Error"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_Error_Message_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_Error.MessageID"),
    MI_T("CIM_Error.MessageArguments"),
};

static MI_CONST MI_ConstStringA CIM_Error_Message_ModelCorrespondence_qual_value =
{
    CIM_Error_Message_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_Error_Message_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_Error_Message_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_Error_Message_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Error_Message_quals[] =
{
    &CIM_Error_Message_ModelCorrespondence_qual,
};

/* property CIM_Error.Message */
static MI_CONST MI_PropertyDecl CIM_Error_Message_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x006D6507, /* code */
    MI_T("Message"), /* name */
    CIM_Error_Message_quals, /* qualifiers */
    MI_COUNT(CIM_Error_Message_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Error, Message), /* offset */
    MI_T("CIM_Error"), /* origin */
    MI_T("CIM_Error"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_Error_MessageArguments_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_Error.MessageID"),
    MI_T("CIM_Error.Message"),
};

static MI_CONST MI_ConstStringA CIM_Error_MessageArguments_ModelCorrespondence_qual_value =
{
    CIM_Error_MessageArguments_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_Error_MessageArguments_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_Error_MessageArguments_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_Error_MessageArguments_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Error_MessageArguments_quals[] =
{
    &CIM_Error_MessageArguments_ModelCorrespondence_qual,
};

/* property CIM_Error.MessageArguments */
static MI_CONST MI_PropertyDecl CIM_Error_MessageArguments_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x006D7310, /* code */
    MI_T("MessageArguments"), /* name */
    CIM_Error_MessageArguments_quals, /* qualifiers */
    MI_COUNT(CIM_Error_MessageArguments_quals), /* numQualifiers */
    MI_STRINGA, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Error, MessageArguments), /* offset */
    MI_T("CIM_Error"), /* origin */
    MI_T("CIM_Error"), /* propagator */
    NULL,
};

/* property CIM_Error.PerceivedSeverity */
static MI_CONST MI_PropertyDecl CIM_Error_PerceivedSeverity_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00707911, /* code */
    MI_T("PerceivedSeverity"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT16, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Error, PerceivedSeverity), /* offset */
    MI_T("CIM_Error"), /* origin */
    MI_T("CIM_Error"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_Error_ProbableCause_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_Error.ProbableCauseDescription"),
};

static MI_CONST MI_ConstStringA CIM_Error_ProbableCause_ModelCorrespondence_qual_value =
{
    CIM_Error_ProbableCause_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_Error_ProbableCause_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_Error_ProbableCause_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_Error_ProbableCause_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Error_ProbableCause_quals[] =
{
    &CIM_Error_ProbableCause_ModelCorrespondence_qual,
};

/* property CIM_Error.ProbableCause */
static MI_CONST MI_PropertyDecl CIM_Error_ProbableCause_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0070650D, /* code */
    MI_T("ProbableCause"), /* name */
    CIM_Error_ProbableCause_quals, /* qualifiers */
    MI_COUNT(CIM_Error_ProbableCause_quals), /* numQualifiers */
    MI_UINT16, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Error, ProbableCause), /* offset */
    MI_T("CIM_Error"), /* origin */
    MI_T("CIM_Error"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_Error_ProbableCauseDescription_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_Error.ProbableCause"),
};

static MI_CONST MI_ConstStringA CIM_Error_ProbableCauseDescription_ModelCorrespondence_qual_value =
{
    CIM_Error_ProbableCauseDescription_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_Error_ProbableCauseDescription_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_Error_ProbableCauseDescription_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_Error_ProbableCauseDescription_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Error_ProbableCauseDescription_quals[] =
{
    &CIM_Error_ProbableCauseDescription_ModelCorrespondence_qual,
};

/* property CIM_Error.ProbableCauseDescription */
static MI_CONST MI_PropertyDecl CIM_Error_ProbableCauseDescription_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00706E18, /* code */
    MI_T("ProbableCauseDescription"), /* name */
    CIM_Error_ProbableCauseDescription_quals, /* qualifiers */
    MI_COUNT(CIM_Error_ProbableCauseDescription_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Error, ProbableCauseDescription), /* offset */
    MI_T("CIM_Error"), /* origin */
    MI_T("CIM_Error"), /* propagator */
    NULL,
};

/* property CIM_Error.RecommendedActions */
static MI_CONST MI_PropertyDecl CIM_Error_RecommendedActions_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00727312, /* code */
    MI_T("RecommendedActions"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_STRINGA, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Error, RecommendedActions), /* offset */
    MI_T("CIM_Error"), /* origin */
    MI_T("CIM_Error"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_Error_ErrorSource_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_Error.ErrorSourceFormat"),
};

static MI_CONST MI_ConstStringA CIM_Error_ErrorSource_ModelCorrespondence_qual_value =
{
    CIM_Error_ErrorSource_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_Error_ErrorSource_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_Error_ErrorSource_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_Error_ErrorSource_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Error_ErrorSource_quals[] =
{
    &CIM_Error_ErrorSource_ModelCorrespondence_qual,
};

/* property CIM_Error.ErrorSource */
static MI_CONST MI_PropertyDecl CIM_Error_ErrorSource_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0065650B, /* code */
    MI_T("ErrorSource"), /* name */
    CIM_Error_ErrorSource_quals, /* qualifiers */
    MI_COUNT(CIM_Error_ErrorSource_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Error, ErrorSource), /* offset */
    MI_T("CIM_Error"), /* origin */
    MI_T("CIM_Error"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_Error_ErrorSourceFormat_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_Error.ErrorSource"),
    MI_T("CIM_Error.OtherErrorSourceFormat"),
};

static MI_CONST MI_ConstStringA CIM_Error_ErrorSourceFormat_ModelCorrespondence_qual_value =
{
    CIM_Error_ErrorSourceFormat_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_Error_ErrorSourceFormat_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_Error_ErrorSourceFormat_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_Error_ErrorSourceFormat_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Error_ErrorSourceFormat_quals[] =
{
    &CIM_Error_ErrorSourceFormat_ModelCorrespondence_qual,
};

static MI_CONST MI_Uint16 CIM_Error_ErrorSourceFormat_value = 0;

/* property CIM_Error.ErrorSourceFormat */
static MI_CONST MI_PropertyDecl CIM_Error_ErrorSourceFormat_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00657411, /* code */
    MI_T("ErrorSourceFormat"), /* name */
    CIM_Error_ErrorSourceFormat_quals, /* qualifiers */
    MI_COUNT(CIM_Error_ErrorSourceFormat_quals), /* numQualifiers */
    MI_UINT16, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Error, ErrorSourceFormat), /* offset */
    MI_T("CIM_Error"), /* origin */
    MI_T("CIM_Error"), /* propagator */
    &CIM_Error_ErrorSourceFormat_value,
};

static MI_CONST MI_Char* CIM_Error_OtherErrorSourceFormat_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_Error.ErrorSourceFormat"),
};

static MI_CONST MI_ConstStringA CIM_Error_OtherErrorSourceFormat_ModelCorrespondence_qual_value =
{
    CIM_Error_OtherErrorSourceFormat_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_Error_OtherErrorSourceFormat_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_Error_OtherErrorSourceFormat_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_Error_OtherErrorSourceFormat_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Error_OtherErrorSourceFormat_quals[] =
{
    &CIM_Error_OtherErrorSourceFormat_ModelCorrespondence_qual,
};

/* property CIM_Error.OtherErrorSourceFormat */
static MI_CONST MI_PropertyDecl CIM_Error_OtherErrorSourceFormat_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x006F7416, /* code */
    MI_T("OtherErrorSourceFormat"), /* name */
    CIM_Error_OtherErrorSourceFormat_quals, /* qualifiers */
    MI_COUNT(CIM_Error_OtherErrorSourceFormat_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Error, OtherErrorSourceFormat), /* offset */
    MI_T("CIM_Error"), /* origin */
    MI_T("CIM_Error"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_Error_CIMStatusCode_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_Error.CIMStatusCodeDescription"),
};

static MI_CONST MI_ConstStringA CIM_Error_CIMStatusCode_ModelCorrespondence_qual_value =
{
    CIM_Error_CIMStatusCode_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_Error_CIMStatusCode_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_Error_CIMStatusCode_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_Error_CIMStatusCode_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Error_CIMStatusCode_quals[] =
{
    &CIM_Error_CIMStatusCode_ModelCorrespondence_qual,
};

/* property CIM_Error.CIMStatusCode */
static MI_CONST MI_PropertyDecl CIM_Error_CIMStatusCode_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0063650D, /* code */
    MI_T("CIMStatusCode"), /* name */
    CIM_Error_CIMStatusCode_quals, /* qualifiers */
    MI_COUNT(CIM_Error_CIMStatusCode_quals), /* numQualifiers */
    MI_UINT32, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Error, CIMStatusCode), /* offset */
    MI_T("CIM_Error"), /* origin */
    MI_T("CIM_Error"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_Error_CIMStatusCodeDescription_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_Error.CIMStatusCode"),
};

static MI_CONST MI_ConstStringA CIM_Error_CIMStatusCodeDescription_ModelCorrespondence_qual_value =
{
    CIM_Error_CIMStatusCodeDescription_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_Error_CIMStatusCodeDescription_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_Error_CIMStatusCodeDescription_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_Error_CIMStatusCodeDescription_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Error_CIMStatusCodeDescription_quals[] =
{
    &CIM_Error_CIMStatusCodeDescription_ModelCorrespondence_qual,
};

/* property CIM_Error.CIMStatusCodeDescription */
static MI_CONST MI_PropertyDecl CIM_Error_CIMStatusCodeDescription_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00636E18, /* code */
    MI_T("CIMStatusCodeDescription"), /* name */
    CIM_Error_CIMStatusCodeDescription_quals, /* qualifiers */
    MI_COUNT(CIM_Error_CIMStatusCodeDescription_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Error, CIMStatusCodeDescription), /* offset */
    MI_T("CIM_Error"), /* origin */
    MI_T("CIM_Error"), /* propagator */
    NULL,
};

static MI_PropertyDecl MI_CONST* MI_CONST CIM_Error_props[] =
{
    &CIM_Error_ErrorType_prop,
    &CIM_Error_OtherErrorType_prop,
    &CIM_Error_OwningEntity_prop,
    &CIM_Error_MessageID_prop,
    &CIM_Error_Message_prop,
    &CIM_Error_MessageArguments_prop,
    &CIM_Error_PerceivedSeverity_prop,
    &CIM_Error_ProbableCause_prop,
    &CIM_Error_ProbableCauseDescription_prop,
    &CIM_Error_RecommendedActions_prop,
    &CIM_Error_ErrorSource_prop,
    &CIM_Error_ErrorSourceFormat_prop,
    &CIM_Error_OtherErrorSourceFormat_prop,
    &CIM_Error_CIMStatusCode_prop,
    &CIM_Error_CIMStatusCodeDescription_prop,
};

static MI_CONST MI_Char* CIM_Error_Version_qual_value = MI_T("2.22.1");

static MI_CONST MI_Qualifier CIM_Error_Version_qual =
{
    MI_T("Version"),
    MI_STRING,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_TRANSLATABLE|MI_FLAG_RESTRICTED,
    &CIM_Error_Version_qual_value
};

static MI_CONST MI_Char* CIM_Error_UMLPackagePath_qual_value = MI_T("CIM::Interop");

static MI_CONST MI_Qualifier CIM_Error_UMLPackagePath_qual =
{
    MI_T("UMLPackagePath"),
    MI_STRING,
    0,
    &CIM_Error_UMLPackagePath_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Error_quals[] =
{
    &CIM_Error_Version_qual,
    &CIM_Error_UMLPackagePath_qual,
};

/* class CIM_Error */
MI_CONST MI_ClassDecl CIM_Error_rtti =
{
    MI_FLAG_CLASS|MI_FLAG_INDICATION, /* flags */
    0x00637209, /* code */
    MI_T("CIM_Error"), /* name */
    CIM_Error_quals, /* qualifiers */
    MI_COUNT(CIM_Error_quals), /* numQualifiers */
    CIM_Error_props, /* properties */
    MI_COUNT(CIM_Error_props), /* numProperties */
    sizeof(CIM_Error), /* size */
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
** CIM_ConcreteJob
**
**==============================================================================
*/

static MI_CONST MI_Char* CIM_ConcreteJob_InstanceID_Override_qual_value = MI_T("InstanceID");

static MI_CONST MI_Qualifier CIM_ConcreteJob_InstanceID_Override_qual =
{
    MI_T("Override"),
    MI_STRING,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_RESTRICTED,
    &CIM_ConcreteJob_InstanceID_Override_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_ConcreteJob_InstanceID_quals[] =
{
    &CIM_ConcreteJob_InstanceID_Override_qual,
};

/* property CIM_ConcreteJob.InstanceID */
static MI_CONST MI_PropertyDecl CIM_ConcreteJob_InstanceID_prop =
{
    MI_FLAG_PROPERTY|MI_FLAG_KEY, /* flags */
    0x0069640A, /* code */
    MI_T("InstanceID"), /* name */
    CIM_ConcreteJob_InstanceID_quals, /* qualifiers */
    MI_COUNT(CIM_ConcreteJob_InstanceID_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_ConcreteJob, InstanceID), /* offset */
    MI_T("CIM_ManagedElement"), /* origin */
    MI_T("CIM_ConcreteJob"), /* propagator */
    NULL,
};

static MI_CONST MI_Uint32 CIM_ConcreteJob_Name_MaxLen_qual_value = 1024U;

static MI_CONST MI_Qualifier CIM_ConcreteJob_Name_MaxLen_qual =
{
    MI_T("MaxLen"),
    MI_UINT32,
    0,
    &CIM_ConcreteJob_Name_MaxLen_qual_value
};

static MI_CONST MI_Char* CIM_ConcreteJob_Name_Override_qual_value = MI_T("Name");

static MI_CONST MI_Qualifier CIM_ConcreteJob_Name_Override_qual =
{
    MI_T("Override"),
    MI_STRING,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_RESTRICTED,
    &CIM_ConcreteJob_Name_Override_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_ConcreteJob_Name_quals[] =
{
    &CIM_ConcreteJob_Name_MaxLen_qual,
    &CIM_ConcreteJob_Name_Override_qual,
};

/* property CIM_ConcreteJob.Name */
static MI_CONST MI_PropertyDecl CIM_ConcreteJob_Name_prop =
{
    MI_FLAG_PROPERTY|MI_FLAG_REQUIRED, /* flags */
    0x006E6504, /* code */
    MI_T("Name"), /* name */
    CIM_ConcreteJob_Name_quals, /* qualifiers */
    MI_COUNT(CIM_ConcreteJob_Name_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_ConcreteJob, Name), /* offset */
    MI_T("CIM_ManagedSystemElement"), /* origin */
    MI_T("CIM_ConcreteJob"), /* propagator */
    NULL,
};

/* property CIM_ConcreteJob.JobState */
static MI_CONST MI_PropertyDecl CIM_ConcreteJob_JobState_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x006A6508, /* code */
    MI_T("JobState"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT16, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_ConcreteJob, JobState), /* offset */
    MI_T("CIM_ConcreteJob"), /* origin */
    MI_T("CIM_ConcreteJob"), /* propagator */
    NULL,
};

/* property CIM_ConcreteJob.TimeOfLastStateChange */
static MI_CONST MI_PropertyDecl CIM_ConcreteJob_TimeOfLastStateChange_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00746515, /* code */
    MI_T("TimeOfLastStateChange"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_DATETIME, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_ConcreteJob, TimeOfLastStateChange), /* offset */
    MI_T("CIM_ConcreteJob"), /* origin */
    MI_T("CIM_ConcreteJob"), /* propagator */
    NULL,
};

static MI_CONST MI_Datetime CIM_ConcreteJob_TimeBeforeRemoval_value = {0,{{0,0,5,0,0}}};

/* property CIM_ConcreteJob.TimeBeforeRemoval */
static MI_CONST MI_PropertyDecl CIM_ConcreteJob_TimeBeforeRemoval_prop =
{
    MI_FLAG_PROPERTY|MI_FLAG_REQUIRED, /* flags */
    0x00746C11, /* code */
    MI_T("TimeBeforeRemoval"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_DATETIME, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_ConcreteJob, TimeBeforeRemoval), /* offset */
    MI_T("CIM_ConcreteJob"), /* origin */
    MI_T("CIM_ConcreteJob"), /* propagator */
    &CIM_ConcreteJob_TimeBeforeRemoval_value,
};

static MI_PropertyDecl MI_CONST* MI_CONST CIM_ConcreteJob_props[] =
{
    &CIM_ConcreteJob_InstanceID_prop,
    &CIM_ManagedElement_Caption_prop,
    &CIM_ManagedElement_Description_prop,
    &CIM_ManagedElement_ElementName_prop,
    &CIM_ManagedSystemElement_InstallDate_prop,
    &CIM_ConcreteJob_Name_prop,
    &CIM_ManagedSystemElement_OperationalStatus_prop,
    &CIM_ManagedSystemElement_StatusDescriptions_prop,
    &CIM_ManagedSystemElement_Status_prop,
    &CIM_ManagedSystemElement_HealthState_prop,
    &CIM_ManagedSystemElement_CommunicationStatus_prop,
    &CIM_ManagedSystemElement_DetailedStatus_prop,
    &CIM_ManagedSystemElement_OperatingStatus_prop,
    &CIM_ManagedSystemElement_PrimaryStatus_prop,
    &CIM_Job_JobStatus_prop,
    &CIM_Job_TimeSubmitted_prop,
    &CIM_Job_ScheduledStartTime_prop,
    &CIM_Job_StartTime_prop,
    &CIM_Job_ElapsedTime_prop,
    &CIM_Job_JobRunTimes_prop,
    &CIM_Job_RunMonth_prop,
    &CIM_Job_RunDay_prop,
    &CIM_Job_RunDayOfWeek_prop,
    &CIM_Job_RunStartInterval_prop,
    &CIM_Job_LocalOrUtcTime_prop,
    &CIM_Job_UntilTime_prop,
    &CIM_Job_Notify_prop,
    &CIM_Job_Owner_prop,
    &CIM_Job_Priority_prop,
    &CIM_Job_PercentComplete_prop,
    &CIM_Job_DeleteOnCompletion_prop,
    &CIM_Job_ErrorCode_prop,
    &CIM_Job_ErrorDescription_prop,
    &CIM_Job_RecoveryAction_prop,
    &CIM_Job_OtherRecoveryAction_prop,
    &CIM_ConcreteJob_JobState_prop,
    &CIM_ConcreteJob_TimeOfLastStateChange_prop,
    &CIM_ConcreteJob_TimeBeforeRemoval_prop,
};

/* parameter CIM_ConcreteJob.RequestStateChange(): RequestedState */
static MI_CONST MI_ParameterDecl CIM_ConcreteJob_RequestStateChange_RequestedState_param =
{
    MI_FLAG_PARAMETER|MI_FLAG_IN, /* flags */
    0x0072650E, /* code */
    MI_T("RequestedState"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT16, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_ConcreteJob_RequestStateChange, RequestedState), /* offset */
};

/* parameter CIM_ConcreteJob.RequestStateChange(): TimeoutPeriod */
static MI_CONST MI_ParameterDecl CIM_ConcreteJob_RequestStateChange_TimeoutPeriod_param =
{
    MI_FLAG_PARAMETER|MI_FLAG_IN, /* flags */
    0x0074640D, /* code */
    MI_T("TimeoutPeriod"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_DATETIME, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_ConcreteJob_RequestStateChange, TimeoutPeriod), /* offset */
};

/* parameter CIM_ConcreteJob.RequestStateChange(): MIReturn */
static MI_CONST MI_ParameterDecl CIM_ConcreteJob_RequestStateChange_MIReturn_param =
{
    MI_FLAG_PARAMETER|MI_FLAG_OUT, /* flags */
    0x006D6E08, /* code */
    MI_T("MIReturn"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT32, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_ConcreteJob_RequestStateChange, MIReturn), /* offset */
};

static MI_ParameterDecl MI_CONST* MI_CONST CIM_ConcreteJob_RequestStateChange_params[] =
{
    &CIM_ConcreteJob_RequestStateChange_MIReturn_param,
    &CIM_ConcreteJob_RequestStateChange_RequestedState_param,
    &CIM_ConcreteJob_RequestStateChange_TimeoutPeriod_param,
};

/* method CIM_ConcreteJob.RequestStateChange() */
MI_CONST MI_MethodDecl CIM_ConcreteJob_RequestStateChange_rtti =
{
    MI_FLAG_METHOD, /* flags */
    0x00726512, /* code */
    MI_T("RequestStateChange"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    CIM_ConcreteJob_RequestStateChange_params, /* parameters */
    MI_COUNT(CIM_ConcreteJob_RequestStateChange_params), /* numParameters */
    sizeof(CIM_ConcreteJob_RequestStateChange), /* size */
    MI_UINT32, /* returnType */
    MI_T("CIM_ConcreteJob"), /* origin */
    MI_T("CIM_ConcreteJob"), /* propagator */
    &schemaDecl, /* schema */
    NULL, /* method */
};

static MI_CONST MI_Char* CIM_ConcreteJob_GetError_Deprecated_qual_data_value[] =
{
    MI_T("CIM_ConcreteJob.GetErrors"),
};

static MI_CONST MI_ConstStringA CIM_ConcreteJob_GetError_Deprecated_qual_value =
{
    CIM_ConcreteJob_GetError_Deprecated_qual_data_value,
    MI_COUNT(CIM_ConcreteJob_GetError_Deprecated_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_ConcreteJob_GetError_Deprecated_qual =
{
    MI_T("Deprecated"),
    MI_STRINGA,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_RESTRICTED,
    &CIM_ConcreteJob_GetError_Deprecated_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_ConcreteJob_GetError_quals[] =
{
    &CIM_ConcreteJob_GetError_Deprecated_qual,
};

static MI_CONST MI_Char* CIM_ConcreteJob_GetError_Error_EmbeddedInstance_qual_value = MI_T("CIM_Error");

static MI_CONST MI_Qualifier CIM_ConcreteJob_GetError_Error_EmbeddedInstance_qual =
{
    MI_T("EmbeddedInstance"),
    MI_STRING,
    0,
    &CIM_ConcreteJob_GetError_Error_EmbeddedInstance_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_ConcreteJob_GetError_Error_quals[] =
{
    &CIM_ConcreteJob_GetError_Error_EmbeddedInstance_qual,
};

/* parameter CIM_ConcreteJob.GetError(): Error */
static MI_CONST MI_ParameterDecl CIM_ConcreteJob_GetError_Error_param =
{
    MI_FLAG_PARAMETER|MI_FLAG_OUT, /* flags */
    0x00657205, /* code */
    MI_T("Error"), /* name */
    CIM_ConcreteJob_GetError_Error_quals, /* qualifiers */
    MI_COUNT(CIM_ConcreteJob_GetError_Error_quals), /* numQualifiers */
    MI_INSTANCE, /* type */
    MI_T("CIM_Error"), /* className */
    0, /* subscript */
    offsetof(CIM_ConcreteJob_GetError, Error), /* offset */
};

static MI_CONST MI_Char* CIM_ConcreteJob_GetError_MIReturn_Deprecated_qual_data_value[] =
{
    MI_T("CIM_ConcreteJob.GetErrors"),
};

static MI_CONST MI_ConstStringA CIM_ConcreteJob_GetError_MIReturn_Deprecated_qual_value =
{
    CIM_ConcreteJob_GetError_MIReturn_Deprecated_qual_data_value,
    MI_COUNT(CIM_ConcreteJob_GetError_MIReturn_Deprecated_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_ConcreteJob_GetError_MIReturn_Deprecated_qual =
{
    MI_T("Deprecated"),
    MI_STRINGA,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_RESTRICTED,
    &CIM_ConcreteJob_GetError_MIReturn_Deprecated_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_ConcreteJob_GetError_MIReturn_quals[] =
{
    &CIM_ConcreteJob_GetError_MIReturn_Deprecated_qual,
};

/* parameter CIM_ConcreteJob.GetError(): MIReturn */
static MI_CONST MI_ParameterDecl CIM_ConcreteJob_GetError_MIReturn_param =
{
    MI_FLAG_PARAMETER|MI_FLAG_OUT, /* flags */
    0x006D6E08, /* code */
    MI_T("MIReturn"), /* name */
    CIM_ConcreteJob_GetError_MIReturn_quals, /* qualifiers */
    MI_COUNT(CIM_ConcreteJob_GetError_MIReturn_quals), /* numQualifiers */
    MI_UINT32, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_ConcreteJob_GetError, MIReturn), /* offset */
};

static MI_ParameterDecl MI_CONST* MI_CONST CIM_ConcreteJob_GetError_params[] =
{
    &CIM_ConcreteJob_GetError_MIReturn_param,
    &CIM_ConcreteJob_GetError_Error_param,
};

/* method CIM_ConcreteJob.GetError() */
MI_CONST MI_MethodDecl CIM_ConcreteJob_GetError_rtti =
{
    MI_FLAG_METHOD, /* flags */
    0x00677208, /* code */
    MI_T("GetError"), /* name */
    CIM_ConcreteJob_GetError_quals, /* qualifiers */
    MI_COUNT(CIM_ConcreteJob_GetError_quals), /* numQualifiers */
    CIM_ConcreteJob_GetError_params, /* parameters */
    MI_COUNT(CIM_ConcreteJob_GetError_params), /* numParameters */
    sizeof(CIM_ConcreteJob_GetError), /* size */
    MI_UINT32, /* returnType */
    MI_T("CIM_ConcreteJob"), /* origin */
    MI_T("CIM_ConcreteJob"), /* propagator */
    &schemaDecl, /* schema */
    NULL, /* method */
};

static MI_CONST MI_Char* CIM_ConcreteJob_GetErrors_Errors_EmbeddedInstance_qual_value = MI_T("CIM_Error");

static MI_CONST MI_Qualifier CIM_ConcreteJob_GetErrors_Errors_EmbeddedInstance_qual =
{
    MI_T("EmbeddedInstance"),
    MI_STRING,
    0,
    &CIM_ConcreteJob_GetErrors_Errors_EmbeddedInstance_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_ConcreteJob_GetErrors_Errors_quals[] =
{
    &CIM_ConcreteJob_GetErrors_Errors_EmbeddedInstance_qual,
};

/* parameter CIM_ConcreteJob.GetErrors(): Errors */
static MI_CONST MI_ParameterDecl CIM_ConcreteJob_GetErrors_Errors_param =
{
    MI_FLAG_PARAMETER|MI_FLAG_OUT, /* flags */
    0x00657306, /* code */
    MI_T("Errors"), /* name */
    CIM_ConcreteJob_GetErrors_Errors_quals, /* qualifiers */
    MI_COUNT(CIM_ConcreteJob_GetErrors_Errors_quals), /* numQualifiers */
    MI_INSTANCEA, /* type */
    MI_T("CIM_Error"), /* className */
    0, /* subscript */
    offsetof(CIM_ConcreteJob_GetErrors, Errors), /* offset */
};

/* parameter CIM_ConcreteJob.GetErrors(): MIReturn */
static MI_CONST MI_ParameterDecl CIM_ConcreteJob_GetErrors_MIReturn_param =
{
    MI_FLAG_PARAMETER|MI_FLAG_OUT, /* flags */
    0x006D6E08, /* code */
    MI_T("MIReturn"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT32, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_ConcreteJob_GetErrors, MIReturn), /* offset */
};

static MI_ParameterDecl MI_CONST* MI_CONST CIM_ConcreteJob_GetErrors_params[] =
{
    &CIM_ConcreteJob_GetErrors_MIReturn_param,
    &CIM_ConcreteJob_GetErrors_Errors_param,
};

/* method CIM_ConcreteJob.GetErrors() */
MI_CONST MI_MethodDecl CIM_ConcreteJob_GetErrors_rtti =
{
    MI_FLAG_METHOD, /* flags */
    0x00677309, /* code */
    MI_T("GetErrors"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    CIM_ConcreteJob_GetErrors_params, /* parameters */
    MI_COUNT(CIM_ConcreteJob_GetErrors_params), /* numParameters */
    sizeof(CIM_ConcreteJob_GetErrors), /* size */
    MI_UINT32, /* returnType */
    MI_T("CIM_ConcreteJob"), /* origin */
    MI_T("CIM_ConcreteJob"), /* propagator */
    &schemaDecl, /* schema */
    NULL, /* method */
};

static MI_MethodDecl MI_CONST* MI_CONST CIM_ConcreteJob_meths[] =
{
    &CIM_Job_KillJob_rtti,
    &CIM_ConcreteJob_RequestStateChange_rtti,
    &CIM_ConcreteJob_GetError_rtti,
    &CIM_ConcreteJob_GetErrors_rtti,
};

static MI_CONST MI_Char* CIM_ConcreteJob_UMLPackagePath_qual_value = MI_T("CIM::Core::CoreElements");

static MI_CONST MI_Qualifier CIM_ConcreteJob_UMLPackagePath_qual =
{
    MI_T("UMLPackagePath"),
    MI_STRING,
    0,
    &CIM_ConcreteJob_UMLPackagePath_qual_value
};

static MI_CONST MI_Char* CIM_ConcreteJob_Deprecated_qual_data_value[] =
{
    MI_T("CIM_ConcreteJob.GetErrors"),
};

static MI_CONST MI_ConstStringA CIM_ConcreteJob_Deprecated_qual_value =
{
    CIM_ConcreteJob_Deprecated_qual_data_value,
    MI_COUNT(CIM_ConcreteJob_Deprecated_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_ConcreteJob_Deprecated_qual =
{
    MI_T("Deprecated"),
    MI_STRINGA,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_RESTRICTED,
    &CIM_ConcreteJob_Deprecated_qual_value
};

static MI_CONST MI_Char* CIM_ConcreteJob_Version_qual_value = MI_T("2.31.1");

static MI_CONST MI_Qualifier CIM_ConcreteJob_Version_qual =
{
    MI_T("Version"),
    MI_STRING,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_TRANSLATABLE|MI_FLAG_RESTRICTED,
    &CIM_ConcreteJob_Version_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_ConcreteJob_quals[] =
{
    &CIM_ConcreteJob_UMLPackagePath_qual,
    &CIM_ConcreteJob_Deprecated_qual,
    &CIM_ConcreteJob_Version_qual,
};

/* class CIM_ConcreteJob */
MI_CONST MI_ClassDecl CIM_ConcreteJob_rtti =
{
    MI_FLAG_CLASS, /* flags */
    0x0063620F, /* code */
    MI_T("CIM_ConcreteJob"), /* name */
    CIM_ConcreteJob_quals, /* qualifiers */
    MI_COUNT(CIM_ConcreteJob_quals), /* numQualifiers */
    CIM_ConcreteJob_props, /* properties */
    MI_COUNT(CIM_ConcreteJob_props), /* numProperties */
    sizeof(CIM_ConcreteJob), /* size */
    MI_T("CIM_Job"), /* superClass */
    &CIM_Job_rtti, /* superClassDecl */
    CIM_ConcreteJob_meths, /* methods */
    MI_COUNT(CIM_ConcreteJob_meths), /* numMethods */
    &schemaDecl, /* schema */
    NULL, /* functions */
    NULL, /* owningClass */
};

/*
**==============================================================================
**
** CIM_EnabledLogicalElement
**
**==============================================================================
*/

static MI_CONST MI_Char* CIM_EnabledLogicalElement_EnabledState_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_EnabledLogicalElement.OtherEnabledState"),
};

static MI_CONST MI_ConstStringA CIM_EnabledLogicalElement_EnabledState_ModelCorrespondence_qual_value =
{
    CIM_EnabledLogicalElement_EnabledState_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_EnabledLogicalElement_EnabledState_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_EnabledLogicalElement_EnabledState_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_EnabledLogicalElement_EnabledState_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_EnabledLogicalElement_EnabledState_quals[] =
{
    &CIM_EnabledLogicalElement_EnabledState_ModelCorrespondence_qual,
};

static MI_CONST MI_Uint16 CIM_EnabledLogicalElement_EnabledState_value = 5;

/* property CIM_EnabledLogicalElement.EnabledState */
static MI_CONST MI_PropertyDecl CIM_EnabledLogicalElement_EnabledState_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0065650C, /* code */
    MI_T("EnabledState"), /* name */
    CIM_EnabledLogicalElement_EnabledState_quals, /* qualifiers */
    MI_COUNT(CIM_EnabledLogicalElement_EnabledState_quals), /* numQualifiers */
    MI_UINT16, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_EnabledLogicalElement, EnabledState), /* offset */
    MI_T("CIM_EnabledLogicalElement"), /* origin */
    MI_T("CIM_EnabledLogicalElement"), /* propagator */
    &CIM_EnabledLogicalElement_EnabledState_value,
};

static MI_CONST MI_Char* CIM_EnabledLogicalElement_OtherEnabledState_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_EnabledLogicalElement.EnabledState"),
};

static MI_CONST MI_ConstStringA CIM_EnabledLogicalElement_OtherEnabledState_ModelCorrespondence_qual_value =
{
    CIM_EnabledLogicalElement_OtherEnabledState_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_EnabledLogicalElement_OtherEnabledState_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_EnabledLogicalElement_OtherEnabledState_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_EnabledLogicalElement_OtherEnabledState_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_EnabledLogicalElement_OtherEnabledState_quals[] =
{
    &CIM_EnabledLogicalElement_OtherEnabledState_ModelCorrespondence_qual,
};

/* property CIM_EnabledLogicalElement.OtherEnabledState */
static MI_CONST MI_PropertyDecl CIM_EnabledLogicalElement_OtherEnabledState_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x006F6511, /* code */
    MI_T("OtherEnabledState"), /* name */
    CIM_EnabledLogicalElement_OtherEnabledState_quals, /* qualifiers */
    MI_COUNT(CIM_EnabledLogicalElement_OtherEnabledState_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_EnabledLogicalElement, OtherEnabledState), /* offset */
    MI_T("CIM_EnabledLogicalElement"), /* origin */
    MI_T("CIM_EnabledLogicalElement"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_EnabledLogicalElement_RequestedState_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_EnabledLogicalElement.EnabledState"),
};

static MI_CONST MI_ConstStringA CIM_EnabledLogicalElement_RequestedState_ModelCorrespondence_qual_value =
{
    CIM_EnabledLogicalElement_RequestedState_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_EnabledLogicalElement_RequestedState_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_EnabledLogicalElement_RequestedState_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_EnabledLogicalElement_RequestedState_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_EnabledLogicalElement_RequestedState_quals[] =
{
    &CIM_EnabledLogicalElement_RequestedState_ModelCorrespondence_qual,
};

static MI_CONST MI_Uint16 CIM_EnabledLogicalElement_RequestedState_value = 12;

/* property CIM_EnabledLogicalElement.RequestedState */
static MI_CONST MI_PropertyDecl CIM_EnabledLogicalElement_RequestedState_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0072650E, /* code */
    MI_T("RequestedState"), /* name */
    CIM_EnabledLogicalElement_RequestedState_quals, /* qualifiers */
    MI_COUNT(CIM_EnabledLogicalElement_RequestedState_quals), /* numQualifiers */
    MI_UINT16, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_EnabledLogicalElement, RequestedState), /* offset */
    MI_T("CIM_EnabledLogicalElement"), /* origin */
    MI_T("CIM_EnabledLogicalElement"), /* propagator */
    &CIM_EnabledLogicalElement_RequestedState_value,
};

static MI_CONST MI_Uint16 CIM_EnabledLogicalElement_EnabledDefault_value = 2;

/* property CIM_EnabledLogicalElement.EnabledDefault */
static MI_CONST MI_PropertyDecl CIM_EnabledLogicalElement_EnabledDefault_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0065740E, /* code */
    MI_T("EnabledDefault"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT16, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_EnabledLogicalElement, EnabledDefault), /* offset */
    MI_T("CIM_EnabledLogicalElement"), /* origin */
    MI_T("CIM_EnabledLogicalElement"), /* propagator */
    &CIM_EnabledLogicalElement_EnabledDefault_value,
};

/* property CIM_EnabledLogicalElement.TimeOfLastStateChange */
static MI_CONST MI_PropertyDecl CIM_EnabledLogicalElement_TimeOfLastStateChange_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00746515, /* code */
    MI_T("TimeOfLastStateChange"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_DATETIME, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_EnabledLogicalElement, TimeOfLastStateChange), /* offset */
    MI_T("CIM_EnabledLogicalElement"), /* origin */
    MI_T("CIM_EnabledLogicalElement"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_EnabledLogicalElement_AvailableRequestedStates_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_EnabledLogicalElement.RequestStateChange"),
    MI_T("CIM_EnabledLogicalElementCapabilities.RequestedStatesSupported"),
};

static MI_CONST MI_ConstStringA CIM_EnabledLogicalElement_AvailableRequestedStates_ModelCorrespondence_qual_value =
{
    CIM_EnabledLogicalElement_AvailableRequestedStates_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_EnabledLogicalElement_AvailableRequestedStates_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_EnabledLogicalElement_AvailableRequestedStates_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_EnabledLogicalElement_AvailableRequestedStates_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_EnabledLogicalElement_AvailableRequestedStates_quals[] =
{
    &CIM_EnabledLogicalElement_AvailableRequestedStates_ModelCorrespondence_qual,
};

/* property CIM_EnabledLogicalElement.AvailableRequestedStates */
static MI_CONST MI_PropertyDecl CIM_EnabledLogicalElement_AvailableRequestedStates_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00617318, /* code */
    MI_T("AvailableRequestedStates"), /* name */
    CIM_EnabledLogicalElement_AvailableRequestedStates_quals, /* qualifiers */
    MI_COUNT(CIM_EnabledLogicalElement_AvailableRequestedStates_quals), /* numQualifiers */
    MI_UINT16A, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_EnabledLogicalElement, AvailableRequestedStates), /* offset */
    MI_T("CIM_EnabledLogicalElement"), /* origin */
    MI_T("CIM_EnabledLogicalElement"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_EnabledLogicalElement_TransitioningToState_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_EnabledLogicalElement.RequestStateChange"),
    MI_T("CIM_EnabledLogicalElement.RequestedState"),
    MI_T("CIM_EnabledLogicalElement.EnabledState"),
};

static MI_CONST MI_ConstStringA CIM_EnabledLogicalElement_TransitioningToState_ModelCorrespondence_qual_value =
{
    CIM_EnabledLogicalElement_TransitioningToState_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_EnabledLogicalElement_TransitioningToState_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_EnabledLogicalElement_TransitioningToState_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_EnabledLogicalElement_TransitioningToState_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_EnabledLogicalElement_TransitioningToState_quals[] =
{
    &CIM_EnabledLogicalElement_TransitioningToState_ModelCorrespondence_qual,
};

static MI_CONST MI_Uint16 CIM_EnabledLogicalElement_TransitioningToState_value = 12;

/* property CIM_EnabledLogicalElement.TransitioningToState */
static MI_CONST MI_PropertyDecl CIM_EnabledLogicalElement_TransitioningToState_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00746514, /* code */
    MI_T("TransitioningToState"), /* name */
    CIM_EnabledLogicalElement_TransitioningToState_quals, /* qualifiers */
    MI_COUNT(CIM_EnabledLogicalElement_TransitioningToState_quals), /* numQualifiers */
    MI_UINT16, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_EnabledLogicalElement, TransitioningToState), /* offset */
    MI_T("CIM_EnabledLogicalElement"), /* origin */
    MI_T("CIM_EnabledLogicalElement"), /* propagator */
    &CIM_EnabledLogicalElement_TransitioningToState_value,
};

static MI_PropertyDecl MI_CONST* MI_CONST CIM_EnabledLogicalElement_props[] =
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
    &CIM_EnabledLogicalElement_EnabledState_prop,
    &CIM_EnabledLogicalElement_OtherEnabledState_prop,
    &CIM_EnabledLogicalElement_RequestedState_prop,
    &CIM_EnabledLogicalElement_EnabledDefault_prop,
    &CIM_EnabledLogicalElement_TimeOfLastStateChange_prop,
    &CIM_EnabledLogicalElement_AvailableRequestedStates_prop,
    &CIM_EnabledLogicalElement_TransitioningToState_prop,
};

static MI_CONST MI_Char* CIM_EnabledLogicalElement_RequestStateChange_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_EnabledLogicalElement.RequestedState"),
};

static MI_CONST MI_ConstStringA CIM_EnabledLogicalElement_RequestStateChange_ModelCorrespondence_qual_value =
{
    CIM_EnabledLogicalElement_RequestStateChange_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_EnabledLogicalElement_RequestStateChange_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_EnabledLogicalElement_RequestStateChange_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_EnabledLogicalElement_RequestStateChange_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_EnabledLogicalElement_RequestStateChange_quals[] =
{
    &CIM_EnabledLogicalElement_RequestStateChange_ModelCorrespondence_qual,
};

static MI_CONST MI_Char* CIM_EnabledLogicalElement_RequestStateChange_RequestedState_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_EnabledLogicalElement.RequestedState"),
};

static MI_CONST MI_ConstStringA CIM_EnabledLogicalElement_RequestStateChange_RequestedState_ModelCorrespondence_qual_value =
{
    CIM_EnabledLogicalElement_RequestStateChange_RequestedState_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_EnabledLogicalElement_RequestStateChange_RequestedState_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_EnabledLogicalElement_RequestStateChange_RequestedState_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_EnabledLogicalElement_RequestStateChange_RequestedState_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_EnabledLogicalElement_RequestStateChange_RequestedState_quals[] =
{
    &CIM_EnabledLogicalElement_RequestStateChange_RequestedState_ModelCorrespondence_qual,
};

/* parameter CIM_EnabledLogicalElement.RequestStateChange(): RequestedState */
static MI_CONST MI_ParameterDecl CIM_EnabledLogicalElement_RequestStateChange_RequestedState_param =
{
    MI_FLAG_PARAMETER|MI_FLAG_IN, /* flags */
    0x0072650E, /* code */
    MI_T("RequestedState"), /* name */
    CIM_EnabledLogicalElement_RequestStateChange_RequestedState_quals, /* qualifiers */
    MI_COUNT(CIM_EnabledLogicalElement_RequestStateChange_RequestedState_quals), /* numQualifiers */
    MI_UINT16, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_EnabledLogicalElement_RequestStateChange, RequestedState), /* offset */
};

/* parameter CIM_EnabledLogicalElement.RequestStateChange(): Job */
static MI_CONST MI_ParameterDecl CIM_EnabledLogicalElement_RequestStateChange_Job_param =
{
    MI_FLAG_PARAMETER|MI_FLAG_OUT, /* flags */
    0x006A6203, /* code */
    MI_T("Job"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_REFERENCE, /* type */
    MI_T("CIM_ConcreteJob"), /* className */
    0, /* subscript */
    offsetof(CIM_EnabledLogicalElement_RequestStateChange, Job), /* offset */
};

/* parameter CIM_EnabledLogicalElement.RequestStateChange(): TimeoutPeriod */
static MI_CONST MI_ParameterDecl CIM_EnabledLogicalElement_RequestStateChange_TimeoutPeriod_param =
{
    MI_FLAG_PARAMETER|MI_FLAG_IN, /* flags */
    0x0074640D, /* code */
    MI_T("TimeoutPeriod"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_DATETIME, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_EnabledLogicalElement_RequestStateChange, TimeoutPeriod), /* offset */
};

static MI_CONST MI_Char* CIM_EnabledLogicalElement_RequestStateChange_MIReturn_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_EnabledLogicalElement.RequestedState"),
};

static MI_CONST MI_ConstStringA CIM_EnabledLogicalElement_RequestStateChange_MIReturn_ModelCorrespondence_qual_value =
{
    CIM_EnabledLogicalElement_RequestStateChange_MIReturn_ModelCorrespondence_qual_data_value,
    MI_COUNT(CIM_EnabledLogicalElement_RequestStateChange_MIReturn_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier CIM_EnabledLogicalElement_RequestStateChange_MIReturn_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &CIM_EnabledLogicalElement_RequestStateChange_MIReturn_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_EnabledLogicalElement_RequestStateChange_MIReturn_quals[] =
{
    &CIM_EnabledLogicalElement_RequestStateChange_MIReturn_ModelCorrespondence_qual,
};

/* parameter CIM_EnabledLogicalElement.RequestStateChange(): MIReturn */
static MI_CONST MI_ParameterDecl CIM_EnabledLogicalElement_RequestStateChange_MIReturn_param =
{
    MI_FLAG_PARAMETER|MI_FLAG_OUT, /* flags */
    0x006D6E08, /* code */
    MI_T("MIReturn"), /* name */
    CIM_EnabledLogicalElement_RequestStateChange_MIReturn_quals, /* qualifiers */
    MI_COUNT(CIM_EnabledLogicalElement_RequestStateChange_MIReturn_quals), /* numQualifiers */
    MI_UINT32, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_EnabledLogicalElement_RequestStateChange, MIReturn), /* offset */
};

static MI_ParameterDecl MI_CONST* MI_CONST CIM_EnabledLogicalElement_RequestStateChange_params[] =
{
    &CIM_EnabledLogicalElement_RequestStateChange_MIReturn_param,
    &CIM_EnabledLogicalElement_RequestStateChange_RequestedState_param,
    &CIM_EnabledLogicalElement_RequestStateChange_Job_param,
    &CIM_EnabledLogicalElement_RequestStateChange_TimeoutPeriod_param,
};

/* method CIM_EnabledLogicalElement.RequestStateChange() */
MI_CONST MI_MethodDecl CIM_EnabledLogicalElement_RequestStateChange_rtti =
{
    MI_FLAG_METHOD, /* flags */
    0x00726512, /* code */
    MI_T("RequestStateChange"), /* name */
    CIM_EnabledLogicalElement_RequestStateChange_quals, /* qualifiers */
    MI_COUNT(CIM_EnabledLogicalElement_RequestStateChange_quals), /* numQualifiers */
    CIM_EnabledLogicalElement_RequestStateChange_params, /* parameters */
    MI_COUNT(CIM_EnabledLogicalElement_RequestStateChange_params), /* numParameters */
    sizeof(CIM_EnabledLogicalElement_RequestStateChange), /* size */
    MI_UINT32, /* returnType */
    MI_T("CIM_EnabledLogicalElement"), /* origin */
    MI_T("CIM_EnabledLogicalElement"), /* propagator */
    &schemaDecl, /* schema */
    NULL, /* method */
};

static MI_MethodDecl MI_CONST* MI_CONST CIM_EnabledLogicalElement_meths[] =
{
    &CIM_EnabledLogicalElement_RequestStateChange_rtti,
};

static MI_CONST MI_Char* CIM_EnabledLogicalElement_UMLPackagePath_qual_value = MI_T("CIM::Core::CoreElements");

static MI_CONST MI_Qualifier CIM_EnabledLogicalElement_UMLPackagePath_qual =
{
    MI_T("UMLPackagePath"),
    MI_STRING,
    0,
    &CIM_EnabledLogicalElement_UMLPackagePath_qual_value
};

static MI_CONST MI_Char* CIM_EnabledLogicalElement_Version_qual_value = MI_T("2.22.0");

static MI_CONST MI_Qualifier CIM_EnabledLogicalElement_Version_qual =
{
    MI_T("Version"),
    MI_STRING,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_TRANSLATABLE|MI_FLAG_RESTRICTED,
    &CIM_EnabledLogicalElement_Version_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_EnabledLogicalElement_quals[] =
{
    &CIM_EnabledLogicalElement_UMLPackagePath_qual,
    &CIM_EnabledLogicalElement_Version_qual,
};

/* class CIM_EnabledLogicalElement */
MI_CONST MI_ClassDecl CIM_EnabledLogicalElement_rtti =
{
    MI_FLAG_CLASS|MI_FLAG_ABSTRACT, /* flags */
    0x00637419, /* code */
    MI_T("CIM_EnabledLogicalElement"), /* name */
    CIM_EnabledLogicalElement_quals, /* qualifiers */
    MI_COUNT(CIM_EnabledLogicalElement_quals), /* numQualifiers */
    CIM_EnabledLogicalElement_props, /* properties */
    MI_COUNT(CIM_EnabledLogicalElement_props), /* numProperties */
    sizeof(CIM_EnabledLogicalElement), /* size */
    MI_T("CIM_LogicalElement"), /* superClass */
    &CIM_LogicalElement_rtti, /* superClassDecl */
    CIM_EnabledLogicalElement_meths, /* methods */
    MI_COUNT(CIM_EnabledLogicalElement_meths), /* numMethods */
    &schemaDecl, /* schema */
    NULL, /* functions */
    NULL, /* owningClass */
};

/*
**==============================================================================
**
** CIM_Process
**
**==============================================================================
*/

static MI_CONST MI_Uint32 CIM_Process_Name_MaxLen_qual_value = 1024U;

static MI_CONST MI_Qualifier CIM_Process_Name_MaxLen_qual =
{
    MI_T("MaxLen"),
    MI_UINT32,
    0,
    &CIM_Process_Name_MaxLen_qual_value
};

static MI_CONST MI_Char* CIM_Process_Name_Override_qual_value = MI_T("Name");

static MI_CONST MI_Qualifier CIM_Process_Name_Override_qual =
{
    MI_T("Override"),
    MI_STRING,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_RESTRICTED,
    &CIM_Process_Name_Override_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Process_Name_quals[] =
{
    &CIM_Process_Name_MaxLen_qual,
    &CIM_Process_Name_Override_qual,
};

/* property CIM_Process.Name */
static MI_CONST MI_PropertyDecl CIM_Process_Name_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x006E6504, /* code */
    MI_T("Name"), /* name */
    CIM_Process_Name_quals, /* qualifiers */
    MI_COUNT(CIM_Process_Name_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Process, Name), /* offset */
    MI_T("CIM_ManagedSystemElement"), /* origin */
    MI_T("CIM_Process"), /* propagator */
    NULL,
};

static MI_CONST MI_Uint32 CIM_Process_CSCreationClassName_MaxLen_qual_value = 256U;

static MI_CONST MI_Qualifier CIM_Process_CSCreationClassName_MaxLen_qual =
{
    MI_T("MaxLen"),
    MI_UINT32,
    0,
    &CIM_Process_CSCreationClassName_MaxLen_qual_value
};

static MI_CONST MI_Char* CIM_Process_CSCreationClassName_Propagated_qual_value = MI_T("CIM_OperatingSystem.CSCreationClassName");

static MI_CONST MI_Qualifier CIM_Process_CSCreationClassName_Propagated_qual =
{
    MI_T("Propagated"),
    MI_STRING,
    MI_FLAG_DISABLEOVERRIDE|MI_FLAG_TOSUBCLASS,
    &CIM_Process_CSCreationClassName_Propagated_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Process_CSCreationClassName_quals[] =
{
    &CIM_Process_CSCreationClassName_MaxLen_qual,
    &CIM_Process_CSCreationClassName_Propagated_qual,
};

/* property CIM_Process.CSCreationClassName */
static MI_CONST MI_PropertyDecl CIM_Process_CSCreationClassName_prop =
{
    MI_FLAG_PROPERTY|MI_FLAG_KEY, /* flags */
    0x00636513, /* code */
    MI_T("CSCreationClassName"), /* name */
    CIM_Process_CSCreationClassName_quals, /* qualifiers */
    MI_COUNT(CIM_Process_CSCreationClassName_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Process, CSCreationClassName), /* offset */
    MI_T("CIM_Process"), /* origin */
    MI_T("CIM_Process"), /* propagator */
    NULL,
};

static MI_CONST MI_Uint32 CIM_Process_CSName_MaxLen_qual_value = 256U;

static MI_CONST MI_Qualifier CIM_Process_CSName_MaxLen_qual =
{
    MI_T("MaxLen"),
    MI_UINT32,
    0,
    &CIM_Process_CSName_MaxLen_qual_value
};

static MI_CONST MI_Char* CIM_Process_CSName_Propagated_qual_value = MI_T("CIM_OperatingSystem.CSName");

static MI_CONST MI_Qualifier CIM_Process_CSName_Propagated_qual =
{
    MI_T("Propagated"),
    MI_STRING,
    MI_FLAG_DISABLEOVERRIDE|MI_FLAG_TOSUBCLASS,
    &CIM_Process_CSName_Propagated_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Process_CSName_quals[] =
{
    &CIM_Process_CSName_MaxLen_qual,
    &CIM_Process_CSName_Propagated_qual,
};

/* property CIM_Process.CSName */
static MI_CONST MI_PropertyDecl CIM_Process_CSName_prop =
{
    MI_FLAG_PROPERTY|MI_FLAG_KEY, /* flags */
    0x00636506, /* code */
    MI_T("CSName"), /* name */
    CIM_Process_CSName_quals, /* qualifiers */
    MI_COUNT(CIM_Process_CSName_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Process, CSName), /* offset */
    MI_T("CIM_Process"), /* origin */
    MI_T("CIM_Process"), /* propagator */
    NULL,
};

static MI_CONST MI_Uint32 CIM_Process_OSCreationClassName_MaxLen_qual_value = 256U;

static MI_CONST MI_Qualifier CIM_Process_OSCreationClassName_MaxLen_qual =
{
    MI_T("MaxLen"),
    MI_UINT32,
    0,
    &CIM_Process_OSCreationClassName_MaxLen_qual_value
};

static MI_CONST MI_Char* CIM_Process_OSCreationClassName_Propagated_qual_value = MI_T("CIM_OperatingSystem.CreationClassName");

static MI_CONST MI_Qualifier CIM_Process_OSCreationClassName_Propagated_qual =
{
    MI_T("Propagated"),
    MI_STRING,
    MI_FLAG_DISABLEOVERRIDE|MI_FLAG_TOSUBCLASS,
    &CIM_Process_OSCreationClassName_Propagated_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Process_OSCreationClassName_quals[] =
{
    &CIM_Process_OSCreationClassName_MaxLen_qual,
    &CIM_Process_OSCreationClassName_Propagated_qual,
};

/* property CIM_Process.OSCreationClassName */
static MI_CONST MI_PropertyDecl CIM_Process_OSCreationClassName_prop =
{
    MI_FLAG_PROPERTY|MI_FLAG_KEY, /* flags */
    0x006F6513, /* code */
    MI_T("OSCreationClassName"), /* name */
    CIM_Process_OSCreationClassName_quals, /* qualifiers */
    MI_COUNT(CIM_Process_OSCreationClassName_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Process, OSCreationClassName), /* offset */
    MI_T("CIM_Process"), /* origin */
    MI_T("CIM_Process"), /* propagator */
    NULL,
};

static MI_CONST MI_Uint32 CIM_Process_OSName_MaxLen_qual_value = 256U;

static MI_CONST MI_Qualifier CIM_Process_OSName_MaxLen_qual =
{
    MI_T("MaxLen"),
    MI_UINT32,
    0,
    &CIM_Process_OSName_MaxLen_qual_value
};

static MI_CONST MI_Char* CIM_Process_OSName_Propagated_qual_value = MI_T("CIM_OperatingSystem.Name");

static MI_CONST MI_Qualifier CIM_Process_OSName_Propagated_qual =
{
    MI_T("Propagated"),
    MI_STRING,
    MI_FLAG_DISABLEOVERRIDE|MI_FLAG_TOSUBCLASS,
    &CIM_Process_OSName_Propagated_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Process_OSName_quals[] =
{
    &CIM_Process_OSName_MaxLen_qual,
    &CIM_Process_OSName_Propagated_qual,
};

/* property CIM_Process.OSName */
static MI_CONST MI_PropertyDecl CIM_Process_OSName_prop =
{
    MI_FLAG_PROPERTY|MI_FLAG_KEY, /* flags */
    0x006F6506, /* code */
    MI_T("OSName"), /* name */
    CIM_Process_OSName_quals, /* qualifiers */
    MI_COUNT(CIM_Process_OSName_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Process, OSName), /* offset */
    MI_T("CIM_Process"), /* origin */
    MI_T("CIM_Process"), /* propagator */
    NULL,
};

static MI_CONST MI_Uint32 CIM_Process_CreationClassName_MaxLen_qual_value = 256U;

static MI_CONST MI_Qualifier CIM_Process_CreationClassName_MaxLen_qual =
{
    MI_T("MaxLen"),
    MI_UINT32,
    0,
    &CIM_Process_CreationClassName_MaxLen_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Process_CreationClassName_quals[] =
{
    &CIM_Process_CreationClassName_MaxLen_qual,
};

/* property CIM_Process.CreationClassName */
static MI_CONST MI_PropertyDecl CIM_Process_CreationClassName_prop =
{
    MI_FLAG_PROPERTY|MI_FLAG_KEY, /* flags */
    0x00636511, /* code */
    MI_T("CreationClassName"), /* name */
    CIM_Process_CreationClassName_quals, /* qualifiers */
    MI_COUNT(CIM_Process_CreationClassName_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Process, CreationClassName), /* offset */
    MI_T("CIM_Process"), /* origin */
    MI_T("CIM_Process"), /* propagator */
    NULL,
};

static MI_CONST MI_Uint32 CIM_Process_Handle_MaxLen_qual_value = 256U;

static MI_CONST MI_Qualifier CIM_Process_Handle_MaxLen_qual =
{
    MI_T("MaxLen"),
    MI_UINT32,
    0,
    &CIM_Process_Handle_MaxLen_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Process_Handle_quals[] =
{
    &CIM_Process_Handle_MaxLen_qual,
};

/* property CIM_Process.Handle */
static MI_CONST MI_PropertyDecl CIM_Process_Handle_prop =
{
    MI_FLAG_PROPERTY|MI_FLAG_KEY, /* flags */
    0x00686506, /* code */
    MI_T("Handle"), /* name */
    CIM_Process_Handle_quals, /* qualifiers */
    MI_COUNT(CIM_Process_Handle_quals), /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Process, Handle), /* offset */
    MI_T("CIM_Process"), /* origin */
    MI_T("CIM_Process"), /* propagator */
    NULL,
};

/* property CIM_Process.Priority */
static MI_CONST MI_PropertyDecl CIM_Process_Priority_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00707908, /* code */
    MI_T("Priority"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT32, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Process, Priority), /* offset */
    MI_T("CIM_Process"), /* origin */
    MI_T("CIM_Process"), /* propagator */
    NULL,
};

/* property CIM_Process.ExecutionState */
static MI_CONST MI_PropertyDecl CIM_Process_ExecutionState_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0065650E, /* code */
    MI_T("ExecutionState"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT16, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Process, ExecutionState), /* offset */
    MI_T("CIM_Process"), /* origin */
    MI_T("CIM_Process"), /* propagator */
    NULL,
};

/* property CIM_Process.OtherExecutionDescription */
static MI_CONST MI_PropertyDecl CIM_Process_OtherExecutionDescription_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x006F6E19, /* code */
    MI_T("OtherExecutionDescription"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_STRING, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Process, OtherExecutionDescription), /* offset */
    MI_T("CIM_Process"), /* origin */
    MI_T("CIM_Process"), /* propagator */
    NULL,
};

/* property CIM_Process.CreationDate */
static MI_CONST MI_PropertyDecl CIM_Process_CreationDate_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0063650C, /* code */
    MI_T("CreationDate"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_DATETIME, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Process, CreationDate), /* offset */
    MI_T("CIM_Process"), /* origin */
    MI_T("CIM_Process"), /* propagator */
    NULL,
};

/* property CIM_Process.TerminationDate */
static MI_CONST MI_PropertyDecl CIM_Process_TerminationDate_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0074650F, /* code */
    MI_T("TerminationDate"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_DATETIME, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Process, TerminationDate), /* offset */
    MI_T("CIM_Process"), /* origin */
    MI_T("CIM_Process"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_Process_KernelModeTime_Units_qual_value = MI_T("MilliSeconds");

static MI_CONST MI_Qualifier CIM_Process_KernelModeTime_Units_qual =
{
    MI_T("Units"),
    MI_STRING,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_TOSUBCLASS|MI_FLAG_TRANSLATABLE,
    &CIM_Process_KernelModeTime_Units_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Process_KernelModeTime_quals[] =
{
    &CIM_Process_KernelModeTime_Units_qual,
};

/* property CIM_Process.KernelModeTime */
static MI_CONST MI_PropertyDecl CIM_Process_KernelModeTime_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x006B650E, /* code */
    MI_T("KernelModeTime"), /* name */
    CIM_Process_KernelModeTime_quals, /* qualifiers */
    MI_COUNT(CIM_Process_KernelModeTime_quals), /* numQualifiers */
    MI_UINT64, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Process, KernelModeTime), /* offset */
    MI_T("CIM_Process"), /* origin */
    MI_T("CIM_Process"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_Process_UserModeTime_Units_qual_value = MI_T("MilliSeconds");

static MI_CONST MI_Qualifier CIM_Process_UserModeTime_Units_qual =
{
    MI_T("Units"),
    MI_STRING,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_TOSUBCLASS|MI_FLAG_TRANSLATABLE,
    &CIM_Process_UserModeTime_Units_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Process_UserModeTime_quals[] =
{
    &CIM_Process_UserModeTime_Units_qual,
};

/* property CIM_Process.UserModeTime */
static MI_CONST MI_PropertyDecl CIM_Process_UserModeTime_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0075650C, /* code */
    MI_T("UserModeTime"), /* name */
    CIM_Process_UserModeTime_quals, /* qualifiers */
    MI_COUNT(CIM_Process_UserModeTime_quals), /* numQualifiers */
    MI_UINT64, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Process, UserModeTime), /* offset */
    MI_T("CIM_Process"), /* origin */
    MI_T("CIM_Process"), /* propagator */
    NULL,
};

static MI_CONST MI_Char* CIM_Process_WorkingSetSize_Units_qual_value = MI_T("Bytes");

static MI_CONST MI_Qualifier CIM_Process_WorkingSetSize_Units_qual =
{
    MI_T("Units"),
    MI_STRING,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_TOSUBCLASS|MI_FLAG_TRANSLATABLE,
    &CIM_Process_WorkingSetSize_Units_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Process_WorkingSetSize_quals[] =
{
    &CIM_Process_WorkingSetSize_Units_qual,
};

/* property CIM_Process.WorkingSetSize */
static MI_CONST MI_PropertyDecl CIM_Process_WorkingSetSize_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0077650E, /* code */
    MI_T("WorkingSetSize"), /* name */
    CIM_Process_WorkingSetSize_quals, /* qualifiers */
    MI_COUNT(CIM_Process_WorkingSetSize_quals), /* numQualifiers */
    MI_UINT64, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(CIM_Process, WorkingSetSize), /* offset */
    MI_T("CIM_Process"), /* origin */
    MI_T("CIM_Process"), /* propagator */
    NULL,
};

static MI_PropertyDecl MI_CONST* MI_CONST CIM_Process_props[] =
{
    &CIM_ManagedElement_InstanceID_prop,
    &CIM_ManagedElement_Caption_prop,
    &CIM_ManagedElement_Description_prop,
    &CIM_ManagedElement_ElementName_prop,
    &CIM_ManagedSystemElement_InstallDate_prop,
    &CIM_Process_Name_prop,
    &CIM_ManagedSystemElement_OperationalStatus_prop,
    &CIM_ManagedSystemElement_StatusDescriptions_prop,
    &CIM_ManagedSystemElement_Status_prop,
    &CIM_ManagedSystemElement_HealthState_prop,
    &CIM_ManagedSystemElement_CommunicationStatus_prop,
    &CIM_ManagedSystemElement_DetailedStatus_prop,
    &CIM_ManagedSystemElement_OperatingStatus_prop,
    &CIM_ManagedSystemElement_PrimaryStatus_prop,
    &CIM_EnabledLogicalElement_EnabledState_prop,
    &CIM_EnabledLogicalElement_OtherEnabledState_prop,
    &CIM_EnabledLogicalElement_RequestedState_prop,
    &CIM_EnabledLogicalElement_EnabledDefault_prop,
    &CIM_EnabledLogicalElement_TimeOfLastStateChange_prop,
    &CIM_EnabledLogicalElement_AvailableRequestedStates_prop,
    &CIM_EnabledLogicalElement_TransitioningToState_prop,
    &CIM_Process_CSCreationClassName_prop,
    &CIM_Process_CSName_prop,
    &CIM_Process_OSCreationClassName_prop,
    &CIM_Process_OSName_prop,
    &CIM_Process_CreationClassName_prop,
    &CIM_Process_Handle_prop,
    &CIM_Process_Priority_prop,
    &CIM_Process_ExecutionState_prop,
    &CIM_Process_OtherExecutionDescription_prop,
    &CIM_Process_CreationDate_prop,
    &CIM_Process_TerminationDate_prop,
    &CIM_Process_KernelModeTime_prop,
    &CIM_Process_UserModeTime_prop,
    &CIM_Process_WorkingSetSize_prop,
};

static MI_MethodDecl MI_CONST* MI_CONST CIM_Process_meths[] =
{
    &CIM_EnabledLogicalElement_RequestStateChange_rtti,
};

static MI_CONST MI_Char* CIM_Process_UMLPackagePath_qual_value = MI_T("CIM::System::Processing");

static MI_CONST MI_Qualifier CIM_Process_UMLPackagePath_qual =
{
    MI_T("UMLPackagePath"),
    MI_STRING,
    0,
    &CIM_Process_UMLPackagePath_qual_value
};

static MI_CONST MI_Char* CIM_Process_Version_qual_value = MI_T("2.10.0");

static MI_CONST MI_Qualifier CIM_Process_Version_qual =
{
    MI_T("Version"),
    MI_STRING,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_TRANSLATABLE|MI_FLAG_RESTRICTED,
    &CIM_Process_Version_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST CIM_Process_quals[] =
{
    &CIM_Process_UMLPackagePath_qual,
    &CIM_Process_Version_qual,
};

/* class CIM_Process */
MI_CONST MI_ClassDecl CIM_Process_rtti =
{
    MI_FLAG_CLASS, /* flags */
    0x0063730B, /* code */
    MI_T("CIM_Process"), /* name */
    CIM_Process_quals, /* qualifiers */
    MI_COUNT(CIM_Process_quals), /* numQualifiers */
    CIM_Process_props, /* properties */
    MI_COUNT(CIM_Process_props), /* numProperties */
    sizeof(CIM_Process), /* size */
    MI_T("CIM_EnabledLogicalElement"), /* superClass */
    &CIM_EnabledLogicalElement_rtti, /* superClassDecl */
    CIM_Process_meths, /* methods */
    MI_COUNT(CIM_Process_meths), /* numMethods */
    &schemaDecl, /* schema */
    NULL, /* functions */
    NULL, /* owningClass */
};

/*
**==============================================================================
**
** Apache_HTTPDProcess
**
**==============================================================================
*/

/* property Apache_HTTPDProcess.PercentUserTime */
static MI_CONST MI_PropertyDecl Apache_HTTPDProcess_PercentUserTime_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x0070650F, /* code */
    MI_T("PercentUserTime"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT16, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDProcess, PercentUserTime), /* offset */
    MI_T("Apache_HTTPDProcess"), /* origin */
    MI_T("Apache_HTTPDProcess"), /* propagator */
    NULL,
};

/* property Apache_HTTPDProcess.WorkingSetSizeMB */
static MI_CONST MI_PropertyDecl Apache_HTTPDProcess_WorkingSetSizeMB_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00776210, /* code */
    MI_T("WorkingSetSizeMB"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT32, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDProcess, WorkingSetSizeMB), /* offset */
    MI_T("Apache_HTTPDProcess"), /* origin */
    MI_T("Apache_HTTPDProcess"), /* propagator */
    NULL,
};

static MI_PropertyDecl MI_CONST* MI_CONST Apache_HTTPDProcess_props[] =
{
    &CIM_ManagedElement_InstanceID_prop,
    &CIM_ManagedElement_Caption_prop,
    &CIM_ManagedElement_Description_prop,
    &CIM_ManagedElement_ElementName_prop,
    &CIM_ManagedSystemElement_InstallDate_prop,
    &CIM_Process_Name_prop,
    &CIM_ManagedSystemElement_OperationalStatus_prop,
    &CIM_ManagedSystemElement_StatusDescriptions_prop,
    &CIM_ManagedSystemElement_Status_prop,
    &CIM_ManagedSystemElement_HealthState_prop,
    &CIM_ManagedSystemElement_CommunicationStatus_prop,
    &CIM_ManagedSystemElement_DetailedStatus_prop,
    &CIM_ManagedSystemElement_OperatingStatus_prop,
    &CIM_ManagedSystemElement_PrimaryStatus_prop,
    &CIM_EnabledLogicalElement_EnabledState_prop,
    &CIM_EnabledLogicalElement_OtherEnabledState_prop,
    &CIM_EnabledLogicalElement_RequestedState_prop,
    &CIM_EnabledLogicalElement_EnabledDefault_prop,
    &CIM_EnabledLogicalElement_TimeOfLastStateChange_prop,
    &CIM_EnabledLogicalElement_AvailableRequestedStates_prop,
    &CIM_EnabledLogicalElement_TransitioningToState_prop,
    &CIM_Process_CSCreationClassName_prop,
    &CIM_Process_CSName_prop,
    &CIM_Process_OSCreationClassName_prop,
    &CIM_Process_OSName_prop,
    &CIM_Process_CreationClassName_prop,
    &CIM_Process_Handle_prop,
    &CIM_Process_Priority_prop,
    &CIM_Process_ExecutionState_prop,
    &CIM_Process_OtherExecutionDescription_prop,
    &CIM_Process_CreationDate_prop,
    &CIM_Process_TerminationDate_prop,
    &CIM_Process_KernelModeTime_prop,
    &CIM_Process_UserModeTime_prop,
    &CIM_Process_WorkingSetSize_prop,
    &Apache_HTTPDProcess_PercentUserTime_prop,
    &Apache_HTTPDProcess_WorkingSetSizeMB_prop,
};

static MI_CONST MI_Char* Apache_HTTPDProcess_RequestStateChange_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_EnabledLogicalElement.RequestedState"),
};

static MI_CONST MI_ConstStringA Apache_HTTPDProcess_RequestStateChange_ModelCorrespondence_qual_value =
{
    Apache_HTTPDProcess_RequestStateChange_ModelCorrespondence_qual_data_value,
    MI_COUNT(Apache_HTTPDProcess_RequestStateChange_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier Apache_HTTPDProcess_RequestStateChange_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &Apache_HTTPDProcess_RequestStateChange_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST Apache_HTTPDProcess_RequestStateChange_quals[] =
{
    &Apache_HTTPDProcess_RequestStateChange_ModelCorrespondence_qual,
};

static MI_CONST MI_Char* Apache_HTTPDProcess_RequestStateChange_RequestedState_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_EnabledLogicalElement.RequestedState"),
};

static MI_CONST MI_ConstStringA Apache_HTTPDProcess_RequestStateChange_RequestedState_ModelCorrespondence_qual_value =
{
    Apache_HTTPDProcess_RequestStateChange_RequestedState_ModelCorrespondence_qual_data_value,
    MI_COUNT(Apache_HTTPDProcess_RequestStateChange_RequestedState_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier Apache_HTTPDProcess_RequestStateChange_RequestedState_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &Apache_HTTPDProcess_RequestStateChange_RequestedState_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST Apache_HTTPDProcess_RequestStateChange_RequestedState_quals[] =
{
    &Apache_HTTPDProcess_RequestStateChange_RequestedState_ModelCorrespondence_qual,
};

/* parameter Apache_HTTPDProcess.RequestStateChange(): RequestedState */
static MI_CONST MI_ParameterDecl Apache_HTTPDProcess_RequestStateChange_RequestedState_param =
{
    MI_FLAG_PARAMETER|MI_FLAG_IN, /* flags */
    0x0072650E, /* code */
    MI_T("RequestedState"), /* name */
    Apache_HTTPDProcess_RequestStateChange_RequestedState_quals, /* qualifiers */
    MI_COUNT(Apache_HTTPDProcess_RequestStateChange_RequestedState_quals), /* numQualifiers */
    MI_UINT16, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDProcess_RequestStateChange, RequestedState), /* offset */
};

/* parameter Apache_HTTPDProcess.RequestStateChange(): Job */
static MI_CONST MI_ParameterDecl Apache_HTTPDProcess_RequestStateChange_Job_param =
{
    MI_FLAG_PARAMETER|MI_FLAG_OUT, /* flags */
    0x006A6203, /* code */
    MI_T("Job"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_REFERENCE, /* type */
    MI_T("CIM_ConcreteJob"), /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDProcess_RequestStateChange, Job), /* offset */
};

/* parameter Apache_HTTPDProcess.RequestStateChange(): TimeoutPeriod */
static MI_CONST MI_ParameterDecl Apache_HTTPDProcess_RequestStateChange_TimeoutPeriod_param =
{
    MI_FLAG_PARAMETER|MI_FLAG_IN, /* flags */
    0x0074640D, /* code */
    MI_T("TimeoutPeriod"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_DATETIME, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDProcess_RequestStateChange, TimeoutPeriod), /* offset */
};

static MI_CONST MI_Char* Apache_HTTPDProcess_RequestStateChange_MIReturn_ModelCorrespondence_qual_data_value[] =
{
    MI_T("CIM_EnabledLogicalElement.RequestedState"),
};

static MI_CONST MI_ConstStringA Apache_HTTPDProcess_RequestStateChange_MIReturn_ModelCorrespondence_qual_value =
{
    Apache_HTTPDProcess_RequestStateChange_MIReturn_ModelCorrespondence_qual_data_value,
    MI_COUNT(Apache_HTTPDProcess_RequestStateChange_MIReturn_ModelCorrespondence_qual_data_value),
};

static MI_CONST MI_Qualifier Apache_HTTPDProcess_RequestStateChange_MIReturn_ModelCorrespondence_qual =
{
    MI_T("ModelCorrespondence"),
    MI_STRINGA,
    0,
    &Apache_HTTPDProcess_RequestStateChange_MIReturn_ModelCorrespondence_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST Apache_HTTPDProcess_RequestStateChange_MIReturn_quals[] =
{
    &Apache_HTTPDProcess_RequestStateChange_MIReturn_ModelCorrespondence_qual,
};

/* parameter Apache_HTTPDProcess.RequestStateChange(): MIReturn */
static MI_CONST MI_ParameterDecl Apache_HTTPDProcess_RequestStateChange_MIReturn_param =
{
    MI_FLAG_PARAMETER|MI_FLAG_OUT, /* flags */
    0x006D6E08, /* code */
    MI_T("MIReturn"), /* name */
    Apache_HTTPDProcess_RequestStateChange_MIReturn_quals, /* qualifiers */
    MI_COUNT(Apache_HTTPDProcess_RequestStateChange_MIReturn_quals), /* numQualifiers */
    MI_UINT32, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDProcess_RequestStateChange, MIReturn), /* offset */
};

static MI_ParameterDecl MI_CONST* MI_CONST Apache_HTTPDProcess_RequestStateChange_params[] =
{
    &Apache_HTTPDProcess_RequestStateChange_MIReturn_param,
    &Apache_HTTPDProcess_RequestStateChange_RequestedState_param,
    &Apache_HTTPDProcess_RequestStateChange_Job_param,
    &Apache_HTTPDProcess_RequestStateChange_TimeoutPeriod_param,
};

/* method Apache_HTTPDProcess.RequestStateChange() */
MI_CONST MI_MethodDecl Apache_HTTPDProcess_RequestStateChange_rtti =
{
    MI_FLAG_METHOD, /* flags */
    0x00726512, /* code */
    MI_T("RequestStateChange"), /* name */
    Apache_HTTPDProcess_RequestStateChange_quals, /* qualifiers */
    MI_COUNT(Apache_HTTPDProcess_RequestStateChange_quals), /* numQualifiers */
    Apache_HTTPDProcess_RequestStateChange_params, /* parameters */
    MI_COUNT(Apache_HTTPDProcess_RequestStateChange_params), /* numParameters */
    sizeof(Apache_HTTPDProcess_RequestStateChange), /* size */
    MI_UINT32, /* returnType */
    MI_T("CIM_EnabledLogicalElement"), /* origin */
    MI_T("CIM_EnabledLogicalElement"), /* propagator */
    &schemaDecl, /* schema */
    (MI_ProviderFT_Invoke)Apache_HTTPDProcess_Invoke_RequestStateChange, /* method */
};

static MI_MethodDecl MI_CONST* MI_CONST Apache_HTTPDProcess_meths[] =
{
    &Apache_HTTPDProcess_RequestStateChange_rtti,
};

static MI_CONST MI_ProviderFT Apache_HTTPDProcess_funcs =
{
  (MI_ProviderFT_Load)Apache_HTTPDProcess_Load,
  (MI_ProviderFT_Unload)Apache_HTTPDProcess_Unload,
  (MI_ProviderFT_GetInstance)Apache_HTTPDProcess_GetInstance,
  (MI_ProviderFT_EnumerateInstances)Apache_HTTPDProcess_EnumerateInstances,
  (MI_ProviderFT_CreateInstance)Apache_HTTPDProcess_CreateInstance,
  (MI_ProviderFT_ModifyInstance)Apache_HTTPDProcess_ModifyInstance,
  (MI_ProviderFT_DeleteInstance)Apache_HTTPDProcess_DeleteInstance,
  (MI_ProviderFT_AssociatorInstances)NULL,
  (MI_ProviderFT_ReferenceInstances)NULL,
  (MI_ProviderFT_EnableIndications)NULL,
  (MI_ProviderFT_DisableIndications)NULL,
  (MI_ProviderFT_Subscribe)NULL,
  (MI_ProviderFT_Unsubscribe)NULL,
  (MI_ProviderFT_Invoke)NULL,
};

static MI_CONST MI_Char* Apache_HTTPDProcess_UMLPackagePath_qual_value = MI_T("CIM::System::Processing");

static MI_CONST MI_Qualifier Apache_HTTPDProcess_UMLPackagePath_qual =
{
    MI_T("UMLPackagePath"),
    MI_STRING,
    0,
    &Apache_HTTPDProcess_UMLPackagePath_qual_value
};

static MI_CONST MI_Char* Apache_HTTPDProcess_Version_qual_value = MI_T("1.0.0");

static MI_CONST MI_Qualifier Apache_HTTPDProcess_Version_qual =
{
    MI_T("Version"),
    MI_STRING,
    MI_FLAG_ENABLEOVERRIDE|MI_FLAG_TRANSLATABLE|MI_FLAG_RESTRICTED,
    &Apache_HTTPDProcess_Version_qual_value
};

static MI_Qualifier MI_CONST* MI_CONST Apache_HTTPDProcess_quals[] =
{
    &Apache_HTTPDProcess_UMLPackagePath_qual,
    &Apache_HTTPDProcess_Version_qual,
};

/* class Apache_HTTPDProcess */
MI_CONST MI_ClassDecl Apache_HTTPDProcess_rtti =
{
    MI_FLAG_CLASS, /* flags */
    0x00617313, /* code */
    MI_T("Apache_HTTPDProcess"), /* name */
    Apache_HTTPDProcess_quals, /* qualifiers */
    MI_COUNT(Apache_HTTPDProcess_quals), /* numQualifiers */
    Apache_HTTPDProcess_props, /* properties */
    MI_COUNT(Apache_HTTPDProcess_props), /* numProperties */
    sizeof(Apache_HTTPDProcess), /* size */
    MI_T("CIM_Process"), /* superClass */
    &CIM_Process_rtti, /* superClassDecl */
    Apache_HTTPDProcess_meths, /* methods */
    MI_COUNT(Apache_HTTPDProcess_meths), /* numMethods */
    &schemaDecl, /* schema */
    &Apache_HTTPDProcess_funcs, /* functions */
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

/* property Apache_HTTPDServerStatistics.RequestsTotal */
static MI_CONST MI_PropertyDecl Apache_HTTPDServerStatistics_RequestsTotal_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00726C0D, /* code */
    MI_T("RequestsTotal"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT64, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDServerStatistics, RequestsTotal), /* offset */
    MI_T("Apache_HTTPDServerStatistics"), /* origin */
    MI_T("Apache_HTTPDServerStatistics"), /* propagator */
    NULL,
};

/* property Apache_HTTPDServerStatistics.RequestsPerSecond */
static MI_CONST MI_PropertyDecl Apache_HTTPDServerStatistics_RequestsPerSecond_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00726411, /* code */
    MI_T("RequestsPerSecond"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT32, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDServerStatistics, RequestsPerSecond), /* offset */
    MI_T("Apache_HTTPDServerStatistics"), /* origin */
    MI_T("Apache_HTTPDServerStatistics"), /* propagator */
    NULL,
};

/* property Apache_HTTPDServerStatistics.KBPerRequest */
static MI_CONST MI_PropertyDecl Apache_HTTPDServerStatistics_KBPerRequest_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x006B740C, /* code */
    MI_T("KBPerRequest"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT32, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDServerStatistics, KBPerRequest), /* offset */
    MI_T("Apache_HTTPDServerStatistics"), /* origin */
    MI_T("Apache_HTTPDServerStatistics"), /* propagator */
    NULL,
};

/* property Apache_HTTPDServerStatistics.KBPerSecond */
static MI_CONST MI_PropertyDecl Apache_HTTPDServerStatistics_KBPerSecond_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x006B640B, /* code */
    MI_T("KBPerSecond"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT32, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDServerStatistics, KBPerSecond), /* offset */
    MI_T("Apache_HTTPDServerStatistics"), /* origin */
    MI_T("Apache_HTTPDServerStatistics"), /* propagator */
    NULL,
};

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
    &Apache_HTTPDServerStatistics_RequestsTotal_prop,
    &Apache_HTTPDServerStatistics_RequestsPerSecond_prop,
    &Apache_HTTPDServerStatistics_KBPerRequest_prop,
    &Apache_HTTPDServerStatistics_KBPerSecond_prop,
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

/* property Apache_HTTPDVirtualHost.HTTPPort */
static MI_CONST MI_PropertyDecl Apache_HTTPDVirtualHost_HTTPPort_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00687408, /* code */
    MI_T("HTTPPort"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT16, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDVirtualHost, HTTPPort), /* offset */
    MI_T("Apache_HTTPDVirtualHost"), /* origin */
    MI_T("Apache_HTTPDVirtualHost"), /* propagator */
    NULL,
};

/* property Apache_HTTPDVirtualHost.HTTPSPort */
static MI_CONST MI_PropertyDecl Apache_HTTPDVirtualHost_HTTPSPort_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x00687409, /* code */
    MI_T("HTTPSPort"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT16, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDVirtualHost, HTTPSPort), /* offset */
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
    &Apache_HTTPDVirtualHost_HTTPPort_prop,
    &Apache_HTTPDVirtualHost_HTTPSPort_prop,
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

/* property Apache_HTTPDVirtualHostStatistics.MSPerRequest */
static MI_CONST MI_PropertyDecl Apache_HTTPDVirtualHostStatistics_MSPerRequest_prop =
{
    MI_FLAG_PROPERTY, /* flags */
    0x006D740C, /* code */
    MI_T("MSPerRequest"), /* name */
    NULL, /* qualifiers */
    0, /* numQualifiers */
    MI_UINT32, /* type */
    NULL, /* className */
    0, /* subscript */
    offsetof(Apache_HTTPDVirtualHostStatistics, MSPerRequest), /* offset */
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
    &Apache_HTTPDVirtualHostStatistics_MSPerRequest_prop,
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
    &Apache_HTTPDProcess_rtti,
    &Apache_HTTPDServer_rtti,
    &Apache_HTTPDServerStatistics_rtti,
    &Apache_HTTPDVirtualHost_rtti,
    &Apache_HTTPDVirtualHostCertificate_rtti,
    &Apache_HTTPDVirtualHostStatistics_rtti,
    &CIM_Collection_rtti,
    &CIM_ConcreteJob_rtti,
    &CIM_EnabledLogicalElement_rtti,
    &CIM_Error_rtti,
    &CIM_InstalledProduct_rtti,
    &CIM_Job_rtti,
    &CIM_LogicalElement_rtti,
    &CIM_ManagedElement_rtti,
    &CIM_ManagedSystemElement_rtti,
    &CIM_Process_rtti,
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

