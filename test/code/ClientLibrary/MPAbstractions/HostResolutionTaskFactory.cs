//-----------------------------------------------------------------------
// <copyright file="HostResolutionTaskFactory.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    /// <summary>
    /// Default implementation of the IHostResolutionTaskFactory interface.
    /// </summary>
    public class HostResolutionTaskFactory : IHostResolutionTaskFactory
    {
        public IHostResolutionTask CreateResolutionTask()
        {
            return new HostResolutionTask();
        }
    }
}