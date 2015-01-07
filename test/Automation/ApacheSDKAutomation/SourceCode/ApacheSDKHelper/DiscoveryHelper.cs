//-----------------------------------------------------------------------
// <copyright file="DiscoveryHelper.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>a-brdust</author>
// <description></description>
// <history>3/30/2009 1:01:41 PM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Apache.SDK.ApacheSDKHelper
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Threading;
    using System.Xml;
    using Microsoft.EnterpriseManagement;
    using Microsoft.EnterpriseManagement.Common;
    using Microsoft.EnterpriseManagement.Configuration;
    using Microsoft.EnterpriseManagement.ConnectorFramework;
    using Microsoft.EnterpriseManagement.Monitoring;
    using Scx.Test.Common;

    /// <summary>
    /// Description for DiscoveryHelper.
    /// </summary>
    public class DiscoveryHelper
    {
        #region Private Fields

        /// <summary>
        /// Store the local management group.
        /// </summary>
        private ManagementGroup managementGroup;

        /// <summary>
        /// Connector GUID used to load MCF Administration framework.
        /// </summary>
        private Guid connectorGuid;

        /// <summary>
        /// Host name of the OM RMS installation
        /// </summary>
        private string rootManagementServer;

        /// <summary>
        /// Host name of the OM installation used for discovery.
        /// This host must be in the same management group as rootManagementServer, and
        /// may or may not be the same as rootManagementServer
        /// </summary>
        private string managementServer;

        /// <summary>
        /// OM default resource pool name;
        /// </summary>
        private string defaultResourcePool;

        /// <summary>
        /// Path to the SCX CIMD log on the remote client
        /// </summary>
        private string scxcimLogPath = "/var/opt/microsoft/scx/log/scxcimd.log";

        /// <summary>
        /// MCF Administration framework.
        /// </summary>
        private IConnectorFrameworkManagement mcfAdmin;

        /// <summary>
        /// Monitoring Connector used to monitor the discovered client.
        /// </summary>
        private MonitoringConnector monitoringConnector;

        /// <summary>
        /// Store the POSIX command shell mechanism
        /// </summary>
        private RunPosixCmd posixCmd;

        /// <summary>
        /// Store the POSIX remote file transfer mechanism
        /// </summary>
        private PosixCopy posixCopy;

        /// <summary>
        /// Store information about the client machine to be discovered.
        /// </summary>
        private ClientInfo clientInfo;

        /// <summary>
        /// Helper class for executing tasks
        /// </summary>
        private TasksHelper tasksHelper;

        /// <summary>
        /// Helper class for checking monitor status
        /// </summary>
        private MonitorHelper monitorHelper;

        /// <summary>
        /// Operations Manager certificate signing task name
        /// </summary>
        private string certificateSigningTaskName = "Microsoft.Unix.Agent.GetCert.Task";

        /// <summary>
        /// Monitoring class name of the operations manager server
        /// </summary>
        private string serverMonitoringClassName = "Microsoft.SystemCenter.ManagementServicePool";

        /// <summary>
        /// Monitoring class of the operations manager server
        /// </summary>
        private ManagementPackClass serverMonitoringClass;

        /// <summary>
        /// List of monitoring objects containing a single element representing the operations manager server
        /// </summary>
        private List<MonitoringObject> serverTargetList;

        /// <summary>
        /// Logger function
        /// </summary>
        private ScxLogDelegate logger = ScxMethods.ScxNullLogDelegate;

        /// <summary>
        /// CredentialSet object used as elevation authentition, unpreviledged username, password, and supassword
        /// </summary>
        private Credential scxElevatedCredential;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DiscoveryHelper class.
        /// </summary>
        /// <param name="rootManagementServerInfo">Information about the OM Root Management Server which will perform discovery.</param>
        public DiscoveryHelper(OMInfo rootManagementServerInfo)
        {
            this.managementGroup = rootManagementServerInfo.LocalManagementGroup;

            this.connectorGuid = new Guid("0607F2C4-A652-4ece-87BB-795ACA31E457"); // 0607F2C4-A652-4ece-87BB-795ACA31E457 // 0607F2C4-A652-4ece-87BB-795ACA31E456

            this.mcfAdmin = this.managementGroup.ConnectorFramework;

            this.rootManagementServer = rootManagementServerInfo.OMServer;
            this.managementServer = rootManagementServerInfo.OMServer;
            this.defaultResourcePool = rootManagementServerInfo.OMDefaultResourcePool;

            this.monitorHelper = new MonitorHelper(rootManagementServerInfo);

            this.tasksHelper = new TasksHelper(ScxMethods.ScxNullLogDelegate, rootManagementServerInfo);

            this.serverMonitoringClass = this.tasksHelper.GetMonitoringClass(this.serverMonitoringClassName);

            this.serverTargetList = this.tasksHelper.GetTaskTargetList(this.serverMonitoringClass, this.defaultResourcePool);

            try
            {
                this.monitoringConnector = this.mcfAdmin.GetConnector(this.connectorGuid);
            }
            catch (ObjectNotFoundException)
            {
                // the connector does not exist, so create it
                ConnectorInfo connectorInfo = new ConnectorInfo();
                connectorInfo.DiscoveryDataIsManaged = true;
                connectorInfo.Description = "This is a Unix Computer Connector to used by BVTs";
                connectorInfo.DisplayName = "BVT Unix Computer Connector";
                connectorInfo.Name = "BVT Unix Computer Connector";
                this.monitoringConnector = this.mcfAdmin.Setup(connectorInfo, this.connectorGuid);
            }

            if (!this.monitoringConnector.Initialized)
            {
                this.monitoringConnector.Initialize();
            }

            this.SetManagementServer(this.rootManagementServer);
            this.UseAgentMaintenanceAccount = false;
        }

        /// <summary>
        /// Initializes a new instance of the DiscoveryHelper class.
        /// </summary>
        /// <param name="rootManagementServerInfo">Information about the OM Root Management Server which will perform discovery.</param>
        /// <param name="clientInfo">Information about the client machine to discover.</param>
        public DiscoveryHelper(OMInfo rootManagementServerInfo, ClientInfo clientInfo)
            : this(rootManagementServerInfo)
        {
            this.SetClientInfo(clientInfo);
            this.scxElevatedCredential = new Credential(clientInfo.User, "su", clientInfo.UserPassword, clientInfo.SuperUserPassword, string.Empty, string.Empty);
        }

        /// <summary>
        /// Initializes a new instance of the DiscoveryHelper class.
        /// </summary>
        /// <param name="rootManagementServerInfo">Information about the OM Root Management Server which will perform discovery.</param>
        /// <param name="clientInfo">Information about the client machine to discover.</param>
        /// <param name="managementServer">The OM Management Server to use.  This may differ from the Root Management Server,
        /// but must be part of the same management group.</param>
        public DiscoveryHelper(OMInfo rootManagementServerInfo, ClientInfo clientInfo, string managementServer)
            : this(rootManagementServerInfo, clientInfo)
        {
            this.SetManagementServer(managementServer);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the MonitorHelper
        /// </summary>
        public MonitorHelper MonitorHelperField
        {
            get { return this.monitorHelper; }
            set { this.monitorHelper = value; }
        }

        /// <summary>
        /// Gets the RunPosixCmd
        /// </summary>
        public RunPosixCmd PosixCmd
        {
            get { return this.posixCmd; }
        }

        /// <summary>
        /// Gets the clientInof occasiated with the DiscoveryHelper
        /// </summary>
        public ClientInfo ClientInfo
        {
            get { return this.clientInfo; }
        }

        /// <summary>
        /// Gets List of monitoring objects containing a single element representing the UNIX\Linux host since SDK test 1 unix\linux computer one time.
        /// </summary>
        public List<MonitoringObject> TargetList
        {
            get
            {
                List<MonitoringObject> targetList = null;
                const int MaxTries = 5;
                int tries = 0;
                while (tries < MaxTries)
                {
                    MonitoringObject computerObject = this.monitorHelper.GetComputerObject(this.clientInfo.HostName);
                    if (computerObject != null)
                    {
                        targetList = new List<MonitoringObject> { computerObject };
                        break;
                    }
                    else
                    {
                        tries++;
                        this.logger(string.Format("Fail to get the monitoring object for host '{0}': tries={1}/{2}", this.clientInfo.HostName, tries, MaxTries));
                        System.Threading.Thread.Sleep(30000);
                    }
                }

                return targetList;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether using agent maintenance account in deploy, install, unistall or not.
        /// </summary>
        public bool UseAgentMaintenanceAccount
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets agent maintenance account 
        /// </summary>
        public Credential AgentMaintenanceCredential
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the log file delegate
        /// </summary>
        public ScxLogDelegate Logger
        {
            set
            {
                this.logger = value;
                this.tasksHelper.Logger = value;
            }
        }

        #endregion Properties

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
        /// Add management server information to the current instance
        /// </summary>
        /// <param name="managementServer">The management server to be used for discovery. 
        /// This may be different than the Root Management Server which hosts the SDK service.</param>
        public void SetManagementServer(string managementServer)
        {
            //// this.managementServer = managementServer;

            this.serverTargetList = this.tasksHelper.GetTaskTargetList(this.serverMonitoringClass, this.defaultResourcePool);
        }

        /// <summary>
        /// Wait until it is possible to verify the client is added to OM and correctly monitoring
        /// at least the heartbeat and certificate monitors on the agent.
        /// </summary>
        /// <param name="logger">log delegate method</param>
        /// <param name="minutesToWait">Number of minutes to wait for client verification</param>
        public void WaitForClientVerification(ScxLogDelegate logger, int minutesToWait)
        {
            bool clientVerified = this.VerifySystemInOM();

            int numTries = 0;

            while (numTries < minutesToWait && !clientVerified)
            {
                Thread.Sleep(new TimeSpan(0, 1, 0));

                numTries++;

                clientVerified = this.VerifySystemInOM();

                logger(string.Format("VerifySystem({0})={1} after {2}/{3} tries", this.clientInfo.HostName, clientVerified, numTries, minutesToWait));
            }

            if (clientVerified)
            {
                logger("client verified: " + this.clientInfo.HostName);
            }
            else
            {
                throw new Exception("client verification failed: " + this.clientInfo.HostName);
            }
        }

        /// <summary>
        /// Add system to Operations Manager using SSH discovery
        /// </summary>
        public void DiscoverClientSSH()
        {
            string discoveryTaskName = "Microsoft.Unix.SSH.Discovery.Task";

            ManagementPackTask task = this.tasksHelper.GetMonitoringTask(discoveryTaskName);

            Pair<string, string>[] configOverrideParams = new Pair<string, string>[] 
            {
                new Pair<string, string>("Host", this.clientInfo.HostName),
                new Pair<string, string>("TargetSystem", this.clientInfo.HostName),
                new Pair<string, string>("Port", "22"),
                new Pair<string, string>("UserName", this.clientInfo.SuperUser),
                new Pair<string, string>("Password", this.clientInfo.SuperUserPassword)
            };

            Microsoft.EnterpriseManagement.Runtime.TaskConfiguration monitorTaskConfig = this.tasksHelper.GetMonitoringTaskConfiguration(task, configOverrideParams);

            IList<Microsoft.EnterpriseManagement.Runtime.TaskResult> results =
                this.tasksHelper.RunTask(this.serverTargetList, task, monitorTaskConfig);

            if (results.Count != 1)
            {
                throw new DiscoveryHelperException(string.Format("RunTask returned {0} results; expecting 1", results.Count));
            }

            XmlDocument resultXml = new XmlDocument();
            resultXml.LoadXml(results[0].Output);
            XmlElement resultXmlRoot = resultXml.DocumentElement;

            string returnCode = this.GetXmlNodeValue(resultXmlRoot, "returnCode");
            string sshStdOut = this.GetXmlNodeValue(resultXmlRoot, "stdout");

            if (returnCode != "0")
            {
                throw new DiscoveryHelperException(string.Format("SSH Discvory had returnCode={0}, stdout={1}; expecting returnCode=0", returnCode, sshStdOut));
            }

            string sshHostName = this.clientInfo.HostName;
            string sshIPAddr = this.clientInfo.IPAddr;
            string[] sshStdOutParts = sshStdOut.Split(new char[] { ' ' });
            string sshUnameArchitecture = sshStdOutParts[sshStdOutParts.Length - 1];

            this.logger("DiscoverClientSSH: sshUnameArch: " + sshUnameArchitecture);

            Pair<string, string>[] monitoringPropertyValues = new Pair<string, string>[] 
            {
                new Pair<string, string>("PrincipalName", sshHostName),
                new Pair<string, string>("DNSName", sshHostName),
                new Pair<string, string>("IPAddress", sshIPAddr),
                new Pair<string, string>("NetworkName", sshHostName),
                new Pair<string, string>("DisplayName", sshHostName),
                new Pair<string, string>("Architecture", sshUnameArchitecture)
            };

            this.InsertClientToDB(monitoringPropertyValues);
        }

        /// <summary>
        /// Add system to Operations Manager using WSMAN discovery
        /// </summary>
        public void DiscoverClientWSMan()
        {
            string discoveryTaskName = "Microsoft.Unix.WSMan.Discovery.Task";

            ManagementPackTask task = this.tasksHelper.GetMonitoringTask(discoveryTaskName);
            
            // Wsman now only support taking the regular username and password, rathar than the xml username and xml password.
            Pair<string, string>[] configOverrideParams = new Pair<string, string>[] 
            {
                new Pair<string, string>("Host", this.clientInfo.HostName),
                new Pair<string, string>("TargetSystem", this.clientInfo.HostName),
                new Pair<string, string>("Port", "22"),
                new Pair<string, string>("UserName", this.clientInfo.SuperUser),
                new Pair<string, string>("Password", this.clientInfo.SuperUserPassword)
            };

            Microsoft.EnterpriseManagement.Runtime.TaskConfiguration monitorTaskConfig = 
                this.tasksHelper.GetMonitoringTaskConfiguration(task, configOverrideParams);

            IList<Microsoft.EnterpriseManagement.Runtime.TaskResult> results =
                this.tasksHelper.RunTask(this.serverTargetList, task, monitorTaskConfig);

            if (results.Count != 1)
            {
                throw new DiscoveryHelperException(string.Format("RunTask returned {0} results; expecting 1", results.Count));
            }

            XmlDocument resultXml = new XmlDocument();
            resultXml.LoadXml(results[0].Output);
            XmlElement resultXmlRoot = resultXml.DocumentElement;

            string wsmanHostName = this.GetXmlNodeValue(resultXmlRoot, "p:Hostname");
            string wsmanIPAddr = Dns.GetHostEntry(wsmanHostName).AddressList[0].ToString();
            string wsmanAgentVersion = this.GetXmlNodeValue(resultXmlRoot, "p:VersionString");
            string wsmanUnameArchitecture = this.GetXmlNodeValue(resultXmlRoot, "p:UnameArchitecture");

            Pair<string, string>[] monitoringPropertyValues = new Pair<string, string>[] 
            {
                new Pair<string, string>("PrincipalName", wsmanHostName),
                new Pair<string, string>("DNSName", wsmanHostName),
                new Pair<string, string>("IPAddress", wsmanIPAddr),
                new Pair<string, string>("NetworkName", wsmanHostName),
                new Pair<string, string>("DisplayName", wsmanHostName),
                new Pair<string, string>("AgentVersion", wsmanAgentVersion),
                new Pair<string, string>("Architecture", wsmanUnameArchitecture),
                new Pair<string, string>("SSHPort", "22")
            };

            this.InsertClientToDB(monitoringPropertyValues);
        }

        /// <summary>
        /// Deletes system from Operations Manager.
        /// </summary>
        public void DeleteSystemFromOM()
        {
            MonitoringObject computerObject;

            try
            {
                computerObject = this.monitorHelper.GetComputerObject(this.clientInfo.HostName);
            }
            catch (Exception)
            {
                this.logger("Unable to find computer object with name: " + this.clientInfo.HostName);
                return;
            }

            IncrementalDiscoveryData discoveryData = new IncrementalDiscoveryData();
            discoveryData.Remove(computerObject);
            discoveryData.Commit(this.monitoringConnector);
        }

        /// <summary>
        /// Deletes all system from Operations Manager.
        /// </summary>
        /// <param name="targetComputerClass">The target computer class, such as Microsoft.Unix.Computer</param>
        public void DeleteAllSystemsFromOM(string targetComputerClass)
        {
            IncrementalDiscoveryData discoveryData = new IncrementalDiscoveryData();
            ManagementPackClassCriteria classesQuery = new ManagementPackClassCriteria(string.Format("Name = '{0}'", targetComputerClass));
            IList<ManagementPackClass> monitoringClasses = this.managementGroup.EntityTypes.GetClasses(classesQuery);
            IObjectReader<MonitoringObject> monitoringObjects;
            monitoringObjects = this.managementGroup.EntityObjects.GetObjectReader<MonitoringObject>(monitoringClasses[0], ObjectQueryOptions.Default);
            foreach (MonitoringObject computerObject in monitoringObjects)
            {
                this.logger("Deleting from OM: " + computerObject);
                discoveryData.Remove(computerObject);
                discoveryData.Commit(this.monitoringConnector);
            }
        }

        /// <summary>
        /// Verify status of target system in Operations Manager (heartbeat and certificate monitors)
        /// </summary>
        /// <returns>Whether target system can be verified as monitored in Operations Manager.</returns>
        public bool VerifySystemInOM()
        {
            bool certVerified = false;
            bool heartbeatVerified = false;

            MonitoringObject computerObject;

            try
            {
                //IPHostEntry hostList = Dns.GetHostEntry(this.clientInfo.HostName);
                computerObject = this.monitorHelper.GetComputerObject(this.clientInfo.HostName);
            }
            catch (Exception)
            {
                this.logger("Unable to find computer object with name: " + this.clientInfo.HostName);
                return false;
            }

            MonitorInfo[] monitors = this.monitorHelper.GetMonitorInfoArray(computerObject);

            foreach (MonitorInfo monitor in monitors)
            {
                if (monitor.Name.Contains("Heartbeat"))
                {
                    if (monitor.Health == HealthState.Success)
                    {
                        heartbeatVerified = true;
                    }
                    else
                    {
                        this.logger("Could not verify system in OM: Heartbeat failed.");
                        return false;
                    }
                }

                if (monitor.Name.Contains("Certificate") && !monitor.Name.Contains("Apache"))
                {
                    if (monitor.Health == HealthState.Success)
                    {
                        certVerified = true;
                    }
                    else
                    {
                        this.logger("Could not verify system in OM: Certificate failed.");
                        return false;
                    }
                }
            }

            if (!heartbeatVerified)
            {
                this.logger("Could not verify system in OM: Heartbeat monitor not initialized.");
                return false;
            }

            if (!certVerified)
            {
                this.logger("Could not verify system in OM: Certificate monitor not initialized.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Cleans the test system.
        /// </summary>
        public void CleanupRemoteClient()
        {
            this.posixCmd.RunCmd(this.clientInfo.CleanupCommand);

            // record uninstallation in SCX CIMD log
            try
            {
                this.posixCmd.RunCmd(string.Format("echo \"{0} INFO discoveryHelper: agent uninstalled by {1} (cleanup command)\" >> {2}", DateTime.Now.ToString(), this.managementServer, this.scxcimLogPath));
            }
            catch (Exception ex)
            {
                this.logger(ex.Message);
            }
        }

        /// <summary>
        /// Uninstalls the agent from the remote system, without doing any cleanup.
        /// </summary>
        public void UninstallClientTask()
        {
            string taskName = this.clientInfo.ManagementPackName + ".Agent.Uninstall.Task";

            ManagementPackTask task = this.tasksHelper.GetMonitoringTask(taskName);

            // Uninstall task support taking both regular and xml user name and password. Use username and password in old basic scenarios while Agent Maintenance account using xml username\password to increase coverage.
            Pair<string, string>[] configOverrideParams = null;
            if (this.UseAgentMaintenanceAccount)
            {
                configOverrideParams = new Pair<string, string>[] 
                {
                    new Pair<string, string>("Host", this.clientInfo.HostName),
                };
            }
            else
            {
                configOverrideParams = new Pair<string, string>[] 
                {
                    new Pair<string, string>("Host", this.clientInfo.HostName),
                    new Pair<string, string>("UserName", this.clientInfo.SuperUser),
                    new Pair<string, string>("Password", this.clientInfo.SuperUserPassword)
                };
            }

            Microsoft.EnterpriseManagement.Runtime.TaskConfiguration monitorTaskConfig = this.tasksHelper.GetMonitoringTaskConfiguration(task, configOverrideParams);

            IList<Microsoft.EnterpriseManagement.Runtime.TaskResult> results =
                this.tasksHelper.RunTask(this.TargetList, task, monitorTaskConfig);

            // record uninstallation in SCX CIMD log
            this.posixCmd.RunCmd(string.Format("echo \"{0} INFO discoveryHelper: agent uninstalled by {1} (uninstall task)\" >> {2}", DateTime.Now.ToString(), this.managementServer, this.scxcimLogPath));
        }

        /// <summary>
        /// Uninstalls the agent from the remote system, without doing any cleanup for Astro's test coverage improve.
        /// </summary>
        /// <param name="taskName">The name of elevated uninstall agent task</param>
        /// <returns>return the results of the task</returns>
        public IList<Microsoft.EnterpriseManagement.Runtime.TaskResult> ElevatedUninstallClient(string taskName)
        {
            ManagementPackTask task = this.tasksHelper.GetMonitoringTask(taskName);

            // Uninstall task support taking both regular and xml user name and password. For elevation, we need to pass the xml user name and xml password.
            Pair<string, string>[] configOverrideParams = new Pair<string, string>[] 
            {
                new Pair<string, string>("Host", this.clientInfo.HostName),
                new Pair<string, string>("UserName", this.scxElevatedCredential.XmlUserName),
                new Pair<string, string>("Password", this.scxElevatedCredential.XmlPassword)
            };

            Microsoft.EnterpriseManagement.Runtime.TaskConfiguration monitorTaskConfig = this.tasksHelper.GetMonitoringTaskConfiguration(task, configOverrideParams);
            
            IList<Microsoft.EnterpriseManagement.Runtime.TaskResult> results =
                this.tasksHelper.RunTask(this.TargetList, task, monitorTaskConfig);

            // record uninstallation in SCX CIMD log
            this.posixCmd.RunCmd(string.Format("echo \"{0} INFO discoveryHelper: agent uninstalled by {1} (uninstall task)\" >> {2}", DateTime.Now.ToString(), this.managementServer, this.scxcimLogPath));

            return results;
        }

        /// <summary>
        /// Installs the agent on the remote system
        /// </summary>
        /// <param name="kitFilePath">The full path to the agent file</param>
        public void InstallClient(string kitFilePath)
        {
            this.logger("Deploying client from " + kitFilePath);
            string installerName = this.DeployClient(kitFilePath);

            this.logger("Installing client: " + installerName);

            string taskName = this.clientInfo.ManagementPackName + ".Agent.Install.Task";

            ManagementPackTask task = this.tasksHelper.GetMonitoringTask(taskName);

            // Install task support taking both regular and xml user name and password. Use username and password in old basic scenarios while Agent Maintenance account using xml username\password to increase coverage.
            Pair<string, string>[] configOverrideParams;
            if (this.UseAgentMaintenanceAccount)
            {
                configOverrideParams = new Pair<string, string>[] 
                {
                    new Pair<string, string>("Host", this.clientInfo.HostName),
                    new Pair<string, string>("UserName", this.AgentMaintenanceCredential.XmlUserName),
                    new Pair<string, string>("Password", this.AgentMaintenanceCredential.XmlPassword),
                    new Pair<string, string>("InstallPackage", installerName)
                };
            }
            else
            {
                configOverrideParams = new Pair<string, string>[] 
                {
                    new Pair<string, string>("Host", this.clientInfo.HostName),
                    new Pair<string, string>("UserName", this.clientInfo.SuperUser),
                    new Pair<string, string>("Password", this.clientInfo.SuperUserPassword),
                    new Pair<string, string>("InstallPackage", installerName)
                };
            }

            Microsoft.EnterpriseManagement.Runtime.TaskConfiguration monitorTaskConfig = this.tasksHelper.GetMonitoringTaskConfiguration(task, configOverrideParams);

            IList<Microsoft.EnterpriseManagement.Runtime.TaskResult> results =
                this.tasksHelper.RunTask(this.serverTargetList, task, monitorTaskConfig);

            // record installation in SCX CIMD log
            this.posixCmd.RunCmd(string.Format("echo \"{0} INFO discoveryHelper: agent {1} installed by {2} from {3} using {4}\" >> {5}", DateTime.Now.ToString(), installerName, this.managementServer, kitFilePath, taskName, this.scxcimLogPath));
        }

        /// <summary>
        /// Get available agent by enumAvailableAgentsTask
        /// </summary>        
        /// <returns>return task result</returns>
        public IList<Microsoft.EnterpriseManagement.Runtime.TaskResult> GetAvailableAgentList()
        {
            this.logger("Enumerate available agents.");
            string enumAvailableAgentsTaskName = "Microsoft.Unix.EnumerateAvailableAgents.Task";
            ManagementPackTask task = this.tasksHelper.GetMonitoringTask(enumAvailableAgentsTaskName);

            Pair<string, string>[] configOverrideParams = new Pair<string, string>[] 
                {
                };

            Microsoft.EnterpriseManagement.Runtime.TaskConfiguration monitorTaskConfig = 
                this.tasksHelper.GetMonitoringTaskConfiguration(task, configOverrideParams);

            IList<Microsoft.EnterpriseManagement.Runtime.TaskResult> results =
                this.tasksHelper.RunTask(this.serverTargetList, task, monitorTaskConfig);

            return results;
        }

        /// <summary>
        /// Installs the agent on the remote system using evaluate task for Astro's Test coverage improvement
        /// </summary>
        /// <param name="kitFilePath">The full path to the agent file</param>
        /// <param name="taskName">The name of elevated install agent task</param>
        /// <returns>return the results of the task</returns>
        public IList<Microsoft.EnterpriseManagement.Runtime.TaskResult> ElevatedInstallClient(string kitFilePath, string taskName)
        {
            this.logger("Deploying client from " + kitFilePath);
            string installerName = this.DeployClient(kitFilePath, true);

            this.logger("Installing client: " + installerName);

            ManagementPackTask task = this.tasksHelper.GetMonitoringTask(taskName);

            // Install task support taking both regular and xml user name and password. For elevation, we need to pass the xml user name and xml password.
            Pair<string, string>[] configOverrideParams = new Pair<string, string>[] 
            {
                new Pair<string, string>("Host", this.clientInfo.HostName),
                new Pair<string, string>("Port", "22"),
                new Pair<string, string>("UserName", this.scxElevatedCredential.XmlUserName),
                new Pair<string, string>("Password", this.scxElevatedCredential.XmlPassword),
                new Pair<string, string>("InstallPackage", installerName)
            };

            Microsoft.EnterpriseManagement.Runtime.TaskConfiguration monitorTaskConfig = this.tasksHelper.GetMonitoringTaskConfiguration(task, configOverrideParams);

            IList<Microsoft.EnterpriseManagement.Runtime.TaskResult> results =
                this.tasksHelper.RunTask(this.serverTargetList, task, monitorTaskConfig);

            // record installation in SCX CIMD log
            this.posixCmd.RunCmd(string.Format("echo \"{0} INFO discoveryHelper: agent {1} installed by {2} from {3} using {4}\" >> {5}", DateTime.Now.ToString(), installerName, this.managementServer, kitFilePath, taskName, this.scxcimLogPath));

            return results;
        }

        /// <summary>
        /// Upgrades the agent on the remote system
        /// </summary>
        /// <param name="kitFilePath">The full path to the new agent file</param>
        public void UpgradeClient(string kitFilePath)
        {
            this.logger("Deploying client from " + kitFilePath);
            string installerName = this.DeployClient(kitFilePath);

            this.logger("Upgrading client: " + installerName);

            string taskName = this.clientInfo.ManagementPackName + ".Agent.Upgrade.Task";

            ManagementPackTask task = this.tasksHelper.GetMonitoringTask(taskName);

            // Upgrade task support taking both regular ("root") and xml user name and password. Use username and password in old basic scenarios while Agent Maintenance account using xml username\password to increase coverage.
            Pair<string, string>[] configOverrideParams = null;
            if (this.UseAgentMaintenanceAccount)
            {
                configOverrideParams = new Pair<string, string>[] 
                {
                    new Pair<string, string>("Host", this.clientInfo.HostName),
                    new Pair<string, string>("InstallPackage", installerName)
                };
            }
            else
            {
                configOverrideParams = new Pair<string, string>[] 
                {
                    new Pair<string, string>("Host", this.clientInfo.HostName),
                    new Pair<string, string>("UserName", this.clientInfo.SuperUser),
                    new Pair<string, string>("Password", this.clientInfo.SuperUserPassword),
                    new Pair<string, string>("InstallPackage", installerName)
                };
            }

            Microsoft.EnterpriseManagement.Runtime.TaskConfiguration monitorTaskConfig = this.tasksHelper.GetMonitoringTaskConfiguration(task, configOverrideParams);

            IList<Microsoft.EnterpriseManagement.Runtime.TaskResult> results =
                this.tasksHelper.RunTask(this.TargetList, task, monitorTaskConfig);
        }
        
        /// <summary>
        /// Upgrades the agent on the remote system for Astro's test coverage improvement
        /// </summary>
        /// <param name="kitFilePath">The full path to the new agent file</param>
        /// <param name="taskName">The name of elevated upgrade agent task</param>
        /// <returns>return the results of the task</returns>
        public IList<Microsoft.EnterpriseManagement.Runtime.TaskResult> ElevatedUpgradeClient(string kitFilePath, string taskName)
        {
            this.logger("Deploying client from " + kitFilePath);
            string installerName = this.DeployClient(kitFilePath, true);

            this.logger("Upgrading client: " + installerName);

            ManagementPackTask task = this.tasksHelper.GetMonitoringTask(taskName);

            // Upgrade task support taking both regular and xml user name and password. For elevation, we need to pass the xml user name and xml password.
            Pair<string, string>[] configOverrideParams = new Pair<string, string>[] 
            {
                new Pair<string, string>("Host", this.clientInfo.HostName),
                new Pair<string, string>("Port", "22"),
                new Pair<string, string>("UserName", this.scxElevatedCredential.XmlUserName),
                new Pair<string, string>("Password", this.scxElevatedCredential.XmlPassword),
                new Pair<string, string>("InstallPackage", installerName)
            };

            Microsoft.EnterpriseManagement.Runtime.TaskConfiguration monitorTaskConfig = this.tasksHelper.GetMonitoringTaskConfiguration(task, configOverrideParams);

            IList<Microsoft.EnterpriseManagement.Runtime.TaskResult> results =
                this.tasksHelper.RunTask(this.TargetList, task, monitorTaskConfig);
            return results;
        }

        /// <summary>
        /// Create a signed certificate using OpsMgr tasks.
        /// </summary>
        public void CreateSignedCert()
        {
            ManagementPackTask task = this.tasksHelper.GetMonitoringTask(this.certificateSigningTaskName);

            // GetCert task support taking both regular and xml user name and password. Here we use xml username and passwod to make it persistant with the elevate methods.
            Pair<string, string>[] configOverrideParams = new Pair<string, string>[] 
            {
                new Pair<string, string>("Host", this.clientInfo.HostName),
                new Pair<string, string>("Port", "22"),
                new Pair<string, string>("UserName", this.clientInfo.SuperUser),
                new Pair<string, string>("Password", this.clientInfo.SuperUserPassword)
            };

            Microsoft.EnterpriseManagement.Runtime.TaskConfiguration monitorTaskConfig = this.tasksHelper.GetMonitoringTaskConfiguration(task, configOverrideParams);

            IList<Microsoft.EnterpriseManagement.Runtime.TaskResult> results =
                this.tasksHelper.RunTask(this.serverTargetList, task, monitorTaskConfig);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Retrieve the value of a given Xml Node, for example if p:HostName is the name of a node, return the value.
        /// </summary>
        /// <param name="resultXmlRoot">XML Document to examine</param>
        /// <param name="nodeName">Name of the node to retrieve the value of</param>
        /// <returns>The value of the node</returns>
        private string GetXmlNodeValue(XmlElement resultXmlRoot, string nodeName)
        {
            string result;

            XmlNodeList nodeList = resultXmlRoot.GetElementsByTagName(nodeName);
            if (nodeList.Count != 1)
            {
                throw new DiscoveryHelperException(string.Format("{0} XML nodes matching {1}; expecting 1 match within outer XML: '{2}'", nodeList.Count, nodeName, resultXmlRoot.OuterXml));
            }

            result = nodeList[0].InnerText.Trim();

            if (string.IsNullOrEmpty(result))
            {
                throw new DiscoveryHelperException(string.Format("Null or empty contents of {0}", nodeName));
            }

            return result;
        }

        /// <summary>
        /// Insert the client into the Operations Manager Database.  The client computer object will be modified to hold
        /// the monitoring property values in monitoringPropertyValues.  The first element of each pair is the monitoring property
        /// name, the second value is the corresponding monitoring property value.
        /// </summary>
        /// <param name="monitoringPropertyValues">An array of key/value pairs holding the monitoring property values required for the client computer object</param>
        private void InsertClientToDB(Pair<string, string>[] monitoringPropertyValues)
        {
            IncrementalDiscoveryData discoveryData = new IncrementalDiscoveryData();

            ManagementPackClass posixComputerClass = this.GetMonitoringClass("Microsoft.Unix.Computer");

            ManagementPackClass clientComputerClass = this.GetMonitoringClass(this.clientInfo.TargetComputerClass);

            ManagementPackClass systemEntityClass = this.GetMonitoringClass("System.Entity");

            CreatableEnterpriseManagementObject clientComputerObject = new CreatableEnterpriseManagementObject(this.managementGroup, clientComputerClass);

            foreach (Pair<string, string> value in monitoringPropertyValues)
            {
                if (value.First == "DisplayName")
                {
                    clientComputerObject[systemEntityClass.PropertyCollection[value.First]].Value = value.Second;
                }
                else
                {
                    clientComputerObject[posixComputerClass.PropertyCollection[value.First]].Value = value.Second;
                }
            }

            ManagementPackClass managementServicePoolClass =
                this.GetMonitoringClass("Microsoft.SystemCenter.ManagementServicePool");

            ManagementPackRelationship shouldManageClass =
                this.GetMonitoringRelationshipClass("Microsoft.SystemCenter.ManagementActionPointShouldManageEntity");

            CreatableEnterpriseManagementRelationshipObject relationshipObject =
                new CreatableEnterpriseManagementRelationshipObject(this.managementGroup, shouldManageClass);
            
            MonitoringObject managementServicePoolObject =
                this.GetMonitoringObject(managementServicePoolClass, this.defaultResourcePool);

            relationshipObject.SetSource(managementServicePoolObject);
            relationshipObject.SetTarget(clientComputerObject);

            discoveryData.Add(relationshipObject);
            discoveryData.Add(clientComputerObject);
            discoveryData.Commit(this.monitoringConnector);
        }

        /// <summary>
        /// Deploys the agent to the remote system.  Made private to avoid confusion in the interface, and because the method is never used
        /// except as a prerequisite to another method call.
        /// </summary>
        /// <param name="kitFilePath">Full path the agent</param>
        /// <param name="forElevatedAgentTask">true: deploy agent into \tmp\scx-scxuser; false:deploy agent into \tmp\scx-root </param>
        /// <returns>Uncompressed name of the deployed file</returns>
        private string DeployClient(string kitFilePath, bool forElevatedAgentTask)
        {
            string kitFileName = Path.GetFileName(kitFilePath);

            string architecture = this.clientInfo.Architecture;

            // The management packs use a different string for "powerpc" than the agent names.
            if (architecture == "ppc")
            {
                architecture = "powerpc";
            }

            // On MacOS, the architecture name in in the management pack task is incorrect (x86, should be x64).  Known issue.
            if (this.clientInfo.PlatformTag.Contains("macos"))
            {
                architecture = "x86";
            }

            string taskName = string.Concat(this.clientInfo.ManagementPackName, ".", architecture, ".Agent.Deploy.Task");

            ManagementPackTask task = this.tasksHelper.GetMonitoringTask(taskName);

            Pair<string, string>[] configOverrideParams = null;

            string username = forElevatedAgentTask ? this.clientInfo.User : this.clientInfo.SuperUser;
            string password = forElevatedAgentTask ? this.clientInfo.UserPassword : this.clientInfo.SuperUserPassword;
            if (forElevatedAgentTask)
            {
                // Deploy task only support taking xml user name and password. 
                configOverrideParams = new Pair<string, string>[] 
                {
                    new Pair<string, string>("Host", this.clientInfo.HostName),
                    new Pair<string, string>("Port", "22"),
                    new Pair<string, string>("UserName", this.scxElevatedCredential.XmlUserName),
                    new Pair<string, string>("Password", this.scxElevatedCredential.XmlPassword),
                    new Pair<string, string>("SourceFile", kitFilePath),
                    new Pair<string, string>("TargetFile", kitFileName)
                };
            }
            else if (this.UseAgentMaintenanceAccount)
            {
                configOverrideParams = new Pair<string, string>[] 
                {
                    new Pair<string, string>("Host", this.clientInfo.HostName),
                    new Pair<string, string>("Port", "22"),
                    new Pair<string, string>("SourceFile", kitFilePath),
                    new Pair<string, string>("TargetFile", kitFileName)
                };
            }
            else 
            {
                // Deploy task only support taking xml user name and password. 
                configOverrideParams = new Pair<string, string>[] 
                {
                    new Pair<string, string>("Host", this.clientInfo.HostName),
                    new Pair<string, string>("Port", "22"),
                    new Pair<string, string>("UserName", this.clientInfo.SuperUser),
                    new Pair<string, string>("Password", this.clientInfo.SuperUserPassword),
                    new Pair<string, string>("SourceFile", kitFilePath),
                    new Pair<string, string>("TargetFile", kitFileName)
                };
            }

            Microsoft.EnterpriseManagement.Runtime.TaskConfiguration monitorTaskConfig = this.tasksHelper.GetMonitoringTaskConfiguration(task, configOverrideParams);

            IList<Microsoft.EnterpriseManagement.Runtime.TaskResult> results =
                this.tasksHelper.RunTask(this.serverTargetList, task, monitorTaskConfig);

            string installerName = kitFileName;

            if (kitFileName.EndsWith(".Z"))
            {
                installerName = kitFileName.Substring(0, kitFileName.LastIndexOf("."));
            }
            else if (kitFileName.EndsWith(".gz"))
            {
                installerName = kitFileName.Substring(0, kitFileName.LastIndexOf("."));
            }

            this.logger("returning installerName = " + installerName);

            return installerName;
        }

        /// <summary>
        /// Deploys the agent to the remote system.  Made private to avoid confusion in the interface, and because the method is never used
        /// except as a prerequisite to another method call.
        /// </summary>
        /// <param name="kitFilePath">Full path the agent</param>
        /// <returns>Uncompressed name of the deployed file</returns>
        private string DeployClient(string kitFilePath)
        {
            return this.DeployClient(kitFilePath, false);
        }

        /// <summary>
        /// Retrieves the monitoring class matching a given class name.
        /// </summary>
        /// <param name="className">The name of the class to match, for example, Microsoft.unix.computer</param>
        /// <returns>The MonitoringClass instance</returns>
        private ManagementPackClass GetMonitoringClass(string className)
        {
            ManagementPackClassCriteria classesQuery = new ManagementPackClassCriteria(string.Format("Name = '{0}'", className));
            IList<ManagementPackClass> monitoringClasses = this.managementGroup.EntityTypes.GetClasses(classesQuery);

            if (monitoringClasses.Count == 0)
            {
                throw new DiscoveryHelperException("Failed to find monitoring class " + className);
            }

            return monitoringClasses[0];
        }

        /// <summary>
        /// Get the Monitoring Relationship Class.
        /// </summary>
        /// <param name="relationshipName">Relationship Name</param>
        /// <returns>Monitoring Relationship Class</returns>
        private ManagementPackRelationship GetMonitoringRelationshipClass(string relationshipName)
        {
            IList<ManagementPackRelationship> relationshipClasses;
            ManagementPackRelationshipCriteria classesQuery = new ManagementPackRelationshipCriteria(string.Format("Name = '{0}'", relationshipName));
            relationshipClasses = this.managementGroup.EntityTypes.GetRelationshipClasses(classesQuery);
            if (relationshipClasses.Count == 0)
            {
                throw new DiscoveryHelperException("Failed to find monitoring relationship " + relationshipName);
            }

            return relationshipClasses[0];
        }

        /// <summary>
        /// Retrieve the monitoring object
        /// </summary>
        /// <param name="objectClass">Monitoring Class</param>
        /// <param name="principalName">Principal Name</param>
        /// <returns>Monitoring Object</returns>
        private MonitoringObject GetMonitoringObject(ManagementPackClass objectClass, string principalName)
        {
            IObjectReader<MonitoringObject> monitoringObjects;
            EnterpriseManagementObjectCriteria query = new EnterpriseManagementObjectCriteria(
                string.Format("DisplayName = '{0}'", principalName),
                objectClass);
            monitoringObjects = this.managementGroup.EntityObjects.GetObjectReader<MonitoringObject>(query, ObjectQueryOptions.Default);
            if (monitoringObjects.Count == 0)
            {
                throw new DiscoveryHelperException("Failed to find monitoring object " + principalName);
            }

            return monitoringObjects.GetData(0);
        }

        #endregion Private Methods

        #endregion Methods
    }
    
    /// <summary>
    /// An exception specific to the ManageMonitors class
    /// </summary>
    public class DiscoveryHelperException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the DiscoveryHelperException class.
        /// </summary>
        /// <param name="msg">A message describing the nature of the problem.</param>
        public DiscoveryHelperException(string msg)
            : base(msg)
        {
        }
    }
}
