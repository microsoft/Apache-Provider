//-----------------------------------------------------------------------
// <copyright file="ManageScxRunAccounts.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>v-lizhou</author>
// <description></description>
// <history>4/21/2011 5:05:51 PM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Apache.SDK.ApacheSDKHelper
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Security;

    using Microsoft.EnterpriseManagement;
    using Microsoft.EnterpriseManagement.Security;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;
    using Scx.Test.Common;

    /// <summary>
    /// Class for managing ScxRunAs Accounts
    /// </summary>
    public class ManageScxRunAccounts : ManageAccounts
    {
        #region Private Fields

        /// <summary>
        /// Credential object
        /// </summary>
        private Credential credential = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ManageScxRunAccounts class
        /// </summary>
        public ManageScxRunAccounts()
        {
            this.ScxRunAsAccountType = ScxCredentialRef.RunAsAccountType.Monitoring;
            this.CredentialType = ScxCredentialSettings.CredentialType.BasicAuth;
            this.AccountType = ScxCredentialSettings.AccountType.Priviledged;
            this.ElevationType = ScxCredentialSettings.ElevationType.None;
        }

        #endregion
        
        #region Accessors

        /// <summary>
        /// Gets or sets the ScxRunAsAccountType: Maintenance or Monitoring
        /// </summary>
        public ScxCredentialRef.RunAsAccountType ScxRunAsAccountType { get; set; }

        /// <summary>
        /// Gets the Credential object related to the account
        /// </summary>
        public Credential Credential
        { 
            get
            {
                return this.credential ?? (this.credential = new Credential(
                                                                                 this.AccountName,
                                                                                 this.ElevationTypeValue,
                                                                                 this.AccountPassword,
                                                                                 this.SuPassword,
                                                                                 this.SSHKey,
                                                                                 this.SSHKeyPassword));
            }
        }

        /// <summary>
        /// Gets or sets the account's credential type: sshkey or BasicAuth
        /// </summary>
        public ScxCredentialSettings.CredentialType CredentialType { get; set; }

        /// <summary>
        /// Gets or sets the account type,  priviledged or Unpriviledged
        /// </summary>
        public ScxCredentialSettings.AccountType AccountType { get; set; }

        /// <summary>
        /// Gets or sets ssh credential type: sshkey WithPassphrase or sshkey WithoutPassphrase
        /// </summary>
        public ScxCredentialSettings.SSHCredentialType SSHCredentialType { get; set; }

        /// <summary>
        /// Gets or sets elevation type: none, su or sudo.
        /// </summary>
        public ScxCredentialSettings.ElevationType ElevationType { get; set; }

        /// <summary>
        /// Gets the elevation type of the credential.
        /// </summary>
        public string ElevationTypeValue
        {
            get
            {
                if (this.ElevationType == ScxCredentialSettings.ElevationType.None && this.AccountType == ScxCredentialSettings.AccountType.Priviledged)
                {
                    return string.Empty;
                }

                if (this.ElevationType == ScxCredentialSettings.ElevationType.Sudo)
                {
                    return "sudo";
                }

                if (this.ScxRunAsAccountType == ScxCredentialRef.RunAsAccountType.Maintenance && this.ElevationType == ScxCredentialSettings.ElevationType.Su)
                {
                    return "su";
                }

                return "sudo";
            }
        }

        /// <summary>
        /// Gets a value indicating whether this sshkey is used
        /// </summary>
        public bool IsSSHKey
        {
            get
            {
                return this.ScxRunAsAccountType == ScxCredentialRef.RunAsAccountType.Maintenance ? this.CredentialType == ScxCredentialSettings.CredentialType.SSHKey : false;
            }
        }

        /// <summary>
        /// Gets ssh key file path
        /// </summary>
        public string KeyFile
        {
            get
            {
                string keyFile = string.Empty;
                if (this.IsSSHKey)
                {
                    keyFile = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).FullName, "SSHKey");

                    string sshkeyFileName = this.SSHCredentialType == ScxCredentialSettings.SSHCredentialType.WithPassphrase ?
                        ScxCredentialSettings.PriSSHkeyWithPassphrase : ScxCredentialSettings.PriSSHkeyWithoutPassphrase;

                    keyFile = Path.Combine(keyFile, sshkeyFileName);
                }

                return keyFile;
            }
        }

        /// <summary>
        /// Gets ssh key
        /// </summary>
        public string SSHKey
        {
            get
            {
                if (this.IsSSHKey)
                {
                    List<string> keyContents = new List<string>();

                    using (StreamReader reader = File.OpenText(this.KeyFile))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            keyContents.Add(line);
                        }
                    }

                    SecureString sshKey = new PuttyKeyHandler(keyContents).Validate();

                    return DecryptSecureString(sshKey);
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets ssh key password
        /// </summary>
        public string SSHKeyPassword
        {
            get
            {
                string sshkeyPassword = string.Empty;
                if (this.IsSSHKey && this.SSHCredentialType == ScxCredentialSettings.SSHCredentialType.WithPassphrase)
                {
                    sshkeyPassword = SSHKeyHelper.GetSSHKeyPassphrase(Path.Combine(Directory.GetParent(Environment.CurrentDirectory).FullName, "SSHKey"));
                }

                return sshkeyPassword;
            }
        }

        /// <summary>
        /// Gets su password
        /// </summary>
        public string SuPassword
        {
            get
            {
                string suPassword = string.Empty;
                if (this.ElevationType == ScxCredentialSettings.ElevationType.Su)
                {
                    suPassword = ScxCredentialSettings.SuPassphrase;
                }

                return suPassword;
            }
        }

        #endregion
        #region StaticMethods

        /// <summary>
        /// Decrypte secure string
        /// </summary>
        /// <param name="secureString">The given secure string.</param>        
        /// <returns>Clear text of the secure string.</returns>
        public static string DecryptSecureString(SecureString secureString)
        {
            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(ptr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(ptr);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Create this RunAs accounn in the OM
        /// </summary>
        /// <param name="mg">Management group representing connection to ops manager</param>
        public override void CreateAccount(ManagementGroup mg)
        {
            this.logger(string.Format("Creating Scx Run As account: Username '{0}', Elevation '{1}', SSH Key File '{2}'", this.AccountName, this.ElevationTypeValue, this.KeyFile));

            SecureString passwd = new SecureString();
            char[] accountPasswd = this.ScxRunAsAccountType == ScxCredentialRef.RunAsAccountType.Monitoring ?
                this.Credential.Password.ToCharArray(0, this.Credential.Password.Length) :
                this.Credential.XmlPassword.ToCharArray(0, this.Credential.XmlPassword.Length);
            foreach (char c in accountPasswd)
            {
                passwd.AppendChar(c);
            }

            ManagementGroupConnection managementGroupConnection = new ManagementGroupConnection(mg);
            SCXRunAsAccountFactory a = new SCXRunAsAccountFactory(managementGroupConnection);
            a.CreateRunAsAccount(
                this.DisplayName,
                string.Empty,
                this.credential.XmlUserName,
                passwd,
                this.ScxRunAsAccountType,
                !string.IsNullOrEmpty(this.Credential.SSHKey),
                false);
        }
    
        /// <summary>
        /// Delete this SCX RunAs account in the OM
        /// </summary>
        /// <param name="mg">Management group representing connection to ops manager</param>
        public override void DeleteAccount(ManagementGroup mg)
        {
            this.logger(string.Format("Deleting Scx Run As account '{0}'", this.DisplayName));

            SecureData runAsAccount = ManageAccounts.AccountExists(mg, this.DisplayName) ? ManageAccounts.GetOpsRunAsAccount(mg, this.DisplayName) : null;
            if (runAsAccount != null)
            {
                ManagementGroupConnection managementGroupConnection = new ManagementGroupConnection(mg);
                SCXRunAsAccountFactory scxRunAsAccountFactory = new SCXRunAsAccountFactory(managementGroupConnection);

                ScxCredentialRef credentialRef = null;
                try
                {
                    if (runAsAccount.Id != null)
                    {
                        credentialRef = scxRunAsAccountFactory.GetScxRunAsAccount((Guid)runAsAccount.Id);
                    }
                }
                catch (ManagedObjectNotFoundException)
                {
                    // No scxCredentialRef related to this account. Just delete the basic account.
                    mg.Security.DeleteSecureData(runAsAccount);
                }

                if (credentialRef != null)
                {
                    scxRunAsAccountFactory.DeleteScxRunAsAccount(credentialRef);
                }
            }
        }

        #endregion
    }
}
