// <copyright file="ManageAccounts.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// Created by Srinivas Alamuru (a-srinia@microsoft.com)
// Created date  2009-03-24
// Class for managing accounts (creting and associating with profiles)

namespace Scx.Test.Apache.SDK.ApacheSDKHelper
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Microsoft.EnterpriseManagement;
    using Microsoft.EnterpriseManagement.Administration;
    using Microsoft.EnterpriseManagement.Common;
    using Microsoft.EnterpriseManagement.Configuration;
    using Microsoft.EnterpriseManagement.Monitoring;
    using Microsoft.EnterpriseManagement.Security;

    /// <summary>
    /// Abstract class for managing accounts on OM
    /// </summary>
    public abstract class ManageAccounts
    {
        /// <summary>
        /// Log Delegate to allow writing using a log mechanism provided by the user.
        /// </summary>
        protected LogDelegate logger = NullLogDelegate;

        /// <summary>
        /// Profiles added
        /// </summary>
        protected Hashtable addedProfiles = new Hashtable(1);

        #region Delegates

        /// <summary>
        /// Delegate allowing output to the log file without having to accept an external class instance such as IContext.
        /// </summary>
        /// <param name="logMsg">Log message</param>
        public delegate void LogDelegate(string logMsg);

        #endregion

        /// <summary>
        /// Gets or sets RunAs Account Name
        /// </summary>
        public string AccountName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets RunAs Account Display Name
        /// </summary>
        public string DisplayName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets RunAs Account Password
        /// </summary>
        public string AccountPassword
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the RunAs profiles this account is added to
        /// </summary>
        public virtual ArrayList AssociatedProfiles
        {
            get
            {
                ArrayList associatedProfiles = new ArrayList(this.addedProfiles.Count);
                associatedProfiles.AddRange(this.addedProfiles.Keys);
                return associatedProfiles;
            }
        }

        #region Static Methods

        /// <summary>
        /// Trivial log delegate function which does nothing.
        /// </summary>
        /// <param name="logMsg">Log message</param>
        public static void NullLogDelegate(string logMsg)
        {
            // do nothing
        }

        /// <summary>
        /// Determine whether an account already exists in OM
        /// </summary>
        /// <param name="mg">Local management group</param>
        /// <param name="accountName">The account display name, e.g., 'unixroot'</param>
        /// <returns>Whether the account exists</returns>
        public static bool AccountExists(ManagementGroup mg, string accountName)
        {
            IList<SecureData> accountCollection = mg.Security.GetSecureData();

            foreach (SecureData account in accountCollection)
            {
                if (account.Name == accountName)
                {
                    return true;
                }
            }

            return false;
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

        #endregion

        /// <summary>
        /// Create this RunAs accounn in the OM
        /// </summary>
        /// <param name="mg">Management group representing connection to ops manager</param>
        public abstract void CreateAccount(ManagementGroup mg);

        /// <summary>
        /// Delete this RunAs account in the OM
        /// </summary>
        /// <param name="mg">Management group representing connection to ops manager</param>
        public virtual void DeleteAccount(ManagementGroup mg)
        {
            SecureData runAsAccount = GetOpsRunAsAccount(mg, this.DisplayName);
            mg.Security.DeleteSecureData(runAsAccount);
        }

        /// <summary>
        /// Gets a read only collection of MonitoringSecureReference types representing RunAs profiles
        /// </summary>
        /// <param name="profileDisplayName">Runas profile display name</param>
        /// <param name="mg">Management group</param>
        /// <returns>Readonly collection of MonitoringSecureReference representing RunAs profiles</returns>
        public IList<ManagementPackSecureReference> GetOpsRunAsProfiles(string profileDisplayName, ManagementGroup mg)
        {
            Debug.Assert(
                false == string.IsNullOrEmpty(profileDisplayName),
                "Profile display name can't be null or empty");

            IList<ManagementPackSecureReference> runAsProfiles;
            string query = string.Format("DisplayName LIKE '{0}'", profileDisplayName);
            ManagementPackSecureReferenceCriteria msrCriteria = new ManagementPackSecureReferenceCriteria(query);
            runAsProfiles = mg.Security.GetSecureReferences(msrCriteria);

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
        /// Adds the RunAs account to an existing profile
        /// </summary>
        /// <param name="profileDisplayName">Display name of the profile</param>
        /// <param name="mg">ManagementGroup representing connection to ops manager</param>
        public void AddThisAccountToExistingProfile(string profileDisplayName, ManagementGroup mg)
        {
            // Add RunAs Account to RunAs Profile
            this.logger("Add Run As Account '" + this.DisplayName + "' to Run As Profile " + profileDisplayName);

            SecureData runAsAccount = GetOpsRunAsAccount(mg, this.DisplayName);

            IList<ManagementPackSecureReference> runAsProfiles = this.GetOpsRunAsProfiles(profileDisplayName, mg);

            ManagementPackClassCriteria classesQuery = new ManagementPackClassCriteria(string.Format("Name = '{0}'", "Microsoft.SystemCenter.HealthService"));
            IList<ManagementPackClass> healthServiceMonitorClasses =
                mg.EntityTypes.GetClasses(classesQuery);

            if (healthServiceMonitorClasses.Count == 0)
            {
                throw new ManageAccountsException("No health service monitoring class found");
            }

            ManagementPackClass healthServiceClass = healthServiceMonitorClasses[0];

            IObjectReader<MonitoringObject> healthServices = mg.EntityObjects.GetObjectReader<MonitoringObject>(healthServiceClass, ObjectQueryOptions.Default);

            if (healthServices.Count == 0)
            {
                throw new ManageAccountsException("No health services found");
            }

            // If windows machines are discovered by OM, there will be multiple healthServices existed. Here we will add runAsAccount to OM machine.
            Guid healthServiceId = healthServices.GetData(0).Id;
            var monitoringObjects = from o in healthServices
                    where o.FullName.IndexOf(System.Net.Dns.GetHostName(), StringComparison.CurrentCultureIgnoreCase) >= 0
                    select o;
            if (monitoringObjects.Count() > 0)
            {
                healthServiceId = monitoringObjects.First().Id;
            }

            if (runAsAccount.Id != null)
            {
                // Remove the account associated with this profile before adding the account to it
                IList<SecureDataHealthServiceReference> healthServiceReferences = mg.Security.GetSecureDataHealthServiceReferenceBySecureReferenceId(runAsProfiles[0].Id);
                foreach (SecureDataHealthServiceReference healthServiceReference in healthServiceReferences)
                {
                    this.logger("Deleting accounts from profile '" + runAsProfiles[0].DisplayName + "' which associated with Health Service");
                    mg.Security.DeleteSecureDataHealthServiceReference(healthServiceReference);
                }

                SecureDataHealthServiceReference secureReferenceData =
                    new SecureDataHealthServiceReference((Guid)runAsAccount.Id, runAsProfiles[0].Id, healthServiceId);
                mg.Security.InsertSecureDataHealthServiceReference(secureReferenceData);
            }

            this.addedProfiles.Add(profileDisplayName, this.AccountName);

            this.logger("Run As Account added to Run As Profile");
        }

        /// <summary>
        /// Adds the RunAs account to an existing profile to manage all health services.
        /// </summary>
        /// <param name="profileDisplayName">Display name of the profile</param>
        /// <param name="mg">ManagementGroup representing connection to ops manager</param>
        public void AddThisAccountToExistingProfileToAllHealthService(string profileDisplayName, ManagementGroup mg)
        {
            // Add RunAs Account to RunAs Profile
            this.logger("Add Run As Account '" + this.DisplayName + "' to Run As Profile " + profileDisplayName);

            SecureData runAsAccount = GetOpsRunAsAccount(mg, this.DisplayName);

            IList<ManagementPackSecureReference> runAsProfiles = this.GetOpsRunAsProfiles(profileDisplayName, mg);

            IList<MonitoringObject> healthServiceList = this.GetHealthServiceList(mg);

            foreach (MonitoringObject healthService in healthServiceList)
            {
                if (runAsAccount.Id != null)
                {
                    SecureDataHealthServiceReference secureReferenceData =
                        new SecureDataHealthServiceReference((Guid)runAsAccount.Id, runAsProfiles[0].Id, healthService.Id);
                    mg.Security.InsertSecureDataHealthServiceReference(secureReferenceData);
                }
            }

            this.addedProfiles.Add(profileDisplayName, this.AccountName);

            this.logger("Run As Account added to Run As Profile");
        }

        /// <summary>
        /// Adds the RunAs account to an existing profile to manage all health services.
        /// </summary>
        /// <param name="profileDisplayName">Display name of the profile</param>
        /// <param name="mg">ManagementGroup representing connection to ops manager</param>
        /// <param name="name">the name of the host</param>
        public void AddThisAccountToExistingProfile(string profileDisplayName, ManagementGroup mg, string name)
        {
            // Add RunAs Account to RunAs Profile
            this.logger("Add Run As Account '" + this.DisplayName + "' to Run As Profile " + profileDisplayName);

            SecureData runAsAccount = GetOpsRunAsAccount(mg, this.DisplayName);
            IList<ManagementPackSecureReference> runAsProfiles = this.GetOpsRunAsProfiles(profileDisplayName, mg);

            IList<MonitoringObject> healthServiceList = this.GetHealthServiceList(mg);

            IEnumerable<MonitoringObject> query = from o in healthServiceList
                                                  where o.FullName.ToString().Contains(name)
                                                  select o;
            if (runAsAccount.Id != null)
            {
                SecureDataHealthServiceReference secureReferenceData =
                    new SecureDataHealthServiceReference((Guid)runAsAccount.Id, runAsProfiles[0].Id, query.First().Id);
                mg.Security.InsertSecureDataHealthServiceReference(secureReferenceData);
            }

            this.addedProfiles.Add(profileDisplayName, this.AccountName);

            this.logger("Run As Account added to Run As Profile");
        }

        /// <summary>
        /// Delete this RunAs account from an existing profile
        /// </summary>
        /// <param name="profileDisplayName">Display name of the profile</param>
        /// <param name="mg">ManagementGroup representing connection to ops manager</param>
        public void DeleteThisAccountFromAnExistingProfile(string profileDisplayName, ManagementGroup mg)
        {
            // Add RunAs Account to RunAs Profile
            this.logger("Delete Run As Account '" + this.DisplayName + "' to Run As Profile " + profileDisplayName);

            SecureData runAsAccount = GetOpsRunAsAccount(mg, this.DisplayName);
            IList<ManagementPackSecureReference> runAsProfiles = this.GetOpsRunAsProfiles(profileDisplayName, mg);

            ManagementPackClassCriteria classesQuery = new ManagementPackClassCriteria(string.Format("Name = '{0}'", "Microsoft.SystemCenter.HealthService"));
            IList<ManagementPackClass> healthServiceMonitorClasses =
                mg.EntityTypes.GetClasses(classesQuery);

            if (healthServiceMonitorClasses.Count == 0)
            {
                throw new ManageAccountsException("No health service monitoring class found");
            }

            ManagementPackClass healthServiceClass = healthServiceMonitorClasses[0];

            IObjectReader<MonitoringObject> healthServices = mg.EntityObjects.GetObjectReader<MonitoringObject>(healthServiceClass, ObjectQueryOptions.Default);

            if (runAsAccount.Id != null)
            {
                IList<SecureDataHealthServiceReference> healthServiceReferences = mg.Security.GetSecureDataHealthServiceReferenceBySecureDataId((Guid)runAsAccount.Id);
                foreach (SecureDataHealthServiceReference healthServiceReference in healthServiceReferences)
                {
                    ManagementPackSecureReference secureReference =
                        healthServiceReference.ManagementGroup.Security.GetSecureReference(healthServiceReference.SecureReferenceId);
                    if (secureReference.DisplayName == profileDisplayName)
                    {
                        this.logger("Deleting the account from profile '" + secureReference.DisplayName + "'");
                        mg.Security.DeleteSecureDataHealthServiceReference(healthServiceReference);
                    }
                }
            }

            this.addedProfiles.Remove(profileDisplayName);
            this.logger("Run As Account deleted from Run As Profile");
        }

        /// <summary>
        /// Delete this RunAs account from an existing profile
        /// </summary>
        /// <param name="profileDisplayName">Display name of the profile</param>
        /// <param name="mg">ManagementGroup representing connection to ops manager</param>
        /// <param name="name">The name of the host</param>
        public void DeleteThisAccountFromAnExistingProfile(string profileDisplayName, ManagementGroup mg, string name)
        {
            // Add RunAs Account to RunAs Profile
            this.logger("Delete RunAs Account '" + this.DisplayName + "' to Run As Profile " + profileDisplayName);

            SecureData runAsAccount = GetOpsRunAsAccount(mg, this.DisplayName);
            IList<ManagementPackSecureReference> runAsProfiles = this.GetOpsRunAsProfiles(profileDisplayName, mg);

            IList<MonitoringObject> healthServiceList = this.GetHealthServiceList(mg);

            IEnumerable<MonitoringObject> query = from o in healthServiceList
                                                  where o.FullName.ToString().Contains(name)
                                                  select o;
            IList<SecureDataHealthServiceReference> healthServiceReferences = mg.Security.GetSecureDataHealthServiceReferenceByHealthServiceId(query.First().Id);
            foreach (SecureDataHealthServiceReference healthServiceReference in healthServiceReferences)
            {
                ManagementPackSecureReference secureReference =
                    healthServiceReference.ManagementGroup.Security.GetSecureReference(healthServiceReference.SecureReferenceId);
                if (string.Compare(secureReference.DisplayName, profileDisplayName, true) == 0)
                {
                    this.logger("Deleting the account from profile '" + secureReference.DisplayName + "'");
                    mg.Security.DeleteSecureDataHealthServiceReference(healthServiceReference);
                }
            }

            this.addedProfiles.Remove(profileDisplayName);
            this.logger("Run As Account deleted from Run As Profile");
        }

        /// <summary>
        /// Distributes this account to all management servers
        /// </summary>
        /// <param name="mg">ManagementGroup representing connection to OM</param>
        public void DistributeToPeerManagementServers(ManagementGroup mg)
        {
            SecureData runAsAccount = GetOpsRunAsAccount(mg, this.DisplayName);

            IList<ManagementServer> allManagementServers = mg.Administration.GetAllManagementServers();

            foreach (ManagementServer managementServer in allManagementServers)
            {
                ManagementGroup peerMg = managementServer.ManagementGroup;

                if (peerMg.Id == mg.Id)
                {
                    // Don't work on self 
                    continue;
                }

                if (false == peerMg.IsConnected)
                {
                    this.logger("Not connected to peer management server " + managementServer.PrincipalName);
                    peerMg = ManagementGroup.Connect(peerMg.ConnectionSettings);
                }

                this.logger("Adding Run As account to peer management server " + managementServer.PrincipalName);
                peerMg.Security.InsertSecureData(runAsAccount);
            }
        }

        /// <summary>
        /// Set Distribution As LessSecure
        /// </summary>
        /// <param name="mg">ManagementGroup representing connection to OM</param>
        public void SetDistributionAsLessSecure(ManagementGroup mg)
        {
            SecureData runAsAccount = GetOpsRunAsAccount(mg, this.DisplayName);

            IApprovedHealthServicesForDistribution<PartialMonitoringObject> healthService = new ApprovedHealthServicesForDistribution<PartialMonitoringObject>();
            healthService.Result = ApprovedHealthServicesResults.All;
            mg.Security.SetApprovedHealthServicesForDistribution<PartialMonitoringObject>(runAsAccount, healthService);
        }

        /// <summary>
        /// Get HealthService List
        /// </summary>
        /// <param name="mg">ManagementGroup representing connection to OM</param>
        /// <returns> Return a list object of monitoring object</returns>
        public IList<MonitoringObject> GetHealthServiceList(ManagementGroup mg)
        {
            ManagementPackClassCriteria classesQuery = new ManagementPackClassCriteria(string.Format("Name = '{0}'", "Microsoft.SystemCenter.HealthService"));
            IList<ManagementPackClass> healthServiceMonitorClasses =
                mg.EntityTypes.GetClasses(classesQuery);

            if (healthServiceMonitorClasses.Count == 0)
            {
                throw new ManageAccountsException("No health service monitoring class found");
            }

            ManagementPackClass healthServiceClass = healthServiceMonitorClasses[0];

            IObjectReader<MonitoringObject> healthServices = mg.EntityObjects.GetObjectReader<MonitoringObject>(healthServiceClass, ObjectQueryOptions.Default);

            if (healthServices.Count == 0)
            {
                throw new ManageAccountsException("No health services found");
            }

            return healthServices.GetRange(0, healthServices.Count);
        }

        /// <summary>
        /// Set the log delegate to allow logging using a mechanism provided by the user
        /// </summary>
        /// <param name="logDlg">Logging delegate method</param>
        public void SetLogDelegate(LogDelegate logDlg)
        {
            this.logger = logDlg;
        }
    }
}