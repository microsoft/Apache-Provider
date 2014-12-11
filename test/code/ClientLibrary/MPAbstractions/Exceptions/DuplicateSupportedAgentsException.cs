//-----------------------------------------------------------------------
// <copyright file="DuplicateSupportedAgentsException.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;

    /// <summary>
    /// Exception thrown when more than one supported agent matching a criteria is found.
    /// </summary>
    [Serializable]
    public class DuplicateSupportedAgentsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the DuplicateSupportedAgentsException class.
        /// </summary>
        public DuplicateSupportedAgentsException()
            : base(Strings.DuplicateSupportedAgentsMessage)
        {
        }
    }
}