//-----------------------------------------------------------------------
// <copyright file="IManagedObjectFactory.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using System;
    using System.Collections.Generic;
    using Microsoft.EnterpriseManagement.Common;

    /// <summary>
    /// Creates Enterprise Management Objects. Typically an instance of a class defined in a Management Pack.
    /// </summary>
    public interface IManagedObjectFactory
    {
        /// <summary>
        /// Creates a new ManagedObject.
        /// </summary>
        /// <returns>New EnterpriseManagementObject.</returns>
        IManagedObject CreateNewManagedObject();

        /// <summary>
        /// Creates a ManagedObject from data already in the database.
        /// </summary>
        /// <param name="displayName">Display name to use as key when searching for the object.</param>
        /// <param name="isRegularExpression">Indicate if the input is a regular expression.</param>
        /// <returns>Object retrieved from the OpsMgr SDK.</returns>
        IEnumerable<IManagedObject> GetExistingManagedObject(string displayName, bool isRegularExpression);

        /// <summary>
        /// Returns all existing instances of this type.
        /// </summary>
        /// <returns>Enumeration of all instances of this type.</returns>
        IEnumerable<IManagedObject> GetAllInstances();

        /// <summary>
        /// Creates a ManagedObject from data already in the database.
        /// </summary>
        /// <param name="id">Id to use as key when searching for the object.</param>
        /// <returns>Object retrieved from the OpsMgr SDK.</returns>
        IManagedObject GetExistingManagedObjectById(Guid id);

        /// <summary>
        /// Creates a ManagedObject from a given criteria
        /// </summary>
        /// <param name="criteria">Criteria to use for query the object.</param>
        /// <returns>Object retrieved from the OpsMgr SDK.</returns>
        IManagedObject GetManagedObjectWithCriteria(string criteria);
    }
}
