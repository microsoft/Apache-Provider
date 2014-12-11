//-----------------------------------------------------------------------
// <copyright file="TaskInvocationResult.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using Microsoft.EnterpriseManagement.Runtime;
    using Runtime = Microsoft.EnterpriseManagement.Runtime;

    /// <summary>
    /// Wrapper for the OpsMgr SDK TaskResult class.
    /// </summary>
    internal class TaskInvocationResult : ITaskInvocationResult
    {
        /// <summary>
        /// Wrapped TaskResult.
        /// </summary>
        private TaskResult result;

        /// <summary>
        /// Initializes a new instance of the TaskInvocationResult class.
        /// </summary>
        /// <param name="result">TaskResult to wrap.</param>
        public TaskInvocationResult(Runtime.TaskResult result)
        {
            this.result = result;
        }

        /// <summary>
        /// Gets the task invocation status.
        /// </summary>
        public TaskStatus Status
        {
            get { return this.result.Status; }
        }

        /// <summary>
        /// Gets the task output string.
        /// </summary>
        public string Output
        {
            get { return this.result.Output; }
        }

        /// <summary>
        /// Gets the task invocation error code.
        /// </summary>
        public int? ErrorCode
        {
            get { return this.result.ErrorCode; }
        }

        /// <summary>
        /// Gets the task invocation error message.
        /// </summary>
        public string ErrorMessage
        {
            get { return this.result.ErrorMessage; }
        }
    }
}
