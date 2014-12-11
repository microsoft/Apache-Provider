// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DiscoveryCriteria.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Net;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core;

    /// <summary>
    /// Defines criteria for discovering Linux/Unix systems.
    /// </summary>
    public class DiscoveryCriteria : IEnumerable<DiscoveryTargetEndpoint>, ICloneable, INotifyPropertyChanged
    {
        #region Constants and Fields

        /// <summary>
        ///     Internal holder for credentials associated with this host.
        /// </summary>
        private CredentialSet credentials = new CredentialSet();

        /// <summary>
        /// Backing field for the EnableRunasCredentials property.
        /// </summary>
        private bool enableRunasCredentials;

        /// <summary>
        /// Backing field for the EnableOnlyWSManDiscovery property.
        /// </summary>
        private bool enableOnlyWsManDiscovery;

        /// <summary>
        ///     Holds the list of scopes associated with this criterion.
        /// </summary>
        private DiscoveryScopeCollection scopes = new DiscoveryScopeCollection();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the DiscoveryCriteria class.
        /// </summary>
        public DiscoveryCriteria()
        {
            this.credentials.CollectionChanged += (sender, e) => this.OnPropertyChanged("CredentialSet");
            this.scopes.CollectionChanged += (sender, e) => this.OnPropertyChanged("Scopes");
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs after the value of an application settings property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the credential set associated with this criteria.
        /// </summary>
        /// <value>The Credential Set owned by this object.</value>
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
                    bool bind = this.credentials != value;

                    this.credentials = value;
                    this.OnPropertyChanged("CredentialSet");

                    if (bind)
                    {
                        this.credentials.CollectionChanged += (sender, e) => this.OnPropertyChanged("CredentialSet");
                    }
                }
                else
                {
                    this.credentials.Clear();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether EnableRunasCredentials.
        /// </summary>
        public bool EnableRunasCredentials
        {
            get
            {
                return this.enableRunasCredentials;
            }

            set
            {
                this.enableRunasCredentials = value;
                this.OnPropertyChanged("EnableRunasCredentials");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether EnableOnlyWSManDiscovery.
        /// </summary>
        public bool EnableOnlyWSManDiscovery
        {
            get
            {
                return this.enableOnlyWsManDiscovery;
            }

            set
            {
                this.enableOnlyWsManDiscovery = value;
                this.OnPropertyChanged("EnableOnlyWSManDiscovery");
            }
        }

        /// <summary>
        /// Gets or sets Scopes.
        /// </summary>
        [CLSCompliant(false)]
        public DiscoveryScopeCollection Scopes
        {
            get
            {
                return this.scopes;
            }

            set
            {
                if (null != value)
                {
                    bool bind = this.scopes != value;

                    this.scopes = value;
                    this.OnPropertyChanged("Scopes");

                    if (bind)
                    {
                        this.scopes.CollectionChanged += (sender, e) => this.OnPropertyChanged("Scopes");
                    }
                }
                else
                {
                    this.scopes.Clear();
                }
            }
        }

        #endregion

        #region Implemented Interfaces

        #region ICloneable

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public object Clone()
        {
            var copy = new DiscoveryCriteria
                {
                    EnableOnlyWSManDiscovery = this.EnableOnlyWSManDiscovery,
                    CredentialSet = (CredentialSet)this.CredentialSet.Clone(),
                    EnableRunasCredentials = this.EnableRunasCredentials,
                    Scopes = (DiscoveryScopeCollection)this.Scopes.Clone()
                };

            return copy;
        }

        #endregion

        #region IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region IEnumerable<DiscoveryTargetEndpoint>

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<DiscoveryTargetEndpoint> GetEnumerator()
        {
            foreach (IDiscoveryScope scope in this.Scopes)
            {
                foreach (IPHostEntry hostent in scope)
                {
                    var dte = new DiscoveryTargetEndpoint
                        {
                            CredentialSet = this.credentials, 
                            HostName = hostent.HostName, 
                            IP = (hostent.AddressList != null) ? hostent.AddressList[0] : IPAddress.None, 
                            SSHPort = scope.SshPort
                        };

                    yield return dte;
                }
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Invokes the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">
        /// Name of the Property that triggered the event.
        /// </param>
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}