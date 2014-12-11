//-----------------------------------------------------------------------
// <copyright file="ManagedObjectFactory.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text.RegularExpressions;

    using Microsoft.EnterpriseManagement;
    using Microsoft.EnterpriseManagement.Common;
    using Microsoft.EnterpriseManagement.Configuration;
    using Microsoft.EnterpriseManagement.Monitoring;

    /// <summary>
    /// Class that wraps the CreatableEnterpriseManagementObject from the OpsMgr SDK.
    /// </summary>
    public class ManagedObjectFactory : IManagedObjectFactory
    {
        /// <summary>
        /// Handle for tracing.
        /// </summary>
        private static readonly TraceSource trace = new TraceSource("Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction");

        /// <summary>
        /// Enterprise management group to use for creating EnterpriseManagementObjects.
        /// </summary>
        private readonly EnterpriseManagementGroup group;

        /// <summary>
        /// Management pack class that this factory creates instances of.
        /// </summary>
        private readonly ManagementPackClass managementPackClass;

        /// <summary>
        /// Class used for retrieving instances from the SDK.
        /// </summary>
        private readonly IEntityObjectsManagement entityObjects;

        /// <summary>
        /// Class used for retrieving management pack classes from the SDK.
        /// </summary>
        private readonly IEntityTypeManagement entityTypes;

        /// <summary>
        /// Initializes a new instance of the ManagedObjectFactory class.
        /// </summary>
        /// <param name="group">Management group where object live.</param>
        /// <param name="entityTypes">SDK class that is used for looking up classes.</param>
        /// <param name="entityObjects">SDK class that is used for looking up instances.</param>
        /// <param name="managementPackClassName">Name of management pack class that this factory handles.</param>
        public ManagedObjectFactory(
            EnterpriseManagementGroup group, 
            IEntityTypeManagement entityTypes, 
            IEntityObjectsManagement entityObjects, 
            string managementPackClassName)
        {
            this.group = group;
            this.entityTypes = entityTypes;
            this.entityObjects = entityObjects;

            var criteria = new ManagementPackClassCriteria(String.Format(CultureInfo.InvariantCulture, "Name = '{0}'", managementPackClassName));

            trace.TraceEvent(TraceEventType.Information, 1, "Requesting management pack classes from SDK using query criteria: {0}", criteria.Criteria);

            IList<ManagementPackClass> managementPackClasses = this.entityTypes.GetClasses(criteria);

            trace.TraceEvent(TraceEventType.Information, 2, "Number of management pack classes returned: {0}", managementPackClasses.Count);

            if (managementPackClasses.Count != 1)
            {
                throw new ManagementPackTypeNotFoundException(managementPackClassName);
            }

            this.managementPackClass = managementPackClasses[0];
        }

        /// <summary>
        /// Creates a new enterprise management object.
        /// </summary>
        /// <returns>A new EnterpriseManagementObject.</returns>
        public IManagedObject CreateNewManagedObject()
        {
            CreatableEnterpriseManagementObject emo = this.CreateEmptyEnterpriseManagementObject();
            return new ManagedObject(emo);
        }

        /// <summary>
        /// Creates an EnterpriseManagementObject from data already in the database.
        /// </summary>
        /// <param name="displayName">Display name or wildcard to use as key when searching for the object.</param>
        /// <param name="isRegularExpression">Indicate if the input is a regular expression.</param>
        /// <returns>Object retrieved from the OpsMgr SDK.</returns>
        public IEnumerable<IManagedObject> GetExistingManagedObject(string displayName, bool isRegularExpression)
        {
            IObjectReader<EnterpriseManagementObject> reader =
                this.entityObjects.GetObjectReader<EnterpriseManagementObject>(this.managementPackClass, ObjectQueryOptions.Default);

            trace.TraceEvent(TraceEventType.Information, 4, "Number of Enterprise Management Objects returned: {0}", reader.Count);

            var retVal = new List<IManagedObject>();
            for (int i = 0; i < reader.Count; ++i)
            {
                var obj = reader.GetData(i);
                if (obj != null && (isRegularExpression ? Regex.IsMatch(obj.DisplayName, displayName) : obj.DisplayName == displayName))
                {
                    retVal.Add(new ManagedObject(obj));
                }
            }

            if (0 == retVal.Count)
            {
                throw new ManagedObjectNotFoundException(displayName);
            }

            return retVal;
        }

        /// <summary>
        /// Creates an EnterpriseManagementObject from data already in the database.
        /// </summary>
        /// <param name="id">Guid Id to use as key when searching for the object.</param>
        /// <returns>Object retrieved from the OpsMgr SDK.</returns>
        public IManagedObject GetExistingManagedObjectById(Guid id)
        {
            trace.TraceEvent(TraceEventType.Information, 3, "Requesting Enterprise Management Objects from SDK using query criteria: {0}", id);

            EnterpriseManagementObject managedObj =
                this.entityObjects.GetObject<EnterpriseManagementObject>(id, ObjectQueryOptions.Default);

            if (managedObj == null)
            {
                throw new ManagedObjectNotFoundException(id.ToString());
            }

            return new ManagedObject(managedObj);
        }

        /// <summary>
        /// Returns all existing instances of this type.
        /// </summary>
        /// <returns>Enumeration of all instances of this type.</returns>
        public IEnumerable<IManagedObject> GetAllInstances()
        {
            var retval = new List<IManagedObject>();
            foreach (var opsMgrObject in this.entityObjects.GetObjectReader<EnterpriseManagementObject>(this.managementPackClass, ObjectQueryOptions.Default))
            {
                retval.Add(new ManagedObject(opsMgrObject));
            }

            return retval;
        }

        /// <summary>
        /// Creates a ManagedObject from a given criteria
        /// </summary>
        /// <param name="criteria">Criteria to use for query the object.</param>
        /// <returns>Object retrieved from the OpsMgr SDK.</returns>
        public IManagedObject GetManagedObjectWithCriteria(string criteria)
        {
            trace.TraceEvent(TraceEventType.Information, 3, "Requesting Enterprise Management Objects from SDK using query criteria: {0}", criteria);

            MonitoringObjectCriteria msCriteria = new MonitoringObjectCriteria(criteria, this.managementPackClass);

            IObjectReader<EnterpriseManagementObject> reader =
                this.group.EntityObjects.GetObjectReader<EnterpriseManagementObject>(msCriteria, ObjectQueryOptions.Default);

            trace.TraceEvent(TraceEventType.Information, 4, "Number of Enterprise Management Objects returned: {0}", reader.Count);

            if (0 == reader.Count)
            {
                throw new ManagedObjectNotFoundException(criteria);
            }

            return new ManagedObject(reader.GetData(0));
        }

        /// <summary>
        /// Creates an empty EnterpriseManagementObject. Can be overridden in test code.
        /// </summary>
        /// <returns>New CreatableEnterpriseManagementObject.</returns>
        protected virtual CreatableEnterpriseManagementObject CreateEmptyEnterpriseManagementObject()
        {
            return new CreatableEnterpriseManagementObject(this.group, this.managementPackClass);
        }
    }
}
