//-----------------------------------------------------------------------
// <copyright file="HostResolutionFailedException.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;

    /// <summary>
    /// Exception thrown when the host resolution task fails to resolve a host name.
    /// </summary>
    [Serializable]
    public class HostResolutionFailedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the HostResolutionFailedException class.
        /// </summary>
        /// <param name="message">Error message from task.</param>
        public HostResolutionFailedException(string message)
            : base(message)
        {
        }
    }
}
