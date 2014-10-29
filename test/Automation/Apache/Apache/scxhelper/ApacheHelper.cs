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
        /// Path to a local cache directory
        /// </summary>
        private string localCachePath = ".";

        /// <summary>
        /// UNC path to agent directory 
        /// </summary>
        private string dropLocation;

        /// <summary>
        /// Path to the SCX log on the remote client.
        /// </summary>
        private const string scxLogPath = "/var/opt/microsoft/scx/log/scx.log";

        /// <summary>
        /// Path to the SCX CIMD log on the remote client
        /// </summary>
        private const string scxcimLogPath = "/var/opt/microsoft/scx/log/scxcimd.log";

        /// <summary>
        /// Full path to the agent package file.
        /// </summary>
        private string fullApachePath;

        /// <summary>
        /// Agent FileInfo structure, populated from RecurseToFile()
        /// </summary>
        private FileInfo ApacheFile;

        /// <summary>
        /// Agent package extension
        /// </summary>
        private string agentPkgExt;

        /// <summary>
        /// Specific agent date to wait for
        /// </summary>
        private DateTime specificApacheDate;

        /// <summary>
        /// Text contained within directory structure denoting agent architecture
        /// </summary>
        private string directoryTag;

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
        /// Installation command for agent on Posix host
        /// </summary>
        private string installApacheCmd;

        /// <summary>
        /// Uninstallation command for agent on Posix host
        /// </summary>
        private string uninstallApacheCmd;

        /// <summary>
        /// Path and name of decompression utility
        /// </summary>
        private string decompressUtil;

        /// <summary>
        /// Extension for compressed file
        /// </summary>
        private string compressionExt;

        /// <summary>
        /// Log Delegate to allow writing using a log mechanism provided by the user.
        /// </summary>
        private ScxLogDelegate logger = ScxMethods.ScxNullLogDelegate;

        private string installApachePkg;
        private string startApacheCmd;
        private string checkServiceCmd;
        private string checkApacheInstalled;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ApacheHelper class.
        /// </summary>
        /// <param name="logger">Log delegate method (takes single string as argument)</param>
        /// <param name="hostName">Name of Posix host</param>
        /// <param name="userName">Valid user on Posix host</param>
        /// <param name="password">Password for user</param>
        /// <param name="installApacheCmd">Command to install agent package on Posix host</param>
        /// <param name="uninstallApacheCmd">Command to uninstall agent package from Posix host</param>
        public ApacheHelper(ScxLogDelegate logger, string hostName, string userName, string password, string installApacheCmd, string uninstallApacheCmd)
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

            if (string.IsNullOrEmpty(installApacheCmd))
            {
                throw new ArgumentNullException("installApacheCmd not set");
            }

            if (string.IsNullOrEmpty(uninstallApacheCmd))
            {
                throw new ArgumentNullException("uninstallApacheCmd not set");
            }

            this.specificApacheDate = new DateTime(0);
            this.logger = logger;
            this.hostName = hostName;
            this.userName = userName;
            this.password = password;
            this.installApacheCmd = installApacheCmd;
            this.uninstallApacheCmd = uninstallApacheCmd;
        }

        public ApacheHelper() { }

        public ApacheHelper
           (ScxLogDelegate logger,
           string hostName,
           string userName,
           string password,
           string installApacheCmd,
           string uninstallApacheCmd,
           string installApachePkg,
           string startApacheCmd,
           string checkServiceCmd,
           string checkApacheInstalled,
           string dropLocation)
            : this(logger, hostName, userName, password, installApacheCmd, uninstallApacheCmd)
        {
            if (string.IsNullOrEmpty(dropLocation))
            {
                throw new ArgumentNullException("dropLocation");
            }

            this.installApachePkg = installApachePkg;
            this.startApacheCmd = startApacheCmd;
            this.checkServiceCmd = checkServiceCmd;
            this.checkApacheInstalled = checkApacheInstalled;
            this.dropLocation = dropLocation;
        }
   
        #endregion Constructors

        #region Properties

        /// <summary>
        /// Bad name is due to conflict with previous logger class. Changing that might break other code. TODO: Fix this later.
        /// </summary>
        public ILogger GenericLogger { set { genericLogger = value; } }

        /// <summary>
        /// Gets or sets the LocalCachePath property, path to local directory for current agent files
        /// </summary>
        public string LocalCachePath
        {
            get { return this.localCachePath; }
            set { this.localCachePath = value; }
        }

        /// <summary>
        /// Gets or sets the DropLocation property 
        /// </summary>
        public string DropLocation
        {
            get { return this.dropLocation; }
            set { this.dropLocation = value; }
        }

        /// <summary>
        /// Gets or sets the FullApachePath property
        /// </summary>
        public string FullApachePath
        {
            get { return this.fullApachePath; }
            set { this.fullApachePath = value; }
        }

        /// <summary>
        /// Gets or sets the AgentPkgExt property for the package file extension
        /// </summary>
        public string AgentPkgExt
        {
            get { return this.agentPkgExt; }
            set { this.agentPkgExt = value; }
        }

        /// <summary>
        /// Gets or sets the DirectoryTag property to match within directory name
        /// </summary>
        public string DirectoryTag
        {
            get { return this.directoryTag; }
            set { this.directoryTag = value; }
        }

        /// <summary>
        /// Gets or sets the installApacheCmd property to install the SCX agent.  Use formatted specification with {0} for file name
        /// </summary>
        public string InstallApacheCmd
        {
            get { return this.installApacheCmd; }
            set { this.installApacheCmd = value; }
        }

        /// <summary>
        /// Gets or sets the UninstallApacheCmd property to uninstall the SCX agent
        /// </summary>
        public string UninstallApacheCmd
        {
            get { return this.uninstallApacheCmd; }
            set { this.uninstallApacheCmd = value; }
        }

        /// <summary>
        /// Gets or sets path, binary, and options for decompression utility. Use formatted specification with {0} for file name
        /// </summary>
        public string DecompressUtil
        {
            get { return this.decompressUtil; }
            set { this.decompressUtil = value; }
        }

        /// <summary>
        /// Gets or sets file extension used by compression utility, .Z, .gz, etc.
        /// </summary>
        public string CompressionExt
        {
            get { return this.compressionExt; }
            set { this.compressionExt = value; }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Install an agent on Posix host. Set AgentArchitecture and DirectoryTag properties.
        /// </summary>
        /// <remarks>WaitForNewAgent is optional.  If it is not run, then FullApachePath must be set.</remarks>
        public void Install()
        {
            if (string.IsNullOrEmpty(this.fullApachePath) == true)
            {
                throw new ArgumentNullException("FullApachePath not set");
            }

            if (string.IsNullOrEmpty(this.installApacheCmd) == true)
            {
                throw new ArgumentNullException("installApacheCmd not set");
            }

            // User-specified Apache path takes precedent
            string ApacheName;
            ApacheName = this.ApacheFile == null ? Path.GetFileName(this.fullApachePath) : this.ApacheFile.Name;

            PosixCopy copyToHost = new PosixCopy(this.hostName, this.userName, this.password);
            // Copy from server to Posix host
            this.logger("Copying Apache from drop server to host");
            copyToHost.CopyTo(this.fullApachePath, "/tmp/" + ApacheName);

            // Begin installation
            RunPosixCmd execInstall = new RunPosixCmd(this.hostName, this.userName, this.password);
            
            // Execute installation command
            execInstall.FileName = string.Format(this.installApacheCmd, ApacheName);
            execInstall.Arguments = string.Empty;
            this.logger(string.Format("Installing Apache to {0}: command: {1} ", this.hostName, execInstall.FileName));
            execInstall.RunCmd();
            this.logger("Install() installation out: " + execInstall.StdOut);

            // record installation in SCX CIMD log
            string serverHostName = Dns.GetHostEntry(Dns.GetHostName()).HostName;
            execInstall.RunCmd(string.Format("echo \"{0} INFO ApacheHelper: Apache {1} installed by {2} from {3}\" >> {4}", DateTime.Now.ToString(), ApacheName, serverHostName, this.fullApachePath, scxcimLogPath));

            // Delete temporary file
            this.logger("Removing files in /tmp");
            execInstall.FileName = "/bin/rm";
            execInstall.Arguments = "-f /tmp/" + ApacheName;
            execInstall.RunCmd();
        }
        
        public void FindApache(bool useTodaysApache, string date, int minutes, bool latestOnly)
        {
            DateTime ApacheDate = new DateTime(0);
            string pathToApache = string.Empty;

            if (useTodaysApache == true)
            {
                ApacheDate = DateTime.Today;
            }
            else
            {
                // If an absolute Apache date has been specified, initialize optional ApacheDate
                if (string.IsNullOrEmpty(date) == false)
                {
                    ApacheDate = DateTime.Parse(date, new System.Globalization.CultureInfo("en"));
                }
                else if (this.specificApacheDate.Ticks != 0)
                {
                    ApacheDate = this.specificApacheDate;
                }
            }

            if (string.IsNullOrEmpty(this.AgentPkgExt))
            {
                throw new ArgumentNullException("ApachePkgExt");
            }

            if (string.IsNullOrEmpty(this.directoryTag))
            {
                throw new ArgumentNullException("DirectoryTag");
            }

            // Try to find Apache
            this.logger("FindApache: dropLocation=" + this.dropLocation + ", ApachePkgExt=" + this.AgentPkgExt + ", directoryTag=" + this.directoryTag);
            while ((this.RecurseToFile(out pathToApache, this.dropLocation, this.AgentPkgExt, this.directoryTag) == false) && (minutes > 0))
            {
                minutes--;
                this.logger(string.Format("Apache not found.  Waiting for 1 minute; {0} attempts remaining", minutes));
                System.Threading.Thread.Sleep(minutes * 60000);
            }

            this.logger("FindApache: pathToApache=" + pathToApache);
            if (string.IsNullOrEmpty(pathToApache) == false)
            {
                this.fullApachePath = pathToApache;
            }
            else
            {
                throw new Exception("Apache not found on drop server");
            }
        }

        public void VerifySSH()
        {
            this.logger("Verifying if client respond to SSH login");
            RunPosixCmd echoTest = new RunPosixCmd(this.hostName, this.userName, this.password);

            try
            {
                echoTest.RunCmd("echo test", 3);

                if (echoTest.ExitCode != 0)
                {
                    throw new Exception(echoTest.ExitCode.ToString());
                }
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("VerifySSH(): Could not SSH to remote host: '{0}' ExitCode: {1}", hostName, e.Message));
            }

            this.logger(string.Format("Ssh test against {0} succeeded", hostName));
        }

        /// <summary>
        /// Uninstall an agent from a Posix host
        /// </summary>
        public void Uninstall()
        {
            if (string.IsNullOrEmpty(this.fullApachePath) == true)
            {
                throw new ArgumentNullException("FullApachePath not set");
            }

            if (string.IsNullOrEmpty(this.uninstallApacheCmd) == true)
            {
                throw new ArgumentNullException("uninstallApacheCmd not set");
            }

            // User-specified Apache path takes precedent
            string ApacheName;
            ApacheName = this.ApacheFile == null ? Path.GetFileName(this.fullApachePath) : this.ApacheFile.Name;

            try
            {
                RunPosixCmd CheckApacheFile = new RunPosixCmd(this.hostName, this.userName, this.password);
                CheckApacheFile.FileName = "/bin/ls /tmp/" + ApacheName;
                CheckApacheFile.RunCmd();
            }
            catch (ApplicationException ae)
            {

                PosixCopy copyToHost = new PosixCopy(this.hostName, this.userName, this.password);
                // Copy from server to Posix host
                this.logger("Copying Apache from drop server to host"+ae.Message);
                copyToHost.CopyTo(this.fullApachePath, "/tmp/" + ApacheName);
            }
            
            // Begin installation
            RunPosixCmd execUninstall = new RunPosixCmd(this.hostName, this.userName, this.password);

            // Execute installation command
            execUninstall.FileName = string.Format(this.uninstallApacheCmd, ApacheName);
            execUninstall.Arguments = string.Empty;
            this.logger(string.Format("UnInstalling Apache to {0}: command: {1} ", this.hostName, execUninstall.FileName));
            execUninstall.RunCmd();
            this.logger("UnInstall() Uninstallation out: " + execUninstall.StdOut);

        }


        public void CheckApacheStatus(string hostName, string checkApacheInstalled, string checkServiceCmd, string startApacheCmd, string userName, string password)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                throw new ArgumentNullException("hostName");
            }
            if(string.IsNullOrEmpty(checkApacheInstalled))
            {
                throw new ArgumentNullException("checkApacheInstalled");
            }

            if (string.IsNullOrEmpty(startApacheCmd))
            {
                throw new ArgumentNullException("startApacheCmd");
            }

            genericLogger.Write("Check Apache status:", hostName, checkServiceCmd);
            RunPosixCmd execUninstall = new RunPosixCmd(hostName, userName, password)
            {
                FileName = checkApacheInstalled,
                Arguments = string.Empty
            };

            try
            {
                execUninstall.RunCmd();
                //genericLogger.Write("Uninstall agent from '{0}' successfully.", hostName);

                string stdout = execUninstall.StdOut;

                if (string.IsNullOrEmpty(stdout))
                {
                    throw new Exception("The apache didn't install on client");
                }

                execUninstall.FileName = checkServiceCmd;

                try
                {
                    execUninstall.RunCmd();
                }
                catch (ApplicationException e)
                {
                    string message = e.InnerException.ToString();
                    if (!(message.Contains("httpd is stopped")))
                    { 
                        throw; 
                    }                      
                }

                stdout = execUninstall.StdOut;

                if(!(stdout.Contains("running")))
                {
                    execUninstall.FileName = startApacheCmd;
                    execUninstall.RunCmd();
                    System.Threading.Thread.Sleep(1000);

                    execUninstall.FileName = checkServiceCmd;
                    execUninstall.RunCmd();

                    stdout = execUninstall.StdOut;
                    if (!(stdout.Contains("running")))
                    {
                        throw new Exception("Start apache instance failed.");
                    }
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion Public Methods

        #region Private Methods
        private bool RecurseToFile(out string fullPath, string rootPath, string fileExt, string directoryArchTag)
        {
            DirectoryInfo di;
            fullPath = string.Empty;

            try
            {
                di = new DirectoryInfo(rootPath);
            }
            catch
            {
                genericLogger.Write("RecurseToFile: Could not get directory listing of root path!: " + rootPath);
                return false;
            }

            try
            {
                //Get the latest folder and try to locate the Apache.
                genericLogger.Write("Attempting to find the new Apache...");
                DirectoryInfo diNew = new DirectoryInfo(rootPath).
                    GetDirectories(string.Format("{0}*", directoryArchTag)).First().
                    GetDirectories().OrderByDescending(d => d.CreationTimeUtc).First();

                // Split the platform. E.g. directoryTag: "AIX5.3_ppc", it will return "AIX".
                string platform = Regex.Split(directoryArchTag, "[1-9]", RegexOptions.IgnoreCase)[0];
                fullPath = diNew.GetDirectories(string.Format("*{0}*", platform))[0].GetFiles(string.Format("*{0}", fileExt))[0].FullName;
                return true;
            }
            catch
            {
                genericLogger.Write("Apache not found!");
                return false;
            }
        }
        
        #endregion Private Methods
        #endregion Methods
    }
}
