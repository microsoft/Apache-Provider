//-----------------------------------------------------------------------
// <copyright file="SSHKeyHelper.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>v-roliu</author>
// <description></description>
//-----------------------------------------------------------------------

namespace Scx.Test.Common
{
    using System;
    using System.IO;
    using Scx.Test.Common;

    /// <summary>
    /// Initializes a new instance of the SSHKeyHelper class for preparing the ssh key test environment.
    /// </summary>
    public class SSHKeyHelper
    {
        #region Private Filed

        /// <summary>
        /// The account for remote network connect
        /// </summary>
        private const string ConnectionAccount = "root";

        /// <summary>
        /// Rhe password for remote network connect
        /// </summary>
        private const string ConnectionPassword = "OpsMgr2007R2";

        /// <summary>
        /// The name of the register file on remote UNIX/Linux machine.
        /// </summary>
        private const string RemoteRegisterFile = "authorized_keys";

        /// <summary>
        /// The name of the sshkeyPassphraseFile
        /// </summary>
        private const string SSHKeyPassphraseFile = "SSHkeyPassphrase.txt";

        /// <summary>
        /// The location of the basic ssh key file 
        /// </summary>
        private string sourPath = null;

        /// <summary>
        /// The location of the specific ssh key file
        /// </summary>
        private string destPath = null; 

        /// <summary>
        /// The basic ssh key name
        /// </summary>
        private string basicKey = null;
 
        /// <summary>
        /// The hostname of UNIX/LInux client.
        /// </summary>
        private string hostname = null;

        /// <summary>
        /// The account to logon UNIX/Linux machine. such as root or scxuser
        /// </summary>
        private string logonAccount = null;

        /// <summary>
        /// The ciphertext in both basic private ssh key and public key
        /// </summary>
        private string ciphertext = string.Empty;

        /// <summary>
        /// The name of specific ssh key file against each platform
        /// </summary>
        private string specificKey = string.Empty;

        /// <summary>
        /// The location of the new created specific ssh key 
        /// </summary>
        private string specificKeyLocation;

        /// <summary>
        /// The path of the register folder on remote UNIX/Linux machine.
        /// </summary>
        private string remoteRegisterFolderPath = null;

        /// <summary>
        /// The full path of the register file on remote UNIX/Linux machiine
        /// </summary>
        private string fullRegisterPath;

        /// <summary>
        /// Class RunPosixCmd
        /// </summary>
        private RunPosixCmd executeCmd;

        /// <summary>
        /// Log Delegate to allow writing using a log mechanism provided by the user.
        /// </summary>
        private LogDelegate logger = NullLogDelegate;

        #endregion

        /// <summary>
        /// Initializes a new instance of the SSHKeyHelper class for preparing ssh key test environment
        /// </summary>
        /// <param name="sourPath">The location of the basic ssh key file</param>
        /// <param name="destPath">The location of the new created specific ssh key file </param>
        /// <param name="basicKey">The basic ssh key name</param>
        /// <param name="hostname">The hostname of UNIX/LInux client.</param>
        /// <param name="remoteRegisterFolderPath">The path of the register folder on remote UNIX/Linux machine.</param>
        /// <param name="logonAccount">This is the logonAccount</param>
        /// <param name="logDlg">The MCF interface</param>
        public SSHKeyHelper(string sourPath, string destPath, string basicKey, string hostname, string remoteRegisterFolderPath, string logonAccount, LogDelegate logDlg)
        {
            this.sourPath = sourPath;
            this.destPath = destPath;
            this.basicKey = basicKey;
            this.hostname = hostname;
            this.remoteRegisterFolderPath = remoteRegisterFolderPath;
            this.logonAccount = logonAccount;
            this.specificKey = hostname + ".pub";
            this.logger = logDlg;
        }

        #region Delegates

        /// <summary>
        /// Delegate allowing output to the log file without having to accept an external class instance such as IContext.
        /// </summary>
        /// <param name="logMsg">Log message</param>
        public delegate void LogDelegate(string logMsg);

        #endregion

        #region Static Methods

        /// <summary>
        /// Trivial log delegate function which does nothing.
        /// </summary>
        /// <param name="logMsg">Log message</param>
        public static void NullLogDelegate(string logMsg)
        {
            // do nothing
        }

        #endregion

        #region Static Method

        /// <summary>
        /// public method for get ssh key password
        /// </summary>
        /// <param name="sshKeyFolder">The folder of SSHKey</param>>
        /// <returns>SSH Key password</returns>
        public static string GetSSHKeyPassphrase(string sshKeyFolder)
        {
            string sshKeypassphrase = string.Empty;

            string keyFile = Path.Combine(sshKeyFolder, SSHKeyPassphraseFile);
            if (!File.Exists(keyFile))
            {
                throw new Exception("No ssh key passphrase file exist.");
            }

            using (StreamReader sr = File.OpenText(keyFile))
            {
                sshKeypassphrase = sr.ReadLine().Trim();
                sr.Close();
            }

            return sshKeypassphrase;
        }

