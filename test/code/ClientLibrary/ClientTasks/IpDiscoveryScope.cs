// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IpDiscoveryScope.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
//   Defines the IpDiscoveryScope type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Net;

    public class IpDiscoveryScope : IDiscoveryScope
    {
        private IPAddress ip;

        [CLSCompliant(false)]
        public IpDiscoveryScope(IPAddress ip, ushort sshPort)
        {
            this.ip = ip;
            this.SshPort = sshPort;
        }

        public IEnumerator<IPHostEntry> GetEnumerator()
        {
            var list = new List<IPHostEntry>
                {
                    new IPHostEntry { HostName = null, AddressList = new[] { this.ip }, Aliases = null } 
                };

            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public string SpecificationString
        {
            get
            {
                return this.ip == null ? string.Copy(string.Empty) : this.ip.ToString();
            }

            set
            {
                this.ip = IPAddress.Parse(value);
            }
        }

        [CLSCompliant(false)]
        public ushort SshPort { get; set; }

        public IPAddress IP 
        { 
            get
            {
                return this.ip;
            }

            set
            {
                this.ip = value;
            }
        }

        public object Clone()
        {
            /* Constructor does not prohibit initialization with a null IPAddress. Clone needs to support the same implementation. */
            var newIpAddress = this.ip != null ? new IPAddress(this.ip.GetAddressBytes()) : null;

            return new IpDiscoveryScope(newIpAddress, this.SshPort);
        }
    }
}