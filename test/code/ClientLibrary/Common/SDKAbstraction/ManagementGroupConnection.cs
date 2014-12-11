//-----------------------------------------------------------------------
// <copyright file="ManagementGroupConnection.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using Microsoft.EnterpriseManagement;
    using Microsoft.EnterpriseManagement.Common;
    using Microsoft.EnterpriseManagement.Configuration;
    using Microsoft.EnterpriseManagement.ConnectorFramework;
    using Microsoft.EnterpriseManagement.Monitoring;
    using Microsoft.EnterpriseManagement.Security;
    using Microsoft.Win32;
    using Runtime = Microsoft.EnterpriseManagement.Runtime;

    /// <summary>
    /// Simple wrapper for an EnterpriseManagementGroup.
    /// </summary>
    public class ManagementGroupConnection : IManagementGroupConnection, IDisposable
    {
        /// <summary>
        /// Management group that this instance wraps.
        /// </summary>
        private ManagementGroup managementGroup;

        /// <summary>
        ///     THe registry subkey and value name in HKCU
        /// </summary>
        private const string regPathUser = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Microsoft Operations Manager\3.0\User Settings";
        private const string regValueNameUser = @"SDKServiceMachine";

        /// <summary>
        ///     The registry subkey and value name in HKLM
        /// </summary>
        private const string regPathHost = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Microsoft Operations Manager\3.0\Machine Settings";
        private const string regValueNameHost = @"DefaultSDKServiceMachine";

        /// <summary>
        /// Handle for tracing.
        /// </summary>
        private static readonly TraceSource Trace =
            new TraceSource("Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction");

        /// <summary>
        ///     Gets the value of the default management server.
        /// </summary>
        public static string DefaultManagementServerName
        {
            get
            {
                string serverName = null;
                var userValue = (string)Registry.GetValue(regPathUser, regValueNameUser, string.Empty);
                var hostValue = (string)Registry.GetValue(regPathHost, regValueNameHost, string.Empty);

                if (null != userValue)
                {
                    serverName = userValue;
                }

                if (string.IsNullOrEmpty(serverName) && (null != hostValue))
                {
                    serverName = hostValue;
                }

                if (string.IsNullOrEmpty(serverName))
                {
                    serverName = @"localhost";
                }

                return serverName;
            }
        }

        /// <summary>
        /// Gets a list of all the management servers in the management group.
        /// </summary>
        /// <returns>List of names of the management servers in the group.</returns>
        public ICollection<string> GetAllManagementServerNames()
        {
            var mscoll = this.managementGroup.Administration.GetAllManagementServers();
            return mscoll.Select(s => s.Name).ToList();
        }

        /// <summary>
        /// Gets a list of all the management resource pool in the management group.
        /// </summary>
        /// <returns>List of names of the management resource pool in the group.</returns>
        public ICollection<string> GetAllResourcePoolNames()
        {
            var rpcoll = this.managementGroup.Administration.GetManagementServicePools();
            return rpcoll.Select(s => s.DisplayName).ToList();
        }

        /// <summary>
        /// Return a management action point for a specific named management server (useful for gateway ManagementServers).
        /// </summary>
        /// <param name="managementActionPoint">Name of the management server to manage remote UNIX/Linux hosts</param>
        /// <returns>IManagedObject reflecting the management action point that should run remote tasks</returns>
        public IManagedObject GetManagementActionPoint(string managementActionPoint)
        {
            return this.GetManagementServicePool(managementActionPoint);
        }

        /// <summary>
        /// Initializes a new instance of the ManagementGroupConnection class.
        /// </summary>
        /// <param name="managementGroup">the management groupConnection object.</param>
        public ManagementGroupConnection(EnterpriseManagementGroup managementGroup)
        {
            this.managementGroup          = managementGroup as ManagementGroup;
            this.TaskConfigurationFactory = new TaskConfigurationFactory();
            this.TaskResultFactory        = new TaskInvocationResultFactory();
        }

        #region IManagementGroupConnection Members

        /// <summary>
        /// Gets the task configuration management from the wrapped management group.
        /// </summary>
        public ITaskConfigurationManagement TaskConfigManagement
        {
            get
            {
                return this.managementGroup.TaskConfiguration;
            }
        }

        /// <summary>
        /// Gets the task configuration factory.
        /// </summary>
        public ITaskConfigurationFactory TaskConfigurationFactory { get; private set; }

        /// <summary>
        /// Gets the task invocation result factory.
        /// </summary>
        public ITaskInvocationResultFactory TaskResultFactory { get; private set; }

        /// <summary>
        /// Gets the task runtime management from the wrapped management group.
        /// </summary>
        public Runtime.ITaskRuntimeManagement TaskRuntime
        {
            get
            {
                return this.managementGroup.TaskRuntime;
            }
        }

        /// <summary>
        /// Gets the Entity type management from the wrapped management group.
        /// </summary>
        public IEntityTypeManagement EntityTypes
        {
            get
            {
                return this.managementGroup.EntityTypes;
            }
        }

        /// <summary>
        /// Gets the Entity objects management from the wrapped management group.
        /// </summary>
        public IEntityObjectsManagement EntityObjects
        {
            get
            {
                return this.managementGroup.EntityObjects;
            }
        }

        /// <summary>
        /// Creates Enterprise Management Object Factories.
        /// </summary>
        /// <param name="managementPackClassName">Name of type of Management Object this factory will create.</param>
        /// <returns>A new Enterprise Management Object factory.</returns>
        public virtual IManagedObjectFactory CreateManagedObjectFactory(string managementPackClassName)
        {
            return new ManagedObjectFactory(this.managementGroup, this.EntityTypes, this.EntityObjects, managementPackClassName);
        }

        /// <summary>
        /// Creates a transaction object that can be used for discovery data.
        /// </summary>
        /// <returns>A new IncrementalDiscoveryData instance.</returns>
        public IIncrementalDiscoveryData CreateDiscoveryData()
        {
            return new IncrementalDiscoveryDataWrapper(new IncrementalDiscoveryData(), this.managementGroup);
        }

        /// <summary>
        /// Creates a new relationship object factory.
        /// </summary>
        /// <param name="relationshipTypeName">Name of type of relationships this factory will create.</param>
        /// <returns>New relationship object factory.</returns>
        public virtual IRelationshipObjectFactory CreateRelationshipObjectFactory(string relationshipTypeName)
        {
            return new RelationshipObjectFactory(this.managementGroup, this.EntityTypes, this.EntityObjects, relationshipTypeName);
        }

        /// <summary>
        /// Creates a new relationship object factory.
        /// </summary>
        /// <param name="relationshipTypeNames">Names of type of relationships this factory will create.</param>
        /// <returns>New relationship object factory.</returns>
        public virtual IRelationshipObjectFactory CreateRelationshipObjectFactory(IList<string> relationshipTypeNames)
        {
            return new RelationshipObjectFactory(this.managementGroup, this.EntityTypes, this.EntityObjects, relationshipTypeNames);
        }

        /// <summary>
        /// Returns a management service from a given management server name.
        /// </summary>
        /// <param name="resourcepool">The management server name.</param>
        /// <returns>The management service object.</returns>
        public IManagedObject GetManagementServicePool(string resourcepool)
        {
            IManagedObjectFactory factory = this.CreateManagedObjectFactory("Microsoft.SystemCenter.ManagementServicePool");

            var pool = factory.GetExistingManagedObject(resourcepool, false);

            Debug.Assert(pool.Count() == 1, "Only one management service pool is expected.");

            return pool.First();
        }

        /// <summary>
        /// Returns a health service from a given management server name.
        /// </summary>
        /// <param name="managementServerName">The management server name.</param>
        /// <returns>The HealthService object.</returns>
        public IManagedObject GetHealthService(string managementServerName)
        {
            IManagedObjectFactory factory = this.CreateManagedObjectFactory("Microsoft.SystemCenter.HealthService");

            var hs = factory.GetExistingManagedObject(managementServerName, false);

            Debug.Assert(hs.Count() == 1, "Only one management service pool is expected.");

            return hs.First();
        }

        /// <summary>
        /// Returns a management service from a given management server name.
        /// </summary>
        /// <param name="managementServer">The management server name.</param>
        /// <returns>The management service object.</returns>
        public IManagedObject GetManagementServiceFromManagementServer(string managementServer)
        {
            IManagedObjectFactory factory = this.CreateManagedObjectFactory("Microsoft.SystemCenter.ManagementService");

            var service = factory.GetExistingManagedObject(managementServer, false);

            Debug.Assert(service.Count() == 1, "Only one management service pool is expected.");

            return service.First();
        }

        /// <summary>
        /// Get an entity object by its ID
        /// </summary>
        /// <param name="id">The id of entity object.</param>
        /// <returns>The created manage object.</returns>
        public IManagedObject GetEntityObjectById(Guid id)
        {
            var emo = this.managementGroup.EntityObjects.GetObject<MonitoringObject>(id, ObjectQueryOptions.Default);

            if (null == emo)
            {
                throw new ManagedObjectNotFoundException(id.ToString());
            }

            return new ManagedObject(emo);
        }

        /// <summary>
        /// Insert a RunAs account into the MangementGroup associated with this
        /// connection.
        /// </summary>
        /// <param name="secureData">The RunAs account to insert.</param>
        public void InsertRunAsAccount(SecureData secureData)
        {
            this.managementGroup.Security.InsertSecureData(secureData);
        }

        /// <summary>
        /// Get all the profile names a run as account is associated with
        /// </summary>
        /// <param name="id">The RunAs account Id.</param>
        /// <returns>Return the list of profile names.</returns>
        public IList<string> GetRunAsAccountAssociation(Guid id)
        {
            var secureData = GetScxRunAsAccount(id);

            StringBuilder sb = new StringBuilder();
            foreach (byte b in secureData.SecureStorageId)
            {
                sb.Append(b.ToString("X2"));
            }

            List<string> retVal = new List<string>();
            ManagementPackOverrideCriteria moc = new ManagementPackOverrideCriteria("Value=\'" + sb + "\'");
            IList<ManagementPackOverride> overrides = managementGroup.Overrides.GetOverrides(moc);
            foreach (ManagementPackSecureReferenceOverride msro in overrides)
            {
                ManagementPackSecureReference mpsr = managementGroup.Security.GetSecureReference(msro.SecureReference.Id);
                string profileText = mpsr.DisplayName ?? mpsr.Name;
                retVal.Add(profileText);
            }

            return retVal;
        }

        /// <summary>
        /// Sets the distribution policy for a RunAsAccount within the ManagementGroup.
        /// </summary>
        /// <param name="secureData">The RunAs account.</param>
        /// <param name="healthServices">The approved health service distribution.</param>
        public void SetRunAsAccountDistribution(SecureData secureData, IApprovedHealthServicesForDistribution<EnterpriseManagementObject> healthServices)
        {
            this.managementGroup.Security.SetApprovedHealthServicesForDistribution(secureData, healthServices);
        }

        /// <summary>
        /// Delete a RunAs account from the MangementGroup associated with this
        /// connection.
        /// </summary>
        /// <param name="secureData">The RunAs account to delete.</param>
        public void DeleteRunAsAccount(SecureData secureData)
        {
            this.managementGroup.Security.DeleteSecureData(secureData);
        }

        /// <summary>
        /// Get a RunAs account from a given ID
        /// </summary>
        /// <param name="id">The id of RunAs account.</param>
        /// <returns>The run as account.</returns>
        public SecureData GetScxRunAsAccount(Guid id)
        {
            SecureData runasAccount = null;
            try
            {
                runasAccount = this.managementGroup.Security.GetSecureData(id);
            }
            catch (ObjectNotFoundException e)
            {
                Trace.TraceEvent(
                    TraceEventType.Warning, 2, "Unable to retrieve secure data for account: " + id + " Error: " + e.Message);
            }

            return runasAccount;
        }

        /// <summary>
        /// Get all the approved HealthService objects for the RunAs account
        /// with the given ID.
        /// </summary>
        /// <param name="id">The id of RunAs account.</param>
        /// <returns>The secure distribution.</returns>
        public ScxRunAsAccountSecureDistribution GetSecureDistributions(Guid id)
        {
            var hs = new ScxRunAsAccountSecureDistribution();

            var secureData = GetScxRunAsAccount(id);

            var distribution = 
                this.managementGroup.Security.GetApprovedHealthServicesForDistribution<EnterpriseManagementObject>(secureData);

            hs.SecureDistribution = distribution.Result == ApprovedHealthServicesResults.Specified || distribution.Result == ApprovedHealthServicesResults.None;

            foreach (var emo in distribution.HealthServices)
            {
                hs.HealthServices.Add(emo.Id, emo.DisplayName);
            }

            return hs;
        }

        #endregion

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ManagementGroupConnection()
        {
            this.Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Do not dispose this.managementGroup. Doing so will cause unexpected behavior.
                this.managementGroup = null;
            }
        }
    }
}
