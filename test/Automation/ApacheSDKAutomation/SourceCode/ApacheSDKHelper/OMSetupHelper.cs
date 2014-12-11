//-----------------------------------------------------------------------
// <copyright file="OMSetupHelper.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>sris</author>
// <description>Setup class for initial OperationsManager Setup</description>
// <history>5/18/2011 10:58: AM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Apache.SDK.ApacheSDKHelper
{
    using System;
    using System.IO;
    using Microsoft.EnterpriseManagement.Common;
    using Scx.Test.Common;
    using Microsoft.EnterpriseManagement.Security;

    /// <summary>
    /// OMSetupHelper includes the steps needed for running a test pass on all platforms.
    /// This includes setup steps for the Operations Manager installation which would affect all platforms.
    /// </summary>
    public class OMSetupHelper
    {
        #region Private Fields

        /// <summary>
        /// Information about the OM server.
        /// </summary>
        private OMInfo info;

        /// <summary>
        /// Management Pack helper class
        /// </summary>
        private ManageMP manageMP;

        /// <summary>
        /// AccountHelper object 
        /// </summary>
        private AccountHelper accountHelper;

        /// <summary>
        /// ILogger object
        /// </summary>
        private ILogger logger = new ConsoleLogger();

        #endregion Private Fields
        /// <summary>
        /// Default logger is the <see cref="ConsoleLogger"/>.
        /// </summary>
        public ILogger Logger { set { logger = value; } }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the OMSetupHelper class.
        /// </summary>
        public OMSetupHelper()
        {
        }

        #endregion Constructors

        #region Static methods

        /// <summary>
        /// Write the message to console
        /// </summary>
        /// <param name="logMsg">Log message</param>
        public static void LogToConsole(string logMsg)
        {
            ILogger logger = new ConsoleLogger();
            logger.Write(logMsg);
        }

        #endregion

        /// <summary>
        /// Initializes the OM SetupHelper with the specified info. It should be invoked in setup xml file before any other actions.
        /// </summary>
        /// <param name="omserver">OM server name</param>
        /// <param name="omusername">OM username</param>
        /// <param name="omdomain">OM domain name</param>
        /// <param name="ompassword">OM password</param>
        public void InitializeOMSetupHelper(string omserver, string omusername, string omdomain, string ompassword)
        {
            info = new OMInfo(omserver, omusername, omdomain, ompassword);
            logger.Write("OMInfo: " + Environment.NewLine + this.info.ToString());

            manageMP = new ManageMP(LogToConsole, info);
            accountHelper = new AccountHelper(info);
        }

        /// <summary>
        /// Create an account with the specified info
        /// </summary>
        /// <param name="accountType">Account's type: "monitoring", "agentmaintenance" or "basicauth"</param>
        /// <param name="displayName">Account's display name</param>
        /// <param name="userName">Account's user name</param>
        /// <param name="password">Account's password</param>
        /// <param name="elevationType">Elevation type: "", "su" or "sudo"</param>
        /// <param name="suPassword">su password</param>
        /// <param name="sshKeyPath">SSH key file path</param>
        /// <param name="sshPassphrase">SSH key passphrase </param>
        public void CreateAccount(
                                   string accountType,
                                   string displayName,
                                   string userName,
                                   string password = "",
                                   string elevationType = "",
                                   string suPassword = "",
                                   string sshKeyPath = "",
                                   string sshPassphrase = "")
        {
            if (string.IsNullOrEmpty(accountType))
            {
                throw new System.ArgumentNullException("Account Type is empty");
            }

            if (accountType != "monitoring" && accountType != "agentmaintenance" && accountType != "basicauth")
            {
                throw new System.ArgumentException("Account type is not monitoring or agentmaintenance or basicauth");
            }

            if (string.IsNullOrEmpty(userName))
            {
                throw new System.ArgumentNullException("User name is empty");
            }

            if (string.IsNullOrEmpty(displayName))
            {
                throw new System.ArgumentNullException("Display name is empty");
            }

            if (accountHelper == null)
            {
                throw new InvalidOperationException("AccountHelper is not initialized. Please call InitializeOMSetupHelper first.");
            }

            // Delete the account if it already exists.
            accountHelper.CleanupAccount(displayName);

            // Create an account for data given according to the account type
            switch (accountType)
            {
                case "basicauth":
                    accountHelper.CreateBasicAuthenAccount(displayName, userName, password);
                    break;
                case "monitoring":
                    accountHelper.CreateMonitoringAccount(displayName, userName, password, elevationType);
                    break;
                case "agentmaintenance":
                    accountHelper.CreateAgentMaintenanceAccount(displayName, userName, password, elevationType, suPassword, sshKeyPath, sshPassphrase);
                    break;
            }
        }

        /// <summary>
        /// Associate the specified account with the specified profile.
        /// </summary>
        /// <param name="accountName">Account's name</param>
        /// <param name="profileType">Profiles's display name</param>
        /// <param name="target">Target object's name</param>
        public void AssociateProfile(string accountName, string profileType, string target = "")
        {
            if (string.IsNullOrEmpty(profileType))
            {
                throw new System.ArgumentNullException("Profile Type is empty");
            }

            if (profileType != "actionaccount" && profileType != "agentmaintenance" && profileType != "privilegedaccount")
            {
                throw new System.ArgumentException("Profile type is not actionaccount or agentmaintenance or privilegedaccount");
            }

            // check for displayname is null or empty
            if (string.IsNullOrEmpty(accountName))
            {
                throw new System.ArgumentNullException("Account name is empty");
            }

            // check if an account exists with the displayname throw exception if not
            if (!AccountHelper.AccountExists(this.info.LocalManagementGroup, accountName))
            {
                throw new System.ArgumentException("The account does not exist.");
            }

            if (accountHelper == null)
            {
                throw new InvalidOperationException("AccountHelper is not initialized. Please call InitializeOMSetupHelper first.");
            }

            accountHelper.AddAccountToExistingProfile(AccountHelper.GetProfile(profileType), accountName, target);
        }

        /// <summary>
        /// Import Management Packs
        /// </summary>
        /// <param name="pathName">ManagementPack folder full path</param>
        /// <param name="filePattern">file pattern for MP file to load.Ex: *.xml or *. MP</param>
        public void ImportMPs(string pathName = "", string filePattern = "")
        {
            if (string.IsNullOrEmpty(pathName))
            {
                throw new System.ArgumentNullException("pathName is null or empty");
            }

            if (!Directory.Exists(pathName))
            {
                throw new System.IO.DirectoryNotFoundException("folder path not found :" + pathName);
            }

            if (this.manageMP == null)
            {
                throw new InvalidOperationException("ManageMP class instance is not initialized. Please call InitializeOMSetupHelper first.");
            }

            this.manageMP.ManagementPackDirectory = pathName;

            if (string.IsNullOrEmpty(filePattern))
            {
                logger.Write("Importing all *.Mp's from " + pathName);
                this.manageMP.ImportManagementPacks();
            }
            else
            {
                logger.Write("Importing MP's of filepattern: " + filePattern + " from " + pathName);
                this.manageMP.ImportManagementPacks(filePattern);

            }
        }

        /// <summary>
        /// Uninstall UNIX Management Packs
        /// </summary>
        /// <param name="filePattern">file pattern for MP file to load.Ex: Microsoft.UNIX or Microsoft.ACS</param>
        public void UninstallMPs(string filePattern)
        {
            if (string.IsNullOrEmpty(filePattern))
            {
                throw new System.ArgumentNullException("filePattern is null or empty");
            }

            this.manageMP.UninstallManagementPacks(filePattern);
        }

        /// <summary>
        /// Uninstall UNIX Management Packs Bundle
        /// </summary>
        /// <param name="filePattern">file pattern for MP file to load.Ex: Microsoft.UNIX or Microsoft.ACS</param>
        public void UninstallBundle(string filePattern)
        {
            if (string.IsNullOrEmpty(filePattern))
            {
                throw new System.ArgumentNullException("filePattern is null or empty");
            }

            this.manageMP.UninstallBundle(filePattern);
        }

        /// <summary>
        /// Import Management Packs
        /// </summary>
        /// <param name="pathName">ManagementPack folder full path</param>
        /// <param name="filePattern">file pattern for MP file to load.Ex: *.xml or *. MP</param>
        public void ImportBundle(string pathName = "", string filePattern = "")
        {
            if (string.IsNullOrEmpty(pathName))
            {
                throw new System.ArgumentNullException("PathName is null or empty");
            }

            if (!Directory.Exists(pathName))
            {
                throw new System.IO.DirectoryNotFoundException("Folder path not found :" + pathName);
            }

            if (this.manageMP == null)
            {
                throw new InvalidOperationException("ManageMP class instance is not initialized. Please call InitializeOMSetupHelper first.");
            }

            this.manageMP.ManagementPackDirectory = pathName;

            if (string.IsNullOrEmpty(filePattern))
            {
                logger.Write("File pattern is null or empty , defaulting to *.mpb");
                this.manageMP.ImportBundle();
            }
            else
            {
                logger.Write("Importing bundle of MPs' filepattern: " + filePattern + " from " + pathName);
                this.manageMP.ImportBundle(filePattern);
            }
        }
    }
}
