//-----------------------------------------------------------------------
// <copyright file="NoMatchingInstallableAgentException.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;

    /// <summary>
    /// Exception thrown when failing to find a kit file for installing on a particular host.
    /// </summary>
    [Serializable]
    public class NoMatchingInstallableAgentException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the NoMatchingInstallableAgentException class.
        /// </summary>
        public NoMatchingInstallableAgentException()
            : base(Strings.NoMatchingInstallableAgentMessage)
        {
        }
    }
}
