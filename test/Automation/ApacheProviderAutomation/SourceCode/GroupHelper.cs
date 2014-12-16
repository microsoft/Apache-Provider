//-----------------------------------------------------------------------
// <copyright file="GroupHelper.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>v-jeyin@microsoft.com</author>
// <description></description>
//-----------------------------------------------------------------------

namespace Scx.Test.Apache.Provider
{
    using System;
    using System.IO;
    using Infra.Frmwrk;
    using Scx.Test.Common;

    public class GroupHelper : ISetup, ICleanup
    /*ICleanup*/
    {
        //private SshHelper sshHelper;

        #region Private Fields

        /// <summary>
        /// Required: Name of Posix host (DNS or IP)
        /// </summary>
        private string hostName;

        /// <summary>
        /// Required: User name
        /// </summary>
        private string userName;

        /// <summary>
        /// Required: User's password
        /// </summary>
        private string password;

        /// <summary>
        /// Required: Command to install agent
        /// </summary>
        private string installOmCmd;
        private string installApacheCmd;

        /// <summary>
        /// Required: Command to remove agent
        /// </summary>
        private string uninstallOmCmd;
        private string uninstallApacheCmd;

        /// <summary>
        /// Required: Command to clean the Client Machine:Uninstall/delete scx direcotries
        /// </summary>
        private string cleanupOmCmd;
        private string cleanupApacheCmd;

        /// <summary>
        /// Required: extension used by agent package
        /// </summary>
        private string agentPkgExt;

        /// <summary>
        /// Required: string tag used in agent directory name
        /// </summary>
        private string directoryTag;

        /// <summary>
        /// Required: string tag used in identifying the platform
        /// </summary>
        private string platformTag;
        private string ApacheTag;

        /// <summary>
        /// Optional: allows configuring download of earlier agents in case of build failure
        /// </summary>
        private bool latestOnly = true;

        /// <summary>
        /// Optional: Allows using the Agents from local path
        /// </summary>
        private bool useAgentLocation = false;

        ///<summary>
        ///Optional: check Apache status command
        ///</summary>

        private string installApachePkg;
        private string startApacheCmd;
        private string checkServiceCmd;
        /// <summary>
        /// Class AgentHelper : contain some operations of agent.
        /// </summary>
        private AgentHelper agentHelper;
        private ApacheHelper apacheHelper;

        #endregion

        #region Public method
        public void Setup(IContext ctx)
        {
            try
            {
                if (ctx.Records.HasKey("skipagentdeploy") &&
                    ctx.Records.GetValue("skipagentdeploy") == "true")
                {
                    ctx.Trc("Skipping agent deployment/cleanup");
                    return;
                }

                foreach (string key in ctx.Records.GetKeys())
                {
                    ctx.Trc("Setup key=" + key + ", value=" + ctx.Records.GetValue(key));
                }

                this.hostName = ctx.Records.GetValue("hostname");
                if (string.IsNullOrEmpty(this.hostName))
                {
                    throw new GroupAbort("hostname not specified");
                }

                ctx.Trc("GroupAgentDeploy: hostname = " + this.hostName);

                this.userName = ctx.Records.GetValue("username");
                this.password = ctx.Records.GetValue("password");
                this.installOmCmd = ctx.Records.GetValue("InstallOMCmd");
                this.uninstallOmCmd = ctx.Records.GetValue("UninstallOMCmd");
                this.cleanupOmCmd = ctx.Records.GetValue("CleanupOMCmd");
                this.agentPkgExt = ctx.Records.GetValue("AgentPkgExt");
                this.directoryTag = ctx.Records.GetValue("DirectoryTag");
                this.platformTag = ctx.Records.GetValue("PlatformTag");
                this.ApacheTag = ctx.Records.GetValue("ApacheTag");
                this.installApacheCmd = ctx.Records.GetValue("installApacheCmd");
                this.uninstallApacheCmd = ctx.Records.GetValue("UninstallApacheCmd");
                this.cleanupApacheCmd = ctx.Records.GetValue("CleanupApacheCmd");
                this.installApachePkg = ctx.Records.GetValue("installApachePkg");
                this.startApacheCmd = ctx.Records.GetValue("startApacheCmd");
                this.checkServiceCmd = ctx.Records.GetValue("checkServiceCmd");
                bool.TryParse(ctx.Records.GetValue("latestOnly"), out this.latestOnly);

                // determine whether to search for older agents if agent is not present in the newest folder on the drop server
                bool.TryParse(ctx.Records.GetValue("useagentlocation"), out this.useAgentLocation);

                // Must specify use local agent or latest agent to test.
                if (!this.latestOnly && !this.useAgentLocation)
                {
                    throw new GroupAbort("Error! Both latestOnly and useAgentLocal value is 'false'. Please specify use local agents or latest agents to test!");
                }

                if (string.IsNullOrEmpty(this.userName) ||
                    string.IsNullOrEmpty(this.password) || string.IsNullOrEmpty(this.installOmCmd) ||
                    string.IsNullOrEmpty(this.agentPkgExt) || string.IsNullOrEmpty(this.directoryTag) ||
                    string.IsNullOrEmpty(this.platformTag) || string.IsNullOrEmpty(this.uninstallOmCmd) || string.IsNullOrEmpty(this.cleanupOmCmd))
                {
                    throw new GroupAbort("Error, check UserName, Password, installOmCmd, uninstallOmCmd,cleanupOmCmd, AgentPkgExt, DirectoryTag, platformTag");
                }

                this.agentHelper = new AgentHelper(ctx.Trc, this.hostName, this.userName, this.password, this.installOmCmd, this.cleanupOmCmd);

                this.agentHelper.VerifySSH();

                this.agentHelper.SynchDateTime();

                this.apacheHelper = new ApacheHelper(ctx.Trc, this.hostName, this.userName, this.password);

                this.apacheHelper.CheckApacheServiceStatus(this.checkServiceCmd);

                this.SetApacheAgentPath(ctx);

                this.apacheHelper.UninstallApacheAgent(this.uninstallApacheCmd);

                this.CleanupAgent(ctx);

                this.InstallAgent(ctx);

                this.apacheHelper.InstallApacheAgent(this.installApacheCmd);
            }
            catch (Exception ex)
            {
                throw new GroupAbort(ex.Message);
            }
        }

        /// <summary>
        /// Remove agent from Posix host
        /// </summary>
        /// <param name="ctx">MCF group context</param>


        public void Cleanup(IContext ctx)
        {
            try
            {
                // Check for Warnings in SCX logs
                this.agentHelper.ScxLogHelper("wsman", "Warning", true, this.platformTag, null);

                // Check for Errors in SCX logs
                this.agentHelper.ScxLogHelper("wsman", "Error", false, this.platformTag, null);

                bool installOnly = false;   // Optional: If exists in records and has a value, then agent won't be removed

                if (ctx.Records.HasKey("skipagentdeploy") &&
                    ctx.Records.GetValue("skipagentdeploy") == "true")
                {
                    ctx.Trc("Skipping agent deployment/cleanup");
                    return;
                }

                if (ctx.Records.HasKey("installonly") &&
                    ctx.Records.GetValue("installonly") == "true")
                {
                    installOnly = true;
                }

                if (installOnly == false)
                {
                    this.apacheHelper.UninstallApacheAgent(this.uninstallApacheCmd);
                    this.apacheHelper.Cleanup();
                    this.UninstallAgent(ctx);
                }
            }
            catch (Exception ex)
            {
                throw new GroupAbort(ex.Message);
            }

        }

        #endregion
        #region private method
        /// <summary>
        /// install the agent to the host
        /// </summary>
        /// <param name="ctx">Current MCF Context</param>
        private void InstallAgent(IContext ctx)
        {
            this.InstallAgentHelper(ctx, this.hostName);
        }

        /// <summary>
        /// Clean up and install an agent to a particular hostname
        /// </summary>
        /// <param name="ctx">Current MCF Context</param>
        /// <param name="targetHostName">Host name to install to</param>
        private void InstallAgentHelper(IContext ctx, string targetHostName)
        {
            this.agentHelper.AgentPkgExt = this.agentPkgExt;
            this.agentHelper.DirectoryTag = this.directoryTag;

            if (ctx.Records.HasKey("AgentCachePath") == true)
            {
                this.agentHelper.LocalCachePath = ctx.Records.GetValue("AgentCachePath");
            }

            // Optional decompression
            if (ctx.Records.HasKey("Decompression"))
            {
                this.agentHelper.DecompressUtil = ctx.Records.GetValue("Decompression");
                this.agentHelper.CompressionExt = ctx.Records.GetValue("CompressionExt");
                if (string.IsNullOrEmpty(this.agentHelper.DecompressUtil) ||
                    string.IsNullOrEmpty(this.agentHelper.CompressionExt))
                {
                    throw new GroupAbort("Check values for Decompression and CompressionExt records");
                }
            }

            if (this.useAgentLocation)
            {
                string localAgents = ctx.Records.GetValue("OMAgentLocation");

                ctx.Trc("Searching for agent in " + localAgents);
                DirectoryInfo di = new DirectoryInfo(localAgents);
                FileInfo[] fi = di.GetFiles("*" + this.platformTag + "*");

                if (fi.Length == 0)
                {
                    throw new GroupAbort("Found no agent installer matching platformtag: " + this.platformTag);
                }

                if (fi.Length > 1)
                {
                    throw new GroupAbort("Found more than one agent installer matching platformtag: " + this.platformTag);
                }

                this.agentHelper.FullAgentPath = fi[0].FullName;
            }
            else
            {
                ctx.Trc("Searching for agent");
                this.agentHelper.DropLocation = ctx.Records.GetValue("remoteAgents");
                if (string.IsNullOrEmpty(this.agentHelper.DropLocation))
                {
                    throw new GroupAbort("AgentHelper.Droplocation is not set");
                }

                this.agentHelper.FindAgent(false, string.Empty, 10, this.latestOnly);
            }

            ctx.Trc("Installing agent");
            this.agentHelper.Install();
        }

        /// <summary>
        /// Uninstall the agent
        /// </summary>
        /// <param name="ctx">Current MCF Context</param>
        private void UninstallAgent(IContext ctx)
        {
            try
            {
                this.agentHelper.Uninstall();
            }
            catch (Exception ex)
            {
                ctx.Trc("Uninstall failed: " + ex.Message);
            }
        }

        private void SetApacheAgentPath(IContext ctx)
        {
            string apachelocation = ctx.Records.GetValue("ApachesLocation");

            this.apacheHelper.SetApacheAgentFullPath(apachelocation, this.ApacheTag, true);
        }


        private void CleanupAgent(IContext ctx)
        {
            try
            {
                // Calling the AgentHelper Uninstall method with Cleanup OM CMD
                this.agentHelper.Uninstall();
            }
            catch (Exception ex)
            {
                ctx.Trc("Cleanup failed: " + ex.Message);
            }
        }

        private string GetSshcomPath()
        {
            string sshcomPath = "";

            string mcfLocation = Environment.CommandLine
                .Split(' ')[0]
                .Replace("\"", ""); // C:\\DSC_TFS\\DSC\\Test\\DSC\\DSC\\bin\\Debug\\MCF\\MCF3.3-CLR4\\MCF.exe

            string debugPath = Path.GetDirectoryName(
                Path.GetDirectoryName(
                Path.GetDirectoryName(mcfLocation))); // C:\\DSC_TFS\\DSC\\Test\\DSC\\DSC\\bin\\Debug

            sshcomPath = Path.Combine(debugPath, "x64", "sshcom.dll");

            // C:\\DSC_TFS\\DSC\\Test\\DSC\\DSC\\bin\\Debug\\x64\\sshcom.dll
            return sshcomPath;
        }
        #endregion
    }
}
