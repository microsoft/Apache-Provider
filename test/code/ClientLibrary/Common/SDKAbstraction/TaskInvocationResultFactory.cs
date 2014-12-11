//-----------------------------------------------------------------------
// <copyright file="TaskInvocationResultFactory.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using System;
    using Microsoft.EnterpriseManagement.Runtime;

    /// <summary>
    /// Creates ITaskInvocationResult instances from TaskResult instances from the OpsMgr SDK.
    /// </summary>
    internal class TaskInvocationResultFactory : ITaskInvocationResultFactory
    {
        /// <summary>
        /// Creates ITaskInvocationResult instances from TaskResult instances from the OpsMgr SDK.
        /// </summary>
        /// <param name="result">TaskResult from OpsMgr SDK.</param>
        /// <returns>ITaskInvocationResult wrapping the TaskResult.</returns>
        public ITaskInvocationResult CreateTaskInvocationResult(TaskResult result)
        {
            return new TaskInvocationResult(result);
        }
    }
}
