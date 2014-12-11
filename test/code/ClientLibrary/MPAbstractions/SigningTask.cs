//-----------------------------------------------------------------------
// <copyright file="SigningTask.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core;

    /// <summary>
    /// Abstracts the various versions of the Management Pack Signing task.
    /// </summary>
    public class SigningTask : Task
    {
        /// <summary>
        /// Handle for tracing.
        /// </summary>
        private static readonly TraceSource traceSource = new TraceSource("Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions");

        /// <summary>
        /// Initializes a new instance of the SigningTask class.
        /// </summary>
        public SigningTask()
            : base("Microsoft.Unix.Agent.GetCert.Task")
        {
        }

        /// <summary>
        /// Gets or sets the Host overridable parameter of the task.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the Port overridable parameter of the task.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the credentials (SSH and optionally elevation) to be used for signing.
        /// </summary>
        public CredentialSet Credentials { get; set; }

        /// <summary>
        /// Executes the task on a particular management groupConnection using targetHost as target.
        /// </summary>
        /// <param name="managementGroupConnection">ManagementGroupConnection on which to execute the task.</param>
        /// <param name="managementActionPoint">The health service to be used.</param>
        /// <returns>Results of the task.</returns>
        public virtual IRemoteCmdTaskResult Execute(IManagementGroupConnection managementGroupConnection, IManagedObject managementActionPoint)
        {
            if (managementActionPoint == null)
            {
                throw new ArgumentNullException("managementActionPoint");
            }

            this.OverrideParameter("Host", this.Host);
            this.OverrideParameter("Port", this.Port.ToString(CultureInfo.InvariantCulture));

            if (!string.IsNullOrEmpty(this.Credentials.SshUserName))
            {
                this.OverrideParameter("UserName", this.Credentials.GetXmlUserName(CredentialUsage.SshDiscovery));
                this.OverrideParameter("Password", this.Credentials.GetXmlPassword(CredentialUsage.SshDiscovery));
            }
            traceSource.TraceEvent(TraceEventType.Information, 33, "Executing Signing task for host '{0}'.", this.Host);
            string result = DoExecute(managementGroupConnection, managementActionPoint);
            traceSource.TraceEvent(TraceEventType.Information, 34, "Done executing signing task for host '{0}'.", this.Host);
            return new SSHTaskResult(result);
        }
    }
}
