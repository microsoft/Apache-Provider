//-----------------------------------------------------------------------
// <copyright file="AccountHelper.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>v-lizhou</author>
// <description></description>
// <history>5/25/2011 2:35:52 PM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Apache.SDK.ApacheSDKHelper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Security;

    using Microsoft.EnterpriseManagement;
    using Microsoft.EnterpriseManagement.Common;
    using Microsoft.EnterpriseManagement.Configuration;
    using Microsoft.EnterpriseManagement.Monitoring;
    using Microsoft.EnterpriseManagement.Security;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;
    using Scx.Test.Common;

    public class AccountHelper
    {
        #region Private Fields

        /// <summary>
        /// ILogger object
        /// </summary>
        private ILogger logger = new ConsoleLogger();

        /// <summary>
        /// Information about the OM server.
        /// </summary>
        private OMInfo info;

        /// <summary>
        /// ManagementGroup object that used to manage the accounts
        /// </summary>
        private ManagementGroup managementGroup;

        #endregion Private Fields

        /// <summary>
        /// Default logger is the <see cref="ConsoleLogger"/>.
        /// </summary>
        public ILogger Logger { set { logger = value; } }

        /// <summary>
        /// Initializes a new instance of the AccountHelper class.
        /// </summary>
        /// <param name="info">OMInfo instance containing the imformation needed to connect to the OM SDK service.</param>
        public AccountHelper(OMInfo info)
        {
            this.info = info;
            this.managementGroup = this.info.LocalManagementGroup;
        }

        #region Static methods
        /// <summary>
        /// Determine whether an account already exists in OM
        /// </summary>
        /// <param name="mg">Local management group</param>
        /// <param name="accountName">The account display name, e.g., 'unixroot'</param>
        /// <returns>Whether the account exists</returns>
        public static bool AccountExists(ManagementGroup mg, string accountName)
        {
            IList<SecureData> accountCollection = mg.Security.GetSecureData();

            return accountCollection.Any(account => account.Name == accountName);
        }

        /// <summary>
        /// Get the RunAs accounn with the specified display name in the OM
        /// </summary>
        /// <param name="mg">Management group representing connection to ops manager</param>
        /// <param name="dispalyName">Account's display name</param>
        /// <returns>MonitoringSecureData representing a RunAs account</returns>
        public static SecureData GetOpsRunAsAccount(ManagementGroup mg, string dispalyName)
        {
            Debug.Assert(
                false == string.IsNullOrEmpty(dispalyName),
                "DisplayName of the account is not set");

            string query = "Name LIKE '" + dispalyName.Trim() + "'";

            SecureDataCriteria runAsAccountCriteria = new SecureDataCriteria(query);
            IList<SecureData> runAsAccounts = mg.Security.GetSecureData(runAsAccountCriteria);
            if (runAsAccounts.Count == 0)
            {
                throw new ManageAccountsException("No Run As account found with " + query);
            }

            Debug.Assert(
                1 == runAsAccounts.Count,
                string.Format("More than one account found with display name {0}", dispalyName));
            return runAsAccounts[0];
        }

        /// <summary>
        /// Gets a read only collection of MonitoringSecureReference types representing RunAs profiles
        /// </summary>
        /// <param name="mg">Management group</param>
        /// <param name="profileDisplayName">Runas profile display name</param>
        /// <returns>Readonly collection of MonitoringSecureReference representing RunAs profiles</returns>
        public static IList<ManagementPackSecureReference> GetOpsRunAsProfiles(ManagementGroup mg, string profileDisplayName)
        {
            Debug.Assert(
                false == string.IsNullOrEmpty(profileDisplayName),
                "Profile display name can't be null or empty");

            string query = string.Format("DisplayName LIKE '{0}'", profileDisplayName);
            ManagementPackSecureReferenceCriteria msrCriteria = new ManagementPackSecureReferenceCriteria(query);
            IList<ManagementPackSecureReference> runAsProfiles = mg.Security.GetSecureReferences(msrCriteria);

            if (runAsProfiles.Count == 0)
            {
                throw new ManageAccountsException("No Run As profiles found with " + query);
            }

            Debug.Assert(
                1 == runAsProfiles.Count,
                string.Format("More than one profile found for display name '{0}'", profileDisplayName));
            return runAsProfiles;
        }

        /// <summary>
        /// Get Monitoring object list
        /// </summary>
        /// <param name="mg">ManagementGroup representing connection to OM</param>
        /// <returns>Readonly collection of MonitoringObject representing monitoring objects</returns>
        public static IList<MonitoringObject> GetMonitoringObjectList(ManagementGroup mg)
        {
            IList<ManagementPackClass> managePackClasses = mg.EntityTypes.GetClasses();

            if (managePackClasses.Count == 0)
            {
                throw new ManageAccountsException("No management pack class found");
            }

            IObjectReader<MonitoringObject> monitoringObjects = mg.EntityObjects.GetObjectReader<MonitoringObject>(managePackClasses[0], ObjectQueryOptions.Default);

            if (monitoringObjects.Count == 0)
            {
                throw new ManageAccountsException("No monitoring object found");
            }

            return monitoringObjects.GetRange(0, monitoringObjects.Count);
        }

        /// <summary>
        /// Get profile display name
        /// </summary>
        /// <param name="profileType">UNIX\Linux profile type</param>
        /// <returns>Profile display name</returns>
        public static string GetProfile(string profileType)
        {
            string profile = string.Empty;
            switch (profileType)
            {
                case "actionaccount":
                    profile = "UNIX/Linux Action Account";
                    break;
                case "privilegedaccount":
                    profile = "UNIX/Linux Privileged Account";
                    break;
                case "agentmaintenance":
                    profile = "UNIX/Linux Agent Maintenance Account";
                    break;
            }

            return profile;
        }

        /// <summary>
        /// Decrypte secure string
        /// </summary>
        /// <param name="secureString">The given secure string.</param>        
        /// <returns>Clear text of the secure string.</returns>
        public static string DecryptSecureString(SecureString secureString)
        {
            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(ptr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(ptr);
            }
        }
        #endregion

        /// <summary>
        /// Create a Basic Authentication RunAs account in the OM
        /// </summary>
        /// <param name="displayName">Account's display name</param>
        /// <param name="userName">Account's user name</param>
        /// <param name="password">Account's password</param>
        public void CreateBasicAuthenAccount(string displayName, string userName, string password)
        {
            this.logger.Write(string.Format("Creating a Baisc Authentication Run As Account with display name '{0}'.", displayName));

            if (string.IsNullOrEmpty(displayName))
            {
                throw new ArgumentNullException("Display name of the account is not set");
            }

            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException("Name of the account is not set");
            }
           
            BasicCredentialSecureData runAsAccount = new BasicCredentialSecureData();

            SecureString passwd = new SecureString();
            char[] accountPasswd = password.ToCharArray(0, password.Length);
            foreach (char c in accountPasswd)
            {
                passwd.AppendChar(c);
            }

            runAsAccount.Name = displayName;
            runAsAccount.Description = string.Empty;
            runAsAccount.UserName = userName;
            runAsAccount.Data = passwd;
     
            this.managementGroup.Security.InsertSecureData(runAsAccount);

            // Set secure distribution to "Less Secure"
            ManagementGroupConnection managementGroupConnection = new ManagementGroupConnection(this.managementGroup);
            var distributionHealthServices = new ApprovedHealthServicesForDistribution<EnterpriseManagementObject>
               {
                   Result = ApprovedHealthServicesResults.All
               };
            managementGroupConnection.SetRunAsAccountDistribution(runAsAccount, distributionHealthServices);
        }

        /// <summary>
        /// Create a Monitoring RunAs account in the OM
        /// </summary>
        /// <param name="displayName">Account's display name</param>
        /// <param name="userName">Account's user name</param>
        /// <param name="password">Account's password</param>
        /// <param name="elevationType">Elevation type: "" or "sudo"</param>
        /// <param name="secureDistribution">Indicates if the run as account distribution should be secured.</param>
        public void CreateMonitoringAccount(string displayName, string userName, string password, string elevationType, bool secureDistribution = false)
        {
            this.logger.Write(string.Format("Creating Monitoring account with display name '{0}': Username '{1}', Elevation '{2}'", displayName, userName, elevationType));

            if (string.IsNullOrEmpty(displayName))
            {
                throw new ArgumentNullException("Display name of the account is not set");
            }

            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException("Name of the account is not set");
            }

            if (elevationType != string.Empty && elevationType != "sudo")
            {
                throw new ArgumentException("Elevation type is not none or sudo");
            }

            Credential credential = new Credential(userName, elevationType, password);
            this.CreateScxRunAsAccount(
                credential.XmlUserName,
                credential.Password,
                displayName,
                ScxCredentialRef.RunAsAccountType.Monitoring, 
                secureDistribution);
        }

        /// <summary>
        /// Create an Agent Maintenance account with the specified info
        /// </summary>
        /// <param name="displayName">Account's display name</param>
        /// <param name="userName">Account's user name</param>
        /// <param name="password">Account's password</param>
        /// <param name="elevationType">Elevation type: "", "su" or "sudo"</param>
        /// <param name="suPassword">su password</param>
        /// <param name="sshKeyPath">SSH key file path</param>
        /// <param name="sshPassphrase">SSH key passphrase </param>
        /// <param name="secureDistribution">Indicates if the run as account distribution should be secured.</param>
        public void CreateAgentMaintenanceAccount(
            string displayName,
            string userName,
            string password,
            string elevationType,
            string suPassword,
            string sshKeyPath,
            string sshPassphrase,
            bool secureDistribution = false)
        {
            this.logger.Write(string.Format("Creating an Agent Maitenance account with display name '{0}': Username '{1}', Elevation '{2}', Key File '{3}'",
                displayName,
                userName,
                elevationType,
                sshKeyPath));

            if (string.IsNullOrEmpty(displayName))
            {
                throw new ArgumentNullException("Display name of the account is not set");
            }

            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException("Name of the account is not set");
            }

            if (elevationType != string.Empty && elevationType != "sudo" && elevationType != "su")
            {
                throw new ArgumentException("Elevation type is not none, su or sudo");
            }

            // Get SSHKey from the specified sshkey file if it exists.
            string sshKey = string.Empty;
            if (!string.IsNullOrEmpty(sshKeyPath))
            {
                if (File.Exists(sshKeyPath))
                {
                    List<string> keyContents = new List<string>();
                    using (StreamReader reader = File.OpenText(sshKeyPath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            keyContents.Add(line);
                        }
                    }

                    SecureString sshKeyValue = new PuttyKeyHandler(keyContents).Validate();
                    sshKey = DecryptSecureString(sshKeyValue);
                }
                else
                {
                    throw new ArgumentException(string.Format("Key file '{0}' does not exist", sshKeyPath));
                }
            }

            Credential credential = new Credential(userName, elevationType, password, suPassword, sshKey, sshPassphrase);
            this.CreateScxRunAsAccount(
                credential.XmlUserName, 
                credential.XmlPassword, 
                displayName,
                ScxCredentialRef.RunAsAccountType.Maintenance, 
                secureDistribution, 
                !string.IsNullOrEmpty(sshKey));
        }

        /// <summary>
        /// Delete the specified RunAs account in the OM
        /// </summary>
        /// <param name="displayName">Account' display name</param>
        public void DeleteAccount(string displayName)
        {
            this.logger.Write(string.Format("Deleting Scx Run As account '{0}'", displayName));

            SecureData runAsAccount = AccountExists(this.managementGroup, displayName) ? GetOpsRunAsAccount(this.managementGroup, displayName) : null;
            if (runAsAccount != null)
            {
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
                    // No scxCredentialRef related to this account. Just delete the basic account.
                    this.managementGroup.Security.DeleteSecureData(runAsAccount);
                }

                if (credentialRef != null)
                {
                    scxRunAsAccountFactory.DeleteScxRunAsAccount(credentialRef);
                }
            }
        }

        /// <summary>
        /// Adds the RunAs account to an existing profile to manage all health services.
        /// </summary>
        /// <param name="profileDisplayName">Display name of the profile</param>
        /// <param name="accountName">Account' display name</param>
        /// <param name="targetObjectName">Target object name</param>
        public void AddAccountToExistingProfile(string profileDisplayName, string accountName, string targetObjectName = "")
        {
            // Add RunAs Account to RunAs Profile
            this.logger.Write(string.Format("Adding Run As Account '{0}' to Run As Profile '{1}'", accountName, profileDisplayName));

            SecureData runAsAccount = GetOpsRunAsAccount(this.managementGroup, accountName);
            IList<ManagementPackSecureReference> runAsProfiles = GetOpsRunAsProfiles(this.managementGroup, profileDisplayName);

            if (string.IsNullOrEmpty(targetObjectName))
            {
                ManagementPackClassCriteria classesQuery = new ManagementPackClassCriteria(string.Format("Name = '{0}'", "Microsoft.SystemCenter.HealthService"));
                IList<ManagementPackClass> healthServiceMonitorClasses =
                    this.managementGroup.EntityTypes.GetClasses(classesQuery);

                if (healthServiceMonitorClasses.Count == 0)
                {
                    throw new ManageAccountsException("No health service monitoring class found");
                }

                IObjectReader<MonitoringObject> healthServices = 
                    this.managementGroup.EntityObjects.GetObjectReader<MonitoringObject>(healthServiceMonitorClasses[0], ObjectQueryOptions.Default);

                if (healthServices.Count == 0)
                {
                    throw new ManageAccountsException("No health services found");
                }
                
                Guid healthServiceId = healthServices.GetData(0).Id;

                if (runAsAccount.Id != null)
                {
                    SecureDataHealthServiceReference secureReferenceData =
                        new SecureDataHealthServiceReference((Guid)runAsAccount.Id, runAsProfiles[0].Id, healthServiceId);
                    this.managementGroup.Security.InsertSecureDataHealthServiceReference(secureReferenceData);

                    this.logger.Write(string.Format("Added Run As Account '{0}' to Run As Profile '{1}'.", accountName, profileDisplayName));
                }
            }
            else
            {
                IList<MonitoringObject> monitoringObjectList = GetMonitoringObjectList(this.managementGroup);

                IEnumerable<MonitoringObject> query = from o in monitoringObjectList
                                                      where o.DisplayName.ToString().Contains(targetObjectName) || o.FullName.ToString().Contains(targetObjectName)
                                                      select o;

                if (query.Count() == 0)
                {
                    throw new ManageAccountsException("No matched monitoring object found");
                }

               //// TODO: Add the account with the profile with the specified target object. 
                throw new NotImplementedException("TODO: Add the account with the profile with the specified target object. ");
            }
        }

        /// <summary>
        /// Delete the specified RunAs account from an existing profile
        /// </summary>
        /// <param name="profileDisplayName">Display name of the profile</param>
        /// <param name="accountName">Account' display name</param>
        /// <param name="targetObjectName">Host Name</param>
        public void DeleteAccountFromAnExistingProfile(string profileDisplayName, string accountName, string targetObjectName="")
        {
            SecureData runAsAccount = GetOpsRunAsAccount(this.managementGroup, accountName);
            IList<ManagementPackSecureReference> runAsProfiles = GetOpsRunAsProfiles(this.managementGroup, profileDisplayName);

            if (string.IsNullOrEmpty(targetObjectName))
            {
                IList<SecureDataHealthServiceReference> healthServiceReferences = 
                    runAsAccount.Id != null ? this.managementGroup.Security.GetSecureDataHealthServiceReferenceBySecureDataId((Guid)runAsAccount.Id) : null;
                
                // Remove the health service reference related to the specified profile
                if (healthServiceReferences != null)
                {
                    foreach (SecureDataHealthServiceReference healthServiceReference in healthServiceReferences)
                    {
                        ManagementPackSecureReference secureReference =
                            healthServiceReference.ManagementGroup.Security.GetSecureReference(healthServiceReference.SecureReferenceId);
                        if (string.Compare(secureReference.DisplayName, profileDisplayName, true) == 0)
                        {
                            this.logger.Write(string.Format("Deleting account '{0}' from profile '{1}'", accountName, secureReference.DisplayName));
                            this.managementGroup.Security.DeleteSecureDataHealthServiceReference(healthServiceReference);
                        }
                    }
                }
            }
            else
            {
                IList<MonitoringObject> monitoringObjectList = GetMonitoringObjectList(this.managementGroup);

                IEnumerable<MonitoringObject> query = from o in monitoringObjectList
                                                      where o.FullName.ToString().Contains(targetObjectName)
                                                      select o;

                //// TODO: Delete the account from the profile with the specified target object
                throw new NotImplementedException("TODO: Delete the account from the profile with the specified target object");
            }         
        }

        /// <summary>
        /// Delete the specifiled RunAs account from all existing profiles
        /// </summary>
        /// <param name="accountName">Account' display name</param>
        public void DeleteAccountFromExistingProfiles(string accountName)
        {
            SecureData runAsAccount = GetOpsRunAsAccount(this.managementGroup, accountName);

            if (runAsAccount.Id != null)
            {
                IList<SecureDataHealthServiceReference> healthServiceReferences = this.managementGroup.Security.GetSecureDataHealthServiceReferenceBySecureDataId((Guid)runAsAccount.Id);
                foreach (SecureDataHealthServiceReference healthServiceReference in healthServiceReferences)
                {
                    ManagementPackSecureReference secureReference =
                        healthServiceReference.ManagementGroup.Security.GetSecureReference(healthServiceReference.SecureReferenceId);

                    this.logger.Write("Deleting Run As Account '" + accountName + "' from Run As Profile " + secureReference.DisplayName);
                    this.managementGroup.Security.DeleteSecureDataHealthServiceReference(healthServiceReference);
                }
            }
        }

        /// <summary>
        /// Delete this RunAs account from the profiles and delete the account if it exists in the OM
        /// </summary>
        /// <param name="accountName">Account's name</param>
        public void CleanupAccount(string accountName)
        {
            if (AccountExists(this.managementGroup, accountName))
            {
                // Remove the account from all the existing profiles
                this.DeleteAccountFromExistingProfiles(accountName);

                this.DeleteAccount(accountName);
            }
        }

        /// <summary>
        /// Create this SCX RunAs accounn in the OM
        /// </summary>
        /// <param name="username">Account's username</param>
        /// <param name="password">Account's password</param>
        /// <param name="displayName">Account' display name</param>
        /// <param name="scxRunAsAccountType">ScxRunAsAccountType: Monitoring or AgentMaintenance</param>
        /// <param name="secureDistribution">Indicates if the run as account distribution should be secured.</param>
        /// <param name="isSSHKey">Indicate if this account using SSH Key.</param>
        private void CreateScxRunAsAccount(
            string username,
            string password,
            string displayName,
            ScxCredentialRef.RunAsAccountType scxRunAsAccountType,
            bool secureDistribution = false,
            bool isSSHKey = false)
        {
            SecureString passwd = new SecureString();
            char[] accountPasswd = password.ToCharArray(0, password.Length);
            foreach (char c in accountPasswd)
            {
                passwd.AppendChar(c);
            }

            ManagementGroupConnection managementGroupConnection = new ManagementGroupConnection(this.managementGroup);
            SCXRunAsAccountFactory accountFactory = new SCXRunAsAccountFactory(managementGroupConnection);
            accountFactory.CreateRunAsAccount(
                displayName,
                string.Empty,
                username,
                passwd,
                scxRunAsAccountType,
                isSSHKey,
                secureDistribution);
        }
    }
}
