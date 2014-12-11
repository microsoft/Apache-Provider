// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CredentialSetUsage.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core
{
    public enum CredentialSetUsage
    {
        Monitoring = 0x0001, 
        Maintenance = 0x0002, 
        AgentUninstall = 0x0004
    }
}