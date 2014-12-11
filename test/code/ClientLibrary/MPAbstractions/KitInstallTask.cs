//-----------------------------------------------------------------------
// <copyright file="KitInstallTask.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;
    using ClientTasks;
    using Common.SDKAbstraction;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core;

    /// <summary>
    /// Class representing the MP task to install an SCX Kit on some target platform.
    /// </summary>
    public class KitInstallTask : Task, IKitInstallTask
    {
        #region Lifecycle

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions.KitInstallTask">KitInstallTask</see>
        ///     class.
        /// </summary>
        /// <param name="supportedAgent">Supported agent data for which to create a task.</param>
        public KitInstallTask(ISupportedAgent supportedAgent) :
            base(String.Empty)
        {
            if (null == supportedAgent)
            {
                throw new ArgumentNullException("supportedAgent");
            }

            this.Timeout = TimeSpan.FromMinutes(5.0);
            this.supportedAgent   = supportedAgent;
        }

        #endregion Lifecycle

        #region Properties (4)

        /// <summary>
        ///     Gets or sets the set of PosixHostCredentials to use. If a separate elevated
        ///     credential is required from the SSH login, add it to this set with the
        ///     appropriate CredentialUsage.
        /// </summary>
        /// <remarks>
        ///     The implementation(s) of this interface adheres to the following behavior
        ///     by default. The set is queried for elevation credentials after having 
        ///     logged in to the system and will attempt to use <em>only</em> the returned
        ///     credentials to install the kit. Failing that, no elevation attempt will be
        ///     attempted.
        /// </remarks>
        public CredentialSet CredentialSet { get; set; }

        /// <summary>
        /// Gets or sets the name of the package to install.
        /// </summary>
        /// <remarks>
        /// Looks like the working dir/path is fixed by task.
        /// </remarks>
        /// <value>
        /// The name of the package to install, including any version and extension
        /// decorations.
        /// </value>
        public string PackageName
        {
            get; set;
        }

        /// <summary>
        ///     Gets or sets the hostname of the endpoint to install the kit to.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        ///     Gets or sets the port to use for SSH.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the amount of time that the task will wait before it balks.
        /// </summary>
        /// <remarks>
        /// Default timeout is set to five minutes.
        /// </remarks>
        /// <value>
        /// TimeSpan corresponding to amount of time to wait.
        /// </value>
        /// <seealso cref="T:System.Time.TimeSpan">TimeSpan</seealso>
        public TimeSpan Timeout { get; set; }

        #endregion Properties 
   
        #region Methods (1)

        /// <summary>
        ///     Executes the installation orders of a previously copied SCX kit at the 
        ///     endpoint.
        /// </summary>
        /// <param name="managementGroupConnection">The ManagementGroupConnection to execute on.</param>
        /// <param name="managementActionPoint">The heath service object to be used.</param>
        /// <returns>
        ///     An instance of SSHTaskResult that encapsulates the last known exit
        ///     code, as well as the lossage from both stdout/stderr.
        /// </returns>
        public IRemoteCmdTaskResult Execute(IManagementGroupConnection managementGroupConnection, IManagedObject managementActionPoint)
        {
            if (managementActionPoint == null)
            {
                throw new ArgumentNullException("managementActionPoint");
            }

            string taskName = this.ResultantTaskNameForAgent();
            SetTaskName(taskName);

            this.SetParameterOverrides();

            string res = this.DoExecute(managementGroupConnection, managementActionPoint);

            return new SSHTaskResult(res);
        }

        /// <summary>   
        ///     Resultant task name for agent or Empty if not supported.
        /// </summary>
        /// <returns>
        ///     The name of the task appropriate to the agentInfo given, or
        ///     String.Empty if it isn't supported.
        /// </returns>
        private string ResultantTaskNameForAgent()
        {
            int taskPrefixLen = this.supportedAgent.SupportedManagementPackClassName.LastIndexOf(".");
            string taskPrefix = this.supportedAgent.SupportedManagementPackClassName.Substring(0, taskPrefixLen);

            var s = String.Format("{0}.Agent.Install.Task", taskPrefix);

            return s;
        }

        /// <summary>
        ///     Sets the parameter overrides.
        /// </summary>
        private void SetParameterOverrides()
        {
            if (String.IsNullOrWhiteSpace(this.Host))
            {
                throw new ArgumentNullException("Host", Strings.KitInstallTask_SetParameterOverrides_Hostname_cannot_be_null_or_empty);
            }

            if (this.Port <= 0)
            {
                throw new ArgumentException("Port", Strings.KitInstallTask_SetParameterOverrides_Port_for_ssh_must_be_positive_integer);
            }

            if (String.IsNullOrWhiteSpace(this.PackageName))
            {
                throw new ArgumentNullException("PackageName", Strings.KitInstallTask_SetParameterOverrides_Package_name_cannot_be_null_or_empty);
            }

            OverrideParameter("Host", this.Host);
            OverrideParameter("Port", this.Port.ToString());

            if (!string.IsNullOrEmpty(this.CredentialSet.SshUserName))
            {
                OverrideParameter("UserName", this.CredentialSet.GetXmlUserName(CredentialUsage.SshDiscovery));
                OverrideParameter("Password", this.CredentialSet.GetXmlPassword(CredentialUsage.SshDiscovery));
            }
            OverrideParameter("InstallPackage", this.PackageName);
            OverrideParameter("TimeoutSeconds", String.Format("{0}", (uint)this.Timeout.TotalSeconds));
        }

        #endregion Methods

        private readonly ISupportedAgent supportedAgent;
    }
}
