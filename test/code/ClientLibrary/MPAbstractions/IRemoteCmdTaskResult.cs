//-----------------------------------------------------------------------
// <copyright file="IRemoteCmdTaskResult.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    /// <summary>
    ///     Represents the output from any task that effectively executes a command line or script over
    ///     our ssh connection.
    /// </summary>
    public interface IRemoteCmdTaskResult
    {
        /// <summary>
        /// Gets the exit code of task invocation.
        /// </summary>
        int ExitCode { get; }

        /// <summary>
        /// Gets the exception message associated with the task invocation.
        /// </summary>
        string ExceptionMessage { get; }
        
        /// <summary>
        /// Gets the standard output from task invocation.
        /// </summary>
        string StdOut { get; }

        /// <summary>
        /// Gets the standard error from task invocation.
        /// </summary>
        string StdErr { get; }
    }
}
