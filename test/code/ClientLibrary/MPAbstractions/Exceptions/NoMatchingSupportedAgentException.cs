//-----------------------------------------------------------------------
// <copyright file="NoMatchingSupportedAgentException.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;

    /// <summary>
    /// Exception thrown when a discovered host is not supported by any currently imported management pack.
    /// </summary>
    [Serializable]
    public class NoMatchingSupportedAgentException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the NoMatchingSupportedAgentException class.
        /// </summary>
        public NoMatchingSupportedAgentException()
            : base(Strings.NoMatchingSupportedAgentMessage)
        {
        }
    }
}
