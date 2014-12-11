// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGetUnixComputerOperatingSystem.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System.Collections.Generic;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;

    public interface IGetUnixComputerOperatingSystem
    {
        #region Public Methods

        /// <summary>
        /// Returns all of the UNIX/Linux operating system instances.
        /// </summary>
        /// <param name="managementGroupConnection">
        ///   The management groupConnection to connect to.
        /// </param>
        /// <returns>
        /// This method will return a list of UNIX/Linux operating system.
        /// </returns>
        IEnumerable<IUnixComputerOperatingSystem> Invoke(IManagementGroupConnection managementGroupConnection);

        #endregion
    }
}