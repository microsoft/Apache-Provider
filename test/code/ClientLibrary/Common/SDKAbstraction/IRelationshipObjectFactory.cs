//-----------------------------------------------------------------------
// <copyright file="IRelationshipObjectFactory.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using System;
    using System.Collections.Generic;
    
    /// <summary>
    /// Creates objects that relate enterprise management objects together. Could for example be used for a "ManagedBy" relationship.
    /// </summary>
    public interface IRelationshipObjectFactory
    {
        /// <summary>
        /// Creates an SDK representation of a relationship between the two parameters.
        /// </summary>
        /// <param name="source">Source object of relationship.</param>
        /// <param name="target">Target object of relationship.</param>
        /// <returns>The SDK representation of this relationship.</returns>
        IRelationshipObject CreateRelationshipObject(IManagedObject source, IManagedObject target);

        /// <summary>
        /// Returns a unique existing relationship by target.
        /// </summary>
        /// <param name="target">Target for relationships.</param>
        /// <returns>A relationship object with target as target.</returns>
        IRelationshipObject GetExistingRelationshipWhereTarget(IManagedObject target);

        IList<IRelationshipObject> GetExistingRelationships();

        /// <summary>
        /// Returns a unique existing relationship by id.
        /// </summary>
        /// <param name="id">source id for relationships.</param>
        /// <returns>A set of relationship objects whose the source object has the given id.</returns>
        IEnumerable<IRelationshipObject> GetAllRelatedObjects(Guid id);
    }
}
