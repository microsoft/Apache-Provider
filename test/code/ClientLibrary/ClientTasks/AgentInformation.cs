//-----------------------------------------------------------------------
// <copyright file="AgentInformation.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System;

    /// <summary>
    /// Contains discovered information about the SCX agent.
    /// </summary>
    public class AgentInformation
    {
        /// <summary>
        /// Gets or sets the agent version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the agent has been found to be installed.
        /// </summary>
        public bool IsInstalled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the agent has been found to be signed.
        /// </summary>
        public bool IsSigned { get; set; }

        /// <summary>
        /// Gets or sets the error details of an exception
        /// </summary>
        public Exception ErrorDetails { get; set; }
    }
}
