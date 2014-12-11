// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnixComputerOperatingSystem.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;

    public class UnixComputerOperatingSystem : IUnixComputerOperatingSystem
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the UnixComputerOperatingSystem class.
        /// </summary>
        /// <param name="managedObject">
        /// Managed object containing the data for the operating system instance.
        /// </param>
        public UnixComputerOperatingSystem(IManagedObject managedObject)
        {
            if (managedObject == null)
            {
                throw new ArgumentNullException("managedObject");
            }

            this.ManagedObject = managedObject;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the computer that matches this Operating System instance.
        /// </summary>
        public string ComputerName
        {
            get
            {
                return this.ManagedObject.GetPropertyValue("PrincipalName");
            }
        }

        /// <summary>
        /// Gets the managed object representation of the operating system instance.
        /// </summary>
        public IManagedObject ManagedObject { get; private set; }

        /// <summary>
        /// Gets the name of the operating system.
        /// </summary>
        public string Platform
        {
            get
            {
                return this.ManagedObject.DisplayName;
            }
        }

        /// <summary>
        /// Gets the version of the operating system.
        /// </summary>
        public string Version
        {
            get
            {
                return this.ManagedObject.GetPropertyValue("OSVersion");
            }
        }

        #endregion
    }
}