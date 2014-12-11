// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RunAsAccountBase.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement
{
    using System;
    using System.Diagnostics;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Properties;

    public abstract class RunAsAccountBase
    {
        /// <summary>
        ///     Gets the unique identifier assigned to the Run As account.
        /// </summary>
        public Guid Id
        {
            get
            {
                return RunAsAccount.RunAsAccountId;
            }
        }


        /// <summary>
        ///     Gets the type of the Run As account, either Monitor or Maintenance.
        /// </summary>
        public ScxRunAsAccountType Type
        { 
            get
            {
                Debug.Assert(
                    RunAsAccount.Credential.Usage != CredentialSetUsage.AgentUninstall,
                    string.Format(
                        "The SCX Run As account '{0}' contains an unexpected meta-type: {1}.",
                        RunAsAccount.RunAsAccountId,
                        RunAsAccount.Credential.Usage));

                return (RunAsAccount.Credential.Usage == CredentialSetUsage.Monitoring)
                           ? ScxRunAsAccountType.Monitor
                           : ScxRunAsAccountType.Maintenance;
            }
        }


        /// <summary>
        ///     Gets or sets the name of the Run As account.
        /// </summary>
        /// <remarks>
        ///     Setting <see langword="null"/> values are silently ignored,
        ///     i.e. the property is <i>not</i> changed.
        /// </remarks>
        public string Name
        {
            get
            {
                return RunAsAccount.DisplayName;
            }

            set
            {
                // if the value is null, keep the current value
                if (value == null)
                {
                    return;
                }

                if (string.IsNullOrEmpty(value) || value.Trim().Length == 0)
                {
                    throw new ArgumentException(Resources.RunAsAccount_RunAsAccountMustHaveValidName, @"Name");
                }

                RunAsAccount.DisplayName = value;
            }
        }


        /// <summary>
        ///     Gets or sets the description of the Run As account.
        /// </summary>
        /// <remarks>
        ///     Setting <see langword="null"/> values are silently ignored,
        ///     i.e. the property is <i>not</i> changed.  An empty string
        ///     is allowed.
        /// </remarks>
        public string Description
        {
            get
            {
                return RunAsAccount.Description;
            }

            set
            {
                // if the value is null, keep the current value
                if (value == null)
                {
                    return;
                }

                RunAsAccount.Description = value;

                if (string.IsNullOrEmpty(value) || value.Trim().Length == 0)
                {
                    RunAsAccount.Description = string.Empty;
                }
            }
        }


        /// <summary>
        ///     Gets or sets the name of the managed host user associated with
        ///     the Run As account.
        /// </summary>
        /// <remarks>
        ///     Setting <see langword="null"/> values are silently ignored,
        ///     i.e. the property is <i>not</i> changed.
        /// </remarks>
        public string Username
        {
            get
            {
                return PrimaryCredential.PrincipalName;
            }

            set
            {
                // if the value is null, keep the current value
                if (value == null)
                {
                    return;
                }

                if (string.IsNullOrEmpty(value) || value.Trim().Length == 0)
                {
                    throw new ArgumentException(Resources.RunAsAccount_RunAsAccountMustHaveValidUsername, @"Username");
                }

                // throws IllegalAuthenticationCharacterException if value contains non-ASCII characters
                CmdletSupport.CheckAuthenticationDataEncoding(value, @"Username");

                PrimaryCredential.PrincipalName = value;
            }
        }


        /// <summary>
        ///     Gets the date and time of the last modification to the Run As account.
        /// </summary>
        public DateTime LastModified
        {
            get
            {
                return RunAsAccount.LastModified;
            }
        }


        public ScxRunAsAccount RunAsAccount { get; private set; }


        /// <summary>
        ///     Gets the primary PosixHostCredential for this Run As account.
        /// </summary>
        protected abstract PosixHostCredential PrimaryCredential { get; }


        /// <summary>
        ///     Construct an SCX Monitor Run As account.
        /// </summary>
        /// <param name="runAsAccount">
        ///     An ScxRunAsAccount core object.
        /// </param>
        protected RunAsAccountBase(ScxRunAsAccount runAsAccount)
        {
            RunAsAccount = runAsAccount;
        }


        protected internal static ScxRunAsAccount CreateScxRunAsAccount(string name, string description, string username, IAuthenticationDataGenerator authenticationData)
        {
            // throws IllegalAuthenticationCharacterException if username contains non-ASCII characters
            CmdletSupport.CheckAuthenticationDataEncoding(username, @"Username");

            if (string.IsNullOrEmpty(name) || name.Trim().Length == 0)
            {
                throw new ArgumentException(Resources.RunAsAccount_RunAsAccountMustHaveValidName, "Name");
            }

            if (string.IsNullOrEmpty(username) || username.Trim().Length == 0)
            {
                throw new ArgumentException(Resources.RunAsAccount_RunAsAccountMustHaveValidUsername, "Username");
            }

            if (description == null || description.Trim().Length == 0)
            {
                description = string.Empty;
            }

            return new ScxRunAsAccount
            {
                DisplayName = name,
                Description = description,
                Credential = authenticationData.GenerateAuthenticationData(username)
            };
        }

        protected internal interface IAuthenticationDataGenerator
        {
            /// <summary>
            ///     Generate a <see cref="CredentialSet"/> for the given user.
            /// </summary>
            /// <param name="username">
            ///     The username to associate with the <see cref="CredentialSet"/>.
            /// </param>
            /// <returns>
            ///     This method returns a <see cref="CredentialSet"/> for the given user.
            /// </returns>
            CredentialSet GenerateAuthenticationData(string username);
        }
    }
}