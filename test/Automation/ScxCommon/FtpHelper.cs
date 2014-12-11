//-----------------------------------------------------------------------
// <copyright file="FtpHelper.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>sunilmu</author>
// <description></description>
// <history>1/12/2009 8:47:39 AM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Commom
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Tamir.SharpSsh;
    using Infra.Frmwrk;
    using System.IO;

    /// <summary>
    /// Ftp utility class.
    /// </summary>
    public class FtpHelper
    {
        #region Private Fields

        /// <summary>Username to use while connecting to the system.</summary>
        private string username;

        /// <summary>Password to use while connecting to the system.</summary>
        private string password;

        /// <summary>System to connect to.</summary>
        private string system;

        /// <summary>MCF context object.</summary>
        private IContext context;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Constructor for the FtpHelper class.
        /// </summary>
        /// <param name="ctx">Context object.</param>
        /// <param name="sys">sys.</param>
        /// <param name="usr">usr.</param>
        /// <param name="pwd">pwd.</param>
        public FtpHelper(IContext ctx, string sys, string usr, string pwd)
        {
            this.context = ctx;
            this.username = usr;
            this.password = pwd;
            this.system = sys;
        }

        /// <summary>
        /// Constructor for the FtpHelper class.
        /// </summary>
        /// <param name="ctx">ctx.</param>
        public FtpHelper(IContext ctx)
        {
            this.context = ctx;
            GroupReader groupReader = ctx.ParentContext.UserObject as GroupReader;
            if (null != groupReader)
            {
                this.username = groupReader.Username;
                this.password = groupReader.Password;
                this.system = groupReader.System;
            }
        }

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Transfers single file to the unix box.
        /// </summary>
        /// <param name="sourceFile">File path to copy from.</param>
        /// <param name="destinationFile">File path to copy to.</param>
        public void TransferFile(string sourceFile, string destinationFile)
        {
            this.CopySingleFile(sourceFile, destinationFile);
        }

        /// <summary>
        /// Transfers multiple files to unix box.
        /// </summary>
        /// <param name="sourceDirectory">Directory containing the files.</param>
        /// <param name="destinationDirectory">Destination directory name.</param>
        /// <remarks>The source and the destination folder paths. This method will copy each file retaining the original name.</remarks>
        public void TransferMultipleFiles(string sourceDirectory, string destinationDirectory)
        {
            DirectoryInfo di = new DirectoryInfo(sourceDirectory);
            FileInfo[] filesList = di.GetFiles();
            foreach (FileInfo fi in filesList)
            {
                this.CopySingleFile(fi.FullName, Path.Combine(destinationDirectory, fi.Name));
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Method responsible for the actual transfer.
        /// </summary>
        /// <param name="sourceFile">Source file.</param>
        /// <param name="destinationFile">Destination file.</param>
        private void CopySingleFile(string sourceFile, string destinationFile)
        {
            string stage = string.Empty;
            try
            {
                stage = "Initializing Sftp object";
                Sftp sftp = new Sftp(this.system, this.username, this.password);
                stage = "Connecting to " + this.system;
                sftp.Connect();
                stage = "Copying files";
                sftp.Put(sourceFile, destinationFile);
                stage = "Closing connection";
                if (sftp.Connected)
                {
                    sftp.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ftp action failed for {0} during: {1}.", sourceFile, stage), ex);
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}
