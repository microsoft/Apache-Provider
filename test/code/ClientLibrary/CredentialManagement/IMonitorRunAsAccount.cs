// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMonitorRunAsAccount.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement
{
    using System.Security;

    /// <summary>
    ///     This interface creates a named type for SCX Monitor Run As accounts.
    /// </summary>
    public interface IMonitorRunAsAccount : IScxRunAsAccount
    {
        /// <summary>
        ///     Sets the authentication data for the Run As account.
        /// </summary>
        /// <remarks>
        ///     Setting <see langword="null"/> values are silently ignored,
        ///     i.e. the property is <i>not</i> changed.
        /// </remarks>
        IMonitorAuthenticationData AuthenticationData { set; }


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
        IMonitorAuthenticationData AuthenticationDataFactory(SecureString password, bool useSudo);
    }
}