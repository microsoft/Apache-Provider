// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUnixComputerOperatingSystemFactory.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System.Collections.Generic;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks;

    public interface IUnixComputerOperatingSystemFactory
    {
        #region Public Methods

        /// <summary>
        /// Creates a list of all UNIX/Linux operating system instances already presented in OpsMgr database.
        /// </summary>
        /// <returns>
        /// A list of UnixComputer instances.
        /// </returns>
        IEnumerable<IUnixComputerOperatingSystem> GetAllExistingInstances();

        #endregion
    }
}