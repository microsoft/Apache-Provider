//-----------------------------------------------------------------------
// <copyright file="IIncrementalDiscoveryData.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    /// <summary>
    /// IncrementalDiscoveryData can be used as a transaction to the database.
    /// </summary>
    public interface IIncrementalDiscoveryData
    {
        /// <summary>
        /// Adds a managed object to the transaction.
        /// </summary>
        /// <param name="managedObject">IManagedObject to add to transaction.</param>
        void Add(IManagedObject managedObject);

        /// <summary>
        /// Adds an enterprise management relationship object to the transaction.
        /// </summary>
        /// <param name="relationshipObject">RelationshipObject to add to transaction.</param>
        void Add(IRelationshipObject relationshipObject);

        /// <summary>
        /// Obliterates the managed object.
        /// </summary>
        /// <param name="managedObj">A reference to the object to be remove.</param>
        void Remove(IManagedObject managedObj);

        /// <summary>
        /// Commits transaction to database.
        /// </summary>
        void Commit();

        /// <summary>
        /// Adds the deletion of a relationship object to the transaction.
        /// </summary>
        /// <param name="relationshipObject">Relationship to delete.</param>
        void Remove(IRelationshipObject relationshipObject);
    }
}
