// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SshKeyAuthenticationDataGenerator.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement
{
    using System;
    using System.Security;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Properties;

    public class SshKeyAuthenticationDataGenerator : RunAsAccountBase.IAuthenticationDataGenerator, IMaintenanceAuthenticationData
    {
        /// <summary>
        ///     Generate a <see cref="CredentialSet"/> for the given user.
        /// </summary>
        /// <param name="username">
        ///     The username to associate with the <see cref="CredentialSet"/>.
        /// </param>
        /// <returns>
        ///     This method returns a <see cref="CredentialSet"/> for the given user.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
            Justification = "False positive; FxCop doesn't seem capable of detecting that PHC added to a CS are disposed by the CS.")]
        public CredentialSet GenerateAuthenticationData(string username)
        {
            // throws IllegalAuthenticationCharacterException if username contains non-ASCII characters
            CmdletSupport.CheckAuthenticationDataEncoding(username, @"Username");

            PosixHostCredential sshCredential = null;
            PosixHostCredential suCredential  = null;

            try
            {
                sshCredential = new PosixHostCredential
                    {
                        PrincipalName = username,
                        KeyFile       = keyfile,
                        Passphrase    = passphrase,
                        Usage         = CredentialUsage.SshDiscovery |
                                            (elevation == ElevationType.SudoElevation ? CredentialUsage.SshSudoElevation : CredentialUsage.None)
                    };

                sshCredential.ReadAndValidateSshKey();

                CredentialSet credentialSet = new CredentialSet
                {
                    Usage = CredentialSetUsage.Maintenance,
                    IsSSHKey = true
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

        /// <summary>
        ///     Construct a PasswordAuthenticationDataGenerator for non-elevated
        ///     or <i>sudo</i> elevated privileges.
        /// </summary>
        /// <param name="keyfile">
        ///     This is the path to a valid PuTTY formatted SSH private keyfile,
        ///     i.e. a valid path to a .PPK file.
        /// </param>
        /// <param name="passphrase">
        ///     The passphrase for the SSH key, may be the empty string if the
        ///     key is not passphrase encrypted.
        /// </param>
        /// <param name="useSudo">
        ///     True if <i>sudo</i> elevation is allowed; false otherwise.
        /// </param>
        public SshKeyAuthenticationDataGenerator(string keyfile, SecureString passphrase, bool useSudo)
            : this(keyfile, passphrase, (useSudo ? ElevationType.SudoElevation : ElevationType.None), null)
        {
        }


        /// <summary>
        ///     Construct a PasswordAuthenticationDataGenerator for <i>su</i>
        ///     elevated privileges.
        /// </summary>
        /// <param name="keyfile">
        ///     This is the path to a valid PuTTY formatted SSH private keyfile,
        ///     i.e. a valid path to a .PPK file.
        /// </param>
        /// <param name="passphrase">
        ///     The passphrase for the SSH key, may be the empty string if the
        ///     key is not passphrase encrypted.
        /// </param>
        /// <param name="suPassword">
        ///     The <i>su</i> password.
        /// </param>
        public SshKeyAuthenticationDataGenerator(string keyfile, SecureString passphrase, SecureString suPassword)
            : this(keyfile, passphrase, ElevationType.SuElevation, suPassword)
        {
        }


        private SshKeyAuthenticationDataGenerator(string keyfile, SecureString passphrase, ElevationType elevation, SecureString suPassword)
        {
            if (string.IsNullOrEmpty(keyfile) || keyfile.Trim().Length == 0)
            {
                throw new ArgumentException(Resources.SshKeyAuthenticationDataGenerator_InvalidRunAsAccountKeyfile, @"SshKey");
            }

            if (passphrase == null)
            {
                throw new ArgumentNullException(
                    Resources.SshKeyAuthenticationDataGenerator_NullRunAsAccountPassphrase, @"Passphrase");
            }

            // throws IllegalAuthenticationCharacterException if passphrase contains non-ASCII characters
            CmdletSupport.CheckAuthenticationDataEncoding(passphrase, @"Passphrase");

            if (elevation == ElevationType.SuElevation && suPassword == null)
            {
                throw new ArgumentNullException(
                    Resources.PasswordAuthenticationDataGenerator_NullRunAsAccountSuPassword, @"SuPassword");
            }

            // throws IllegalAuthenticationCharacterException if suPassword contains non-ASCII characters
            CmdletSupport.CheckAuthenticationDataEncoding(suPassword, @"SuPassword");

            this.keyfile = keyfile;

            this.passphrase = passphrase.Copy();
            this.passphrase.MakeReadOnly();

            if (suPassword != null)
            {
                this.suPassword = suPassword.Copy();
                this.suPassword.MakeReadOnly();
            }

            this.elevation = elevation;
        }
 

        private readonly string        keyfile;
        private readonly SecureString  passphrase;
        private readonly SecureString  suPassword;
        private readonly ElevationType elevation;
   }
}