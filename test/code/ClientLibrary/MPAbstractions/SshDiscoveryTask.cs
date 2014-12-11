// ----------------------------------------------------------------------------------------------------
// <copyright file="SshDiscoveryTask.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;
    using System.Diagnostics;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core;

    /// <summary>
    ///     Implementation of SSH Script Based Discovery
    /// </summary>
    public class SshDiscoveryTask : Task, ISshDiscoveryTask
    {
        #region Constructors (2) 

        /// <summary>
        /// Initializes a new instance of the SshDiscoveryTask class.
        /// </summary>
        public SshDiscoveryTask() : 
            base(TaskName)
        {
            this.SSHPort = DefaultSSHPort;
            this.Hostname = String.Empty;
            this.Credential = new CredentialSet();
        }      

        #endregion Constructors 

        #region Fields (2) 

        /// <summary>
        ///     Name of the task (see the Management Pack)
        /// </summary>
        private const string TaskName = "Microsoft.Unix.DiscoveryScript.Discovery.Task";

        /// <summary>
        ///     Default port number to use to connect (see /etc/services).
        /// </summary>
        private const int DefaultSSHPort = 22;

        /// <summary>
        ///     Constant that represents the default timeout for this task.
        /// </summary>
        /// <value>
        /// The default timeout (time to wait before it's assumed the task has failed)
        /// before termination.
        /// </value>
        /// <seealso cref="T:System.TimeSpan">TimeSpan</seealso>
        private static TimeSpan defaultTimeout = new TimeSpan(0, 0, 20);

        #endregion Fields 

        #region Implementation of ISshDiscoveryTask

        /// <summary>
        /// Gets or sets the name of the computer system that this task attempts to target.
        /// </summary>
        /// <remarks>
        /// The FQDN must be resolvable from the MO
        /// </remarks>
        /// <value>
        /// This <i>must be</i> the fully-qualified domain name (FQDN) of the host.
        /// </value>
        public string Hostname
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the port number to use for SSH.
        /// </summary>
        /// <value>
        /// Integer. The port number used to talk to sshd on the target. Most of the time,
        /// this is 22.
        /// </value>
        public int SSHPort
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the credential to be used for discovery via SSH.
        /// </summary>
        /// <remarks>
        /// Depending on how your infra is set up, this credential may be used differently
        /// at the target executing the task. Common SSH configurations allow non-root login
        /// via SSH using simple username/password; in this case, the PrincipalName and
        /// PassPhrase are used in that manner.
        /// <para></para>
        /// <para>When SSH is configured for certificate-based authentication (<i>only,
        /// without fallback to password</i>), the PrincipalName is used as the username. In
        /// this case however, the PassPhrase is used to access the private key of the
        /// certificate associated with the Principal at the host (e.g.
        /// ~PrincipalName/.ssh/authorized_keys2 file on Unix and
        /// %HOMEDRIVE%%HOMEPATH%\ssh\id_pub.pvk) so the monitoring object executing the
        /// discovery task can prove its identity to the remote host.</para>
        /// </remarks>
        /// <value>
        /// CredentialSet containing the credentials to use. See Remarks for more details.
        /// </value>
        public CredentialSet Credential
        {
            get; set;
        }

        /// <summary>
        ///     Execute the SSH discovery task at the targetHost within the management groupConnection
        ///     given.
        /// </summary>
        /// <param name="managementGroupConnection">Name of the ManagementGroupConnection.</param>
        /// <param name="managementActionPoint">Target health service to execute on.</param>
        /// <returns>
        ///     Returns an SshDiscoveryTaskResult instance that contains the information gleaned
        ///     about the target via this Discovery process.
        /// </returns>
        public IRemoteCmdTaskResult Execute(IManagementGroupConnection managementGroupConnection, IManagedObject managementActionPoint)
        {
            Trc.TraceInformation(@"Executing SSH Discovery for {0}", this.Hostname);
            this.SetParameterOverrides();

            string xmlresult = this.DoExecute(managementGroupConnection, managementActionPoint);
            Trc.TraceInformation("SSH Discovery for '{0}' Returned:\n{1}\n", this.Hostname, xmlresult);

            return new SSHTaskResult(xmlresult);
        }

        #endregion

        #region Internal Helpers

        /// <summary>
        /// Programs the task parameters into the overridable dict of the task to be passed
        /// to the MP.
        /// </summary>
        /// <exception cref="T:System.ArgumentNullException">This exception is thrown if any
        /// argument or parameter is <see langword="null"/> and cannot be. Nullable
        /// parameters are replaced with a reasonable default.</exception>
        /// <exception cref="T:System.ArgumentException">T:System.ArgumentException</exception>
        private void SetParameterOverrides()
        {
            if (String.IsNullOrWhiteSpace(this.Hostname))
            {
                throw new ArgumentException(Strings.SshDiscoveryTask_Hostname_null_or_empty, "Hostname");
            }

            OverrideParameter("Host", this.Hostname);
            Trc.TraceInformation("    SSH Discovery.Host    = {0}", this.Hostname);

            if (this.SSHPort < 0)
            {
                throw new ArgumentException(Strings.SshDiscoveryTask_SSHPort_Negative, "SSHPort");
            }

            OverrideParameter("Port", String.Format("{0}", this.SSHPort == 0 ? DefaultSSHPort : this.SSHPort));
            Trc.TraceInformation("    SSH Discovery.Port    = {0}", this.SSHPort == 0 ? DefaultSSHPort : this.SSHPort);

            // Break up into user and pass from cred
            if (null == this.Credential)
            {
                throw new ArgumentException(Strings.SshDiscoveryTask_Credential_Null_or_Empty, "Credential");
            }

            if (Credential.CredentialsForAny(CredentialUsage.SshDiscovery | CredentialUsage.SshSudoElevation) == PosixHostCredential.Empty)
            {
                throw new ArgumentException(Strings.SshDiscoveryTask_Credentials_do_not_support_SSH, "Credential");
            }

            if (!string.IsNullOrEmpty(this.Credential.SshUserName))
            {
                OverrideParameter("UserName", this.Credential.GetXmlUserName(CredentialUsage.SshDiscovery));
                OverrideParameter("Password", this.Credential.GetXmlPassword(CredentialUsage.SshDiscovery));
            }
            Trc.TraceInformation("    SSH Discovery.Cred    = {0} / *****", this.Credential.GetXmlUserName(CredentialUsage.SshDiscovery));

            this.OverrideParameter(
                "TimeoutSeconds",
                String.Format("{0:#####}", defaultTimeout.TotalSeconds));
        }

        #endregion Internal Helpers

        #region Fields

        /// <summary>
        /// Handle for tracing.
        /// </summary>
        private static readonly TraceSource Trc = new TraceSource("Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions");

        #endregion Fields
    }
}
