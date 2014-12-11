// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGetDNSHostName.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;

    /// <summary>
    ///     This defines the interface for client access to the DNS lookup MP task
    /// </summary>
    public interface IGetDNSHostName
    {
        #region Public Methods

        /// <summary>
        /// given a hostname, return its corresponding FQDN
        /// </summary>
        /// <param name="hostname">
        /// input hostname
        /// </param>
        /// <param name="managementGroupConnection">
        /// The ManagementGroupConnection to use.
        /// </param>
        /// <param name="managementActionPointName">
        /// the name of the resource pool to connect to
        /// </param>
        /// <returns>
        /// Fully qualified domain name
        /// </returns>
        string Invoke(string hostname, IManagementGroupConnection managementGroupConnection, string managementActionPointName);

        /// <summary>
        ///     given a hostname, return its corresponding FQDN
        /// </summary>
        /// 
        /// <param name="hostname">input hostname</param>
        /// <param name="managementGroup">The management group to connect to.</param>
        /// <param name="managementActionPoint">the management action point to connect to</param>
        /// <returns>
        /// Fully qualified domain name
        /// </returns>
        string Invoke(string hostname, IManagementGroupConnection managementGroup, IManagedObject managementActionPoint);

        #endregion
    }
}