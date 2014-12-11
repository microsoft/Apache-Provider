//-----------------------------------------------------------------------
// <copyright file="WSManAuthenticationErrorException.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;

    /// <summary>
    /// Exception thrown when 
    /// </summary>
    [Serializable]
    public class WSManAuthenticationErrorException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the WSManAuthenticationErrorException class.
        /// </summary>
        /// <param name="message">Message from task.</param>
        public WSManAuthenticationErrorException(string message)
            : base(message)
        {
        }
    }
}
