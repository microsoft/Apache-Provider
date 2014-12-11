//-----------------------------------------------------------------------
// <copyright file="UpdateResult.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System;
    using System.Diagnostics;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks.Properties;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.Utilities;

    public enum UpdateAction
    {
        MissingVersionData,
        RequiresUpdate,
        AlreadyUpToDate,
        HigherVersionInstalled
    }

    /// <summary>
    /// Holds information about the results of a host update.
    /// Used to display to the user what the result is.  To simplify handling this, we'll stick with simple data structures.
    /// </summary>
    public class UpdateResult
    {
        #region Types


        #endregion Types

        #region Lifecycle

        /// <summary>
        ///     Instantiates this object with the given hostname and credentials.
        /// </summary>
        /// <param name="hostname">The name of the server.</param>
        public UpdateResult(string hostname)
        {
            if (String.IsNullOrWhiteSpace(hostname))
            {
                throw new ArgumentNullException("hostname", Resources.UpdateResult_UpdateResult_A_hostname_must_be_specified);
            }

            Hostname = hostname;
        }

        #endregion Lifecycle

        #region Properties

        /// <summary>
        ///     Hostname or IP address of the managed server targeted for update.
        /// </summary>
        public string Hostname { get; private set; }

        /// <summary>
        ///     Computer type as defined in the Management Pack.
        /// </summary>
        public string ComputerType { get; set; }

        /// <summary>
        ///     <code>true</code> if the managed server is running Linux;
        ///     <code>false</code> otherwise.
        /// </summary>
        public bool IsLinux
        {
            get
            {
                return ComputerType.Contains(".Linux");
            }
        }

        /// <summary>
        ///     Starting version of the SCX agent prior to applying any update actions.
        /// </summary>
        public UnixAgentVersion StartVersion { get; set; }

        /// <summary>
        ///     Version of the SCX agent available for install/update.
        /// </summary>
        /// 
        /// <note>
        ///     If this property is equal to the StartVersion than targeted server
        ///     is already using the most current version and no further action is
        ///     required, i.e. Action is AlreadyUpToDate.
        /// </note>
        public UnixAgentVersion InstallableVersion { get; set; }

        /// <summary>
        ///     Path to the installable SCX agent kit.
        /// </summary>
        public string InstallableKit { get; set; }

        /// <summary>
        ///     Action attempted by the update workflow.
        /// </summary>
        public UpdateAction Action
        {
            get
            {
                UpdateAction result = UpdateAction.MissingVersionData;

                if ((StartVersion != null) && (InstallableVersion != null))
                {
                    if (StartVersion == InstallableVersion)
                    {
                        result = UpdateAction.AlreadyUpToDate;
                    }
                    else if (StartVersion < InstallableVersion)
                    {
                        result = UpdateAction.RequiresUpdate;
                    }
                    else
                    {
                        result = UpdateAction.HigherVersionInstalled;
                    }
                }

                return result;
            }
        }

        /// <summary>
        ///     Returns the update success or failure. The absence of any error
        ///     data indicates success.
        /// </summary>
        public bool Succeeded
        {
            get
            {
                return null == ErrorData;
            }
        }

        /// <summary>
        ///     If this property is not null then it contains the exception that
        ///     caused the update to fail.  This may also contain UNIX error codes,
        ///     standard output, and standard error.
        /// </summary>
        public Exception ErrorData { get; set; }

        #endregion Properties
    }
}