//-----------------------------------------------------------------------
// <copyright file="ScxCredentialSettings.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>v-lizhou</author>
// <description></description>
// <history4/21/2011 5:05:51 PM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Apache.SDK.ApacheSDKHelper
{
    using System;

    /// <summary>
    /// Description for ScxCredentialSettings.
    /// </summary>
    public static class ScxCredentialSettings
    {
        /// <summary>
        /// The name of public ssh key with passphrase 
        /// </summary>
        public const string PubSSHkeyWithPassphrase = "with_passphrase.pub";

        /// <summary>
        /// The name of public ssh key without passphrase
        /// </summary>
        public const string PubSSHkeyWithoutPassphrase = "without_passphrase.pub";

        /// <summary>
        /// The name of private ssh key with passphrase 
        /// </summary>
        public const string PriSSHkeyWithPassphrase = "with_passphrase.ppk";

        /// <summary>
        /// The name of private ssh key without passphrase
        /// </summary>
        public const string PriSSHkeyWithoutPassphrase = "without_passphrase.ppk";

        /// <summary>
        /// The name of private ssh key without passphrase
        /// </summary>
        public const string SuPassphrase = "OpsMgr2007R2";

        /// <summary>
        /// Type of credential (one of SSH Key or Basic Auth)
        /// </summary>
        public enum CredentialType
        {
            /// <summary>
            /// Use SSH Key
            /// </summary>
            SSHKey = 0,

            /// <summary>
            /// Use Basic Auth
            /// </summary>
            BasicAuth = 1,
        }

        /// <summary>
        /// Account type to be used for discovery of the computer
        /// </summary>
        public enum AccountType
        {
            /// <summary>
            /// Use priviledged account for discovery
            /// </summary>
            Priviledged = 0,

            /// <summary>
            /// Use unpriviledged user account for discovery
            /// </summary>
            Unpriviledged = 1
        }

        /// <summary>
        /// Elevated credential settings type
        /// </summary>
        public enum ElevationType
        {
            /// <summary>
            /// No elevatted type
            /// </summary>
            None = 0,

            /// <summary>
            /// su elevated type
            /// </summary>
            Su = 1,

            /// <summary>
            /// sudo elevated type
            /// </summary>
            Sudo = 2
        }

        /// <summary>
        /// SSH Credential type
        /// </summary>
        public enum SSHCredentialType
        {
            /// <summary>
            /// SSH credential type with passphress
            /// </summary>
            WithPassphrase = 0,

            /// <summary>
            /// SSH credential type without passphress
            /// </summary>
            WithoutPassphrase = 1
        }

        /// <summary>
        /// UNIX\Linux profile type
        /// </summary>
        public enum ProfileType
        {
            /// <summary>
            /// UNIX\Linux Action Account profile
            /// </summary>
            ActionAccount = 0,

            /// <summary>
            /// UNIX\Linux Privileged Account profile
            /// </summary>
            PrivilegedAccount = 1,

            /// <summary>
            /// UNIX\Linux Agent Maintenance Account profile
            /// </summary>
            AgentMaintenanceAccount = 2
        }

        /// <summary>
        /// Account type
        /// </summary>
        public enum UnixAccountType
        {
            /// <summary>
            /// Basic privileged Authentication Account
            /// </summary>
            BasicSuperUser = 0,

            /// <summary>
            /// Basic Authentication Account
            /// </summary>
            BasicNonSuperUser = 1,

            /// <summary>
            /// UNIX\Linux Monitoring Account
            /// </summary>
            ScxMonitoringNonPrivileged = 2,

            /// <summary>
            /// UNIX\Linux Monitoring Account with sudo
            /// </summary>
            ScxMonitoringWithSudo = 3,

            /// <summary>
            /// UNIX\Linux Privileged Monitoring Account
            /// </summary>
            ScxMonitoringPrivileged = 4,

            /// <summary>
            /// UNIX\Linux Agent Maintenance Account
            /// </summary>
            ScxAgentMaintenance = 5
        }
    }
}