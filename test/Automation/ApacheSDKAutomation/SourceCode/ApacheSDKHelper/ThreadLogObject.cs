//-----------------------------------------------------------------------
// <copyright file="ThreadLogObject.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>a-brdust</author>
// <description></description>
// <history>7/28/2009 3:54:47 PM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Apache.SDK.ApacheSDKHelper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Text;
    using System.Threading;
    using Microsoft.EnterpriseManagement.Configuration;
    using Microsoft.EnterpriseManagement.Monitoring;
    using Scx.Test.Common;

    /// <summary>
    /// This object is used for creating an individual log file for the status of discovery/testing against a single stress client.
    /// The use of a number of these objects allows logging in parallel as tests are run in parallel.
    /// </summary>
    public class ThreadLogObject
    {
        /// <summary>
        /// Path of discovered client log
        /// </summary>
        private string logPath = "client_logs";

        /// <summary>
        /// Initializes a new instance of the ThreadLogObject class.
        /// </summary>
        /// <param name="hostName">HostName of target computer</param>
        public ThreadLogObject(string hostName)
        {
            this.HostName = hostName;

            if (!Directory.Exists(this.LogPath))
            {
                Directory.CreateDirectory(this.LogPath);
            }

            this.LogFileName = string.Format("{0}\\{1}.log", this.LogPath, this.HostName);
            FileInfo tempFileInfo = new FileInfo(this.LogFileName);
            this.LogFileStream = tempFileInfo.Open(FileMode.Append, FileAccess.Write, FileShare.Read);
        }

        #region Properties

        /// <summary>
        /// Gets the path of discover log, default as "client_logs"
        /// </summary>
        public string LogPath
        {
            get
            {
                if (this.logPath == null)
                {
                    this.logPath = "client_logs";
                }

                return this.logPath;
            }
        }

        /// <summary>
        /// Gets or sets hostname of the discovery machine
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Gets or sets the name of the discovery log file
        /// </summary>
        public string LogFileName { get; set; }

        /// <summary>
        /// Gets or sets the stream of the discovery log file
        /// </summary>
        public FileStream LogFileStream { get; set; }

        #endregion

        /// <summary>
        /// writhe a log message in a line in a log file
        /// </summary>
        /// <param name="logMessage">the message of the log which will be written in log file</param>
        public void WriteLine(string logMessage)
        {
            string timestamp = DateTime.Now.ToString() + ": ";

            foreach (char c in timestamp.ToCharArray())
            {
                this.LogFileStream.WriteByte((byte)c);
            }

            foreach (char c in logMessage.ToCharArray())
            {
                this.LogFileStream.WriteByte((byte)c);
            }

            // Write CR LF to close out the line
            this.LogFileStream.WriteByte(0xD);
            this.LogFileStream.WriteByte(0xA);

            this.LogFileStream.Flush();
        }

        /// <summary>
        /// Close the stream of the log file
        /// </summary>
        public void CloseLog()
        {
            this.LogFileStream.Close();
        }
    }
}