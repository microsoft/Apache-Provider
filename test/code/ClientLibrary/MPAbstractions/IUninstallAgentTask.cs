//-----------------------------------------------------------------------
// <copyright file="IUninstallAgentTask.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;

    public interface IUninstallAgentTask
    {
        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="managementGroupConnection">The ManagementGroupConnection on which to execute task.</param>
        /// <returns>Remote execution result.</returns>
        IRemoteCmdTaskResult Execute(IManagementGroupConnection managementGroupConnection);
    }
}