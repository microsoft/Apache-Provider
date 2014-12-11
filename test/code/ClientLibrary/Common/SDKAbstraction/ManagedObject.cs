//-----------------------------------------------------------------------
// <copyright file="ManagedObject.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.EnterpriseManagement.Common;
    using Microsoft.EnterpriseManagement.Configuration;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.Utilities;

    /// <summary>
    /// Base class for managed objects (i.e. instances of types defined in management pack).
    /// </summary>
    public class ManagedObject : IManagedObject
    {
        /// <summary>
        /// Gets the display name of the managed object
        /// </summary>
        public string DisplayName
        {
            get
            {
                return this.OpsMgrObject.DisplayName;
            }
        }

        /// <summary>
        /// Gets the Id from wrapped object. 
        /// </summary>
        public Guid Id
        {
            get
            {
                return this.OpsMgrObject.Id;
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the ManagedObject class.
        /// </summary>
        /// <param name="opsMgrObject">OpsMgr representation of the managed object.</param>
        public ManagedObject(EnterpriseManagementObject opsMgrObject)
        {
            this.OpsMgrObject = opsMgrObject;
        }

        /// <summary>
        /// Gets the value of a named property.
        /// </summary>
        /// <param name="propertyName">The name of the property to return.</param>
        /// <returns>The value of the property.</returns>
        public string GetPropertyValue(string propertyName)
        {
            foreach (var property in this.GetProperties())
            {
                if (this.GetPropertyName(property) == propertyName)
                {
                    return this.GetMPPropertyValue(property);
                }
            }

            throw new ManagedObjectPropertyNotFoundException(propertyName);
        }

        /// <summary>
        /// Return this object is an instance of the given class.
        /// </summary>
        /// <param name="managementGroupConnection">The mangement group connection.</param>
        /// <param name="managementPackClassName">The management pack class.</param>
        /// <returns>True if this is an instance of the given class.</returns>
        public virtual bool IsInstanceOfMPClass(IManagementGroupConnection managementGroupConnection, string managementPackClassName)
        {
            var criteria1 = new ManagementPackClassCriteria(String.Format(CultureInfo.InvariantCulture, "Name = '{0}'", managementPackClassName));

            var managementPackClasses = managementGroupConnection.EntityTypes.GetClasses(criteria1);

            if (managementPackClasses == null || managementPackClasses.Count != 1)
            {
                throw new ManagementPackTypeNotFoundException(managementPackClassName);
            }

            return this.OpsMgrObject.IsInstanceOf(managementPackClasses[0]);
        }

        /// <summary>
        /// Sets the value of a named property.
        /// </summary>
        /// <param name="propertyName">Name of property to set.</param>
        /// <param name="value">Value to set</param>
        public void SetPropertyValue(string propertyName, string value)
        {
            foreach (var property in this.GetProperties())
            {
                if (this.GetPropertyName(property) == propertyName)
                {
                    this.SetMPPropertyValue(property, value);
                    return;
                }
            }

            throw new ManagedObjectPropertyNotFoundException(propertyName);
        }

        /// <summary>
        /// Sets the value of a named property.
        /// </summary>
        /// <param name="propertyName">Name of property to set.</param>
        /// <param name="value">Value to set</param>
        public void SetPropertyValue(string propertyName, int value)
        {
            this.SetPropertyValue(propertyName, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Gets the contained OpsMgr managed object.
        /// </summary>
        public EnterpriseManagementObject OpsMgrObject { get; private set; }

        /// <summary>
        /// This will get the name of the most derived class of this type that happens to be registered in OpsMgr.
        /// </summary>
        /// <returns>The name of the most derived class of this type that happens to be registered in OpsMgr.</returns>
        public string GetMostDerivedClassName()
        {
            var derivedClasses = this.OpsMgrObject.GetMostDerivedClasses();
            if (1 != derivedClasses.Count)
            {
                throw new UniqueDerivedTypeNotFoundException(this.OpsMgrObject.FullName, derivedClasses.Count);
            }

            return derivedClasses[0].Name;
        }

        /// <summary>
        /// Commits changes to database.
        /// </summary>
        public void Commit()
        {
            this.OpsMgrObject.Commit();
        }

        /// <summary>
        /// Sets the value of a property. Overridden in tests.
        /// </summary>
        /// <param name="managementPackProperty">Property to set value for.</param>
        /// <param name="value">Value to set.</param>
        protected virtual void SetMPPropertyValue(ManagementPackProperty managementPackProperty, string value)
        {
            this.OpsMgrObject[managementPackProperty].Value = value;
        }

        /// <summary>
        /// Gets the value of a property. Overridden in tests.
        /// </summary>
        /// <param name="managementPackProperty">Property to get value for.</param>
        /// <returns>Value of property.</returns>
        protected virtual string GetMPPropertyValue(ManagementPackProperty managementPackProperty)
        {
            return this.OpsMgrObject[managementPackProperty].Value as string;
        }

        /// <summary>
        /// Gets the properties from wrapped object. Overridden in tests.
        /// </summary>
        /// <returns>Properties from wrapped object.</returns>
        protected virtual IList<ManagementPackProperty> GetProperties()
        {
            return this.OpsMgrObject.GetProperties();
        }

        /// <summary>
        /// Gets the name of a property. Overridden in tests.
        /// </summary>
        /// <param name="managementPackProperty">Property to get name for.</param>
        /// <returns>Name of property.</returns>
        protected virtual string GetPropertyName(ManagementPackProperty managementPackProperty)
        {
            return managementPackProperty.Name;
        }
    }
}
