//-----------------------------------------------------------------------
// <copyright file="DeployModuleTask.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>v-jeyin</author>
// <description></description>
// <history>12/10/2014 2:30:10 PM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Apache.SDK.ApacheSDKTests
{
    using System;
    using System.Collections.Generic;
    using Infra.Frmwrk;    
    using Microsoft.EnterpriseManagement.Monitoring;
    using Scx.Test.Common;
    using Scx.Test.Apache.SDK.ApacheSDKHelper;

    /// <summary>
    /// Description for DeployModuleTask with powershell.
    /// Test Case 724163: [Apache]<SDK><Task>Verify if install Apache CIM Module works by running task "Install/Upgrade Apache CIM Module" with valid root account.
    /// </summary>
    public class DeployModuleTaskWithPS : ISetup, IRun, IVerify, ICleanup
    {
        #region Private Fields

        /// <summary>
        /// Imformation about the OM server.
        /// </summary>
        private OMInfo info;

        /// <summary>
        /// Information about the client machine
        /// </summary>
        private ClientInfo clientInfo;
        
        /// <summary>
        /// Apache agent helper class
        /// </summary>
        private ApacheAgentHelper apacheAgentHelper;
        
        /// <summary>
        /// The Location of Apache Cim module
        /// </summary>
        private string tempApacheCIMModuleLocation= @"C:\Windows\Temp\ApacheCimProv";

        private PsHelper psHelper;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DeployModuleTask class.
        /// </summary>
        public DeployModuleTaskWithPS()
        {
        }

        #endregion Constructors

        #region Methods

        #region Test Framework Methods

        /// <summary>
        /// Framework setup method
        /// </summary>
        /// <param name="ctx">Current context</param>
        void ISetup.Setup(IContext ctx)
        {
            ctx.Trc("ApacheSDKTests.DeployModuleTaskWithPS.Setup");

            try
            {
                this.info = new OMInfo(
                    ctx.ParentContext.Records.GetValue("omserver"),
                    ctx.ParentContext.Records.GetValue("omusername"),
                    ctx.ParentContext.Records.GetValue("omdomain"),
                    ctx.ParentContext.Records.GetValue("ompassword"),
                    ctx.ParentContext.Records.GetValue("defaultresourcepool"));

                this.clientInfo = new ClientInfo(
                    ctx.ParentContext.Records.GetValue("hostname"),
                    ctx.ParentContext.Records.GetValue("ipaddress"),
                    ctx.ParentContext.Records.GetValue("targetcomputerclass"),
                    ctx.ParentContext.Records.GetValue("managementpack"),
                    ctx.ParentContext.Records.GetValue("architecture"),
                    ctx.ParentContext.Records.GetValue("nonsuperuser"),
                    ctx.ParentContext.Records.GetValue("nonsuperuserpwd"),
                    ctx.ParentContext.Records.GetValue("superuser"),
                    ctx.ParentContext.Records.GetValue("superuserpwd"),
                    ctx.ParentContext.Records.GetValue("packagename"),
                    ctx.ParentContext.Records.GetValue("cleanupcommand"),
                    ctx.ParentContext.Records.GetValue("platformtag"));

                ctx.Trc("OMInfo: " + Environment.NewLine + this.info.ToString());

                ctx.Trc("ClientInfo: " + Environment.NewLine + this.clientInfo.ToString());

                psHelper = new PsHelper();
                psHelper.Setup();

                /*this.tasksHelper = new TasksHelper(ctx.Trc, this.info);

                this.monitorHelper = new MonitorHelper(this.info);

                string monitorInstanceClass = ctx.Records.GetValue("InstanceClass");

                this.apacheInstance = this.monitorHelper.GetMonitoringObject(monitorInstanceClass,this.clientInfo.HostName);*/

                //Uninstall Apache CIm Module
                this.apacheAgentHelper = new ApacheAgentHelper(this.info, this.clientInfo) { Logger = ctx.Trc };
                string fullApacheAgentPath = tempApacheCIMModuleLocation;
                if (ctx.ParentContext.Records.HasKey("useTaskInstallApacheAgent") &&
                    ctx.ParentContext.Records.GetValue("useTaskInstallApacheAgent").ToLower() == "false")
                {
                    fullApacheAgentPath = ctx.ParentContext.Records.GetValue("apacheAgentPath");
                }
                string tag = ctx.ParentContext.Records.GetValue("apacheTag");
                this.apacheAgentHelper.UninstallApacheAgentWihCommand(fullApacheAgentPath, tag);
             }
            catch (Exception ex)
            {
                this.Abort(ctx, ex.ToString());
            }

            ctx.Trc("ApacheSDKTests.DeployModuleTaskWithPS.Setup complete");
        }

        /// <summary>
        /// Framework Run method
        /// </summary>
        /// <param name="ctx">Current context</param>
        void IRun.Run(IContext ctx)
        {
            ctx.Trc("ApacheSDKTests.DeployModuleTaskWithPS.Run");

            string scripts = ctx.Records.GetValue("scripts");
            string[] scriptArray = scripts.Split(';');
            
            try
            {
                psHelper.Run(scriptArray);
            }
            catch (Exception ex)
            {
                this.Abort(ctx, ex.ToString());
            }

            ctx.Trc("ApacheSDKTests.DeployModuleTaskWithPS.Run complete");
        }

        /// <summary>
        /// Framework verify method
        /// </summary>
        /// <param name="ctx">Current context</param>
        void IVerify.Verify(IContext ctx)
        {
            ctx.Trc("ApacheSDKTests.DeployModuleTaskWithPS.Verify");
            try
            {
                string key = ctx.Records.GetValue("expectedOutputKey");
                string value = ctx.Records.GetValue("expectedOutputKeyWord");

                psHelper.CheckOutput(key , value);

                bool apacheAgentInstalled = this.apacheAgentHelper.VerifyApacheAgentInstalled();
                if (!apacheAgentInstalled)
                {
                    this.Fail(ctx, "Fail: didn't find CIM prov installed on client");
                }
            }
            catch (Exception ex)
            {
                this.Fail(ctx, ex.Message);
            }

            ctx.Trc("ApacheSDKTests.DeployModuleTaskWithPS.Verify.Complete!");
        }

        /// <summary>
        /// Framework cleanup method
        /// </summary>
        /// <param name="ctx">Current context</param>
        void ICleanup.Cleanup(IContext ctx)
        {
            ctx.Trc("ApacheSDKTests.DeployModuleTaskWithPS.Cleanup");

            bool useTaskInstallApacheAgent = false;
            if (ctx.Records.HasKey("useTaskInstallApacheAgent") &&
               ctx.Records.GetValue("useTaskInstallApacheAgent") == "true")
            {
                useTaskInstallApacheAgent = true;
            }
           
            bool apacheAgentInstalled = this.apacheAgentHelper.VerifyApacheAgentInstalled();

            if (useTaskInstallApacheAgent)
            {
                if(!apacheAgentInstalled)
                {
                    this.apacheAgentHelper.InstallApacheAgentWihTask();
                }
            }
            else
            {
                string fullApacheAgentPath = tempApacheCIMModuleLocation;
                string tag = ctx.ParentContext.Records.GetValue("apacheTag");
                if (apacheAgentInstalled)
                {
                    this.apacheAgentHelper.UninstallApacheAgentWihCommand(fullApacheAgentPath, tag);              
                }

                fullApacheAgentPath = ctx.ParentContext.Records.GetValue("apacheAgentPath");
                this.apacheAgentHelper.InstallApacheAgentWihCommand(fullApacheAgentPath, tag);
            }

            ctx.Trc("ApacheSDKTests.DeployModuleTaskWithPS.Cleanup finished");
        }

        #endregion Test Framework Methods

        #region Private Methods

        /// <summary>
        /// Get special task status object of Microsoft.EnterpriseManagement.Runtime.TaskStatus for the expect 
        /// </summary>
        /// <param name="expectedStatus">expected status</param>
        /// <returns>return Microsoft.EnterpriseManagement.Runtime.TaskStatus object</returns>
        private Microsoft.EnterpriseManagement.Runtime.TaskStatus GetExpectedTaskStatus(string expectedStatus)
        {
            switch (expectedStatus.ToLower())
            {
                case "success":
                    {
                        return Microsoft.EnterpriseManagement.Runtime.TaskStatus.Succeeded;
                    }

                case "fail":
                    {
                        return Microsoft.EnterpriseManagement.Runtime.TaskStatus.Failed;
                    }

                default:
                    {
                        return Microsoft.EnterpriseManagement.Runtime.TaskStatus.Scheduled;
                    }
            }
        }

        /// <summary>
        /// Fail the text case by printing out a log message and throwing an exception
        /// </summary>
        /// <param name="ctx">Current context</param>
        /// <param name="msg">Error message</param>
        private void Fail(IContext ctx, string msg)
        {
            ctx.Trc("SecurityHealth FAIL: " + msg);
            throw new VarFail(msg);
        }

        /// <summary>
        /// Abort the text case by printing out a log message and throwing an exception
        /// </summary>
        /// <param name="ctx">Current context</param>
        /// <param name="msg">Error message</param>
        private void Abort(IContext ctx, string msg)
        {
            ctx.Trc("SecurityHealth ABORT: " + msg);
            throw new VarAbort(msg);
        }

        #endregion Private Methods

        #endregion Methods
    }
}
