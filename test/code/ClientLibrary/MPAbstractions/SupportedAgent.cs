//-----------------------------------------------------------------------
// <copyright file="SupportedAgent.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.Utilities;

    /// <summary>
    /// Represents supported agent information found in Management Packs.
    /// </summary>
    public class SupportedAgent : ISupportedAgent
    {
        private IManagedObject managedObject;

        /// <summary>
        /// Initializes a new instance of the SupportedAgent class from the data retrieved from OpsMgr SDK.
        /// </summary>
        /// <param name="sdkData">Data retrieved from SDK.</param>
        public SupportedAgent(IManagedObject sdkData)
        {
            this.managedObject = sdkData;
        }

        /// <summary>
        /// Gets the Operating System alias.
        /// </summary>
        public string OS
        {
            get
            {
                return this.managedObject.GetPropertyValue("OS");
            }
        }

        /// <summary>
        /// Gets the architecture.
        /// </summary>
        public string Architecture
        {
            get
            {
                return this.managedObject.GetPropertyValue("Arch");
            }
        }

        /// <summary>
        /// Gets the supported agent version including build number.
        /// </summary>
        public UnixAgentVersion AgentVersion
        {
            get
            {
                return new UnixAgentVersion(this.managedObject.GetPropertyValue("Version") + "-" + this.managedObject.GetPropertyValue("Build"));
            }
        }

        /// <summary>
        /// Gets the extension of the kit name. E.g. rpm.
        /// </summary>
        public string KitExtension
        {
            get
            {
                return this.managedObject.GetPropertyValue("Ext");
            }
        }

        /// <summary>
        /// Gets the minimum version of the OS that this kit supports.
        /// </summary>
        public OperatingSystemVersion MinimumSupportedOSVersion
        {
            get
            {
                return new OperatingSystemVersion(this.managedObject.GetPropertyValue("MinVersion"));
            }
        }

        /// <summary>
        /// Gets the architecture name.
        /// </summary>
        public string ArchitectureFriendlyName
        {
            get
            {
                return this.managedObject.GetPropertyValue("ArchFriendly");
            }
        }

        /// <summary>
        /// Gets the architecture name as represented in the kit filename.
        /// </summary>
        public string KitNameArchitecture
        {
            get
            {
                return this.managedObject.GetPropertyValue("ArchKitName");
            }
        }

        /// <summary>
        /// Gets the name of the ManagementPack class that is used to monitor computers supported by this agent.
        /// </summary>
        public string SupportedManagementPackClassName
        {
            get
            {
                return this.managedObject.GetPropertyValue("ComputerType");
            }
        }

        /// <summary>
        /// Gets the OS version string as represented in the kit filename.
        /// </summary>
        public string KitNameOSVersion
        {
            get
            {
                return this.managedObject.GetPropertyValue("KitOSVersion");
            }
        }

        /// <summary>
        /// Gets the OS version string as represented in the Management Pack tasks.
        /// </summary>
        public string TaskVersion
        {
            get
            {
                return this.managedObject.GetPropertyValue("TaskVersion");
            }
        }
    }
}
