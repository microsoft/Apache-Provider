// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CmdletSupport.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Security;
    using System.Text;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Exceptions;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Properties;

    using SDKManagementGroupConnection = Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction.ManagementGroupConnection;
    using SystemCenterCoreConnection   = Microsoft.SystemCenter.Core.Connection.Connection;


    public class CmdletSupport
    {
        /// <summary>
        ///     Create a new SCX Monitor Run As account and add it to the database.
        /// </summary>
        /// <param name="systemCenterCoreConnection">
        ///     This is the SCManagementGroupConnection from the cmdlet.
        /// </param>
        /// <param name="name">
        ///     This is the name for the new Run As account.
        /// </param>
        /// <param name="description">
        ///     This is the description for the new Run As account.
        /// </param>
        /// <param name="username">
        ///     This is the name of the managed host user that will be associated
        ///     with the new Run As account.
        /// </param>
        /// <param name="authentication">
        ///     The authentication data for the managed host user.
        /// </param>
        /// <param name="secureDistribution">
        ///     The approved health services distribution.  A null distribution
        ///     indicates that all health services are approved, i.e. the "Less
        ///     Secure" option in the UI.  An empty list (i.e. <c>secureDistribution.Count() == 0</c>)
        ///     indicates that no health services are approved.  Otherwise, the
        ///     list should contain the entity ID for each health services approved
        ///     to use this Run As account.
        /// </param>
        /// <returns>
        ///     On success this method returns an IMonitorRunAsAccount that can
        ///     be used in PowerShell to inspect the non-secret data associated
        ///     with the new Run As account.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if <paramref name="name"/>, <paramref name="username"/>
        ///     or <paramref name="authentication"/> are <see langword="null"/>, or if
        ///     <paramref name="name"/> or <paramref name="username"/> is empty
        ///     or only contains whitespace.
        /// </exception>
        [CLSCompliantAttribute(false)]
        public static IMonitorRunAsAccount NewMonitorAccount(
            SystemCenterCoreConnection systemCenterCoreConnection,
            string                     name,
            string                     description,
            string                     username,
            IMonitorAuthenticationData authentication,
            IEnumerable<Guid>          secureDistribution)
        {
            Debug.Assert(
                systemCenterCoreConnection != null,
                @"A management group connection is required to create a new SCX Monitor Run As account!");

            MonitorRunAsAccount result = new MonitorRunAsAccount(name, description, username, authentication);

            using (var sdkManagementGroupConnection = GetSDKManagementGroupConnection(systemCenterCoreConnection))
            {
                ScxRunAsAccountSecureDistribution scxRunAsAccountSecureDistribution = new ScxRunAsAccountSecureDistribution(secureDistribution);

                ScxRunAsAccountHelper.CreateScxRunAsAccount(sdkManagementGroupConnection, result.RunAsAccount, scxRunAsAccountSecureDistribution);
            }

            return result;
        }


        /// <summary>
        ///     Updates an existing SCX Monitor Run As account in the database.
        /// </summary>
        /// <param name="systemCenterCoreConnection">
        ///     This is the SCManagementGroupConnection from the cmdlet.
        /// </param>
        /// <param name="runAsAccount">
        ///     An existing SCX Monitor Run As account that was previously
        ///     retrieved from the database.
        /// </param>
        /// <param name="secureDistribution">
        ///     The approved health services distribution.  A null distribution
        ///     indicates that all health services are approved, i.e. the "Less
        ///     Secure" option in the UI.  An empty list (i.e. <c>secureDistribution.Count() == 0</c>)
        ///     indicates that no health services are approved.  Otherwise, the
        ///     list should contain the entity ID for each health services approved
        ///     to use this Run As account.
        /// </param>
        /// <returns>
        ///     On success this method returns an IMonitorRunAsAccount that can
        ///     be used in PowerShell to inspect the non-secret data associated
        ///     with the new Run As account.
        /// </returns>
        [CLSCompliantAttribute(false)]
        public static IMonitorRunAsAccount SetMonitorAccount(
            SystemCenterCoreConnection systemCenterCoreConnection,
            IMonitorRunAsAccount       runAsAccount,
            IEnumerable<Guid>          secureDistribution)
        {
            Debug.Assert(
                systemCenterCoreConnection != null,
                @"A management group connection is required to modify an existing SCX Monitor Run As account!");

            using (var sdkManagementGroupConnection = GetSDKManagementGroupConnection(systemCenterCoreConnection))
            {
                ScxRunAsAccountSecureDistribution scxRunAsAccountSecureDistribution = new ScxRunAsAccountSecureDistribution(secureDistribution);

                ScxRunAsAccountHelper.UpdateScxRunAsAccount(
                    sdkManagementGroupConnection,
                    ((MonitorRunAsAccount)runAsAccount).RunAsAccount,
                    scxRunAsAccountSecureDistribution);
            }

            return runAsAccount;
        }


        /// <summary>
        ///     Create a new SCX Maintenance Run As account and add it to the database.
        /// </summary>
        /// <param name="systemCenterCoreConnection">
        ///     This is the SCManagementGroupConnection from the cmdlet.
        /// </param>
        /// <param name="name">
        ///     This is the name for the new Run As account.
        /// </param>
        /// <param name="description">
        ///     This is the description for the new Run As account.
        /// </param>
        /// <param name="username">
        ///     This is the name of the managed host user that will be associated
        ///     with the new Run As account.
        /// </param>
        /// <param name="authentication">
        ///     The authentication data for the managed host user.
        /// </param>
        /// <param name="secureDistribution">
        ///     The approved health services distribution.  A null distribution
        ///     indicates that all health services are approved, i.e. the "Less
        ///     Secure" option in the UI.  An empty list (i.e. <c>secureDistribution.Count() == 0</c>)
        ///     indicates that no health services are approved.  Otherwise, the
        ///     list should contain the entity ID for each health services approved
        ///     to use this Run As account.
        /// </param>
        /// <returns>
        ///     On success this method returns an IMaintenanceRunAsAccount that
        ///     can be used in PowerShell to inspect the non-secret data associated
        ///     with the new Run As account.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if <paramref name="name"/>, <paramref name="username"/>
        ///     or <paramref name="authentication"/> are <see langword="null"/>, or if
        ///     <paramref name="name"/> or <paramref name="username"/> is empty
        ///     or only contains whitespace.
        /// </exception>
        [CLSCompliantAttribute(false)]
        public static IMaintenanceRunAsAccount NewMaintenanceAccount(
            SystemCenterCoreConnection        systemCenterCoreConnection,
            string                            name,
            string                            description,
            string                            username,
            IMaintenanceAuthenticationData    authentication,
            IEnumerable<Guid>                 secureDistribution)
        {
            Debug.Assert(
                systemCenterCoreConnection != null,
                @"A management group connection is required to create a new SCX Maintenance Run As account!");

            MaintenanceRunAsAccount result = new MaintenanceRunAsAccount(name, description, username, authentication);

            using (var sdkManagementGroupConnection = GetSDKManagementGroupConnection(systemCenterCoreConnection))
            {
                ScxRunAsAccountSecureDistribution scxRunAsAccountSecureDistribution = new ScxRunAsAccountSecureDistribution(secureDistribution);

                ScxRunAsAccountHelper.CreateScxRunAsAccount(sdkManagementGroupConnection, result.RunAsAccount, scxRunAsAccountSecureDistribution);
            }

            return result;
        }


        /// <summary>
        ///     Updates an existing SCX Maintenance Run As account in the database.
        /// </summary>
        /// <param name="systemCenterCoreConnection">
        ///     This is the SCManagementGroupConnection from the cmdlet.
        /// </param>
        /// <param name="runAsAccount">
        ///     An existing SCX Maintenance Run As account that was previously
        ///     retrieved from the database.
        /// </param>
        /// <param name="secureDistribution">
        ///     The approved health services distribution.  A null distribution
        ///     indicates that all health services are approved, i.e. the "Less
        ///     Secure" option in the UI.  An empty list (i.e. <c>secureDistribution.Count() == 0</c>)
        ///     indicates that no health services are approved.  Otherwise, the
        ///     list should contain the entity ID for each health services approved
        ///     to use this Run As account.
        /// </param>
        /// <returns>
        ///     On success this method returns an IMaintenanceRunAsAccount that
        ///     can be used in PowerShell to inspect the non-secret data associated
        ///     with the new Run As account.
        /// </returns>
        [CLSCompliantAttribute(false)]
        public static IMaintenanceRunAsAccount SetMaintenanceAccount(
            SystemCenterCoreConnection systemCenterCoreConnection,
            IMaintenanceRunAsAccount   runAsAccount,
            IEnumerable<Guid>          secureDistribution)
        {
            Debug.Assert(
                systemCenterCoreConnection != null,
                @"A management group connection is required to modify an existing SCX Maintenance Run As account!");

            using (var sdkManagementGroupConnection = GetSDKManagementGroupConnection(systemCenterCoreConnection))
            {
                ScxRunAsAccountSecureDistribution scxRunAsAccountSecureDistribution = new ScxRunAsAccountSecureDistribution(secureDistribution);

                ScxRunAsAccountHelper.UpdateScxRunAsAccount(
                    sdkManagementGroupConnection,
                    ((MaintenanceRunAsAccount)runAsAccount).RunAsAccount,
                    scxRunAsAccountSecureDistribution);
            }

            return runAsAccount;
        }


        /// <summary>
        ///     Gets the SCX Run As account with the given <paramref name="objectId"/>.
        /// </summary>
        /// <param name="systemCenterCoreConnection">
        ///     This is the SCManagementGroupConnection from the cmdlet.
        /// </param>
        /// <param name="objectId">
        ///     The object identifier for the SCX Run As account to lookup.
        /// </param>
        /// <returns>
        ///     This method returns the SCX Run As account with the given
        ///     <paramref name="objectId"/>.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if the object associated with <paramref name="objectId"/>
        ///     is not a valid SCX Run As account.
        /// </exception>
        [CLSCompliantAttribute(false)]
        public static IScxRunAsAccount GetScxRunAsAccount(SystemCenterCoreConnection systemCenterCoreConnection, Guid objectId)
        {
            Debug.Assert(
                systemCenterCoreConnection != null,
                @"A management group connection is required to lookup an SCX Run As account!");

            ScxRunAsAccount scxRunAsAccount;

            using (var sdkManagementGroupConnection = GetSDKManagementGroupConnection(systemCenterCoreConnection))
            {
                // the following method will throw ArgumentException if objectId is
                // not associated with a managed object that maps to an ScxCredentialRef
                scxRunAsAccount = ScxRunAsAccountHelper.GetScxRunAsAccount(sdkManagementGroupConnection, objectId);
            }

            switch (scxRunAsAccount.Credential.Usage)
            {
                case CredentialSetUsage.Monitoring:
                    return new MonitorRunAsAccount(scxRunAsAccount);

                case CredentialSetUsage.Maintenance:
                    return new MaintenanceRunAsAccount(scxRunAsAccount);

                default:
                    throw new ArgumentException(
                        string.Format(CultureInfo.CurrentCulture, Resources.ScxRunAsPSSupport_BadScxRunAsAccountId, objectId), @"objectId");
            }
        }


        /// <summary>
        ///     Enumerates all the SCX Run As accounts.
        /// </summary>
        /// <param name="systemCenterCoreConnection">
        ///     This is the SCManagementGroupConnection from the cmdlet.
        /// </param>
        /// <returns>
        ///     An enumeration of all the SCX Run As accounts.
        /// </returns>
        [CLSCompliantAttribute(false)]
        public static IEnumerable<IScxRunAsAccount> EnumerateScxRunAsAccounts(SystemCenterCoreConnection systemCenterCoreConnection)
        {
            Debug.Assert(
                systemCenterCoreConnection != null,
                @"A management group connection is required to enumerate the SCX Run As accounts!");

            IEnumerable<ScxRunAsAccount> coreRepresentationList;

            using (var sdkManagementGroupConnection = GetSDKManagementGroupConnection(systemCenterCoreConnection))
            {
                coreRepresentationList = ScxRunAsAccountHelper.EnumerateScxRunAsAccount(sdkManagementGroupConnection);
            }

            List<IScxRunAsAccount> result = new List<IScxRunAsAccount>(coreRepresentationList.Count());

            foreach (ScxRunAsAccount coreRunAs in coreRepresentationList)
            {
                switch (coreRunAs.Credential.Usage)
                {
                    case CredentialSetUsage.Monitoring:
                        result.Add(new MonitorRunAsAccount(coreRunAs));
                        break;

                    case CredentialSetUsage.Maintenance:
                        result.Add(new MaintenanceRunAsAccount(coreRunAs));
                        break;

                    default:
                        Debug.Assert(
                            false,
                            string.Format(
                                "The SCX Run As account '{0}' contains an unexpected meta-type: {1}.",
                                coreRunAs.RunAsAccountId,
                                coreRunAs.Credential.Usage));
                        break;
                }
            }

            return result;
        }


        /// <summary>
        ///     Removes the SCX Run As account with the given <paramref name="objectId"/>.
        /// </summary>
        /// <param name="systemCenterCoreConnection">
        ///     This is the SCManagementGroupConnection from the cmdlet.
        /// </param>
        /// <param name="objectId">
        ///     The object identifier for the SCX Run As account to remove.
        /// </param>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if the object associated with <paramref name="objectId"/>
        ///     is not a valid SCX Run As account.
        /// </exception>
        [CLSCompliantAttribute(false)]
        public static void RemoveScxRunAsAccount(SystemCenterCoreConnection systemCenterCoreConnection, Guid objectId)
        {
            Debug.Assert(
                systemCenterCoreConnection != null,
                @"A management group connection is required to remove an SCX Run As account!");

            using (var sdkManagementGroupConnection = GetSDKManagementGroupConnection(systemCenterCoreConnection))
            {
                // the following method will throw ArgumentException if objectId is
                // not associated with a managed object that maps to an ScxCredentialRef
                ScxRunAsAccountHelper.DeleteScxRunAsAccount(sdkManagementGroupConnection, objectId);
            }
        }


        /// <summary>
        ///     Get the CrossPlatform SDK Abstraction layer wrapper for an SCManagementGroupConnection.
        /// </summary>
        /// <param name="systemCenterCoreConnection">
        ///     This is the SCManagementGroupConnection from the cmdlet.
        /// </param>
        /// <returns>
        ///     This method returns a Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction.ManagementGroupConnection
        ///     wrapper for a Microsoft.EnterpriseManagement.ManagementGroup (OM layer) from a given Microsoft.SystemCenter.Core.Connection.Connection.
        /// </returns>
        private static SDKManagementGroupConnection GetSDKManagementGroupConnection(SystemCenterCoreConnection systemCenterCoreConnection)
        {
            return new SDKManagementGroupConnection(systemCenterCoreConnection.GetService<EnterpriseManagement.ManagementGroup>());
        }

        /// <summary>
        ///     Checks if the value string is composed entirely of characters
        ///     that belong to the ASCII encoding, i.e. characters limited to
        ///     the lowest 128 Unicode characters (U+0000 to U+007F).
        /// </summary>
        /// <param name="value">
        ///     The string to check.
        /// </param>
        /// <param name="paramName">
        ///     The name of the parameter being checked.
        /// </param>
        public static void CheckAuthenticationDataEncoding(string value, string paramName)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            Encoding enc = Encoding.GetEncoding("us-ascii", new EncoderExceptionFallback(), new DecoderExceptionFallback());

            try
            {
                // this will throw EncoderFallbackException if there are any
                // characters that are not members of the ASCII encoding
                // character set
                enc.GetBytes(value);
            }
            catch (EncoderFallbackException encoderExceptionFallback)
            {
                throw new IllegalAuthenticationCharacterException(paramName, encoderExceptionFallback.Index);
            }
        }


        /// <summary>
        ///     Checks if the value string is composed entirely of characters
        ///     that belong to the ASCII encoding, i.e. characters limited to
        ///     the lowest 128 Unicode characters (U+0000 to U+007F).
        /// </summary>
        /// <param name="value">
        ///     The secure string to check.
        /// </param>
        /// <param name="paramName">
        ///     The name of the parameter being checked.
        /// </param>
        public static void CheckAuthenticationDataEncoding(SecureString value, string paramName)
        {
            CheckAuthenticationDataEncoding(SecureStringHelper.Decrypt(value), paramName);
        }
   }
}