using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.EnterpriseManagement.Monitoring;
using Scx.Test.Common;

namespace Scx.Test.Apache.SDK.ApacheSDKHelper
{
    public class ApacheAgentHelper
    {
        #region Private Fields
        /// <summary>
        /// Helper class for checking monitor status
        /// </summary>
        private MonitorHelper monitorHelper;

        /// <summary>
        /// Store information about the client machine to be discovered.
        /// </summary>
        private ClientInfo clientInfo;

        /// <summary>
        /// Logger function
        /// </summary>
        private ScxLogDelegate logger = ScxMethods.ScxNullLogDelegate;

        /// <summary>
        /// Store the POSIX command shell mechanism
        /// </summary>
        private RunPosixCmd posixCmd;

        /// <summary>
        /// Store the POSIX remote file transfer mechanism
        /// </summary>
        private PosixCopy posixCopy;

        /// <summary>
        /// Helper class to run task
        /// </summary>
        private TasksHelper tasksHelper;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ApacheAgentHelper class.
        /// </summary>
        /// <param name="rootManagementServerInfo">Information about the OM Root Management Server which will perform discovery.</param>
        public ApacheAgentHelper(OMInfo rootManagementServerInfo)
        {
            this.monitorHelper = new MonitorHelper(rootManagementServerInfo);
            this.tasksHelper = new TasksHelper(logger, rootManagementServerInfo);
        }

