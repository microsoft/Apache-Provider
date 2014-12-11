//-----------------------------------------------------------------------
// <copyright file="IUnixComputer.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.Utilities;

    /// <summary>
    /// Representation of a UnixComputer.
    /// </summary>
    public interface IUnixComputer
    {
        /// <summary>
        /// Gets or sets the name of the computer.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the IP Address of the computer instance.
        /// </summary>
        string IPAddress { get; set; }

        /// <summary>
        /// Gets or sets the SSH Port of the computer instance.
        /// </summary>
        int SSHPort { get; set; }

        /// <summary>
        /// Gets or sets the version of the x-plat agent installed on this computer.
        /// </summary>
        UnixAgentVersion AgentVersion { get; set; }

        /// <summary>
        /// Gets or sets the architecture of this computer instance.
        /// </summary>
        string Architecture { get; set; }

        /// <summary>
        /// Gets or sets the Id of this computer instance.
        /// </summary>
        Guid Id { get; set; }
    }
}