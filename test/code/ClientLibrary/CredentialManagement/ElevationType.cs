// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ElevationType.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement
{
    /// <summary>
    ///     This enumerates the types of user privilege elevation that are
    ///     supported for SCX Run As Accounts.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         None indicates that privilege elevation is not specified for the
    ///         associated account.  However, it is still possible that the user
    ///         for the associated credential could be a privileged user, such as
    ///         the <i>root</i> user.
    ///     </para><para>
    ///         SudoElevation indicates that privileges can be elevated using
    ///         the POSIX <i>sudo(8)</i> command, which allows the current user
    ///         to run commands with <i>root</i> user privileges.  This method
    ///         of elevation does not require any secondary authentication.
    ///         This method of elevation requires that the associated user
    ///         credentials have been granted permission in the <i>sudoers(5)</i>
    ///         configuration file.
    ///     </para><para>
    ///         SuElevation indicates that privileges can be elevated using
    ///         the POSIX <i>su(1)</i> command, which substitutes the effective
    ///         user and group IDs with that of the <i>root</i> user.  This
    ///         method requires the authentication data to contain both the user
    ///         password and the <i>root</i> user password.
    ///     </para>
    /// </remarks>
    public enum ElevationType
    {
        None,
        SudoElevation,
        SuElevation
    }
}