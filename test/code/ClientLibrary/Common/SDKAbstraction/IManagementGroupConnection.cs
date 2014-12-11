// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IManagementGroupConnection.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using System;
    using System.Collections.Generic;

    using Microsoft.EnterpriseManagement;
    using Microsoft.EnterpriseManagement.Common;
    using Microsoft.EnterpriseManagement.Security;

    using ITaskRuntimeManagement = Microsoft.EnterpriseManagement.Runtime.ITaskRuntimeManagement;

    /// <summary>
    /// Represents a management group.
    /// </summary>
    public interface IManagementGroupConnection
    {
        #region Properties

        /// <summary>
        /// Gets the Entity Objects Management which can be used to query for Management Pack data type instances.
        /// </summary>
        IEntityObjectsManagement EntityObjects { get; }

        /// <summary>
        /// Gets the Entity type management which can be used to query for Management Pack Data types.
        /// </summary>
        IEntityTypeManagement EntityTypes { get; }

        /// <summary>
        /// Gets the Task Configuration Management which can be queried for task data.
        /// </summary>
        ITaskConfigurationManagement TaskConfigManagement { get; }

        /// <summary>
        /// Gets the Task configuration factory which can configure the parameters of tasks.
        /// </summary>
        ITaskConfigurationFactory TaskConfigurationFactory { get; }

        /// <summary>
        /// Gets the task invocation result factory.
        /// </summary>
        ITaskInvocationResultFactory TaskResultFactory { get; }

        /// <summary>
        /// Gets the Task Runtime Management which is used to run tasks.
        /// </summary>
        ITaskRuntimeManagement TaskRuntime { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a data type to use as a transaction.
        /// </summary>
        /// <returns>
        /// A new Incremental discovery data.
        /// </returns>
        IIncrementalDiscoveryData CreateDiscoveryData();

        /// <summary>
        /// Creating Enterprise Management Object factories.
        /// </summary>
        /// <param name="managementPackClassName">
        /// Name of type of Management Object this factory will create.
        /// </param>
        /// <returns>
        /// A new Enterprise Management Object factory.
        /// </returns>
        IManagedObjectFactory CreateManagedObjectFactory(string managementPackClassName);

        /// <summary>
        /// Creates a new relationship object factory.
        /// </summary>
        /// <param name="relationshipTypeName">
        /// Name of relationship type that this factory will create.
        /// </param>
        /// <returns>
        /// A new relationship object factory.
        /// </returns>
        IRelationshipObjectFactory CreateRelationshipObjectFactory(string relationshipTypeName);

        /// <summary>
        /// Creates a new relationship object factory.
        /// </summary>
        /// <param name="relationshipTypeNames">
        /// Names of relationship type that this factory will create.
        /// </param>
        /// <returns>
        /// A new relationship object factory.
        /// </returns>
        IRelationshipObjectFactory CreateRelationshipObjectFactory(IList<string> relationshipTypeNames);

        /// <summary>
        /// Delete a RunAs account into management group
        /// </summary>
        /// <param name="secureData">
        /// The RunAs account.
        /// </param>
        void DeleteRunAsAccount(SecureData secureData);

        /// <summary>
        /// Gets a list of all the management servers in the management group.
        /// </summary>
        /// <returns>
        /// List of names of the management servers in the group.
        /// </returns>
        ICollection<string> GetAllManagementServerNames();

        /// <summary>
        /// Gets a list of all the management resource pool in the management group.
        /// </summary>
        /// <returns>
        /// List of names of the management resource pool in the group.
        /// </returns>
        ICollection<string> GetAllResourcePoolNames();

        /// <summary>
        /// Get an entity object by its ID
        /// </summary>
        /// <param name="id">
        /// The id of entity object.
        /// </param>
        /// <returns>
        /// The created manage object.
        /// </returns>
        IManagedObject GetEntityObjectById(Guid id);

        /// <summary>
        /// Return a health service for a specific named management server (useful for gateway ManagementServers).
        /// </summary>
        /// <param name="managementServerName">
        /// Name of the management server to manage remote UNIX/Linux hosts
        /// </param>
        /// <returns>
        /// IManagedObject reflecting the health service that should run remote tasks
        /// </returns>
        IManagedObject GetHealthService(string managementServerName);

        /// <summary>
        /// Return a resource pool for a specific named management server (useful for gateway ManagementServers).
        /// </summary>
        /// <param name="managementActionPoint">
        /// Name of the management server to manage remote UNIX/Linux hosts
        /// </param>
        /// <returns>
        /// IManagedObject reflecting the resource pool that should run remote tasks
        /// </returns>
        IManagedObject GetManagementActionPoint(string managementActionPoint);

        /// <summary>
        /// Returns a management service from a given management server name.
        /// </summary>
        /// <param name="managementServer">
        /// The management server name.
        /// </param>
        /// <returns>
        /// The management service object.
        /// </returns>
        IManagedObject GetManagementServiceFromManagementServer(string managementServer);

        /// <summary>
        /// Get the resource pool from its name
        /// </summary>
        /// <param name="resourcePoolName">
        /// The resource pool name.
        /// </param>
        /// <returns>
        /// The resource pool object.
        /// </returns>
        IManagedObject GetManagementServicePool(string resourcePoolName);

        /// <summary>
        /// Get all the profile names a run as account is associated with
        /// </summary>
        /// <param name="id">
        /// The RunAs account Id.
        /// </param>
        /// <returns>
        /// Return the list of profile names.
        /// </returns>
        IList<string> GetRunAsAccountAssociation(Guid id);

        /// <summary>
        /// Get a RunAs account from its id
        /// </summary>
        /// <param name="id">
        /// The RunAs account id.
        /// </param>
        /// <returns>
        /// The RunAs account.
        /// </returns>
        SecureData GetScxRunAsAccount(Guid id);

        /// <summary>
        /// Get all the approved HealthService objects for the RunAs account
        /// with the given ID.
        /// </summary>
        /// <param name="id">
        /// The id of RunAs account.
        /// </param>
        /// <returns>
        /// The secure distribution.
        /// </returns>
        ScxRunAsAccountSecureDistribution GetSecureDistributions(Guid id);

        /// <summary>
        /// Insert a RunAs account into management group
        /// </summary>
        /// <param name="secureData">
        /// The RunAs account.
        /// </param>
        void InsertRunAsAccount(SecureData secureData);

        /// <summary>
        /// Sets the distribution policy for a RunAsAccount within the ManagementGroup.
        /// </summary>
        /// <param name="secureData">
        /// The RunAs account.
        /// </param>
        /// <param name="healthServices">
        /// The approved health service distribution.
        /// </param>
        void SetRunAsAccountDistribution(
            SecureData secureData, IApprovedHealthServicesForDistribution<EnterpriseManagementObject> healthServices);

        #endregion
    }
}