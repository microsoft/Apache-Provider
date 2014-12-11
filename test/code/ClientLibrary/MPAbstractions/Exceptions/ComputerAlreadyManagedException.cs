//-----------------------------------------------------------------------
// <copyright file="ComputerAlreadyManagedException.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;

    /// <summary>
    /// Exception thrown when trying to add a computer to the database when it's already there.
    /// </summary>
    [Serializable]
    public class ComputerAlreadyManagedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the ComputerAlreadyManagedException class.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="innerException">Inner exception from OpsMgr SDK.</param>
        public ComputerAlreadyManagedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
