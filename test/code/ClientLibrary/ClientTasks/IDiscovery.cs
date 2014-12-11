//-----------------------------------------------------------------------
// <copyright file="IDiscovery.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System.Collections.Generic;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;

    /// <summary>
    ///     This defines the interface for workflow tasks that discover unmanaged
    ///     hosts on a network that is part of a SystemCenter OperationsManager
    ///     environment.
    /// </summary>
    public interface IDiscovery
    {
        /// <summary>
        ///     Iterate through a list of criteria and attempt to determine what,
        ///     if anything, we know about the host matching each criterion.
        /// </summary>
        ///
        /// <param name="criteria">A collection of discovery criteria that can be used to probe the network for unmanaged hosts.</param>
        /// <param name="managementGroupConnection">The ManagementGroupConnection to connect with.</param>
        /// <param name="managementActionPoint">Health service or resource pool to execute tasks on.</param>
        ///
        /// <returns>
        ///     This method will return an enumerable collection containing the
        ///     hostnames of each host that was unmanaged.
        /// </returns>
        IEnumerable<DiscoveryResult> Invoke(IList<DiscoveryCriteria> criteria, IManagementGroupConnection managementGroupConnection, string managementActionPoint);

        /// <summary>
        ///     Iterate through a list of criteria and attempt to determine what,
        ///     if anything, we know about the host matching each criterion.
        /// </summary>
        /// 
        /// <param name="criteria">A collection of discovery criteria that can be used to probe the network for unmanaged hosts.</param>
        /// <param name="managementGroupConnection">The ManagementGroupConnection to connect with.</param>
        /// <param name="managementActionPoint">Health service or resource pool to execute tasks on.</param>
        /// 
        /// <returns>
        ///     This method will return an enumerable collection containing the
        ///     hostnames of each host that was unmanaged.
        /// </returns>
        IEnumerable<DiscoveryResult> Invoke(
                                        IList<DiscoveryCriteria> criteria,
                                        IManagementGroupConnection managementGroupConnection, 
                                        IManagedObject managementActionPoint);

        /// <summary>
        /// The helper class for showing/cancelling progress
        /// </summary>
        ProgressHelper Helper { get; }
    }
}
