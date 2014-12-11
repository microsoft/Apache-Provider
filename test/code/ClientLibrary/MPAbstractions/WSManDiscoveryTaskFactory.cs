//-----------------------------------------------------------------------
// <copyright file="WSManDiscoveryTaskFactory.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    /// <summary>
    /// Factory for creating WSManDiscoveryTask instances.
    /// </summary>
    public class WSManDiscoveryTaskFactory : IWSManDiscoveryTaskFactory
    {
        /// <summary>
        /// Creates a new WSManDiscoveryTask instance.
        /// </summary>
        /// <returns>A new WSManDiscoveryTask instance.</returns>
        public IWSManDiscoveryTask CreateWSManDiscoveryTask()
        {
            return new WSManDiscoveryTask();
        }

        /// <summary>
        /// Creates a new WSManDiscoveryTask instance.
        /// </summary>
        /// <param name="taskEnumType">Define the type of wsman discovery task.</param>
        /// <returns>A new WSManDiscoveryTask instance.</returns>
        public IWSManDiscoveryTask CreateWSManDiscoveryTask(WsmanDiscoveryTaskEnum taskEnumType)
        {
            IWSManDiscoveryTask task = null;      
            if ( taskEnumType == WsmanDiscoveryTaskEnum.UpgradeVerificationTask)
            {
                task = new UpgradeVerificationTask();
            }
            else
            {
                task = new WSManDiscoveryTask();  
            }

            return task;
        }
    }
}
