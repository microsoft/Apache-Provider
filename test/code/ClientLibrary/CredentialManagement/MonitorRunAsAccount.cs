// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MonitorRunAsAccount.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement
{
    using System.Diagnostics;
    using System.Security;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core;

    public class MonitorRunAsAccount : RunAsAccountBase, IMonitorRunAsAccount
    {
        /// <summary>
        ///     Gets the primary PosixHostCredential for this Run As account.
        /// </summary>
        protected override PosixHostCredential PrimaryCredential
        {
            get
            {
                PosixHostCredential wsManCredential = RunAsAccount.Credential.CredentialsForAll(CredentialUsage.WsManDiscovery);
                Debug.Assert(wsManCredential != null, "SCX Monitor Run As accounts must have a PosixHostCredential with CredentialUsage.WsManDiscovery!");

                return wsManCredential;
            }
        }


        /// <summary>
        ///     Sets the authentication data for the Run As account.
        /// </summary>
        /// <remarks>
        ///     Setting <see langword="null"/> values are silently ignored,
        ///     i.e. the property is <i>not</i> changed.
        /// </remarks>
        public IMonitorAuthenticationData AuthenticationData
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
        ///     Get authentication data that can be used with an SCX Monitor
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
        ///     This method returns IMonitorAuthenticationData that can be used
        ///     with an SCX Monitor Run As account.
        /// </returns>
        public IMonitorAuthenticationData AuthenticationDataFactory(SecureString password, bool useSudo)
        {
            return MonitorAuthenticationDataFactory(password, useSudo);
        }


        /// <summary>
        ///     Construct an SCX Monitor Run As account.
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
        public MonitorRunAsAccount(string name, string description, string username, IMonitorAuthenticationData authenticationData)
            : base(CreateScxRunAsAccount(name, description, username, authenticationData as IAuthenticationDataGenerator))
        {
        }


        /// <summary>
        ///     Construct an SCX Monitor Run As account from the core object.
        /// </summary>
        /// <param name="scxRunAsAccount">
        ///     An ScxRunAsAccount core object to wrap as an SCX Monitor Run As account.
        /// </param>
        public MonitorRunAsAccount(ScxRunAsAccount scxRunAsAccount)
            : base(scxRunAsAccount)
        {
        }


        public static IMonitorAuthenticationData MonitorAuthenticationDataFactory(SecureString password, bool useSudo)
        {
            return new PasswordAuthenticationDataGenerator(CredentialSetUsage.Monitoring, password, useSudo);
        }
    }
}