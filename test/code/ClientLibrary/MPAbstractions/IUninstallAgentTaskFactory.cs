//-----------------------------------------------------------------------
// <copyright file="IUninstallAgentTaskFactory.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core;

    public interface IUninstallAgentTaskFactory
    {
        IUninstallAgentTask CreateTask(IPersistedUnixComputer unixComputer, CredentialSet credentials);
    }
}
