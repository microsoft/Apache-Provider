//-----------------------------------------------------------------------
// <copyright file="SingleHostDiscoveryCriteria.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net;

    /// <summary>
    /// This class contains the expanded information required to complete the discovery
    /// process for a single host.
    /// </summary>
    public class SingleHostDiscoveryCriteria
    {
        #region Properties (4) 

        /// <summary>
        /// Gets or sets the resolved name of the host.
        /// </summary>
        public string DNSName { get; set; }

        /// <summary>
        /// Gets or sets the IP address of the host.
        /// </summary>
        public IPAddress IP { get; set; }

        /// <summary>
        /// Gets or sets the port where SSH is listening.
        /// </summary>
        public int SSHPort { get; set; }

        /// <summary>
        ///     Gets the credential set associated with this criteria.
        /// </summary>
        /// <value>The Credential Set owned b this object.</value>
        public CredentialSet CredentialSet
        {
            get
            {
                return m_Credentials;
            }
            set
            {
                if (null != value)
                {
                    m_Credentials = value;
                }
                else
                {
                    m_Credentials.Clear();
                }
            }
        }


        #endregion Properties 

        #region Fields (1) 

        /// <summary>
        ///     Internal holder for credentials associated with this host.
        /// </summary>
        private CredentialSet m_Credentials = new CredentialSet();

        #endregion Fields 
    }
}
