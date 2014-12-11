//-----------------------------------------------------------------------
// <copyright file="IdnSupport.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------



namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Security;

    /// <summary>
    /// enum type for IDN options
    /// </summary>
    public enum IdnOptions
    {        
        /// <summary>
        /// Allow unassigned query behavior per RFC 3454
        /// </summary>
        IdnAllowUnassigned = 0x01,

        /// <summary>
        /// Apply STD3 ASCII restrictions for legal characters
        /// </summary>
        IdnUseStd3AsciiRules = 0x02
    }

    /// <summary>
    /// Helper class to support Internationalized Domain Name (IDN)
    /// </summary>
    public class IdnSupport
    {
        /// <summary>
        /// A wrapper around Uri.CheckHostName, but with IDN support
        /// </summary>
        /// <param name="hostName">A unicode string representing host name</param>
        /// <returns>A UriHostNameType which can be Basic, IPv4, IPv6, Dns, or Unknown</returns>
        public static UriHostNameType ScxCheckHostName(string hostName)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                return UriHostNameType.Unknown;
            }

            UriHostNameType type = Uri.CheckHostName(hostName);
            if (type != UriHostNameType.Unknown)
            {
                // Uri.CheckHostName and IPAddress.Parse work differently for a valid ipv6 address with a trailing '%' character
                // return the Unknown type when CheckHostName succeeds but IPAddress.Parse fails
                IPAddress tmp;
                if ((type == UriHostNameType.IPv6 || type == UriHostNameType.IPv4) && IPAddress.TryParse(hostName, out tmp) == false)
                {
                    return UriHostNameType.Unknown;
                }

                return type;
            }

            string punyCode = IdnToAscii(hostName, IdnOptions.IdnUseStd3AsciiRules);

            if (string.IsNullOrEmpty(punyCode))
            {
                return UriHostNameType.Unknown;
            }

            return Uri.CheckHostName(punyCode);
        }

        /// <summary>
        /// Converts an internationalized domain name (IDN) or another internationalized label to a Unicode (wide character) 
        /// representation of the ASCII string that represents the name in the Punycode transfer encoding syntax. 
        /// </summary>
        /// <param name="unicodeString">the IDN string</param>
        /// <param name="flags">the conversion option</param>
        /// <returns>the Punycode string</returns>
        public static string IdnToAscii(string unicodeString, IdnOptions flags)
        {
            int length = SafeNativeMethods.IdnToAscii((uint)flags, unicodeString, (uint)unicodeString.Length, null, 0);

            if (length != 0)
            {
                string buffer = new string('.', length);

                length = SafeNativeMethods.IdnToAscii((uint)flags, unicodeString, (uint)unicodeString.Length, buffer, (uint)buffer.Length);
                if (length == 0)
                {
                    int    error   = Marshal.GetLastWin32Error();
                    string message = string.Format(
                        "Failed calling managed code IdnToAscii, error {0}: {1}",
                        error,
                        new Win32Exception(error).Message);

                    Debug.Assert(length != 0, message);
                }

                return buffer;
            }

            return string.Empty;
        }

        /// <summary>
        ///     This class resolves the FxCop warning CA1060: Move P/Invokes to
        ///     NativeMethods class.
        /// </summary>
        /// <remarks>
        ///     Platform Invocation methods, such as those that are marked by
        ///     using the System.Runtime.InteropServices.DllImportAttribute
        ///     attribute, or methods that are defined by using the Declare
        ///     keyword in Visual Basic, access unmanaged code. These methods
        ///     should be in one of the following classes: NativeMethods,
        ///     SafeNativeMethods or UnsafeNativeMethods.  This module places
        ///     the P/Invokes method IdnToAscii into SafeNativeMethods because
        ///     the IdnToAscii method does not present an entry point that
        ///     would put the local system at risk.
        /// <para />
        ///     It should be noted that there is reason to use caution with
        ///     IdnToAscii.  However, this has to do with how a user might
        ///     use the resulting Punycode address.  It does not have anything
        ///     to do with local system security or anything that could be
        ///     prevented if the UnsafeNativeMethods class was used instead.
        /// <para />
        ///     For more information on IdnToAscii, checkout the MSDN.
        /// </remarks>
        [SuppressUnmanagedCodeSecurity]
        private static class SafeNativeMethods
        {
            [DllImport(
                "Normaliz.dll",
                EntryPoint            = "IdnToAscii",
                SetLastError          = true,
                CharSet               = CharSet.Unicode,
                ExactSpelling         = true,
                BestFitMapping        = false,
                ThrowOnUnmappableChar = true,
                CallingConvention     = CallingConvention.StdCall)]
            public static extern int IdnToAscii(
                uint                                          dwFlags,
                [In, MarshalAs(UnmanagedType.LPWStr)] string  lpUnicodeCharStr,
                uint                                          cchUnicodeChar,
                [Out, MarshalAs(UnmanagedType.LPWStr)] string lpAsciiCharStr,
                uint                                          cchAsciiChar);
        }
    }
}
