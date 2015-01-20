//-----------------------------------------------------------------------
// <copyright file="VerifyCimProvCompatibility.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>v-jeali</author>
// <description></description>
// <history>12/8/2014 10:29:44 PM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Apache.Provider.VerifyCimProv
{
    using Infra.Frmwrk;
    using Scx.Test.Common;
    using System;
    using System.IO;

    /// <summary>
    /// VerifyCimProvCompatibility
    /// </summary>
    class VerifyCimProvCompatibility : VerifyCimProvHelper, ISetup, IRun, IVerify, ICleanup
    {
        string apacheCmd = string.Empty;
        string installOlderApacheCmd = string.Empty;
        bool isUpgrade = false;
        string apacheOlderAgentFullName = string.Empty;
        string apacheAgentFullName = string.Empty;
        string commandStdOut = string.Empty;
        string expectedFolderCount = string.Empty;
        string verifyFolderExistCmd = string.Empty;
        string verifyApacheInstalledCmd = string.Empty;

        /// <summary>
        /// setup
        /// </summary>
        /// <param name="ctx">setup</param>
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

            if (ctx.Records.HasKey("installOlderApacheCmd"))
            {
                installOlderApacheCmd = ctx.Records.GetValue("installOlderApacheCmd");
            }

            if (ctx.Records.HasKey("isUpgrade") && ctx.Records.GetValue("isUpgrade").ToUpper() == "TRUE")
            {
                this.isUpgrade = true;
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

            string apachelocation = ctx.ParentContext.Records.GetValue("ApachesLocation");
            string oldApachelocation = ctx.Records.GetValue("oldApachesLocation");
            this.apacheOlderAgentFullName = SearchForApacheInApcheAgentPath(oldApachelocation, this.ApacheTag);
            this.apacheAgentFullName = SearchForApacheInApcheAgentPath(apachelocation, this.ApacheTag);
            this.CopyApacheAgent(this.apacheAgentFullName, "/tmp/");
            this.CopyApacheAgent(this.apacheOlderAgentFullName, "/tmp/");

            // install older apache version.
            if (this.isUpgrade)
            {
                try
                {
                    // Uninstall Apache Agent. for upgrade test.
                    this.ApacheHelper.UninstallApacheAgent(this.UninstallApacheCmd);
                    this.ApacheHelper.RunCmd(string.Format(this.installOlderApacheCmd, Path.GetFileName(this.apacheOlderAgentFullName)));
                }
                catch (Exception e)
                {
                    throw new Exception("install older apache CimProv agent failed: " + e.Message);
                }
            }
        }

        /// <summary>
        /// Running case
        /// </summary>
        /// <param name="ctx">ctx</param>
        public void Run(IContext ctx)
        {
            try
            {
                if (isUpgrade)
                {
                    commandStdOut = this.ApacheHelper.RunCmd(string.Format(this.apacheCmd, this.ApacheHelper.apacheAgentName)).StdOut;
                }
                else
                {
                    this.ApacheHelper.RunCmd(string.Format(this.apacheCmd, Path.GetFileName(this.apacheOlderAgentFullName)));
                }
            }
            catch (Exception e)
            {
                if (isUpgrade)
                {
                    throw new Exception("Upgrade apache CimProv agent failed: " + e.Message);
                }
            }

        }

        /// <summary>
        /// Verify 
        /// </summary>
        /// <param name="ctx">ctx</param>
        public void Verify(IContext ctx)
        {
            if (isUpgrade)
            {
                // verify command stdout:
                VerifyInstallLog(commandStdOut, "FAILED", false);
                this.VerifyInstallFolders(verifyFolderExistCmd, this.expectedFolderCount, true);

                this.VerifyApacheInstalled(verifyApacheInstalledCmd, true);
            }
        }

        /// <summary>
        /// Cleanup
        /// </summary>
        /// <param name="ctx">ctx</param>
        public void Cleanup(IContext ctx)
        {
            if (isUpgrade)
            {
                this.ApacheHelper.UninstallApacheAgent(this.UninstallApacheCmd);
            }
            else
            {
                this.ApacheHelper.RunCmd(string.Format("sh /tmp/{0} --purge", Path.GetFileName(this.apacheOlderAgentFullName)));
            }

            this.ApacheHelper.RunCmd(string.Format(this.apacheCmd, this.ApacheHelper.apacheAgentName));
        }

        /// <summary>
        /// CopyApacheAgent
        /// </summary>
        /// <param name="apacheAgentFullName">apacheAgentFullName</param>
        /// <param name="targetHostAgentloaclPath">targetHostAgentloaclPath</param>
        public void CopyApacheAgent(string apacheAgentFullName, string targetHostAgentloaclPath)
        {
            CopyFileToHost(apacheAgentFullName, targetHostAgentloaclPath + Path.GetFileName(apacheAgentFullName));
        }

        /// <summary>
        /// SearchForApacheInApcheAgentPath
        /// </summary>
        /// <param name="apcheAgentFullloaclPath">apcheAgentFullloaclPath</param>
        /// <param name="apcheTag">apcheTag</param>
        /// <returns>apcheAgentFullNamePath</returns>
        private static string SearchForApacheInApcheAgentPath(string apcheAgentFullloaclPath, string apcheTag)
        {
            //"Searching for apache in " + apcheAgentFullPath;
            DirectoryInfo di = new DirectoryInfo(apcheAgentFullloaclPath);
            FileInfo[] fi = di.GetFiles("*" + apcheTag + "*");
            if (fi.Length == 0)
            {
                throw new Exception("Found no apache installer matching ApacheTag: " + apcheTag);
            }

            if (fi.Length > 1)
            {
                throw new Exception("Found more than one apache installer matching ApacheTag: " + apcheTag);
            }

            // User-specified Apache path takes precedent
            string apacheAgentName = fi[0].FullName;
            return apacheAgentName;
        }

        /// <summary>
        /// CopyFileToHost
        /// </summary>
        /// <param name="from">from</param>
        /// <param name="to">to</param>
        private void CopyFileToHost(string from, string to)
        {

            PosixCopy copyToHost = new PosixCopy(this.HostName, this.UserName, this.Password);
            // Copy from server to Posix host
            copyToHost.CopyTo(from, to);
        }
    }
}
