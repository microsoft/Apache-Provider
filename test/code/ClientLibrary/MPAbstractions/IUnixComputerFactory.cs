//-----------------------------------------------------------------------
// <copyright file="IUnixComputerFactory.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;
    using System.Collections.Generic;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;

    /// <summary>
    /// Factory for creating AddNewHostTask instances.
    /// </summary>
    public interface IUnixComputerFactory
    {
        /// <summary>
        /// Creates a new UnixComputer instance.
        /// </summary>
        /// <param name="computerClassName">the name of the computer class object from the MP</param>
        /// <param name="healthService">Health service that should manage the new computer.</param>
        /// <returns>A new UnixComputer instance.</returns>
        IPersistableUnixComputer CreateUnixComputer(string computerClassName, IManagedObject healthService);

         /// <summary>
        /// Creates a new PersistedUnixComputer instance.
        /// </summary>
        /// <param name="managedObject">the managed object</param>
        /// <returns>A new PersistedUnixComputer instance.</returns>
        IPersistedUnixComputer CreatePersistedUnixComputer(IManagedObject managedObject);

        /// <summary>
        /// Creates a new UnixComputer instance from data already present in OpsMgr database.
        /// </summary>
        /// <param name="computerHostName">The FQDN of the computer</param>
        /// <param name="isRegularExpression">Indicate if the input is a regular expression.</param>
        /// <returns>A new UnixComputer instance.</returns>
        IEnumerable<IPersistedUnixComputer> GetExistingUnixComputer(string computerHostName, bool isRegularExpression);

        /// <summary>
        /// Creates a list of new UnixComputer instances from data already presented in OpsMgr database.
        /// </summary>
        /// <returns>A list of UnixComputer instances.</returns>
        IEnumerable<IPersistedUnixComputer> GetAllExistingUnixComputers();

        /// <summary>
        /// Creates a list of new UnixComputer instances from the given resource pool.
        /// </summary>
        /// <param name="id">The resource pool id</param>
        /// <returns>A list of UnixComputer instances.</returns>
        IEnumerable<IPersistedUnixComputer> GetExistingUnixComputerFromResourcePool(Guid id);

        /// <summary>
        /// Creates a list of new UnixComputer instances from the given management server.
        /// </summary>
        /// <param name="id">The management server id</param>
        /// <returns>A list of UnixComputer instances.</returns>
        IEnumerable<IPersistedUnixComputer> GetExistingUnixComputerFromManagementServer(Guid id);
    }
}