        #endregion

        #region Public Method

        /// <summary>
        /// public method for ssh key testenvironment prepare
        /// </summary>
        public void PrepareTestEnvironment()
        {
            try
            {
                this.fullRegisterPath = this.remoteRegisterFolderPath + "/" + RemoteRegisterFile;

                this.executeCmd = new RunPosixCmd(this.hostname, ConnectionAccount, ConnectionPassword);

                this.logger(string.Format("Start to create specific SSHKey {0} based on {1} ", this.specificKey, this.basicKey));
                this.specificKeyLocation = this.CreateSpecificSSHKey();

                this.logger(string.Format("Verify the ssh key {0} is registered on client.", this.specificKey));
                if (!this.IsSSHKeyRegistered())
                {
                    this.logger(string.Format("The SSH key {0} has not been registered.", this.specificKey));
                    this.CopySSHkeyToClient();

                    // Check if the key register path exists. If not create it.
                    this.CheckKeyRegisterFile();

                    // Check if the group setting is correct.
                    this.CheckGroupSetting();

                    if (!this.IsSSHKeyRegistered())
                    {
                        this.logger("Registering...");
                        this.RegisterPublicSSHKey();

                        this.logger("Verify the register action is successful.");
                        if (this.IsSSHKeyRegistered())
                        {
                            this.logger("Register successful.");
                        }
                    }
                    else
                    {
                        this.logger(string.Format("The ssh key has already been registered on client {0}.", this.hostname));
                    }
                }
                else
                {
                    this.logger("verify the group settings for the registered ssh key");
                    this.CheckGroupSetting();
                }
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Fail to configure ssh key on client {0} -- " + e.Message, this.hostname)); 
            }
        }
        #endregion 

        #region Private Method

        /// <summary>
        /// Method to create specific ssh key file
        /// </summary>
        /// <returns>The new created specific ssh key name</returns>
        private string CreateSpecificSSHKey()
        {
            string sourFile = Path.Combine(this.sourPath, this.basicKey);
            string destFile = Path.Combine(this.destPath, this.specificKey);
            if (!File.Exists(sourFile))
            {
                throw new Exception("No ssh key file exist.");
            }

            if (!Directory.Exists(this.destPath))
            {
                Directory.CreateDirectory(this.destPath);
            }

            if (File.Exists(destFile))
            {
                File.Delete(destFile);
            }

            using (StreamReader sr = File.OpenText(sourFile))
            {
                this.ciphertext = sr.ReadLine() + " " + this.logonAccount + "@" + this.hostname + '\n';
                this.logger(string.Format("Create the specific ciphertext based on the basic ssh key : \n'{0}'\n", this.ciphertext));
                sr.Close();
            }

            this.logger(string.Format("Writing specific ssh key to {0}", destFile));
            using (StreamWriter sw = File.CreateText(destFile))
            {
                sw.Write(this.ciphertext);
                sw.Flush();
                sw.Close();
            }

            return destFile;
        }

        /// <summary>
        /// Copy new created ssh key file to remote UNIX/Linux client
        /// </summary>
        private void CopySSHkeyToClient()
        {
            if (!File.Exists(this.specificKeyLocation))
            {
                throw new Exception(string.Format("The SSH key for {0} does not exist.", this.hostname));
            }

            PosixCopy copyKeyToHost = new PosixCopy(this.hostname, ConnectionAccount, ConnectionPassword);
            this.logger(string.Format("Copy the specific ssh key file to path /tmp/{0}", this.specificKey));
            copyKeyToHost.CopyTo(this.specificKeyLocation, "/tmp/" + this.specificKey);
        }

        /// <summary>
        /// Verify if the the key register path exists. If not, create it
        /// </summary>
        private void CheckKeyRegisterFile()
        {
            try
            {
                this.executeCmd.RunCmd(string.Format("ls {0}", this.fullRegisterPath));
                this.logger(string.Format("The register path {0} has been created before.", this.fullRegisterPath));
            }
            catch (Exception)
            {
                this.logger(string.Format("The register path {0} has not been created. Creating now...", this.fullRegisterPath));
                string[] paths = this.remoteRegisterFolderPath.Split('/');
                string cachePath = string.Empty;
                for (int i = 1; i < paths.Length; i++)
                {
                    cachePath = cachePath + "/" + paths[i];
                    this.CreateRegisterFolder(cachePath);
                }

                // Create ssh key register file
                this.CreateRegisterFile();
            }
        }

