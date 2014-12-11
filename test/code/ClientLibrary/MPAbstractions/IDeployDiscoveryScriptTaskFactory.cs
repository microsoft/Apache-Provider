//-----------------------------------------------------------------------
// <copyright file="IDeployDiscoveryScriptTaskFactory.cs" company="Microsoft">
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
    /// Abstract factory for IDeployDiscoveryScriptTask.
    /// </summary>
    public interface IDeployDiscoveryScriptTaskFactory
    {
        /// <summary>
        /// Factory method for IDeployDiscoveryScript tasks.
        /// </summary>
        /// <returns>A new IDeployDiscoveryScriptTask.</returns>
        IDeployDiscoveryScriptTask CreateTask();
    }
}
