//-----------------------------------------------------------------------
// <copyright file="IDeployment.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System.Collections.Generic;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;

    /// <summary>
    /// This interface documents the API for clients to deploy discovered hosts in System Center Operations Manager.
    /// </summary>
    public interface IDeployment
    {
        /// <summary>
        ///     Invokes the steps necessary to deploy the SCX agent on a collection
        ///     of discover results.
        /// </summary>
        /// 
        /// <param name="discoveryResults">A collection of discover results,
        ///     such as those returned by IDiscovery.
        /// </param>
        /// <param name="managementGroupConnection">The ManagementGroupConnection to use.</param>
        /// 
        /// <returns>
        ///     This method will return a list of SCX agents.
        /// </returns>
        IEnumerable<InstallAgentResult> Invoke(IEnumerable<DiscoveryResult> discoveryResults, IManagementGroupConnection managementGroupConnection);

        /// <summary>
        /// The helper class for showing/cancelling progress
        /// </summary>
        ProgressHelper Helper { get; }
    }
}
