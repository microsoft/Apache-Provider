// ----------------------------------------------------------------------------------------------------
// <copyright file="CredentialUsage.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core
{
    using System;

    /// <summary>
    /// Enumeration to indicate what valid uses this credential is intended for.
    /// </summary>
    [Flags]
    public enum CredentialUsage
    {
        /// <summary>
        ///     Default value indicates no specified usage (hence, not usable).
        /// </summary>
        None                    = 0x0000,

        /// <summary>
        ///     Credential to be used for WS-Man based discovery.
        /// </summary>
        WsManDiscovery          = 0x0001,

        /// <summary>
        ///     Credential to be used for SSH Script based discovery.
        /// </summary>
        SshDiscovery            = 0x0002,

        /// <summary>
        ///     RESERVED. Currently only used for tests.
        /// </summary>
        ServiceLogin = 0x0004,

        /// <summary>
        ///     RESERVED. Currently only used for tests.
        /// </summary>
        PrivilegedServiceLogin = 0x0008,

        /// <summary>
        ///     RESERVED. Currently only used for tests.
        /// </summary>
        SshSudoElevation           = 0x0010,

        /// <summary>
        /// Credential to be used for SU elevation
        /// </summary>
        SuElevation             = 0x0020,

        /// <summary>
        ///     RESERVED. Currently only used for tests.
        /// </summary>
        WsmanSudoElevation = 0x0040,

        /// <summary>
        ///     Credential may be used for any purpose. This is usually
        ///     for outfits with SSO (for instance).
        /// </summary>
        Any                    = 0xffff
    }
}