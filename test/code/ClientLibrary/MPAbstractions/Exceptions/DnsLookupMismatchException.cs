//-----------------------------------------------------------------------
// <copyright file="DnsLookupMismatchException.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;

    /// <summary>
    /// Exception thrown by host name resoulution activity when DNS has mismatched forward and reverse lookup to a name.
    /// </summary>
    [Serializable]
    public class DnsLookupMismatchException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the DnsLookupMismatchException class.
        /// </summary>
        /// <param name="message">Error message from Host name resoulution.</param>
        public DnsLookupMismatchException(string message)
            : base(message)
        {
        }
    }
}
