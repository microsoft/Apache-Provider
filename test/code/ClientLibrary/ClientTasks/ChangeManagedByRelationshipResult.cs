//-----------------------------------------------------------------------
// <copyright file="ChangeManagedByRelationshipResult.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks.Properties;

    /// <summary>
    ///     Result object indicating the state of a ChangeManagedByRelationship operation. This is simple DTO returned
    ///     by the cmdlet to the shell.
    /// </summary>
    public class ChangeManagedByRelationshipResult
    {
        #region Lifecycle

        /// <summary>
        ///     Constructs an instance of the result object.
        /// </summary>
        /// <param name="agent">The PersistedUnixComputer of the system to change.</param>
        /// <param name="changeException">
        ///     If the ChangeManagedByRelationship had failed, this contains the exception details. The operations
        ///     is assumed to have succeeded if this value is null.
        /// </param>
        public ChangeManagedByRelationshipResult(IPersistedUnixComputer agent, Exception changeException)
        {
            if (agent == null)
            {
                throw new ArgumentNullException("agent", Resources.ChangeManagedByResult_AgentNull);
            }

            Agent = agent;
            ErrorData = changeException;
        }

        #endregion Lifecycle

        #region Properties

        /// <summary>
        ///     The hostname to which this result pertains.
        /// </summary>
        public IPersistedUnixComputer Agent { get; set; }

        /// <summary>
        ///     Returns the uninstallation success or failure. The absence of any error data
        ///     indicates success.
        /// </summary>
        public bool Succeeded
        {
            get
            {
                return null == ErrorData;
            }
        }

        /// <summary>
        ///     Details about error, if any. Can be null.
        /// </summary>
        public Exception ErrorData { get; set; }

        #endregion Properties
    }
}