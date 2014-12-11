//-----------------------------------------------------------------------
// <copyright file="IHostResolutionTaskFactory.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    /// <summary>
    /// Abstract factory for HostResolutionTasks.
    /// </summary>
    public interface IHostResolutionTaskFactory
    {
        /// <summary>
        /// Creates a new instance of a HostResolutionTask.
        /// </summary>
        /// <returns>A new instance of a HostResolutionTask.</returns>
        IHostResolutionTask CreateResolutionTask();
    }
}