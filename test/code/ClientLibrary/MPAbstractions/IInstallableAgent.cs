//-----------------------------------------------------------------------
// <copyright file="IInstallableAgent.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.Utilities;

    /// <summary>
    /// This interface describes what a user may want to know about an agent file that can be installed.
    /// </summary>
    public interface IInstallableAgent
    {
        string Path { get; }

        UnixAgentVersion AgentVersion { get; }
    }
}