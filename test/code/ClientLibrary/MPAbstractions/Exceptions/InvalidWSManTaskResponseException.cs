//-----------------------------------------------------------------------
// <copyright file="InvalidWSManTaskResponseException.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Exception thrown when the code fails to parse the information returned by wsman discovery.
    /// </summary>
    [Serializable]
    public class InvalidWSManTaskResponseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the InvalidWSManTaskResponseException class.
        /// </summary>
        /// <param name="result">Task response that the code failed to parse.</param>
        /// <param name="innerException">Inner exception.</param>
        public InvalidWSManTaskResponseException(string result, Exception innerException)
            : base(string.Format(CultureInfo.CurrentCulture, Strings.WSManTaskResultParseFailure, result), innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the InvalidWSManTaskResponseException class.
        /// </summary>
        /// <param name="result">Task response that the code failed to parse.</param>
        public InvalidWSManTaskResponseException(string result)
            : base(string.Format(CultureInfo.CurrentCulture, Strings.WSManTaskResultParseFailure, result))
        {
        }
    }
}
