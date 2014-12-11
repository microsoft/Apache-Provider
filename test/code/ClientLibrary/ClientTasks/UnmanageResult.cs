//-----------------------------------------------------------------------
// <copyright file="UnmanageResult.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks.Properties;

    /// <summary>
    ///     Result object indicating the state of an unmange action. This is a
    ///     simple data transfer object returned by the cmdlet to the shell.
    /// </summary>
    public class UnmanageResult
    {
        /// <summary>
        ///     Instantiates this object with the given hostname.
        /// </summary>
        /// <param name="agent">The IPersistedUnixComputer to unmanage.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public UnmanageResult(IPersistedUnixComputer agent)
        {
            if (agent == null)
            {
                throw new ArgumentNullException("agent", Resources.UnmanageResult_Agent_Not_Specified);
            }

            Agent = agent;
        }

        /// <summary>
        ///     Hostname of the managed server targeted by unmanage.
        /// </summary>
        public IPersistedUnixComputer Agent { get; private set; }

        /// <summary>
        ///     Returns the unmanage success or failure. The absence of any
        ///     error data indicates success.
        /// </summary>
        public bool Succeeded
        {
            get
            {
                return null == ErrorData;
            }
        }

        /// <summary>
        ///     This property is null if the unmanage completed successfully;
        ///     otherwise it contains an exception with details on the cause of
        ///     the failure.  When the cause of the exception occurred on the
        ///     managed host agent, the exception data might include information
        ///     from the host-side command such as a POSIX error code and text
        ///     from the standard output and error streams.
        /// </summary>
        public Exception ErrorData { get; set; }
    }
}