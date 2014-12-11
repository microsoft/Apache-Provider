//-----------------------------------------------------------------------
// <copyright file="IDeployDiscoveryScriptTask.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core;

    /// <summary>
    /// Represents the Microsoft.Unix.DiscoveryScript.Deploy.Task found in the UNIX library MP.
    /// </summary>
    public interface IDeployDiscoveryScriptTask
    {
        /// <summary>
        /// Gets or sets the Host overridable parameter. This is the FQDN of the host that the agent will be deployed to.
        /// </summary>
        string Host { get; set; }

        /// <summary>
        /// Gets or sets the Port overridable parameter. This is the SSH port.
        /// </summary>
        int Port { get; set; }

        /// <summary>
        /// Gets or sets the credentials to use for the task.
        /// </summary>
        CredentialSet Credentials { get; set; }

        /// <summary>
        /// Gets or sets the SourceFile overridable parameter. This is the full local path to the directory where the script resides on the management server.
        /// </summary>
        string SourceFile { get; set; }

        /// <summary>
        /// Executes the deployment task.
        /// </summary>
        /// <param name="managementGroupConnection">Handle to the OpsMgr SDK.</param>
        /// <param name="managementActionPoint">Target health service to execute on.</param>
        /// <returns>Output of the task.</returns>
        IRemoteCmdTaskResult Execute(IManagementGroupConnection managementGroupConnection, IManagedObject managementActionPoint);
    }
}
