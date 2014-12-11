// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMaintenanceRunAsAccount.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement
{
    using System.Security;

    /// <summary>
    ///     This interface creates a named type for SCX Maintenance Run As accounts.
    /// </summary>
    public interface IMaintenanceRunAsAccount : IScxRunAsAccount
    {
        /// <summary>
        ///     Sets the authentication data for the Run As account.
        /// </summary>
        /// <remarks>
        ///     Setting <see langword="null"/> values are silently ignored,
        ///     i.e. the property is <i>not</i> changed.
        /// </remarks>
        IMaintenanceAuthenticationData AuthenticationData { set; }


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
        IMaintenanceAuthenticationData AuthenticationDataFactory(SecureString password, bool useSudo);


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
        IMaintenanceAuthenticationData AuthenticationDataFactory(string keyfile, SecureString passphrase, bool useSudo);


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
        IMaintenanceAuthenticationData AuthenticationDataFactory(SecureString password, SecureString suPassword);


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
        IMaintenanceAuthenticationData AuthenticationDataFactory(string keyfile, SecureString passphrase, SecureString suPassword);
    }
}