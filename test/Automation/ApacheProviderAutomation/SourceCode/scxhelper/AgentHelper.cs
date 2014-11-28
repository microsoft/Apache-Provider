//-----------------------------------------------------------------------
// <copyright file="AgentHelper.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>a-brmill</author>
// <description></description>
// <history>4/1/2009 5:33:56 PM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Common
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;

    /// <summary>
    /// The AgentHelper class provides functionality to install and remove
    /// an agent on a Posix host.
    /// </summary>
    /// <example>
    /// AgentHelper helper = new AgentHelper("scxom-rhel51-01.scx.com", "root", "OpsMgr2007R2", "rpm -i", "rpm -e scx");
    /// helper.AgentPkgExt = "rpm";
    /// helper.DirectoryTag = "Linux_REDHAT_5.0_x86_32";
    /// helper.FindAgent(false, null, 3); // don't wait for today's agent, no specific date (latest), three minute timeout
    /// helper.Install();
    /// </example>
    public class AgentHelper
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
        /// Path to local UnixAgents directory 
        /// </summary>
        private string localAgentPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                        @"System Center 2012\Operations Manager\Server\AgentManagement\UnixAgents");

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
        private string fullAgentPath;

        /// <summary>
        /// Agent FileInfo structure, populated from RecurseToFile()
        /// </summary>
        private FileInfo agentFile;

        /// <summary>
        /// Agent package extension
        /// </summary>
        private string agentPkgExt;

        /// <summary>
        /// Specific agent date to wait for
        /// </summary>
        private DateTime specificAgentDate;

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
        private string installOmCmd;

        /// <summary>
        /// Uninstallation command for agent on Posix host
        /// </summary>
        private string uninstallOmCmd;

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

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the AgentHelper class.
        /// </summary>
        /// <param name="logger">Log delegate method (takes single string as argument)</param>
        /// <param name="hostName">Name of Posix host</param>
        /// <param name="userName">Valid user on Posix host</param>
        /// <param name="password">Password for user</param>
        /// <param name="installOmCmd">Command to install agent package on Posix host</param>
        /// <param name="uninstallOmCmd">Command to uninstall agent package from Posix host</param>
        public AgentHelper(ScxLogDelegate logger, string hostName, string userName, string password, string installOmCmd, string uninstallOmCmd)
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

            if (string.IsNullOrEmpty(installOmCmd))
            {
                throw new ArgumentNullException("installOmCmd not set");
            }

            if (string.IsNullOrEmpty(uninstallOmCmd))
            {
                throw new ArgumentNullException("uninstallOmCmd not set");
            }

            this.specificAgentDate = new DateTime(0);
            this.logger = logger;
            this.hostName = hostName;
            this.userName = userName;
            this.password = password;
            this.installOmCmd = installOmCmd;
            this.uninstallOmCmd = uninstallOmCmd;
        }

        /// <summary>
        /// Need this for calling the public methods directly from the varmap.
        /// </summary>
        public AgentHelper() { }

        /// <summary>
        /// Initializes a new instance of the AgentHelper class.
        /// </summary>
        /// <param name="logger">Log delegate method (takes single string as argument)</param>
        /// <param name="hostName">Name of Posix host</param>
        /// <param name="userName">Valid user on Posix host</param>
        /// <param name="password">Password for user</param>
        /// <param name="installOmCmd">Command to install agent package on Posix host</param>
        /// <param name="uninstallOmCmd">Command to uninstall agent package from Posix host</param>
        /// <param name="dropLocation">Drop location which is part of the agent file name</param>
        public AgentHelper
            (ScxLogDelegate logger,
            string hostName,
            string userName,
            string password,
            string installOmCmd,
            string uninstallOmCmd,
            string dropLocation)
            : this(logger, hostName, userName, password, installOmCmd, uninstallOmCmd)
        {
            if (string.IsNullOrEmpty(dropLocation))
            {
                throw new ArgumentNullException("dropLocation");
            }

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
        /// Gets or sets the FullAgentPath property
        /// </summary>
        public string FullAgentPath
        {
            get { return this.fullAgentPath; }
            set { this.fullAgentPath = value; }
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
        /// Gets or sets the InstallOmCmd property to install the SCX agent.  Use formatted specification with {0} for file name
        /// </summary>
        public string InstallOmCmd
        {
            get { return this.installOmCmd; }
            set { this.installOmCmd = value; }
        }

        /// <summary>
        /// Gets or sets the UninstallOmCmd property to uninstall the SCX agent
        /// </summary>
        public string UninstallOmCmd
        {
            get { return this.uninstallOmCmd; }
            set { this.uninstallOmCmd = value; }
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
        /// Execute 'echo' on the remote machine as a test of the ability to make an ssh connection.
        /// </summary>
        /// <exception cref="Exception"></exception>
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
        /// Set the time on the remote UNIX machine to equal that on the local server running MCF
        /// </summary>
        public void SynchDateTime()
        {
            RunPosixCmd dateCmd = new RunPosixCmd(this.hostName, this.userName, this.password);

            try
            {
                DateTime now = DateTime.Now;

                string dateCmdString = string.Format("date {0:00}{1:00}{2:00}{3:00}{4:0000}", now.Month, now.Day, now.Hour, now.Minute, now.Year);

                if (this.directoryTag.StartsWith("AIX"))
                {
                    this.logger("Seconds field not updated in AIX");
                }
                else if (this.directoryTag.StartsWith("HPUX"))
                {
                    this.logger("Seconds field not updated in HPUX");

                    // HPUX forces you to answer 'yes' at the prompt when moving time backwards.
                    dateCmdString = "echo \"yes\" | " + dateCmdString;
                }
                else
                {
                    dateCmdString += string.Format(".{0:00}", now.Second);
                }

                this.logger("Synchronizing client date to server date: " + now.ToString());
                dateCmd.RunCmd(dateCmdString, 3);

                dateCmd.RunCmd("date");
                this.logger("Verifying client date: " + dateCmd.StdOut);
            }
            catch (Exception e)
            {
                this.logger(string.Format("SynchTime(): Unable to synchronize time on remote host: '{0}' - {1}", this.hostName, e.Message));
            }
        }

        /// <summary>
        /// Find a new agent with optional wait
        /// </summary>
        /// <param name="useTodaysAgent">Use today's date (wait for today's agent)</param>
        /// <param name="date">Optional date</param>
        /// <param name="minutes">Time, in minutes, to wait if agent is unavailable. 0=no wait</param>
        /// <param name="latestOnly">If true, will fail if it cannot find agent in the latest build folder</param>
        public void FindAgent(bool useTodaysAgent, string date, int minutes, bool latestOnly)
        {
            DateTime agentDate = new DateTime(0);
            string pathToAgent = string.Empty;

            if (useTodaysAgent == true)
            {
                agentDate = DateTime.Today;
            }
            else
            {
                // If an absolute agent date has been specified, initialize optional agentDate
                if (string.IsNullOrEmpty(date) == false)
                {
                    agentDate = DateTime.Parse(date, new System.Globalization.CultureInfo("en"));
                }
                else if (this.specificAgentDate.Ticks != 0)
                {
                    agentDate = this.specificAgentDate;
                }
            }

            if (string.IsNullOrEmpty(this.agentPkgExt))
            {
                throw new ArgumentNullException("AgentPkgExt");
            }

            if (string.IsNullOrEmpty(this.directoryTag))
            {
                throw new ArgumentNullException("DirectoryTag");
            }

            // Try to find agent
            this.logger("FindAgent: dropLocation=" + this.dropLocation + ", agentPkgExt=" + this.agentPkgExt + ", directoryTag=" + this.directoryTag);
            while ((this.RecurseToFile(out pathToAgent, this.dropLocation, this.agentPkgExt, this.directoryTag) == false) && (minutes > 0))
            {
                minutes--;
                this.logger(string.Format("Agent not found.  Waiting for 1 minute; {0} attempts remaining", minutes));
                System.Threading.Thread.Sleep(minutes * 60000);
            }

            this.logger("FindAgent: pathToAgent=" + pathToAgent);
            if (string.IsNullOrEmpty(pathToAgent) == false)
            {
                this.fullAgentPath = pathToAgent;
            }
            else
            {
                throw new Exception("Agent not found on drop server");
            }
        }

        /// <summary>
        /// Install an agent on Posix host. Set AgentArchitecture and DirectoryTag properties.
        /// </summary>
        /// <remarks>WaitForNewAgent is optional.  If it is not run, then FullAgentPath must be set.</remarks>
        public void Install()
        {
            // for the moment, don't use the cache in BVT's.
            bool useCache = false;

            if (string.IsNullOrEmpty(this.fullAgentPath) == true)
            {
                throw new ArgumentNullException("FullAgentPath not set");
            }

            if (string.IsNullOrEmpty(this.installOmCmd) == true)
            {
                throw new ArgumentNullException("InstallOmCmd not set");
            }

            // User-specified agent path takes precedent
            string agentName;
            agentName = this.agentFile == null ? Path.GetFileName(this.fullAgentPath) : this.agentFile.Name;

            PosixCopy copyToHost = new PosixCopy(this.hostName, this.userName, this.password);

            if (useCache)
            {
                // Copy to cache if necessary
                if (File.Exists(Path.Combine(this.localCachePath, agentName)) == false)
                {
                    this.logger("Copying agent to cache");
                    System.IO.File.Copy(this.fullAgentPath, Path.Combine(this.localCachePath, agentName));
                }

                // Copy from cache to Posix host
                this.logger("Copying agent from cache to host");
                copyToHost.CopyTo(Path.Combine(this.localCachePath, agentName), "/tmp/" + agentName);
            }
            else
            {
                // Copy from server to Posix host
                this.logger("Copying agent from drop server to host");
                copyToHost.CopyTo(this.fullAgentPath, "/tmp/" + agentName);
            }

            // Begin installation
            RunPosixCmd execInstall = new RunPosixCmd(this.hostName, this.userName, this.password);

            // Execute installation command
            execInstall.FileName = string.Format(this.installOmCmd, agentName);
            execInstall.Arguments = string.Empty;
            this.logger(string.Format("Installing agent to {0}: command: {1} ", this.hostName, execInstall.FileName));
            execInstall.RunCmd();
            this.logger("Install() installation out: " + execInstall.StdOut);

            // record installation in SCX CIMD log
            string serverHostName = Dns.GetHostEntry(Dns.GetHostName()).HostName;
            execInstall.RunCmd(string.Format("echo \"{0} INFO agentHelper: agent {1} installed by {2} from {3}\" >> {4}", DateTime.Now.ToString(), agentName, serverHostName, this.fullAgentPath, scxcimLogPath));

            // Delete temporary file
            this.logger("Removing files in /tmp");
            execInstall.FileName = "/bin/rm";
            execInstall.Arguments = "-f /tmp/" + agentName;
            execInstall.RunCmd();
        }

        /// <summary>
        /// Uninstall an agent from a Posix host
        /// </summary>
        public void Uninstall()
        {
            Uninstall(this.hostName, this.uninstallOmCmd, this.userName, this.password);
        }

        /// <summary>
        /// Uninstall an agent from a Posix host.
        /// This is a generic method which can be called from varmap, for example in multihost tests
        /// </summary>
        /// <param name="hostName">The hostNames you want to uninstall. Seperated by comma.</param>
        /// <param name="uninstallOmCmd">Uninstall commands. Seperated by comma.</param>
        /// <param name="userName">username to connect host.</param>
        /// <param name="password">password for username.</param>
        public void Uninstall(string hostName, string uninstallOmCmd, string userName, string password)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                throw new ArgumentNullException("hostName");
            }
            if (string.IsNullOrEmpty(uninstallOmCmd))
            {
                throw new ArgumentNullException("UninstallOmCmd");
            }

            genericLogger.Write("Uninstalling agent: from {0}: {1} ", hostName, uninstallOmCmd);
            RunPosixCmd execUninstall = new RunPosixCmd(hostName, userName, password)
            {
                FileName = uninstallOmCmd,
                Arguments = string.Empty
            };

            try
            {
                execUninstall.RunCmd();
                genericLogger.Write("Uninstall agent from '{0}' successfully.", hostName);
            }
            catch (Exception e)
            {
                this.logger(string.Format("Uninstall agent from '{0}' failed. Error message: '{1}'", hostName, e.ToString()));
            }
        }

        /// <summary>
        /// Uninstall an agent from some Posix hosts, it can be different platforms, EX. Rhel and Solaris.
        /// This is a generic method which can be called from varmap, for example in multihost tests 
        /// </summary>
        /// <param name="hostNames">The hostnames you want to uninstalled. Can be different platforms.</param>
        /// <param name="uninstallOmCmds">Uninstall command for each matched platform.</param>
        /// <param name="userName">username to connect host.</param>
        /// <param name="password">password for username.</param>
        public void Uninstall(string[] hostNames, string[] uninstallOmCmds, string userName, string password)
        {
            if (hostNames==null || uninstallOmCmds == null)
            {
                throw new ArgumentNullException("hostNames or UninstallOmCmds");
            }

            for (int index = 0; index < hostNames.Length; index++)
            {
                Uninstall(hostNames[index], uninstallOmCmds[index], userName, password);
            }
        }

        /// <summary>
        /// Added this class to be able to call methods from the varmap.
        /// </summary>
        /// <param name="hostname">Client to talk to.</param>
        /// <param name="user">Super user.</param>
        /// <param name="pwd">Password for the super user.</param>
        /// <param name="instOmCmd">Command that can be used to install the agent. (Optional) Can be left empty if not required.</param>
        /// <param name="uninstOmCmd">Command that can be used to uninstall the agent. (Optional) Can be left empty if not required.</param>
        public void Init(string hostname, string user, string pwd, string instOmCmd = "", string uninstOmCmd = "")
        {
            hostName = hostname;
            userName = user;
            password = pwd;
            installOmCmd = instOmCmd;
            uninstallOmCmd = uninstOmCmd;
        }

        /// <summary>
        /// Stages the agents for the build folder. Picks the new agent and the old agent specified 
        /// in newAgentPath and oldAgentPath varmap nodes.  
        /// If newAgent path is blank, the latest agent from builds folder is staged. 
        /// If oldAgentPath is blank, the oldest agent from local agents directory path is staged. 
        /// This method needs to be changed to support staging folders on multiple management servers.
        /// </summary>
        /// <param name="dirTag">Directory tag on the build server.</param>
        /// <param name="platformTag">Platform tag which is part of the agent file name.</param>
        /// <param name="oldAgentPath">Old agent will be copied from here if provided, or will be calculated.</param>
        /// <param name="newAgentPath">New agent will be copied from here if provided, or will be calculated.</param>
        public void StageAgents(
            string dropLocation,
            string dirTag,
            string platformTag,
            string oldAgentPath = "",
            string newAgentPath = "",
            string omServers = "")
        {
            if (string.IsNullOrEmpty(dropLocation))
            {
                throw new ArgumentException("Could not locate latest agent without specified drop location.");
            }

            if (string.IsNullOrEmpty(dirTag) || string.IsNullOrEmpty(platformTag))
            {
                throw new ArgumentNullException("dirTag or platformTag");
            }

            FileInfo newAgentFile;
            if (!string.IsNullOrEmpty(newAgentPath))
            {
                newAgentFile = new FileInfo(newAgentPath);
            }
            else
            {
                //Get the latest folder and try to locate the agent.
                genericLogger.Write("Attempting to find the new agent.");
                DirectoryInfo diNew = new DirectoryInfo(dropLocation).
                    GetDirectories(string.Format("{0}{1}{0}", "*", dirTag)).First().
                    GetDirectories().OrderByDescending(d => d.CreationTimeUtc).First();

                newAgentFile = diNew.GetFiles(string.Format("{0}{1}{0}", "*", platformTag),
                    SearchOption.AllDirectories).FirstOrDefault();
            }

            FileInfo oldAgentFile = null;
            if (!string.IsNullOrEmpty(oldAgentPath))
            {
                oldAgentFile = new FileInfo(oldAgentPath);
            }
            else
            {
                // Get oldest agent from local agents directory
                DirectoryInfo diOld = new DirectoryInfo(localAgentPath);
                oldAgentFile = diOld.GetFiles(string.Format("{0}{1}{0}", "*", platformTag)).OrderByDescending(f => f.CreationTimeUtc).Last();
            }

            CopyAgents(oldAgentFile, newAgentFile, platformTag, omServers);
        }

        /// <summary>
        /// Stages the agents for the build folder. Picks the new agent and the old agent specified 
        /// in newAgentPath and oldAgentPath varmap nodes.  
        /// If newAgent path is blank, the latest agent from builds folder is staged. 
        /// If oldAgentPath is blank, the oldest agent from local agents directory path is staged. 
        /// </summary>
        /// <param name="dirTags">Directory tag on the build server.</param>
        /// <param name="platformTags">Platform tag which is part of the agent file name.</param>
        /// <param name="oldAgentPath">Old agent will be copied from here if provided, or will be calculated.</param>
        /// <param name="newAgentPath">New agent will be copied from here if provided, or will be calculated.</param>
        public void StageAgents(
            string dropLocation,
            string[] dirTags,
            string[] platformTags,
            string oldAgentPath = "",
            string newAgentPath = "",
            string omServers = "")
        {   
            if (string.IsNullOrEmpty(dropLocation))
            {
                throw new ArgumentException("Could not locate latest agent without specified drop location.");
            }

            if (dirTags == null || platformTags == null)
            {
                throw new ArgumentNullException("dirTags or platformTags");
            }
            if (dirTags.Length != platformTags.Length)
            {
                throw new ApplicationException("Argument DirTags should have same count with PlatformTags.");
            }

            for (int index = 0; index < dirTags.Length; index++)
            {
                StageAgents(dropLocation, dirTags[index], platformTags[index], oldAgentPath, newAgentPath, omServers);
            }
        }

        /// <summary>
        /// Installs the required agent. This will use a combination of versionIndicator and platformTag
        /// or just the path parameter to figure out agent that should be installed on the client system.
        /// </summary>
        /// <param name="versionIndicator">Indicates the version to install. Allowed values = "old"|"new".</param>
        /// <param name="platformTag">Platform tag which is part of the agent file name.</param>
        /// <param name="path">Full path of the agent. If this is specified, the other two parameters will be ignored.</param>
        public void InstallAgent(string versionIndicator, string platformTag, string path)
        {
            InstallAgent(this.hostName, versionIndicator, platformTag, path, this.userName, this.password);
        }

        /// <summary>
        /// Installs the required agent. This will use a combination of versionIndicator and platformTag
        /// or just the path parameter to figure out agent that should be installed on the client system.
        /// This is a generic install agent method to be called from varmap, for example in multihost tests
        /// This method should not be called in varmap.
        /// </summary>
        /// <param name="hostName">Hostname of machine to install agent</param>
        /// <param name="versionIndicator">Indicates the version to install. Allowed values = "old"|"new".</param>
        /// <param name="platformTag">Platform tag which is part of the agent file name.</param>
        /// <param name="path">Full path of the agent. If this is specified, the other two parameters will be ignored.</param>
        /// <param name="installomcommand">Install command for current host.</param>
        public void InstallAgent(string hostName, string versionIndicator, string platformTag, string path, string userName, string password, string installomcommand = "")
        {
            if ((string.IsNullOrEmpty(versionIndicator) || string.IsNullOrEmpty(platformTag)) &&
                 string.IsNullOrEmpty(path))
            {
                throw new ApplicationException(@"Either the combination of versionIndicator and platformTag should be provided " +
                    "or the complete agent path should be provided");
            }
            if (string.IsNullOrEmpty(installomcommand))
            {
                installomcommand = this.installOmCmd;
            }

            FileInfo agent = !string.IsNullOrEmpty(path) ? new FileInfo(path) : GetAgentLocation(versionIndicator, platformTag);
            if (!agent.Exists)
            {
                throw new FileNotFoundException(string.Format("Could not file agent at {0}", agent.FullName));
            }

            genericLogger.Write("Installing agent from: {0}", agent.FullName);
            PosixCopy copyToHost = new PosixCopy(hostName, userName, password);
            copyToHost.CopyTo(agent.FullName, "/tmp/" + agent.Name);

            RunPosixCmd execInstall = new RunPosixCmd(hostName, userName, password)
            {
                FileName = string.Format(installomcommand, agent.Name),
                Arguments = string.Empty
            };
            genericLogger.Write("Installing agent to '{0}': command: '{1}' ", hostName, execInstall.FileName);
            try
            {
                execInstall.RunCmd();
                genericLogger.Write("Install() installation out: " + execInstall.StdOut);
            }
            catch (Exception e)
            {
                this.logger(string.Format("Install agent failed on '{0}'. Error message: '{1}'", hostName, e.ToString()));
            }

            // Delete temporary file
            genericLogger.Write("Removing files in /tmp");
            execInstall.FileName = "/bin/rm";
            execInstall.Arguments = "-f /tmp/" + agent.Name;
            try
            {
                execInstall.RunCmd();
            }
            catch (Exception ex)
            {
                this.logger(string.Format("Removing files in /tmp failed. {0}", ex.ToString()));
            }
        }

        /// <summary>
        /// Installs the required agent on multiple hosts. This will use a combination of versionIndicator and platformTag
        /// or just the path parameter to figure out agent that should be installed on the client system.
        /// This is a generic install agent method to be called from varmap, for example in multihost tests
        /// </summary>
        /// <param name="hostNames">Hostnames of machines to install agent</param>
        /// <param name="versionIndicator">Indicates the version to install. Allowed values = "old"|"new".</param>
        /// <param name="platformTags">Platform tag which is part of the agent file name.</param>
        /// <param name="path">Full path of the agent. If this is specified, the other two parameters will be ignored.</param>
        /// <param name="userName">Valid user for hostname.</param>
        /// <param name="password">Password for user.</param>
        public void InstallAgent(string[] hostNames, string versionIndicator, string[] platformTags, string path, string userName, string password)
        {
            if (hostNames == null)
            {
                hostNames = this.hostName.Split(',');
            }
            if ((platformTags == null) || (hostNames.Length != platformTags.Length))
            {
                throw new ApplicationException("platfromTags not set or host count don't match platformTags.");
            }
            string[] installcommands = this.installOmCmd.Split(',');

            for (int index = 0; index < hostNames.Length; index++)
            {
                InstallAgent(hostNames[index], versionIndicator, platformTags[index], path, userName, password, installcommands[index]);
            }
        }

        /// <summary>
        /// Based on the provided input, it will return the value of the version indicated in the file name.
        /// </summary>
        /// <param name="versionIndicator">Indicates the version to install. Allowed values = "old"|"new".</param>
        /// <param name="platformTag">Platform tag which is part of the agent file name.</param>
        /// <returns>Version of the agent.</returns>
        public string GetAgentVersion(string versionIndicator, string platformTag)
        {
            FileInfo agent = GetAgentLocation(versionIndicator, platformTag);

            genericLogger.Write("Returning version info for: {0}", agent.FullName);
            return (!agent.Exists) ? 
                string.Empty : 
                Regex.Match(agent.Name, @"scx-(?<version>\d\.\d\.\d-\d+).*").Groups["version"].Value;
        }

        /// <summary>
        /// LogHelper Method to report Error and Warning Messages in the Scx*.log
        /// </summary>
        /// <param name="automationName">The name of automation, such sdk, wsman or wsmanstress.</param>
        /// <param name="logSeverity">the log file severity</param>
        /// <param name="scxlogIgnore">Ignore scxlog or not</param>
        /// <param name="platformTag">the platform tag in the test file</param>
        /// <param name="zoneName">the zone name of solaris platform</param>
        public void ScxLogHelper(string automationName, string logSeverity, bool scxlogIgnore, string platformTag, string zoneName)
        {
            RunPosixCmd runcmd = new RunPosixCmd(this.hostName, this.userName, this.password);
            PosixCopy posixCopy = new PosixCopy(this.hostName, this.userName, this.password);
            string logFileName = string.IsNullOrEmpty(zoneName) ? logFileName = string.Format("scx.test.{0}.{1}.{2}.log", automationName, platformTag, logSeverity) :
                string.Format("scx.test.{0}.{1}.{2}.{3}.log", automationName, platformTag, zoneName, logSeverity);

            string remoteFilePath = "/var/opt/microsoft/scx/log/" + logFileName;
            string localFilePath = Path.Combine(System.Environment.CurrentDirectory, logFileName);

            // Grep for LogSeverity in SCX and SCXCIMD log          
            string grepScxLog = string.Format("cat {0}|grep {1}>>{2}", scxLogPath, logSeverity, remoteFilePath);
            string grepScxCimdLog = string.Format("cat {0}|grep {1}>>{2}", scxcimLogPath, logSeverity, remoteFilePath);
            string fullCommand = string.Format("{0};{1};", grepScxLog, grepScxCimdLog);

            try
            {
                // This command will always return exit code "1". While the command execute successfully without any error message.
                // You can use command "echo $?" to see the exit code after you run the command by manual on UNIX/Linux machine.
                runcmd.RunCmd(fullCommand);
            }
            catch (Exception e)
            {
                // Exception happens when there is 0 results while doing Grep So tracing this exception
                this.logger(e.Message);
            }

            try
            {
                posixCopy.CopyFrom(remoteFilePath, localFilePath);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception in PosixCopyFrom" + ex.Message);
            }

            FileInfo file = new FileInfo(localFilePath);
            if (file.Exists && (file.Length != 0))
            {
                if (!scxlogIgnore)
                {
                    throw new FileNotFoundException(string.Format("Unexpected {0} Messages in SCX Logs(scx.log/scxcimd.log).Please see the {1} for more details", logSeverity, logFileName));
                }
            }
            else if (file.Exists && file.Length == 0)
            {
                file.Delete();
            }
        }

        /// <summary>
        /// Gets one specified agent full name
        /// </summary>
        /// <param name="versionIndicator">Indicates the version to install. Allowed values = "old"|"new".</param>
        /// <param name="platformTag">Platform tag which is part of the agent file name.</param>
        /// <returns>Agent full path of the agent which matches the search criteria.</returns>
        public string GetAgentFile(string versionIndicator, string platformTag)
        {
            if (string.IsNullOrEmpty(versionIndicator))
            {
                throw new ArgumentNullException(versionIndicator);
            }
            if (string.IsNullOrEmpty(platformTag))
            {
                throw new ArgumentNullException(platformTag);
            }

            return GetAgentLocation(versionIndicator, platformTag).FullName;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Recurse a series of subdirectories to a file, based on matching a literal value
        /// </summary>
        /// <param name="fullPath">Full path, including file name</param>
        /// <param name="rootPath">Root of staring directory</param>
        /// <param name="fileExt">File extension for agent</param>
        /// <param name="directoryArchTag">Tag text for directory name architecture</param>
        /// <returns>true = file has been found</returns>
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
                //Get the latest folder and try to locate the agent.
                genericLogger.Write("Attempting to find the new agent...");
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
                genericLogger.Write("Agent not found!");
                return false;
            }
        }

        /// <summary>
        /// Returns the right file based on the inputs.
        /// </summary>
        /// <param name="versionIndicator">Indicates the version to install. Allowed values = "old"|"new".</param>
        /// <param name="platformTag">Platform tag which is part of the agent file name.</param>
        /// <returns>File location of the agent which matches the search criteria.</returns>
        private FileInfo GetAgentLocation(string versionIndicator, string platformTag)
        {
            string agentFolder;
            switch (versionIndicator.ToLower())
            {
                case "old":
                    agentFolder = Path.Combine(Environment.CurrentDirectory, @"AgentCache\Old");
                    break;

                case "new":
                    agentFolder = localAgentPath;
                    break;

                default:
                    throw new ArgumentException("Allowed values for versioIndicator: \"new\" or \"old\"");
            }

            FileInfo[] files = new DirectoryInfo(agentFolder).GetFiles(string.Format("{0}{1}{0}", "*", platformTag));
            if (files.Length == 0)
            {
                genericLogger.Write("Could not find any file that mathes all the search criteria");
                throw new ApplicationException("Could not find any file that mathes all the search criteria");
            }

            
            // If there are multpicle files that match the criteria, 
            // this will always return the file with the latest version.
            
            return files.OrderByDescending(f => f.Name).First();
        }

        /// <summary>
        /// TODO: need to fix this
        /// This logic is to copyagents in a local management server. This needs to be changed for MultiMS scenario
        /// </summary>
        /// <param name="oldAgent">full path of old agent</param>
        /// <param name="newAgent">full path of new agent</param>
        /// <param name="platformTag">Platform tag</param>
        private void CopyAgents(FileInfo oldAgent, FileInfo newAgent, string platformTag, string omServers)
        {
            if (oldAgent == null || !oldAgent.Exists)
            {
                throw new ArgumentException("Could not locate old agent with specified data.");
            }

            genericLogger.Write("Old agent to be used: {0}", oldAgent.FullName);
            if (newAgent == null || !newAgent.Exists)
            {
                throw new ArgumentException("Could not locate new agent with specified data.");
            }

            genericLogger.Write("New agent to be used: {0}", newAgent.FullName);
            
            if (string.IsNullOrEmpty(platformTag))
            {
                throw new ArgumentNullException("PlatformTag");
            }

            const string stagingDir = @"AgentCache\old";
            if (!Directory.Exists(stagingDir))
            {
                Directory.CreateDirectory(stagingDir);
            }
            else
            {
                foreach (FileInfo file in
                    new DirectoryInfo(stagingDir).GetFiles(string.Format("{0}{1}{0}", "*", platformTag)))
                {
                    file.Delete();
                }
            }

            string oldAgentDestPath = Path.Combine(Environment.CurrentDirectory, stagingDir, oldAgent.Name);
            File.Copy(oldAgent.FullName, oldAgentDestPath, true);
            genericLogger.Write("Copied older agent to {0}", oldAgentDestPath);

            if (string.IsNullOrEmpty(omServers))
            {
                string newAgentDestPath = Path.Combine(localAgentPath, newAgent.Name);
                File.Copy(newAgent.FullName, Path.Combine(newAgentDestPath), true);
                genericLogger.Write("Copied new agent to {0}", newAgentDestPath);
            }
            else
            {
                string[] omServerArray = omServers.Split(',');

                foreach (string omserver in omServerArray)
                {
                    string localAgentPathinNetworkType = string.Format(@"\\{0}\C$\Program Files\System Center 2012\Operations Manager\Server\AgentManagement\UnixAgents", omserver);
                    string newAgentDestPath = Path.Combine(localAgentPathinNetworkType, newAgent.Name);

                    File.Copy(newAgent.FullName, Path.Combine(newAgentDestPath), true);
                    genericLogger.Write("Copied new agent to {0}", newAgentDestPath);
                }
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}
