//-----------------------------------------------------------------------
// <copyright file="ManageBasicAuthenticationAcconts.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>a-srinia</author>
// <description></description>
// <history>3/25/2009 5:05:51 PM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Apache.SDK.ApacheSDKHelper
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Security;

    using Microsoft.EnterpriseManagement;
    using Microsoft.EnterpriseManagement.Security;

    /// <summary>
    /// Class for managing RunAs Accounts
    /// </summary>
    public class ManageBasicAuthenticationAccounts : ManageAccounts
    {
        #region Private Fields
        #endregion

        #region Accessors
        #endregion

        #region Public Methods

        /// <summary>
        /// Create this RunAs accounn in the OM
        /// </summary>
        /// <param name="mg">Management group representing connection to ops manager</param>
        public override void CreateAccount(ManagementGroup mg)
        {
            BasicCredentialSecureData runAsAccount = new BasicCredentialSecureData();

            SecureString passwd = new SecureString();
            char[] accountPasswd = this.AccountPassword.ToCharArray(0, this.AccountPassword.Length);
            foreach (char c in accountPasswd)
            {
                passwd.AppendChar(c);
            }

            Debug.Assert(
                false == string.IsNullOrEmpty(this.DisplayName),
                "Display name of the account is not set");

            Debug.Assert(
                false == string.IsNullOrEmpty(this.AccountName),
                "Name of the account is not set");

            this.logger("Creating a Run As Account with display name " + this.DisplayName);

            runAsAccount.Name = this.DisplayName;
            runAsAccount.Description = string.Empty;
            runAsAccount.UserName = this.AccountName;
            runAsAccount.Data = passwd;
            mg.Security.InsertSecureData(runAsAccount);

            this.logger("Created a Run As Account with display name " + this.DisplayName);
        }

        #endregion
    }
}
