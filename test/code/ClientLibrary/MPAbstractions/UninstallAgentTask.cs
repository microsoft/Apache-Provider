//-----------------------------------------------------------------------
// <copyright file="UninstallAgentTask.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core;

    /// <summary>
    /// This class implements the task to uninstall the SCX agent from a managed host.
    /// </summary>
    public class UninstallAgentTask : Task, IUninstallAgentTask
    {
        /// <summary>
        /// Gets the name of the task that uninstalls the SCX agent for a given computer type.
        /// </summary>
        /// <param name="unixComputer">MP name of a computer type. E.g. Microsoft.Linux.SLES.10.Computer</param>
        /// <returns>the string that in the MP defines the uninstall task. E.g. Microsoft.Linux.SLES.10.Agent.Uninstall.AdminPage.Task</returns>
        public static string GetTaskName(IPersistedUnixComputer unixComputer)
        {
            string taskBaseName = unixComputer.ManagementPackPlatformIdentifier;
            return string.Format(
                CultureInfo.InvariantCulture, "{0}.Agent.Uninstall.Task", taskBaseName);
        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="managementGroupConnection">ManagementGroupConnection on which to execute task.</param>
        /// <returns>Remote execution result.</returns>
        public IRemoteCmdTaskResult Execute(IManagementGroupConnection managementGroupConnection)
        {
            this.OverrideParameter("Host", this.unixComputer.Name);
            this.OverrideParameter("Port", this.unixComputer.SSHPort.ToString(CultureInfo.InvariantCulture));

            if (this.credentials != null && (this.credentials.CredentialsForAny(CredentialUsage.SshDiscovery | CredentialUsage.SshSudoElevation)
                != PosixHostCredential.Empty))
            {
                if (!string.IsNullOrEmpty(this.credentials.SshUserName))
                {
                    OverrideParameter("UserName", this.credentials.GetXmlUserName(CredentialUsage.SshDiscovery));
                    OverrideParameter("Password", this.credentials.GetXmlPassword(CredentialUsage.SshDiscovery));
                }
            }

            traceSource.TraceInformation(traceFormat, @"executing task.");
            return new SSHTaskResult(this.DoExecute(managementGroupConnection, this.unixComputer.ManagedObject));
        }

        public UninstallAgentTask(IPersistedUnixComputer unixComputer, CredentialSet credentials)
            : base(string.Empty)
        {
            string taskName = GetTaskName(unixComputer);

            this.traceFormat = String.Format(@"UninstallAgentTask: task {0} on {1}:{2} - {{0}}", taskName, unixComputer.Name, unixComputer.SSHPort);

            traceSource.TraceInformation(traceFormat, @"task created.");

            this.SetTaskName(taskName);
            this.unixComputer = unixComputer;
            this.credentials  = credentials;
        }

        private static readonly TraceSource     traceSource = new TraceSource("Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions");

        private readonly IPersistedUnixComputer unixComputer;
        private readonly CredentialSet          credentials;
        private readonly string                 traceFormat;
   }
}