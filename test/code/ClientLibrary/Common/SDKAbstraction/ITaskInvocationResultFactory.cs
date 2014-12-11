//-----------------------------------------------------------------------
// <copyright file="ITaskInvocationResultFactory.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using System;
    using Microsoft.EnterpriseManagement.Runtime;

    /// <summary>
    /// Creates ITaskInvocationResult from OpsMgr SDK TaskResult.
    /// </summary>
    public interface ITaskInvocationResultFactory
    {
        /// <summary>
        /// Creates ITaskInvocationResult from OpsMgr SDK TaskResult.
        /// </summary>
        /// <param name="result">SDK TaskResult.</param>
        /// <returns>ITaskInvocation result representing the result.</returns>
        ITaskInvocationResult CreateTaskInvocationResult(TaskResult result);
    }
}
