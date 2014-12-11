// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IScxRunAsAccount.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement
{
    using System;

    public interface IScxRunAsAccount
    {
        /// <summary>
        ///     Gets the unique identifier assigned to the Run As account.
        /// </summary>
        Guid Id { get; }


        /// <summary>
        ///     Gets the type of the Run As account, either Monitoring or Maintenance.
        /// </summary>
        ScxRunAsAccountType Type { get; }


        /// <summary>
        ///     Gets or sets the name of the Run As account.
        /// </summary>
        /// <remarks>
        ///     Setting <see langword="null"/> values are silently ignored,
        ///     i.e. the property is <i>not</i> changed.
        /// </remarks>
        string Name { get; set; }


        /// <summary>
        ///     Gets or sets the description of the Run As account.
        /// </summary>
        /// <remarks>
        ///     Setting <see langword="null"/> values are silently ignored,
        ///     i.e. the property is <i>not</i> changed.  An empty string
        ///     is allowed.
        /// </remarks>
        string Description { get; set; }


        /// <summary>
        ///     Gets or sets the name of the managed host user associated with
        ///     the Run As account.
        /// </summary>
        /// <remarks>
        ///     Setting <see langword="null"/> values are silently ignored,
        ///     i.e. the property is <i>not</i> changed.
        /// </remarks>
        string Username { get; set; }


        /// <summary>
        ///     Gets the date and time of the last modification to the Run As account.
        /// </summary>
        DateTime LastModified { get; }
    }
}