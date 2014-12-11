//-----------------------------------------------------------------------
// <copyright file="IWSManDiscoveryTaskFactory.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;

    /// <summary>
    /// Define the types of Wsman Discovery 
    /// </summary>
    public enum WsmanDiscoveryTaskEnum
    {
        WsmanDiscoveryTask,
        UpgradeVerificationTask
    }

    /// <summary>
    /// Factory for creating WSManDiscoveryTask instances.
    /// </summary>
    public interface IWSManDiscoveryTaskFactory
    {
        /// <summary>
        /// Creates a new WSManDiscoveryTask instance.
        /// </summary>
        /// <returns>A new WSManDiscoveryTask instance.</returns>
        IWSManDiscoveryTask CreateWSManDiscoveryTask();

        /// <summary>
        /// Creates a new WSManDiscoveryTask instance.
        /// </summary>
        /// <param name="taskEnumType">Define the type of wsman discovery task.</param>
        /// <returns>A new WSManDiscoveryTask instance.</returns>
        IWSManDiscoveryTask CreateWSManDiscoveryTask(WsmanDiscoveryTaskEnum taskEnumType);
    }
}
