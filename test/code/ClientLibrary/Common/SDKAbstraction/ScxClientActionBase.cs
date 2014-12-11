// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScxClientActionBase.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
//   Defines the ScxClientActionBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    public class ScxClientActionBase
    {
        /// <summary>
        /// Return the health service of a target management server.
        /// </summary>
        /// <param name="managementGroupConnection">ManagementGroupConnection that we are currently using</param>
        /// <param name="managementTarget">FQDN representing the target management server to run tasks on</param>
        /// <returns>Health service to run target tasks on</returns>
        protected virtual IManagedObject GetOpsMgrTarget(IManagementGroupConnection managementGroupConnection, string managementTarget)
        {
            return managementGroupConnection.GetManagementActionPoint(managementTarget);
        }
    }
}