        /// <summary>
        /// Initializes a new instance of the ApacheAgentHelper class.
        /// </summary>
        /// <param name="rootManagementServerInfo">Information about the OM Root Management Server which will perform discovery.</param>
        /// <param name="clientInfo">Information about the client machine to discover.</param>
        public ApacheAgentHelper(OMInfo rootManagementServerInfo, ClientInfo clientInfo)
            : this(rootManagementServerInfo)
        {
            this.SetClientInfo(clientInfo);
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Add client information to the current instance
        /// </summary>
        /// <param name="clientInfo">Information about the client</param>
        public void SetClientInfo(ClientInfo clientInfo)
        {
            this.clientInfo = clientInfo;

            this.posixCmd = new RunPosixCmd(
                string.IsNullOrEmpty(clientInfo.HostName) ? clientInfo.IPAddr : clientInfo.HostName,
                clientInfo.SuperUser,
                clientInfo.SuperUserPassword);

            this.posixCopy = new PosixCopy(
                string.IsNullOrEmpty(clientInfo.HostName) ? clientInfo.IPAddr : clientInfo.HostName,
                clientInfo.SuperUser,
                clientInfo.SuperUserPassword);
        }

        /// <summary>
        /// Verify Apache agent is installed on client (IsManaged? is true or false)
        /// </summary>
        /// <returns>Whether apache agent is insalled on client</returns>
        public bool VerifyApacheAgentInstalled()
        {
            MonitoringObject apacheServerInstance;

            try
            {
                //IPHostEntry hostList = Dns.GetHostEntry(this.clientInfo.HostName);
                apacheServerInstance = this.monitorHelper.GetMonitoringObject("Microsoft.ApacheHTTPServer.Installation", this.clientInfo.HostName);
            }
            catch (Exception)
            {
                this.logger("Unable to find computer object with name: " + this.clientInfo.HostName);
                return false;
            }

            return apacheServerInstance.IsManaged;
        }

        /// <summary>
        /// Verify Apache agent is installed on client (IsManaged? is true or false)
        /// </summary>
        /// <returns>Whether apache agent is insalled on client</returns>
        public MonitoringObject GetVirtualHostMonitor(string instanceID)
        {
            MonitoringObject apacheServerInstance;

            try
            {
                //IPHostEntry hostList = Dns.GetHostEntry(this.clientInfo.HostName);
                apacheServerInstance = this.monitorHelper.GetMonitoringObject("Microsoft.ApacheHTTPServer.VirtualHost.Unix", instanceID);
            }
            catch (Exception e)
            {
                this.logger("Unable to find computer object with name: " + instanceID);
                throw e;
            }

            return apacheServerInstance;
        }

        /// <summary>
        /// Install an agent on Posix host. Set AgentArchitecture and DirectoryTag properties.
        /// </summary>
        /// <remarks>WaitForNewAgent is optional.  If it is not run, then FullApachePath must be set.</remarks>
        public void InstallApacheAgentWihCommand(string apcheAgentFullPath, string apcheTag)
        {
            if (string.IsNullOrEmpty(apcheAgentFullPath) == true)
            {
                throw new ArgumentNullException("FullApachePath not set");
            }

            string apacheAgentName = SetApacheAgentFullPath(apcheAgentFullPath, apcheTag);

            try
            {
                this.RunCmd(string.Format("sh /tmp/{0} --install", apacheAgentName));
            }
            catch (Exception e)
            {
                throw new Exception("Install apache CimProv agent failed: " + e.Message);
            }

        }

        /// <summary>
        /// Install an agent with task "Install/Upgrade Apache cim module".
        /// </summary>
        /// <remarks>WaitForNewAgent is optional.  If it is not run, then FullApachePath must be set.</remarks>
        public bool InstallApacheAgentWihTask()
        {

            MonitoringObject apacheServerInstance;

            try
            {
                //IPHostEntry hostList = Dns.GetHostEntry(this.clientInfo.HostName);
                apacheServerInstance = this.monitorHelper.GetMonitoringObject("Microsoft.ApacheHTTPServer.Installation", this.clientInfo.HostName);
            }
            catch (Exception)
            {
                this.logger("Unable to find computer object with name: " + this.clientInfo.HostName);
                return false;
            }
            IList<Microsoft.EnterpriseManagement.Runtime.TaskResult> taskResult = this.tasksHelper.RunConsumerTask(apacheServerInstance, "Microsoft.ApacheHTTPServer.DeployModule.Task");

            if (taskResult != null)
            {
                this.logger(String.Format("Pass: Consumer task execute fine:{0}", "Microsoft.ApacheHTTPServer.Installation"));
            }
            else
            {
                throw new Exception("Consumer Task Executes Fail!");
            }

            if (taskResult[0].Status == Microsoft.EnterpriseManagement.Runtime.TaskStatus.Succeeded)
            {
                this.logger(string.Format("Pass: expectedTaskStatus: {0}, actualTaskStatus: {1}", Microsoft.EnterpriseManagement.Runtime.TaskStatus.Succeeded, taskResult[0].Status));
            }
            else
            {
                throw new Exception(string.Format("Fail: expectedTaskStatus: {0}, actualTaskStatus: {1}", Microsoft.EnterpriseManagement.Runtime.TaskStatus.Succeeded, taskResult[0].Status));
            }

            return true;
        }

        /// <summary>
        /// Install an agent on Posix host. Set AgentArchitecture and DirectoryTag properties.
        /// </summary>
        /// <remarks>WaitForNewAgent is optional.  If it is not run, then FullApachePath must be set.</remarks>
        public void UninstallApacheAgentWihCommand(string apcheAgentFullPath, string apcheTag)
        {
            if (string.IsNullOrEmpty(apcheAgentFullPath) == true)
            {
                throw new ArgumentNullException("FullApachePath not set");
            }

            string apacheAgentName = SetApacheAgentFullPath(apcheAgentFullPath, apcheTag);

            try
            {
                this.RunCmd(string.Format("sh /tmp/{0} --purge", apacheAgentName));
            }
            catch (Exception e)
            {
                throw new Exception("Install apache CimProv agent failed: " + e.Message);
            }

        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Set the apacheFullPath.
        /// </summary>
        /// <returns>The apache agent file name</returns>
        /// <remarks>WaitForNewAgent is optional.  If it is not run, then FullApachePath must be set.</remarks>
        private string SetApacheAgentFullPath(string apcheAgentFullPath, string apcheTag)
        {
            //"Searching for apache in " + this.fullApacheAgentPath;
            DirectoryInfo di = new DirectoryInfo(apcheAgentFullPath);
            FileInfo[] fi = di.GetFiles("*" + apcheTag + "*");
            if (fi.Length == 0)
            {
                throw new Exception("Found no apache installer matching ApacheTag: " + apcheTag);
            }

            if (fi.Length > 1)
            {
                throw new Exception("Found more than one apache installer matching ApacheTag: " + apcheTag);
            }

            // User-specified Apache path takes precedent
            string apacheAgentName = Path.GetFileName(fi[0].FullName);

            PosixCopy copyToHost = new PosixCopy(this.clientInfo.HostName, this.clientInfo.SuperUser, this.clientInfo.SuperUserPassword);
            // Copy from server to Posix host
            this.logger("Copying Apache from drop server to host");
            copyToHost.CopyTo(fi[0].FullName, "/tmp/" + apacheAgentName);

            return apacheAgentName;
        }

        /// <summary>
        /// Run special cmd.
        /// </summary>
        /// <remarks>WaitForNewAgent is optional.  If it is not run, then FullApachePath must be set.</remarks>
        private RunPosixCmd RunCmd(string cmd, string arguments = "")
        {
            // Begin cmd
            RunPosixCmd execCmd = new RunPosixCmd(this.clientInfo.HostName, this.clientInfo.SuperUser, this.clientInfo.SuperUserPassword);

            // Execute command
            execCmd.FileName = cmd;
            execCmd.Arguments = arguments;
            this.logger(string.Format("Run Command {0} on host {1} ", execCmd.FileName, this.clientInfo.HostName));
            execCmd.RunCmd();
            this.logger(string.Format("Command {0} out: {1}", execCmd.FileName, execCmd.StdOut));
            return execCmd;
        }

        #endregion

        #endregion

        #region Properties
        /// <summary>
        /// Sets the log file delegate
        /// </summary>
        public ScxLogDelegate Logger
        {
            set
            {
                this.logger = value;
                //this.tasksHelper.Logger = value;
            }
        }

        #endregion
    }
}
