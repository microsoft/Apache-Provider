//-----------------------------------------------------------------------
// <copyright file="ISshDiscoveryTaskFactory.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;

    /// <summary>
    /// Factory for creating WSManDiscoveryTask instances.
    /// </summary>
    public interface ISshDiscoveryTaskFactory
    {
        /// <summary>
        /// Creates a new WSManDiscoveryTask instance.
        /// </summary>
        /// <returns>A new WSManDiscoveryTask instance.</returns>
        ISshDiscoveryTask CreateSshDiscoveryTask();
    }
}
