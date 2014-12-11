//-----------------------------------------------------------------------
// <copyright file="WSManUnknownErrorException.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;

    /// <summary>
    /// Exception thrown by WSMan discovery when there was an error but the error type is not recognized.
    /// </summary>
    [Serializable]
    public class WSManUnknownErrorException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the WSManUnknownErrorException class.
        /// </summary>
        /// <param name="message">Error message from WSMan invocation.</param>
        public WSManUnknownErrorException(string message)
            : base(message)
        {
        }
    }
}
