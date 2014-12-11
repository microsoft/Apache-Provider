//-----------------------------------------------------------------------
// <copyright file="ScxRunAsAccountHelper.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Security;
    using System.Xml;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Exceptions;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Properties;

    /// <summary>
    /// ScxRunAsAccount helper class.
    /// </summary>
    public class ScxRunAsAccountHelper
    {
        /// <summary>
        /// Create an instance of SCX Run As account.
        /// </summary>
        /// <param name="managementGroupConnection">the management groupConnection object.</param>
        /// <param name="account">the RunAs account.</param>
        /// <param name="secureDistribution">The secure distribution of the RunAs account.</param>
        public static void CreateScxRunAsAccount(IManagementGroupConnection managementGroupConnection, ScxRunAsAccount account, ScxRunAsAccountSecureDistribution secureDistribution)
        {
            using (var factory = new SCXRunAsAccountFactory(managementGroupConnection))
            {
                CreateRunAsAccountFromFactory(factory, account, secureDistribution);
            }
        }

        /// <summary>
        /// Create an instance of SCX Run As account with the given SCX Run As account factory.
        /// </summary>
        /// <param name="factory">the SCX Run As account factory.</param>
        /// <param name="account">the RunAs account.</param>
        /// <param name="secureDistribution">The secure distribution of the RunAs account.</param>
        private static void CreateRunAsAccountFromFactory(SCXRunAsAccountFactory factory, ScxRunAsAccount account, ScxRunAsAccountSecureDistribution secureDistribution)
        {
            var usage = CredentialUsage.SshDiscovery;
            var accountType = ScxCredentialRef.RunAsAccountType.Maintenance;
            if (account.Credential.Usage == CredentialSetUsage.Monitoring)
            {
                usage = CredentialUsage.WsManDiscovery;
                accountType = ScxCredentialRef.RunAsAccountType.Monitoring;
            }

            Debug.Assert(account.Credential.CredentialsForAny(usage) != PosixHostCredential.Empty, "Given credential usage is not in credential set.");

            var userName = account.Credential.GetXmlUserName(usage);
            var password = account.Credential.GetXmlPassword(usage);

            try
            {
                factory.CreateRunAsAccount(account.DisplayName, account.Description, userName, password, accountType, account.Credential.IsSSHKey, secureDistribution.SecureDistribution);
            }
            catch (SCXRunAsAccountFactory.CredentialRefInsertionCollisionException credentialRefInsertionCollisionException)
            {
                throw new ScxRunAsAccountAlreadyExistsException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.ScxRunAsAccountHelper_RunAsAccountAlreadyExists,
                        credentialRefInsertionCollisionException.UnixAccountId),
                    credentialRefInsertionCollisionException);
            }
        }

        /// <summary>
        /// Delete an instance of SCX Run As account.
        /// </summary>
        /// <param name="managementGroupConnection">the management groupConnection object.</param>
        /// <param name="id">the RunAs account id.</param>
        public static void DeleteScxRunAsAccount(IManagementGroupConnection managementGroupConnection, Guid id)
        {
            using (var factory = new SCXRunAsAccountFactory(managementGroupConnection))
            {
                var accountRef = factory.GetScxRunAsAccount(id);

                factory.DeleteScxRunAsAccount(accountRef);
            }
        }

        /// <summary>
        /// Get a RunAs account from its id
        /// </summary>
        /// <param name="managementGroupConnection">the management groupConnection object.</param>
        /// <param name="id">The RunAs account id.</param>
        /// <returns>The ScxRunAsAccount instance.</returns>
        public static ScxRunAsAccount GetScxRunAsAccount(IManagementGroupConnection managementGroupConnection, Guid id)
        {
            if (managementGroupConnection == null)
            {
                throw new ArgumentNullException("managementGroupConnection");
            }

            ScxCredentialRef accountRef;
            using (var factory = new SCXRunAsAccountFactory(managementGroupConnection))
            {
                accountRef = factory.GetScxRunAsAccount(id);
            }

            return DeserializeToScxRunAsAccount(accountRef);
        }

        /// <summary>
        /// Get the associated RunAs secure distribution with the given SCX Run As account id
        /// </summary>
        /// <param name="managementGroupConnection">the management server.</param>
        /// <param name="id">The RunAs account id.</param>
        /// <returns>The secure distribution object.</returns>
        public static ScxRunAsAccountSecureDistribution GetRunAsAccountSecureDistribution(IManagementGroupConnection managementGroupConnection, Guid id)
        {
            return managementGroupConnection.GetSecureDistributions(id);
        }

        /// <summary>
        /// Get the associated RunAs profile names with the given SCX Run As account id
        /// </summary>
        /// <param name="managementGroupConnection">the management groupConnection object.</param>
        /// <param name="id">The RunAs account id.</param>
        /// <returns>The list of RunAs profile names.</returns>
        public static IList<string> GetScxRunAsProfiles(IManagementGroupConnection managementGroupConnection, Guid id)
        {
            if (managementGroupConnection == null)
            {
                throw new ArgumentNullException("managementGroupConnection");
            }

            return managementGroupConnection.GetRunAsAccountAssociation(id);
        }

        /// <summary>
        /// Update an instance of SCX Run As account.
        /// </summary>
        /// 
        /// When updating a RunAs account, all deserialized data must be valid
        /// using the logic:
        /// 
        ///     * If there are \i no credentials in an entire \c CredentialSet
        ///       that have properties referencing UnserializedSecret, then the
        ///       set is valid.
        /// 
        ///     * Otherwise, \i all SecureString properties (Passphrase and
        ///       SshKey) \b MUST reference UnserializedSecret.
        /// 
        /// <param name="managementGroupConnection">the management groupConnection object.</param>
        /// <param name="account">the RunAs account.</param>
        /// <param name="secureDistribution">The secure distribution of the RunAs account.</param>
        public static void UpdateScxRunAsAccount(
            IManagementGroupConnection managementGroupConnection,
            ScxRunAsAccount account,
            ScxRunAsAccountSecureDistribution secureDistribution)
        {
            using (var factory = new SCXRunAsAccountFactory(managementGroupConnection))
            {
                UpdateScxRunAsAccountFromFactory(factory, account, secureDistribution);
            }
        }

        /// <summary>
        /// Update an instance of SCX Run As account with the given SCX Run As account factory.
        /// </summary>
        /// 
        /// When updating a RunAs account, all deserialized data must be valid
        /// using the logic:
        /// 
        ///     * If there are \i no credentials in an entire \c CredentialSet
        ///       that have properties referencing UnserializedSecret, then the
        ///       set is valid.
        /// 
        ///     * Otherwise, \i all SecureString properties (Passphrase and
        ///       SshKey) \b MUST reference UnserializedSecret.
        /// 
        /// <param name="factory">the SCX Run As account factory object.</param>
        /// <param name="account">the RunAs account.</param>
        /// <param name="secureDistribution">The secure distribution of the RunAs account.</param>
        private static void UpdateScxRunAsAccountFromFactory(
            SCXRunAsAccountFactory factory,
            ScxRunAsAccount account,
            ScxRunAsAccountSecureDistribution secureDistribution)
        {
            ScxCredentialRef accountRef = factory.GetScxRunAsAccount(account.RunAsAccountId);

            CredentialUsage usage = CredentialUsage.SshDiscovery;
            if (account.Credential.Usage == CredentialSetUsage.Monitoring)
            {
                usage = CredentialUsage.WsManDiscovery;
            }

            Debug.Assert(account.Credential.CredentialsForAny(usage) != PosixHostCredential.Empty, "Given credential usage is not in credential set.");

            string userName = account.Credential.GetXmlUserName(usage);
            SecureString password = null;

            // Check if the account has any credentials that have any secure
            // string members (Passphrase and SshKey) that are set to UnserializedSecret.
            if (HasUnserializedSecret(account))
            {
                // If an account has any unserialized secrets, then all the
                // secure string members of all the associated credentials
                // MUST be UnserializedSecret.
                if (IsUnserializedSecretValid(account) == false)
                {
                    throw new InvalidCredentialException(
                        string.Format(CultureInfo.CurrentCulture, Resources.ScxRunAsAccountHelper_BadDeserialization));
                }
            }
            else
            {
                // If an account doesn't have any unserialized secrets, then
                // it is considered "valid" at this level.  More rigorous validity
                // test should be run on each credential property when and
                // where they are set.
                password = account.Credential.GetXmlPassword(usage);
            }

            factory.UpdateScxRunAsAccount(accountRef, account.DisplayName, account.Description, userName, password, account.Credential.IsSSHKey, secureDistribution);
        }

        /// <summary>
        /// Enumerate all SCX Run As accounts.
        /// </summary>
        /// <param name="managementGroupConnection">the management groupConnection object.</param>
        /// <returns>The RunAs accounts.</returns>
        public static IEnumerable<ScxRunAsAccount> EnumerateScxRunAsAccount(IManagementGroupConnection managementGroupConnection)
        {
            List<ScxRunAsAccount> retVal;
            using (var factory = new SCXRunAsAccountFactory(managementGroupConnection))
            {
                retVal = factory.GetAllScxRunAsAccount().Select(DeserializeToScxRunAsAccount).ToList();
            }

            return retVal;
        }

        /// <summary>
        /// Checks if any of the secret data for the given credential was deserialized.
        /// </summary>
        /// 
        /// This method \i only checks if a credential has \i any unserialized secrets.
        /// If a credential has unserialized secrets, then it should be tested to see
        /// if it has valid, unserialized secrets.  A credential has valid unserialized
        /// secrets only if all the the \c SecureString properties (Passphrase and
        /// SshKey) are unserialized secrets.
        /// 
        /// <param name="credential">
        /// The credential to check.
        /// </param>
        /// 
        /// <returns>
        /// This method returns \c true if any of the secret data for \c credential
        /// was deserialized; false otherwise.
        /// </returns>
        /// 
        /// <seealso cref="AreSecretsValid"/>
        public static bool HasUnserializedSecret(PosixHostCredential credential)
        {
            return ReferenceEquals(credential.Passphrase, UnserializedSecret) ||
                   ReferenceEquals(credential.SshKey, UnserializedSecret);
        }

        /// <summary>
        /// Checks if \i all the secret data for the given credential is unserialized
        /// secrets or \i none of it is.
        /// </summary>
        /// 
        /// A credential has valid unserialized secrets only if all the the \c SecureString
        /// properties (Passphrase and SshKey) are unserialized secrets.
        /// 
        /// This method is more efficient if it is called after a call to \c HasUnserializedSecret.
        /// If \c HasUnserializedSecret returns false, this method doesn't even
        /// need to be called, since a credential with \i no unserialized secrets
        /// always has "valid" unserialized secrets.
        /// 
        /// <param name="credential">
        /// The credential to check.
        /// </param>
        /// 
        /// <returns>
        /// This method returns \c true if any of the secret data for \c credential
        /// was deserialized; false otherwise.
        /// </returns>
        /// 
        /// <seealso cref="HasUnserializedSecret"/>
        public static bool AreSecretsValid(PosixHostCredential credential)
        {
            return ReferenceEquals(credential.Passphrase, UnserializedSecret) ==
                   ReferenceEquals(credential.SshKey, UnserializedSecret);
        }

        /// <summary>
        /// Convert a ScxcredentialRef object to ScxRunAsAccount object.
        /// </summary>
        /// <param name="credentialRef">the ScxcredentialRef object.</param>
        /// <returns>The RunAs accounts.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", 
               Justification = "This would be possible to re-write using the temp var pattern but the effort is not justified.")]
        private static ScxRunAsAccount DeserializeToScxRunAsAccount(ScxCredentialRef credentialRef)
        {
            var retVal = new ScxRunAsAccount
                {
                    Credential = new CredentialSet(),
                    DisplayName = credentialRef.Name,
                    Description = credentialRef.Description,
                    LastModified = credentialRef.LastModified,
                    RunAsAccountId = new Guid(credentialRef.Key)
                };

            // During deserialization, this MUST be done before the actual
            // SshDiscovery credential is set or the property settor will
            // blow away the UnserializedSecret!!!
            retVal.Credential.IsSSHKey = credentialRef.IsSSHKey;

            retVal.Credential.Usage = CredentialSetUsage.Maintenance;
            if (credentialRef.Type == ScxCredentialRef.RunAsAccountType.Monitoring)
            {
                retVal.Credential.Usage = CredentialSetUsage.Monitoring;
            }

            var userName = credentialRef.UserName;

            var doc = new XmlDocument();
            doc.LoadXml(userName);

            var userid = doc.SelectSingleNode("//UserId");
            var elevationType = doc.SelectSingleNode("//Elev");

            Debug.Assert(userid != null && elevationType != null, "User name is not in expected XML format.");

            // wsman credential
            if (retVal.Credential.Usage == CredentialSetUsage.Monitoring)
            {
                PosixHostCredential wsManCredential = new PosixHostCredential
                    {
                        Usage = CredentialUsage.WsManDiscovery,
                        PrincipalName = userid.InnerText,
                        Passphrase = UnserializedSecret,
                        SshKey = UnserializedSecret,
                    };

                if (elevationType.InnerText == "sudo")
                {
                    wsManCredential.Usage |= CredentialUsage.WsmanSudoElevation;
                }

                retVal.Credential.Add(wsManCredential);

                return retVal;
            }

            // SSH credential
            PosixHostCredential sshCredential = new PosixHostCredential
                {
                    Usage = CredentialUsage.SshDiscovery,
                    PrincipalName = userid.InnerText,
                    Passphrase = UnserializedSecret,
                    SshKey = UnserializedSecret,
                };

            retVal.Credential.Add(sshCredential);

            // string.IsNullOrWhiteSpace(value)) is not available in .NET 3.5
            if (string.IsNullOrEmpty(elevationType.InnerText) || elevationType.InnerText.Trim().Length == 0)
            {
                // no SSH elevation
                sshCredential.Usage |= CredentialUsage.PrivilegedServiceLogin;
            }
            else if (elevationType.InnerText == "sudo")
            {
                // SSH SUDO elevation
                PosixHostCredential sshSudoCredential = new PosixHostCredential
                {
                    // SUDO has no secrets, so no need to set them to UnserializedSecret.
                    Usage = CredentialUsage.SshSudoElevation
                };

                retVal.Credential.Add(sshSudoCredential);
            }
            else if (elevationType.InnerText == "su")
            {
                // SSH SU elevation
                PosixHostCredential sshSuCredential = new PosixHostCredential
                {
                    Usage = CredentialUsage.SuElevation,
                    PrincipalName = "root",
                    Passphrase = UnserializedSecret,
                    SshKey = UnserializedSecret,
                };

                retVal.Credential.Add(sshSuCredential);
            }
            else
            {
                throw new InvalidCredentialException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.ScxRunAsAccountHelper_UnknownElevationType,
                        elevationType.InnerText));
            }

            return retVal;
        }

        /// <summary>
        /// Checks if any credentials in the account have any unserialized secrets.
        /// </summary>
        /// 
        /// If an account has unserialized secrets, then it should be tested to
        /// determine if it has valid, unserialized secrets.  An account has valid
        /// unserialized secrets only if all the associated credentials have \i only
        /// unserialized secrets.
        /// 
        /// <param name="account">
        /// The account to test.
        /// </param>
        /// 
        /// <returns>
        /// This method returns \c true if any of the credentials associated with
        /// an account have valid unserialized secrets.
        /// </returns>
        /// 
        /// <seealso cref="AreSecretsValid"/>
        private static bool HasUnserializedSecret(ScxRunAsAccount account)
        {
            return account.Credential.Credentials.Any(HasUnserializedSecret);
        }

        /// <summary>
        /// Checks if an account with at least one unserialized secret has only
        /// unserialized secrets.
        /// </summary>
        /// 
        /// This method assumes that \c HasUnserializedSecret was already used
        /// to determine one or more credentials in the account have unserialized
        /// secrets.
        /// 
        /// <param name="account">
        /// The account to test.
        /// </param>
        /// 
        /// <returns>
        /// This method returns \c true if all the credentials associated with
        /// an account have valid unserialized secrets.
        /// </returns>
        private static bool IsUnserializedSecretValid(ScxRunAsAccount account)
        {
            // SUDO elevation has no secrets, so if Usage == SshSudoElevation,
            // then the credential can be considered to have valid unserialized
            // secrets.  If a single credential is overloaded, for example, if
            // Usage == SshDiscovery | SshSudoElevation, then it must pass the
            // second, more rigorous conditions.
            return
                account.Credential.Credentials.All(
                    credential =>
                    ((credential.Usage == CredentialUsage.SshSudoElevation) ||
                     (ReferenceEquals(credential.Passphrase, UnserializedSecret) &&
                      ReferenceEquals(credential.SshKey, UnserializedSecret))));
        }

        private static readonly SecureString UnserializedSecret;

        // This method initializes the <see cref="UnserializedSecret"/> field
        // and sets the object instance's read-only attribute.
        static ScxRunAsAccountHelper()
        {
            UnserializedSecret = SecureStringHelper.Encrypt(@"UnserializedSecret");
            UnserializedSecret.MakeReadOnly();
        }
    }
}
