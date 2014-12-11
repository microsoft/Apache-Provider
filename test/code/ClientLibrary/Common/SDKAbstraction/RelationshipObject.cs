//-----------------------------------------------------------------------
// <copyright file="RelationshipObject.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using Microsoft.EnterpriseManagement.Common;

    /// <summary>
    /// Class representing relationships in OpsMgr. Could for example be a managed-by relationship.
    /// </summary>
    public class RelationshipObject : IRelationshipObject
    {
        private EnterpriseManagementRelationshipObject<EnterpriseManagementObject> opsMgrRepresentation;

        /// <summary>
        /// This constructor creates a relationship from an existing OpsMgr relationship.
        /// </summary>
        /// <param name="opsMgrType">Relationship retrieved from OpsMgr.</param>
        public RelationshipObject(EnterpriseManagementRelationshipObject<EnterpriseManagementObject> opsMgrType)
        {
            this.opsMgrRepresentation = opsMgrType;
            this.Source = new ManagedObject(opsMgrType.SourceObject);
            this.Target = new ManagedObject(opsMgrType.TargetObject);
        }

        /// <summary>
        /// This constructor creates a relationship from an empty creatable relationship from OpsMgr.
        /// </summary>
        /// <param name="opsMgrType">Empty relationship retrieved from OpsMgr.</param>
        /// <param name="source">Source of relationship.</param>
        /// <param name="target">Target of relationship.</param>
        public RelationshipObject(CreatableEnterpriseManagementRelationshipObject opsMgrType, IManagedObject source, IManagedObject target)
        {
            this.Source = source;
            this.Target = target;
            opsMgrType.SetSource(source.OpsMgrObject);
            opsMgrType.SetTarget(target.OpsMgrObject);
            this.opsMgrRepresentation = opsMgrType;
        }

        public IManagedObject Source { get; private set; }

        public IManagedObject Target { get; private set; }

        public EnterpriseManagementRelationshipObject<EnterpriseManagementObject> OpsMgrRepresentation
        {
            get
            {
                return this.opsMgrRepresentation;
            }
        }
    }
}