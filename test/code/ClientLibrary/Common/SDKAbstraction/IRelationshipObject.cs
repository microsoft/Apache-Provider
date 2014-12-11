//-----------------------------------------------------------------------
// <copyright file="IRelationshipObject.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using Microsoft.EnterpriseManagement.Common;

    /// <summary>
    /// Interface representing relationships in OpsMgr. Could for example be a managed-by relationship.
    /// </summary>
    public interface IRelationshipObject
    {
        IManagedObject Source { get; }

        IManagedObject Target { get; }

        EnterpriseManagementRelationshipObject<EnterpriseManagementObject> OpsMgrRepresentation { get; }
    }
}