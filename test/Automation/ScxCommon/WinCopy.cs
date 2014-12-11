//-----------------------------------------------------------------------
// <copyright file="WinCopy.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>a-brmill</author>
// <description></description>
// <history>1/20/2009 1:15:59 PM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Common
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Description for PosixCopy.
    /// </summary>
    public class WinCopy : IFileCopyHelper
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the WinCopy class for MCF usage.
        /// </summary>
        public WinCopy()
        {
        }
       
        /// <summary>
        /// Initializes a new instance of the WinCopy class.
        /// </summary>
        /// <param name="hostName">This is the hostname of the machine</param>
        /// <param name="userName">This is the username</param>
        /// <param name="password">This is the count password</param>
        public WinCopy(string hostName, string userName, string password)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                throw new ArgumentException("hostName is null or empty");
            }

            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("userName is null or empty");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("password is null or empty");
            }

            this.HostName = hostName;
            this.UserName = userName;
            this.Password = password;
        }

        #endregion Constructors

        #region protected Fields

        /// <summary>
        /// Gets or sets hostname.
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Gets or sets UserName.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets Password.
        /// </summary>
        public string Password { get; set; }

        #endregion protected Fields

        #region Methods

        #region Public Methods

        /// <summary>
        /// Interface method to copy files from a windows host
        /// </summary>
        /// <param name="sourcePath">Path for source file(s)</param>
        /// <param name="destPath">Destination path</param>
        public void CopyFrom(string sourcePath, string destPath)
        {
            if (String.IsNullOrEmpty(sourcePath))
            {
                throw new ArgumentException("sourcePath is null or empty");
            }

            if (String.IsNullOrEmpty(destPath))
            {
                throw new ArgumentException("destPath is null or empty");
            }

            File.Copy(sourcePath, destPath, true);
        }

        /// <summary>
        /// Interface method to copy files to a windows host
        /// </summary>
        /// <param name="sourcePath">Path for source file(s)</param>
        /// <param name="destPath">Destination path</param>
        public void CopyTo(string sourcePath, string destPath)
        {
            if (String.IsNullOrEmpty(sourcePath))
            {
                throw new ArgumentException("sourcePath is null or empty");
            }

            if (String.IsNullOrEmpty(destPath))
            {
                throw new ArgumentException("destPath is null or empty");
            }

            File.Copy(sourcePath, destPath, true);
        }

        #endregion Public Methods

        #endregion Methods

        #region IFileCopyHelper Members

        #endregion
    }
}
