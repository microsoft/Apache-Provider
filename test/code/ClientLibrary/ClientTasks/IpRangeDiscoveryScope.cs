// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IpRangeDiscoveryScope.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
//   Defines the IpRangeDiscoveryScope type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Net;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks.Properties;

    public class IpRangeDiscoveryScope : IDiscoveryScope
    {
        [CLSCompliant(false)]
        public IpRangeDiscoveryScope(IPAddress startAddress, IPAddress endAddress, ushort sshPort)
        {
            this.StartAddress = startAddress;
            this.EndAddress = endAddress;

            this.SshPort = sshPort;
        }

        public IPAddress StartAddress { get; set; }

        public IPAddress EndAddress { get; set; }

        private const int MaxHostsThreshold = 1000;

        public bool ValidateIpAddressRange()
        {
            if (this.StartAddress != null && this.EndAddress != null)
            {
                var startAdr = new SCXIpAddress(StartAddress);
                var endAdr = new SCXIpAddress(EndAddress);
                int addrCount = 0;
                for (var curAdr = startAdr; curAdr < endAdr; curAdr++)
                {
                    if (addrCount > MaxHostsThreshold - 1)
                    {
                        return false;
                    }

                    addrCount++;
                }
            }

            return true;
        }

        public IEnumerator<IPHostEntry> GetEnumerator()
        {
            var startAdr = new SCXIpAddress(StartAddress);
            var endAdr = new SCXIpAddress(EndAddress);
            IList<IPHostEntry> retVal = new List<IPHostEntry>();

            if (startAdr > endAdr)
            {
                startAdr = endAdr;
                endAdr = new SCXIpAddress(StartAddress);
            }

            for (var curAdr = startAdr; curAdr < endAdr; curAdr++)
            {
                if (retVal.Count > MaxHostsThreshold - 1)
                {
                    throw new InvalidIpAddressRangeException(Resources.IPRange_TooLarge);
                }

                retVal.Add(new IPHostEntry { HostName = curAdr.GetIpAddress().ToString(), AddressList = new[] { curAdr.GetIpAddress() } });
            }

            retVal.Add(new IPHostEntry { HostName = endAdr.GetIpAddress().ToString(), AddressList = new[] { endAdr.GetIpAddress() } });

            return retVal.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public string SpecificationString
        {
            get
            {
                return string.Format(CultureInfo.CurrentCulture, Resources.IPRange, this.StartAddress, this.EndAddress);
            }

            set
            {
                // TODO: should be using ScopeBuilder here.
                return;
            }
        }

        [CLSCompliant(false)]
        public ushort SshPort { get; set; }

        public object Clone()
        {
            /* Constructor does not prohibit initialization with a null IPAddress. Clone needs to support the same implementation. */
            var newStartIpAddress = this.StartAddress != null ? new IPAddress(this.StartAddress.GetAddressBytes()) : null;
            var newEndIpAddress = this.EndAddress != null ? new IPAddress(this.EndAddress.GetAddressBytes()) : null;

            return new IpRangeDiscoveryScope(newStartIpAddress, newEndIpAddress, this.SshPort);
        }
    }
}