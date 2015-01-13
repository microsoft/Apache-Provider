//-----------------------------------------------------------------------
// <copyright file="PerformanceWithTwoStateHealth.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>v-litin</author>
// <description>PerformanceWithTwoStateHealth</description>
//-----------------------------------------------------------------------

namespace Scx.Test.Apache.SDK.ApacheSDKTests
{
    using System;
    using System.Threading;
    using System.IO;
    using Infra.Frmwrk;
    using Microsoft.EnterpriseManagement.Configuration;
    using Microsoft.EnterpriseManagement.Monitoring;
    using Scx.Test.Common;
    using Scx.Test.Apache.SDK.ApacheSDKHelper;
    class PerformanceWithTwoStateHealth : PerformanceHealthBase, ISetup, IRun, IVerify, ICleanup
    {
 /// <summary>
        /// Initializes a new instance of the PerformanceWithTwoStateHealth class
        /// </summary>
        public PerformanceWithTwoStateHealth()
        {
        }

        #region Test Framework Methods

        /// <summary>
        /// All Shell scripts location.
        /// </summary>
        private string scriptsLocation = System.Environment.CurrentDirectory;

        /// <summary>
        /// Creating ports script name.
        /// </summary>
        private string breakScriptName = "breakWebRequestTotalResponseConf.sh";

        /// <summary>
        /// Where to run action cmd.
        /// </summary>
        private string actionCmdType = "Client";

