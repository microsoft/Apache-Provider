// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGetUnixComputer.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
//   Defines the IGetUnixComputer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System;
    using System.Collections.Generic;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;

    /// <summary>
    ///     This defines the interface for workflow actions that return
    ///     SCX agensts.
    /// </summary>
    public interface IGetUnixComputer
    {
        /// <summary>
        ///     Iterate through a list of host names, return the agent objects.
        /// </summary>
        /// 
        /// <param name="hostnames">A collection of host names.</param>
        /// <param name="managementGroupConnection">The management group connection to use.</param>
        /// 
        /// <returns>
        ///     This method will return a list of SCX agents for
        ///     each of the given hostnames managed by the management groupConnection.
        /// </returns>
        IEnumerable<IPersistedUnixComputer> Invoke(IEnumerable<string> hostnames, IManagementGroupConnection managementGroupConnection);

        /// <summary>
        ///     Iterate through a list of host names, return the agent objects.
        /// </summary>
        /// 
        /// <param name="hostnames">A collection of host names.</param>
        /// <param name="managementGroupConnection">The management group connection to use.</param>
        /// <param name="isRegularExpression">Indicate if the input is a regular expression.</param>
        /// 
        /// <returns>
        ///     This method will return a list of SCX agents for
        ///     each of the given hostnames.
        /// </returns>
        IEnumerable<IPersistedUnixComputer> Invoke(IEnumerable<string> hostnames, IManagementGroupConnection managementGroupConnection, bool isRegularExpression);


        /// <summary>
        ///     return all the agent objects managed by the given resource pool.
        /// </summary>
        /// 
        /// <param name="managementGroupConnection">The managementgroup to connect to.</param>
        /// <param name="id">The id of the resource pool to query.</param>
        /// 
        /// <returns>
        ///     This method will return a list of SCX agents for the given resource pool.
        /// </returns>
        IEnumerable<IPersistedUnixComputer> Invoke(IManagementGroupConnection managementGroupConnection, Guid id);

        /// <summary>
        ///     return all the agent objects managed by the given management server.
        /// </summary>
        /// 
        /// <param name="managementGroupConnection">The managementgroup to connect to.</param>
        /// <param name="managementServerName">The name of the management server to query.</param>
        /// 
        /// <returns>
        ///     This method will return a list of SCX agents for the given management server.
        /// </returns>
        IEnumerable<IPersistedUnixComputer> GetUnixComputerManagedByManagementServer(IManagementGroupConnection managementGroupConnection, string managementServerName);

        /// <summary>
        ///     return all the agent objects managed by the given management server.
        /// </summary>
        /// 
        /// <param name="managementGroupConnection">The management groupConnection to connect to.</param>
        /// <param name="resourcePoolName">The name of the resource pool to query.</param>
        /// 
        /// <returns>
        ///     This method will return a list of SCX agents for the given resource pool.
        /// </returns>
        IEnumerable<IPersistedUnixComputer> Invoke(IManagementGroupConnection managementGroupConnection, string resourcePoolName);

        /// <summary>
        ///     return all the agent objects managed by the given ID.
        /// </summary>
        /// 
        /// <param name="ids">A collection of host Ids.</param>
        /// <param name="managementGroupConnection">The management groupConnection to connect to.</param>
        /// <returns>
        ///     This method will return a list of SCX agents for the Ids.
        /// </returns>
        IEnumerable<IPersistedUnixComputer> Invoke(IEnumerable<Guid> ids, IManagementGroupConnection managementGroupConnection);

        /// <summary>
        ///     return all the agent objects managed.
        /// </summary>
        /// <param name="managementGroupConnection">The management groupConnection to connect to.</param>
        /// <returns>
        ///     This method will return a list of SCX agents.
        /// </returns>
        IEnumerable<IPersistedUnixComputer> Invoke(IManagementGroupConnection managementGroupConnection);
    }
}
