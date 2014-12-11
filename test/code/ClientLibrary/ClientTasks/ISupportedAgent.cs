//-----------------------------------------------------------------------
// <copyright file="ISupportedAgent.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.Utilities;

    /// <summary>
    /// Represents supported agent information found in Management Packs.
    /// </summary>
    public interface ISupportedAgent
    {
        /// <summary>
        /// Gets the supported agent version including build number.
        /// </summary>
        UnixAgentVersion AgentVersion { get; }

        /// <summary>
        /// Gets the Operating System alias.
        /// </summary>
        string OS { get; }

        /// <summary>
        /// Gets the architecture.
        /// </summary>
        string Architecture { get; }

        /// <summary>
        /// Gets the extension of the kit name. E.g. rpm.
        /// </summary>
        string KitExtension { get; }

        /// <summary>
        /// Gets the minimum version of the OS that this kit supports.
        /// </summary>
        OperatingSystemVersion MinimumSupportedOSVersion { get; }

        /// <summary>
        /// Gets the architecture name.
        /// </summary>
        string ArchitectureFriendlyName { get; }

        /// <summary>
        /// Gets the architecture name as represented in the kit filename.
        /// </summary>
        string KitNameArchitecture { get; }

        /// <summary>
        /// Gets the name of the ManagementPack class that is used to monitor computers supported by this agent.
        /// </summary>
        string SupportedManagementPackClassName { get; }

        /// <summary>
        /// Gets the OS version string as represented in the kit filename.
        /// </summary>
        string KitNameOSVersion { get; }

        /// <summary>
        /// Gets the OS version string as represented in the Management Pack tasks.
        /// </summary>
        string TaskVersion { get; }
    }
}
