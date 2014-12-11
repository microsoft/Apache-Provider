// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PosixHostCredential.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Security;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Exceptions;

    /// <summary>
    ///     Represents a credential to a POSIX host.
    /// </summary>
    public class PosixHostCredential : INotifyPropertyChanged, ICloneable, IDisposable
    {
        #region Fields (1) 

        /// <summary>
        /// Reference to the (Singleton) Instance.
        /// </summary>
        public static PosixHostCredential Empty
        {
            get { return empty; }
        }

        #endregion Fields 

        #region Events

        /// <summary>
        /// Occurs after the value of an application settings property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Occurs after the internal secrets have been cleared.
        /// </summary>
        public event EventHandler<EventArgs> SecretsCleared;

        #endregion Events

        #region Lifecycle (1)

        /// <summary>
        ///     Initializes static members of the <see cref="PosixHostCredential"/> class.
        /// </summary>
        /// <remarks>
        ///     Creates the (const) Empty instance.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
            Justification = "Static instance will not go out of scope until termination.")]
        static PosixHostCredential()
        {
            empty = new PosixHostCredential
            {
                PrincipalName = string.Empty,
                Passphrase = new SecureString(),
                SshKey = new SecureString(),
                Usage = CredentialUsage.None
            };
        }

        #endregion Lifecycle

        #region Properties (4)

        /// <summary>
        /// Gets or sets a reference to the CredentialSet that contains this instance.
        /// </summary>
        public CredentialSet ParentCredentialSet { get; set; }

        /// <summary>
        ///     Gets or sets the passphrase.
        /// </summary>
        /// <remarks>
        ///     The passphrase is stored in an encrypted format (DPAPI) that is compatible with
        ///     other system facilities. When forwarding to a remote system, it <i>must</i>
        /// </remarks>
        /// <value>
        ///     The passphrase associated with this credential.
        /// </value>
        public SecureString Passphrase
        {
            get
            {
                return passphrase;
            }

            set
            {
                this.passphrase = value;
                OnPropertyChanged("Passphrase");
            }
        }

        /// <summary>
        ///     Gets or sets the SSH key.
        /// </summary>
        /// <value>
        ///     The SSH key associated with this credential.
        /// </value>
        public SecureString SshKey
        {
            get
            {
                return sshKey;
            }

            set
            {
                this.sshKey = value;
                OnPropertyChanged("SshKey");
            }
        }

        /// <summary>   
        ///     Gets or sets the name of the principal. 
        /// </summary>
        /// <value> 
        ///     The name of the principal. 
        /// </value>
        public string PrincipalName
        {
            get
            {
                return this.principal;
            }

            set
            {
                // string.IsNullOrWhiteSpace(value)) is not available in .NET 3.5
                if (string.IsNullOrEmpty(value) || value.Trim().Length == 0)
                {
                    value = string.Empty;
                }

                bool raisePropChanged = value != this.principal;

                this.principal = value;

                if (raisePropChanged)
                {
                    OnPropertyChanged("PrincipalName");
                }
            }
        }


        /// <summary>
        /// Gets or sets the usages for this credential.
        /// </summary>
        /// <value>The usages (bitwise OR) for this credential.</value>
        public CredentialUsage Usage
        {
            get
            {
                return usage;
            }

            set
            {
                bool raisePropChanged = value != this.usage;

                usage = value;

                if (raisePropChanged)
                {
                    OnPropertyChanged("Usage");
                }
            }
        }

        /// <summary>
        /// Gets or sets the path to the key file that can be used for SSH authentication.
        /// </summary>
        public string KeyFile
        {
            get
            {
                return keyFile;
            }

            set
            {
                bool raisePropChanged = value != this.keyFile;

                keyFile = value;

                if (raisePropChanged)
                {
                    OnPropertyChanged("KeyFile");
                }
            }
        }

        #endregion Properties 

        #region Methods

        /// <summary>
        /// Check if the credential supports any of the purposes indicated in <paramref name="uses"/>.
        /// </summary>
        /// <param name="uses">The intended purpose being queried.</param>
        /// <returns>
        /// <see langword="true"/> if any of the indicated purpose(s) are supported;
        /// <see langword="false"/> if <i>none</i> of the requested purposes are valid for this credential.
        /// </returns>
        public bool CanBeUsedForAny(CredentialUsage uses)
        {
            return (Usage & uses) != 0;
        }

        /// <summary>
        /// Checks if a credential is valid for the indicated purpose(s).
        /// </summary>
        /// <param name="uses">The purpose(s) being queried.</param>
        /// <returns>
        /// <see langword="true"/> if the use(s) indicated in <paramref name="uses "/>are
        /// supported by this credential; <see langword="false"/> if the intent cannot be
        /// supported.
        /// </returns>
        public bool CanBeUsedForAll(CredentialUsage uses)
        {
            return ((Usage & uses) ^ uses) == 0;
        }

        /// <summary>
        /// Invokes the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the Property that triggered the event.</param>
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Invokes the SecretsCleared event.
        /// </summary>
        public void InvokeSecretsCleared()
        {
            EventHandler<EventArgs> handler = this.SecretsCleared;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
            Justification = "This would be possible to re-write using the temp var pattern but the effort is not justified.")]
        public object Clone()
        {
            // this is needed because the verification of placeholder secrets is
            // reference based, and if placeholder secrets are used, the references
            // need to be preserved
            bool persistSecrets = ScxRunAsAccountHelper.HasUnserializedSecret(this);

            PosixHostCredential tempPhc = null;
            PosixHostCredential clonedPhc = null;

            try
            {
                tempPhc = new PosixHostCredential
                {
                    PrincipalName = this.PrincipalName != null ? string.Copy(this.PrincipalName) : null,
                    Passphrase =
                        this.Passphrase != null
                            ? persistSecrets ? this.Passphrase : this.Passphrase.Copy()
                            : null,
                    SshKey =
                        this.SshKey != null ? persistSecrets ? this.SshKey : this.SshKey.Copy() : null,
                    KeyFile = this.keyFile != null ? string.Copy(this.KeyFile) : null,
                    Usage = this.Usage
                };
                clonedPhc = tempPhc;
                tempPhc = null;
            }
            finally
            {
                if (tempPhc != null)
                {
                    tempPhc.Dispose();
                }
            }
            return clonedPhc;
        }

        /// <summary>
        /// Read in the SSH private key bits from the specific file and validate it.
        /// </summary>
        /// <exception cref="InvalidSshKeyException">
        /// Thrown when the SSH key cannot be validated.
        /// </exception>
        public void ReadAndValidateSshKey()
        {
            if (string.IsNullOrEmpty(this.KeyFile))
            {
                this.sshKey = new SecureString();
            }
            else
            {
                List<string> keyContents = new List<string>();

                using (StreamReader reader = File.OpenText(this.keyFile))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        keyContents.Add(line);
                    }
                }

                if (keyContents.Count == 0)
                {
                    throw new InvalidSshKeyException(InvalidSshKeyException.ValidationFault.UnrecognizedFormat);
                }

                using (PuttyKeyHandler puttyKeyHandler = new PuttyKeyHandler(keyContents))
                {
                    this.sshKey = puttyKeyHandler.Validate();
                }
            }
        }

        /// <summary>
        ///     Frees disposable resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (sshKey != null)
                {
                    sshKey.Dispose();
                    sshKey = null;
                }

                if (passphrase != null)
                {
                    passphrase.Dispose();
                    passphrase = null;
                }
            }
        }

        #endregion Methods

        #region Fields

        private string principal;
        private CredentialUsage usage;
        private string keyFile;
        private SecureString sshKey;
        private SecureString passphrase;

        /// <summary>
        /// (Singleton) Instance representing an invalid or empty credential.
        /// </summary>
        private static readonly PosixHostCredential empty;

        #endregion Fields
    }
}
