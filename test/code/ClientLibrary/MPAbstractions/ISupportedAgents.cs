//-----------------------------------------------------------------------
// <copyright file="ISupportedAgents.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks;

    /// <summary>
    /// Service that can be used to retrieve supported agent information.
    /// </summary>
    public interface ISupportedAgents
    {
        /// <summary>
        /// Retrive information about the agent which supports this operating system.
        /// </summary>
        /// <param name="operatingSystem">Operating system alias to get support information for.</param>
        /// <param name="architecture">Architecture to get support information for.</param>
        /// <param name="operatingSystemVersion">Version of operating system to get support information for.</param>
        /// <returns>A SupportedAgent class.</returns>
        ISupportedAgent GetSupportedAgent(string operatingSystem, string architecture, OperatingSystemVersion operatingSystemVersion);

        /// <summary>
        /// Retrive information about the agent which supports this operating system.
        /// </summary>
        /// <param name="supportedMPClassName">ManagementPack class that is used to monitor computers supported by this agent.</param>
        /// <param name="architecture">Architecture to get support information for.</param>
        /// <returns>A SupportedAgent class.</returns>
        ISupportedAgent GetSupportedAgent(string supportedMPClassName, string architecture);
    }
}
