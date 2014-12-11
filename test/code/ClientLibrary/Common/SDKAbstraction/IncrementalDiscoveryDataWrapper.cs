//-----------------------------------------------------------------------
// <copyright file="IncrementalDiscoveryDataWrapper.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using System;
    using Microsoft.EnterpriseManagement;
    using Microsoft.EnterpriseManagement.Common;
    using Microsoft.EnterpriseManagement.ConnectorFramework;

    /// <summary>
    /// Wrapps the IncrementalDiscoveryData datatype from the OpsMgr SDK. This can be used as a transaction object for discovery data.
    /// </summary>
    public class IncrementalDiscoveryDataWrapper : IIncrementalDiscoveryData
    {
        /// <summary>
        /// SDK object wrapped by this instance.
        /// </summary>
        private IncrementalDiscoveryData wrappedData;

        /// <summary>
        /// Management group this transaction belongs to.
        /// </summary>
        private EnterpriseManagementGroup group;

        /// <summary>
        /// Initializes a new instance of the IncrementalDiscoveryDataWrapper class.
        /// </summary>
        /// <param name="incrementalDiscoveryData">SDK instance to wrap.</param>
        /// <param name="group">Management group this transaction belongs to.</param>
        public IncrementalDiscoveryDataWrapper(IncrementalDiscoveryData incrementalDiscoveryData, EnterpriseManagementGroup group)
        {
            this.wrappedData = incrementalDiscoveryData;
            this.group = group;
        }

        /// <summary>
        /// Adds an enterprise object to the transaction.
        /// </summary>
        /// <param name="managedObject">object to add to transaction.</param>
        public void Add(IManagedObject managedObject)
        {
            this.wrappedData.Add(managedObject.OpsMgrObject);
        }

        /// <summary>
        /// Adds a relationship to the transaction.
        /// </summary>
        /// <param name="relationshipObject">Relationship object to add to transaction.</param>
        public void Add(IRelationshipObject relationshipObject)
        {
            this.wrappedData.Add(relationshipObject.OpsMgrRepresentation);
        }

        /// <summary>
        /// Obliterates the managed object.
        /// </summary>
        /// <param name="managedObj">A reference to the object to be remove.</param>
        public void Remove(IManagedObject managedObj)
        {
            this.wrappedData.Remove(managedObj.OpsMgrObject);
        }

        /// <summary>
        /// Commits the data to the database.
        /// </summary>
        public void Commit()
        {
            try
            {
                this.DoCommit();
            }
            catch (DiscoveryDataInsertionCollisionException ex)
            {
                throw new DataInsertionCollisionException(ex);
            }
        }

        /// <summary>
        /// Adds the deletion of a relationship object to the transaction.
        /// </summary>
        /// <param name="relationshipObject">Relationship to delete.</param>
        public void Remove(IRelationshipObject relationshipObject)
        {
            this.wrappedData.Remove(relationshipObject.OpsMgrRepresentation);
        }

        protected virtual void DoCommit()
        {
            this.wrappedData.Commit(this.group);
        }

        /// <summary>
        /// Exception thrown when inserted data collides with existing data.
        /// </summary>
        [Serializable]
        public class DataInsertionCollisionException : Exception
        {
            public DataInsertionCollisionException(DiscoveryDataInsertionCollisionException innerException)
                : base(innerException.Message, innerException)
            {
            }
        }
    }
}
