//-----------------------------------------------------------------------
// <copyright file="ScxCredentialRef.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Security;

    using Microsoft.EnterpriseManagement.Security;

    public class ScxCredentialRef
    {
        public enum RunAsAccountType
        {
            None = 0,
            Monitoring = 1,
            Maintenance = 2
        }

        /// <summary>
        /// Gets or sets the secure data.
        /// </summary>
        public SecureData RunAsAcount
        {
            get
            {
                return this.runAsAccount;
            }

            set
            {
                if (null != Key && (null != value) && (null != value.Id) && (!string.Equals(Key, value.Id.ToString())))
                {
                    throw new ArgumentException(Strings.ScxCredentialRef_RunAsAcount_Key_and_account_ID_do_not_match);
                }

                this.runAsAccount = value;
            }
        }

        /// <summary>
        /// Gets the encrypted account secrets.
        /// </summary>
        /// <returns>SecureString instance of the account secrets.</returns>
        public SecureString Data
        {
            get { return this.RunAsAcount.Data; }
        }

        public ScxCredentialRef(IManagedObject managedObject)
        {
            this.ManagedObject = managedObject;
            this.runAsAccount = null;
        }

        /// <summary>
        /// Gets or sets the secure data ID used by OM.
        /// </summary>
        public string Key
        {
            get 
            {
                if (null != ManagedObject)
                {
                    return this.ManagedObject.GetPropertyValue("Key");
                }

                return string.Empty;
            }

            // Key and CredentialId are duplicated to each other in MP Microsoft.Unix.CredentialRef class.
            // However, Key property is defined as a key property of the class and used internally.
            // CredentialId is used by OM to hide SCX RunAs accounts.
            set
            {
                this.ManagedObject.SetPropertyValue("Key", value);
                this.ManagedObject.SetPropertyValue("CredentialId", value);
            }
        }

        /// <summary>
        /// Gets or sets the flag to indicate if this is an SSH key.
        /// </summary>
        public bool IsSSHKey
        {
            get
            {
                if (null != ManagedObject)
                {
                    return Convert.ToBoolean(this.ManagedObject.GetPropertyValue("IsSSHKey"));
                }

                return false;
            }

            set
            {
                this.ManagedObject.SetPropertyValue("IsSSHKey", Convert.ToString(value));
            }
        }

        /// <summary>
        /// Gets or sets the type of this RunAs account.
        /// </summary>
        public RunAsAccountType Type
        {
            get
            {
                if (null != ManagedObject)
                {
                    var type = int.Parse(this.ManagedObject.GetPropertyValue("Type"), CultureInfo.InvariantCulture);
                    return (RunAsAccountType)type;
                }

                return RunAsAccountType.None;
            }

            set
            {
                this.ManagedObject.SetPropertyValue("Type", (int)value);
            }
        }

        /// <summary>
        /// Gets this RunAs account name.
        /// </summary>
        public string Name
        {
            get
            {
                return null != RunAsAcount ? RunAsAcount.Name : string.Empty;
            }
        }

        /// <summary>
        /// Gets this RunAs account user name.
        /// </summary>
        public string UserName
        {
            get
            {
                if (RunAsAcount != null)
                {
                    var account = RunAsAcount as BasicCredentialSecureData;
                    Debug.Assert(account != null, "Run As account is not a type of BasicCredentialSecureData.");
                    return account.UserName;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets this RunAs account description.
        /// </summary>
        public string Description
        {
            get
            {
                return null != RunAsAcount ? RunAsAcount.Description : string.Empty;
            }
        }

        /// <summary>
        /// Gets the last time this account is modified.
        /// </summary>
        public DateTime LastModified
        {
            get
            {
                return null != RunAsAcount ? RunAsAcount.LastModified : DateTime.MinValue;
            }
        }

        public IManagedObject ManagedObject { get; private set; }

        private SecureData runAsAccount;
    }
}
