//-----------------------------------------------------------------------
// <copyright file="WSManResolutionErrorException.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;

    /// <summary>
    /// Exception thrown by WSMan discovery when host resolution failed.
    /// </summary>
    [Serializable]
    public class WSManResolutionErrorException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the WSManResolutionErrorException class.
        /// </summary>
        /// <param name="message">Error message from WinRM invocation.</param>
        public WSManResolutionErrorException(string message)
            : base(message)
        {
        }
    }
}
