//-----------------------------------------------------------------------
// <copyright file="IWSManDiscoveryTask.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core;

    /// <summary>
    /// Represents a WSMan Discovery Task.
    /// </summary>
    public interface IWSManDiscoveryTask
    {
        /// <summary>
        /// Gets or sets the TargetSystem overridable parameter which is the system to discover.
        /// </summary>
        string TargetSystem { get; set; }

        /// <summary>
        /// Gets or sets the credential to be used for discovery via wsman.
        /// </summary>
        CredentialSet Credential { get; set; }

        /// <summary>
        /// Gets or sets the TimeOutInMS overridable parameter which controls WinRM timeout.
        /// </summary>
        int TimeoutInMS { get; set; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="managementGroupConnection">The ManagementGroupConnection to execute on.</param>
        /// <param name="managementActionPoint">Target health service to execute on.</param>
        /// <returns>The results of the task execution.</returns>
        IWSManDiscoveryTaskResult Execute(IManagementGroupConnection managementGroupConnection, IManagedObject managementActionPoint);
    }
}
