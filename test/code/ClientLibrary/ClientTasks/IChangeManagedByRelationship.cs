//-----------------------------------------------------------------------
// <copyright file="IChangeManagedByRelationship.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System.Collections.Generic;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;

    /// <summary>
    ///     This defines the interface for workflow actions that change the
    ///     System Center Operations Manager managed-by relationship for managed
    ///     hosts .
    /// </summary>
    public interface IChangeManagedByRelationship
    {
        /// <summary>
        ///     Iterate through a list of managed hosts, changing the existing
        ///     managed-by relationship to the new management server.
        /// </summary>
        /// 
        /// <param name="agents">A collection of IPersistedUnixComputer(s) to change.</param>
        /// <param name="managementServerName">The name of the management server of the new managed-by relationship.</param>
        /// <param name="managementGroupConnection">The ManagementGroupConnection to use.</param>
        /// 
        /// <returns>
        ///     This method will return a list containing a change-manage-by
        ///     result for each of the given hostnames.
        /// </returns>
        IEnumerable<ChangeManagedByRelationshipResult> Invoke(IEnumerable<IPersistedUnixComputer> agents, string managementServerName, IManagementGroupConnection managementGroupConnection);

        /// <summary>
        ///     Iterate through a list of managed hosts, changing the existing
        ///     managed-by relationship to the new management server.
        /// </summary>
        /// 
        /// <param name="agents">A collection of IPersistedUnixComputer(s) to change.</param>
        /// <param name="managementActionPoint">The management server of the new managed-by relationship.</param>
        /// <param name="managementGroupConnection">The ManagementGroupConnection to use.</param>
        /// 
        /// <returns>
        ///     This method will return a list containing a change-manage-by
        ///     result for each of the given hostnames.
        /// </returns>
        IEnumerable<ChangeManagedByRelationshipResult> Invoke(IEnumerable<IPersistedUnixComputer> agents, IManagedObject managementActionPoint, IManagementGroupConnection managementGroupConnection);
    }
}