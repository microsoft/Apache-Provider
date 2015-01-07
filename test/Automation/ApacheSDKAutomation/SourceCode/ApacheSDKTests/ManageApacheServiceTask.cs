//-----------------------------------------------------------------------
// <copyright file="ManageApacheServiceTask.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>v-yabo</author>
// <description></description>
// <history>12/24/2014 3:21:10 PM: Created</history>
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
    /// Description for ManageApacheServiceTask.
    /// Test Case 725217 Verify Reload Apache HTTP Server (Graceful) task works.
    /// Test Case 725218 Verify Restart Apache HTTP Server task works.
    /// Test Case 725219 Verify Update Discovered Properties task works
    /// </summary>
    /// 
    public class ManageApacheServiceTask : ISetup, IRun, IVerify, ICleanup
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
        /// Helper class to check status of monitors.
        /// </summary>
        private MonitorHelper monitorHelper;

        /// <summary>
        /// A MonitoroingObject representing the client machine.
        /// </summary>
        private MonitoringObject apacheInstance;

        /// <summary>
        /// Helper class to run task
        /// </summary>
        private TasksHelper tasksHelper;

        /// <summary>
        /// object to hosd consumer task result
        /// </summary>
        private IList<Microsoft.EnterpriseManagement.Runtime.TaskResult> taskResult;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DiscoverServerTask class.
        /// </summary>
        public ManageApacheServiceTask()
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
            ctx.Trc("ApacheSDKTests.DeployModuleTask.Setup");

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

                this.tasksHelper = new TasksHelper(ctx.Trc, this.info);

                this.monitorHelper = new MonitorHelper(this.info);

                string monitorInstanceClass = ctx.Records.GetValue("InstanceClass");

                this.apacheInstance = this.monitorHelper.GetMonitoringObject(monitorInstanceClass, this.clientInfo.HostName);
                
            }
            catch (Exception ex)
            {
                this.Abort(ctx, ex.ToString());
            }

            ctx.Trc("ApacheSDKTests.DeployModuleTask.Setup complete");
        }

        /// <summary>
        /// Framework Run method
        /// </summary>
        /// <param name="ctx">Current context</param>
        void IRun.Run(IContext ctx)
        {
            ctx.Trc("ApacheSDKTests.ReloadApacheTask.Run");

            string entity = ctx.Records.GetValue("entityname");
            string consumerTask = ctx.Records.GetValue("taskname");

            ctx.Trc("SDKTests.ConsumerTaskHealth.Run: ConsumerTaskHealth test: " + entity);

            try
            {
                this.taskResult = this.tasksHelper.RunConsumerTask(this.apacheInstance, consumerTask);
            }
            catch (Exception ex)
            {
                this.Abort(ctx, ex.ToString());
            }

            ctx.Trc("ApacheSDKTests.ReloadApacheTask.Run complete");
        }

        /// <summary>
        /// Framework verify method
        /// </summary>
        /// <param name="ctx">Current context</param>
        void IVerify.Verify(IContext ctx)
        {
            ctx.Trc("ApacheSDKTests.ReloadApacheTask.Verify");
            try
            {
                Microsoft.EnterpriseManagement.Runtime.TaskStatus expectedTaskStatus = this.GetExpectedTaskStatus(ctx.Records.GetValue("expectedTaskStatus"));
                string expectedOutputKeyWord = ctx.Records.GetValue("expectedOutputKeyWord");

                if (this.taskResult != null)
                {
                    ctx.Trc(String.Format("Pass: Consumer task execute fine:{0}", ctx.Records.GetValue("taskname")));
                }
                else
                {
                    this.Fail(ctx, "Consumer Task Executes Fail!");
                }

                if (this.taskResult[0].Status == expectedTaskStatus)
                {
                    ctx.Trc(string.Format("Pass: expectedTaskStatus: {0}, actualTaskStatus: {1}", expectedTaskStatus, this.taskResult[0].Status));
                }
                else
                {
                    this.Fail(ctx, string.Format("Fail: expectedTaskStatus: {0}, actualTaskStatus: {1}", expectedTaskStatus, this.taskResult[0].Status));
                }

                if (this.taskResult[0].Output.Contains(expectedOutputKeyWord))
                {
                    ctx.Trc(string.Format("Pass: expectedOutputKeyWord: {0}, actualOutputKeyWord: {1}", expectedOutputKeyWord, this.taskResult[0].Output));
                }
                else
                {
                    this.Fail(ctx, string.Format("Fail: expectedOutputKeyWord: {0}, actualOutputKeyWord: {1}", expectedOutputKeyWord, this.taskResult[0].Output));
                }

            }
            catch (Exception ex)
            {
                this.Fail(ctx, ex.Message);
            }

            ctx.Trc("ApacheSDKTests.ReloadApacheTask.Verify.Complete!");
        }

        /// <summary>
        /// Framework cleanup method
        /// </summary>
        /// <param name="ctx">Current context</param>
        void ICleanup.Cleanup(IContext ctx)
        {
            ctx.Trc("ApacheSDKTests.ReloadApacheTask.Cleanup");
            ctx.Trc("ApacheSDKTests.ReloadApacheTask.Cleanup finished");
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
