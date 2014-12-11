//-----------------------------------------------------------------------
// <copyright file="UpdateAgentTask.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System.Globalization;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core;

    /// <summary>
    /// This class implements the task to update the SCX agent on a managed host.
    /// </summary>
    public class UpdateAgentTask : Task, IUpdateAgentTask
    {
        private readonly IPersistedUnixComputer unixComputer;

        /// <summary>
        /// Initializes a new instance of the UpdateAgentTask class.
        /// </summary>
        /// <param name="unixComputer">Computer which to update.</param>
        public UpdateAgentTask(IPersistedUnixComputer unixComputer)
            : base(string.Format(CultureInfo.InvariantCulture, "{0}.Agent.Upgrade.Task", unixComputer.ManagementPackPlatformIdentifier))
        {
            this.unixComputer = unixComputer;
        }

        /// <summary>
        /// Gets or sets the credentials to use for the task.
        /// </summary>
        public CredentialSet Credentials { get; set; }

        /// <summary>
        /// Gets or sets the InstallPackage overridable parameter. This is the filename part of the package file
        /// with any compression extensions removed. E.g. for a Solaris package, the file we transfer is called something
        /// like "scx-1.0.4-248.solaris.10.sparc.pkg.Z" but this parameter should only be "scx-1.0.4-248.solaris.10.sparc.pkg".
        /// </summary>
        public string InstallPackage { get; set; }

        /// <summary>
        /// Executes the update task.
        /// </summary>
        /// <param name="managementGroupConnection">Handle to the OpsMgr SDK.</param>
        /// <returns>The results of the task execution.</returns>
        public IRemoteCmdTaskResult Execute(IManagementGroupConnection managementGroupConnection)
        {
            this.OverrideParameter("Host", this.unixComputer.Name);
            this.OverrideParameter("Port", this.unixComputer.SSHPort.ToString());

            // Use RunAs credential if SSH credential is not provided.
            if (this.Credentials.CredentialsForAny(CredentialUsage.SshDiscovery | CredentialUsage.SshSudoElevation) 
                != PosixHostCredential.Empty)
            {
                if (!string.IsNullOrEmpty(this.Credentials.SshUserName))
                {
                    this.OverrideParameter("UserName", this.Credentials.GetXmlUserName(CredentialUsage.SshDiscovery));
                    this.OverrideParameter("Password", this.Credentials.GetXmlPassword(CredentialUsage.SshDiscovery));
                }
            }

            this.OverrideParameter("InstallPackage", this.InstallPackage);
            this.OverrideParameter("TimeoutSeconds", "120");

            string result = this.DoExecute(managementGroupConnection, this.unixComputer.ManagedObject);
            return new SSHTaskResult(result);
        }
    }
}
