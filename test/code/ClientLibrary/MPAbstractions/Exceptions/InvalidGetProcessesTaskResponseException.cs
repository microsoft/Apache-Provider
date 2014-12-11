// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidGetProcessesTaskResponseException.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions.Exceptions
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Exception thrown when the code fails to parse the information returned by host resolution task.
    /// </summary>
    [Serializable]
    public class InvalidGetProcessesTaskResponseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the InvalidHostResolutionTaskResponseException class.
        /// </summary>
        /// <param name="result">Task response that the code failed to parse.</param>
        public InvalidGetProcessesTaskResponseException(string result)
            : base(string.Format(CultureInfo.CurrentCulture, Strings.GetProcessesTaskResultParseFailure, result))
        {
        }
    }
}