// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SCXIpAddress.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Sockets;

    /// <summary>
    /// This class is the SCX representation of IP address. It is created for the
    /// purpose of extracting all the IP addresses in a given IP address range, which
    /// could be either IPv4 or IPv6.
    /// </summary>
    public class SCXIpAddress
    {
        /// <summary>
        ///  IP address represented in bytes.
        /// </summary>
        public byte[] AddressBytes
        {
            get { return addrBytes; }
        }

        /// <summary>
        ///  The length of the IP address.
        /// </summary>
        public int AddressLength
        {
            get { return addrLen; }
        }

        private byte[] addrBytes;

        private int addrLen;

        /// <summary>
        ///     Creates an instance from a given IP address.
        /// </summary>
        /// <param name="ipAddress">IPAddress instance.</param>
        public SCXIpAddress(IPAddress ipAddress)
        {
            Debug.Assert(ipAddress.AddressFamily == AddressFamily.InterNetwork || ipAddress.AddressFamily == AddressFamily.InterNetworkV6, "only IPv4 or IPv6 address is allowed");
            addrBytes = ipAddress.GetAddressBytes();

            addrLen = ipAddress.AddressFamily == AddressFamily.InterNetwork ? 4 : 16;
        }

        /// <summary>
        ///     Creates an instance from a given SCXIpAddress instance.
        /// </summary>
        /// <param name="copy">The SCXIpAddress object.</param>
        public SCXIpAddress(SCXIpAddress copy)
        {
            addrLen = copy.addrLen;
            addrBytes = new byte[addrLen];

            for (int i = 0; i < addrLen; ++i)
            {
                addrBytes[i] = copy.addrBytes[i];
            }
        }

        /// <summary>
        ///     Implict coverter to regular IPAddress class.
        /// </summary>
        /// <param name="addr">The SCXIpAddress object</param>
        /// <returns>An IPAddress object</returns>
        public static implicit operator IPAddress(SCXIpAddress addr)
        {
            return new IPAddress(addr.addrBytes);
        }

        /// <summary>
        ///     Compare two SCXIpAddress instances by their IP addresses.
        /// </summary>
        /// <param name="addr1">The first SCXIpAddress object.</param>
        /// <param name="addr2">The second SCXIpAddress object.</param>
        /// <returns>True if the first address is less than the second one.</returns>
        public static bool operator <(SCXIpAddress addr1, SCXIpAddress addr2)
        {
            Debug.Assert(addr1.addrLen == addr2.addrLen, "Only the addresses with the same type is allowed.");

            for (int i = 0; i < addr1.addrLen; ++i)
            {
                if (addr1.addrBytes[i] < addr2.addrBytes[i])
                {
                    return true;
                }

                if (addr1.addrBytes[i] > addr2.addrBytes[i])
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        ///     Compare two SCXIpAddress instances by their IP addresses.
        /// </summary>
        /// <param name="addr1">The first SCXIpAddress object</param>
        /// <param name="addr2">The second SCXIpAddress object</param>
        /// <returns>True if the first address is larger than the second one.</returns>
        public static bool operator >(SCXIpAddress addr1, SCXIpAddress addr2)
        {
            Debug.Assert(addr1.addrLen == addr2.addrLen, "Only two addresses with the same type is allowed.");

            for (int i = 0; i < addr1.addrLen; ++i)
            {
                if (addr1.addrBytes[i] > addr2.addrBytes[i])
                {
                    return true;
                }

                if (addr1.addrBytes[i] < addr2.addrBytes[i])
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        ///    Overload ++ operator.
        /// </summary>
        /// <param name="addr">SCXIpAddress object</param>
        /// <returns>SCXIpAddress object with address value added by one.</returns>
        public static SCXIpAddress operator ++(SCXIpAddress addr)
        {
            for (int i = addr.addrLen - 1; i >= 0; --i)
            {
                if (addr.addrBytes[i] < 0xff)
                {
                    addr.addrBytes[i]++;
                    break;
                }

                addr.addrBytes[i] = 0;
            }

            return addr;
        }

        

        /// <summary>
        ///    Get IPAddress instance.
        /// </summary>
        /// <returns>IPAddress object represented by this SCXIpAddress instance.</returns>
        public IPAddress GetIpAddress()
        {
            return new IPAddress(this.addrBytes); 
        }
    }
}
