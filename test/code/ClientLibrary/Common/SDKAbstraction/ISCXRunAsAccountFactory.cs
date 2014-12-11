// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISCXRunAsAccountFactory.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using System;
    using System.Collections.Generic;
    using System.Security;

    public interface ISCXRunAsAccountFactory
    {
        #region Public Methods

        /// <summary>
        /// Creates scx run as account, insert it into managementgroup and save it to the database.
        /// </summary>
        /// <param name="displayName">
        /// Display name of the run as account.
        /// </param>
        /// <param name="description">
        /// Description of the run as account.
        /// </param>
        /// <param name="userName">
        /// The user name of the run as account in XML formate. The user name should
        /// in the same formate as the one generated from GetXMLUserName function of CredentialSet.
        /// </param>
        /// <param name="password">
        /// The XML formate password encryped in SecureString. The password should be
        /// the one generated from GetXMLPassword function of CredentialSet.
        /// </param>
        /// <param name="runAsType">
        /// The RunAs account type.
        /// </param>
        /// <param name="isSshKey">
        /// Indicate if this account using SSH Key.
        /// </param>
        /// <param name="secureDistribution">
        /// Indicates if the run as account distribution should be secured.
        /// </param>
        void CreateRunAsAccount(
            string displayName, 
            string description, 
            string userName, 
            SecureString password, 
            ScxCredentialRef.RunAsAccountType runAsType, 
            bool isSshKey, 
            bool secureDistribution);

        /// <summary>
        /// Delete a scx RunAs account
        /// </summary>
        /// <param name="accountRef">
        /// The RunAs account ref.
        /// </param>
        void DeleteScxRunAsAccount(ScxCredentialRef accountRef);

        /// <summary>
        /// Get all scx RunAs account
        /// </summary>
        /// <returns>
        /// The RunAs accounts.
        /// </returns>
        IEnumerable<ScxCredentialRef> GetAllScxRunAsAccount();

        /// <summary>
        /// Gets a RunAs account from its id
        /// </summary>
        /// <param name="id">
        /// The RunAs account id.
        /// </param>
        /// <returns>
        /// The RunAs account reference.
        /// </returns>
        ScxCredentialRef GetScxRunAsAccount(Guid id);

        /// <summary>
        /// Update a scx RunAs account
        /// </summary>
        /// <param name="accountRef">
        /// The RunAs account ref.
        /// </param>
        /// <param name="displayName">
        /// The RunAs account display name.
        /// </param>
        /// <param name="description">
        /// The RunAs account description.
        /// </param>
        /// <param name="userName">
        /// The RunAs account user name.
        /// </param>
        /// <param name="password">
        /// The RunAs account password.
        /// </param>
        /// <param name="isSshKey">
        /// Indicate if SSH key is used.
        /// </param>
        /// <param name="secureDistribution">
        /// Secure distribution of the RunAs account.
        /// </param>
        void UpdateScxRunAsAccount(
            ScxCredentialRef accountRef, 
            string displayName, 
            string description, 
            string userName, 
            SecureString password, 
            bool isSshKey, 
            ScxRunAsAccountSecureDistribution secureDistribution);

        #endregion
    }
}