//-----------------------------------------------------------------------
// <copyright file="TaskInvocationException.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Exception thrown when a task could not be invoked. This is a critical failure.
    /// </summary>
    [Serializable]
    public class TaskInvocationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the TaskInvocationException class.
        /// </summary>
        /// <param name="errorCode">Error code from the task result.</param>
        /// <param name="message">Error message from the task result.</param>
        public TaskInvocationException(int errorCode, string message)
            : base(string.Format(CultureInfo.CurrentCulture, Strings.TaskInvocationExceptionMessage, errorCode, message))
        {
        }

        /// <summary>
        /// Initializes a new instance of the TaskInvocationException class.
        /// </summary>
        /// <param name="message">Error message from the task result.</param>
        public TaskInvocationException(string message)
            : base(message)
        {
        }
    }
}
