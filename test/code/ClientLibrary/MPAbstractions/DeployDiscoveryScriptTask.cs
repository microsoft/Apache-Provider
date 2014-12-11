//-----------------------------------------------------------------------
// <copyright file="DeployDiscoveryScriptTask.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core;

    /// <summary>
    /// Represents the Microsoft.Unix.DiscoveryScript.Deploy.Task found in the UNIX library MP.
    /// </summary>
    public class DeployDiscoveryScriptTask : Task, IDeployDiscoveryScriptTask
    {
        /// <summary>
        /// Initializes a new instance of the DeployDiscoveryScriptTask class.
        /// </summary>
        public DeployDiscoveryScriptTask()
            : base("Microsoft.Unix.DiscoveryScript.Deploy.Task")
        {
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
        /// Gets or sets the SourceFile overridable parameter. This is the full local path to the directory where the script resides on the management server.
        /// </summary>
        public string SourceFile { get; set; }

        /// <summary>
        /// Executes the deployment task.
        /// </summary>
        /// <param name="managementGroupConnection">Handle to the OpsMgr SDK.</param>
        /// <param name="managementActionPoint">Target health service to execute on.</param>
        /// <returns>Output of the task.</returns>
        public IRemoteCmdTaskResult Execute(IManagementGroupConnection managementGroupConnection, IManagedObject managementActionPoint)
        {
            this.OverrideParameter("Host", this.Host);
            this.OverrideParameter("Port", this.Port.ToString());

            if (Credentials.CredentialsForAny(CredentialUsage.SshDiscovery | CredentialUsage.SshSudoElevation) == PosixHostCredential.Empty)
            {
                throw new ArgumentException(Strings.SshDiscoveryTask_Credentials_do_not_support_SSH, "Credential");
            }

            if (!string.IsNullOrEmpty(this.Credentials.SshUserName))
            {
                this.OverrideParameter("UserName", this.Credentials.GetXmlUserName(CredentialUsage.SshDiscovery));
                this.OverrideParameter("Password", this.Credentials.GetXmlPassword(CredentialUsage.SshDiscovery));
            }
            this.OverrideParameter("SourceFile", this.SourceFile);

            string result = this.DoExecute(managementGroupConnection, managementActionPoint);
            return new SSHTaskResult(result);
        }
    }
}
