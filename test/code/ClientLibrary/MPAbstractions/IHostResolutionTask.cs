//-----------------------------------------------------------------------
// <copyright file="IHostResolutionTask.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;

    /// <summary>
    /// Defines the interface for the HostResolutionTask.
    /// </summary>
    public interface IHostResolutionTask
    {
        /// <summary>
        /// Gets or sets the NameOrIPToResolve overrideable parameter of the task.
        /// </summary>
        string NameOrIPToResolve { get; set; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="group">ManagementGroup to execute on.</param>
        /// <param name="managementActionPoint">The management action point to execute on.</param>
        /// <returns>The results of the host resolution.</returns>
        HostResolutionTaskResult Execute(IManagementGroupConnection group, IManagedObject managementActionPoint);
    }
}
