//-----------------------------------------------------------------------
// <copyright file="MissingRunasAccountException.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;

    /// <summary>
    /// Exception thrown when wsman fails because of an untrusted certificate.
    /// </summary>
    [Serializable]
    public class MissingRunasAccountException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the MissingRunasAccountException class.
        /// </summary>
        /// <param name="message">Error message from task.</param>
        public MissingRunasAccountException(string message)
            : base(message)
        {
        }
    }
}
