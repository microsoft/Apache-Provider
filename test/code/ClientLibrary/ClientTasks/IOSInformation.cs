//-----------------------------------------------------------------------
// <copyright file="IOSInformation.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System;

    /// <summary>
    /// Represents properties of a discovered operating system.
    /// </summary>
    public interface IOSInformation
    {
        /// <summary>
        /// Gets the Operating System Name.
        /// E.g. AIX, HP-UX, Red Hat Enterprise Linux Server, SUSE Linux Enterprise Server.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the Operating System version.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Gets the architecture as reported by uname.
        /// </summary>
        string Architecture { get; }

        /// <summary>
        /// Gets the short Operating System name. E.g. SLES
        /// </summary>
        string Alias { get; }

        /// <summary>
        ///     Gets a value indicating whether the OS is of the Linux constellation of OS.
        /// </summary>
        /// <remarks>
        ///     For some reason, the deployment step needs to know this as it does something
        ///     differently for Linux vs. (all) other Unix systems.
        /// </remarks>
        bool IsLinux { get; }
    }
}
