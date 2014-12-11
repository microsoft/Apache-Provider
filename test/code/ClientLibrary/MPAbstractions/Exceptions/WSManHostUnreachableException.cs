//-----------------------------------------------------------------------
// <copyright file="WSManHostUnreachableException.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;

    /// <summary>
    /// Exception thrown by WSMan discovery when host is unreachable.
    /// </summary>
    [Serializable]
    public class WSManHostUnreachableException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the WSManHostUnreachableException class.
        /// </summary>
        /// <param name="message">Error message from WinRM.</param>
        public WSManHostUnreachableException(string message)
            : base(message + Strings.WSManHostUnreachableException_Explanatory_Text)
        {
        }
    }
}
