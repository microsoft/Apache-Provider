// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PasswordAuthenticationDataGenerator.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement
{
    using System;
    using System.Diagnostics;
    using System.Security;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Properties;

    public class PasswordAuthenticationDataGenerator
        : RunAsAccountBase.IAuthenticationDataGenerator, IMonitorAuthenticationData, IMaintenanceAuthenticationData
    {
        public CredentialSet GenerateAuthenticationData(string username)
        {
            // throws IllegalAuthenticationCharacterException if username contains non-ASCII characters
            CmdletSupport.CheckAuthenticationDataEncoding(username, @"Username");

            CredentialSet result = null;

            switch (accountType)
            {
                case CredentialSetUsage.Monitoring:
                    result = GenerateMonitorAuthenticationData(username);
                    break;

                case CredentialSetUsage.Maintenance:
                    result = GenerateMaintenanceAuthenticationData(username);
                    break;

                default:
                    Debug.Fail("The Run As account type is an unknown or unsupported type.");
                    break;
            }

            return result;
        }


        /// <summary>
        ///     Construct a PasswordAuthenticationDataGenerator for non-elevated
        ///     or <i>sudo</i> elevated privileges.
        /// </summary>
        /// <param name="runAsAccountType">
        ///     This specifies the type of Run As account the authentication
        ///     data is for, either <see cref="CredentialSetUsage.Monitoring"/>
        ///     or <see cref="CredentialSetUsage.Maintenance"/>.
        /// </param>
        /// <param name="password">
        ///     The password.
        /// </param>
        /// <param name="useSudo">
        ///     True if <i>sudo</i> elevation is allowed; false otherwise.
        /// </param>
        public PasswordAuthenticationDataGenerator(CredentialSetUsage runAsAccountType, SecureString password, bool useSudo)
            : this(runAsAccountType, password, (useSudo ? ElevationType.SudoElevation : ElevationType.None), null)
        {
        }


        /// <summary>
        ///     Construct a PasswordAuthenticationDataGenerator for <i>su</i>
        ///     elevated privileges.
        /// </summary>
        /// <param name="password">
        ///     The password.
        /// </param>
        /// <param name="suPassword">
        ///     The <i>su</i> password.
        /// </param>
        public PasswordAuthenticationDataGenerator(SecureString password, SecureString suPassword)
            : this(CredentialSetUsage.Maintenance, password, ElevationType.SuElevation, suPassword)
        {
        }


        private PasswordAuthenticationDataGenerator(CredentialSetUsage runAsAccountType, SecureString password, ElevationType elevation, SecureString suPassword)
        {
            if (password == null)
            {
                throw new ArgumentNullException(
                    Resources.PasswordAuthenticationDataGenerator_NullRunAsAccountPassword, @"Password");
            }

            // throws IllegalAuthenticationCharacterException if password contains non-ASCII characters
            CmdletSupport.CheckAuthenticationDataEncoding(password, @"Password");

            if (runAsAccountType == CredentialSetUsage.Monitoring && elevation == ElevationType.SuElevation)
            {
                throw new ArgumentException(Resources.PasswordAuthenticationDataGenerator_InvalidMonitorRunAsAccountElevation, @"elevation");
            }

            if (elevation == ElevationType.SuElevation && suPassword == null)
            {
                throw new ArgumentNullException(
                    Resources.PasswordAuthenticationDataGenerator_NullRunAsAccountSuPassword, @"SuPassword");
            }

            // throws IllegalAuthenticationCharacterException if suPassword contains non-ASCII characters
            CmdletSupport.CheckAuthenticationDataEncoding(suPassword, @"SuPassword");

            Debug.Assert(
                runAsAccountType == CredentialSetUsage.Monitoring || runAsAccountType == CredentialSetUsage.Maintenance,
                "The only supported account types are Monitoring and Maintenance.");

            this.accountType = runAsAccountType;

            this.password    = password.Copy();
            this.password.MakeReadOnly();

            if (suPassword != null)
            {
                this.suPassword = suPassword.Copy();
                this.suPassword.MakeReadOnly();
            }

            this.elevation = elevation;
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
            Justification = "False positive; FxCop doesn't seem capable of detecting that PHC added to a CS are disposed by the CS.")]
        private CredentialSet GenerateMonitorAuthenticationData(string username)
        {
            PosixHostCredential wsManCredential = null;

            try
            {
                CredentialSet credentialSet = new CredentialSet
                {
                    Usage = CredentialSetUsage.Monitoring,
                    IsSSHKey = false
                };

                wsManCredential = new PosixHostCredential
                    {
                        PrincipalName = username,
                        Passphrase    = password,
                        Usage         = CredentialUsage.WsManDiscovery |
                                            (elevation == ElevationType.SudoElevation ? CredentialUsage.WsmanSudoElevation : CredentialUsage.None)
                    };

                credentialSet.Add(wsManCredential);
                wsManCredential = null;

                return credentialSet;
            }
            finally 
            {
                if (wsManCredential != null)
                {
                    wsManCredential.Dispose();
                }
            }
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
            Justification = "False positive; FxCop doesn't seem capable of detecting that PHC added to a CS are disposed by the CS.")]
        private CredentialSet GenerateMaintenanceAuthenticationData(string username)
        {
            PosixHostCredential sshCredential = null;
            PosixHostCredential suCredential  = null;

            try
            {
                CredentialSet credentialSet = new CredentialSet
                {
                    Usage    = CredentialSetUsage.Maintenance,
                    IsSSHKey = false
                };

                sshCredential = new PosixHostCredential
                    {
                        PrincipalName = username,
                        Passphrase    = password,
                        Usage         = CredentialUsage.SshDiscovery |
                                            (elevation == ElevationType.SudoElevation ? CredentialUsage.SshSudoElevation : CredentialUsage.None)
                    };

                credentialSet.Add(sshCredential);
                sshCredential = null;

                if (elevation == ElevationType.SuElevation)
                {
                    suCredential = new PosixHostCredential
                        {
                            Passphrase = suPassword,
                            Usage      = CredentialUsage.SuElevation
                        };

                    credentialSet.Add(suCredential);
                    suCredential = null;
                }

                return credentialSet;
            }
            finally 
            {
                if (sshCredential != null)
                {
                    sshCredential.Dispose();
                }

                if (suCredential != null)
                {
                    suCredential.Dispose();
                }
            }
        }


        private readonly CredentialSetUsage accountType;
        private readonly SecureString       password;
        private readonly SecureString       suPassword;
        private readonly ElevationType      elevation;
    }
}