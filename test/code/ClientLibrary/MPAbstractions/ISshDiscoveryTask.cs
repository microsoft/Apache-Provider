// ----------------------------------------------------------------------------------------------------
// <copyright file="ISshDiscoveryTask.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core;

    /// <summary>
    /// Interface to represent the discovery task for POSIX systems using SSH.
    /// </summary>
    public interface ISshDiscoveryTask
    {
        /// <summary>
        ///         Gets or sets the name of the computer system that this task attempts to target.
        /// </summary>
        /// <remarks>
        ///     The FQDN must be resolvable from the MO
        /// </remarks>
        /// <value>
        ///     This <i>must be</i> the fully-qualified domain name (FQDN) of the host.
        /// </value>
        string Hostname { get; set; }

        /// <summary>
        /// Gets or sets the port number to use for SSH.
        /// </summary>
        /// <value>
        /// Integer. The port number used to talk to sshd on the target. Most of the time,
        /// this is 22.
        /// </value>
        int SSHPort { get; set; }

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
        /// PosixHostCredential containing the credentials to use. See Remarks for more details.
        /// </value>
        CredentialSet Credential { get; set; }

        /// <summary>
        /// Execute the SSH discovery task at the targetHost with the ManagementGroupConnection
        /// given.
        /// </summary>
        /// <param name="managementGroupConnection">The ManagementGroupConnection to execute with.</param>
        /// <param name="managementActionPoint">Target health service to execute on.</param>
        /// <returns>
        /// Returns an ISshDiscoveryTaskResult instance that contains the information gleaned
        /// about the target via this Discovery process.
        /// </returns>
        IRemoteCmdTaskResult Execute(IManagementGroupConnection managementGroupConnection, IManagedObject managementActionPoint);
    }
}
