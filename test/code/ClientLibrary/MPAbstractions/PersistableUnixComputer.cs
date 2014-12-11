// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersistableUnixComputer.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
//   Defines the PersistableUnixComputer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;

    internal class PersistableUnixComputer : UnixComputer, IPersistableUnixComputer
    {
        /// <summary>
        /// Relationship defining what health service manages this computer.
        /// </summary>
        private readonly IRelationshipObject managedByRelationship;

        /// <summary>
        /// The ManagementGroupConnection that this computer belongs to.
        /// </summary>
        private readonly IManagementGroupConnection managementGroupConnection;

        public PersistableUnixComputer(IManagedObject managedObject, IRelationshipObject managedByRelationship, IManagementGroupConnection managementGroupConnection)
            : base(managedObject)
        {
            this.managementGroupConnection = managementGroupConnection;
            this.managedByRelationship = managedByRelationship;
        }

        /// <summary>
        /// Save this computer instance to the database.
        /// </summary>
        public void Persist()
        {
            IIncrementalDiscoveryData persistenceSession = this.managementGroupConnection.CreateDiscoveryData();

            persistenceSession.Add(this.ManagedObject);
            persistenceSession.Add(this.managedByRelationship);

            try
            {
                persistenceSession.Commit();
            }
            catch (IncrementalDiscoveryDataWrapper.DataInsertionCollisionException ex)
            {
                string message = String.Format(Strings.PersistableUnixComputer_Persist_The_computer_named__0__is_already_managed_by_OpsMgr, this.Name);
                throw new ComputerAlreadyManagedException(message, ex);
            }
        }

        /// <summary>
        /// Get the managed object.
        /// </summary>
        /// <returns>
        /// Return the associated managed object.
        /// </returns>
        public IManagedObject GetManagedObject()
        {
            return this.ManagedObject;
        }
    }
}