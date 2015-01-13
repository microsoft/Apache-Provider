//-----------------------------------------------------------------------
// <copyright file="PerformanceHealthBase.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>v-litin</author>
// <description>abstract class for holding common functions</description>
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
    /// Enumeration of the override State
    /// </summary>
    public enum OverrideState
    {
        /// <summary>
        /// Override State: Default
        /// </summary>
        Default,

        /// <summary>
        /// Override State: Override
        /// </summary>
        Override
    }

    /// <summary>
    /// Base class for testing performance monitors
    /// </summary>
    public abstract class PerformanceHealthBase
    {
        #region Private Fields

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
        /// Period of time to wait for OM Server to update its internal state to match changes on the agent.
        /// </summary>
        private TimeSpan serverWaitTime = new TimeSpan(0, 0, 30);

        /// <summary>
        /// Maximum number of times to wait for server state change
        /// </summary>
        private int maxServerWaitCount = 15;

        /// <summary>
        /// Holds any unexpected exception generated from within this.ExecuteActionCommand();
        /// </summary>
        private Exception executeActionCommandException = null;

        /// <summary>
        /// Apache agent helper class
        /// </summary>
        private ApacheAgentHelper apacheAgentHelper;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the information about OM under test
        /// </summary>
        protected OMInfo Info
        {
            get { return this.info; }
            set { this.info = value; }
        }

        /// <summary>
        /// Gets or sets the information about client
        /// </summary>
        protected ClientInfo ClientInfo
        {
            get { return this.clientInfo; }
            set { this.clientInfo = value; }
        }

        /// <summary>
        /// Gets or sets the monitorHelper object
        /// </summary>
        protected MonitorHelper MonitorHelper
        {
            get { return this.monitorHelper; }
            set { this.monitorHelper = value; }
        }

        /// <summary>
        /// Gets or sets the alertHelper object
        /// </summary>
        protected AlertsHelper AlertHelper
        {
            get { return this.alertHelper; }
            set { this.alertHelper = value; }
        }

        /// <summary>
        /// Gets or sets the overrideHelper object
        /// </summary>
        protected OverrideHelper OverrideHelper
        {
            get { return this.overrideHelper; }
            set { this.overrideHelper = value; }
        }

        /// <summary>
        /// Gets or sets the computer  object
        /// </summary>
        protected MonitoringObject ComputerObject
        {
            get { return this.computerObject; }
            set { this.computerObject = value; }
        }

        /// <summary>
        /// Gets or sets the ApacheAgentHelper
        /// </summary>
        public ApacheAgentHelper ApacheAgentHelper
        {
            get { return apacheAgentHelper; }
            set { apacheAgentHelper = value; }
        }


        protected TimeSpan ServerWaitTime
        {
            get { return this.serverWaitTime; }
            set { this.serverWaitTime = value; }
        }

        /// <summary>
        /// Gets or sets the max server wait time
        /// </summary>
        protected int MaxServerWaitCount
        {
            get { return this.maxServerWaitCount; }
            set { this.maxServerWaitCount = value; }
        }

        /// <summary>
        /// Gets or sets the execute action command exceptione
        /// </summary>
        protected Exception ExecuteActionCommandException
        {
            get { return this.executeActionCommandException; }
            set { this.executeActionCommandException = value; }
        }

        #endregion
        /// <summary>
        /// Verifies that the monitor specified in the testing context is in the required state
        /// </summary>
        /// <param name="ctx">Current testing context</param>
        /// <param name="requiredState">Required monitor state</param>
        protected void VerifyMonitor(IContext ctx, HealthState requiredState)
        {
            string monitorName = ctx.Records.GetValue("monitorname");
            string monitorTarget = ctx.Records.GetValue("monitortarget");

            string monitorTag = string.IsNullOrEmpty(monitorTarget) ? monitorName : string.Format("{0} -> {1}", monitorName, monitorTarget);

            HealthState monitorHealth = HealthState.Uninitialized;

            if (monitorHealth == requiredState)
            {
                ctx.Alw("PASS: monitor in state: " + monitorHealth.ToString());
            }
            else
            {
                int numTries = 0;

                ctx.Trc("VerifyMonitor: " + monitorTag);

                while (numTries < this.maxServerWaitCount && monitorHealth != requiredState)
                {
                    //if (numTries > 0)
                    //{
                        this.Wait(ctx);
                    //}

                    monitorHealth = string.IsNullOrEmpty(monitorTarget)
                        ? this.monitorHelper.GetMonitorHealthState(this.computerObject, monitorName)
                        : this.monitorHelper.GetMonitorHealthState(this.computerObject, monitorName, monitorTarget);

                    numTries++;

                    if (monitorHealth == requiredState)
                    {
                        ctx.Alw("PASS: monitor in state: " + monitorHealth.ToString());
                        return;
                    }

                    ctx.Trc(string.Format("monitor {0} -> {1} in state: {2} after {3}/{4} tries.", monitorName, monitorTarget, monitorHealth.ToString(), numTries, this.maxServerWaitCount));
                }

                this.Fail(ctx, string.Format("monitor {0} -> {1} in state: {2} after {3}/{4}/ tries.", monitorName, monitorTarget, monitorHealth.ToString(), numTries, this.maxServerWaitCount));
            }
        }  


        /// <summary>
        /// Verifies that the alert specified in the testing context is active
        /// </summary>
        /// <param name="ctx">Current testing context</param>
        /// <param name="shouldExist">Whether the alert is expected to be active</param>
        protected void VerifyAlert(IContext ctx, bool shouldExist)
        {
            string monitorName = ctx.Records.GetValue("monitorname");
            string monitorTarget = ctx.Records.GetValue("monitortarget");

            MonitoringAlert alert = this.alertHelper.GetMonitorAlert(this.computerObject, monitorName, monitorTarget);

            ctx.Trc(string.Format("VerifyAlert: {0}  shouldExist: {1}", monitorName, shouldExist));

            bool alertExists = !shouldExist;

   
            int numTries = 0;

            while (numTries < this.maxServerWaitCount && alertExists != shouldExist)
            {
                //if (numTries > 0)
                //{
                    this.Wait(ctx);
                //}

                numTries++;

                alert = this.alertHelper.GetMonitorAlert(this.computerObject, monitorName, monitorTarget);

                alertExists = this.alertHelper.IsActive(alert);

                ctx.Trc(alertExists
                    ? string.Format("alert {0} -> {1} -> \"{2}\" in state: active after {3}/{4} tries.", monitorName, monitorTarget, alert.Name, numTries, this.maxServerWaitCount)
                    : string.Format("alert {0} -> {1} in state: inactive after {2}/{3} tries.", monitorName, monitorTarget, numTries, this.maxServerWaitCount));
            }

            if (alertExists == shouldExist)
            {
                ctx.Alw("PASS: alert in state: " + (alertExists ? "active" : "inactive"));
            }
            else if (alert != null && !string.IsNullOrEmpty(alert.Name))
            {
                this.Fail(ctx, string.Format("alert {0}->\"{1}\" in state: {2}", monitorName, alert.Name, (alertExists ? "active" : "inactive")));
            }
            else
            {
                this.Fail(ctx, string.Format("alert {0} in state: {1}", monitorName, (alertExists ? "active" : "inactive")));
            }
        }

        /// <summary>
        /// Get the enumeration of the expected monitor state
        /// </summary>
        /// <param name="expectedMonitorState">the expected state of the monitor</param>
        /// <returns>Return the enumeration of the expected monitor state</returns>
        protected HealthState GetRequiredState(string expectedMonitorState)
        {
            switch (expectedMonitorState)
            {
                case "Warning":
                    {
                        return HealthState.Warning;
                    }

                case "Error":
                    {
                        return HealthState.Error;
                    }

                case "Success":
                    {
                        return HealthState.Success;
                    }

                case "Uninitialized":
                    {
                        return HealthState.Uninitialized;
                    }

                default:
                    {
                        return HealthState.Uninitialized;
                    }
            }
        }

        /// <summary>
        /// Closes all alerts relevant to the current test
        /// </summary>
        /// <param name="ctx">current context</param>
        protected void CloseMatchingAlerts(IContext ctx)
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
        protected void RecoverMonitorIfFailed(IContext ctx)
        {
            string monitorName = ctx.Records.GetValue("monitorname");
            string monitorTarget = ctx.Records.GetValue("monitortarget");
            string recoveryCmd = ctx.Records.GetValue("recoverycmd");

            int numTries = 0;

            HealthState monitorHealth = HealthState.Uninitialized;

            // run the recovery command
            if (string.IsNullOrEmpty(recoveryCmd))
            {
                recoveryCmd = "/tmp/scxtestapp.pl -stop";
            }

            while (numTries < this.maxServerWaitCount && monitorHealth == HealthState.Uninitialized)
            {
                if (numTries > 0)
                {
                    this.Wait(ctx);
                }

                monitorHealth = string.IsNullOrEmpty(monitorTarget)
                    ? this.monitorHelper.GetMonitorHealthState(this.computerObject, monitorName)
                    : this.monitorHelper.GetMonitorHealthState(this.computerObject, monitorName, monitorTarget);

                numTries++;

                if (monitorHealth == HealthState.Success)
                {
                    ctx.Trc("monitor in state: " + monitorHealth.ToString());
                    return;
                }

                ctx.Trc(string.Format("monitor {0} in state: {1} after {2}/{3} tries.", monitorName, monitorHealth.ToString(), numTries, this.maxServerWaitCount));
            }

            if (monitorHealth == HealthState.Error)
            {
                ctx.Alw("RecoverMonitorIfFailed(): recovering: " + monitorName);
                this.PosixCmd(ctx, recoveryCmd);
            }
            else if (monitorHealth == HealthState.Uninitialized)
            {
                this.Abort(ctx, string.Format("monitor {0} in state: {1} after {2}/{3} tries.", monitorName, monitorHealth.ToString(), numTries, this.maxServerWaitCount));
            }
        }

        /// <summary>
        /// Run the performance test action asynchronously
        /// </summary>
        /// <param name="ctx">Current context</param>
        protected void RunActionCmdAsynchronous(IContext ctx)
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
        protected void ExecuteActionCommand(object ctx)
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
        /// <param name="output">parameter for returning the output of the command</param>
        protected void PosixCmd(IContext ctx, string cmd, out string output)
        {
            RunPosixCmd posixCmd = new RunPosixCmd(
                this.clientInfo.HostName,
                this.clientInfo.SuperUser,
                this.clientInfo.SuperUserPassword)
            {
                IgnoreExit = true
            };

            bool success = false;

            int maxTries = 3;

            for (int tries = 0; !success && tries < maxTries; tries++)
            {
                try
                {
                    posixCmd.RunCmd(cmd);
                    success = true;
                }
                catch (Exception ex)
                {
                    ctx.Err(string.Format("PosixCmd('{0}') failed on attempt {1}/{2}: {3}", cmd, tries, maxTries, ex.Message));
                    success = false;
                }

                if (!success)
                {
                    this.Wait(ctx);
                }
            }

            if (!success)
            {
                throw new Exception(string.Format("Failed to run SSH command after {0} tries: {1}", maxTries, cmd));
            }

            output = posixCmd.StdOut;
        }

        /// <summary>
        /// Wrapper method to enable executing RunPosixCmd with a single parameter containing
        /// both file name and arguments. This runs binary commands only - shell scripts
        /// will result in an uncaught exception in a separate thread.
        /// </summary>
        /// <param name="ctx">Current MCF context</param>
        /// <param name="cmd">Command line, for example '/etc/init.d/syslog restart'</param>
        /// <returns>return the output of the command</returns>
        protected string PosixCmd(IContext ctx, string cmd)
        {
            RunPosixCmd posixCmd = new RunPosixCmd(
                this.clientInfo.HostName,
                this.clientInfo.SuperUser,
                this.clientInfo.SuperUserPassword)
            {
                IgnoreExit = true
            };

            bool success = false;

            int maxTries = 3;

            for (int tries = 0; !success && tries < maxTries; tries++)
            {
                try
                {
                    posixCmd.RunCmd(cmd);
                    success = true;
                }
                catch (Exception ex)
                {
                    ctx.Err(string.Format("PosixCmd('{0}') failed on attempt {1}/{2}: {3}", cmd, tries, maxTries, ex.Message));
                    success = false;
                }

                if (!success)
                {
                    this.Wait(ctx);
                }
            }

            if (!success)
            {
                throw new Exception(string.Format("Failed to run SSH command after {0} tries: {1}", maxTries, cmd));
            }

            return posixCmd.StdOut;
        }

        /// <summary>
        /// Generic wait method for use to allow OM internal state to stabilize.
        /// </summary>
        /// <param name="ctx">Current context</param>
        protected void Wait(IContext ctx)
        {
            ctx.Trc(string.Format("Waiting for {0}...", this.serverWaitTime));
            System.Threading.Thread.Sleep(this.serverWaitTime);
        }

        /// <summary>
        /// Wait for the given number of seconds
        /// </summary>
        /// <param name="ctx">Current context</param>
        /// <param name="seconds">Number of seconds to wait</param>
        protected void Wait(IContext ctx, int seconds)
        {
            ctx.Trc(string.Format("Waiting for {0}...", seconds));
            System.Threading.Thread.Sleep(new TimeSpan(0, 0, seconds));
        }

        /// <summary>
        /// Fail the text case by printing out a log message and throwing an exception
        /// </summary>
        /// <param name="ctx">Current context</param>
        /// <param name="msg">Error message</param>
        protected void Fail(IContext ctx, string msg)
        {
            ctx.Trc("PerformanceHealth FAIL: " + msg);
            throw new VarFail(msg);
        }

        /// <summary>
        /// Abort the text case by printing out a log message and throwing an exception
        /// </summary>
        /// <param name="ctx">Current context</param>
        /// <param name="msg">Error message</param>
        protected void Abort(IContext ctx, string msg)
        {
            ctx.Trc("PerformanceHealth ABORT: " + msg);
            throw new VarAbort(msg);
        }

        /// <summary>
        /// Utility method determining whether to skil the current test case
        /// </summary>
        /// <param name="ctx">Current context</param>
        /// <returns>whether to skip the test case</returns>
        protected bool SkipThisTest(IContext ctx)
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
        /// GetVirtualHost monitor.
        /// </summary>
        /// <param name="ctx">Current context</param>
        protected MonitoringObject GetVitualHostMonitor(string hostname, string instanceID)
        {
            MonitoringObject monitor = null;
            try
            {
                monitor = this.apacheAgentHelper.GetVirtualHostMonitor(hostname + "," + instanceID);
            }
            catch (Exception)
            {
                if (hostname.Contains(".scx.com"))
                {
                    int index = hostname.IndexOf(".scx.com");
                    string tempHost = hostname.Substring(0, index);
                    monitor = this.apacheAgentHelper.GetVirtualHostMonitor(tempHost + "," + instanceID);
                }
            }

            return monitor;
        }
    }
}
