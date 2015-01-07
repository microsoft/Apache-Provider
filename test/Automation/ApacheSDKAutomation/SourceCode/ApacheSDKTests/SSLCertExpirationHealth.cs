//-----------------------------------------------------------------------
// <copyright file="SSLCertExpirationHealth.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>v-litin</author>
// <description>SSLCertExpirationHealth</description>
//-----------------------------------------------------------------------

namespace Scx.Test.Apache.SDK.ApacheSDKTests
{
    using System;
    using System.Threading;
    using Infra.Frmwrk;
    using Microsoft.EnterpriseManagement.Configuration;
    using Microsoft.EnterpriseManagement.Monitoring;
    using Scx.Test.Common;
    using Scx.Test.Apache.SDK.ApacheSDKHelper;

    public class SSLCertExpirationHealth : PerformanceHealthBase, ISetup, IRun, IVerify, ICleanup
    {
        /// <summary>
        /// Initializes a new instance of the SSLCertExpirationHealth class
        /// </summary>
        public SSLCertExpirationHealth()
        {
        }

        /// <summary>
        /// All Shell scripts location.
        /// </summary>
        private string scriptsLocation = System.Environment.CurrentDirectory;

        /// <summary>
        /// Creating ports script name.
        /// </summary>
        private string PortScriptName = "createPortforApache.sh";

        /// <summary>
        /// Reverting ports script name.
        /// </summary>
        private string revertScriptName = "revertapacheconf.sh";

        /// <summary>
        /// Creating SSL certificate script name.
        /// </summary>
        private string SslScriptName = "createSSLCertification.sh";

        /// <summary>
        /// Break Conf File script name.
        /// </summary>
        private string breakConfScriptName = "breakConfFile.sh";

        /// <summary>
        /// Reverting conf file script name.
        /// </summary>
        private string revertConfScriptName = "revertConfFile.sh";

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

            ctx.Trc("SDKTests.SSLCertExpirationHealth.Setup");

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

                this.CloseMatchingAlerts(ctx);

                this.VerifyMonitor(ctx, HealthState.Success);

                this.VerifyAlert(ctx, false);

                //copy all ssl certificate script to '/tmp/'
                PosixCopy copyToHost = new PosixCopy(this.ClientInfo.HostName, this.ClientInfo.SuperUser, this.ClientInfo.SuperUserPassword);
                copyToHost.CopyTo(scriptsLocation + "/" + PortScriptName, "/tmp/" + PortScriptName);
                copyToHost.CopyTo(scriptsLocation + "/" + revertScriptName, "/tmp/" + revertScriptName);
                copyToHost.CopyTo(scriptsLocation + "/" + SslScriptName, "/tmp/" + SslScriptName);
                copyToHost.CopyTo(scriptsLocation + "/" + revertConfScriptName, "/tmp/" + revertConfScriptName);
                copyToHost.CopyTo(scriptsLocation + "/" + breakConfScriptName, "/tmp/" + breakConfScriptName);
            }
            catch (Exception ex)
            {
                this.Abort(ctx, ex.ToString());
            }

            ctx.Trc("SDKTests.SSLCertExpirationHealth.Setup complete");
        }

        /// <summary>
        /// Framework Run method
        /// </summary>
        /// <param name="ctx">Current context</param>
        void IRun.Run(IContext ctx)
        {
            string entity = ctx.Records.GetValue("entityname");
            string action = ctx.Records.GetValue("ActionCmd");
            string monitorName = ctx.Records.GetValue("monitorname");
            string diagnosticName = ctx.Records.GetValue("diagnostics");
            string recoveryCmd = ctx.Records.GetValue("PostCmd");
            string targetOSClassName = ctx.ParentContext.Records.GetValue("targetosclass");

            if (this.SkipThisTest(ctx))
            {
                return;
            }

            ctx.Trc("SDKTests.SSLCertExpirationHealth.Run with entity " + entity);

            try
            {
                ctx.Alw("Running command synchronously (may take a while): " + action);
                RunCmd(action);

                string expectedMonitorState = ctx.Records.GetValue("ExpectedState");
                HealthState requiredState = this.GetRequiredState(expectedMonitorState);

                //// Waiting for 5 minutes directly to make sure that OM can get the latest monitor state
                //// because the latest monitor state might be the same as the initial state
                if (ctx.Records.GetValue("ExpectedState").Equals("success", StringComparison.CurrentCultureIgnoreCase))
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
            }
            catch (Exception ex)
            {
                this.Fail(ctx, ex.ToString());
            }
            finally 
            {
                // Run the recovery command
                if (!(string.IsNullOrEmpty(recoveryCmd)))
                {
                    ctx.Trc("Running recovery command: " + recoveryCmd);
                    RunCmd(recoveryCmd);
                    this.VerifyAlert(ctx, false);
                }
            }

            ctx.Trc("SDKTests.SSLCertExpirationHealth.Run complete");
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

            ctx.Trc("SDKTests.SSLCertExpirationHealth.Verify");

            try
            {
                this.VerifyMonitor(ctx, HealthState.Success);
            }
            catch (Exception ex)
            {
                this.Fail(ctx, ex.ToString());
            }

            ctx.Trc("SDKTests.SSLCertExpirationHealth.Verify complete");
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

            //remove all scripts
            RunCmd("rm -rf /tmp/*.sh");

            ctx.Trc("SDKTests.SSLCertExpirationHealth.Cleanup finished");
        }

        #endregion Test Framework Methods

        #region Private Methods

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
