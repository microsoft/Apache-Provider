//-----------------------------------------------------------------------
// <copyright file="WinRMBasicAuthDisabledException.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;

    /// <summary>
    /// Thrown when it is discovered that Basic Auth has not been enabled for WinRM on the management server.
    /// </summary>
    [Serializable]
    public class WinRMBasicAuthDisabledException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the WinRMBasicAuthDisabledException class.
        /// </summary>
        /// <param name="message">Error message from WinRM.</param>
        public WinRMBasicAuthDisabledException(string message)
            : base(message)
        {
        }
    }
}