        /// <summary>
        /// Verify the permission setting is set correctly. If not, set it.
        /// </summary>
        private void CheckPermissionSettings()
        {
            this.logger("Verifying if permition setting is correct.");

            this.executeCmd.RunCmd(string.Format("ls -al {0}", this.fullRegisterPath));
            string result = this.executeCmd.StdOut;
            if (!result.Contains("-wx-------"))
            {
                this.executeCmd.RunCmd(string.Format("chmod 600 {0}", this.fullRegisterPath));
            }

            this.executeCmd.RunCmd(string.Format("ls -ld {0}", this.remoteRegisterFolderPath));
            result = this.executeCmd.StdOut;
            if (!result.Contains("dwxr------"))
            {
                this.executeCmd.RunCmd(string.Format("chmod 700 {0}", this.remoteRegisterFolderPath));
            }
        }

        /// <summary>
        /// Create register folder one by one
        /// </summary>
        /// <param name="folderPath">The folder path</param>
        private void CreateRegisterFolder(string folderPath)
        {
            try
            {
                this.executeCmd.RunCmd(string.Format("ls -ld {0}", folderPath));
                this.logger(string.Format("Folder {0} has been created.", folderPath));
            }
            catch (Exception)
            {
                this.logger(string.Format("Folder {0} is created.", folderPath));
                this.executeCmd.RunCmd(string.Format("mkdir {0}", folderPath));
            }
        }

        /// <summary>
        /// Create register file under register path
        /// </summary>
        private void CreateRegisterFile()
        {
            this.logger(string.Format("Creating file {0}", this.fullRegisterPath));
            this.executeCmd.RunCmd(string.Format("touch {0}", this.fullRegisterPath));

            this.CheckPermissionSettings();
        }

        /// <summary>
        /// Register the public ssh key for remote UNIX/Linux client
        /// </summary>
        private void RegisterPublicSSHKey()
        {
            string tmpPath = "/tmp/" + this.hostname + ".pub";
            this.logger(string.Format("Register the ssh key {0} with command : 'cat {1} >> {2}'", this.specificKey, tmpPath, this.fullRegisterPath));
            this.executeCmd.RunCmd(string.Format("cat {0} >> {1}", tmpPath, this.fullRegisterPath));
        }

        /// <summary>
        /// Verify the group setting of the register path and file
        /// </summary>
        private void CheckGroupSetting()
        {
            this.logger("Verifying if group setting is correct...");

            this.executeCmd.RunCmd(string.Format("groups {0}", this.logonAccount));
            this.logger(string.Format("The currect group of {0} is \'{1}\'", this.logonAccount, this.executeCmd.StdOut));

            string[] groupName = this.executeCmd.StdOut.Split(new char[] { ' ', '\r', '\n' });
            string groupsName = groupName.Length > 2 ? groupName[2] : groupName[0];
            this.logger(string.Format("The currect group name is {0}", groupName));

            this.executeCmd.RunCmd(string.Format("ls -al {0}", this.fullRegisterPath));
            this.logger(string.Format("The currect group settings of the file '{0}' is \'{1}\'", this.fullRegisterPath, this.executeCmd.StdOut));

            string[] groupInfo = this.executeCmd.StdOut.Split(new char[] { ' ', '\r', '\n' });
            try
            {
                this.logger(string.Format("Verify the account \'{0}\' and group \'{1}\' has been correctly setting.", this.logonAccount, groupsName));
                if (!(groupInfo[2].Equals(this.logonAccount) && groupInfo[3].Equals(groupsName)))
                {
                    this.logger(string.Format("The group of the register file has not been set. Setting now with command : 'chown -R {0}:{1} {2}'", this.logonAccount, groupsName, this.remoteRegisterFolderPath));
                    this.executeCmd.RunCmd(string.Format("chown -R {0}:{1} {2}", this.logonAccount, groupsName, this.remoteRegisterFolderPath));
                }
                else
                {
                    this.logger("The setting of the group is healthy.");
                }
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Error when change owner to current account -- " + e.Message), e);
            }
        }

        /// <summary>
        /// Verify the ssh key is register or not
        /// </summary>
        /// <returns>The value or isRegistered</returns>
        private bool IsSSHKeyRegistered()
        {
            bool isRegistered = false;
            try
            {
                this.executeCmd.RunCmd(string.Format("cat {0}", this.fullRegisterPath));
                if (this.executeCmd.StdOut.Contains(this.ciphertext))
                {
                    isRegistered = true;
                }
            }
            catch (Exception)
            {
            }

            return isRegistered;
        }
        #endregion
    }
}
