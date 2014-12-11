//-----------------------------------------------------------------------
// <copyright file="UninstallAgentTaskFactory.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core;

    public class UninstallAgentTaskFactory : IUninstallAgentTaskFactory
    {
        public IUninstallAgentTask CreateTask(IPersistedUnixComputer unixComputer, CredentialSet credentials)
        {
            return new UninstallAgentTask(unixComputer, credentials);
        }
    }
}
