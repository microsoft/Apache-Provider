//-----------------------------------------------------------------------
// <copyright file="InvalidSSHTaskResponseException.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;

    /// <summary>
    /// Exception thrown when SSHTask response failed to be parsed as the expected XML format
    /// </summary>
    [Serializable]
    public class InvalidSSHTaskResponseException : Exception
    {
        /// <summary>
        /// Initialize a new instance of InvalidSSHTaskResponseException
        /// </summary>
        /// <param name="message">The error message of the exception object</param>
        public InvalidSSHTaskResponseException(string message)
            : base(message)
        {
        }
    }
}
