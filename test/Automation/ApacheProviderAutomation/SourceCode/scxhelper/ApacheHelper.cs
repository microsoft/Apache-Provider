//-----------------------------------------------------------------------
// <copyright file="ApacheHelper.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>v-litin</author>
// <description></description>
// <history>7/15/2014 11:33:56 AM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Common
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;

    /// <summary>
    /// The ApacheHelper class provides functionality to install and remove
    /// an agent on a Posix host.
    /// </summary>
    /// <example>
    /// ApacheHelper helper = new ApacheHelper("scxom-rhel51-01.scx.com", "root", "OpsMgr2007R2", "rpm -i", "rpm -e scx");
    /// helper.AgentPkgExt = "rpm";
    /// helper.DirectoryTag = "Linux_REDHAT_5.0_x86_32";
    /// helper.FindAgent(false, null, 3); // don't wait for today's agent, no specific date (latest), three minute timeout
    /// helper.Install();
    /// </example>
    public class ApacheHelper
    {
        #region Private Fields

        /// <summary>
        /// Generic logger. This is the ConsoleLogger by default.
        /// </summary>
        private ILogger genericLogger = new ConsoleLogger();

        /// <summary>
        /// Full path to the agent package file.
        /// </summary>
        private string fullApacheAgentPath;

        private string apacheTag;

        /// <summary>
        /// Posix host name
        /// </summary>
        private string hostName;

        /// <summary>
        /// Valid user name on Posix host
        /// </summary>
        private string userName;

        /// <summary>
        /// Password for user
        /// </summary>
        private string password;

        /// <summary>
        /// The name of apache agent.
        /// </summary>
        public string apacheAgentName;

        /// <summary>
        /// Log Delegate to allow writing using a log mechanism provided by the user.
        /// </summary>
        private ScxLogDelegate logger = ScxMethods.ScxNullLogDelegate;

        //private string installApachePkg;
        //private string startApacheCmd;
        //private string checkServiceCmd;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ApacheHelper class.
        /// </summary>
        /// <param name="logger">Log delegate method (takes single string as argument)</param>
        /// <param name="hostName">Name of Posix host</param>
        /// <param name="userName">Valid user on Posix host</param>
        /// <param name="password">Password for user</param>
        public ApacheHelper(ScxLogDelegate logger, string hostName, string userName, string password)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                throw new ArgumentNullException("hostName not set");
            }

            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException("userName not set");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password not set");
            }

            this.logger = logger;
            this.hostName = hostName;
            this.userName = userName;
            this.password = password;
        }

        /// <summary>
        /// Initializes a new instance of the ApacheHelper class.
        /// </summary>
        /// <param name="logger">Log delegate method (takes single string as argument)</param>
        /// <param name="hostName">Name of Posix host</param>
        /// <param name="userName">Valid user on Posix host</param>
        /// <param name="password">Password for user</param>
        public ApacheHelper(ScxLogDelegate logger, string hostName, string userName, string password, string apcheAgentFullPath, string apacheTag, bool needCopyFile = false)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                throw new ArgumentNullException("hostName not set");
            }

            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException("userName not set");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password not set");
            }

            if (string.IsNullOrEmpty(apcheAgentFullPath))
            {
                throw new ArgumentNullException("apcheAgentFullPath not set");
            }

            this.logger = logger;
            this.hostName = hostName;
            this.userName = userName;
            this.password = password;
            this.SetApacheAgentFullPath(apcheAgentFullPath, apacheTag, needCopyFile);
        }

        public ApacheHelper() { }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Bad name is due to conflict with previous logger class. Changing that might break other code. TODO: Fix this later.
        /// </summary>
        public ILogger GenericLogger { set { genericLogger = value; } }

        /// <summary>
        /// Gets or sets the FullApachePath property
        /// </summary>
        public string FullApachePath
        {
            get { return this.fullApacheAgentPath; }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Set the apacheFullPath.
        /// </summary>
        /// <remarks>WaitForNewAgent is optional.  If it is not run, then FullApachePath must be set.</remarks>
        public void SetApacheAgentFullPath(string apcheAgentFullPath, string apcheTag, bool needCopyFile = false)
        {
            this.fullApacheAgentPath = apcheAgentFullPath;
            this.apacheTag = apcheTag;
            //"Searching for apache in " + this.fullApacheAgentPath;
            DirectoryInfo di = new DirectoryInfo(this.fullApacheAgentPath);
            FileInfo[] fi = di.GetFiles("*" + this.apacheTag + "*");
            if (fi.Length == 0)
            {
                throw new Exception("Found no apache installer matching ApacheTag: " + this.apacheTag);
            }

            if (fi.Length > 1)
            {
                throw new Exception("Found more than one apache installer matching ApacheTag: " + this.apacheTag);
            }

            // User-specified Apache path takes precedent
            apacheAgentName = Path.GetFileName(fi[0].FullName);

            if (needCopyFile)
            {
                PosixCopy copyToHost = new PosixCopy(this.hostName, this.userName, this.password);
                // Copy from server to Posix host
                this.logger("Copying Apache from drop server to host");
                copyToHost.CopyTo(fi[0].FullName, "/tmp/" + apacheAgentName);
            }
        }

        /// <summary>
        /// Run special cmd.
        /// </summary>
        /// <remarks>WaitForNewAgent is optional.  If it is not run, then FullApachePath must be set.</remarks>
        public RunPosixCmd RunCmd(string cmd, string arguments = "")
        {
            // Begin cmd
            RunPosixCmd execCmd = new RunPosixCmd(this.hostName, this.userName, this.password);

            // Execute command
            execCmd.FileName = cmd;
            execCmd.Arguments = arguments;
            this.logger(string.Format("Run Command {0} on host {1} ", execCmd.FileName, this.hostName));
            execCmd.RunCmd();
            this.logger(string.Format("Command {0} out: {1}", execCmd.FileName, execCmd.StdOut));
            return execCmd;
        }

        /// <summary>
        /// Install an agent on Posix host. Set AgentArchitecture and DirectoryTag properties.
        /// </summary>
        /// <remarks>WaitForNewAgent is optional.  If it is not run, then FullApachePath must be set.</remarks>
        public void InstallApacheAgent(string installCmd)
        {
            if (string.IsNullOrEmpty(this.fullApacheAgentPath) == true)
            {
                throw new ArgumentNullException("FullApachePath not set");
            }

            try
            {
                this.RunCmd(string.Format(installCmd, apacheAgentName));
            }
            catch (Exception e)
            {
                throw new Exception("Install apache CimProv agent failed: " + e.Message);
            }

        }

        /// <summary>
        /// Uninstall an agent from a Posix host
        /// </summary>
        public void UninstallApacheAgent(string uninstallCmd)
        {
            try
            {
                this.RunCmd(string.Format(uninstallCmd, apacheAgentName));
            }
            catch (Exception e)
            {
                throw new Exception("Uninstall apache CimProv agent failed: " + e.Message);
            }

        }

        /// <summary>
        /// Check apache server status
        /// return values:
        /// true: running
        /// false: not running
        /// </summary>
        public bool CheckApacheServiceStatus(string checkApacheServiceStatus, string startApacheServiceCmd)
        {
            try
            {
                RunPosixCmd returnValue = this.RunCmd(checkApacheServiceStatus);
                if (returnValue.StdOut.ToLower().Contains("pid") || returnValue.StdOut.ToLower().Contains("/usr/local/apache2/bin/httpd") || returnValue.StdOut.ToLower().Contains("is running"))
                {
                    return true;
                }
                else
                {
                    StartApacheServiceStatus(startApacheServiceCmd);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Check apache status failed: " + e.Message);
            }
            return false;
        }


        /// <summary>
        /// Start apache server
        /// </summary>
        public void StartApacheServiceStatus(string startApacheService)
        {
            try
            {
                this.RunCmd(startApacheService);
            }
            catch (Exception e)
            {
                throw new Exception("Check apache status failed: " + e.Message);
            }
        }

        /// <summary>
        /// Stop apache server
        /// </summary>
        public void StopApacheServiceStatus(string stopApacheService)
        {
            try
            {
                this.RunCmd(stopApacheService);
            }
            catch (Exception e)
            {
                throw new Exception("Check apache status failed: " + e.Message);
            }
        }

        /// <summary>
        /// restart apache server
        /// </summary>
        public void RestartApacheServiceStatus(string restartApacheService)
        {
            try
            {
                this.RunCmd(restartApacheService);
            }
            catch (Exception e)
            {
                throw new Exception("Check apache status failed: " + e.Message);
            }
        }

        /// <summary>
        /// Cleanup the temp files
        /// </summary>
        public void Cleanup()
        {
            // Delete temporary file
            this.logger("Removing files in /tmp");
            // Begin cmd
            RunPosixCmd execCmd = new RunPosixCmd(this.hostName, this.userName, this.password);

            // Execute command
            execCmd.FileName = "/bin/rm";
            execCmd.Arguments = "-f /tmp/" + apacheAgentName;
            execCmd.RunCmd();
        }

        #endregion Public Methods

        #endregion Methods
    }
}
