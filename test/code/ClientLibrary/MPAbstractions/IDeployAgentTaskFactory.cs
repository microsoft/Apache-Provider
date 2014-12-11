//-----------------------------------------------------------------------
// <copyright file="IDeployAgentTaskFactory.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Abstract factory for IDeployAgentTask.
    /// </summary>
    public interface IDeployAgentTaskFactory
    {
        /// <summary>
        /// Factory method for IDeployAgentTask.
        /// </summary>
        /// <param name="isLinux">The class will invoke a different task based on this value.</param>
        /// <param name="operatingSystem">Operating system name as represented in the MP task name.</param>
        /// <param name="operatingSystemVersion">Operating system version as represented in the MP task name.</param>
        /// <param name="architecture">System architecture as represented in the MP task name.</param>
        /// <param name="isInstallation">Flag indicating if it is used for installation or upgrade.</param>
        /// <returns>A new IDeployAgentTask.</returns>
        IDeployAgentTask CreateTask(bool isLinux, string operatingSystem, string operatingSystemVersion, string architecture, bool isInstallation);
    }
}
