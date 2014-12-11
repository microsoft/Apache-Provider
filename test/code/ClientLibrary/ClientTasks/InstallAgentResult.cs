//-----------------------------------------------------------------------
// <copyright file="InstallAgentResult.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System;
    using System.Diagnostics;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks.Properties;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.Utilities;

    public enum InstallAction
    {
        ErrorNoAgentIsInstalled,
        InstalledFromNoAgent,
        InstalledFromMissingAgentVersionInfo,
        AlreadyUpToDate,
        NoInstalledFromSupportedVersion,
        InstalledFromNonSupportedVersion
    }

    /// <summary>
    /// Holds information about the results of install agent.
    /// </summary>
    public class InstallAgentResult
    {
        #region Types


        #endregion Types

        #region Lifecycle

        /// <summary>
        ///     Instantiates this object with the given discovery result.
        /// </summary>
        /// <param name="discoveryResult">The discovery result the server.</param>
        public InstallAgentResult(DiscoveryResult discoveryResult)
        {
            if (discoveryResult == null)
            {
                throw new ArgumentNullException("discoveryResult", Resources.InstallAgentResult_InstallAgentResult_A_discovery_result_must_be_specified);
            }

            DiscoveryResult = discoveryResult;

            Hostname = discoveryResult.Criteria.HostName;

            UnixComputer = null;

            if (discoveryResult.SupportedAgent != null)
            {
                SupportedVersion = discoveryResult.SupportedAgent.AgentVersion;
            }

            StartedAgentInfo = discoveryResult.AgentData;
        }

        #endregion Lifecycle

        #region Properties

        /// <summary>
        ///     Hostname or IP address of the managed server targeted for update.
        /// </summary>
        public string Hostname { get; private set; }

        /// <summary>
        ///     Action attempted by the install workflow.
        /// </summary>
        public InstallAction Action
        {
            get 
            {
                if (UnixComputer == null || !this.Succeeded)
                {
                    return InstallAction.ErrorNoAgentIsInstalled;
                }

                if ((StartedAgentInfo == null || StartedAgentInfo.IsInstalled == false) && UnixComputer != null)
                {
                    return InstallAction.InstalledFromNoAgent;
                }

                if (string.IsNullOrEmpty(StartedAgentInfo.Version))
                {
                    return InstallAction.InstalledFromMissingAgentVersionInfo;
                }

                UnixAgentVersion startedVersion = new UnixAgentVersion(StartedAgentInfo.Version);

                if (startedVersion < UnixComputer.AgentVersion && startedVersion < SupportedVersion)
                {
                    return InstallAction.InstalledFromNonSupportedVersion;
                }

                if (startedVersion == InstallableAgentVersion)
                {
                    return InstallAction.AlreadyUpToDate;
                }

                return InstallAction.NoInstalledFromSupportedVersion;
            }
        }

        /// <summary>
        /// Gets or sets any agent information discovered.
        /// </summary>
        public AgentInformation StartedAgentInfo { get; set; }

        /// <summary>
        /// Gets or sets supported agent version.
        /// </summary>
        public UnixAgentVersion SupportedVersion { get; set; }

        /// <summary>
        /// Gets or sets latest installable agent version.
        /// </summary>
        public UnixAgentVersion InstallableAgentVersion { get; set; }

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

        public DiscoveryResult DiscoveryResult { get; private set; }

        /// <summary>
        ///     The created unix computer.
        /// </summary>
        public IPersistedUnixComputer UnixComputer { get; set; }

 
        /// <summary>
        ///     If this property is not null then it contains the exception that
        ///     caused the update to fail.  This may also contain UNIX error codes,
        ///     standard output, and standard error.
        /// </summary>
        public Exception ErrorData { get; set; }

        #endregion Properties
    }
}