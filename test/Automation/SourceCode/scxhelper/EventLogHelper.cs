//-----------------------------------------------------------------------
// <copyright file="EventLogHelper.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>a-brdust</author>
// <description></description>
// <history>5/13/2009 11:09:42 AM: Created
//          2/18/2010 3:57 PM Renamed and updated latest changes  a-srsant
// </history>
//-----------------------------------------------------------------------

namespace Scx.Test.Common
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Eventing.Reader;
    using System.Security.Principal;

    /// <summary>
    /// Helper class to retrieve and filter events from the Windows Security Event Log
    /// </summary>
    public class EventLogHelper
    {
        /// <summary>
        /// Whether to use System.Diagnostics.Eventing.Reader; this is available on Server 2008, but not on Server 2003 and earlier.
        /// </summary>
        private bool useEventingReader;

        /// <summary>
        /// Instance of the .Net EventLog class for accessing the Security Event Log (Used when System.Diagnostics.Eventing.Reader is unavailable)
        /// </summary>
        private EventLog legacyEventLog;

        /// <summary>
        /// The event log to access, for example, 'Security', 'Application', or 'Operations Manager'
        /// </summary>
        private string eventLogName;

        /// <summary>
        /// Log delegate method
        /// </summary>
        private ScxLogDelegate logger = ScxMethods.ScxNullLogDelegate;

        /// <summary>
        /// Initializes a new instance of the EventLogHelper class
        /// </summary>
        /// <param name="logger">instance of ScxLogDelegate</param>
        /// <param name="eventLogName">Event Log Name</param>
        public EventLogHelper(ScxLogDelegate logger, string eventLogName)
        {
            this.logger = logger;

            this.eventLogName = eventLogName;

            this.logger(string.Format("OS Platform: {0}  Version: {1} VersionMajor: {2}", System.Environment.OSVersion.Platform, System.Environment.OSVersion.VersionString, Environment.OSVersion.Version.Major));

            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                throw new PlatformNotSupportedException("SELReadHelper only supported on windows server platform (Environment.OSVersion.Platform == PlatformID.Win32NT)");
            }

            // Use old style NT EventLog (Windows 2003 Server and earlier)
            if (Environment.OSVersion.Version.Major <= 5)
            {
                this.useEventingReader = false;
                this.legacyEventLog = new EventLog(this.eventLogName);
            }
            else
            {
                // Use new style NT EventLog (Windows 2008 Server and later)
                this.useEventingReader = true;
            }
        }

        #region public methods

        /// <summary>
        /// Retrieve a complete list of events from the Security Event Log with earliest creation time 
        /// with the given log entry type (warning, error, etc.) and event ID
        /// </summary>
        /// <param name="earliestCreationTime">Earliest Creation time of the log entry</param>
        /// <param name="entryLevel">The log entry level (warning, error, success audit, etc)</param>
        /// <param name="eventID">The ID of the event type</param>
        /// <param name="eventLogName">The name of event log category (Security, Operations Manager, etc)</param>
        /// <returns>A list of ScxEventLogEntry objects</returns>       
        public List<ScxEventLogEntry> GetEvents(DateTime earliestCreationTime, ScxEventLevel entryLevel, long eventID, string eventLogName = "Security")
        {
            this.logger("EntryLevel: " + entryLevel.ToString());
            this.logger("EventID: " + eventID.ToString());

            List<ScxEventLogEntry> entries = this.GetEvents(earliestCreationTime, eventID, eventLogName);

            List<ScxEventLogEntry> results = new List<ScxEventLogEntry>();

            foreach (ScxEventLogEntry entry in entries)
            {
                if (entry.Level == entryLevel)
                {
                    results.Add(entry);
                }
            }

            return results;
        }

        /// <summary>
        /// Retrieve a complete list of events from the Event Log younger than the given maximum age, 
        /// with the given log event ID
        /// </summary>
        /// <param name="earliestCreationTime">The earliest creation time of the eventlog entry</param>
        /// <param name="eventID">The ID of the event type</param>
        /// <param name="eventLogName">The name of event log category (Security, Operations Manager, etc)</param>
        /// <returns>A list of ScxEventLogEntry objects</returns>
        public List<ScxEventLogEntry> GetEvents(DateTime earliestCreationTime, long eventID, string eventLogName = "Security")
        {
            this.logger("EventID: " + eventID.ToString());

            List<ScxEventLogEntry> entries = new List<ScxEventLogEntry>();

            if (this.useEventingReader)
            {
                entries = this.GetEventingReaderEvents(eventID, earliestCreationTime, eventLogName);
            }
            else
            {
                entries = this.GetLegacyEvents(eventID, earliestCreationTime);
            }

            return entries;
        }

        /// <summary>
        /// Writing Events into Windows Event Log : This Currently tested only for 2003
        /// </summary>
        /// <param name="logName">EventLog Name</param>
        /// <param name="eventSource">Event Source</param>
        /// <param name="eventMessage">Event Message</param>
        /// <param name="eventId">ID of the event</param>
        /// <param name="entryLevel">Event Level(Error/Warning/Info etc)</param>
        public void WriteEvent(string logName, string eventSource, string eventMessage, int eventId, ScxEventLevel entryLevel)
        {
            if (!EventLog.Exists(logName))
            {
                throw new ArgumentException(string.Format("EventLog:{0} doesnot exist in the Local Computer", logName));
            }

            try
            {
                if (!EventLog.SourceExists(eventSource))
                {
                    EventLog.CreateEventSource(eventSource, logName);
                }

                EventLog eventLog = new EventLog(logName, ".", eventSource);
                eventLog.WriteEntry(eventMessage, this.ScxEventLevelToEntryType(entryLevel), eventId);
            }
            catch (Exception ex)
            {
                this.logger("Exception in writing into EventLog: " + ex.Message);
            }
        }

        /// <summary>
        /// Convert the given EventType used for SCX ACS into a ScxEventLevel (eventlog log level).
        /// </summary>
        /// <param name="eventType">the integer EventType</param>
        /// <returns>An ScxSELLevel object providing an abstraction of the WindowsXP/W2K3 SEL log level and the Windows Vista/W2K8 log level</returns>
        public ScxEventLevel GetEventType(uint eventType)
        {
            switch (eventType)
            {
                case 0:
                    return ScxEventLevel.FailureAudit;
                case 1:
                    return ScxEventLevel.SuccessAudit;
                case 2:
                    return ScxEventLevel.Error;
                default:
                    throw new Exception("Invalid eventtype.  EventType must in range [0,1,2].  Actual: " + eventType);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Retrieve a complete list of events from the Security Event Log younger than the given maximum age.  
        /// </summary>
        /// <param name="eventID">The ID of the event type</param>
        /// <param name="earliestCreationTime">The earliest creation time of the eventlog entry</param>
        /// <returns>A list of ScxEventLogEntry objects</returns>
        private List<ScxEventLogEntry> GetLegacyEvents(long eventID, DateTime earliestCreationTime)
        {
            List<ScxEventLogEntry> results = new List<ScxEventLogEntry>();

            EventLogEntryCollection eventLogEntries = null;

            DateTime earliestCreationTimeFound = DateTime.Now;

            try
            {
                eventLogEntries = this.legacyEventLog.Entries;
            }
            catch (Exception ex)
            {
                this.logger("Exception in retrieving EventLogEntryCollection: " + ex.Message);
                return results;
            }

            int eventLogEntryCount = 0;
            int defunctEventLogEntryCount = 0;

            try
            {
                eventLogEntryCount = eventLogEntries.Count;
            }
            catch (Exception e)
            {
                this.logger(string.Format("GetLegacyEvents('{0}'): Failed to read count of event log entries(returning empty list): '{1}'", earliestCreationTime, e.Message));
                return results;
            }

            for (int i = 0; i < eventLogEntryCount; i++)
            {
                try
                {
                    EventLogEntry eventLogEntry = eventLogEntries[i];

                    if (eventLogEntry.TimeGenerated < earliestCreationTimeFound)
                    {
                        earliestCreationTimeFound = eventLogEntry.TimeGenerated;
                    }

                    if (eventID == eventLogEntry.InstanceId &&
                        eventLogEntry.TimeGenerated >= earliestCreationTime)
                    {
                        ScxEventLogEntry scxEntry = new ScxEventLogEntry();
                        scxEntry.CreationDate = eventLogEntry.TimeGenerated;
                        scxEntry.Level = this.EntryTypeToScxEventLevel(eventLogEntry.EntryType);
                        scxEntry.EventTypeID = eventLogEntry.InstanceId;
                        scxEntry.Category = eventLogEntry.Category;
                        scxEntry.User = eventLogEntry.UserName;
                        scxEntry.Computer = eventLogEntry.MachineName;
                        scxEntry.Message = eventLogEntry.Message;
                        results.Add(scxEntry);
                    }
                }
                catch (Exception)
                {
                    defunctEventLogEntryCount++;
                }
            }

            if (defunctEventLogEntryCount > 0)
            {
                this.logger(string.Format("GetLegacyEvents('{0}'): {1}/{2} events could not be read (possibly deleted during enumeration)", earliestCreationTime, defunctEventLogEntryCount, eventLogEntryCount));
            }

            this.logger(string.Format("Maximum event age found: {0}", DateTime.Now - earliestCreationTimeFound));

            return results;
        }

        /// <summary>
        /// Retrieve a complete list of events from the Security Event Log matching the given eventID.
        /// </summary>
        /// <param name="eventID">The ID of the event type</param>
        /// <param name="earliestCreationTime">The earliest creation time of the eventlog entry</param>
        /// <returns>A list of ScxEventLogEntry objects</returns>
        private List<ScxEventLogEntry> GetEventingReaderEvents(long eventID, DateTime earliestCreationTime, string eventLogName)
        {
            List<ScxEventLogEntry> results = new List<ScxEventLogEntry>();
            string queryString = "*[System/EventID=" + eventID + "]";
            EventLogQuery securityEventsQuery = new EventLogQuery(eventLogName, PathType.LogName, queryString);
            EventLogReader logReader = new EventLogReader(securityEventsQuery);

            DateTime earliestCreationTimeFound = DateTime.Now;

            for (EventRecord eventInstance = logReader.ReadEvent();
                null != eventInstance; eventInstance = logReader.ReadEvent())
            {
                if ((DateTime)eventInstance.TimeCreated < earliestCreationTimeFound)
                {
                    earliestCreationTimeFound = (DateTime)eventInstance.TimeCreated;
                }

                if ((DateTime)eventInstance.TimeCreated >= earliestCreationTime)
                {
                    ScxEventLogEntry scxEntry = new ScxEventLogEntry();
                    scxEntry.CreationDate = (DateTime)eventInstance.TimeCreated;
                    scxEntry.Level = this.EventLevelToSELLevel(eventInstance.LevelDisplayName, eventInstance.KeywordsDisplayNames);
                    scxEntry.EventTypeID = (long)eventInstance.Id;
                    scxEntry.Category = eventInstance.TaskDisplayName;
                    scxEntry.User = this.GetUserNameFromSID(eventInstance.UserId);
                    scxEntry.Computer = eventInstance.MachineName;
                    scxEntry.Message = eventInstance.FormatDescription();
                    results.Add(scxEntry);
                }
            }

            this.logger(string.Format("Maximum event age found: {0}", DateTime.Now - earliestCreationTimeFound));

            return results;
        }

        /// <summary>
        /// Converts the EventLogEntryType enumeration used on Server 2003 and before into the SCXSELLevel enumeration.  
        /// The two enumerations contain the exact same set of possible values.
        /// </summary>
        /// <param name="entryType">The Server 2003 enumeration (EventLogEntryType</param>
        /// <returns>The ScxSELLevel enumeration</returns>
        private ScxEventLevel EntryTypeToScxEventLevel(EventLogEntryType entryType)
        {
            switch (entryType)
            {
                case EventLogEntryType.Information:
                    return ScxEventLevel.Information;
                case EventLogEntryType.Warning:
                    return ScxEventLevel.Warning;
                case EventLogEntryType.Error:
                    return ScxEventLevel.Error;
                case EventLogEntryType.SuccessAudit:
                    return ScxEventLevel.SuccessAudit;
                case EventLogEntryType.FailureAudit:
                    return ScxEventLevel.FailureAudit;
                default:
                    throw new ArgumentException("No ScxEventLogEntryType case for " + entryType.ToString());
            }
        }

        /// <summary>
        /// Converts the SCXSELLevel to EventLogEntryType enumeration used on Server 2003.  
        /// The two enumerations contain the exact same set of possible values.
        /// </summary>
        /// <param name="entryLevel">The ScxSELLevel enumeration</param>
        /// <returns>The Server 2003 enumeration (EventLogEntryType)</returns>
        private EventLogEntryType ScxEventLevelToEntryType(ScxEventLevel entryLevel)
        {
            switch (entryLevel)
            {
                case ScxEventLevel.Information:
                    return EventLogEntryType.Information;
                case ScxEventLevel.Warning:
                    return EventLogEntryType.Warning;
                case ScxEventLevel.Error:
                    return EventLogEntryType.Error;
                case ScxEventLevel.SuccessAudit:
                    return EventLogEntryType.SuccessAudit;
                case ScxEventLevel.FailureAudit:
                    return EventLogEntryType.FailureAudit;
                default:
                    throw new ArgumentException("No matching ScxEventLevel case for " + entryLevel.ToString());
            }
        }

        /// <summary>
        /// Convert the information in Server 2008-style event logs corresponding to log level 
        /// into a value for the ScxSELLevel enumeration (warning, error, etc).  Server 2008 uses a combination
        /// of a list of keywords and a level to represent this.
        /// </summary>
        /// <param name="levelString">A level string.  This is the display string for log level in Server 2008 events for a value internally represented as a byte</param>
        /// <param name="keywords">An enumerable collection of keywords.  These are the display keywords from Server 2008 events; internally in Server 2008 these are represented as a bitmask</param>
        /// <returns>The ScxSELLevel enumeration (Warning, Error, etc)</returns>
        private ScxEventLevel EventLevelToSELLevel(string levelString, IEnumerable<string> keywords)
        {
            string keyword = string.Empty;

            foreach (string key in keywords)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    keyword = key;
                    break;
                }
            }

            if (keyword == "Audit Success")
            {
                return ScxEventLevel.SuccessAudit;
            }
            else if (keyword == "Audit Failure")
            {
                return ScxEventLevel.FailureAudit;
            }
            else if (levelString == "Warning")
            {
                return ScxEventLevel.Warning;
            }
            else if (levelString == "Error")
            {
                return ScxEventLevel.Error;
            }
            else
            {
                return ScxEventLevel.Information;
            }
        }

        /// <summary>
        /// Convert a SID into a text username, NT style (DOMAIN\\user), or an empty string if the SID is null or invalid.
        /// </summary>
        /// <param name="sid">An object representing the SID</param>
        /// <returns>The user name</returns>
        private string GetUserNameFromSID(SecurityIdentifier sid)
        {
            if (sid == null)
            {
                return string.Empty;
            }

            NTAccount account = (NTAccount)sid.Translate(typeof(NTAccount));
            return account.ToString();
        }

        #endregion
    }
}
