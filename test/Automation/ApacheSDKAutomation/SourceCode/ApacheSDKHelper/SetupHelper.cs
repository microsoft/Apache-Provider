//-----------------------------------------------------------------------
// <copyright file="SetupHelper.cs" company="Microsoft">
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
    using System.IO;
    using System.Threading;
    using Microsoft.EnterpriseManagement.Configuration;
    using Microsoft.EnterpriseManagement.Monitoring;
    using Scx.Test.Common;

    /// <summary>
    /// Description for SetupHelper.
    /// </summary>
    public class SetupHelper
    {
        #region Private Fields

        /// <summary>
        /// Imformation about the OM server.
        /// </summary>
        private OMInfo info;

        /// <summary>
        /// Client information
        /// </summary>
        private ClientInfo clientInfo;

        /// <summary>
        /// Monitor helper class
        /// </summary>
        private MonitorHelper monitorHelper;

        /// <summary>
        /// Helper class to check status of alerts.
        /// </summary>
        private AlertsHelper alertHelper;

        /// <summary>
        /// Computer object
        /// </summary>
        private MonitoringObject computerObject;

        /// <summary>
        /// Discovery helper class
        /// </summary>
        private DiscoveryHelper discoveryHelper;

        /// <summary>
        /// AgentHelper class from ScxCommon for manual finding agent on server.
        /// </summary>
        private AgentHelper agentHelper;

        /// <summary>
        /// Full path to the old agent
        /// </summary>
        private string fullNewAgentPath;

        /// <summary>
        /// Path to the agent cache; deployment tasks cannot deploy directly from a network share.
        /// </summary>
        private string agentCachePath = "agentCache";

        /// <summary>
        /// Format to string to use in reporting known issues.  Contains two fields, Issue ID and Issue description.
        /// </summary>
        private string knownIssueFormat;

        /// <summary>
        /// Period of time to wait for OM Server to update its internal state to match changes on the agent.
        /// </summary>
        private TimeSpan serverWaitTime = new TimeSpan(0, 1, 0);

        /// <summary>
        /// Time at which test run began
        /// </summary>
        private DateTime testGroupStart;

        /// <summary>
        /// Maximum number of times to wait for server state change
        /// </summary>
        private int maxServerWaitCount = 10;
        
        /// <summary>
        /// The log delegate method
        /// </summary>
        private ScxLogDelegate logger = ScxMethods.ScxNullLogDelegate;

        /// <summary>
        /// The MCF IContext object
        /// </summary>
        private ScxContextValueDelegate contextValue = ScxMethods.ScxNullContextValueDelegateDelegate;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SetupHelper class.
        /// </summary>
        /// <param name="logger">Log delegate method</param>
        /// <param name="contextValue">MCF context object</param>
        /// <param name="discoverClient">Whether to discover target computer client</param>
        public SetupHelper(ScxLogDelegate logger, ScxContextValueDelegate contextValue, bool discoverClient)
        {
            this.logger = logger;
            this.contextValue = contextValue;

            bool clientDiscovered = false;

            bool latestOnly = true;
            bool useLocalAgents = false;

            this.testGroupStart = DateTime.Now;

            this.knownIssueFormat = this.contextValue("knownIssueFormat");

            this.info = new OMInfo(
                    this.contextValue("omserver"),
                    this.contextValue("omusername"),
                    this.contextValue("omdomain"),
                    this.contextValue("ompassword"),
                    this.contextValue("defaultresourcepool"));

            this.clientInfo = new ClientInfo(
                this.contextValue("hostname"),
                this.contextValue("ipaddress"),
                this.contextValue("targetcomputerclass"),
                this.contextValue("managementpack"),
                this.contextValue("architecture"),
                this.contextValue("nonsuperuser"),
                this.contextValue("nonsuperuserpwd"),
                this.contextValue("superuser"),
                this.contextValue("superuserpwd"),
                this.contextValue("packagename"),
                this.contextValue("cleanupcommand"),
                this.contextValue("platformtag"));

            this.discoveryHelper = new DiscoveryHelper(this.info, this.clientInfo);
            this.discoveryHelper.Logger = this.logger;

            this.monitorHelper = new MonitorHelper(this.info);

            this.alertHelper = new AlertsHelper(this.info);

            this.agentHelper = new AgentHelper(
                this.logger,
                this.clientInfo.HostName,
                this.clientInfo.SuperUser,
                this.clientInfo.SuperUserPassword,
                "echo", //// this agent helper instance will never perform an install
                this.clientInfo.CleanupCommand);

            this.agentHelper.AgentPkgExt = this.contextValue("AgentPkgExt");
            this.agentHelper.DirectoryTag = this.contextValue("DirectoryTag");
            this.agentHelper.DropLocation = this.contextValue("remoteagents");

            this.agentHelper.VerifySSH();

            this.agentHelper.SynchDateTime();

            this.agentCachePath = Path.Combine(System.Environment.CurrentDirectory, this.agentCachePath);

            // determine whether to search for older agents if agent is not present in the newest folder on the drop server
            latestOnly = this.contextValue("latestonly").Equals("false") ? false : true;

            // determine whether to search for older agents if agent is not present in the newest folder on the drop server
            useLocalAgents = this.contextValue("uselocalagents").Equals("true") ? true : false;

            clientDiscovered = this.discoveryHelper.VerifySystemInOM();

            if (discoverClient && !clientDiscovered)
            {
                if (useLocalAgents)
                {
                    string localAgents = this.contextValue("localagents");

                    this.logger("Searching for agent in " + localAgents);
                    DirectoryInfo di = new DirectoryInfo(localAgents);
                    FileInfo[] fi = di.GetFiles("*" + this.clientInfo.PlatformTag + "*");

                    if (fi.Length == 0)
                    {
                        throw new FileNotFoundException("Found no agent installer matching platformtag: " + this.clientInfo.PlatformTag);
                    }

                    if (fi.Length > 1)
                    {
                        throw new FileNotFoundException("Found more than one agent installer matching platformtag: " + this.clientInfo.PlatformTag);
                    }

                    this.fullNewAgentPath = fi[0].FullName;
                }
                else
                {
                    if (!Directory.Exists(this.agentCachePath))
                    {
                        this.logger("Creating agent cache directory: " + this.agentCachePath);
                        Directory.CreateDirectory(this.agentCachePath);
                    }

                    this.logger("Searching for new agent");
                    this.agentHelper.FindAgent(false, string.Empty, 5, latestOnly);
                    this.fullNewAgentPath = Path.Combine(this.agentCachePath, Path.GetFileName(this.agentHelper.FullAgentPath));

                    if (File.Exists(this.fullNewAgentPath))
                    {
                        this.logger("Agent already in cache: " + this.fullNewAgentPath);
                    }
                    else
                    {
                        this.logger("Copying agent to: " + this.fullNewAgentPath);
                        File.Copy(this.agentHelper.FullAgentPath, this.fullNewAgentPath);
                    }
                }

                this.logger("Cleaning up remote system using SSH");
                this.CleanupAgent();

                this.logger("Deploying/installing agent on client machine");
                this.discoveryHelper.InstallClient(this.fullNewAgentPath);

                this.logger("Signing agent");
                this.discoveryHelper.CreateSignedCert();

                this.logger("Adding client to OM");
                this.discoveryHelper.DiscoverClientWSMan();
            }

            this.discoveryHelper.WaitForClientVerification(this.logger, this.maxServerWaitCount);

            this.computerObject = this.monitorHelper.GetComputerObject(this.clientInfo.HostName);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets OMInfo which contains data about an Operations Manager installation
        /// </summary>
        public OMInfo OMServerInfo
        {
            get
            {
                return this.info;
            }
        }

        /// <summary>
        /// Gets ClientInfo which contains information about the client machine used during discovery by Operations Manager
        /// </summary>
        public ClientInfo OMClientInfo
        {
            get
            {
                return this.clientInfo;
            }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Verifies that the monitor specified is initialized
        /// </summary>
        /// <param name="monitorName">Monitor Name</param>
        /// <param name="monitorTarget">Monitor Target</param>
        /// <param name="minutesToWait">Number of minutes to wait for initialization</param>
        public void WaitForMonitorInitialization(string monitorName, string monitorTarget, int minutesToWait)
        {
            string monitorTag = string.IsNullOrEmpty(monitorTarget) ? monitorName : string.Format("{0} -> {1}", monitorName, monitorTarget);

            int numTries = 0;

            HealthState monitorHealth = this.monitorHelper.GetMonitorHealthState(this.computerObject, monitorName, monitorTarget);

            this.logger("WaitForMonitorInitialization: " + monitorTag);

            while (numTries < minutesToWait && monitorHealth == HealthState.Uninitialized)
            {
                if (numTries > 0)
                {
                    System.Threading.Thread.Sleep(new TimeSpan(0, 1, 0));
                }

                if (string.IsNullOrEmpty(monitorTarget))
                {
                    monitorHealth = this.monitorHelper.GetMonitorHealthState(this.computerObject, monitorName);
                }
                else
                {
                    monitorHealth = this.monitorHelper.GetMonitorHealthState(this.computerObject, monitorName, monitorTarget);
                }

                numTries++;

                if (monitorHealth != HealthState.Uninitialized)
                {
                    this.logger("Monitor initialized: state = " + monitorHealth.ToString());
                    return;
                }

                this.logger(string.Format("monitor {0} in state: {1} after {2}/{3} tries.", monitorName, monitorHealth.ToString(), numTries, minutesToWait));
            }

            throw new Exception(string.Format("monitor {0} in state: {1} after {2}/{3} tries.", monitorName, monitorHealth.ToString(), numTries, minutesToWait));
        }

        /// <summary>
        /// If the monitor is in an error state, attempt a recovery.
        /// </summary>
        /// <param name="monitorName">Name of the monitor</param>
        public void RecoverMonitorIfFailed(string monitorName)
        {
            string targetOSClass = this.contextValue("targetosclass");

            string recoveryName = monitorName.Replace(".Monitor", ".Restart");

            int numTries = 0;
            int maxTries = 10;

            HealthState monitorHealth = HealthState.Uninitialized;

            while (numTries < maxTries && monitorHealth == HealthState.Uninitialized)
            {
                if (numTries > 0)
                {
                    System.Threading.Thread.Sleep(new TimeSpan(0, 1, 0));
                }

                monitorHealth = this.monitorHelper.GetMonitorHealthState(this.computerObject, monitorName);

                numTries++;

                if (monitorHealth == HealthState.Success)
                {
                    this.logger("monitor in state: " + monitorHealth.ToString());
                    return;
                }

                this.logger(string.Format("monitor {0} in state: {1} after {2}/{3} tries.", monitorName, monitorHealth.ToString(), numTries, maxTries));
            }

            if (monitorHealth == HealthState.Error)
            {
                this.logger("RecoverMonitorIfFailed(): recovering: " + monitorName);

                RecoveryHelper recoveryHelper = new RecoveryHelper(recoveryName, this.info.LocalManagementGroup);

                RecoveryResult recoveryResult = recoveryHelper.SubmitRecoveryOn(
                    "Microsoft.Unix.OperatingSystem",
                    targetOSClass + ":" + this.clientInfo.HostName,
                    monitorName);

                if (recoveryResult.Status != TaskStatus.Succeeded)
                {
                    throw new Exception("Recovery FAILED: " + recoveryName);
                }
                else
                {
                    this.logger("Recovery successful: " + recoveryName);
                }
            }
        }

        /// <summary>
        /// Verify that the syslog service is running on the remote client.  This requires a manual check
        /// via SSH to ensure there is no spurious healthy state.  See WI 13972.
        /// </summary>
        public void VerifySyslogStatus()
        {
            RunPosixCmd posixCmd = new RunPosixCmd(
                this.clientInfo.HostName,
                this.clientInfo.SuperUser,
                this.clientInfo.SuperUserPassword);

            string psArgs = "-ef";

            string syslogProcessName = "syslog";

            if (this.clientInfo.PlatformTag.Contains("macos"))
            {
                psArgs = "-x";
            }

            try
            {
                posixCmd.RunCmd(string.Format("ps {0} | grep {1} | grep -v grep | grep -c .", psArgs, syslogProcessName));
            }
            catch (Exception ex)
            {
                this.logger("ps command generated exception: " + ex.Message);
            }

            int processCount = int.Parse(posixCmd.StdOut);

            if (processCount <= 0)
            {
                this.logger("Syslog not running on remote client.  Waiting for OpsMgr to detect error (See WI 13972)");

                string monitorName = this.clientInfo.ManagementPackName + ".Process.Syslog.Monitor";

                int numTries = 0;

                HealthState monitorHealth = HealthState.Uninitialized;

                while (numTries < this.maxServerWaitCount && monitorHealth != HealthState.Error)
                {
                    Thread.Sleep(new TimeSpan(0, 1, 0));

                    monitorHealth = this.monitorHelper.GetMonitorHealthState(this.computerObject, monitorName);

                    numTries++;

                    if (monitorHealth == HealthState.Error)
                    {
                        this.logger("monitor in state: " + monitorHealth.ToString());
                        return;
                    }

                    this.logger(string.Format("monitor {0} in state: {1} after {2}/{3} tries.", monitorName, monitorHealth.ToString(), numTries, this.maxServerWaitCount));
                }
            }
        }

        /// <summary>
        /// Verifies that the monitor specified in the testing context is in the required state
        /// </summary>
        /// <param name="monitorName">Name of the monitor to verify</param>
        /// <param name="monitorTarget">Target of the monitor to verify, e.g., '/tmp'</param>
        /// <param name="requiredState">Required monitor state</param>
        public void VerifyMonitor(string monitorName, string monitorTarget, HealthState requiredState)
        {
            string monitorTag = string.IsNullOrEmpty(monitorTarget) ? monitorName : string.Format("{0} -> {1}", monitorName, monitorTarget);

            HealthState monitorHealth = HealthState.Uninitialized;

            int numTries = 0;

            this.logger("VerifyMonitor: " + monitorTag);

            while (numTries < this.maxServerWaitCount && monitorHealth != requiredState)
            {
                if (numTries > 0)
                {
                    this.Wait();
                }

                if (string.IsNullOrEmpty(monitorTarget))
                {
                    monitorHealth = this.monitorHelper.GetMonitorHealthState(this.computerObject, monitorName);
                }
                else
                {
                    monitorHealth = this.monitorHelper.GetMonitorHealthState(this.computerObject, monitorName, monitorTarget);
                }

                numTries++;

                if (monitorHealth == requiredState)
                {
                    this.logger("PASS: monitor in state: " + monitorHealth.ToString());
                    return;
                }

                this.logger(string.Format("monitor {0} in state: {1} after {2}/{3} tries.", monitorName, monitorHealth.ToString(), numTries, this.maxServerWaitCount));
            }

            throw new Exception(string.Format("monitor {0} in state: {1} after {2}/{3} tries.", monitorName, monitorHealth.ToString(), numTries, this.maxServerWaitCount));
        }

        /// <summary>
        /// Verifies that the alert specified in the testing context is active
        /// </summary>
        /// <param name="ruleName">Verify alert matching this rule name</param>
        /// <param name="shouldExist">Whether the alert is expected to be active.</param>
        public void VerifyAlert(string ruleName, bool shouldExist)
        {
            MonitoringAlert alert = this.alertHelper.GetRuleAlert(this.computerObject, ruleName);

            this.logger(string.Format("VerifyAlert: {0}  shouldExist: {1}", ruleName, shouldExist));

            bool alertExists = !shouldExist;

            int numTries = 0;

            while (numTries < this.maxServerWaitCount && alertExists != shouldExist)
            {
                if (numTries > 0)
                {
                    Thread.Sleep(new TimeSpan(0, 1, 0));
                }

                numTries++;

                alert = this.alertHelper.GetRuleAlert(this.computerObject, ruleName);

                alertExists = this.alertHelper.IsActive(alert);

                if (alertExists)
                {
                    this.logger(string.Format("alert {0} -> \"{1}\" in state: active after {2}/{3} tries.", ruleName, alert.Name, numTries, this.maxServerWaitCount));
                }
                else
                {
                    this.logger(string.Format("alert {0} in state: inactive after {1}/{2} tries.", ruleName, numTries, this.maxServerWaitCount));
                }
            }

            if (alertExists == shouldExist)
            {
                this.logger("PASS: alert in state: " + (alertExists ? "active" : "inactive"));
            }
            else if (alert != null && !string.IsNullOrEmpty(alert.Name))
            {
                throw new Exception(string.Format("alert {0}->\"{1}\" in state: {2}", ruleName, alert.Name, (alertExists ? "active" : "inactive")));
            }
            else
            {
                throw new Exception(string.Format("alert {0} in state: {1}", ruleName, (alertExists ? "active" : "inactive")));
            }
        }

        /// <summary>
        /// Closes the currently active alert
        /// </summary>
        /// <param name="ruleName">Close alerts matching this rule name</param>
        public void CloseAlert(string ruleName)
        {
            MonitoringAlert alert = this.alertHelper.GetRuleAlert(this.computerObject, ruleName);

            this.logger(string.Format("Closing alert {0} -> {1}", ruleName, alert.Name));
            this.alertHelper.CloseAlert(alert);
        }

        /// <summary>
        /// Closes all alerts relevant to the current test
        /// </summary>
        /// <param name="ruleName">Close alerts matching this rule name</param>
        public void CloseMatchingAlerts(string ruleName)
        {
            List<MonitoringAlert> alerts = this.alertHelper.GetRuleAlerts(this.computerObject, ruleName);

            this.logger(string.Format("Closing {0} pre-existing alerts.", alerts.Count));

            foreach (MonitoringAlert alert in alerts)
            {
                this.logger(string.Format("Closing alert {0} -> {1}", ruleName, alert.Name));
                this.alertHelper.CloseAlert(alert);
            }
        }

        /// <summary>
        /// Generic wait method for use to allow OM internal state to stabilize.
        /// </summary>
        public void Wait()
        {
            this.logger(string.Format("Waiting for {0}...", this.serverWaitTime));
            System.Threading.Thread.Sleep(this.serverWaitTime);
        }

        /// <summary>
        /// Overloaded Wait method for use to allow OM Internal state to stabilize
        /// </summary>
        /// <param name="hours">Hours in timespan to sleep</param>
        /// <param name="minutes">Minutes in timespan to sleep</param>
        /// <param name="seconds">Seconds in timespan to sleep</param>
        public void Wait(int hours, int minutes, int seconds)
        {
            this.logger(string.Format("Waiting for {0}hours {1} minutes {2} seconds...", hours, minutes, seconds));
            System.Threading.Thread.Sleep(new TimeSpan(hours, minutes, seconds));
        }

        /// <summary>
        /// Cleanup after test
        /// </summary>
        public void Cleanup()
        {
            if (this.contextValue("installonly") != "true")
            {
                this.discoveryHelper.DeleteSystemFromOM();

                this.UninstallAgent();
            }

            TimeSpan testDuration = DateTime.Now - this.testGroupStart;

            this.logger("Test group complete after " + testDuration.ToString());
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Uninstall from the remote client using Discovery Uninstall Task, taking zones into account
        /// </summary>
        private void UninstallAgent()
        {
            // ignore exceptions from cleanup
            try
            {
                this.logger("Uninstalling agent from client machine");
                this.discoveryHelper.UninstallClientTask();
            }
            catch (Exception e)
            {
                this.logger("Client Uninstall failed: " + e.Message);
            }
        }

        /// <summary>
        /// Clean up agent from the remote client, taking zones into account
        /// </summary>
        private void CleanupAgent()
        {
            // ignore exceptions from cleanup
            try
            {
                this.logger("Cleaning agent from client machine");
                this.discoveryHelper.CleanupRemoteClient();
            }
            catch (Exception e)
            {
                this.logger("Client cleanup failed: " + e.Message);
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}
