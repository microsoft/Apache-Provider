// ----------------------------------------------------------------------------------------------------
// <copyright file="ManagementServerInfo.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;

    public class ManagementServerInfo
    {
        /// <summary>
        /// Gets the Default Management Server name.
        /// </summary>
        /// <returns>The default management server</returns>
        public string GetDefaultManagementServer()
        {
            string defaultServer = string.Empty;

            if (!String.IsNullOrEmpty(GetDefaultManagementServerName()))
            {
                defaultServer = GetDefaultManagementServerName();
            }

            return defaultServer;
        }

        /// <summary>
        /// Internal function to access ManagementGroupConnection.DefaultManagementServerName (for test purposes).
        /// </summary>
        /// <returns>The property: ManagementGroupConnection.DefaultManagementServerName</returns>
        protected virtual string GetDefaultManagementServerName()
        {
            return ManagementGroupConnection.DefaultManagementServerName;
        }
    }
}
