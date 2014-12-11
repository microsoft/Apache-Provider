//-----------------------------------------------------------------------
// <copyright file="SSHTaskFailedException.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Exception thrown when SSHTask failed for some reason, for example, invalid SSH credential
    /// </summary>
    [Serializable]
    public class SSHTaskFailedException : Exception
    {
        /// <summary>
        /// Initialize a new instance of SSHTaskFailedException
        /// </summary>
        /// <param name="message">The error message of the exception object</param>
        public SSHTaskFailedException(string message)
            : base(message)
        {
        }
    }
}
