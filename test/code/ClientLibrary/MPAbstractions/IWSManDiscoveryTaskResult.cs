//-----------------------------------------------------------------------
// <copyright file="IWSManDiscoveryTaskResult.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;
    using System.Xml;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks;

    /// <summary>
    /// Represents the information that can be retrieved from a WSManDiscovery.
    /// </summary>
    public interface IWSManDiscoveryTaskResult
    {
        /// <summary>
        /// Gets the Operating System Information reported by the task.
        /// </summary>
        IOSInformation OSInfo { get; }

        /// <summary>
        /// Gets any agent version discovered.
        /// </summary>
        string AgentVersion { get; }
    }
}
