//-----------------------------------------------------------------------
// <copyright file="DiscoveryResult.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks.Properties;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;

    /// <summary>
    /// Represents information about the results of a host discovery.
    /// </summary>
    public class DiscoveryResult
    {
        /// <summary>
        ///     Instantiates this object with the given criteria.
        /// </summary>
        /// <param name="criteria">The single host discovery criteria.</param>
        public DiscoveryResult(DiscoveryTargetEndpoint criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException("criteria", Resources.DiscoveryResult_Criteria_NULL);
            }

            Criteria = criteria;
        }

        /// <summary>
        /// Gets or sets the (single host) criteria that was used for this discovery.
        /// </summary>
        public DiscoveryTargetEndpoint Criteria { get; set; }

        /// <summary>
        /// Gets or sets the Operating System Information discovered.
        /// </summary>
        public IOSInformation OSInfo { get; set; }

        /// <summary>
        /// Gets or sets the Supported Agent information discovered from imported management packs.
        /// </summary>
        public ISupportedAgent SupportedAgent { get; set; }

        /// <summary>
        /// Gets or sets the agent kit to install. May be empty.
        /// </summary>
        public string InstallableKit { get; set; }

        /// <summary>
        /// Gets or sets any agent information discovered.
        /// </summary>
        public AgentInformation AgentData { get; set; }

        /// <summary>
        /// Gets or sets any ManagementActionPoint.
        /// </summary>
        public IManagedObject ManagementActionPoint { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating the Endpoint already exists within the ManagementActionPoint
        /// </summary>
        public bool? EndpointAlreadyManaged { get; set; }

        /// <summary>
        /// Gets the name of the remote host
        /// </summary>
        public string HostName 
        { 
            get
            {
                if (Criteria != null)
                {
                    return Criteria.HostName;
                }

                return string.Empty;
            }
        }

        /// <summary>
        ///     Returns the update success or failure. The absence of any error
        ///     data indicates success.
        /// </summary>
        public bool Succeeded
        {
            get
            {
                return null == ErrorData;
            }
        }

        /// <summary>
        ///     If this property is not null then it contains the exception that
        ///     caused the update to fail.  This may also contain UNIX error codes,
        ///     standard output, and standard error.
        /// </summary>
        public Exception ErrorData { get; set; }
    }
}
