// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DiscoveryScopeBuilder.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System;
    using System.Globalization;
    using System.Net;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks.Properties;

    /// <summary>
    ///  This class is used for building IDiscoveryScope instances.
    ///  A scope can either be a a single hostname, a single IP address or a range of IP addresses.
    ///  Using this builder, the scope can be built using several modes of interaction. Either by
    ///  entering a free form string in the Address property, or by using one of the Hostname, IP
    ///  or Range properties. All methods will affect the same data. Once the user is happy with the scope,
    ///  it can be built into an appropriate IDiscoveryScope.
    /// </summary>
    public class DiscoveryScopeBuilder
    {
        [CLSCompliant(false)]
        public const ushort K_DEFAULT_SSH_PORT = 22;

        #region Lifecycle

        /// <summary>
        ///     Creates an empty DiscoveryScopeBuilder.
        /// </summary>
        public DiscoveryScopeBuilder()
        {
            this.SshPort = K_DEFAULT_SSH_PORT;
            this.firstAddress = string.Empty;
            this.secondAddress = string.Empty;
            this.Range = new IPRangeDiscoveryScopeFactory(this);
            this.IP = new IpDiscoveryScopeFactory(this);
            this.Hostname = new HostnameDiscoveryScopeFactory(this);
            this.discoveryScopeFactoryImpl = this.Hostname;
        }

        /// <summary>
        ///     Creates a scopeBuilder from a hostOrRange string.
        /// </summary>
        /// <param name="hostOrRange">
        ///     This can be either the name of the host, the IP of the host or two IPs separated by a dash indicating a range.
        /// </param>
        /// <example>
        ///     <para>"myhost.contoso.com"</para>
        ///     <para>"webserver"</para>
        ///     <para>"192.168.0.1"</para>
        ///     <para>"192.168.0.1-192.168.0.10"</para>
        /// </example>
        public DiscoveryScopeBuilder(string hostOrRange)
            : this()
        {
            this.Address = string.IsNullOrWhiteSpace(hostOrRange) ? hostOrRange : hostOrRange.Trim();
        }

        /// <summary>
        /// Creates a ScopeBuilder from an existing IDiscoveryScope.
        /// </summary>
        /// <param name="scope">DiscoveryScope to populate builder from.</param>
        [CLSCompliant(false)]
        public DiscoveryScopeBuilder(IDiscoveryScope scope) : this(scope.SpecificationString)
        {
        }

        #endregion Lifecycle

        #region Properties

        [CLSCompliant(false)]
        public ushort SshPort
        {
            get
            {
                return this.sshPort;
            }

            set
            {
                if (0 == value)
                {
                    throw new ArgumentOutOfRangeException("value", value, Resources.SSH_Port_Limitations);
                }

                this.sshPort = value;
            }
        }

        /// <summary>
        /// Gets or sets the current scope address. This can be the name of the host, the IP of the host
        /// or the two IPs of a range separated by a dash. When set, the code will try to deduct what type of
        /// scope this is (by parsing) but if it cannot, it will be treted as a Hostname (albeit an invalid one).
        /// </summary>
        /// <example>
        ///     <para>"myhost.contoso.com"</para>
        ///     <para>"webserver"</para>
        ///     <para>"192.168.0.1"</para>
        ///     <para>"192.168.0.1-192.168.0.10"</para>
        /// </example>
        public string Address
        {
            get
            {
                return this.discoveryScopeFactoryImpl.ToString();
            }

            set
            {
                this.firstAddress = string.Empty;
                this.secondAddress = string.Empty;
                this.Range = new IPRangeDiscoveryScopeFactory(this);
                this.IP = new IpDiscoveryScopeFactory(this);
                this.Hostname = new HostnameDiscoveryScopeFactory(this);
                this.discoveryScopeFactoryImpl = this.Hostname;

                var trimmedVal = string.IsNullOrWhiteSpace(value) ? value : value.Trim();
                if (!this.Range.SetIfValid(trimmedVal))
                {
                    if (!this.IP.SetIfValid(trimmedVal))
                    {
                        // We will default to hostname wether it's valid or not.
                        this.Hostname.Hostname = trimmedVal;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the scope data in a form where it can be edited as an IP range.
        /// </summary>
        public IPRangeDiscoveryScopeFactory Range { get; private set; }

        /// <summary>
        /// Gets the scope data in a form where it can be edited as an IP address.
        /// </summary>
        public IpDiscoveryScopeFactory IP { get; private set; }

        /// <summary>
        /// Gets the scope data in a form where it can be edited as a hostname (or FQDN).
        /// </summary>
        public HostnameDiscoveryScopeFactory Hostname { get; private set; }

        /// <summary>
        /// If the scope data is invalid, it gets the reason why. Else it returns the empty string.
        /// </summary>
        public string ValidationError
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Address))
                {
                    return Resources.ScopeValidation_AddressIsEmpty;
                }
               
                var isSingleIPAddress = !IP.IsIpAddressRange;
                if (string.IsNullOrWhiteSpace(this.Hostname.ValidationError) ||
                    (isSingleIPAddress && string.IsNullOrWhiteSpace(this.IP.ValidationError)) ||
                    ((!isSingleIPAddress) && string.IsNullOrWhiteSpace(this.Range.IpAddressValidationError)))
                {
                    return string.Empty;
                }

                return ((!isSingleIPAddress) && this.Range.IpRangeTooLargeError) ? Resources.IPRange_TooLarge : Resources.ScopeValidation_AddressIsInvalid;
            }
        }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Creates the actual scope implementation.
        /// </summary>
        /// <returns>A new IDiscoveryScope implementation.</returns>
        [CLSCompliant(false)]
        public IDiscoveryScope CreateScope()
        {
            return this.discoveryScopeFactoryImpl.CreateScope();
        }

        #endregion Public Methods

        #region Fields

        private ushort sshPort;

        private string firstAddress;

        private string secondAddress;

        /// <summary>
        /// This will point to the scope builder that is the best guess as to what type of scope the user wants to create.
        /// </summary>
        private IDiscoveryScopeFactory discoveryScopeFactoryImpl;

        #endregion Fields

        #region Classes and Interfaces

        private interface IDiscoveryScopeFactory
        {
            IDiscoveryScope CreateScope();
        }

        /// <summary>
        /// Presents a Hostname centric view of the scope data.
        /// </summary>
        public class HostnameDiscoveryScopeFactory : IDiscoveryScopeFactory
        {
            private DiscoveryScopeBuilder scopeBuilder;

            public HostnameDiscoveryScopeFactory(DiscoveryScopeBuilder scopeBuilder)
            {
                this.scopeBuilder = scopeBuilder;
            }

            /// <summary>
            /// When this property is set, it is assumed that the user is trying to create a hostname scope
            /// so the current scopebuilder is set to "this".
            /// </summary>
            public string Hostname
            {
                get
                {
                    return this.scopeBuilder.firstAddress;
                }

                set
                {
                    this.scopeBuilder.firstAddress = value;
                    this.scopeBuilder.discoveryScopeFactoryImpl = this;
                }
            }

            public bool TryParse(string address)
            {
                this.Hostname = address;
                return true;
            }

            [CLSCompliant(false)]
            public IDiscoveryScope CreateScope()
            {
                if (!string.IsNullOrWhiteSpace(this.ValidationError))
                {
                    throw new InvalidScopeSpecificationException(this.ValidationError);
                }

                return new HostnameDiscoveryScope(this.Hostname, this.scopeBuilder.sshPort);
            }

            /// <summary>
            /// Gets the reason why this is not a valid hostname, or the empty string if it is a valid hostname.
            /// </summary>
            public string ValidationError
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(this.Hostname))
                    {
                        return Resources.ScopeValidation_HostnameIsEmpty;
                    }

                    if (IdnSupport.ScxCheckHostName(this.Hostname) != UriHostNameType.Dns)
                    {
                        return Resources.ScopeValidation_HostnameIsInvalid;
                    }

                    return string.Empty;
                }
            }

            public override string ToString()
            {
                return this.Hostname;
            }
        }

        /// <summary>
        /// Presents an IP centric view of the scope data.
        /// </summary>
        public class IpDiscoveryScopeFactory : IDiscoveryScopeFactory
        {
            private readonly DiscoveryScopeBuilder scopeBuilder;

            public IpDiscoveryScopeFactory(DiscoveryScopeBuilder scopeBuilder)
            {
                this.scopeBuilder = scopeBuilder;
            }

            /// <summary>
            /// Indicate if the current address is an address range.
            /// </summary>
            public bool IsIpAddressRange
            {
                get
                {
                    return
                        !(string.IsNullOrWhiteSpace(this.scopeBuilder.firstAddress) ||
                          string.IsNullOrWhiteSpace(this.scopeBuilder.secondAddress));
                }
            }

            /// <summary>
            /// When this property is set, it is assumed that the user is trying to create an IP scope
            /// so the current scopebuilder is set to "this".
            /// </summary>
            public string Address
            {
                get
                {
                    return this.scopeBuilder.firstAddress;
                }

                set
                {
                    this.scopeBuilder.firstAddress = value;
                    this.scopeBuilder.discoveryScopeFactoryImpl = this;
                }
            }

            public override string ToString()
            {
                return this.Address;
            }

            public bool SetIfValid(string address)
            {
                if (IdnSupport.ScxCheckHostName(address) != UriHostNameType.IPv4 &&
                    IdnSupport.ScxCheckHostName(address) != UriHostNameType.IPv6)
                {
                    return false;
                }
                
                this.Address = address;
                return true;
            }

            [CLSCompliant(false)]
            public IDiscoveryScope CreateScope()
            {
                if (!string.IsNullOrWhiteSpace(this.ValidationError))
                {
                    throw new InvalidScopeSpecificationException(this.ValidationError);
                }

                return new IpDiscoveryScope(IPAddress.Parse(this.Address), this.scopeBuilder.sshPort);
            }

            /// <summary>
            /// Gets the reason why this is not a valid IP address or the empty string if it is.
            /// </summary>
            public string ValidationError
            {
                get
                {
                    var address = string.IsNullOrWhiteSpace(this.Address) ? this.Address : this.Address.Trim();
                    if (string.IsNullOrWhiteSpace(address))
                    {
                        return Resources.ScopeValidation_IPIsEmpty;
                    }

                    if (IdnSupport.ScxCheckHostName(address) != UriHostNameType.IPv4 &&
                        IdnSupport.ScxCheckHostName(address) != UriHostNameType.IPv6)
                    {
                        return Resources.ScopeValidation_IPIsInvalid;
                    }

                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Presents an IP-range centric view of the scope data.
        /// </summary>
        public class IPRangeDiscoveryScopeFactory : IDiscoveryScopeFactory
        {
            private readonly DiscoveryScopeBuilder scopeBuilder;

            public IPRangeDiscoveryScopeFactory(DiscoveryScopeBuilder scopeBuilder)
            {
                this.scopeBuilder = scopeBuilder;
            }

            /// <summary>
            /// When this property is set, it is assumed that the user is trying to create a range scope
            /// so the current scopebuilder is set to "this".
            /// </summary>
            public string StartAddress
            {
                get
                {
                    return this.scopeBuilder.firstAddress;
                }

                set
                {
                    this.scopeBuilder.firstAddress = value;
                    this.scopeBuilder.discoveryScopeFactoryImpl = this;
                }
            }

            /// <summary>
            /// When this property is set, it is assumed that the user is trying to create a range scope
            /// so the current scopebuilder is set to "this".
            /// </summary>
            public string EndAddress
            {
                get
                {
                    return this.scopeBuilder.secondAddress;
                }

                set
                {
                    this.scopeBuilder.secondAddress = value;
                    this.scopeBuilder.discoveryScopeFactoryImpl = this;
                }
            }

            public override string ToString()
            {
                return string.Format(CultureInfo.CurrentCulture, Resources.IPRange, this.StartAddress, this.EndAddress);
            }

            public bool SetIfValid(string address)
            {
                var rangeParts = address.Split(new[] { '-', '\u2013', '\u2014' });
                if (2 != rangeParts.Length)
                {
                    return false;
                }

                string first = rangeParts[0].Trim();
                string last = rangeParts[1].Trim();

                if (IdnSupport.ScxCheckHostName(first) == UriHostNameType.IPv4 ||
                    IdnSupport.ScxCheckHostName(first) == UriHostNameType.IPv6)
                {
                    this.StartAddress = first;
                }
                else
                {
                    return false;
                }

                if ((IdnSupport.ScxCheckHostName(last) == UriHostNameType.IPv4 && IdnSupport.ScxCheckHostName(first) == UriHostNameType.IPv4) ||
                    (IdnSupport.ScxCheckHostName(last) == UriHostNameType.IPv6 && IdnSupport.ScxCheckHostName(first) == UriHostNameType.IPv6) ||
                    last.Equals(string.Empty))
                {
                    if (last.Equals(string.Empty) == false)
                    {
                        this.EndAddress = last;
                    }

                    return true;
                }

                return false;
            }

            [CLSCompliant(false)]
            public IDiscoveryScope CreateScope()
            {
                if (!string.IsNullOrWhiteSpace(this.StartAddressValidationError))
                {
                    throw new InvalidScopeSpecificationException(this.StartAddressValidationError);
                }

                if (!string.IsNullOrWhiteSpace(this.EndAddressValidationError))
                {
                    throw new InvalidScopeSpecificationException(this.EndAddressValidationError);
                }

                return new IpRangeDiscoveryScope(IPAddress.Parse(this.StartAddress), IPAddress.Parse(this.EndAddress), this.scopeBuilder.sshPort);
            }

            /// <summary>
            /// Gets the reason why the start address is not a valid IP address or the empty string if it is.
            /// </summary>
            public string StartAddressValidationError
            {
                get
                {
                    var startAddress = string.IsNullOrWhiteSpace(this.StartAddress)
                                           ? this.StartAddress
                                           : this.StartAddress.Trim();
                    var endAddress = string.IsNullOrWhiteSpace(this.EndAddress)
                                           ? this.EndAddress
                                           : this.EndAddress.Trim();

                    if (string.IsNullOrWhiteSpace(startAddress))
                    {
                        return Resources.ScopeValidation_RangeStartIsEmpty;
                    }

                    if (IdnSupport.ScxCheckHostName(startAddress) != UriHostNameType.IPv4 &&
                        IdnSupport.ScxCheckHostName(startAddress) != UriHostNameType.IPv6)
                    {
                        return Resources.ScopeValidation_RangeStartIsInvalid;
                    }
                    else if ((IdnSupport.ScxCheckHostName(startAddress) == UriHostNameType.IPv6 &&
                              IdnSupport.ScxCheckHostName(endAddress) == UriHostNameType.IPv4) ||
                             (IdnSupport.ScxCheckHostName(startAddress) == UriHostNameType.IPv4 &&
                              IdnSupport.ScxCheckHostName(endAddress) == UriHostNameType.IPv6))
                    {
                        return Resources.ScopeValidation_RangeValuesInconsistent;
                    }

                    return string.Empty;
                }
            }

            /// <summary>
            /// Check if IP addresses are valid.
            /// </summary>
            public string IpAddressValidationError
            {
                get
                {
                    var startAddress = string.IsNullOrWhiteSpace(this.StartAddress)
                                          ? this.StartAddress
                                          : this.StartAddress.Trim();
                    var endAddress = string.IsNullOrWhiteSpace(this.EndAddress)
                                           ? this.EndAddress
                                           : this.EndAddress.Trim();

                    if (string.IsNullOrWhiteSpace(startAddress))
                    {
                        return Resources.ScopeValidation_RangeStartIsEmpty;
                    }

                    if (string.IsNullOrWhiteSpace(endAddress))
                    {
                        return Resources.ScopeValidation_RangeEndIsEmpty;
                    }

                    if (IdnSupport.ScxCheckHostName(startAddress) != UriHostNameType.IPv4 &&
                        IdnSupport.ScxCheckHostName(startAddress) != UriHostNameType.IPv6)
                    {
                        return Resources.ScopeValidation_RangeStartIsInvalid;
                    }

                    if (IdnSupport.ScxCheckHostName(endAddress) != UriHostNameType.IPv4 &&
                        IdnSupport.ScxCheckHostName(endAddress) != UriHostNameType.IPv6)
                    {
                        return Resources.ScopeValidation_RangeEndIsInvalid;
                    }

                    if ((IdnSupport.ScxCheckHostName(startAddress) == UriHostNameType.IPv6 &&
                              IdnSupport.ScxCheckHostName(endAddress) == UriHostNameType.IPv4) ||
                             (IdnSupport.ScxCheckHostName(startAddress) == UriHostNameType.IPv4 &&
                              IdnSupport.ScxCheckHostName(endAddress) == UriHostNameType.IPv6))
                    {
                        return Resources.ScopeValidation_RangeValuesInconsistent;
                    }

                    if (IpRangeTooLargeError)
                    {
                        return Resources.IPRange_TooLarge;
                    }

                    return string.Empty;
                }
            }

            /// <summary>
            /// True if current IP address range is too large.
            /// </summary>
            public bool IpRangeTooLargeError
            {
                get
                {
                    var startAddress = string.IsNullOrWhiteSpace(this.StartAddress)
                                         ? this.StartAddress
                                         : this.StartAddress.Trim();
                    var endAddress = string.IsNullOrWhiteSpace(this.EndAddress)
                                           ? this.EndAddress
                                           : this.EndAddress.Trim();

                    var range = new IpRangeDiscoveryScope(IPAddress.Parse(startAddress), IPAddress.Parse(endAddress), 0);
                    return !range.ValidateIpAddressRange();
                }
            }

            /// <summary>
            /// Gets the reason why the end address is not a valid IP address or the empty string if it is.
            /// </summary>
            public string EndAddressValidationError
            {
                get
                {
                    var startAddress = string.IsNullOrWhiteSpace(this.StartAddress)
                                           ? this.StartAddress
                                           : this.StartAddress.Trim();
                    var endAddress = string.IsNullOrWhiteSpace(this.EndAddress)
                                           ? this.EndAddress
                                           : this.EndAddress.Trim();

                    if (string.IsNullOrWhiteSpace(endAddress))
                    {
                        return Resources.ScopeValidation_RangeEndIsEmpty;
                    }

                    if (IdnSupport.ScxCheckHostName(endAddress) != UriHostNameType.IPv4 &&
                        IdnSupport.ScxCheckHostName(endAddress) != UriHostNameType.IPv6)
                    {
                        return Resources.ScopeValidation_RangeEndIsInvalid;
                    }
                    else if ((IdnSupport.ScxCheckHostName(startAddress) == UriHostNameType.IPv6 &&
                              IdnSupport.ScxCheckHostName(endAddress) == UriHostNameType.IPv4) ||
                             (IdnSupport.ScxCheckHostName(startAddress) == UriHostNameType.IPv4 &&
                              IdnSupport.ScxCheckHostName(endAddress) == UriHostNameType.IPv6))
                    {
                        return Resources.ScopeValidation_RangeValuesInconsistent;
                    }

                    return string.Empty;
                }
            }
        }

        #endregion
    }
}