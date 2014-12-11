//-----------------------------------------------------------------------
// <copyright file="IInstallableAgents.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks;

    /// <summary>
    /// Represents a collection of installable agents.
    /// </summary>
    public interface IInstallableAgents
    {
        /// <summary>
        /// Gets the folder where Discovery Script resides.
        /// </summary>
        string DiscoveryScriptFolder { get; }

        /// <summary>
        /// Gets the installation path for a particular supported agent.
        /// </summary>
        /// <param name="agent">Supported agent data for which to retrieve installation path.</param>
        /// <returns>Local path on the management server to the agent file supported by the argument.</returns>
        IInstallableAgent GetInstallableAgentFor(ISupportedAgent agent);
    }
}
