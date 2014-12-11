// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ElevationType.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement
{
    /// <summary>
    ///     This enumerates the meta-types of SCX Run As Accounts that are supported.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Monitor accounts are used to monitor data collected by the agent
    ///         on a managed UNIX/Linux host.  These accounts connect using WsMan/
    ///         WinRM/HTTPS protocol.  Only username/password authentication is
    ///         allowed.  The remote privilege elevation types allowed are none
    ///         and <i>su</i> elevation.
    ///     </para><para>
    ///         Maintenance accounts are used to maintain the agent on a managed
    ///         UNIX/Linux host (i.e. discover, install, update and/or uninstall).
    ///         These accounts connect using the SSH protocol.  Both username/password
    ///         authentication and SSH key authentication is allowed.  The remote
    ///         privilege elevation types allowed are none, <i>su</i> elevation and
    ///         <i>sudo</i> elevation.
    ///     </para>
    /// </remarks>
    public enum ScxRunAsAccountType
    {
        Monitor,
        Maintenance
    }
}