//-----------------------------------------------------------------------
// <copyright file="RelationshipObjectFactory.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Microsoft.EnterpriseManagement;
    using Microsoft.EnterpriseManagement.Common;
    using Microsoft.EnterpriseManagement.Configuration;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction.Exceptions;

    /// <summary>
    /// Creates OpsMgr SDK EnterpriseManagementRelationshipObject instances.
    /// </summary>
    public class RelationshipObjectFactory : IRelationshipObjectFactory
    {
        /// <summary>
        /// Management group where the relationships are defined.
        /// </summary>
        private EnterpriseManagementGroup managementGroup;

        /// <summary>
        /// Type of relationships that this factory creates.
        /// </summary>
        private List<ManagementPackRelationship> relationshipTypes;

        private IEntityObjectsManagement entityObjects;

        /// <summary>
        /// Initializes a new instance of the RelationshipObjectFactory class.
        /// </summary>
        /// <param name="managementGroup">Management group where relationships live.</param>
        /// <param name="entityTypes">SDK class that is used for looking up relationship types.</param>
        /// <param name="entityObjects">SDK class that is used for looking up relationship objects.</param>
        /// <param name="relationshipTypeName">Name of relationship type that this factory can create.</param>
        public RelationshipObjectFactory(
            EnterpriseManagementGroup managementGroup,
            IEntityTypeManagement entityTypes,
            IEntityObjectsManagement entityObjects,
            string relationshipTypeName)
        {
            this.managementGroup = managementGroup;
            this.entityObjects = entityObjects;

            ManagementPackRelationshipCriteria criteria = new ManagementPackRelationshipCriteria(string.Format("Name = '{0}'", relationshipTypeName));
            var relationshipClasses = entityTypes.GetRelationshipClasses(criteria);
            if (1 != relationshipClasses.Count)
            {
                throw new ManagementPackRelationshipTypeNotFoundException(relationshipTypeName);
            }

            this.relationshipTypes = new List<ManagementPackRelationship>
                {
                    relationshipClasses[0]
                };
        }

        /// <summary>
        /// Initializes a new instance of the RelationshipObjectFactory class.
        /// </summary>
        /// <param name="managementGroup">Management group where relationships live.</param>
        /// <param name="entityTypes">SDK class that is used for looking up relationship types.</param>
        /// <param name="entityObjects">SDK class that is used for looking up relationship objects.</param>
        /// <param name="relationshipTypeNames">Names of relationship type that this factory can create.</param>
        public RelationshipObjectFactory(
            EnterpriseManagementGroup managementGroup,
            IEntityTypeManagement entityTypes,
            IEntityObjectsManagement entityObjects,
            IEnumerable<string> relationshipTypeNames)
        {
            this.managementGroup = managementGroup;
            this.entityObjects = entityObjects;
            this.relationshipTypes = new List<ManagementPackRelationship>();

            foreach (var relationshipTypeName in relationshipTypeNames)
            {
                ManagementPackRelationshipCriteria criteria =
                    new ManagementPackRelationshipCriteria(string.Format("Name = '{0}'", relationshipTypeName));
                var relationshipClasses = entityTypes.GetRelationshipClasses(criteria);
                if (1 != relationshipClasses.Count)
                {
                    throw new ManagementPackRelationshipTypeNotFoundException(relationshipTypeName);
                }

                this.relationshipTypes.Add(relationshipClasses[0]);
            }
        }

        /// <summary>
        /// Creates an OpsMgr SDK representation of the relationship between the two parameters.
        /// </summary>
        /// <param name="source">Source of the relationship.</param>
        /// <param name="target">Target of the relationship.</param>
        /// <returns>SDK representation of the relationship.</returns>
        public IRelationshipObject CreateRelationshipObject(IManagedObject source, IManagedObject target)
        {
            if (1 != this.relationshipTypes.Count)
            {
                throw new ArgumentException(Strings.RelationshipObjectFactory_CreateRelationshipObject_Only_one_relationship_type);
            }

            var opsMgrRepresentation = new CreatableEnterpriseManagementRelationshipObject(this.managementGroup, this.relationshipTypes[0]);
            return new RelationshipObject(opsMgrRepresentation, source, target);
        }

        /// <summary>
        /// Returns a unique existing relationship by target.
        /// </summary>
        /// <param name="target">Target for relationships.</param>
        /// <returns>A relationship object with target as target.</returns>
        public IRelationshipObject GetExistingRelationshipWhereTarget(IManagedObject target)
        {
            var relationships = this.GetRelationships(target);

            // detect orphaned unix comptuers 
            // (those previously associated with a resource pool that has been deleted).
            if (relationships.Count == 0)
            {
                throw new ManagedObjectRelationshipNotFoundException(target.DisplayName);
            }

            // we expect a 1:1 resource pool/unix computer mapping
            if (relationships.Count > 1)
            {
                string warningText = "More than one managed-by relationship detected for unix computer " +
                                     target.DisplayName;
                Trace.TraceWarning(warningText, target.DisplayName);
                Debug.Assert(false, warningText);
            }

            return new RelationshipObject(relationships[0]);
        }

        public IList<IRelationshipObject> GetExistingRelationships()
        {
            var relationships = this.GetRelationships();
            var retval = new List<IRelationshipObject>();

            foreach (var relationship in relationships)
            {
                retval.Add(new RelationshipObject(relationship));
            }

            return retval;
        }

        public IEnumerable<IRelationshipObject> GetAllRelatedObjects(Guid id)
        {
            var retval = new List<IRelationshipObject>();

            var objects = this.entityObjects.GetRelationshipObjectsWhereSource<EnterpriseManagementObject>(
                id,
                relationshipTypes,
                DerivedClassTraversalDepth.None,
                TraversalDepth.OneLevel,
                ObjectQueryOptions.Default);

            foreach (var relationship in objects)
            {
                retval.Add(new RelationshipObject(relationship));
            }

            return retval;
        }

        /// <summary>
        /// Overridden in tests.
        /// </summary>
        /// <param name="target">Target to use as criteria for searching for relationships.</param>
        /// <returns>A list of relationships.</returns>
        protected virtual IList<EnterpriseManagementRelationshipObject<EnterpriseManagementObject>> GetRelationships(IManagedObject target)
        {
            return this.entityObjects.GetRelationshipObjectsWhereTarget<EnterpriseManagementObject>(
                target.OpsMgrObject.Id,
                this.relationshipTypes,
                DerivedClassTraversalDepth.None,
                TraversalDepth.OneLevel,
                new ObjectQueryOptions { DefaultPropertyRetrievalBehavior = ObjectPropertyRetrievalBehavior.All });
        }

        protected virtual IList<EnterpriseManagementRelationshipObject<EnterpriseManagementObject>> GetRelationships()
        {
            return this.entityObjects.GetRelationshipObjects<EnterpriseManagementObject>(
                this.relationshipTypes,
                DerivedClassTraversalDepth.None,
                new ObjectQueryOptions { DefaultPropertyRetrievalBehavior = ObjectPropertyRetrievalBehavior.All });
        }
    }
}
