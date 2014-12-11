//-----------------------------------------------------------------------
// <copyright file="GroupReader.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>sunilmu</author>
// <description>Utility for group set up.</description>
// <history>11/25/2008 8:47:39 AM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Common
{
    using System;
    using Infra.Frmwrk;

    /// <summary>
    /// Function unknown, see Sunil
    /// </summary>
    public class GroupReader : ISetup
    {
        #region ISetup Members

        /// <summary>
        /// Administrator name
        /// </summary>
        private string scxadmin = string.Empty;

        /// <summary>
        /// Host name (filling in a minimum of 10 characters)
        /// </summary>
        private string hostname = string.Empty;

        /// <summary>
        /// User name (filling in a minimum of 10 characters)
        /// </summary>
        private string username = string.Empty;

        /// <summary>
        /// Password (filling in a minimum of 10 characters)
        /// </summary>
        private string password = string.Empty;

        /// <summary>
        /// System name? (filling in a minimum of 10 characters)
        /// </summary>
        private string system = string.Empty;

        /// <summary>
        /// Gets Default summary goes here
        /// </summary>
        public string ScxAdmin
        {
            get { return this.scxadmin; }
        }

        /// <summary>
        /// Gets Default summary goes here
        /// </summary>
        public string HostName
        {
            get { return this.hostname; }
        }

        /// <summary>
        /// Gets Default summary goes here
        /// </summary>
        public string UserName
        {
            get { return this.username;  }
        }

        /// <summary>
        /// Gets Default summary goes here
        /// </summary>
        public string Password
        {
            get { return this.password; }
        }

        /// <summary>
        /// Gets Default summary goes here
        /// </summary>
        public string System
        {
            get { return this.system; }
        }

        /// <summary>
        /// Default summary for Setup goes here
        /// </summary>
        /// <param name="ctx">MCF context</param>
        public void Setup(IContext ctx)
        {
            this.scxadmin = ctx.Records.GetValue("scxadmin");
            this.hostname = ctx.Records.GetValue("hostname");
            this.username = ctx.Records.GetValue("username");
            this.password = ctx.Records.GetValue("password");
        }

        #endregion
    }
}
