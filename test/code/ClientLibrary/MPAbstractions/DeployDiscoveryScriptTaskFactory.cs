//-----------------------------------------------------------------------
// <copyright file="DeployDiscoveryScriptTaskFactory.cs" company="Microsoft">
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
    /// Default implementation of the IDeployDiscoveryScriptTaskFactory. This one generates DeployDiscoveryScriptTask instances.
    /// </summary>
    public class DeployDiscoveryScriptTaskFactory : IDeployDiscoveryScriptTaskFactory
    {
        /// <summary>
        /// Factory method for DeployDiscoveryScript tasks.
        /// </summary>
        /// <returns>A new DeployDiscoveryScriptTask.</returns>
        public IDeployDiscoveryScriptTask CreateTask()
        {
            return new DeployDiscoveryScriptTask();
        }
    }
}
