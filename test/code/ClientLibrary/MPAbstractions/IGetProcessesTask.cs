// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGetProcessesTask.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;

    /// <summary>
    /// Defines the interface for the GetProcessesTask.
    /// </summary>
    public interface IGetProcessesTask
    {
        /// <summary>
        /// Gets or sets the TargetSystem overrideable parameter of the task.
        /// </summary>
        string TargetSystem { get; set; }

        #region Public Methods

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="managementGroupConnection">
        /// The ManagementGroupConnection on which to execute task.
        /// </param>
        /// <param name="managementActionPoint">The mangement action point to execute on.</param>
        /// <returns>
        /// Remote execution result.
        /// </returns>
        IGetProcessesTaskResult Execute(IManagementGroupConnection managementGroupConnection, IManagedObject managementActionPoint);

        #endregion
    }
}