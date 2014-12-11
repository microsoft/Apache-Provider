// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SCXRunAsAccountFactory.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Security;
    using System.Security.Permissions;

    using Microsoft.EnterpriseManagement;
    using Microsoft.EnterpriseManagement.Common;
    using Microsoft.EnterpriseManagement.Security;

    public class SCXRunAsAccountFactory : ISCXRunAsAccountFactory, IDisposable
    {
        #region Constants and Fields

        /// <summary>
        /// Handle for tracing.
        /// </summary>
        private static readonly TraceSource Trace =
            new TraceSource("Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction");

        /// <summary>
        /// Management group connection.
        /// </summary>
        private IManagementGroupConnection managementGroupConnection;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the SCXRunAsAccountFacotry class.
        /// </summary>
        /// <param name="connection">
        /// the management group connection object.
        /// </param>
        public SCXRunAsAccountFactory(IManagementGroupConnection connection)
        {
            this.managementGroupConnection = connection;
        }

        ~SCXRunAsAccountFactory()
        {
            this.Dispose(false);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Enumerate all the SCX RunAs account using the default Management server name
        /// </summary>
        /// <param name="enterpriseManagementGroup">
        /// The management group.
        /// </param>
        /// <returns>
        /// The RunAs account IDs.
        /// </returns>
        public static IEnumerable<Guid> EnumerateSCXRunAsAccounts(EnterpriseManagementGroup enterpriseManagementGroup)
        {
            IManagedObjectFactory objectFactory;

            using (var managementGroupConnection = new ManagementGroupConnection(enterpriseManagementGroup))
            {
                objectFactory = managementGroupConnection.CreateManagedObjectFactory("Microsoft.Unix.CredentialRef");
            }

            IEnumerable<IManagedObject> mgrObjects = objectFactory.GetAllInstances();

            var ids = new List<Guid>();
            foreach (IManagedObject credential in mgrObjects)
            {
                var credentialObj = new ScxCredentialRef(credential);
                string id = credentialObj.Key;
                ids.Add(new Guid(id));
            }

            return ids;
        }

        #endregion

        #region Implemented Interfaces

        #region IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region ISCXRunAsAccountFactory

        /// <summary>
        /// Creates SCX RunAs account, insert it into managementGroupConnection
        /// and save it to the database.
        /// </summary>
        /// <param name="displayName">
        /// Display name of the run as account.
        /// </param>
        /// <param name="description">
        /// Description of the run as account.
        /// </param>
        /// <param name="userName">
        /// The user name of the RunAs account in XML format. The user name should
        /// be in the same format as the one generated from GetXMLUserName function
        /// of CredentialSet.
        /// </param>
        /// <param name="password">
        /// The XML formatted password encrypted in SecureString. The password should be
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
        public void CreateRunAsAccount(
            string displayName, 
            string description, 
            string userName, 
            SecureString password, 
            ScxCredentialRef.RunAsAccountType runAsType, 
            bool isSshKey, 
            bool secureDistribution)
        {
            var account = new BasicCredentialSecureData
                {
                    UserName = userName,
                    Name = displayName,
                    Data = password,
                    Description = description 
                };

            this.managementGroupConnection.InsertRunAsAccount(account);

            var distributionHealthServices = new ApprovedHealthServicesForDistribution<EnterpriseManagementObject>
                {
                    Result = secureDistribution ? ApprovedHealthServicesResults.None : ApprovedHealthServicesResults.All 
                };

            this.managementGroupConnection.SetRunAsAccountDistribution(account, distributionHealthServices);

            this.CreateRunAsAcccountRef(account, runAsType, isSshKey);
        }

        /// <summary>
        /// Delete an SCX RunAs account
        /// </summary>
        /// <param name="accountRef">
        /// The RunAs account ref.
        /// </param>
        public void DeleteScxRunAsAccount(ScxCredentialRef accountRef)
        {
            if (null == accountRef || null == accountRef.RunAsAcount || null == accountRef.ManagedObject)
            {
                throw new ArgumentException(Strings.SCXRunAsAccountFactory_DeleteScxRunAsAccount_Account_cannot_be_empty);
            }

            this.managementGroupConnection.DeleteRunAsAccount(accountRef.RunAsAcount);

            IIncrementalDiscoveryData persistenceSession = this.managementGroupConnection.CreateDiscoveryData();
            persistenceSession.Remove(accountRef.ManagedObject);
            persistenceSession.Commit();
        }

        /// <summary>
        /// Get all SCX RunAs accounts.
        /// </summary>
        /// <returns>
        /// The RunAs accounts.
        /// </returns>
        public IEnumerable<ScxCredentialRef> GetAllScxRunAsAccount()
        {
            Trace.TraceEvent(TraceEventType.Information, 1, "Get all SCX Run As account.");

            IManagedObjectFactory objectFactory =
                this.managementGroupConnection.CreateManagedObjectFactory("Microsoft.Unix.CredentialRef");
            IEnumerable<IManagedObject> mgrObjects = objectFactory.GetAllInstances();

            Trace.TraceEvent(
                TraceEventType.Information, 2, "Got " + mgrObjects.Count() + " SCX Run As account in total.");

            var accounts = new List<ScxCredentialRef>();
            foreach (IManagedObject credential in mgrObjects)
            {
                var account = new ScxCredentialRef(credential);
                SecureData secureData = this.managementGroupConnection.GetScxRunAsAccount(new Guid(account.Key));

                if (null != secureData)
                {
                    account.RunAsAcount = secureData;
                    accounts.Add(account);
                }
            }

            return accounts;
        }

        /// <summary>
        /// Gets a RunAs account from its id
        /// </summary>
        /// <param name="id">
        /// The RunAs account id.
        /// </param>
        /// <returns>
        /// The RunAs account reference.
        /// </returns>
        public ScxCredentialRef GetScxRunAsAccount(Guid id)
        {
            IManagedObjectFactory objectFactory =
                this.managementGroupConnection.CreateManagedObjectFactory("Microsoft.Unix.CredentialRef");
            string criteria = String.Format("Key = '{0}'", id);
            IManagedObject mgrObj = objectFactory.GetManagedObjectWithCriteria(criteria);

            if (null == mgrObj)
            {
                throw new ArgumentException(
                    Strings.SCXRunAsAccountFactory_GetScxRunAsAccount + id +
                    Strings.SCXRunAsAccountFactory_GetScxRunAsAccount__cannot_be_found);
            }

            var retVal = new ScxCredentialRef(mgrObj)
                {
                    RunAsAcount = this.managementGroupConnection.GetScxRunAsAccount(id) 
                };

            return retVal;
        }

        /// <summary>
        /// Update an SCX RunAs account.
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
        /// The secure distribution of the RunAs account.
        /// </param>
        public void UpdateScxRunAsAccount(
            ScxCredentialRef accountRef, 
            string displayName, 
            string description, 
            string userName, 
            SecureString password, 
            bool isSshKey, 
            ScxRunAsAccountSecureDistribution secureDistribution)
        {
            if (null == accountRef || null == accountRef.RunAsAcount)
            {
                throw new ArgumentException(Strings.SCXRunAsAccountFactory_DeleteScxRunAsAccount_Account_cannot_be_empty);
            }

            if (userName == null || displayName == null || description == null)
            {
                throw new ArgumentException(Strings.SCXRunAsAccountFactory_UpdateScxRunAsAccount_Null_argument_cannot_be_used_for_updating_RunAs_account);
            }

            var secureData = accountRef.RunAsAcount as BasicCredentialSecureData;
            if (secureData == null)
            {
                throw new ArgumentException(Strings.SCXRunAsAccountFactory_UpdateScxRunAsAccount_Account_is_not_the_type_of_basic_auth);
            }

            secureData.UserName = userName;
            secureData.Description = description;
            secureData.Name = displayName;

            if (password != null)
            {
                secureData.Data = password;
            }

            secureData.Update();

            var servicesForDistribution = new ApprovedHealthServicesForDistribution<EnterpriseManagementObject>();

            if (secureDistribution.SecureDistribution == false)
            {
                servicesForDistribution.Result = ApprovedHealthServicesResults.All;
            }
            else
            {
                if (secureDistribution.HealthServices == null || secureDistribution.HealthServices.Count == 0)
                {
                    servicesForDistribution.Result = ApprovedHealthServicesResults.None;
                }
                else
                {
                    servicesForDistribution.Result = ApprovedHealthServicesResults.Specified;
                    servicesForDistribution.HealthServices = new List<EnterpriseManagementObject>();

                    foreach (var id in secureDistribution.HealthServices)
                    {
                        var healthObj = this.managementGroupConnection.GetEntityObjectById(id.Key);
                        servicesForDistribution.HealthServices.Add(healthObj.OpsMgrObject);
                    }
                }
            }

            this.managementGroupConnection.SetRunAsAccountDistribution(secureData, servicesForDistribution);

            accountRef.IsSSHKey = isSshKey;

            IIncrementalDiscoveryData persistenceSession = this.managementGroupConnection.CreateDiscoveryData();
            persistenceSession.Add(accountRef.ManagedObject);
            persistenceSession.Commit();
        }

        #endregion

        #endregion

        #region Methods

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.managementGroupConnection = null;
            }
        }

        /// <summary>
        /// Create a SCX RunAs account Ref in database
        /// </summary>
        /// <param name="unixAccount">
        /// The RunAs account.
        /// </param>
        /// <param name="type">
        /// The RunAs account type.
        /// </param>
        /// <param name="isSshKey">
        /// Indicate if this RunAs account uses SSH key.
        /// </param>
        private void CreateRunAsAcccountRef(SecureData unixAccount, ScxCredentialRef.RunAsAccountType type, bool isSshKey)
        {
            IManagedObjectFactory objectFactory =
                this.managementGroupConnection.CreateManagedObjectFactory("Microsoft.Unix.CredentialRef");
            IManagedObject credentialManagedObject = objectFactory.CreateNewManagedObject();

            var credentialRef = new ScxCredentialRef(credentialManagedObject)
                {
                    Key = unixAccount.Id.ToString(),
                    Type = type,
                    IsSSHKey = isSshKey 
                };

            IIncrementalDiscoveryData persistenceSession = this.managementGroupConnection.CreateDiscoveryData();

            persistenceSession.Add(credentialRef.ManagedObject);

            try
            {
                persistenceSession.Commit();
            }
            catch (IncrementalDiscoveryDataWrapper.DataInsertionCollisionException dataInsertionCollisionException)
            {
                throw new CredentialRefInsertionCollisionException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.
                            SCXRunAsAccountFactory_CreateRunAsAcccountRef_The_credential__0__is_already_managed_by_Operations_Manager,
                        credentialRef.Key),
                    credentialRef.Key,
                    dataInsertionCollisionException);
            }
        }

        #endregion

        /// <summary>
        /// Exception thrown when inserted CredentialRef collides with existing data.
        /// </summary>
        [Serializable]
        public class CredentialRefInsertionCollisionException : Exception
        {
            public string UnixAccountId { get; private set; }

            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
                info.AddValue("UnixAccountId", UnixAccountId);
            }

            public CredentialRefInsertionCollisionException(string message, string unixAccountId, Exception innerException)
                : base(innerException.Message, innerException)
            {
                UnixAccountId = unixAccountId;
            }

            protected CredentialRefInsertionCollisionException(SerializationInfo info, StreamingContext context)
                : base(info, context)
            {
                if (info == null)
                {
                    throw new ArgumentNullException("info");
                }

                UnixAccountId = info.GetString("UnixAccountId");
            }
        }
    }
}
