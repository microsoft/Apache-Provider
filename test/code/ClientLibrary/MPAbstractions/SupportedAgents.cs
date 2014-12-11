//-----------------------------------------------------------------------
// <copyright file="SupportedAgents.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;

    /// <summary>
    /// Maintains a list of all the supported Unix agent types in the system.
    /// </summary>
    public class SupportedAgents : ISupportedAgents
    {
        /// <summary>
        /// List of all the supported Unix Agent types in the system.
        /// </summary>
        private IList<ISupportedAgent> supportedAgents = new List<ISupportedAgent>();

        /// <summary>
        /// Initializes a new instance of the SupportedAgents class.
        /// </summary>
        /// <param name="managementGroupConnection">Handle to OpsMgr SDK.</param>
        public SupportedAgents(IManagementGroupConnection managementGroupConnection)
        {
            var objectFactory = managementGroupConnection.CreateManagedObjectFactory("Microsoft.Unix.SupportedAgent");
            IEnumerable<IManagedObject> supportedAgentData = objectFactory.GetAllInstances();
            foreach (var agent in supportedAgentData)
            {
                this.supportedAgents.Add(new SupportedAgent(agent));
            }
        }

        /// <summary>
        /// Gets the best matching supported agent in the system given the supplied criteria.
        /// </summary>
        /// <param name="operatingSystem">Operating system that has to match.</param>
        /// <param name="architecture">Architecture that has to match.</param>
        /// <param name="operatingSystemVersion">Operating system version has to be higher than or equal to a minimum supported version.</param>
        /// <returns>Best matching Supported Agent.</returns>
        public ISupportedAgent GetSupportedAgent(string operatingSystem, string architecture, OperatingSystemVersion operatingSystemVersion)
        {
            var matches = from supportedAgent in this.supportedAgents
                          where supportedAgent.OS.Equals(operatingSystem, StringComparison.InvariantCultureIgnoreCase)
                          && supportedAgent.Architecture.Equals(architecture, StringComparison.InvariantCultureIgnoreCase)
                          && ! operatingSystemVersion.IsOlderThan(supportedAgent.MinimumSupportedOSVersion)
                          select supportedAgent;

            return this.GetUniqueResult(matches);
        }

        /// <summary>
        /// Retrive information about the agent which supports this operating system.
        /// </summary>
        /// <param name="supportedMPClassName">ManagementPack class that is used to monitor computers supported by this agent.</param>
        /// <param name="architecture">Architecture to get support information for.</param>
        /// <returns>A SupportedAgent class.</returns>
        public ISupportedAgent GetSupportedAgent(string supportedMPClassName, string architecture)
        {
            var matches = from supportedAgent in this.supportedAgents
                          where supportedAgent.SupportedManagementPackClassName.Equals(supportedMPClassName)
                                && supportedAgent.Architecture.Equals(architecture)
                          select supportedAgent;

            if (matches.Count() == 0)
            {
                throw new NoMatchingSupportedAgentException();
            }

            if (matches.Count() > 1)
            {
                throw new DuplicateSupportedAgentsException();
            }

            return matches.First();
        }

        /// <summary>
        /// Returns the unique result with the highest minimum supported os version.
        /// </summary>
        /// <param name="matches">All matches.</param>
        /// <returns>The match with the highest minimum supported os version.</returns>
        private ISupportedAgent GetUniqueResult(IEnumerable<ISupportedAgent> matches)
        {
            if (matches.Count() == 0)
            {
                throw new NoMatchingSupportedAgentException();
            }

            var retval = matches.ElementAt(0);
            foreach (var match in matches)
            {
                if (retval.MinimumSupportedOSVersion.IsOlderThan(match.MinimumSupportedOSVersion))
                {
                    retval = match;
                }
            }

            return retval;
        }
    }
}
