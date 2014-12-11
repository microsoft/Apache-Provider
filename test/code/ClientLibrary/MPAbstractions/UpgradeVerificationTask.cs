//-----------------------------------------------------------------------
// <copyright file="UpgradeVerificationTask.cs" company="Microsoft">
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
    /// Represents the wsman upgrade verification Task found in the Unix library MP.
    /// </summary>
    public class UpgradeVerificationTask : WSManDiscoveryTaskBase
    {
        /// <summary>
        /// Initializes a new instance of the wsman UpgradeVerification class.
        /// </summary>
        public UpgradeVerificationTask() :
            base("Microsoft.Unix.WSMan.UpgradeVerification.Task")
        {
        }
    }
}
