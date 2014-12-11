// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUnmanageUnixComputer.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
//   Defines the IUnmanageUnixComputer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System.Collections.Generic;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;

    /// <summary>
    ///     This defines the interface for workflow actions that uninstall the
    ///     SCX agent on System Center Operations Manager managed host.
    /// </summary>
    public interface IUnmanageUnixComputer
    {
        /// <summary>
        ///     Iterate through a list of managed hosts, changing them from managed to unmanaged.
        /// </summary>
        /// 
        /// <param name="agents">A collection of managed hosts to unmanage.</param>
        /// <param name="managementGroupConnection">The ManagementGroupConnection to use.</param>
        /// 
        /// <returns>
        ///     This method will return a list containing an unmanage result for
        ///     each of the given hostnames.
        /// </returns>
        IEnumerable<UnmanageResult> Invoke(IEnumerable<IPersistedUnixComputer> agents, IManagementGroupConnection managementGroupConnection);
    }
}
