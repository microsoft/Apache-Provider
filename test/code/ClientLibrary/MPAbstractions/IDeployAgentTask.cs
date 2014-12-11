//-----------------------------------------------------------------------
// <copyright file="IDeployAgentTask.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core;

    /// <summary>
    /// Represents the Agent deploy tasks found in the various Unix MPs.
    /// </summary>
    public interface IDeployAgentTask
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
        /// Gets or sets the SourceFile overridable parameter. This is the full local path file to copy on the management server.
        /// </summary>
        string SourceFile { get; set; }

        /// <summary>
        /// Gets or sets the TargetFile overridable parameter. This is the filename part of the destination file.
        /// </summary>
        string TargetFile { get; set; }

        /// <summary>
        /// Executes the deployment task.
        /// </summary>
        /// <param name="managementGroupConnection">Handle to the OpsMgr SDK.</param>
        /// <param name="managementActionPoint">The health service to be used.</param>
        /// <returns>The results of the task execution.</returns>
        IRemoteCmdTaskResult Execute(IManagementGroupConnection managementGroupConnection, IManagedObject managementActionPoint);
    }
}
