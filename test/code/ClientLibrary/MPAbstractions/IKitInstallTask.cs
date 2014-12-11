//-----------------------------------------------------------------------
// <copyright file="IKitInstallTask.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using Common.SDKAbstraction;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core;

    /// <summary>
    /// Interface representing the MP task to install an SCX Kit on some target platform.
    /// </summary>
    public interface IKitInstallTask
    {        
        /// <summary>
        ///     Gets or sets the hostname of the endpoint to install the kit to.
        /// </summary>
        string Host { get; set; }

        /// <summary>
        ///     Gets or sets the port to use for SSH.
        /// </summary>
        int Port { get; set; }
       
        /// <summary>
        ///     Gets or sets the set of PosixHostCredentials to use. If a separate elevated
        ///     credential is required from the SSH login, add it to0 this set with the
        ///     appropriate CredentialUsage.
        /// </summary>
        /// <remarks>
        ///     The implementation(s) of this interface adheres to the following behavior
        ///     by default. The set is queried for elevation credentials after having 
        ///     logged into the system and will attempt to use <em>only</em> the returned
        ///     credentials to install the kit. Failing that, no elevation attempt will be
        ///     attempted.
        /// </remarks>
        CredentialSet CredentialSet { get; set; }

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
        string PackageName { get; set; }

        /// <summary>
        ///     Executes the installation orders of a previously copied SCX kit at the 
        ///     endpoint.
        /// </summary>
        /// <param name="managementGroupConnection">Th ManagementGroupConnection to execute with.</param>
        /// <param name="managementActionPoint">The health service object to be used.</param>
        /// <returns>
        ///     An instance of SSHTaskResult that encapsulates the last known exit
        ///     code, as well as the lossage from both stdout/stderr.
        /// </returns>
        IRemoteCmdTaskResult Execute(IManagementGroupConnection managementGroupConnection, IManagedObject managementActionPoint);
    }
}

