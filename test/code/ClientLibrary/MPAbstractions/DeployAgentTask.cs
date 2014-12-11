//-----------------------------------------------------------------------
// <copyright file="DeployAgentTask.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core;

    /// <summary>
    /// Represents the Agent deploy tasks found in the various Unix MPs.
    /// </summary>
    public class DeployAgentTask : Task, IDeployAgentTask
    {
        /// <summary>
        /// Initializes a new instance of the DeployAgentTask class.
        /// </summary>
        /// <param name="isLinux">The class will invoke a different task based on this value.</param>
        /// <param name="operatingSystem">Operating system name as represented in the MP task name.</param>
        /// <param name="operatingSystemVersion">Operating system version as represented in the MP task name.</param>
        /// <param name="architecture">System architecture as represented in the MP task name.</param>
        /// <param name="isInstallation">Flag indicating if this task is used for installation or upgrade.</param>
        public DeployAgentTask(bool isLinux, string operatingSystem, string operatingSystemVersion, string architecture, bool isInstallation)
            : base(string.Empty)
        {
            string linuxString = isLinux ? ".Linux" : string.Empty;

            string deployment = isInstallation ? string.Empty : "Upgrade.";

            string taskName = string.Format(
                "Microsoft{0}.{1}.{2}.{3}.Agent.{4}Deploy.Task",
                linuxString,
                operatingSystem,
                operatingSystemVersion,
                architecture,
                deployment);

            this.SetTaskName(taskName);
        }

        /// <summary>
        /// Gets or sets the Host overridable parameter. This is the FQDN of the host that the agent will be deployed to.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the Port overridable parameter. This is the SSH port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the credentials to use for the task.
        /// </summary>
        public CredentialSet Credentials { get; set; }

        /// <summary>
        /// Gets or sets the SourceFile overridable parameter. This is the full local path file to copy on the management server.
        /// </summary>
        public string SourceFile { get; set; }

        /// <summary>
        /// Gets or sets the TargetFile overridable parameter. This is the filename part of the destination file.
        /// </summary>
        public string TargetFile { get; set; }

        /// <summary>
        /// Executes the deployment task.
        /// </summary>
        /// <param name="managementGroupConnection">Handler to access the OpsMgr SDK.</param>
        /// <param name="managementActionPoint">The target EMO.</param>
        /// <returns>The results of the task execution.</returns>
        public IRemoteCmdTaskResult Execute(IManagementGroupConnection managementGroupConnection, IManagedObject managementActionPoint)
        {
            if (managementActionPoint == null)
            {
                throw new ArgumentNullException("managementActionPoint");
            }

            this.OverrideParameter("Host", this.Host);
            this.OverrideParameter("Port", this.Port.ToString());

            // if the SSH credential is not provided, use RunAs credential
            if (this.Credentials.CredentialsForAny(CredentialUsage.SshDiscovery | CredentialUsage.SshSudoElevation)
                != PosixHostCredential.Empty)
            {
                if (!string.IsNullOrEmpty(this.Credentials.SshUserName))
                {
                    this.OverrideParameter("UserName", Credentials.GetXmlUserName(CredentialUsage.SshDiscovery));
                    this.OverrideParameter("Password", Credentials.GetXmlPassword(CredentialUsage.SshDiscovery));
                }
            }

            this.OverrideParameter("SourceFile", this.SourceFile);
            this.OverrideParameter("TargetFile", this.TargetFile);

            string result = this.DoExecute(managementGroupConnection, managementActionPoint);
            return new SSHTaskResult(result);
        }
    }
}
