//-----------------------------------------------------------------------
// <copyright file="WSManDiscoveryTaskBase.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System.Diagnostics;
    using System.Globalization;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core;

    /// <summary>
    /// Represents the base of a WSMan Discovery Task found in the Unix library MP.
    /// </summary>
    public abstract class WSManDiscoveryTaskBase : Task, IWSManDiscoveryTask
    {
        /// <summary>
        /// Handle for tracing.
        /// </summary>
        private static readonly TraceSource Trace = new TraceSource("Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions");

        /// <summary>
        /// Initializes a new instance of the WSManDiscoveryTaskBase class.
        /// </summary>
        protected WSManDiscoveryTaskBase(string mpTaskName) :
            base(mpTaskName)
        {
        }

        /// <summary>
        /// Gets or sets the credential to be used for discovery via wsman.
        /// </summary>
        public CredentialSet Credential { get; set; }

        /// <summary>
        /// Gets or sets the TargetSystem overridable parameter which is the system to discover.
        /// </summary>
        public string TargetSystem { get; set; }

        /// <summary>
        /// Gets or sets the TimeOutInMS overridable parameter which controls WinRM timeout.
        /// </summary>
        public int TimeoutInMS { get; set; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="managementGroupConnection">ManagementGroupConnection to execute on.</param>
        /// <param name="managementActionPoint">Target health service to execute on.</param>
        /// <returns>The results of the task execution.</returns>
        public IWSManDiscoveryTaskResult Execute(IManagementGroupConnection managementGroupConnection, IManagedObject managementActionPoint)
        {
            // Directly use the wsman user name at this moment.
            if (this.Credential.CredentialsForAny(CredentialUsage.WsManDiscovery) != PosixHostCredential.Empty)
            {
                var wsmanCred = this.Credential.CredentialsForAny(CredentialUsage.WsManDiscovery);
                if (!string.IsNullOrEmpty(wsmanCred.PrincipalName))
                {
                    this.OverrideParameter("UserName", wsmanCred.PrincipalName);
                    this.OverrideParameter("Password", wsmanCred.Passphrase);
                }
            }

            this.OverrideParameter("TargetSystem", this.TargetSystem);
            this.OverrideParameter("TimeOutInMS", this.TimeoutInMS.ToString(CultureInfo.InvariantCulture));

            Trace.TraceEvent(TraceEventType.Information, 11, "Executing wsman discovery task for host '{0}'.", this.TargetSystem);
            string result = DoExecute(managementGroupConnection, managementActionPoint);
            Trace.TraceEvent(TraceEventType.Information, 12, "Done executing wsman discovery task for host '{0}'.", this.TargetSystem);
            return new WSManDiscoveryTaskResult(result, this.TargetSystem);
        }
    }
}
