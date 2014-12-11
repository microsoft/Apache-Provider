//-----------------------------------------------------------------------
// <copyright file="SshHelper.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>sunilmu</author>
// <description></description>
// <history>1/9/2009 7:56:06 PM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Commom
{
    using System;
    using sshcomLib;
    using System.Threading;

    /// <summary>
    /// Summary for SshHelper.
    /// </summary>
    public class SshHelper
    {
        #region Private Fields

        private string hostname, username, password;
        private string output = string.Empty, command = string.Empty;
        private int port;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Default Constructor for the SshHelper class.
        /// </summary>
        public SshHelper()
        {
        }

        #endregion Constructors

        #region Properties

        public string Command
        {
            set
            {
                this.command = value;
            }
            get
            {
                return this.command;
            }
        }

        #endregion Properties

        #region Methods

        #region Private Methods

        private void Run()
        {
            scxsshClass ssh = new scxsshClass();
            ssh.ConnectWithPassword(this.hostname, this.port, this.username, this.password);
            ssh.ExecuteCommand2(this.command, out this.output);
        }

        #endregion Private Methods

        #region Public Methods

        public SshHelper(string hname, int p,
            string usr, string pwd)
        {
            this.hostname = hname;
            this.port = p;
            this.username = usr;
            this.password = pwd;
        }

        public string RunCommand()
        {
            if (string.Empty == this.command)
                throw new ArgumentNullException("Error: Please set the \"Command\" property before calling this method");
            Thread thread = new Thread(this.Run);
            thread.Start();
            thread.Join();
            return this.output;
        }

        public string RunCommandWithTimeout(int timeout)
        {
            if (string.Empty == this.command)
                throw new ArgumentNullException("Error: Please set the \"Command\" property before calling this method");
            Thread thread = new Thread(this.Run);
            thread.Start();
            if (!(thread.Join(timeout * 1000)))//Converting seconds to milliseconds
            {
                thread.Abort();
                throw new TimeoutException("Ssh command took too long to execute.\n");
            }

            return this.output;
        }

        #endregion Public Methods

        #endregion Methods
    }
}

