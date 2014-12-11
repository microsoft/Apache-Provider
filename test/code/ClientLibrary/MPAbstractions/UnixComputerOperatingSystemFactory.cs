// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnixComputerOperatingSystemFactory.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System.Collections.Generic;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;

    public class UnixComputerOperatingSystemFactory : IUnixComputerOperatingSystemFactory
    {
        #region Constants and Fields

        /// <summary>
        /// Handle to OpsMgr SDK.
        /// </summary>
        private readonly IManagementGroupConnection managementGroupConnection;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the UnixComputerOperatingSystemFactory class.
        /// </summary>
        /// <param name="managementGroupConnection">
        /// Handle to OpsMgr SDK.
        /// </param>
        public UnixComputerOperatingSystemFactory(IManagementGroupConnection managementGroupConnection)
        {
            this.managementGroupConnection = managementGroupConnection;
        }

        #endregion

        #region Implemented Interfaces

        #region IUnixComputerOperatingSystemFactory

        /// <summary>
        /// Creates a list of all UNIX/Linux operating system instances already presented in OpsMgr database.
        /// </summary>
        /// <returns>
        /// A list of UnixComputer instances.
        /// </returns>
        public virtual IEnumerable<IUnixComputerOperatingSystem> GetAllExistingInstances()
        {
            IManagedObjectFactory operatingSystemFactory =
                this.managementGroupConnection.CreateManagedObjectFactory("Microsoft.Unix.OperatingSystem");

            var retval = new List<IUnixComputerOperatingSystem>();

            IEnumerable<IManagedObject> operatingSystemInstances = operatingSystemFactory.GetAllInstances();

            foreach (IManagedObject operatingSystemInstance in operatingSystemInstances)
            {
                retval.Add(new UnixComputerOperatingSystem(operatingSystemInstance));
            }

            return retval;
        }

        #endregion

        #endregion
    }
}