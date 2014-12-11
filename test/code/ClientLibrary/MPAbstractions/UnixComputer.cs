//-----------------------------------------------------------------------
// <copyright file="UnixComputer.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;
    using System.Globalization;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.Utilities;

    /// <summary>
    /// Represents a UnixComputer from the Unix library management pack.
    /// </summary>
    internal class UnixComputer
    {
        /// <summary>
        /// Initializes a new instance of the UnixComputer class.
        /// This instance will represent the subtype of the UnixComputer management
        /// pack class defined in the input parameter.
        /// </summary>
        /// <param name="managedObject">Managed object containing the data for this unix computer.</param>
        public UnixComputer(IManagedObject managedObject)
        {
            this.ManagedObject = managedObject;
        }

        /// <summary>
        /// Gets or sets the name of the computer.
        /// </summary>
        public string Name
        {
            get
            {
                return this.ManagedObject.GetPropertyValue("PrincipalName");
            }

            set
            {
                this.ManagedObject.SetPropertyValue("PrincipalName", value);
                this.ManagedObject.SetPropertyValue("DNSName", value);
                this.ManagedObject.SetPropertyValue("NetworkName", value);
                this.ManagedObject.SetPropertyValue("DisplayName", value);
            }
        }

        /// <summary>
        /// Gets or sets the IP Address of the computer instance.
        /// </summary>
        public string IPAddress
        {
            get
            {
                return this.ManagedObject.GetPropertyValue("IPAddress");
            }

            set
            {
                this.ManagedObject.SetPropertyValue("IPAddress", value);
            }
        }

        /// <summary>
        /// Gets or sets the SSH Port of the computer instance.
        /// </summary>
        public int SSHPort
        {
            get
            {
                return int.Parse(this.ManagedObject.GetPropertyValue("SSHPort"), CultureInfo.InvariantCulture);
            }

            set
            {
                this.ManagedObject.SetPropertyValue("SSHPort", value);
            }
        }

        /// <summary>
        /// Gets or sets the version of the x-plat agent installed on this computer.
        /// </summary>
        public UnixAgentVersion AgentVersion
        {
            get
            {
                string verstring = this.ManagedObject.GetPropertyValue("AgentVersion");
                UnixAgentVersion ver = null;
                if (!(String.IsNullOrWhiteSpace(verstring) || UnixAgentVersion.TryParse(verstring, out ver)))
                {
                    throw new FormatException(Strings.UnixComputer_AgentVersion_Nonparseable_AgentVersion_from_Managed_Object);
                }

                return ver;
            }

            set
            {
                this.ManagedObject.SetPropertyValue("AgentVersion", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets the architecture of this computer instance.
        /// </summary>
        public string Architecture
        {
            get
            {
                return this.ManagedObject.GetPropertyValue("Architecture");
            }

            set
            {
                this.ManagedObject.SetPropertyValue("Architecture", value);
            }
        }

        /// <summary>
        /// Gets or sets the id of this computer instance.
        /// </summary>
        public Guid Id
        {
            get { return this.ManagedObject.Id; }

            set { value = this.ManagedObject.Id; }
        }

        public IManagedObject ManagedObject { get; private set; }
    }
}
