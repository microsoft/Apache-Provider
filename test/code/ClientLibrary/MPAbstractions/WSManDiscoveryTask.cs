//-----------------------------------------------------------------------
// <copyright file="WSManDiscoveryTask.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System.Diagnostics;
    using System.Globalization;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core;

    /// <summary>
    /// Represents the WSMan Discovery Task found in the Unix library MP.
    /// </summary>
    public class WSManDiscoveryTask : WSManDiscoveryTaskBase
    {
        /// <summary>
        /// Initializes a new instance of the WSManDiscoveryTask class.
        /// </summary>
        public WSManDiscoveryTask() :
            base("Microsoft.Unix.WSMan.Discovery.Task")
        {
        }
    }
}
