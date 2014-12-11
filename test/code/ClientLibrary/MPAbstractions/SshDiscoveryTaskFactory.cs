//-----------------------------------------------------------------------
// <copyright file="SshDiscoveryTaskFactory.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    /// <summary>
    /// Factory for creating SshDiscoveryTask instances.
    /// </summary>
    public class SshDiscoveryTaskFactory : ISshDiscoveryTaskFactory
    {
        /// <summary>
        /// Creates a new SshDiscoveryTask instance.
        /// </summary>
        /// <returns>A new SshDiscoveryTask instance.</returns>
        public ISshDiscoveryTask CreateSshDiscoveryTask()
        {
            return new SshDiscoveryTask();
        }
    }
}
