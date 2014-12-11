//-----------------------------------------------------------------------
// <copyright file="DiscoveryTargetEndpoint.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System.Net;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core;

    /// <summary>
    ///     This class contains the expanded information required to complete the discovery
    ///     process for a single host.
    /// </summary>
    public class DiscoveryTargetEndpoint
    {
        #region Lifecycle

        public DiscoveryTargetEndpoint()
        {
            this.HostName = string.Empty;
            this.IP = IPAddress.None;
            this.credentials = new CredentialSet();
            this.IsWSManOnly = false;
        }

        public DiscoveryTargetEndpoint(string host, string ipstr, int port, CredentialSet creds, bool isWsManOnly)
        {
            this.HostName = host;
            this.IP = IPAddress.Parse(ipstr);
            this.SSHPort = port;
            this.CredentialSet = creds;
            this.IsWSManOnly = isWsManOnly;
        }

        #endregion Lifecycle

        #region Properties (4)

        /// <summary>
        /// Gets or sets the resolved name of the host.
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Gets or sets the IP address of the host.
        /// </summary>
        public IPAddress IP { get; set; }

        /// <summary>
        /// Gets or sets the port where SSH is listening.
        /// </summary>
        public int SSHPort { get; set; }

        /// <summary>
        /// Gets or sets rule to probe only WSMan (instead of WSMan and SSH)
        /// </summary>
        public bool IsWSManOnly { get; set; }

        /// <summary>
        ///     Gets the credential set associated with this criteria.
        /// </summary>
        /// <value>The Credential Set related to this object.</value>
        public CredentialSet CredentialSet
        {
            get
            {
                return this.credentials;
            }

            set
            {
                if (null != value)
                {
                    this.credentials = value;
                }
                else
                {
                    this.credentials.Clear();
                }
            }
        }

        #endregion Properties 

        #region Fields (1) 

        /// <summary>
        ///     Internal holder for credentials associated with this host.
        /// </summary>
        private CredentialSet credentials;

        #endregion Fields 
    }
}
