//-----------------------------------------------------------------------
// <copyright file="UninstallAgentResult.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System;

    /// <summary>
    ///     Result object indicating the state of an uninstall. This is simple
    ///     data transfer object returned by the cmdlet to the shell.
    /// </summary>
    public class UninstallAgentResult
    {
        #region Lifecycle

        /// <summary>
        ///     Instantiates this object with the given managed host.
        /// </summary>
        /// <param name="unixComputer">The managed host targeted for uninstall.</param>
        public UninstallAgentResult(IPersistedUnixComputer unixComputer)
        {
            if (unixComputer == null)
            {
                throw new ArgumentNullException("unixComputer");
            }

            UnixComputer = unixComputer;
        }

        #endregion Lifecycle

        #region Properties

        /// <summary>
        ///     The Unix computer targeted for uninstall.
        /// </summary>
        public IPersistedUnixComputer UnixComputer { get; private set; }

        /// <summary>
        ///     Returns the uninstall success or failure. The absence of any
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
        ///     This property is null if the uninstall completed successfully;
        ///     otherwise it contains an exception with details on the cause of
        ///     the failure.  When the cause of the exception occurred on the
        ///     managed host agent, the exception data might include information
        ///     from the host-side command such as a POSIX error code and text
        ///     from the standard output and error streams.
        /// </summary>
        public Exception ErrorData { get; set; }

        #endregion Properties
    }
}
