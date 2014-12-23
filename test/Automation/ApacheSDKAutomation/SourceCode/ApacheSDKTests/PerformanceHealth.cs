//-----------------------------------------------------------------------
// <copyright file="PerformanceHealth.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>v-andlei</author>
// <description></description>
// <history>12/17/2014 2:28:29 PM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Apache.SDK.ApacheSDKTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Infra.Frmwrk;
    using Microsoft.EnterpriseManagement.Configuration;
    using Microsoft.EnterpriseManagement.Monitoring;
    using Scx.Test.Common;
    using Scx.Test.Apache.SDK.ApacheSDKHelper;
    
    /// <summary>
    /// Class for testing performance monitors
    /// </summary>
    public class PerformanceHealth : ISetup, IRun, IVerify, ICleanup
    {
        #region Private Fields

        /// <summary>
        /// Maximum number of times to wait for server state change
        /// </summary>
        private const int MaxServerWaitCount = 10;

        /// <summary>
        /// Period of time to wait for OM Server to update its internal state to match changes on the agent.
        /// </summary>
        private readonly TimeSpan serverWaitTime = new TimeSpan(0, 1, 0);

        /// <summary>
        /// Information about OM under test
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
        /// Helper class to manipulate management pack overrides
        /// </summary>
        private OverrideHelper overrideHelper;

        /// <summary>
        /// Monitoring object for the unix computer
        /// </summary>
        private MonitoringObject computerObject;

        /// <summary>
        /// Holds any unexpected exception generated from within this.ExecuteActionCommand();
        /// </summary>
        private Exception executeActionCommandException = null;

        #endregion

        /// <summary>
        /// Initializes a new instance of the PerformanceHealth class
        /// </summary>
        public PerformanceHealth()
        {
        }

        #region Test Framework Methods

        /// <summary>
        /// Framework setup method
        /// </summary>
        /// <param name="ctx">Current context</param>
        void ISetup.Setup(IContext ctx)
        {
            if (SkipThisTest(ctx))
            {
                ctx.Trc("BYPASSING TEST CASE (ISetup.Setup): " + ctx.Records.GetValue("entityname"));
                return;
            }

            ctx.Trc("Apache.SDKTests.PerformanceHealth.Setup");

            string recoveryCmd = ctx.Records.GetValue("recoverycmd");

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

                this.monitorHelper = new MonitorHelper(this.info);

                this.alertHelper = new AlertsHelper(this.info);

                this.overrideHelper = new OverrideHelper(ctx.Trc, this.info, ctx.ParentContext.Records.GetValue("testingoverride"));

                this.computerObject = this.monitorHelper.GetComputerObject(this.clientInfo.HostName);

                this.RecoverMonitorIfFailed(ctx);

                // override the monitor threshold back to default values
                this.ApplyDefaultMonitorOverride(ctx, 30);

                this.CloseMatchingAlerts(ctx);

                this.VerifyMonitor(ctx, HealthState.Success);
            }
            catch (Exception ex)
            {
                Abort(ctx, ex.ToString());
            }

            ctx.Trc("Apache.SDKTests.PerformanceHealth.Setup complete");
        }

        /// <summary>
        /// Framework Run method
        /// </summary>
        /// <param name="ctx">Current context</param>
        void IRun.Run(IContext ctx)
        {
            string entity = ctx.Records.GetValue("entityname");
            string action = ctx.Records.GetValue("action");
            string monitorName = ctx.Records.GetValue("monitorname");
            string alertName = ctx.Records.GetValue("alert");
            string diagnosticName = ctx.Records.GetValue("diagnostics");
            string recoveryCmd = ctx.Records.GetValue("recoverycmd");
            string targetOSClassName = ctx.ParentContext.Records.GetValue("targetosclass");

            if (SkipThisTest(ctx))
            {
                return;
            }

            ctx.Trc("Apache.SDKTests.PerformanceHealth.Run with entity " + entity);

            try
            {
                this.VerifyMonitor(ctx, HealthState.Success);

                this.VerifyAlert(ctx, false);

                this.ApplyMonitorOverride(ctx, 30);


                ctx.Alw("Running command: " + action);
                this.RunActionCmdAsynchronous(ctx);
                

                this.VerifyMonitor(ctx, HealthState.Error);

                this.VerifyAlert(ctx, true);

                if (string.IsNullOrEmpty(diagnosticName))
                {
                    ctx.Trc("No diagnostic defined!");
                }
                else
                {
                    ctx.Trc("Verifying diagnostic state: checking " + diagnosticName);
                    DiagnosticHelper diagnosticHelper = new DiagnosticHelper(this.info, diagnosticName);

                    DiagnosticResult diagnosticResult = diagnosticHelper.SubmitDiagnosticOn(
                        this.computerObject,
                        "Microsoft.Unix.OperatingSystem",
                        targetOSClassName + ":" + this.clientInfo.HostName,
                        monitorName);

                    // note: diagnostic output not verified (TODO)
                    if (diagnosticResult.Status != TaskStatus.Succeeded)
                    {
                        Fail(ctx, "Diagnostic FAILED: " + diagnosticName);
                    }
                    else
                    {
                        ctx.Alw("Diagnostic successful: " + diagnosticName);
                        ctx.Trc("Diagnostic output: " + diagnosticResult.Output);
                    }
                }

                // override the monitor threshold back to default values
                this.ApplyDefaultMonitorOverride(ctx, 30);

                this.VerifyAlert(ctx, false);
            }
            catch (Exception ex)
            {
                Fail(ctx, ex.ToString());
            }

            ctx.Trc("Apache.SDKTests.PerformanceHealth.Run complete");
        }

        /// <summary>
        /// Framework verify method
        /// </summary>
        /// <param name="ctx">current context</param>
        void IVerify.Verify(IContext ctx)
        {
            if (SkipThisTest(ctx))
            {
                return;
            }

            ctx.Trc("Apache.SDKTests.PerformanceHealth.Verify");
            
            try
            {
                this.VerifyMonitor(ctx, HealthState.Success);
            }
            catch (Exception ex)
            {
                Fail(ctx, ex.ToString());
            }

            ctx.Trc("Apache.SDKTests.PerformanceHealth.Verify complete");
        }

        /// <summary>
        /// Framework cleanup method
        /// </summary>
        /// <param name="ctx">Current context</param>
        void ICleanup.Cleanup(IContext ctx)
        {
            if (SkipThisTest(ctx))
            {
                return;
            }

            this.DeleteMonitorOverride(ctx);

            string recoveryCmd = ctx.Records.GetValue("recoverycmd");

            // run the recovery command
            if (!string.IsNullOrEmpty(recoveryCmd))
            {
                ctx.Trc("Running recovery command: " + recoveryCmd);
                this.PosixCmd(ctx, recoveryCmd);
            }

            ctx.Trc("Apache.SDKTests.PerformanceHealth.Cleanup finished");
        }

        #endregion Test Framework Methods

        #region Private Methods

        /// <summary>
        /// Fail the text case by printing out a log message and throwing an exception
        /// </summary>
        /// <param name="ctx">Current context</param>
        /// <param name="msg">Error message</param>
        private static void Fail(IContext ctx, string msg)
        {
            ctx.Trc("PerformanceHealth FAIL: " + msg);
            throw new VarFail(msg);
        }

        /// <summary>
        /// Abort the text case by printing out a log message and throwing an exception
        /// </summary>
        /// <param name="ctx">Current context</param>
        /// <param name="msg">Error message</param>
        private static void Abort(IContext ctx, string msg)
        {
            ctx.Trc("PerformanceHealth ABORT: " + msg);
            throw new VarAbort(msg);
        }

        /// <summary>
        /// Utility method determining whether to skil the current test case
        /// </summary>
        /// <param name="ctx">Current context</param>
        /// <returns>whether to skip the test case</returns>
        private static bool SkipThisTest(IContext ctx)
        {
            // list of entities to ignore
            string[] entities = new string[] 
            {
                  //// "TotalPercentProcessorTime",
                  //// "AvailableMBytes",
                  //// "AvailableMBytesSwap",
                  "LogicalDisk",
                  //// "LogicalDiskPercentFreeSpace",
                  "NetworkAdapter",
                  //// "PhysicalDiskReadTime",
                  //// "PhysicalDiskWriteTime",
                  //// "PhysicalDiskTransferTime"
            };

            return entities.Any(entity => ctx.Records.GetValue("entityname") == entity);
        }

        /// <summary>
        /// Apply the monitor override in the varmap such that the monitor under test can easily be put in an error condition
        /// </summary>
        /// <param name="ctx">Current context</param>
        /// <param name="interval">Value to override monitor's Interval parameter to.  
        /// This controls number of seconds the health service waits between polls of the monitor state.</param>
        private void ApplyMonitorOverride(IContext ctx, int interval)
        {
            string monitorName = ctx.Records.GetValue("monitorname");
            string monitorContext = ctx.Records.GetValue("monitorcontext");
            string monitorTarget = ctx.Records.GetValue("monitortarget");
            string monitorThresholdStr = ctx.Records.GetValue("monitorthreshold");
     
            if (!string.IsNullOrEmpty(monitorThresholdStr))
            {
                double monitorThreshold = System.Convert.ToDouble(monitorThresholdStr);

                this.overrideHelper.SetClientMonitorThreshold(
                    this.computerObject, 
                    monitorName, 
                    monitorContext,
                    monitorTarget, 
                    monitorThreshold);

                this.overrideHelper.SetClientMonitorInterval(
                    this.computerObject,
                    monitorName,
                    monitorContext,
                    monitorTarget,
                    interval);
            }
        }

        /// <summary>
        /// Apply a monitor override to put the monitor threshold back to in the default value, as defined in the varmap
        /// </summary>
        /// <param name="ctx">Current context</param>
        /// <param name="interval">Value to override monitor's Interval parameter to.  
        /// This controls number of seconds the health service waits between polls of the monitor state.</param>
        private void ApplyDefaultMonitorOverride(IContext ctx, int interval)
        {
            string monitorName = ctx.Records.GetValue("monitorname");
            string monitorContext = ctx.Records.GetValue("monitorcontext");
            string monitorTarget = ctx.Records.GetValue("monitortarget");
            string monitorThresholdStr = ctx.Records.GetValue("defaultmonitorthreshold");

            if (!string.IsNullOrEmpty(monitorThresholdStr))
            {
                double monitorThreshold = System.Convert.ToDouble(monitorThresholdStr);

                this.overrideHelper.SetClientMonitorThreshold(
                    this.computerObject, 
                    monitorName, 
                    monitorContext, 
                    monitorTarget, 
                    monitorThreshold);

                this.overrideHelper.SetClientMonitorInterval(
                    this.computerObject,
                    monitorName,
                    monitorContext,
                    monitorTarget,
                    interval);
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
            string monitorThresholdStr = ctx.Records.GetValue("defaultmonitorthreshold");

            this.overrideHelper.DeleteClientMonitorThreshold(
                this.computerObject,
                monitorName,
                monitorContext,
                monitorTarget);

            this.overrideHelper.DeleteClientMonitorInterval(
                this.computerObject,
                monitorName,
                monitorContext,
                monitorTarget);
        }

        /// <summary>
        /// Verifies that the monitor specified in the testing context is in the required state
        /// </summary>
        /// <param name="ctx">Current testing context</param>
        /// <param name="requiredState">Required monitor state</param>
        private void VerifyMonitor(IContext ctx, HealthState requiredState)
        {
            string monitorName = ctx.Records.GetValue("monitorname");
            string monitorTarget = ctx.Records.GetValue("monitortarget");

            string monitorTag = string.IsNullOrEmpty(monitorTarget) ? monitorName : string.Format("{0} -> {1}", monitorName, monitorTarget);

            HealthState monitorHealth = HealthState.Uninitialized;

            int numTries = 0;

            ctx.Trc("VerifyMonitor: " + monitorTag);

            while (numTries < MaxServerWaitCount && monitorHealth != requiredState)
            {
                if (numTries > 0)
                {
                    this.Wait(ctx);
                }

                monitorHealth = string.IsNullOrEmpty(monitorTarget) ? this.monitorHelper.GetMonitorHealthState(this.computerObject, monitorName) : this.monitorHelper.GetMonitorHealthState(this.computerObject, monitorName, monitorTarget);

                numTries++;

                if (monitorHealth == requiredState)
                {
                    ctx.Alw("PASS: monitor in state: " + monitorHealth.ToString());
                    return;
                }

                ctx.Trc(string.Format("monitor {0} -> {1} in state: {2} after {3}/{4} tries.", monitorName, monitorTarget, monitorHealth.ToString(), numTries, MaxServerWaitCount));
            }

            Fail(ctx, string.Format("monitor {0} -> {1} in state: {2} after {3}/{4}/ tries.", monitorName, monitorTarget, monitorHealth.ToString(), numTries, MaxServerWaitCount));
        }

        /// <summary>
        /// Verifies that the alert specified in the testing context is active
        /// </summary>
        /// <param name="ctx">Current testing context</param>
        /// <param name="shouldExist">Whether the alert is expected to be active</param>
        private void VerifyAlert(IContext ctx, bool shouldExist)
        {
            string monitorName = ctx.Records.GetValue("monitorname");
            string monitorTarget = ctx.Records.GetValue("monitortarget");

            MonitoringAlert alert = this.alertHelper.GetMonitorAlert(this.computerObject, monitorName, monitorTarget);

            ctx.Trc(string.Format("VerifyAlert: {0}  shouldExist: {1}", monitorName, shouldExist));

            bool alertExists = !shouldExist;

            int numTries = 0;

            while (numTries < MaxServerWaitCount && alertExists != shouldExist)
            {
                if (numTries > 0)
                {
                    this.Wait(ctx);
                }

                numTries++;

                alert = this.alertHelper.GetMonitorAlert(this.computerObject, monitorName, monitorTarget);

                alertExists = this.alertHelper.IsActive(alert);

                if (alertExists && ctx.Records.HasKey("AlertName"))
                {
                    alertExists = alert.Name.ToLower().Equals(ctx.Records.GetValue("AlertName").ToLower());
                }

                ctx.Trc(alertExists
                            ? string.Format("alert {0} -> {1} -> \"{2}\" in state: active after {3}/{4} tries.", monitorName, monitorTarget, alert.Name, numTries, MaxServerWaitCount)
                            : string.Format("alert {0} -> {1} in state: inactive after {2}/{3} tries.", monitorName, monitorTarget, numTries, MaxServerWaitCount));
            }

            if (alertExists == shouldExist)
            {
                ctx.Alw("PASS: alert in state: " + (alertExists ? "active" : "inactive"));
            }
            else if (alert != null && !string.IsNullOrEmpty(alert.Name))
            {
                Fail(ctx, string.Format("alert {0}->\"{1}\" in state: {2}", monitorName, alert.Name, (alertExists ? "active" : "inactive")));
            }
            else
            {
                Fail(ctx, string.Format("alert {0} in state: {1}", monitorName, (alertExists ? "active" : "inactive")));
            }
        }

        /// <summary>
        /// Closes all alerts relevant to the current test
        /// </summary>
        /// <param name="ctx">current context</param>
        private void CloseMatchingAlerts(IContext ctx)
        {
            string monitorName = ctx.Records.GetValue("monitorname");
            string monitorTarget = ctx.Records.GetValue("monitortarget");

            List<MonitoringAlert> alerts = this.alertHelper.GetMonitorAlerts(this.computerObject, monitorName, monitorTarget);

            ctx.Trc(string.Format("Closing {0} pre-existing alerts.", alerts.Count));

            foreach (MonitoringAlert alert in alerts)
            {
                ctx.Trc(string.Format("Closing alert {0} -> {1}", monitorName, alert.Name));
                this.alertHelper.CloseAlert(alert);
            }
        }

        /// <summary>
        /// If the monitor is in an error state, attempt a recovery.
        /// </summary>
        /// <param name="ctx">Current testing context</param>
        private void RecoverMonitorIfFailed(IContext ctx)
        {
            string monitorName = ctx.Records.GetValue("monitorname");
            string monitorTarget = ctx.Records.GetValue("monitortarget");
            string recoveryCmd = ctx.Records.GetValue("recoverycmd");

            int numTries = 0;

            HealthState monitorHealth = HealthState.Uninitialized;

            while (numTries < MaxServerWaitCount && monitorHealth == HealthState.Uninitialized)
            {
                if (numTries > 0)
                {
                    this.Wait(ctx);
                }

                monitorHealth = string.IsNullOrEmpty(monitorTarget) ? this.monitorHelper.GetMonitorHealthState(this.computerObject, monitorName) : this.monitorHelper.GetMonitorHealthState(this.computerObject, monitorName, monitorTarget);

                numTries++;

                if (monitorHealth == HealthState.Success)
                {
                    ctx.Trc("monitor in state: " + monitorHealth.ToString());
                    return;
                }

                ctx.Trc(string.Format("monitor {0} in state: {1} after {2}/{3} tries.", monitorName, monitorHealth.ToString(), numTries, MaxServerWaitCount));
            }

            if (monitorHealth == HealthState.Error)
            {
                ctx.Alw("RecoverMonitorIfFailed(): recovering: " + monitorName);
                if (!string.IsNullOrEmpty(recoveryCmd))
                {
                    this.PosixCmd(ctx, recoveryCmd);
                }
                    
            }
            else if (monitorHealth == HealthState.Uninitialized)
            {
                Abort(ctx, string.Format("monitor {0} in state: {1} after {2}/{3} tries.", monitorName, monitorHealth.ToString(), numTries, MaxServerWaitCount));
            }
        }

        /// <summary>
        /// Run the performance test action asynchronously
        /// </summary>
        /// <param name="ctx">Current context</param>
        private void RunActionCmdAsynchronous(IContext ctx)
        {
            this.executeActionCommandException = null;

            ParameterizedThreadStart posixCmdStart = new ParameterizedThreadStart(this.ExecuteActionCommand);
            Thread posixCmdThread = new Thread(posixCmdStart);
            posixCmdThread.Start(ctx);

            if (this.executeActionCommandException != null)
            {
                throw this.executeActionCommandException;
            }
        }

        /// <summary>
        /// Helper method for ActionCmdAsynchronous
        /// </summary>
        /// <param name="ctx">Current context, cast as an 'object' type</param>
        private void ExecuteActionCommand(object ctx)
        {
            if (!(ctx is IContext))
            {
                throw new VarAbort("Invalid parameter for ExecuteActionCommand: Must be IContext");
            }

            IContext localCtx = (IContext)ctx;
            string action = localCtx.Records.GetValue("action");

            try
            {
                this.PosixCmd(localCtx, action);
            }
            catch (Exception ex)
            {
                this.executeActionCommandException = ex;
            }
        }

        /// <summary>
        /// Wrapper method to enable executing RunPosixCmd with a single parameter containing
        /// both file name and arguments. This runs binary commands only - shell scripts
        /// will result in an uncaught exception in a separate thread.
        /// </summary>
        /// <param name="ctx">Current MCF context</param>
        /// <param name="cmd">Command line, for example '/etc/init.d/syslog restart'</param>
        private void PosixCmd(IContext ctx, string cmd)
        {
            RunPosixCmd posixCmd = new RunPosixCmd(
                this.clientInfo.HostName,
                this.clientInfo.SuperUser,
                this.clientInfo.SuperUserPassword)
                {
                    IgnoreExit = true
                };

            bool success = false;

            const int MaxTries = 3;

            for (int tries = 0; !success && tries < MaxTries; tries++)
            {
                try
                {
                    posixCmd.RunCmd(cmd);
                    success = true;
                }
                catch (Exception ex)
                {
                    ctx.Err(string.Format("PosixCmd('{0}') failed on attempt {1}/{2}: {3}", cmd, tries, MaxTries, ex.Message));
                    success = false;
                }

                if (!success)
                {
                    this.Wait(ctx);
                }
            }

            if (!success)
            {
                throw new Exception(string.Format("Failed to run SSH command after {0} tries: {1}", MaxTries, cmd));
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
        /// Wait for the given number of seconds
        /// </summary>
        /// <param name="ctx">Current context</param>
        /// <param name="seconds">Number of seconds to wait</param>
        private void Wait(IContext ctx, int seconds)
        {
            ctx.Trc(string.Format("Waiting for {0}...", seconds));
            System.Threading.Thread.Sleep(new TimeSpan(0, 0, seconds));
        }

        #endregion
    }
}
