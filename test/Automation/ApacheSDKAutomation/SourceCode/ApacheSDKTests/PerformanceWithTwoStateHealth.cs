//v-litin

namespace Scx.Test.Apache.SDK.ApacheSDKTests
{
    using System;
    using System.Threading;
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

                this.MonitorHelper = new MonitorHelper(this.Info);

                this.AlertHelper = new AlertsHelper(this.Info);

                this.OverrideHelper = new OverrideHelper(ctx.Trc, this.Info, ctx.ParentContext.Records.GetValue("testingoverride"));

                this.ComputerObject = this.MonitorHelper.GetComputerObject(this.ClientInfo.HostName);

                this.RecoverMonitorIfFailed(ctx);

                // If Free Space monitor is enabled, the corresponding Logical Disk % Free Space monitor should be disabled with an override
                this.OverridePerformanceMonitor(ctx, true);

                // Delete the monitor override information from override MP 
                this.DeleteMonitorOverride(ctx);

                // Override the monitor threshold back to default values                
                this.ApplyMonitorOverride(ctx, OverrideState.Default);

                this.CloseMatchingAlerts(ctx);

                this.VerifyMonitor(ctx, HealthState.Success);

                this.VerifyAlert(ctx, false);
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
                if (ctx.Records.GetValue("expectedmonitorstate").Equals("success", StringComparison.CurrentCultureIgnoreCase))
                {
                    this.Wait(ctx, 120);
                }

                ctx.Alw("Running command synchronously (may take a while): " + actionCmd);
                //this.PosixCmd(ctx, actionCmd);
                RunCmd(actionCmd);

                string expectedMonitorState = ctx.Records.GetValue("expectedmonitorstate");
                HealthState requiredState = this.GetRequiredState(expectedMonitorState);
 
                //// Waiting for 5 minutes directly to make sure that OM can get the latest monitor state
                //// because the latest monitor state might be the same as the initial state
                if (ctx.Records.GetValue("expectedmonitorstate").Equals("success",  StringComparison.CurrentCultureIgnoreCase))
                {
                    this.Wait(ctx, 300);

                    this.VerifyMonitor(ctx, requiredState);

                    this.VerifyAlert(ctx, false);
                }
                else
                {
                    this.VerifyMonitor(ctx, requiredState);

                    this.VerifyAlert(ctx, true);

                    if (string.IsNullOrEmpty(diagnosticName))
                    {
                        ctx.Trc("No diagnostic defined!");
                    }
                    else
                    {
                        ctx.Trc("Verifying diagnostic state: checking " + diagnosticName);
                        DiagnosticHelper diagnosticHelper = new DiagnosticHelper(this.Info, diagnosticName);

                        DiagnosticResult diagnosticResult = diagnosticHelper.SubmitDiagnosticOn(
                            this.ComputerObject,
                            "Microsoft.Unix.OperatingSystem",
                            targetOSClassName + ":" + this.ClientInfo.HostName,
                            monitorName);

                        // note: diagnostic output not verified (TODO)
                        if (diagnosticResult.Status != TaskStatus.Succeeded)
                        {
                            this.Fail(ctx, "Diagnostic FAILED: " + diagnosticName);
                        }
                        else
                        {
                            ctx.Alw("Diagnostic successful: " + diagnosticName);
                            ctx.Trc("Diagnostic output: " + diagnosticResult.Output);
                        }
                    }
                }

                // Run the recovery command
                if (string.IsNullOrEmpty(recoveryCmd))
                {
                    recoveryCmd = "/tmp/scxtestapp.pl -stop";
                }

                ctx.Trc("Running recovery command: " + recoveryCmd);
                this.PosixCmd(ctx, recoveryCmd);

                this.VerifyAlert(ctx, false);
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

            try
            {
                this.VerifyMonitor(ctx, HealthState.Success);
            }
            catch (Exception ex)
            {
                this.Fail(ctx, ex.ToString());
            }

            ctx.Trc("SDKTests.PerformanceWithTwoStateHealth.Verify complete");
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

            string recoveryCmd = ctx.Records.GetValue("recoverycmd");

            // run the recovery command
            if (string.IsNullOrEmpty(recoveryCmd))
            {
                recoveryCmd = "/tmp/scxtestapp.pl -stop";
            }

            ctx.Trc("Running recovery command: " + recoveryCmd);
            this.PosixCmd(ctx, recoveryCmd);

            ctx.Trc("SDKTests.PerformanceWithTwoStateHealth.Cleanup finished");
        }

        #endregion Test Framework Methods

        #region Private Methods

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
                //double monitorWarningThreshold = System.Convert.ToDouble(monitorWarningThresholdStr);
                //double monitorErrorThreshold = System.Convert.ToDouble(monitorErrorThresholdStr);
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
            //string managementPack = ctx.ParentContext.Records.GetValue("managementpack");
            //managementPack = (managementPack.Equals("Microsoft.Linux.UniversalR.1")|| managementPack.Equals("Microsoft.Linux.UniversalD.1")) ? "Microsoft.Linux.Universal" : managementPack;

            //string monitorName = managementPack + ".LogicalDisk.PercentFreeSpace.Monitor";
            //string monitorContext = managementPack + ".LogicalDisk";
            //string monitortarget = ctx.Records.GetValue("monitortarget");
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
