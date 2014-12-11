//-----------------------------------------------------------------------
// <copyright file="KitInstallTaskFactory.cs" company="Microsoft">
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
    /// Real factory for kit installer tasks.
    /// </summary>
    public class KitInstallTaskFactory : IKitInstallTaskFactory
    {
        /// <summary>
        /// Create an instance of the KitINstaller task specific to the platform of the
        /// target host.
        /// </summary>
        /// <param name="agentInfo">Info to create the task from.</param>
        /// <returns>A new KitInstallTask.</returns>
        public IKitInstallTask CreateTask(ISupportedAgent agentInfo)
        {
            return new KitInstallTask(agentInfo);
        }
    }
}
