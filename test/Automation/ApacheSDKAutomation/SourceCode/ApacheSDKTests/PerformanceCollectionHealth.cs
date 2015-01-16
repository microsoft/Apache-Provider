//-----------------------------------------------------------------------
// <copyright file="PerformanceCollectionHealth.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>v-ashu</author>
// <description></description>
// <history>8/26/2011 12:00:00 AM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Apache.SDK.ApacheSDKTests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text.RegularExpressions;    
    using Infra.Frmwrk;
    using Microsoft.EnterpriseManagement.Configuration;
    using Microsoft.EnterpriseManagement.Monitoring;
    using Scx.Test.Common;
    using Scx.Test.Apache.SDK.ApacheSDKHelper;
    
    /// <summary>
    /// Class for testing performance collection rules
    /// </summary>
    public class PerformanceCollectionHealth : ISetup, IVerify, ICleanup
    {
        #region Private Fields

        /// <summary>
        /// Period of time to wait for OM Server to update its internal state to match changes on the agent.
        /// </summary>
        private readonly TimeSpan serverWaitTime = new TimeSpan(0, 0, 30);

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
        /// Apache agent helper class
        /// </summary>
        private ApacheAgentHelper apacheAgentHelper;

        /// <summary>
        /// Maximum number of times to wait for server state change
        /// </summary>
        private int maxServerWaitCount = 10;

        /// <summary>
        /// Helper class to manipulate management pack overrides
        /// </summary>
        private OverrideHelper overrideHelper;

        /// <summary>
        /// A MonitoroingObject representing the client machine.
        /// </summary>
        private MonitoringObject monitorObject;


        #endregion

        /// <summary>
        /// Initializes a new instance of the PerformanceCollectionHealth class
        /// </summary>
        public PerformanceCollectionHealth()
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

            ctx.Trc("Scx.Test.Apache.SDK.ApacheSDKTests.PerformanceCollectionHealth.Setup");

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

                this.apacheAgentHelper = new ApacheAgentHelper(this.info, this.clientInfo) { Logger = ctx.Trc };

                this.overrideHelper = new OverrideHelper(ctx.Trc, this.info, ctx.ParentContext.Records.GetValue("testingoverride"));

                string monitorContext = ctx.Records.GetValue("monitorcontext");
                string monitorTarget = ctx.Records.GetValue("monitortarget");
                if (monitorTarget.Equals("VirtualHost"))
                {
                    string instanceID = ctx.Records.GetValue("InstanceID");
                    this.monitorObject = this.GetVitualHostMonitor(this.clientInfo.HostName, instanceID);
                }
                else
                {
                    this.monitorObject = this.monitorHelper.GetMonitoringObject(monitorContext, this.clientInfo.HostName);
                }

                this.ApplyCollectionRuleOverride(ctx);
            }
            catch (Exception ex)
            {
                this.Abort(ctx, ex.ToString());
            }

            ctx.Trc("Scx.Test.Apache.SDK.ApacheSDKTests.PerformanceCollectionHealth.Setup complete");
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

            ctx.Trc("Scx.Test.Apache.SDK.ApacheSDKTests.PerformanceCollectionHealth.Verify");
            
            try
            {
                this.VerifyPerformanceData(ctx);
            }
            catch (Exception ex)
            {
                this.Fail(ctx, ex.ToString());
            }

            ctx.Trc("Scx.Test.Apache.SDK.ApacheSDKTests.PerformanceCollectionHealth.Verify complete");
        }

        /// <summary>
        /// Framework cleanup method
        /// </summary>
        /// <param name="ctx">Current context</param>
        void ICleanup.Cleanup(IContext ctx)
        {
            ctx.Trc("Scx.Test.Apache.SDK.ApacheSDKTests.PerformanceCollectionHealth.Cleanup");
            if (this.SkipThisTest(ctx))
            {
                return;
            }

            this.DeleteCollectionRuleOverride(ctx);

            ctx.Trc("Scx.Test.Apache.SDK.ApacheSDKTests.PerformanceCollectionHealth.Cleanup finished");
        }

        #endregion Test Framework Methods

        #region Private Methods

        /// <summary>
        /// GetVirtualHost monitor.
        /// </summary>
        /// <param name="ctx">Current context</param>
        private MonitoringObject GetVitualHostMonitor(string hostname, string instanceID)
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

        /// <summary>
        /// Apply collection rule Override.
        /// </summary>
        /// <param name="ctx">Current context</param>
        private void ApplyCollectionRuleOverride(IContext ctx)
        {
            string ruleName = ctx.Records.GetValue("rulename");
            string monitorContext = ctx.Records.GetValue("monitorcontext");
            this.overrideHelper.SetClientCollectionRuleParameter(this.monitorObject, ruleName, monitorContext, "Interval", "10");
        }

        /// <summary>
        /// Delete collection rule Override.
        /// </summary>
        /// <param name="ctx">Current context</param>
        private void DeleteCollectionRuleOverride(IContext ctx)
        {
            string ruleName = ctx.Records.GetValue("rulename");
            string monitorContext = ctx.Records.GetValue("monitorcontext");
            this.overrideHelper.DeleteClientCollectionRuleParameter(this.monitorObject, ruleName, monitorContext, "Interval");
        }

        /// <summary>
        /// Verify the performance data.
        /// </summary>
        /// <param name="ctx">Current context</param>
        private void VerifyPerformanceData(IContext ctx)
        {
            // According to specified time range, verify sample performance data
            DateTime baseTime;
            TimeSpan startRange;
            TimeSpan endRange;

            if (false == DateTime.TryParse(ctx.ParentContext.Records.GetValue("basetime"), out baseTime))
            {
                baseTime = DateTime.Now;
            }

            TimeSpan.TryParse(ctx.ParentContext.Records.GetValue("startrange"), out startRange);
            DateTime startTime = baseTime + startRange;
            TimeSpan.TryParse(ctx.ParentContext.Records.GetValue("endrange"), out endRange);
            DateTime endTime = baseTime + endRange;
            ctx.Trc(string.Format("Verify sample performance date in the range: '{0}' - '{1}'", startTime, endTime));

            // Find the monitored logic disk object
            // Find the performance date of the special Collection Rule on special monitoring object
            string ruleName = ctx.Records.GetValue("rulename");
            ctx.Trc(string.Format("Verify sample performance date of collection rule : {0}", ruleName));

            ReadOnlyCollection<MonitoringPerformanceData> perfDataCollection = this.GetPerformanceData(ctx, this.monitorObject, ruleName);

            // Verify performance data
            int sampleValueCount = 0;
            foreach (MonitoringPerformanceData perfData in perfDataCollection)
            {
                ReadOnlyCollection<MonitoringPerformanceDataValue> sampleValues = perfData.GetValues(startTime, endTime);
                sampleValueCount = sampleValues.Count;
                foreach (MonitoringPerformanceDataValue dataValue in sampleValues)
                {                    
                    string pattern = ctx.Records.GetValue("pattern");
                    if (String.IsNullOrEmpty(pattern))
                    {
                        throw new ApplicationException("The value of pattern key is not set!");
                    }
                    else
                    {
                        Regex regex = new Regex(pattern);
                        if (regex.IsMatch(dataValue.SampleValue.ToString()))
                        {
                            ctx.Trc(String.Format("Performance Collection Rule '{0}': Sample Value '{1}' does match pattern '{2}' as expected.", ruleName, dataValue.SampleValue, pattern));
                        }
                        else
                        {
                            throw new VarFail(String.Format("Performance Collection Rule '{0}': Sample Value '{1}' does not match pattern '{2}' unexpectedly.", ruleName, dataValue.SampleValue, pattern));
                        }
                    }
                }
            }

            if (sampleValueCount == 0)
            {
                throw new VarFail(string.Format("FAIL: Collection Rule '{0}' not found", ruleName));
            }

            ctx.Alw(String.Format("PASS: Performance Collection Rule '{0}': {1} Sample Value found during '{2} - {3}'as expected.", ruleName, sampleValueCount, startTime, endTime));
        }

        /// <summary>
        /// Get Performance Data for special collection rule on the special monitoring Object
        /// </summary>
        /// <param name="ctx">Current testing context</param>
        /// <param name="monitoringObject">the special monitoring Object, like logical disk "/"</param>
        /// <param name="ruleName">special collection rule name like "Microsoft.AIX.5.3.LogicalDisk.PercentFreeInodes.Collection"</param>
        /// <returns>Performance Data for special collection rule on the special monitoring Object</returns>
        private ReadOnlyCollection<MonitoringPerformanceData> GetPerformanceData(IContext ctx, MonitoringObject monitoringObject, string ruleName)
        {
            int numTries = 0;

            ReadOnlyCollection<MonitoringPerformanceData> perfDataCollection = null;

            while (numTries < this.maxServerWaitCount && (perfDataCollection == null || perfDataCollection.Count == 0))
            {
                if (numTries > 0)
                {
                    this.Wait(ctx);
                }

                numTries++;

                try
                {
                    // Retrieve performance data                    
                    ManagementPackRuleCriteria ruleCriteria = new ManagementPackRuleCriteria(string.Format("Name='{0}'", ruleName));
                    IList<ManagementPackRule> rules = monitoringObject.ManagementGroup.Monitoring.GetRules(ruleCriteria);
                    if (rules.Count == 0)
                    {
                        throw new VarFail(string.Format("FAIL: Collection Rule '{0}' not found", ruleName));
                    }

                    MonitoringPerformanceDataCriteria dataCriteria = new MonitoringPerformanceDataCriteria(string.Format("MonitoringRuleId='{0}'", rules[0].Id.ToString()));
                    perfDataCollection = monitoringObject.GetMonitoringPerformanceData(dataCriteria);                         
                }
                catch (Exception)
                {
                    ctx.Trc(string.Format("Attempt: {0}/{1} - Collection Rule '{2}' not found", numTries, this.maxServerWaitCount, ruleName));
                    continue;
                }
            }

            if (perfDataCollection == null || perfDataCollection.Count == 0)
            {
                throw new VarFail(string.Format("FAIL: Performance Data for Collection Rule '{0}' not found", ruleName));
            }

            return perfDataCollection;
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
                this.clientInfo.SuperUserPassword);

            posixCmd.IgnoreExit = true;

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
        /// Fail the text case by printing out a log message and throwing an exception
        /// </summary>
        /// <param name="ctx">Current context</param>
        /// <param name="msg">Error message</param>
        private void Fail(IContext ctx, string msg)
        {
            ctx.Trc("PerformanceCollectionHealth FAIL: " + msg);
            throw new VarFail(msg);
        }

        /// <summary>
        /// Abort the text case by printing out a log message and throwing an exception
        /// </summary>
        /// <param name="ctx">Current context</param>
        /// <param name="msg">Error message</param>
        private void Abort(IContext ctx, string msg)
        {
            ctx.Trc("PerformanceCollectionHealth ABORT: " + msg);
            throw new VarAbort(msg);
        }

        /// <summary>
        /// Utility method determining whether to skil the current test case
        /// </summary>
        /// <param name="ctx">Current context</param>
        /// <returns>whether to skip the test case</returns>
        private bool SkipThisTest(IContext ctx)
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

            foreach (string entity in entities)
            {
                if (ctx.Records.GetValue("entityname") == entity)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
