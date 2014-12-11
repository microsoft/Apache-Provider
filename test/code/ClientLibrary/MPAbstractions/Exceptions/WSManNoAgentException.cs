//-----------------------------------------------------------------------
// <copyright file="WSManNoAgentException.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;

    /// <summary>
    /// Exception thrown by WSMan discovery when no agent is answering on the remote machine.
    /// </summary>
    [Serializable]
    public class WSManNoAgentException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the WSManNoAgentException class.
        /// </summary>
        /// <param name="message">Error message from WinRM.</param>
        public WSManNoAgentException(string message)
            : base(message)
        {
        }
    }
}
