//-----------------------------------------------------------------------
// <copyright file="ITaskInvocationResult.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using Microsoft.EnterpriseManagement.Runtime;
    
    /// <summary>
    /// Represents the results of a task invocation.
    /// </summary>
    public interface ITaskInvocationResult
    {
        /// <summary>
        /// Gets the task execution status.
        /// </summary>
        TaskStatus Status { get; }

        /// <summary>
        /// Gets the output from executed task.
        /// </summary>
        string Output { get; }

        /// <summary>
        /// Gets the task execution error code.
        /// </summary>
        int? ErrorCode { get; }

        /// <summary>
        /// Gets the task execution error message.
        /// </summary>
        string ErrorMessage { get; }
    }
}
