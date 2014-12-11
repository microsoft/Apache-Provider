// ----------------------------------------------------------------------------------------------------
// <copyright file="CredentialSet.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------------------------------



namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Security;
    using System.Xml;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Properties;

    /// <summary>
    /// Represents a set of credentials.
    /// </summary>
    /// <remarks>
    /// The same instance cannot be added more than once to a set.
    /// </remarks>
    public class CredentialSet : INotifyPropertyChanged, INotifyCollectionChanged, ICloneable
    {
        #region Constructors (2)

        /// <summary>
        ///     Creates an empty set of credentials.
        /// </summary>
        public CredentialSet()
        {
            this.creds = new ObservableCollection<PosixHostCredential>();

            this.creds.CollectionChanged += (sender, e) => OnCollectionChanged(e);
        }

        #endregion Constructors

        #region Properties (2)

        /// <summary>
        ///     Property returns the user name of the credential set.
        /// </summary>
        public string SshUserName
        {
            get
            {
                PosixHostCredential sshCredential = this.CredentialsForAny(CredentialUsage.SshDiscovery | CredentialUsage.SshSudoElevation);
                return sshCredential.PrincipalName;
            }
        }

        /// <summary>
        ///     Property returns the elevation type of the credential set.
        /// </summary>
        public string SshElevationType
        {
            get
            {
                var sshCredential = this.GetMergedSSHCredential();
                if (sshCredential == PosixHostCredential.Empty)
                {
                    return string.Empty;
                }

                if (sshCredential.Usage == CredentialUsage.SshSudoElevation)
                {
                    return "sudo";
                }

                if (CredentialsForAny(CredentialUsage.SuElevation) != PosixHostCredential.Empty)
                {
                    return "su";
                }

                return string.Empty;
            }
        }

        /// <summary>
        ///     Returns an unmodifiable generic collection idem to this set.
        /// </summary>
        public ReadOnlyCollection<PosixHostCredential> Credentials
        {
            get
            {
                return new ReadOnlyCollection<PosixHostCredential>(this.creds);
            }
        }

        /// <summary>
        ///     Property returns the cardinality of the credential set.
        /// </summary>
        public int Count
        {
            get
            {
                return this.creds.Count;
            }
        }

        /// <summary>
        ///     Does the credential set use an SSHKey for default authentication?
        /// </summary>
        public bool IsSSHKey
        {
            get
            {
                return this.isSSHKey;
            }

            set
            {
                if (this.isSSHKey != value)
                {
                    this.isSSHKey = value;
                    if (this.isSSHKey == false)
                    {
                        var sshCred = this.CredentialsForAny(CredentialUsage.SshDiscovery);
                        if (sshCred != PosixHostCredential.Empty)
                        {
                            sshCred.SshKey = null;
                        }
                    }

                    this.InvokePropertyChanged("IsSSHKey");
                }
            }
        }

        /// <summary>
        ///     the credential's usage (maintenance or monitoring)
        /// </summary>
        public CredentialSetUsage Usage
        {
            get
            {
                return this.usage;
            }

            set
            {
                this.usage = value;
                this.InvokePropertyChanged("Usage");
            }
        }

        #endregion Properties

        #region Events

        /// <summary>
        /// Occurs when an item is added, removed, changed, moved, or the entire list is refreshed.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Occurs when an attribute of the credential set is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Methods (7)

        /// <summary>
        ///     Adds a given credential to this set.
        /// </summary>
        /// <param name="cred">The credential to add.</param>
        /// <remarks>
        ///     Trying to add the same object will throw an exception.
        /// </remarks>
        public void Add(PosixHostCredential cred)
        {
            if (null == cred)
            {
                throw new ArgumentNullException("cred");
            }

            if (cred == PosixHostCredential.Empty)
            {
                throw new ArgumentException(Resources.CredentialSet_Add_Supplied_credentials_are_invalid_or_empty, "cred");
            }

            if (this.creds.Contains(cred))
            {
                throw new ArgumentException(Resources.CredentialSet_Add_Duplicate_credentials_error, "cred");
            }

            if (cred.Usage == CredentialUsage.SuElevation && (CredentialsForAny(CredentialUsage.SshSudoElevation) != PosixHostCredential.Empty))
            {
                throw new ArgumentException(Resources.CredentialSet_Add_Sudo_elevation_SSH_credential_already_exists);
            }

            if (cred.Usage == CredentialUsage.SuElevation && (CredentialsForAny(CredentialUsage.SshDiscovery) == PosixHostCredential.Empty))
            {
                throw new ArgumentException(Resources.CredentialSet_Add_No_SSH_credential_has_been_added);
            }

            if (cred.Usage == CredentialUsage.SshSudoElevation && (CredentialsForAny(CredentialUsage.SuElevation) != PosixHostCredential.Empty))
            {
                throw new ArgumentException(Resources.CredentialSet_Add_SSH_credential_already_exists);
            }

            this.creds.Add(cred);
            cred.ParentCredentialSet = this;
            cred.PropertyChanged += (sender, e) => this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        ///     Adds multiple credentials to the set from any enumerable.
        /// </summary>
        /// <param name="credentials">Source enumerable.</param>
        /// <remarks>
        ///     Credentials are compared using their pointers; if they are the SAME
        ///     object, it will not be added (though no error is raised).
        /// </remarks>
        public void AddRange(IEnumerable<PosixHostCredential> credentials)
        {
            if (null == credentials)
            {
                throw new ArgumentNullException("credentials");
            }

            foreach (PosixHostCredential credential in credentials)
            {
                if ((null != credential) && !this.creds.Contains(credential))
                {
                    this.creds.Add(credential);
                }
            }
        }

        /// <summary>
        /// Removes all credentials from this object
        /// </summary>
        public void Clear()
        {
            this.creds.Clear();
        }

        /// <summary>
        ///     Gets the credentials for the purpose indicated in/> <c>usage</c>.
        /// </summary>
        /// <remarks>
        /// The values from the enumeration are combined as bitwise flags.
        /// </remarks>
        /// <param name="usage">A value or combination of values from <c>CredentialUsage </c>enumeration.</param>
        /// <returns>
        /// A <c>PosixHostCredentials </c>instance if a credential that meets <i>all</i> the noted purposes. If there are multiple matching credentials for this SHDC, only the first one will be returned. Should there be nothing matching the criteria, <c>PosixHostCredentials.Empty </c>will be returned.
        /// </returns>
        /// <seealso cref="CredentialUsage">CredentialUsage
        /// Enumeration</seealso>
        /// <seealso cref="PosixHostCredential">PosixHostCredentials</seealso>
        public PosixHostCredential CredentialsForAll(CredentialUsage usage)
        {
            var ccr = from cred in this.creds
                      where cred.CanBeUsedForAll(usage)
                      select cred;

            return ccr.Count() > 0 ? ccr.First() : PosixHostCredential.Empty;
        }

        /// <summary>
        ///     Gets the credentials for the purpose indicated in <c>usage</c>.
        /// </summary>
        /// <remarks>
        /// The values from the enumeration are combined as bitwise flags.
        /// </remarks>
        /// <param name="usage">A value or combination of values from <c>CredentialUsage </c>enumeration.</param>
        /// <returns>
        /// A <c>PosixHostCredentials </c>instance if a credential that meets <i>all</i> the noted purposes. If there are multiple matching credentials for this SHDC, only the first one will be returned. Should there be nothing matching the criteria, <c>PosixHostCredentials.Empty </c>will be returned.
        /// </returns>
        /// <seealso cref="CredentialUsage">CredentialUsage Enumeration</seealso>
        /// <seealso cref="PosixHostCredential">PosixHostCredentials</seealso>
        public PosixHostCredential CredentialsForAny(CredentialUsage usage)
        {
            var ccr = from cred in this.creds
                      where cred.CanBeUsedForAny(usage)
                      select cred;

            return ccr.FirstOrDefault() ?? PosixHostCredential.Empty;
        }

        /// <summary>
        /// Checks if a given credential instance is associated with this criteria.
        /// </summary>
        /// <param name="cred">The credential to check</param>
        /// <returns>
        /// <see langword="true"/> if the credential is associated with criteria; <see langword="false">false </see>otherwise.
        /// </returns>
        public bool HasCredential(PosixHostCredential cred)
        {
            return this.creds.Contains(cred);
        }

        /// <summary>
        ///     Test if the set has at least ALL the usages specified; that is, for all the 
        ///     anticipated usages, this CredentialSet can supply <see cref="PosixHostCredential"/>.
        /// </summary>
        /// <param name="usages">The set of usages</param>
        /// <returns>TRUE if the set has closure; FALSE if not.</returns>
        public bool IsUsageSupersetOf(CredentialUsage usages)
        {
            CredentialUsage cu = usages;

            foreach (var curcred in this.creds)
            {
                CredentialUsage covers = curcred.Usage;
                cu &= ~covers;
            }

            return cu == 0;
        }

        /// <summary>
        /// Removes the given credential from this set.
        /// </summary>
        /// <param name="cred">The credentials to remove; must not be <see langword="null"/>
        /// and must have been previously added to the criteria.</param>
        /// <exception cref="System.ArgumentNullException">If the <c>cred </c>provided is <see langword="null"/></exception>
        /// <exception cref="System.ArgumentException">If the given credential is not
        /// contained in the set. Note that object identity and not semantic equality is
        /// used to compare credential instances.</exception>
        public void Remove(PosixHostCredential cred)
        {
            if (null == cred)
            {
                throw new ArgumentNullException("cred");
            }

            if (!this.creds.Contains(cred))
            {
                throw new ArgumentException(Resources.CredentialSet_Remove_Credentials_not_found_in_Critieria, "cred");
            }

            cred.ParentCredentialSet = null;
            this.creds.Remove(cred);
        }

        /// <summary>
        /// Raises the CollectionChanged event with the provided arguments.
        /// </summary>
        /// <param name="e">The arguments for the event.</param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler handler = this.CollectionChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public object Clone()
        {
            CredentialSet copy = new CredentialSet();

            foreach (PosixHostCredential posixHostCredential in this.Credentials)
            {
                copy.Add((PosixHostCredential)posixHostCredential.Clone());
            }

            copy.Usage = this.Usage;
            copy.IsSSHKey = this.IsSSHKey;

            return copy;
        }


        /// <summary>
        /// Get user account in XML format
        /// </summary>
        /// <param name="usage">The credential usage.</param>
        /// <returns>
        /// return a XML format string include user name info.
        /// </returns>
        public string GetXmlUserName(CredentialUsage usage)
        {
            if (usage != CredentialUsage.SshDiscovery && usage != CredentialUsage.WsManDiscovery && usage != CredentialUsage.SshSudoElevation)
            {
                throw new ArgumentOutOfRangeException("usage");
            }

            PosixHostCredential credential;

            if (usage == CredentialUsage.SshDiscovery || usage == CredentialUsage.SshSudoElevation)
            {
                credential = this.GetMergedSSHCredential();
            }
            else
            {
                credential = CredentialsForAny(usage);
            }

            var doc = new XmlDocument();

            XmlElement root = doc.CreateElement("SCXUser");
            doc.AppendChild(root);

            var sshElevationType = this.SshElevationType;

            XmlNode elem = doc.CreateNode(XmlNodeType.Element, "UserId", null);
            elem.InnerText = credential.PrincipalName;
            root.AppendChild(elem);

            XmlNode elvElem = doc.CreateNode(XmlNodeType.Element, "Elev", null);
            if (usage == CredentialUsage.WsManDiscovery)
            {
                elvElem.InnerText = string.Empty;

                if (credential.Usage == (CredentialUsage.WsManDiscovery | CredentialUsage.WsmanSudoElevation))
                {
                    elvElem.InnerText = "sudo";
                }
            } 
            else
            {
                elvElem.InnerText = sshElevationType;
            }

            root.AppendChild(elvElem);

            return doc.InnerXml;
        }

        /// <summary>
        /// Get password in XML format
        /// </summary>
        /// <param name="usage">The credential usage.</param>
        /// <returns>
        /// return a XML format string of the password info.
        /// </returns>
        public SecureString GetXmlPassword(CredentialUsage usage)
        {
            if (usage == CredentialUsage.SshDiscovery)
            {
                return GetXmlSshPassword();
            }

            if (usage == CredentialUsage.WsManDiscovery)
            {
                return GetXmlWsmanPassword();
            }

            throw new ArgumentOutOfRangeException("usage");
        }

        /// <summary>
        /// Merge the main SSH credentials with sudo credentials.
        /// </summary>
        /// <returns>
        /// Return a PosixHostCredential with right credential and elevation type.
        /// </returns>
        private PosixHostCredential GetMergedSSHCredential()
        {
            var sshCred = CredentialsForAny(CredentialUsage.SshDiscovery);
            var sshSudoCred = CredentialsForAny(CredentialUsage.SshSudoElevation);

            PosixHostCredential mergedCredential = null;
            PosixHostCredential tempMergedCredential = null;
            try
            {
                if (sshCred != PosixHostCredential.Empty)
                {
                    tempMergedCredential = (PosixHostCredential) sshCred.Clone();
                    // If sudo is also set, mark usage 
                    if (sshSudoCred != PosixHostCredential.Empty)
                    {
                        tempMergedCredential.Usage = CredentialUsage.SshSudoElevation;
                    }
                }
                else if (sshSudoCred != PosixHostCredential.Empty)
                {
                    tempMergedCredential = (PosixHostCredential) sshSudoCred.Clone();
                    System.Diagnostics.Debug.Assert(tempMergedCredential.Usage == CredentialUsage.SshSudoElevation);
                }
                else
                {
                    // Beware - the logic in this area is pretty obscure. This case might 
                    // correspond to tempMergedCredential.Usage being for WSMan monitoring, 
                    // but in a newly creted CS an undefined Usage value is unfortunately
                    // allowed so we don't know at this time.
                    PosixHostCredential emptyCredential = null;

                    try
                    {
                        emptyCredential = new PosixHostCredential();
                        tempMergedCredential = emptyCredential;
                        emptyCredential = null;
                    }
                    finally 
                    {
                        if (emptyCredential != null)
                        {
                            emptyCredential.Dispose();
                        }
                    }
                }

                mergedCredential = tempMergedCredential;
                tempMergedCredential = null;
            }
            finally
            {
                if (tempMergedCredential != null)
                {
                    tempMergedCredential.Dispose();
                }
            }

            return mergedCredential;
        }

        /// <summary>
        /// Get SSH passphrase in XML format
        /// </summary>
        /// <returns>
        /// return a XML format string of the SSH passphrase info.
        /// </returns>
        private SecureString GetXmlSshPassword()
        {
            var sshCredential = GetMergedSSHCredential();

            var suCredential = CredentialsForAny(CredentialUsage.SuElevation);

            var doc = new XmlDocument();

            XmlElement root = doc.CreateElement("SCXSecret");
            doc.AppendChild(root);

            XmlNode passElem = doc.CreateNode(XmlNodeType.Element, "Password", null);
            if (sshCredential.Passphrase != null && sshCredential.Passphrase.Length > 0)
            {
                passElem.InnerText = SecureStringHelper.Decrypt(sshCredential.Passphrase);
            }

            root.AppendChild(passElem);

            XmlNode suPassElem = doc.CreateNode(XmlNodeType.Element, "SuPassword", null);
            if (suCredential.Passphrase != null && suCredential.Passphrase.Length > 0)
            {
                suPassElem.InnerText = SecureStringHelper.Decrypt(suCredential.Passphrase);
            }

            root.AppendChild(suPassElem);

            XmlNode keyElem = doc.CreateNode(XmlNodeType.Element, "SSHKey", null);

            if (this.IsSSHKey && (sshCredential.SshKey == null || sshCredential.SshKey.Length == 0))
            {
                throw new InvalidOperationException(Resources.CredentialSet_GetXmlSshPassword_SshKey_value_not_initialized);
            }

            if (sshCredential.SshKey != null && sshCredential.SshKey.Length > 0)
            {
                keyElem.InnerText = SecureStringHelper.Decrypt(sshCredential.SshKey);
            }

            root.AppendChild(keyElem);

            return SecureStringHelper.Encrypt(doc.InnerXml);
        }


        /// <summary>
        /// Get WsMan passphrase in XML format
        /// </summary>
        /// <returns>
        /// return a XML format string of the WsMan passphrase info.
        /// </returns>
        private SecureString GetXmlWsmanPassword()
        {
            var wsmanCredential = CredentialsForAny(CredentialUsage.WsManDiscovery);

            return wsmanCredential.Passphrase;
        }

        /// <summary>
        /// Invokes the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">
        /// Name of the property that caused the event.
        /// </param>
        protected virtual void InvokePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion Methods

        #region Fields (1)

        /// <summary>
        /// A PosixHostCredential collection used to internally represent this
        /// CredentialSet.  An ObservableCollection is used to allow the UI to
        /// track change notifications.
        /// </summary>
        private ObservableCollection<PosixHostCredential> creds;

        private CredentialSetUsage usage;

        private bool isSSHKey;

        #endregion Fields
    }
}
