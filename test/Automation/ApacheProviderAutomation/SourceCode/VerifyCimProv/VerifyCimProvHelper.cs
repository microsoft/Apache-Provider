
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
    using System;
    using Infra.Frmwrk;
    using Scx.Test.Common;
    /// <summary>
    /// VerifyCimProvBase class. get HostName, UserName... and an Apchehelper instance.
    /// </summary>
    public class VerifyCimProvHelper
    {
        /// <summary>
        /// new a apacheHelper
        /// </summary>
        private ApacheHelper apacheHelper;

        /// <summary>
        /// new a apacheHelper
        /// </summary>
        public ApacheHelper ApacheHelper
        {
            get { return apacheHelper; }
            set { apacheHelper = value; }
        }

        /// <summary>
        /// Required: Name of Posix host (DNS or IP)
        /// </summary>
        private string hostName;

        /// <summary>
        /// Required: Name of Posix host (DNS or IP)
        /// </summary>
        public string HostName
        {
            get { return hostName; }
            set { hostName = value; }
        }

        /// <summary>
        /// Required: User name
        /// </summary>
        private string userName;

        /// <summary>
        /// Required: User name
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        /// <summary>
        /// Required: User's password
        /// </summary>
        private string password;

        /// <summary>
        ///  Required: User's password
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        /// <summary>
        /// Required: Command to install Apache
        /// </summary>
        private string installApacheCmd;

        /// <summary>
        /// Required: Command to install Apache
        /// </summary>
        public string InstallApacheCmd
        {
            get { return installApacheCmd; }
            set { installApacheCmd = value; }
        }

        /// <summary>
        /// Required: Command to remove Apache 
        /// </summary>
        private string uninstallApacheCmd;

        /// <summary>
        /// Required: Command to remove Apache 
        /// </summary>
        public string UninstallApacheCmd
        {
            get { return uninstallApacheCmd; }
            set { uninstallApacheCmd = value; }
        }

        /// <summary>
        /// Required: Command to clean apache:Uninstall and delete scx direcotries
        /// </summary>
        private string cleanupApacheCmd;

        /// <summary>
        /// Required: Command to clean apache:Uninstall and delete scx direcotries
        /// </summary>
        public string CleanupApacheCmd
        {
            get { return cleanupApacheCmd; }
            set { cleanupApacheCmd = value; }
        }

        /// <summary>
        /// Required: ApacheTag to identify the apacheinstalled platfrom.
        /// </summary>
        private string apacheTag;

        /// <summary>
        /// Required: ApacheTag to identify the apacheinstalled platfrom.
        /// </summary>
        public string ApacheTag
        {
            get { return apacheTag; }
            set { apacheTag = value; }
        }

        /// <summary>
        ///  Required: apache Location path on local machine
        /// </summary>
        private string apacheLocation;

        /// <summary>
        ///  Required: apache Location path on local machine
        /// </summary>
        public string ApacheLocation
        {
            get { return apacheLocation; }
            set { apacheLocation = value; }
        }

        /// <summary>
        /// get all the group values from ctx.
        /// </summary>
        /// <param name="ctx">IContext</param>
        public void GetValueFromVarList(IContext ctx)
        {
            // Overrid value from parent.
            this.HostName = ctx.ParentContext.Records.GetValue("hostname");
            if (String.IsNullOrEmpty(this.HostName))
            {
                throw new VarAbort("hostName not specified");
            }

            this.UserName = ctx.ParentContext.Records.GetValue("username");
            if (String.IsNullOrEmpty(this.UserName))
            {
                throw new VarAbort("userName not specified");
            }

            this.Password = ctx.ParentContext.Records.GetValue("password");
            if (String.IsNullOrEmpty(this.Password))
            {
                throw new VarAbort("password not specified");
            }

            this.InstallApacheCmd = ctx.ParentContext.Records.GetValue("installApacheCmd");
            if (String.IsNullOrEmpty(this.InstallApacheCmd))
            {
                throw new VarAbort("hostname not specified");
            }

            this.UninstallApacheCmd = ctx.ParentContext.Records.GetValue("UninstallApacheCmd");
            if (String.IsNullOrEmpty(this.UninstallApacheCmd))
            {
                throw new VarAbort("hostname not specified");
            }

            this.CleanupApacheCmd = ctx.ParentContext.Records.GetValue("CleanupApacheCmd");
            if (String.IsNullOrEmpty(this.CleanupApacheCmd))
            {
                throw new VarAbort("hostname not specified");
            }

            this.ApacheTag = ctx.ParentContext.Records.GetValue("ApacheTag");
            if (String.IsNullOrEmpty(this.ApacheTag))
            {
                throw new VarAbort("hostname not specified");
            }

            this.ApacheLocation = ctx.ParentContext.Records.GetValue("ApachesLocation");
            if (String.IsNullOrEmpty(this.ApacheLocation))
            {
                throw new VarAbort("ApachesLocation not specified");
            }
            apacheHelper = new ApacheHelper(ctx.Trc, this.hostName, this.userName, this.password, this.ApacheLocation, this.ApacheTag, true);
        }

        #region HelpMethod

        /// <summary>
        /// Verify Installation logs.
        /// </summary>
        /// <param name="commandStdOut">Installation command stdOut put</param>
        /// <param name="keyWorlds"> keyworlds</param>
        public void VerifyInstallLog(string commandStdOut, string keyWorlds)
        {
            if (!commandStdOut.ToUpper().Contains(keyWorlds.ToUpper()))
            {
                throw new VarAbort(string.Format("Verify installtion log contains {0} failed", keyWorlds));
            }
        }

        /// <summary>
        /// VerifyInstallation Folders, after install. the apache folder should put under folder /opt/microsoft/ and /etc/opt/microsoft/ and /var/opt/microsoft/
        /// </summary>
        /// <param name="verifyFolderExistCmd">verifyFolderExistCmd</param>
        /// <param name="expectFoldercount">expectFoldercount</param>
        /// <param name="isfolderExist">isfolderExist</param>
        public void VerifyInstallFolders(string verifyFolderExistCmd, string expectFoldercount, bool isfolderExist)
        {
            try
            {
                // verify after installed the apache-cimprov folder should place under folder /opt/microsoft/ and /etc/opt/microsoft/ and /var/opt/microsoft/
                RunPosixCmd runCmd = this.ApacheHelper.RunCmd(verifyFolderExistCmd);
                string stdout = runCmd.StdOut.Replace("\n", "");
                expectFoldercount = expectFoldercount.Replace(",", "");

                if (stdout != expectFoldercount)
                {
                    throw new VarAbort(string.Format("verify after installed the apache-cimprov folder under folder /opt/microsoft/ and /etc/opt/microsoft/ and /var/opt/microsoft/ count should be{0}  failed", expectFoldercount));
                }

            }
            catch
            {
                if (isfolderExist)
                {
                    throw new VarAbort("verify after installed the apache-cimprov folder should place under folder /opt/microsoft/ and /etc/opt/microsoft/ and /var/opt/microsoft/  failed");
                }
            }
        }

        /// <summary>
        /// VerifyApacheInstalled if installed verify the version like 1.0.0-271
        /// </summary>
        /// <param name="verifyApacheInstalledCmd">verifyApacheInstalledCmd</param>
        /// <param name="isInstalled">isInstalled</param>
        public void VerifyApacheInstalled(string verifyApacheInstalledCmd, bool isInstalled)
        {
            try
            {
                // verify the apache pakage version.
                // file name like apache-cimprov-1.0.0-271.universal.1.i686.sh.
                // version is 1.0.0-271.
                // get expected version number:
                string[] nameParts = this.ApacheHelper.apacheAgentName.Split('-');
                string versionNumber = nameParts[2] + '-' + nameParts[3].Split('.')[0];

                // get acutally version number using cmd.
                string commandStdOut = this.ApacheHelper.RunCmd(verifyApacheInstalledCmd).StdOut;
                if (!commandStdOut.Contains(versionNumber))
                {
                    throw new VarAbort("verify the apache version failed");
                }
            }
            catch
            {
                if (isInstalled)
                {
                    throw new VarAbort("running verifyApacheInstalledCmd failed");
                }
            }
        }

        #endregion HelpMethod
    }
}
