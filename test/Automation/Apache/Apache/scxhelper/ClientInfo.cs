//-----------------------------------------------------------------------
// <copyright file="ClientInfo.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>a-brdust</author>
// <description></description>
// <history>3/30/2009 1:18:15 PM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Common
{
    using System;
    using System.Net;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Contains information about the client machine used
    /// during discovery by Operations Manager.
    /// </summary>
    public class ClientInfo
    {
        #region Private Fields

        /// <summary>
        /// Generic logger. This is the ConsoleLogger by default.
        /// </summary>
        private ILogger genericLogger = new ConsoleLogger();

        /// <summary>
        /// The hostname of the client machine being discovered.
        /// </summary>
        private string hostname;

        /// <summary>
        /// The IP Address of the client machine being discovered.
        /// </summary>
        private string ipaddr;

        /// <summary>
        /// The computer class of the client machine being discovered.
        /// </summary>
        private string targetComputerClass;

        /// <summary>
        /// Name of the management pack, for example Microsoft.Linux.SLES.10
        /// </summary>
        private string managementPackName;

        /// <summary>
        /// CPU Architecture of the client machine.
        /// </summary>
        private string architecture;

        /// <summary>
        /// user name (usually 'scxuser')
        /// </summary>
        private string user;

        /// <summary>
        /// user password (usually 'scxuser')
        /// </summary>
        private string userPassword;

        /// <summary>
        /// Superuser name (usually 'root')
        /// </summary>
        private string superUser;

        /// <summary>
        /// Superuser password
        /// </summary>
        private string superUserPassword;

        /// <summary>
        /// Package name used by the install command
        /// </summary>
        private string packageName;

        /// <summary>
        /// POSIX command used to clean the agent from the remote client.
        /// </summary>
        private string cleanupCommand;

        /// <summary>
        /// Platform tag for the remote client.
        /// </summary>
        private string platformTag;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ClientInfo class.
        /// </summary>
        /// <param name="hostname">Hostname of the client machine.</param>
        /// <param name="ipaddr">IP Address of the client machine.</param>
        /// <param name="targetComputerClass">Computer Class of the client machine.</param>
        /// <param name="managementPackName">Name of the management pack, for example Microsoft.Linux.SLES.10</param>
        /// <param name="architecture">CPU Architecture of the client machine.</param>
        /// <param name="user">user account on the client machine (usually 'scxuser')</param>
        /// <param name="userPassword">user password on the client machine (usually 'scxuser')</param>
        /// <param name="superUser">Superuser account on the client machine (usually 'root')</param>
        /// <param name="superUserPassword">Superuser password on the client machine</param>
        /// <param name="packageName">Package name used by the install command</param>
        /// <param name="cleanupCommand">Command on the client machine to remove the scx agent and clean up the system</param>
        /// <param name="platformTag">Platform tag</param>
        public ClientInfo(
            string hostname,
            string ipaddr,
            string targetComputerClass,
            string managementPackName,
            string architecture,
            string user,
            string userPassword,
            string superUser,
            string superUserPassword,
            string packageName,
            string cleanupCommand,
            string platformTag)
        {
            this.hostname = hostname;
            this.ipaddr = ipaddr;
            this.targetComputerClass = targetComputerClass;
            this.managementPackName = managementPackName;
            this.architecture = architecture;
            this.user = user;
            this.userPassword = userPassword;
            this.superUser = superUser;
            this.superUserPassword = superUserPassword;
            this.packageName = packageName;
            this.cleanupCommand = cleanupCommand;
            this.platformTag = platformTag;

            if (string.IsNullOrEmpty(this.ipaddr))
            {
                this.ipaddr = Dns.GetHostEntry(this.hostname).AddressList[0].ToString();
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the ClientInfo class as a copy of another clientInfo instance
        /// </summary>
        /// <param name="copyFrom">ClientInfo object to copy from</param>
        public ClientInfo(ClientInfo copyFrom)
        {
            this.hostname = copyFrom.hostname;
            this.ipaddr = copyFrom.ipaddr;
            this.targetComputerClass = copyFrom.targetComputerClass;
            this.managementPackName = copyFrom.managementPackName;
            this.architecture = copyFrom.architecture;
            this.user = copyFrom.user;
            this.userPassword = copyFrom.userPassword;
            this.superUser = copyFrom.superUser;
            this.superUserPassword = copyFrom.superUserPassword;
            this.packageName = copyFrom.packageName;
            this.cleanupCommand = copyFrom.cleanupCommand;
            this.platformTag = copyFrom.platformTag;
        }

        /// <summary>
        /// Initializes a new instance of the ClientInfo class.
        /// </summary>
        public ClientInfo()
        {
        }

        #endregion Constructors

        #region enums

        /// <summary>
        /// IPVersion enums
        /// </summary>
        private enum IPVersion
        {
            /// <summary>
            /// IpVersion is IPv4.
            /// </summary>
            IPv4,

            /// <summary>
            /// IpVersion is IPv6.
            /// </summary>
            IPv6
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the client Host Name.
        /// </summary>
        public string HostName
        {
            get 
            { 
                return this.hostname; 
            }

            set 
            { 
                this.hostname = value;
                this.ipaddr = Dns.GetHostEntry(this.hostname).AddressList[0].ToString();
            }
        }

        /// <summary>
        /// Gets or sets the client IP Address.
        /// </summary>
        public string IPAddr
        {
            get { return this.ipaddr; }
            set { this.ipaddr = value; }
        }

        /// <summary>
        /// Gets or sets the client Computer Class
        /// </summary>
        public string TargetComputerClass
        {
            get { return this.targetComputerClass; }
            set { this.targetComputerClass = value; }
        }

        /// <summary>
        /// Gets or sets the client Management Pack Name (for example, Microsoft.Linux.SLES.10)
        /// </summary>
        public string ManagementPackName
        {
            get { return this.managementPackName; }
            set { this.managementPackName = value; }
        }

        /// <summary>
        /// Gets or sets the client CPU architecture.
        /// </summary>
        public string Architecture
        {
            get { return this.architecture; }
            set { this.architecture = value; }
        }

        /// <summary>
        /// Gets or sets the client Unprivileged User name.
        /// </summary>
        public string User
        {
            get { return this.user; }
            set { this.user = value; }
        }

        /// <summary>
        /// Gets or sets the client Unprivileged User password.
        /// </summary>
        public string UserPassword
        {
            get { return this.userPassword; }
            set { this.userPassword = value; }
        }

        /// <summary>
        /// Gets or sets the client Super User name.
        /// </summary>
        public string SuperUser
        {
            get { return this.superUser; }
            set { this.superUser = value; }
        }

        /// <summary>
        /// Gets or sets the client Super User password.
        /// </summary>
        public string SuperUserPassword
        {
            get { return this.superUserPassword; }
            set { this.superUserPassword = value; }
        }

        /// <summary>
        /// Gets or sets the package name used by the install command
        /// </summary>
        public string PackageName
        {
            get { return this.packageName; }
            set { this.packageName = value; }
        }

        /// <summary>
        /// Gets or sets the POSIX command used to to clean the agent from the remote client.
        /// </summary>
        public string CleanupCommand
        {
            get { return this.cleanupCommand; }
            set { this.cleanupCommand = value; }
        }

        /// <summary>
        /// Gets or sets the platform tag for the remote client.
        /// </summary>
        public string PlatformTag
        {
            get { return this.platformTag; }
            set { this.platformTag = value; }
        }

        /// <summary>
        /// Gets the client simple host name (excluding domain).
        /// </summary>
        public string SimpleHostName
        {
            get
            {
                if (this.hostname.Contains("."))
                {
                    return this.hostname.Substring(0, this.hostname.IndexOf('.'));
                }
                else
                {
                    return this.hostname;
                }
            }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns a multi-line string displaying the ClientInfo fields.
        /// </summary>
        /// <returns>A multi-line string displaying the ClientInfo fields.</returns>
        public override string ToString()
        {
            return string.Concat(
                "hostname = " + this.hostname + Environment.NewLine,
                "ipaddr = " + this.ipaddr + Environment.NewLine,
                "targetComputerClass = " + this.targetComputerClass + Environment.NewLine,
                "managementPackName = " + this.managementPackName + Environment.NewLine,
                "architecture = " + this.architecture + Environment.NewLine,
                "user = " + this.user + Environment.NewLine,
                "userPassword = " + this.userPassword + Environment.NewLine,
                "superUser = " + this.superUser + Environment.NewLine,
                "superUserPassword = " + this.superUserPassword + Environment.NewLine,
                "packageName = " + this.packageName + Environment.NewLine,
                "cleanupCommand = " + this.cleanupCommand + Environment.NewLine,
                "platformTag = " + this.platformTag + Environment.NewLine);
        }

        /// <summary>
        /// Get the IPv4 Address of the hosts with the given host names
        /// </summary>
        /// <param name="hostnames">Names of host to resolve</param>
        /// <returns>string IPv4 address of host, seperated by Comma.</returns>
        public string GetHostsIPv4Address(string[] hostnames)
        {
            if (hostnames == null)
            {
                throw new ArgumentNullException("hostnames");
            }

            string[] iPV4Addresses = new string[hostnames.Length];
            for (int index = 0; index < hostnames.Length; index++)
            {
                iPV4Addresses[index] = this.GetHostIPv4Address(hostnames[index]);
            }

            return string.Join(",", iPV4Addresses);
        }

        /// <summary>
        /// Get the IPv4 Address of the host with the given host name 
        /// </summary>
        /// <param name="hostname">Name of host to resolve</param>
        /// <returns>string IPv4 address of host</returns>
        public string GetHostIPv4Address(string hostname)
        {
            return this.GetHostIPAddress(hostname, IPVersion.IPv4);
        }

        /// <summary>
        /// Get the IPv6 Address of the hosts with the given host names
        /// </summary>
        /// <param name="hostnames">Names of host to resolve</param>
        /// <returns>string IPv6 address of host, seperated by Comma.</returns>
        public string GetHostsIPv6Address(string[] hostnames)
        {
            if (hostnames == null)
            {
                throw new ArgumentNullException("hostnames");
            }

            string[] iPV6Addresses = new string[hostnames.Length];
            for (int index = 0; index < hostnames.Length; index++)
            {
                iPV6Addresses[index] = this.GetHostIPv4Address(hostnames[index]);
            }

            return string.Join(",", iPV6Addresses);
        }

        /// <summary>
        /// Get the IPv6 Address of the host with the given host name 
        /// </summary>
        /// <param name="hostname">Name of host to resolve</param>
        /// <returns>string IPv6 address of host</returns>
        public string GetHostIPv6Address(string hostname)
        {
            return this.GetHostIPAddress(hostname, IPVersion.IPv6);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Get the IPv4 Address of the host with the given host name.
        /// </summary>
        /// <param name="hostname">Name of host to resolve</param>
        /// <param name="ipVersion">string IPv4 address of host</param>
        /// <returns>Returns ip address</returns>
        private string GetHostIPAddress(string hostname, IPVersion ipVersion)
        {
            // find correct type of IP Address inside IPAddress array
            string ipMatchString = null;
            switch (ipVersion)
            {
                case IPVersion.IPv4:
                    ipMatchString = @"^\d+\.\d+\.\d+\.\d+$";
                    break;
                case IPVersion.IPv6:
                    ipMatchString = @"^[0-9a-fA-F]+(?::[0-9a-fA-F]+){7}$";
                    break;
            }

            this.genericLogger.Write("Resolving {0} IP address for host {1}", ipVersion.ToString(), hostname);

            try
            {
                System.Net.IPAddress[] ipAddrs = System.Net.Dns.GetHostAddresses(hostname);

                foreach (System.Net.IPAddress ip in ipAddrs)
                {
                    if (Regex.Match(ip.ToString(), ipMatchString, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase).Success)
                    {
                        return ip.ToString();
                    }
                }

                throw new Exception(string.Format("No valid {0} Address resolved for host '{1}'", ipVersion.ToString(), hostname));
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Exception getting {0} address for host '{1}'", ipVersion.ToString(), hostname), e);
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}
