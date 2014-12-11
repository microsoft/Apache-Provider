//-----------------------------------------------------------------------
// <copyright file="InvalidSSHDiscoveryTaskResponseException.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Exception thrown when the code fails to parse the information returned by ssh discovery.
    /// </summary>
    [Serializable]
    public class InvalidSSHDiscoveryTaskResponseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the InvalidSSHDiscoveryTaskResponseException class.
        /// </summary>
        /// <param name="result">Task response that the code failed to parse.</param>
        /// <param name="innerException">Inner exception.</param>
        public InvalidSSHDiscoveryTaskResponseException(string result, Exception innerException)
            : base(string.Format(CultureInfo.CurrentCulture, Strings.SSHDiscoveryTaskResultParseFailure, result), innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the InvalidSSHDiscoveryTaskResponseException class.
        /// </summary>
        /// <param name="result">Task response that the code failed to parse.</param>
        public InvalidSSHDiscoveryTaskResponseException(string result)
            : base(string.Format(CultureInfo.CurrentCulture, Strings.SSHDiscoveryTaskResultParseFailure, result))
        {
        }
    }
}
