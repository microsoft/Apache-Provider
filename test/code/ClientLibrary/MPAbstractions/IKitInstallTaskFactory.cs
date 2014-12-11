//-----------------------------------------------------------------------
// <copyright file="IKitInstallTaskFactory.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks;

    /// <summary>
    /// Interface to create platform installer tasks.
    /// </summary>
    public interface IKitInstallTaskFactory
    {
        /// <summary>
        /// Create an instance of the KitINstaller task specific to the platform of the
        /// target host.
        /// </summary>
        /// <param name="agentInfo">Info to create the task from.</param>
        /// <returns>A new IKitInstallTask.</returns>
        IKitInstallTask CreateTask(ISupportedAgent agentInfo);
    }
}
