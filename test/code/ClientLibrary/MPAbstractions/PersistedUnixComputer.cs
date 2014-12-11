// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersistedUnixComputer.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
//   Defines the PersistedUnixComputer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System.Collections.Generic;
    using System.Diagnostics;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction.Exceptions;

    internal class PersistedUnixComputer : UnixComputer, IPersistedUnixComputer
    {
        /// <summary>
        /// Relationship defining what resource pool manages this computer.
        /// </summary>
        private IRelationshipObject managedByRelationship;

        /// <summary>
        /// ManagementGroupConnection that this computer belongs to.
        /// </summary>
        private readonly IManagementGroupConnection managementGroupConnection;

        private IRelationshipObjectFactory relationshipFactory;

        public PersistedUnixComputer(IManagedObject managedObject, IRelationshipObjectFactory relationshipFactory, IManagementGroupConnection managementGroupConnection)
            : base(managedObject)
        {
            this.managementGroupConnection = managementGroupConnection;
            this.relationshipFactory = relationshipFactory;
            this.SetManagementActionPoint();
        }

        public void Update()
        {
            var persistenceSession = this.managementGroupConnection.CreateDiscoveryData();
            persistenceSession.Add(this.ManagedObject);
            
            if (this.ManagingRelationshipShouldBeChanged())
            {
                this.ChangeManagedByRelationship(persistenceSession);
            }

            persistenceSession.Commit();
            this.SetManagementActionPoint();
        }

        public void Unmanage()
        {
            var persistenceSession = this.managementGroupConnection.CreateDiscoveryData();
            persistenceSession.Remove(this.ManagedObject);

            persistenceSession.Commit();
        }

        private void ChangeManagedByRelationship(IIncrementalDiscoveryData persistenceSession)
        {
            if (this.managedByRelationship != null)
            {
                persistenceSession.Remove(this.managedByRelationship);
            }

            var relationship = relationshipFactory.CreateRelationshipObject(
                this.ManagementActionPoint, this.ManagedObject);
            persistenceSession.Add(relationship);
        }

        static Dictionary<System.Guid, IRelationshipObject> existing_relationships = new Dictionary<System.Guid, IRelationshipObject>();
        static bool relationships_initialized = false;

        public void UpdateMAPCache()
        {
            existing_relationships.Clear();
            var list = this.relationshipFactory.GetExistingRelationships();

            foreach (var relationship in list)
            {
                if (existing_relationships.ContainsKey(relationship.Target.Id))
                {

                    // Two identical keys found.. not good
                    string warningText = "More than one managed-by relationship detected for unix computer " +
                        relationship.Target.DisplayName;
                    Trace.TraceWarning(warningText, relationship.Target.DisplayName);
                    Debug.Assert(false, warningText);


                }
                else
                {
                    existing_relationships.Add(relationship.Target.Id, relationship);
                }
            }

            relationships_initialized = true;
        }

        private void SetManagementActionPoint()
        {
            if (!relationships_initialized)
            {
                UpdateMAPCache();
            }

            // Try looking up "quickly" (not trying HealthServiceShouldManageEntity).  In a OM2012 post-upgrade
            // world, this is much faster.  But if we fail, then revert to the slower way.
            //this.managedByRelationship = this.relationshipFactory.GetExistingRelationshipWhereTarget(ManagedObject);
            if (existing_relationships.ContainsKey(this.ManagedObject.Id))
            {
                this.managedByRelationship = existing_relationships[this.ManagedObject.Id];
                this.ManagementActionPoint = this.managedByRelationship.Source;
            }
            else
            {
                // Create a relationship factory consisting of two relationship types so as to
                // query the realtionship objects created with OM 07 R2. 
                IList<string> relationshipNames = new List<string>();
                relationshipNames.Add("Microsoft.SystemCenter.ManagementActionPointShouldManageEntity");
                relationshipNames.Add("Microsoft.SystemCenter.HealthServiceShouldManageEntity");
                var localRelationshipFactory = this.managementGroupConnection.CreateRelationshipObjectFactory(relationshipNames);

                try
                {
                    this.managedByRelationship = localRelationshipFactory.GetExistingRelationshipWhereTarget(this.ManagedObject);
                    this.ManagementActionPoint = managedByRelationship.Source;
                }
                catch (ManagedObjectRelationshipNotFoundException)
                {
                    Trace.TraceWarning("The Persisted Unix Computer {0} has lost its Resource Pool association", ManagedObject.DisplayName);
                    this.managedByRelationship = null;
                    this.ManagementActionPoint = null;
                }
            }
        }

        private bool ManagingRelationshipShouldBeChanged()
        {
            return this.managedByRelationship == null ? true :
                    this.ManagementActionPoint != this.managedByRelationship.Source;
        }

        /// <summary>
        /// Type of unix computer. Eg. Microsoft.Linux.SLES.10.Computer
        /// </summary>
        public string UnixComputerType
        {
            get
            {
                return this.ManagedObject.GetMostDerivedClassName();
            }
        }

        /// <summary>
        /// Gets the string that identifies the platform of this computer in the Management Pack.
        /// E.g. if the UnixComputerType is "Microsoft.Linux.SLES.10.Computer", the platform identifier is
        /// "Microsoft.Linux.SLES.10".
        /// </summary>
        public string ManagementPackPlatformIdentifier
        {
            get
            {
                var lastDot = this.UnixComputerType.LastIndexOf('.');
                return this.UnixComputerType.Substring(0, lastDot);
            }
        }

        public IManagedObject ManagementActionPoint { get; set; }
    }
}
