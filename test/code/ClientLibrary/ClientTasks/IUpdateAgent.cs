//-----------------------------------------------------------------------
// <copyright file="IUpdateAgent.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System.Collections.Generic;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core;

    /// <summary>
    /// This interface documents the API for updating the SCX agent on a managed host in a SystemsCenter
    /// OperationsManager environment.
    /// </summary>
    public interface IUpdateAgent
    {
        /// <summary>
        ///     Invokes the steps necessary to update the SCX agent on a collection
        ///     of hosts.
        /// </summary>
        /// 
        /// <param name="hosts">A collection of hosts.</param>
        /// <param name="credentials">The credentials to use for the update.</param>
        /// <param name="managementGroupConnection">The ManagementGroupConnection to use.</param>
        /// 
        /// <returns>
        ///     This method will return a list containing an update result for
        ///     each of the given hostnames.
        /// </returns>
        IEnumerable<UpdateResult> Invoke(IEnumerable<IPersistedUnixComputer> hosts, CredentialSet credentials, IManagementGroupConnection managementGroupConnection);

        /// <summary>
        /// The helper class for showing/cancelling progress
        /// </summary>
        ProgressHelper Helper { get; }
    }
}
