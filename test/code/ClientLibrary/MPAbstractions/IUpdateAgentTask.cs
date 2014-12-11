//-----------------------------------------------------------------------
// <copyright file="IUpdateAgentTask.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core;

    /// <summary>
    /// This interface abstracts the activities for the Update task found in the various Unix/Linux MPs.
    /// </summary>
    public interface IUpdateAgentTask
    {
        /// <summary>
        /// Gets or sets the credentials to use for the task.
        /// </summary>
        CredentialSet Credentials { get; set; }

        /// <summary>
        /// Gets or sets the InstallPackage overridable parameter. This is the filename part of the package file
        /// with any compression extensions removed. E.g. for a Solaris package, the file we transfer is called something
        /// like "scx-1.0.4-248.solaris.10.sparc.pkg.Z" but this parameter should only be "scx-1.0.4-248.solaris.10.sparc.pkg".
        /// </summary>
        string InstallPackage { get; set; }

        /// <summary>
        /// Executes the deployment task.
        /// </summary>
        /// <param name="managementGroupConnection">Handle to the OpsMgr SDK.</param>
        /// <returns>The results of the task execution.</returns>
        IRemoteCmdTaskResult Execute(IManagementGroupConnection managementGroupConnection);
    }
}
