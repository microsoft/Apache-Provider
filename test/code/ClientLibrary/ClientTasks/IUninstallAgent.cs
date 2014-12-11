//-----------------------------------------------------------------------
// <copyright file="IUninstallAgent.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System.Collections.Generic;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core;

    /// <summary>
    ///     This defines the interface for workflow actions that uninstall the
    ///     SCX agent on System Center Operations Manager managed host.
    /// </summary>
    public interface IUninstallAgent
    {
        /// <summary>
        ///     Iterate through a list of managed hosts, uninstalling the SCX agent.
        /// </summary>
        /// 
        /// <param name="unixComputers">A collection of managed hosts to uninstall.</param>
        /// <param name="credentials">Credentials to use for uninstall, or null to use "RunAs".</param>
        /// <param name="managementGroupConnection">The ManagementGroupConnection to use.</param>
        /// 
        /// <returns>
        ///     This method will return a list containing an uninstall result for
        ///     each of the given hostnames.
        /// </returns>
        IEnumerable<UninstallAgentResult> Invoke(
            IEnumerable<IPersistedUnixComputer> unixComputers,
            CredentialSet                       credentials,
            IManagementGroupConnection managementGroupConnection);

        /// <summary>
        /// The helper class for showing/cancelling progress
        /// </summary>
        ProgressHelper Helper { get; }
    }
}
