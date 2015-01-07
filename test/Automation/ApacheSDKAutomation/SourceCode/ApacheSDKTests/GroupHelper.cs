//-----------------------------------------------------------------------
// <copyright file="GroupHelper.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>a-brdust</author>
// <description></description>
// <history>3/25/2009 2:44:42 PM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Apache.SDK.ApacheSDKTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Infra.Frmwrk;
    using Microsoft.EnterpriseManagement.Monitoring;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions;
    using Common;
    using Scx.Test.Apache.SDK.ApacheSDKHelper;

    /// <summary>
    /// Description for GroupHelper.
    /// </summary>
    public class GroupHelper : ISetup, ICleanup
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
        /// Management Pack helper class
        /// </summary>
        private ManageMP manageMP;

        /// <summary>
        /// Monitor helper class
        /// </summary>
        private MonitorHelper monitorHelper;

        /// <summary>
        /// Discovery helper class
        /// </summary>
        private DiscoveryHelper discoveryHelper;

        /// <summary>
        /// Apache agent helper class
        /// </summary>
        private ApacheAgentHelper apacheAgentHelper;

        /// <summary>
        /// AgentHelper class from ScxCommon for manual finding agent on server.
        /// </summary>
        private AgentHelper agentHelper;

        /// <summary>
        /// A MonitoroingObject representing the client machine.
        /// </summary>
        private MonitoringObject computerObject;

        /// <summary>
        /// A MonitoroingObject representing the Operating System.
        /// </summary>
        private MonitoringObject OperatingSystemObject;
         
        /// <summary>
        /// Full path to the old agent
        /// </summary>
        private string fullNewAgentPath;

        /// <summary>
        /// Full path to the apache Agent
        /// </summary>
        private string fullApacheAgentPath;

        /// <summary>
        /// Path to the agent cache; deployment tasks cannot deploy directly from a network share.
        /// </summary>
        private string agentCachePath = "agentCache";

        /// <summary>
        /// Period of time to wait for OM Server to update its internal state to match changes on the agent.
        /// </summary>
        private readonly TimeSpan serverWaitTime = new TimeSpan(0, 1, 0);

        /// <summary>
        /// Time at which test run began
        /// </summary>
        private DateTime testGroupStart;

        /// <summary>
        /// Maximum number of times to wait for server state change
        /// </summary>
        private const int maxServerWaitCount = 10;

        /// <summary>
        /// Default timeout in milliseconds for normal remote posix commands which should complete quickly.
        /// </summary>
        private const int posixCommandTimeoutMS = 5000;

        /// <summary>
        /// The Location of Apache Cim module
        /// </summary>
        private string tempApacheCIMModuleLocation = @"C:\Windows\Temp\ApacheCimProv";
        
        /// <summary>
        /// Setting the installonly option
        /// </summary>
        private bool installOnly = false;

        #endregion Private Fields

        #region Constructors

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Methods

        #region Test Framework Methods

        /// <summary>
        /// Test framework setup method
        /// </summary>
        /// <param name="ctx">Current context</param>
        void ISetup.Setup(IContext ctx)
        {
            try
            {
                ctx.Trc("ApacheSDKTests.GroupHelper.Setup");

                this.testGroupStart = DateTime.Now;

                bool latestOnly;
                bool useLocalAgents;

                try
                {
                    this.info = new OMInfo(
                            ctx.Records.GetValue("omserver"),
                            ctx.Records.GetValue("omusername"),
                            ctx.Records.GetValue("omdomain"),
                            ctx.Records.GetValue("ompassword"),
                            ctx.Records.GetValue("defaultresourcepool"));
                }
                catch (Exception ex)
                {
                    ctx.Alw("Exception thrown by OMInfo.ctor().  Make sure the test is run with elevated priviledges.");
                    ctx.Alw("Exception Message: " + ex.Message);
                    ctx.Alw("Exception StackTrace: " + ex.StackTrace);
                    if (ex.InnerException != null)
                    {
                        ctx.Alw("InnerException Message: " + ex.InnerException.Message);
                        ctx.Alw("InnerException StackTrace: " + ex.InnerException.StackTrace);
                    }
                }

                this.clientInfo = new ClientInfo(
                    ctx.Records.GetValue("hostname"),
                    ctx.Records.GetValue("ipaddress"),
                    ctx.Records.GetValue("targetcomputerclass"),
                    ctx.Records.GetValue("managementpack"),
                    ctx.Records.GetValue("architecture"),
                    ctx.Records.GetValue("nonsuperuser"),
                    ctx.Records.GetValue("nonsuperuserpwd"),
                    ctx.Records.GetValue("superuser"),
                    ctx.Records.GetValue("superuserpwd"),
                    ctx.Records.GetValue("packagename"),
                    ctx.Records.GetValue("cleanupcommand"),
                    ctx.Records.GetValue("platformtag"));

                ctx.Alw("OMInfo: " + Environment.NewLine + this.info);
                ctx.Alw("ClientInfo: " + Environment.NewLine + this.clientInfo);

                this.discoveryHelper = new DiscoveryHelper(this.info, this.clientInfo) {Logger = ctx.Trc};

                this.apacheAgentHelper = new ApacheAgentHelper(this.info, this.clientInfo) {Logger = ctx.Trc};

                this.monitorHelper = new MonitorHelper(this.info);
                this.agentHelper = new AgentHelper(
                    ctx.Trc,
                    this.clientInfo.HostName,
                    this.clientInfo.SuperUser,
                    this.clientInfo.SuperUserPassword,
                    "echo", //// this agent helper instance will never perform an install
                    this.clientInfo.CleanupCommand)
                                       {
                                           AgentPkgExt = ctx.Records.GetValue("AgentPkgExt"),
                                           DirectoryTag = ctx.Records.GetValue("DirectoryTag"),
                                           DropLocation = ctx.Records.GetValue("remoteagents")
                                       };

                this.agentHelper.VerifySSH();

                this.agentHelper.SynchDateTime();

                // Configure the agent maintenance account
                if (ctx.Records.HasKey("UseAgentMaintenanceAccount") && ctx.Records.GetValue("UseAgentMaintenanceAccount").Equals("true"))
                {
                    this.discoveryHelper.UseAgentMaintenanceAccount = true;
                    this.ConfigureAgentMaintenceAccount(ctx);
                }

                this.agentCachePath = Path.Combine(Environment.CurrentDirectory, this.agentCachePath);
                if (ctx.Records.HasKey("testingoverride"))
                {
                    string overrideMp = ctx.Records.GetValue("testingoverride");

                    if (!string.IsNullOrEmpty(overrideMp))
                    {
                        this.manageMP = new ManageMP(ctx.Alw, this.info)
                                            {
                                                ManagementPackDirectory =
                                                    Path.Combine(Environment.CurrentDirectory, "Overrides")
                                            };
                         
                        // import test override
                        this.manageMP.ImportManagementPacks(overrideMp + ".xml");
                    }
                    else
                    {
                        ctx.Trc("Test Override xml is not provided to import");
                    }
                }

                // optionally delete all existing systems monitored by Operations Manager
                if (ctx.Records.HasKey("deploytestapp") &&
                    ctx.Records.GetValue("deploytestapp") == "true")
                {
                    ctx.Trc("Deploying testapp");
                    this.DeployTestApp(ctx);

                    ctx.Trc("Deploying scxtestapp.pl");
                    this.DeployPerlTestApp(ctx);
                }

                // determine whether to search for older agents if agent is not present in the newest folder on the drop server
                bool.TryParse(ctx.Records.GetValue("latestOnly"), out latestOnly);

                // determine whether to search for older agents if agent is not present in the newest folder on the drop server
                bool.TryParse(ctx.Records.GetValue("uselocalagents"), out useLocalAgents);

                // Must specify use local agent or latest agent to test.
                if (!latestOnly && !useLocalAgents)
                {
                    throw new GroupAbort("Error! Both latestOnly and useLocalAgents value is 'false'. Please specify use local agents or latest agents to test!");
                }

                 if (ctx.Records.HasKey("installonly") &&
                    ctx.Records.GetValue("installonly") == "true")
                 {
                     this.installOnly = true;
                 }

                bool clientDiscovered = this.discoveryHelper.VerifySystemInOM();

                if (!clientDiscovered)
                {
                    if (useLocalAgents)
                    {
                        IList<Microsoft.EnterpriseManagement.Runtime.TaskResult> agentList = this.discoveryHelper.GetAvailableAgentList();
                        if (agentList == null)
                        {                            
                            Abort(ctx, "No available agent list found!");
                        }

                        InstallableAgents agents = new InstallableAgents(agentList[0].Output);

                        ctx.Trc("Searching for agent in " + agents.DiscoveryScriptFolder);
                        DirectoryInfo di = new DirectoryInfo(agents.DiscoveryScriptFolder);
                        FileInfo[] fi = di.GetFiles("*" + this.clientInfo.PlatformTag + "*");

                        if (fi.Length == 0)
                        {
                            Abort(ctx, "Found no agent installer matching platformtag: " + this.clientInfo.PlatformTag);
                        }

                        if (fi.Length > 1)
                        {
                            Abort(ctx, "Found more than one agent installer matching platformtag: " + this.clientInfo.PlatformTag);
                        }

                        this.fullNewAgentPath = fi[0].FullName;
                    }
                    else
                    {
                        if (!Directory.Exists(this.agentCachePath))
                        {
                            ctx.Trc("Creating agent cache directory: " + this.agentCachePath);
                            Directory.CreateDirectory(this.agentCachePath);
                        }

                        ctx.Trc("Searching for new agent");
                        this.agentHelper.FindAgent(false, string.Empty, 5, latestOnly);
                        this.fullNewAgentPath = Path.Combine(this.agentCachePath, Path.GetFileName(this.agentHelper.FullAgentPath));

                        if (File.Exists(this.fullNewAgentPath))
                        {
                            ctx.Trc("Agent already in cache: " + this.fullNewAgentPath);
                        }
                        else
                        {
                            ctx.Trc("Copying agent to: " + this.fullNewAgentPath);
                            File.Copy(this.agentHelper.FullAgentPath, this.fullNewAgentPath);
                        }
                    }

                    ctx.Trc("Cleaning up remote system using SSH");
                    this.CleanupAgent(ctx);

                    ctx.Trc("Deploying/installing agent on client machine");
                    this.discoveryHelper.InstallClient(this.fullNewAgentPath);

                    ctx.Trc("Signing agent");
                    this.discoveryHelper.CreateSignedCert();

                    ctx.Trc("Adding client to OM");
                    this.discoveryHelper.DiscoverClientWSMan();
                }

                this.WaitForClientVerification(ctx);

                bool apacheAgentInstalled = this.apacheAgentHelper.VerifyApacheAgentInstalled();

                if (!apacheAgentInstalled)
                {
                    bool useTaskInstallApacheAgent = false ;
                     if (ctx.Records.HasKey("useTaskInstallApacheAgent") &&
                        ctx.Records.GetValue("useTaskInstallApacheAgent") == "true")
                     {
                         useTaskInstallApacheAgent = true;
                     }

                     if (useTaskInstallApacheAgent)
                     {
                         this.apacheAgentHelper.InstallApacheAgentWihTask();
                     }
                     else
                     { 
                         this.fullApacheAgentPath = ctx.Records.GetValue("apacheAgentPath");
                         string tag = ctx.Records.GetValue("apacheTag");
                         this.apacheAgentHelper.InstallApacheAgentWihCommand(fullApacheAgentPath, tag);
                     }
                }

                this.computerObject = this.monitorHelper.GetComputerObject(this.clientInfo.HostName);

                //this.VerifyHostIsCompletelyDiscoveried(ctx);

            }
            catch (Exception ex)
            {
                ctx.Trc("Exception in GroupHelper Setup: " + ex.StackTrace);
                Abort(ctx, ex.Message);
            }
        }

         /// <summary>
        /// Test framework cleanup method
        /// </summary>
        /// <param name="ctx">Current context</param>
        void ICleanup.Cleanup(IContext ctx)
        {
            try
            {
                ctx.Trc("ApacheSDKTests.GroupHelper.Cleanup");

                // Check for Warnings in SCX logs
                //this.agentHelper.ScxLogHelper("sdk", "Warning", true, ctx.Records.GetValue("platformtag"), ctx.Records.GetValue("zonename"));

                // Check for Errors in SCX logs
                //this.agentHelper.ScxLogHelper("sdk", "Error", false, ctx.Records.GetValue("platformtag"), ctx.Records.GetValue("zonename"));

                if (this.installOnly != true)
                {
                    //Uninstall Apache CIm Module
                    string fullApacheAgentPath = tempApacheCIMModuleLocation;
                    if (ctx.Records.HasKey("useTaskInstallApacheAgent") &&
                        ctx.Records.GetValue("useTaskInstallApacheAgent").ToLower() == "false")
                    {
                        fullApacheAgentPath = ctx.Records.GetValue("apacheAgentPath");
                    }
                    string tag = ctx.Records.GetValue("apacheTag");
                    this.apacheAgentHelper.UninstallApacheAgentWihCommand(fullApacheAgentPath, tag);

                    this.UninstallAgent(ctx);
                    ctx.Trc("Deleting System from OM...");
                    this.discoveryHelper.DeleteSystemFromOM();
                }
             
                TimeSpan testDuration = DateTime.Now - this.testGroupStart;

                ctx.Alw("Test group complete after " + testDuration);
            }
            catch (Exception e)
            {
                throw new VarAbort("Exception occured when cleanup agent. -- " + e.Message);
            }
        }

        #endregion Test Framework Methods

        #region Private Methods

        /// <summary>
        /// Uninstall from the remote client using Discovery Uninstall Task
        /// </summary>
        /// <param name="ctx">Current MCF context</param>
        private void UninstallAgent(IContext ctx)
        {
            // ignore exceptions from cleanup
            try
            {
                ctx.Trc("Uninstalling agent from client machine");
                this.discoveryHelper.UninstallClientTask();
            }
            catch (Exception e)
            {
                ctx.Trc("Client Uninstall failed: " + e.Message);
            }
        }

        /// <summary>
        /// Clean up agent from the remote client
        /// </summary>
        /// <param name="ctx">Current MCF Context</param>
        private void CleanupAgent(IContext ctx)
        {
            // ignore exceptions from cleanup
            try
            {
                ctx.Trc("Cleaning agent from client machine");
                this.discoveryHelper.CleanupRemoteClient();
            }
            catch (Exception e)
            {
                ctx.Trc("Client cleanup failed: " + e.Message);
            }
        }

        /// <summary>
        /// Deploy testapp application to /tmp on the client machine
        /// </summary>
        /// <param name="ctx">Current context</param>
        private void DeployTestApp(IContext ctx)
        {
            // Copy testapp
            const string remoteTestAppName = "testapp";
            string platformTag = ctx.Records.GetValue("platformTag");
            string localTestAppName = string.Format("{0}.{1}", platformTag, remoteTestAppName);

            // Copy the file to /tmp directory
            PosixCopy psxCopy = new PosixCopy(this.clientInfo.HostName, this.clientInfo.SuperUser, this.clientInfo.SuperUserPassword);
            psxCopy.CopyTo(Path.Combine("Testapp", localTestAppName), "/tmp/" + remoteTestAppName);

            // Give execute permissions to the testapp
            RunPosixCmd psxCmd = new RunPosixCmd(this.clientInfo.HostName, this.clientInfo.SuperUser, this.clientInfo.SuperUserPassword)
                                     {
                                         FileName = "chmod",
                                         Arguments = " 755 /tmp/" + remoteTestAppName,
                                         TimeOut = posixCommandTimeoutMS
                                     };
            try
            {
                psxCmd.RunCmd();
            }
            catch (Exception ex)
            {
                ctx.Trc("chmod timed out: " + ex);
            }
        }

        /// <summary>
        /// Deploy perl testapp application to /tmp on the client machine
        /// </summary>
        /// <param name="ctx">Current context</param>
        private void DeployPerlTestApp(IContext ctx)
        {
            // Copy testapp
            const string testAppName = "scxtestapp.pl";

            // Copy the file to /tmp directory
            PosixCopy psxCopy = new PosixCopy(this.clientInfo.HostName, this.clientInfo.SuperUser, this.clientInfo.SuperUserPassword);
            psxCopy.CopyTo(testAppName, "/tmp/" + testAppName);

            // Give execute permissions to the testapp
            RunPosixCmd psxCmd = new RunPosixCmd(this.clientInfo.HostName, this.clientInfo.SuperUser, this.clientInfo.SuperUserPassword)
                                     {
                                         FileName = "chmod",
                                         Arguments = " 755 /tmp/" + testAppName,
                                         TimeOut = posixCommandTimeoutMS
                                     };
            try
            {
                psxCmd.RunCmd();
            }
            catch (Exception ex)
            {
                ctx.Trc("chmod timed out: " + ex);
            }
        }

        /// <summary>
        /// Wait until it is possible to verify the client is added to OM and correctly monitoring
        /// at least the heartbeat and certificate monitors on the agent.
        /// </summary>
        /// <param name="ctx">Current context</param>
        private void WaitForClientVerification(IContext ctx)
        {
            bool clientVerified = this.discoveryHelper.VerifySystemInOM();

            int numTries = 0;

            while (numTries < maxServerWaitCount && !clientVerified)
            {
                this.Wait(ctx);

                numTries++;

                clientVerified = this.discoveryHelper.VerifySystemInOM();

                ctx.Trc(string.Format("VerifySystem({0})={1} after {2}/{3} tries", this.clientInfo.HostName, clientVerified, numTries, maxServerWaitCount));
            }

            if (clientVerified)
            {
                ctx.Alw("client verified: " + this.clientInfo.HostName);
            }
            else
            {
                Abort(ctx, "client verification failed: " + this.clientInfo.HostName);
            }
        }

        /// <summary>
        /// Generic wait method for use to allow OM internal state to stabilize.
        /// </summary>
        /// <param name="ctx">Current context</param>
        private void Wait(IContext ctx)
        {
            ctx.Trc(string.Format("Waiting for {0}...", this.serverWaitTime));
            System.Threading.Thread.Sleep(this.serverWaitTime);
        }

        /// <summary>
        /// Abort the text case by printing out a log message and throwing an exception
        /// </summary>
        /// <param name="ctx">Current context</param>
        /// <param name="msg">Error message</param>
        private static void Abort(IContext ctx, string msg)
        {
            ctx.Trc("GroupHelper ABORT: " + msg);
            throw new GroupAbort(msg);
        }

        /// <summary>
        /// Delete the exist agent maintenance account and create a new one 
        /// </summary>
        /// <param name="ctx">IContext ctx</param>
        private void ConfigureAgentMaintenceAccount(IContext ctx)
        {
            ManageAccountHelper manageAccountHelper = new ManageAccountHelper(this.info, ctx.Trc);

            string credentialSettings = ctx.Records.GetValue("agentcredentialSettings");

            string[] settings = credentialSettings.Split(new[] { ',' });
            if (settings.Length < 4)
            {
                throw new Exception("Agent credential setting is not correct");
            }

            ScxCredentialSettings.CredentialType credentialType = (ScxCredentialSettings.CredentialType)int.Parse(settings[0]);
            ScxCredentialSettings.AccountType accountType = (ScxCredentialSettings.AccountType)int.Parse(settings[1]);
            ScxCredentialSettings.SSHCredentialType sshCredentialType = (ScxCredentialSettings.SSHCredentialType)int.Parse(settings[2]);
            ScxCredentialSettings.ElevationType elevationType = (ScxCredentialSettings.ElevationType)int.Parse(settings[3]);

            string username;
            string password;
            if (accountType == ScxCredentialSettings.AccountType.Priviledged)
            {
                username = ctx.Records.GetValue("superuser");
                password = ctx.Records.GetValue("superuserpwd");
            }
            else
            {
                username = ctx.Records.GetValue("nonsuperuser");
                password = ctx.Records.GetValue("nonsuperuserpwd");
            }

            // Get Agent maintenance account.
            Credential credential = manageAccountHelper.GetAgentAccountCredential(credentialType, accountType, sshCredentialType, elevationType, username, password);

            // If Agent maintenance account not exists, create it.
            if (credential == null)
            {
                // If the account with the same display name exists but differe configuration, delete it
                if (manageAccountHelper.IsUserAccountExists(ScxCredentialSettings.UnixAccountType.ScxAgentMaintenance))
                {
                    manageAccountHelper.DeleteAccount(ScxCredentialSettings.UnixAccountType.ScxAgentMaintenance, ScxCredentialSettings.ProfileType.AgentMaintenanceAccount);
                }

                credential = manageAccountHelper.CreateAgentMaintenanceAccount(credentialType, accountType, sshCredentialType, elevationType, username, password);
            }

            this.discoveryHelper.AgentMaintenanceCredential = credential;
        }

        /// <summary>
        /// Verify that host is complete discovery
        /// </summary>
        /// <param name="ctx">IContext ctx</param>        
        private void VerifyHostIsCompletelyDiscoveried(IContext ctx)
        {
            ctx.Trc("Verifying that the host is completely discovered...");
            try
            {
                //// Do the verification to make sure that the host has been fully discovered
                bool isHostDiscovered = false;
                int numTries = 0;

                while (numTries < maxServerWaitCount && !isHostDiscovered)
                {
                    if (numTries > 0)
                    {
                        this.Wait(ctx);
                    }

                    numTries++;
                    string isManaged;
                    string healthState;

                    this.monitorHelper = new MonitorHelper(this.info);
                    try
                    {
                        this.OperatingSystemObject = this.monitorHelper.GetMonitoringObject("Microsoft.Unix.OperatingSystem", this.clientInfo.HostName);
                        isManaged = OperatingSystemObject.IsManaged.ToString();
                        ctx.Trc(OperatingSystemObject.Path + " is managed = " + isManaged);

                        try
                        {
                            this.computerObject = monitorHelper.GetComputerObject(this.clientInfo.HostName);
                            if (computerObject == null) continue;
                            healthState = computerObject.HealthState.ToString();
                            ctx.Trc(computerObject.Name + " health state = " + healthState);
                        }
                        catch (Exception ex)
                        {
                            ctx.Trc(ex.Message);
                            ctx.Trc("The host " + this.clientInfo.HostName +
                                " is NOT completely discovered after trying " +
                                numTries + " / " + maxServerWaitCount + " times...");
                            continue;
                        }
                    }
                    catch (Exception e)
                    {
                        ctx.Trc(e.Message);
                        ctx.Trc("The host " + this.clientInfo.HostName +
                                " is NOT completely discovered after trying " +
                                numTries + " / " + maxServerWaitCount + " times...");
                        continue;
                    }
                    
                    if (isManaged.Equals("True") && healthState.Equals("Success"))
                    {
                        ctx.Trc("The host " + this.clientInfo.HostName + " is completely discovered after trying " +
                                numTries + " / " + maxServerWaitCount + " times...");
                        isHostDiscovered = true;
                    }
                }
                if (!isHostDiscovered)
                {
                    ctx.Trc("Warning: Failed to verify the host " + this.clientInfo.HostName + " is completely discovered! Continuing but there may be issues...");
                }
            }
            catch (Exception ex)
            {
                Abort(ctx, ex.ToString());
            }
        }
        #endregion Private Methods

        #endregion Methods
    }
}