        /// <summary>
        /// Framework setup method
        /// </summary>
        /// <param name="ctx">Current context</param>
        void ISetup.Setup(IContext ctx)
        {
            if (this.SkipThisTest(ctx))
            {
                ctx.Trc("BYPASSING TEST CASE (ISetup.Setup): " + ctx.Records.GetValue("entityname"));
                return;
            }

            ctx.Trc("SDKTests.PerformanceWithTwoStateHealth.Setup");

            try
            {
                this.Info = new OMInfo(
                    ctx.ParentContext.Records.GetValue("omserver"),
                    ctx.ParentContext.Records.GetValue("omusername"),
                    ctx.ParentContext.Records.GetValue("omdomain"),
                    ctx.ParentContext.Records.GetValue("ompassword"),
                    ctx.ParentContext.Records.GetValue("defaultresourcepool"));

                this.ClientInfo = new ClientInfo(
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

                ctx.Trc("OMInfo: " + Environment.NewLine + this.Info.ToString());

                ctx.Trc("ClientInfo: " + Environment.NewLine + this.ClientInfo.ToString());

                if (ctx.Records.HasKey("ActionType"))
                {
                    this.actionCmdType = ctx.Records.GetValue("ActionType");
                    if (this.actionCmdType.Equals("Server"))
                    {
                        if(ctx.ParentContext.Records.HasKey("apacheHelperABPath"))
                        {
                            string abPath = ctx.ParentContext.Records.GetValue("apacheHelperABPath");
                            if(!Directory.Exists(abPath))
                                throw new VarAbort("Could not find the ab.exe under " + abPath);
                        }
                        else
                        {
                            throw new VarAbort("Please set apacheHelperABPath in VarMap");
                        }
                    }
                }

                this.MonitorHelper = new MonitorHelper(this.Info);

                this.AlertHelper = new AlertsHelper(this.Info);

                this.ApacheAgentHelper = new ApacheAgentHelper(this.Info, this.ClientInfo) { Logger = ctx.Trc };

                this.OverrideHelper = new OverrideHelper(ctx.Trc, this.Info, ctx.ParentContext.Records.GetValue("testingoverride"));

                string instanceID = ctx.Records.GetValue("InstanceID");

                if (instanceID == null)
                {
                    this.ComputerObject = this.MonitorHelper.GetComputerObject(this.ClientInfo.HostName);
                }
                else
                {
                    this.ComputerObject = this.GetVitualHostMonitor(this.ClientInfo.HostName, instanceID);
                }

                //this.RecoverMonitorIfFailed(ctx);

                this.OverridePerformanceMonitor(ctx, true);

                this.DeleteMonitorOverride(ctx);

                this.ApplyMonitorOverride(ctx, OverrideState.Default);

                this.CloseMatchingAlerts(ctx);

                this.VerifyMonitor(ctx, HealthState.Success);

                this.VerifyAlert(ctx, false);

                //copy all ssl certificate script to '/tmp/'
                PosixCopy copyToHost = new PosixCopy(this.ClientInfo.HostName, this.ClientInfo.SuperUser, this.ClientInfo.SuperUserPassword);
                copyToHost.CopyTo(scriptsLocation + "/" + breakScriptName, "/tmp/" + breakScriptName);
            
            }
            catch (Exception ex)
            {
                this.Abort(ctx, ex.ToString());
            }

            ctx.Trc("SDKTests.PerformanceWithTwoStateHealth.Setup complete");
        }

        /// <summary>
        /// Framework Run method
        /// </summary>
        /// <param name="ctx">Current context</param>
        void IRun.Run(IContext ctx)
        {
            string entity = ctx.Records.GetValue("entityname");
            string actionCmd = ctx.Records.GetValue("ActionCmd");
            string monitorName = ctx.Records.GetValue("monitorname");
            string diagnosticName = ctx.Records.GetValue("diagnostics");
            string recoveryCmd = ctx.Records.GetValue("recoverycmd");
            string targetOSClassName = ctx.ParentContext.Records.GetValue("targetosclass");

            if (this.SkipThisTest(ctx))
            {
                return;
            }

            ctx.Trc("SDKTests.PerformanceWithTwoStateHealth.Run with entity " + entity);

            try
            {
                // Apply the monitor Override and wait for it take effect
                this.ApplyMonitorOverride(ctx, OverrideState.Override);

                ctx.Alw("Running command synchronously (may take a while): " + actionCmd);
                if (this.actionCmdType.Equals("Server"))
                {
                    try
                    {
                        /*RunWinCmd winCmd = new RunWinCmd();
                        string abPath = ctx.ParentContext.Records.GetValue("apacheHelperABPath");
                        winCmd.WorkingDirectory = abPath;
                        string[] cmds = actionCmd.Split(' ');
                        winCmd.FileName = cmds[0];
                        string arguments = actionCmd.Substring(cmds[0].Length);
                        winCmd.Arguments = arguments;
                        winCmd.RunCmd();*/
                        this.RunWinCMDWithThread(ctx);
                }
                    catch (Exception)
                    {

                    }
                }
                else 
                {
                RunCmd(actionCmd);
                }

                string ExpectedState = ctx.Records.GetValue("ExpectedState");
                HealthState requiredState = this.GetRequiredState(ExpectedState);
 
                //// Waiting for 5 minutes directly to make sure that OM can get the latest monitor state
                //// because the latest monitor state might be the same as the initial state
                if (ctx.Records.GetValue("ExpectedState").Equals("success",  StringComparison.CurrentCultureIgnoreCase))
                {
                    this.Wait(ctx, 300);

                    this.VerifyMonitor(ctx, requiredState);

                    this.VerifyAlert(ctx, false);
                }
                else
                {
                    this.VerifyMonitor(ctx, requiredState);

                    this.VerifyAlert(ctx, true);
                }

                // Run the recovery command
                if (!(string.IsNullOrEmpty(recoveryCmd)))
                {
                ctx.Trc("Running recovery command: " + recoveryCmd);
                this.PosixCmd(ctx, recoveryCmd);

                this.VerifyAlert(ctx, false);
            }
            }
            catch (Exception ex)
            {
                this.Fail(ctx, ex.ToString());
            }

            ctx.Trc("SDKTests.PerformanceWithTwoStateHealth.Run complete");
        }

        /// <summary>
        /// Framework verify method
        /// </summary>
        /// <param name="ctx">current context</param>
        void IVerify.Verify(IContext ctx)
        {
            if (this.SkipThisTest(ctx))
            {
                return;
            }

            ctx.Trc("SDKTests.PerformanceWithTwoStateHealth.Verify");
        }

        /// <summary>
        /// Framework cleanup method
        /// </summary>
        /// <param name="ctx">Current context</param>
        void ICleanup.Cleanup(IContext ctx)
        {
            if (this.SkipThisTest(ctx))
            {
                return;
            }

            this.DeleteMonitorOverride(ctx);
            this.OverridePerformanceMonitor(ctx, true);

            this.VerifyMonitor(ctx, HealthState.Success);

        }

        #endregion Test Framework Methods

        #region Private Methods

        private void RunWinCMDWithThread(IContext ctx)
        {
            Thread t = new Thread(new ParameterizedThreadStart(RunWinCMD));
            t.Start(ctx);
        }

        private void RunWinCMD(object parObject)
        {
            IContext ctx = (IContext)parObject;
            RunWinCmd winCmd = new RunWinCmd();
            string abPath = ctx.ParentContext.Records.GetValue("apacheHelperABPath");
            string actionCmd = ctx.Records.GetValue("ActionCmd");
            winCmd.WorkingDirectory = abPath;
            string[] cmds = actionCmd.Split(' ');
            winCmd.FileName = cmds[0];
            string arguments = actionCmd.Substring(cmds[0].Length);
            winCmd.Arguments = arguments;
            winCmd.RunCmd();
        }

        /// <summary>
        /// Apply the override to the monitor
        /// </summary>
        /// <param name="ctx">current context</param>
        /// <param name="requiredState">the required override state</param>
        private void ApplyMonitorOverride(IContext ctx, OverrideState requiredState)
        {
            string monitorName = ctx.Records.GetValue("monitorname");
            string monitorContext = ctx.Records.GetValue("monitorcontext");
            string monitorTarget = ctx.Records.GetValue("monitortarget");
            string warningThresholdName = ctx.Records.GetValue("warningThresholdName");
            string errorThresholdName = ctx.Records.GetValue("errorThresholdName");
            string monitorWarningThreshold = null;
            string monitorErrorThreshold = null;

            switch (requiredState)
            {
                case OverrideState.Default:
                    {
                        monitorWarningThreshold = ctx.Records.GetValue("defaultWarningThreshold");
                        monitorErrorThreshold = ctx.Records.GetValue("defaultErrorThreshold");
                        break;
                    }

                case OverrideState.Override:
                    {
                        monitorWarningThreshold = ctx.Records.GetValue("overrideWarningThreshold");
                        monitorErrorThreshold = ctx.Records.GetValue("overrideErrorThreshold");
                        break;
                    }

                default:
                    {
                        break;
                    }
            }

            if (string.IsNullOrEmpty(monitorWarningThreshold)
                || string.IsNullOrEmpty(monitorErrorThreshold))
            {
                this.Abort(ctx, String.Format("The value of {0} , {1} , {2} and {3} should be set", warningThresholdName, errorThresholdName));
            }
            else
            {
                // Override warningThreshold
                this.OverrideHelper.SetClientMonitorParameter(this.ComputerObject, monitorName, monitorContext, monitorTarget, warningThresholdName, monitorWarningThreshold);

                // Override errorThreshold
                this.OverrideHelper.SetClientMonitorParameter(this.ComputerObject, monitorName, monitorContext, monitorTarget, errorThresholdName, monitorErrorThreshold);

                this.OverrideHelper.SetClientMonitorInterval(this.ComputerObject, monitorName, monitorContext, monitorTarget, 30);
            }
        }

        /// <summary>
        /// Apply a monitor override to put the monitor threshold back to in the default value, as defined in the varmap
        /// </summary>
        /// <param name="ctx">Current context</param>
        private void DeleteMonitorOverride(IContext ctx)
        {
            string monitorName = ctx.Records.GetValue("monitorname");
            string monitorContext = ctx.Records.GetValue("monitorcontext");
            string monitorTarget = ctx.Records.GetValue("monitortarget");
            string warningThresholdName = ctx.Records.GetValue("warningThresholdName");
            string errorThresholdName = ctx.Records.GetValue("errorThresholdName");

            this.OverrideHelper.DeleteClientMonitorParameter(this.ComputerObject, monitorName, monitorContext, monitorTarget, warningThresholdName);

            this.OverrideHelper.DeleteClientMonitorParameter(this.ComputerObject, monitorName, monitorContext, monitorTarget, errorThresholdName);

            this.OverrideHelper.DeleteClientMonitorInterval(this.ComputerObject, monitorName, monitorContext, monitorTarget);
        }

        /// <summary>
        /// Enable or disable special monitor object: Microsoft.AIX.5.3.LogicalDisk.PercentFreeSpace.Monitor
        /// </summary>
        /// <param name="ctx">current context</param>
        /// <param name="monitorEnabled">enable monitor or disable monitor</param>
        private void OverridePerformanceMonitor(IContext ctx, bool monitorEnabled)
        {
            string monitorName = ctx.Records.GetValue("monitorname");
            string monitorContext = ctx.Records.GetValue("monitorcontext");
            string monitorTarget = ctx.Records.GetValue("monitortarget");
            this.OverrideHelper.SetClientMonitorEnabled(this.ComputerObject, monitorName, monitorContext, monitorTarget, monitorEnabled);
        }


        private RunPosixCmd RunCmd(string cmd, string arguments = "")
        {
            // Begin cmd
            RunPosixCmd execCmd = new RunPosixCmd(this.ClientInfo.HostName, this.ClientInfo.SuperUser, this.ClientInfo.SuperUserPassword);

            // Execute command
            execCmd.FileName = cmd;
            execCmd.Arguments = arguments;
            execCmd.RunCmd();
            return execCmd;
        }
        #endregion
    }

}
