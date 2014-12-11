//-----------------------------------------------------------------------
// <copyright file="OMInfo.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>a-brdust</author>
// <description></description>
// <history>3/25/2009 11:20:00 AM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Apache.SDK.ApacheSDKHelper
{
    using System;
    using System.Collections.ObjectModel;
    using System.Net;
    using System.Security;
    using Microsoft.EnterpriseManagement;
    using Microsoft.EnterpriseManagement.Administration;

    /// <summary>
    /// Container class for data about an Operations Manager installation
    /// </summary>
    public class OMInfo
    {
        #region Private Fields

        /// <summary>
        /// Operations Manager Hostname (FQDN)
        /// </summary>
        private string omServer = null;

        /// <summary>
        /// Operations Manager User Name
        /// </summary>
        private string omUserName = null;

        /// <summary>
        /// Operations Manager Domain (NT4 Domain, e.g. SCX)
        /// </summary>
        private string omDomain = null;

        /// <summary>
        /// Operations Manager Password
        /// </summary>
        private string omPassword = null;

        /// <summary>
        /// Default resource pool name;
        /// </summary>
        private string omDefaultResourcePool = null;

        /// <summary>
        /// Store the local management group
        /// </summary>
        private ManagementGroup managementGroup;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the OMInfo class
        /// </summary>
        /// <param name="omServer">Operations Manager Hostname</param>
        /// <param name="omUserName">Operations Manager User Name</param>
        /// <param name="omDomain">Operations Manager Domain</param>
        /// <param name="omPassword">Operations Manager Password</param>
        public OMInfo(string omServer, string omUserName, string omDomain, string omPassword)
        {
            this.omUserName = omUserName;
            this.omDomain = omDomain;
            this.omPassword = omPassword;

            if (string.IsNullOrEmpty(omServer))
            {
                this.omServer = Dns.GetHostEntry(Dns.GetHostName()).HostName;
            }
            else
            {
                this.omServer = omServer;
            }

            ManagementGroupConnectionSettings connectionSettings = 
                new ManagementGroupConnectionSettings(this.omServer);

            SecureString securePassword = new SecureString();
            char[] p = omPassword.ToCharArray(0, omPassword.Length);
            foreach (char c in p)
            {
                securePassword.AppendChar(c);
            }

            connectionSettings.UserName = this.omUserName;
            connectionSettings.Domain = this.omDomain;
            connectionSettings.Password = securePassword;

            this.managementGroup = ManagementGroup.Connect(connectionSettings);

            if (!this.managementGroup.IsConnected)
            {
                throw new InvalidOperationException("Not connected to an SDK Service.");
            }
        }

        /// <summary>
        /// Initializes a new instance of the OMInfo class
        /// </summary>
        /// <param name="omServer">The name of the OM server</param>
        /// <param name="omUserName">The account name</param>
        /// <param name="omDomain">The name of the OM domain</param>
        /// <param name="omPassword">The password of the account</param>
        /// <param name="omDefaultResourcePool">OM default resource pool</param>
        public OMInfo(string omServer, string omUserName, string omDomain, string omPassword, string omDefaultResourcePool)
            : this(omServer, omUserName, omDomain, omPassword)
        {
            this.omDefaultResourcePool = omDefaultResourcePool;
        }

        #endregion Constructors

        #region Accessors

        /// <summary>
        /// Gets or sets Operations Manager Hostname
        /// </summary>
        public string OMServer
        {
            get { return this.omServer; }
            set { this.omServer = value; }
        }

        /// <summary>
        /// Gets or sets Operations Manager User Name
        /// </summary>
        public string OMUserName
        {
            get { return this.omUserName; }
            set { this.omUserName = value; }
        }

        /// <summary>
        /// Gets or sets Operations Manager Domain
        /// </summary>
        public string OMDomain
        {
            get { return this.omDomain; }
            set { this.omDomain = value; }   
        }

        /// <summary>
        /// Gets or sets Operations Manager Password
        /// </summary>
        public string OMPassword
        {
            get { return this.omPassword; }
            set { this.omPassword = value; }
        }

        /// <summary>
        /// Gets or sets Operations Manager Password
        /// </summary>
        public string OMDefaultResourcePool
        {
            get { return this.omDefaultResourcePool; }
            set { this.omDefaultResourcePool = value; }
        }

        /// <summary>
        /// Gets the server simple host name (excluding domain).
        /// </summary>
        public string OMSimpleHostName
        {
            get
            {
                if (this.omServer.Contains("."))
                {
                    return this.omServer.Substring(0, this.omServer.IndexOf('.'));
                }
                else
                {
                    return this.omServer;
                }
            }
        }

        /// <summary>
        /// Gets or sets the local management group
        /// </summary>
        public ManagementGroup LocalManagementGroup
        {
            get { return this.managementGroup; }
            set { this.managementGroup = value; }
        }

        #endregion

        #region Public Methods

        #region Methods

        /// <summary>
        /// Returns a multi-line string displaying the OMInfo fields.
        /// </summary>
        /// <returns>A multi-line string displaying the OMInfo fields.</returns>
        public override string ToString()
        {
            if (String.IsNullOrEmpty(this.omDefaultResourcePool))
            {
                return string.Concat(
                    "OM ServerName       = " + this.omServer + Environment.NewLine,
                    "OM UserName         = " + this.omUserName + Environment.NewLine,
                    "OM Domain           = " + this.omDomain + Environment.NewLine,
                    "OM Password         = " + this.omPassword + Environment.NewLine,
                    "OM Management Group = " + this.managementGroup + Environment.NewLine);
            }
            else
            {
                return string.Concat(
                   "OM ServerName          = " + this.omServer + Environment.NewLine,
                   "OM UserName            = " + this.omUserName + Environment.NewLine,
                   "OM Domain              = " + this.omDomain + Environment.NewLine,
                   "OM Password            = " + this.omPassword + Environment.NewLine,
                   "OM Management Group    = " + this.managementGroup + Environment.NewLine,
                   "OM DefaultResourcePool = " + this.omDefaultResourcePool + Environment.NewLine);
            }
        }        

        #endregion Public Methods

        #region Private Methods

        #endregion Private Methods

        #endregion Methods
    }
}
