//-----------------------------------------------------------------------
// <copyright file="ManageAccountHelper.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>v-lizhou</author>
// <description></description>
// <history>5/11/2011 9:27:35 AM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Apache.SDK.ApacheSDKHelper
{
    using System;
    using Microsoft.EnterpriseManagement;
    using Microsoft.EnterpriseManagement.Security;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;

    /// <summary>
    /// Helper class of creating\deleting BasicAuthen\Scx accounts
    /// </summary>
    public class ManageAccountHelper
    {
        #region Private Fields

        /// <summary>
        /// Information about the OM server.
        /// </summary>
        private OMInfo info;

        /// <summary>
        /// Store the local management group
        /// </summary>
        private ManagementGroup managementGroup;

        /// <summary>
        /// Log Delegate to allow writing using a log mechanism provided by the user.
        /// </summary>
        private ManageAccounts.LogDelegate logger;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MonitorHelper class.
        /// </summary>
        /// <param name="info">OMInfo instance containing the imformation needed to connect to the OM SDK service.</param>
        /// <param name="logDelegate">ManageAccounts.LogDelegate object</param>
        public ManageAccountHelper(OMInfo info, ManageAccounts.LogDelegate logDelegate)
        {
            if (logDelegate == null) throw new ArgumentNullException("logDelegate");

            this.info = info;
            this.managementGroup = this.info.LocalManagementGroup;
            this.logger = logDelegate;
        }

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Public Methods
        /// <summary>
        /// Get profile display name
        /// </summary>
        /// <param name="profileType">UNIX\Linux profile type</param>
        /// <returns>Profile display name</returns>
        public static string GetProfile(ScxCredentialSettings.ProfileType profileType)
        {
            string profile = string.Empty;
            switch (profileType)
            {
                case ScxCredentialSettings.ProfileType.ActionAccount:
                    profile = "UNIX/Linux Action Account";
                    break;
                case ScxCredentialSettings.ProfileType.PrivilegedAccount:
                    profile = "UNIX/Linux Privileged Account";
                    break;
                case ScxCredentialSettings.ProfileType.AgentMaintenanceAccount:
                    profile = "UNIX/Linux Agent Maintenance Account";
                    break;
            }

            return profile;
        }

        /// <summary>
        /// Get account display name
        /// </summary>
        /// <param name="accountType">Unix account type</param>
        /// <returns>Account display name</returns>
        public static string GetAccountDisplayName(ScxCredentialSettings.UnixAccountType accountType)
        {
            string displayName = string.Empty;
            switch (accountType)
            {
                case ScxCredentialSettings.UnixAccountType.BasicSuperUser:
                    displayName = "unixroot";
                    break;
                case ScxCredentialSettings.UnixAccountType.BasicNonSuperUser:
                    displayName = "unixnonroot";
                    break;
                case ScxCredentialSettings.UnixAccountType.ScxMonitoringNonPrivileged:
                    displayName = "NonElevatedScxuser";
                    break;
                case ScxCredentialSettings.UnixAccountType.ScxMonitoringWithSudo:
                    displayName = "ElevatedScxuser";
                    break;
                case ScxCredentialSettings.UnixAccountType.ScxMonitoringPrivileged:
                    displayName = "ScxRoot";
                    break;
                case ScxCredentialSettings.UnixAccountType.ScxAgentMaintenance:
                    displayName = "AgentMaintenanceAccount";
                    break;
            }

            return displayName;
        }

        /// <summary>
        /// Verify if the specified account exists
        /// </summary>
        /// <param name="accountType">Unix account type</param>
        /// <param name="accountDisplayName">Account's display name</param>
        /// <returns>true if account exists, otherwise false</returns>
        public bool IsUserAccountExists(ScxCredentialSettings.UnixAccountType accountType, string accountDisplayName = null)
        {
            string displayName = string.IsNullOrEmpty(accountDisplayName) ? GetAccountDisplayName(accountType) : accountDisplayName;

            return ManageAccounts.AccountExists(this.managementGroup, displayName);
        }

        /// <summary>
        /// Verify if the specified account exists
        /// </summary>
        /// <param name="credentialType">Credential type: Basic or sshkey</param>
        /// <param name="accountType">Account's type: privileged or non-privileged</param>
        /// <param name="sshCredentialType">SSH credential type: with passphrase or without passphrase</param>
        /// <param name="elevationType">Elevation type: non, su or sudo</param>
        /// <param name="userName">Unix account' user name</param>
        /// <param name="password">Account's password</param>
        /// <param name="accountDisplayName">Account's display name</param>
        /// <returns>Credential object if account exists, otherwise null</returns>
        public Credential GetAgentAccountCredential(
            ScxCredentialSettings.CredentialType credentialType,
            ScxCredentialSettings.AccountType accountType,
            ScxCredentialSettings.SSHCredentialType sshCredentialType,
            ScxCredentialSettings.ElevationType elevationType,
            string userName,
            string password,
            string accountDisplayName = null)
        {
            string displayName = string.IsNullOrEmpty(accountDisplayName) ? GetAccountDisplayName(ScxCredentialSettings.UnixAccountType.ScxAgentMaintenance) : accountDisplayName;
            SecureData runAsAccount = ManageAccounts.AccountExists(this.managementGroup, displayName) ? ManageAccounts.GetOpsRunAsAccount(this.managementGroup, displayName) : null;
            if (runAsAccount != null)
            {
                ManageScxRunAccounts manageAgentMaintenanceAccount = new ManageScxRunAccounts
                {
                    CredentialType = credentialType,
                    AccountType = accountType,
                    ScxRunAsAccountType = ScxCredentialRef.RunAsAccountType.Maintenance,
                    SSHCredentialType = sshCredentialType,
                    ElevationType = elevationType,
                    AccountName = userName,
                    AccountPassword = password,
                    DisplayName = displayName
                };

                ManagementGroupConnection managementGroupConnection = new ManagementGroupConnection(this.managementGroup);
                SCXRunAsAccountFactory scxRunAsAccountFactory = new SCXRunAsAccountFactory(managementGroupConnection);

                ScxCredentialRef credentialRef = null;
                try
                {
                    if (runAsAccount.Id != null)
                        credentialRef = scxRunAsAccountFactory.GetScxRunAsAccount((Guid)runAsAccount.Id);
                }
                catch (ManagedObjectNotFoundException)
                {
                    // No scxCredentialRef related to this account. 
                }

                if (credentialRef != null && credentialRef.Type == ScxCredentialRef.RunAsAccountType.Maintenance && credentialRef.UserName == manageAgentMaintenanceAccount.Credential.XmlUserName)
                {
                    this.logger(string.Format("Agent Maintenance account with display name '{0}' exists: username : '{1}', Elevation type: '{2}'",
                        displayName,
                        manageAgentMaintenanceAccount.AccountName,
                        manageAgentMaintenanceAccount.ElevationTypeValue));

                    return manageAgentMaintenanceAccount.Credential;
                }
            }

            return null;
        }

        /// <summary>
        /// Creates a Basic authentication account
        /// </summary>
        /// <param name="accountType">Unix account type</param>
        /// <param name="profileType">The UNIX\Linux profile the account associated with</param>
        /// <param name="userName">Account's user name</param>
        /// <param name="password">Account's password</param>
        /// <param name="displayName">Account's display name</param>
        /// <returns>ManageBasicAuthenticationAccounts object</returns>
        public ManageBasicAuthenticationAccounts CreateBasicUserAccount(
            ScxCredentialSettings.UnixAccountType accountType,
            ScxCredentialSettings.ProfileType profileType,
            string userName,
            string password,
            string displayName = null)
        {
            this.logger(string.Format("Creating basic authentication account with username '{0}'...", userName));

            ManageBasicAuthenticationAccounts manageBasicAccounts = new ManageBasicAuthenticationAccounts
            {
                AccountName = userName,
                AccountPassword = password
            };

            this.CreateAccount(manageBasicAccounts, accountType, profileType, displayName);

            return manageBasicAccounts;
        }

        /// <summary>
        /// Creates a scx mornitoring account
        /// </summary>
        /// <param name="accountType">Unix account type</param>
        /// <param name="profileType">The UNIX\Linux profile the account associated with</param>
        /// <param name="userName">Account's user name</param>
        /// <param name="password">Account's password</param>
        /// <param name="displayName">Account's display name</param>
        /// <returns> ManageScxRunAccounts object</returns>
        public ManageScxRunAccounts CreateScxMonitoringAccount(
            ScxCredentialSettings.UnixAccountType accountType,
            ScxCredentialSettings.ProfileType profileType,
            string userName,
            string password,
            string displayName = null)
        {
            this.logger(string.Format("Creating Scx Monitoring account with user name '{0}', elevation type '{1}'...", userName, accountType == ScxCredentialSettings.UnixAccountType.ScxMonitoringWithSudo ? "sudo" : "none"));

            ManageScxRunAccounts manageMonitoringAccounts = new ManageScxRunAccounts()
            {
                AccountName = userName,
                AccountPassword = password,
                ScxRunAsAccountType = ScxCredentialRef.RunAsAccountType.Monitoring
            };

            if (accountType == ScxCredentialSettings.UnixAccountType.ScxMonitoringWithSudo)
            {
                manageMonitoringAccounts.ElevationType = ScxCredentialSettings.ElevationType.Sudo;
            }

            this.CreateAccount(manageMonitoringAccounts, accountType, profileType, displayName);

            return manageMonitoringAccounts;
        }

        /// <summary>
        /// Creates an agent maintenance account and associates it with the "UNIX/Linux Agent Maintenance Account" profile.
        /// </summary>
        /// <param name="credentialType">Credenial type: sshkey or basic</param>
        /// <param name="accountType">Account type: privileged or unprivileged</param>
        /// <param name="sshCredentialType">SSH Credential type: withpassword or withoutpassword</param>
        /// <param name="elevationType">Elevated credential settings type: su, sudo or none </param>
        /// <param name="userName">Account's user name</param>
        /// <param name="password">Account's password</param>
        /// <param name="displayName">Account's display name</param>
        /// <param name="isAssiatedProfile">Assiated with profile</param>
        /// <returns> Credential object </returns>
        public Credential CreateAgentMaintenanceAccount(
            ScxCredentialSettings.CredentialType credentialType,
            ScxCredentialSettings.AccountType accountType,
            ScxCredentialSettings.SSHCredentialType sshCredentialType,
            ScxCredentialSettings.ElevationType elevationType,
            string userName,
            string password,
            string displayName = null,
            bool isAssiatedProfile = true)
        {
            ManageScxRunAccounts manageAgentMaintenanceAccount = new ManageScxRunAccounts()
            {
                AccountName = userName,
                AccountPassword = password,
                ScxRunAsAccountType = ScxCredentialRef.RunAsAccountType.Maintenance,
                CredentialType = credentialType,
                AccountType = accountType,
                SSHCredentialType = sshCredentialType,
                ElevationType = elevationType
            };

            this.CreateAccount(manageAgentMaintenanceAccount, ScxCredentialSettings.UnixAccountType.ScxAgentMaintenance, ScxCredentialSettings.ProfileType.AgentMaintenanceAccount, displayName, isAssiatedProfile);

            return manageAgentMaintenanceAccount.Credential;
        }

        /// <summary>
        /// Delete the existing user accounts from OpsMgr
        /// </summary>
        public void DeleteAccounts()
        {
            this.DeleteAccount(ScxCredentialSettings.UnixAccountType.BasicSuperUser, ScxCredentialSettings.ProfileType.PrivilegedAccount);

            this.DeleteAccount(ScxCredentialSettings.UnixAccountType.BasicNonSuperUser, ScxCredentialSettings.ProfileType.ActionAccount);

            this.DeleteAccount(ScxCredentialSettings.UnixAccountType.ScxMonitoringWithSudo, ScxCredentialSettings.ProfileType.PrivilegedAccount);

            this.DeleteAccount(ScxCredentialSettings.UnixAccountType.ScxMonitoringNonPrivileged, ScxCredentialSettings.ProfileType.ActionAccount);

            this.DeleteAccount(ScxCredentialSettings.UnixAccountType.ScxAgentMaintenance, ScxCredentialSettings.ProfileType.AgentMaintenanceAccount);
        }

        /// <summary>
        /// Delete the priviledged account from OpsMgr
        /// </summary>
        /// <param name="unixAccountType">Unix account type</param>
        /// <param name="profileType">The UNIX\Linux profile the account associated with</param>
        /// <param name="accountDisplayName">Account's display name</param>
        public void DeleteAccount(
            ScxCredentialSettings.UnixAccountType unixAccountType,
            ScxCredentialSettings.ProfileType profileType,
            string accountDisplayName = null)
        {
            ManageAccounts manageAccount = this.RemoveAccountFromProfile(unixAccountType, profileType, accountDisplayName);

            if (manageAccount != null)
            {
                manageAccount.DeleteAccount(this.managementGroup);
            }
        }

        /// <summary>
        /// Delete the priviledged account from OpsMgr
        /// </summary>
        /// <param name="unixAccountType">Unix account type</param>
        /// <param name="profileType">The UNIX\Linux profile the account associated with</param>
        /// <param name="accountDisplayName">Account's display name</param>
        /// <returns>ManageAccounts object if the account exists, otherwise null</returns>
        public ManageAccounts RemoveAccountFromProfile(
            ScxCredentialSettings.UnixAccountType unixAccountType,
            ScxCredentialSettings.ProfileType profileType,
            string accountDisplayName = null)
        {
            ManageAccounts manageAccount = null;
            string displayName = string.IsNullOrEmpty(accountDisplayName) ? GetAccountDisplayName(unixAccountType) : accountDisplayName;

            if (ManageAccounts.AccountExists(this.managementGroup, displayName))
            {

                switch (unixAccountType)
                {
                    case ScxCredentialSettings.UnixAccountType.BasicSuperUser:
                    case ScxCredentialSettings.UnixAccountType.BasicNonSuperUser:
                        manageAccount = new ManageBasicAuthenticationAccounts();
                        break;
                    case ScxCredentialSettings.UnixAccountType.ScxMonitoringNonPrivileged:
                    case ScxCredentialSettings.UnixAccountType.ScxMonitoringWithSudo:
                    case ScxCredentialSettings.UnixAccountType.ScxMonitoringPrivileged:
                    case ScxCredentialSettings.UnixAccountType.ScxAgentMaintenance:
                        manageAccount = new ManageScxRunAccounts();
                        break;
                }

                manageAccount.DisplayName = displayName;
                manageAccount.DeleteThisAccountFromAnExistingProfile(
                    GetProfile(profileType),
                    this.info.LocalManagementGroup);
            }

            return manageAccount;
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Creates a user account
        /// </summary>
        /// <param name="manageAccounts">ManageAccounts object</param>
        /// <param name="accountType">Unix account type</param>
        /// <param name="profileType">The UNIX\Linux profile the account will be associated with</param>
        /// <param name="displayName">Account's display name</param>
        /// <param name="isAssiatedProfile">Assiated with profile</param>
        private void CreateAccount(
            ManageAccounts manageAccounts,
            ScxCredentialSettings.UnixAccountType accountType,
            ScxCredentialSettings.ProfileType profileType,
            string displayName, bool isAssiatedProfile = true)
        {
            manageAccounts.DisplayName = string.IsNullOrEmpty(displayName) ? GetAccountDisplayName(accountType) : displayName;

            manageAccounts.SetLogDelegate(this.logger);
            manageAccounts.CreateAccount(this.info.LocalManagementGroup);

            if (isAssiatedProfile)
            {
                // Associate the account with profile
                manageAccounts.AddThisAccountToExistingProfile(
                    GetProfile(profileType),
                    this.info.LocalManagementGroup);
            }

        }

        #endregion
    }
}