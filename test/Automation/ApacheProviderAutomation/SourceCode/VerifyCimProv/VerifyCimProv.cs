//-----------------------------------------------------------------------
// <copyright file="VerifyCimProv.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>v-jeali</author>
// <description></description>
// <history>11/28/2014 10:29:44 PM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Apache.Provider.VerifyCimProv
{
    using Infra.Frmwrk;
    using System;

    ///// <summary>
    ///// VerifyCimProv Installation
    ///// </summary>
    public class VerifyCimProv : VerifyCimProvHelper, ISetup, IRun, IVerify, ICleanup
    {
        string commandStdOut = string.Empty;
        string verifyFolderExistCmd = string.Empty;
        string verifyApacheInstalledCmd = string.Empty;
        string apacheCmd = string.Empty;
        string expectedFolderCount = string.Empty;
        string installLogKeyWorlds = string.Empty;
        bool isInValidInstall = false;

        /// <summary>
        /// Case setup. Get values from case suite. and Uninstall ApacheAgent.
        /// </summary>
        /// <param name="ctx"></param>
        public void Setup(IContext ctx)
        {
            // get value from base class.
            this.GetValueFromVarList(ctx);

            // get value from current case.
            apacheCmd = ctx.Records.GetValue("apacheCmd");
            if (string.IsNullOrEmpty(this.apacheCmd))
            {
                throw new VarAbort("installCmd command not specified");
            }

            if (ctx.Records.HasKey("isInvalidInstall") &&
                   ctx.Records.GetValue("isInvalidInstall") == "true")
            {
                this.isInValidInstall = true;
            }

            if (ctx.Records.HasKey("installLogKeyWorlds"))
            {
                this.installLogKeyWorlds = ctx.Records.GetValue("installLogKeyWorlds");
            }

            if (ctx.Records.HasKey("expectedFolderCount"))
            {
                this.expectedFolderCount = ctx.Records.GetValue("expectedFolderCount");
            }

            if (ctx.Records.HasKey("verifyFolderExistCmd"))
            {
                verifyFolderExistCmd = ctx.Records.GetValue("verifyFolderExistCmd");
            }

            if (ctx.Records.HasKey("verifyApacheInstalledCmd"))
            {
                verifyApacheInstalledCmd = ctx.Records.GetValue("verifyApacheInstalledCmd");
            }

            // if has to install twice don't need uninstall.
            if (!ctx.Records.HasKey("installTwice") && !ctx.Records.HasKey("isHelpOption") && !ctx.Records.HasKey("isRemoveOption"))
            {
                // Uninstall Apache Agent. 
                this.ApacheHelper.UninstallApacheAgent(this.UninstallApacheCmd);
            }

        }

        /// <summary>
        /// Running Case. install apache using command 
        /// </summary>
        /// <param name="ctx">ctx</param>
        public void Run(IContext ctx)
        {
            try
            {
                commandStdOut = this.ApacheHelper.RunCmd(string.Format(this.apacheCmd, this.ApacheHelper.apacheAgentName)).StdOut;
            }
            catch (Exception e)
            {
                if (!this.isInValidInstall)
                {
                    throw new Exception("Install apache CimProv agent failed: " + e.Message);
                }

            }
        }

        /// <summary>
        /// Verify the apache agent install successfully.
        /// </summary>
        /// <param name="ctx">ctx</param>
        public void Verify(IContext ctx)
        {
            if (!this.isInValidInstall && !ctx.Records.HasKey("isHelpOption") && !ctx.Records.HasKey("isRemoveOption"))
            {
                //this.VerifyInstallLog(commandStdOut, this.installLogKeyWorlds, true);

                this.VerifyInstallFolders(verifyFolderExistCmd, this.expectedFolderCount, true);

                this.VerifyApacheInstalled(verifyApacheInstalledCmd, true);
            }

            if (ctx.Records.HasKey("isHelpOption"))
            {
                this.VerifyInstallLog(commandStdOut, this.installLogKeyWorlds, true);
            }

            if (ctx.Records.HasKey("isRemoveOption"))
            {
                this.VerifyInstallFolders(verifyFolderExistCmd, this.expectedFolderCount, false);

                this.VerifyApacheInstalled(verifyApacheInstalledCmd, false);
            }
        }


        /// <summary>
        /// Cleanup
        /// </summary>
        /// <param name="ctx"></param>
        public void Cleanup(IContext ctx)
        {
            // the uninstall will be down via group clean up.
        }
    }
}
