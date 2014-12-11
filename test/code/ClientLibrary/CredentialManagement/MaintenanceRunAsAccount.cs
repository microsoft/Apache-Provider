// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MaintenanceRunAsAccount.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement
{
    using System.Diagnostics;
    using System.Security;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core;

    public class MaintenanceRunAsAccount : RunAsAccountBase, IMaintenanceRunAsAccount
    {
        /// <summary>
        ///     Gets the primary PosixHostCredential for this Run As account.
        /// </summary>
        protected override PosixHostCredential PrimaryCredential
        {
            get
            {
                PosixHostCredential sshCredential = RunAsAccount.Credential.CredentialsForAll(CredentialUsage.SshDiscovery);
                Debug.Assert(sshCredential != null, "SCX Maintenance Run As accounts must have a PosixHostCredential with CredentialUsage.SshDiscovery!");

                return sshCredential;
            }
        }


        /// <summary>
        ///     Sets the authentication data for the Run As account.
        /// </summary>
        /// <remarks>
        ///     Setting <see langword="null"/> values are silently ignored,
        ///     i.e. the property is <i>not</i> changed.
        /// </remarks>
        public IMaintenanceAuthenticationData AuthenticationData
        {
            set
            {
                // if the value is null, keep the current value
                if (value == null)
                {
                    return;
                }

                RunAsAccount.Credential =
                    ((IAuthenticationDataGenerator)value).GenerateAuthenticationData(Username);
            }
        }


        /// <summary>
        ///     Get authentication data that can be used with an SCX Maintenance
        ///     Run As account.
        /// </summary>
        /// <param name="password">
        ///     The password for the managed host user.
        /// </param>
        /// <param name="useSudo">
        ///     True if this user can elevate privileges using <i>sudo</i>;
        ///     false otherwise.
        /// </param>
        /// <returns>
        ///     This method returns IMaintenanceAuthentication that can be used
        ///     with an SCX Maintenance Run As account.
        /// </returns>
        public IMaintenanceAuthenticationData AuthenticationDataFactory(SecureString password, bool useSudo)
        {
            return MaintenanceAuthenticationDataFactory(password, useSudo);
        }


        /// <summary>
        ///     Get authentication data that can be used with an SCX Maintenance
        ///     Run As account.
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
        /// <returns>
        ///     This method returns IMaintenanceAuthentication that can be used
        ///     with an SCX Maintenance Run As account.
        /// </returns>
        public IMaintenanceAuthenticationData AuthenticationDataFactory(string keyfile, SecureString passphrase, bool useSudo)
        {
            return MaintenanceAuthenticationDataFactory(keyfile, passphrase, useSudo);
        }


        /// <summary>
        ///     Get authentication data that can be used with an SCX Maintenance
        ///     Run As account.
        /// </summary>
        /// <param name="password">
        ///     The password for the managed host user.
        /// </param>
        /// <param name="suPassword">
        ///     The password for <i>su</i> elevated privileges on the managed host.
        /// </param>
        /// <returns>
        ///     This method returns IMaintenanceAuthentication that can be used
        ///     with an SCX Maintenance Run As account.
        /// </returns>
        public IMaintenanceAuthenticationData AuthenticationDataFactory(SecureString password, SecureString suPassword)
        {
            return MaintenanceAuthenticationDataFactory(password, suPassword);
        }


        /// <summary>
        ///     Get authentication data that can be used with an SCX Maintenance
        ///     Run As account.
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
        ///     The password for <i>su</i> elevated privileges on the managed host.
        /// </param>
        /// <returns>
        ///     This method returns IMaintenanceAuthentication that can be used
        ///     with an SCX Maintenance Run As account.
        /// </returns>
        public IMaintenanceAuthenticationData AuthenticationDataFactory(string keyfile, SecureString passphrase, SecureString suPassword)
        {
            return MaintenanceAuthenticationDataFactory(keyfile, passphrase, suPassword);
        }


        /// <summary>
        ///     Construct an SCX Maintenance Run As account.
        /// </summary>
        /// <param name="name">
        ///     The name of the Run As account.
        /// </param>
        /// <param name="description">
        ///     The description of the Run As account.
        /// </param>
        /// <param name="username">
        ///     The name of the managed host user associated with the Run As
        ///     account.
        /// </param>
        /// <param name="authenticationData">
        ///     The authentication data for the managed host user.
        /// </param>
        public MaintenanceRunAsAccount(string name, string description, string username, IMaintenanceAuthenticationData authenticationData)
            : base(CreateScxRunAsAccount(name, description, username, authenticationData as IAuthenticationDataGenerator))
        {
        }


        /// <summary>
        ///     Construct an SCX Maintenance Run As account from the core object.
        /// </summary>
        /// <param name="scxRunAsAccount">
        ///     An ScxRunAsAccount core object to wrap as an SCX Maintenance Run As account.
        /// </param>
        public MaintenanceRunAsAccount(ScxRunAsAccount scxRunAsAccount)
            : base(scxRunAsAccount)
        {
        }


        public static IMaintenanceAuthenticationData MaintenanceAuthenticationDataFactory(SecureString password, bool useSudo)
        {
            return new PasswordAuthenticationDataGenerator(CredentialSetUsage.Maintenance, password, useSudo);
        }


        public static IMaintenanceAuthenticationData MaintenanceAuthenticationDataFactory(string keyfile, SecureString passphrase, bool useSudo)
        {
            return new SshKeyAuthenticationDataGenerator(keyfile, passphrase, useSudo);
        }


        public static IMaintenanceAuthenticationData MaintenanceAuthenticationDataFactory(SecureString password, SecureString suPassword)
        {
            return new PasswordAuthenticationDataGenerator(password, suPassword);
        }


        public static IMaintenanceAuthenticationData MaintenanceAuthenticationDataFactory(string keyfile, SecureString passphrase, SecureString suPassword)
        {
            return new SshKeyAuthenticationDataGenerator(keyfile, passphrase, suPassword);
        }
    }
}