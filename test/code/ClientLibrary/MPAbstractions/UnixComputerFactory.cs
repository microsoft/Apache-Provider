//-----------------------------------------------------------------------
// <copyright file="UnixComputerFactory.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;
    using System.Collections.Generic;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;

    /// <summary>
    /// Factory for creating AddNewHostTask instances.
    /// </summary>
    public class UnixComputerFactory : IUnixComputerFactory
    {
        /// <summary>
        /// Handle to OpsMgr SDK.
        /// </summary>
        private readonly IManagementGroupConnection managementGroupConnection;

        private readonly IRelationshipObjectFactory relationshipFactory;

        /// <summary>
        /// Initializes a new instance of the UnixComputerFactory class.
        /// </summary>
        /// <param name="managementGroupConnection">Handle to OpsMgr SDK.</param>
        public UnixComputerFactory(IManagementGroupConnection managementGroupConnection)
        {
            this.managementGroupConnection = managementGroupConnection;
            IList<string> relationshipNames = new List<string>();
            relationshipNames.Add("Microsoft.SystemCenter.ManagementActionPointShouldManageEntity");
            relationshipFactory = this.managementGroupConnection.CreateRelationshipObjectFactory(relationshipNames);
        }

        /// <summary>
        /// Creates a new UnixComputer instance.
        /// </summary>
        /// <param name="computerClassName">the name of the computer class object from the MP</param>
        /// <param name="targetManagementObject">Health service that should manage this computer.</param>
        /// <returns>A new UnixComputer instance.</returns>
        public IPersistableUnixComputer CreateUnixComputer(string computerClassName, IManagedObject targetManagementObject)
        {
            var objectFactory = this.managementGroupConnection.CreateManagedObjectFactory(computerClassName);
            var computerManagedObject = objectFactory.CreateNewManagedObject();

            IRelationshipObject managedBy = this.relationshipFactory.CreateRelationshipObject(targetManagementObject, computerManagedObject);

            return new PersistableUnixComputer(computerManagedObject, managedBy, this.managementGroupConnection);
        }

        /// <summary>
        /// Creates a new UnixComputer instance that is aready in database.
        /// </summary>
        /// <param name="managedObject">the managed object.</param>
        /// <returns>A new PersistedUnixComputer instance.</returns>
        public IPersistedUnixComputer CreatePersistedUnixComputer(IManagedObject managedObject)
        {
            return new PersistedUnixComputer(managedObject, this.relationshipFactory, this.managementGroupConnection);
        }

        /// <summary>
        /// Creates a new UnixComputer instance from data already present in OpsMgr database.
        /// </summary>
        /// <param name="computerHostName">The FQDN of the computer</param>
        /// <param name="isRegularExpression">Indicate if the input is a regular expression.</param>
        /// <returns>A new UnixComputer instance.</returns>
        public IEnumerable<IPersistedUnixComputer> GetExistingUnixComputer(string computerHostName, bool isRegularExpression)
        {
            var objectFactory = this.managementGroupConnection.CreateManagedObjectFactory("Microsoft.Unix.Computer");

            var retval = new List<IPersistedUnixComputer>();

            foreach (var computer in objectFactory.GetExistingManagedObject(computerHostName, isRegularExpression))
            {
                retval.Add(new PersistedUnixComputer(computer, this.relationshipFactory, this.managementGroupConnection));
            }

            return retval;
        }

        /// <summary>
        /// Creates a list of new UnixComputer instances from data already presented in OpsMgr database.
        /// </summary>
        /// <returns>A list of UnixComputer instances.</returns>
        public IEnumerable<IPersistedUnixComputer> GetAllExistingUnixComputers()
        {
            var retval = new List<IPersistedUnixComputer>();
            var objectFactory = this.managementGroupConnection.CreateManagedObjectFactory("Microsoft.Unix.Computer");
            var computerManagedObject = objectFactory.GetAllInstances();
            foreach (var managedObj in computerManagedObject)
            {
                retval.Add(new PersistedUnixComputer(managedObj, this.relationshipFactory, this.managementGroupConnection));
            }

            return retval;
        }

        /// <summary>
        /// Creates a list of new UnixComputer instances from data already presented in OpsMgr database.
        /// </summary>
        /// <param name="id">The resource pool id</param>
        /// <returns>A list of UnixComputer instances.</returns>
        public IEnumerable<IPersistedUnixComputer> GetExistingUnixComputerFromResourcePool(Guid id)
        {
            var retval = new List<IPersistedUnixComputer>();
            var relationshipObjects = this.relationshipFactory.GetAllRelatedObjects(id);

            foreach (var relationObj in relationshipObjects)
            {
                if (relationObj.Target.IsInstanceOfMPClass(this.managementGroupConnection, "Microsoft.Unix.Computer")) 
                {
                    retval.Add(new PersistedUnixComputer(relationObj.Target, this.relationshipFactory, this.managementGroupConnection));
                }
            }
            
            return retval;
        }

        /// <summary>
        /// Creates a list of new UnixComputer instances managed by mangement server from data already presented in OpsMgr database.
        /// </summary>
        /// <param name="id">The management server id</param>
        /// <returns>A list of UnixComputer instances.</returns>
        public IEnumerable<IPersistedUnixComputer> GetExistingUnixComputerFromManagementServer(Guid id)
        {
            var retval = new List<IPersistedUnixComputer>();
            var factory = this.managementGroupConnection.CreateRelationshipObjectFactory("Microsoft.SystemCenter.HealthServiceShouldManageEntity");
            var relationshipObjects = factory.GetAllRelatedObjects(id);

            foreach (var relationObj in relationshipObjects)
            {
                if (relationObj.Target.IsInstanceOfMPClass(this.managementGroupConnection, "Microsoft.Unix.Computer"))
                {
                    retval.Add(new PersistedUnixComputer(relationObj.Target, factory, this.managementGroupConnection));
                }
            }

            return retval;
        }

        /// <summary>
        /// Creates a new UnixComputer instances already presented in OpsMgr database by its Id.
        /// </summary>
        /// <param name="id">GUID for Unix Computer</param>
        /// <returns>A new UnixComputer instance.</returns>
        public IPersistedUnixComputer GetExistingUnixComputersById(Guid id)
        {
            var objectFactory = this.managementGroupConnection.CreateManagedObjectFactory("Microsoft.Unix.Computer");
            var computerManagedObject = objectFactory.GetExistingManagedObjectById(id);

            return new PersistedUnixComputer(computerManagedObject, this.relationshipFactory, this.managementGroupConnection);
        }
    }
}
