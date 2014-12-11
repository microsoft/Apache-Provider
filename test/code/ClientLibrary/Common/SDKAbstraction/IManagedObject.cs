//-----------------------------------------------------------------------
// <copyright file="IManagedObject.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using System;
    using Microsoft.EnterpriseManagement.Common;

    /// <summary>
    /// Represents an OpsMgr managed object.
    /// </summary>
    public interface IManagedObject
    {
        /// <summary>
        /// Gets the display name of the managed object
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Gets the value of a named property.
        /// </summary>
        /// <param name="propertyName">The name of the property to return.</param>
        /// <returns>The value of the property.</returns>
        string GetPropertyValue(string propertyName);

        /// <summary>
        /// Sets the value of a named property.
        /// </summary>
        /// <param name="propertyName">Name of property to set.</param>
        /// <param name="value">Value to set</param>
        void SetPropertyValue(string propertyName, string value);

        /// <summary>
        /// Sets the value of a named property.
        /// </summary>
        /// <param name="propertyName">Name of property to set.</param>
        /// <param name="value">Value to set</param>
        void SetPropertyValue(string propertyName, int value);

        /// <summary>
        /// Gets the contained OpsMgr managed object.
        /// </summary>
        EnterpriseManagementObject OpsMgrObject { get; }

        /// <summary>
        /// Gets the contained OpsMgr managed object Id.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// This will get the name of the most derived class of this type that happens to be registered in OpsMgr.
        /// </summary>
        /// <returns>The name of the most derived class of this type that happens to be registered in OpsMgr.</returns>
        string GetMostDerivedClassName();

        /// <summary>
        /// Commits changes to database.
        /// </summary>
        void Commit();

        /// <summary>
        /// Return true if this instance is the instance of the given class.
        /// </summary>
        /// <param name="managementGroupConnection">The management group connection.</param>
        /// <param name="managementPackClassName">the management pack class name</param>
        /// <returns>True if this instance is an instance of given class.</returns>
        bool IsInstanceOfMPClass(IManagementGroupConnection managementGroupConnection, string managementPackClassName);
    }
}