// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HostnameDiscoveryScope.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
//   Defines the HostnameDiscoveryScope type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Net;

    public class HostnameDiscoveryScope : IDiscoveryScope
    {
        private string hostname;

        [CLSCompliant(false)]
        public HostnameDiscoveryScope()
        {
            this.hostname = String.Empty;
            this.SshPort = 22;
        }

        [CLSCompliant(false)]
        public HostnameDiscoveryScope(string hostname, ushort sshPort)
        {
            this.hostname = string.IsNullOrWhiteSpace(hostname) ? hostname : hostname.Trim();
            this.SshPort = sshPort;
        }

        public string SpecificationString
        {
            get
            {
                return this.hostname;
            }

            set
            {
                this.hostname = value;
            }
        }

        [CLSCompliant(false)]
        public ushort SshPort { get; set; }

        public IEnumerator<IPHostEntry> GetEnumerator()
        {
            var list = new List<IPHostEntry>
                {
                    new IPHostEntry { HostName = this.hostname, AddressList = new[] { IPAddress.None }, Aliases = null } 
                };

            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public object Clone()
        {
            if (string.IsNullOrWhiteSpace(this.hostname))
            {
                return new HostnameDiscoveryScope();
            }
            else
            {
                return new HostnameDiscoveryScope(string.Copy(this.hostname), this.SshPort);
            }
        }
    }
}