//-----------------------------------------------------------------------
// <copyright file="ScxTypes.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>a-brdust</author>
// <description></description>
// <history>5/13/2009 11:09:42 AM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Common
{
    using System;
    using System.Text;

    /// <summary>
    /// Delegate allowing output to the log file without having to accept an external class instance such as IContext.
    /// </summary>
    /// <param name="logMsg">Log message</param>
    public delegate void ScxLogDelegate(string logMsg);

    /// <summary>
    /// Delegate for accessing the a value within IContext Records even if called from a location in the code lacking
    /// MCF support
    /// </summary>
    /// <param name="key">The MCF record key</param>
    /// <returns>The MCF record value</returns>
    public delegate string ScxContextValueDelegate(string key);

    /// <summary>
    /// The severity level of a Security Event Log (SEL) entry
    /// </summary>
    public enum ScxEventLevel
    {
        /// <summary>
        /// Information level
        /// </summary>
        Information = 0,

        /// <summary>
        /// Warning level
        /// </summary>
        Warning,

        /// <summary>
        /// Error level
        /// </summary>
        Error,

        /// <summary>
        /// Success Audit - Server 2008 levels
        /// </summary>
        SuccessAudit,

        /// <summary>
        /// Failure Audit Server 2008 levels
        /// </summary>
        FailureAudit
    }

    /// <summary>
    /// An abstraction of an Event Log entry which can be used for various types of underlying event logs.
    /// </summary>
    public class ScxEventLogEntry
    {
        /// <summary>
        /// Gets or sets Creation Date of the Event Log Entry
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets  ScxEventLevel level 
        /// </summary>
        public ScxEventLevel Level { get; set; }

        /// <summary>
        /// Gets or sets  Event type ID
        /// </summary>
        public long EventTypeID { get; set; }

        /// <summary>
        /// Gets or sets  Event log entry category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets User created the EventLog Entry
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Gets or sets Computer name
        /// </summary>
        public string Computer { get; set; }

        /// <summary>
        /// Gets or sets  General-purpose message contained in the Windows data structure.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Return a human-readable listing of the event log entry contents
        /// </summary>
        /// <returns>human-readable listing of the event log entry contents</returns>
        public override string ToString()
        {
            StringBuilder outputBuilder = new StringBuilder();

            outputBuilder.AppendFormat("Date:             '{0}'\n", this.CreationDate);
            outputBuilder.AppendFormat("Level:            '{0}'\n", this.Level);
            outputBuilder.AppendFormat("TypeID:           '{0}'\n", this.EventTypeID);
            outputBuilder.AppendFormat("Category:         '{0}'\n", this.Category);
            outputBuilder.AppendFormat("User:             '{0}'\n", this.User);
            outputBuilder.AppendFormat("Computer:         '{0}'\n", this.Computer);
            outputBuilder.AppendFormat("Message:          '{0}'\n", this.Message);

            return outputBuilder.ToString();
        }
    }
}