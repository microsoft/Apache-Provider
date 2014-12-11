//-----------------------------------------------------------------------
// <copyright file="Credential.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>v-lizhou</author>
// <description>Credential class which needed when running tasks.</description>
// <history>2/28/2011 1:01:41 PM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Apache.SDK.ApacheSDKHelper
{
    using System.Xml;
    public class Credential
    {
        /// <summary>
        /// Initializes a new instance of the CredentialSet class.
        /// </summary>
        /// <param name="userId">SCX user's name</param>
        /// <param name="elevation">The value of elevation, such as "su" and "sudo", .etc</param>
        /// <param name="password">Password of scx userId</param>
        /// <param name="suPassword">Password of the su account</param>
        /// <param name="sshKey">Storage of Key contents as Base64 string</param>
        /// <param name="sshPassphrase">Password of SSH</param>
        public Credential(string userId, string elevation, string password, string suPassword = "", string sshKey = "", string sshPassphrase = "")
        {
            this.UserId = userId;
            this.Elev = elevation;
            this.Password = string.IsNullOrWhiteSpace(sshKey) ? password : sshPassphrase;
            this.SuPassword = suPassword;
            this.SSHKey = sshKey;
            this.SSHPassphrase = sshPassphrase;
        }

        /// <summary>
        /// Gets or sets the user name
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the elevation type, "su" or "sudo"
        /// </summary>
        public string Elev { get; set; }

        /// <summary>
        /// Gets or sets password of userId
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets supassword 
        /// </summary>
        public string SuPassword { get; set; }

        /// <summary>
        /// Gets or sets ssh key 
        /// </summary>
        public string SSHKey { get; set; }

        /// <summary>
        /// Gets or sets passphrase of ssh key
        /// </summary>
        public string SSHPassphrase { get; set; }

        /// <summary>
        /// Gets the user name in xml format
        /// </summary>
        public string XmlUserName
        {
            get
            { 
                return string.Format(@"<SCXUser><UserId>{0}</UserId><Elev>{1}</Elev></SCXUser>", this.UserId, this.Elev);
            }
        }

        /// <summary>
        /// Gets the password in xml format
        /// </summary>
        public string XmlPassword
        {
            get
            {
                var doc = new XmlDocument();
                doc.LoadXml("<SCXSecret></SCXSecret>");

                XmlNode passElem = doc.CreateNode(XmlNodeType.Element, "Password", null);
                passElem.InnerText = this.Password;

                doc.DocumentElement.AppendChild(passElem);

                XmlNode suPassElem = doc.CreateNode(XmlNodeType.Element, "SuPassword", null);
                if (this.SuPassword != null && this.SuPassword.Length > 0)
                {
                    suPassElem.InnerText = this.SuPassword;
                }

                doc.DocumentElement.AppendChild(suPassElem);

                XmlNode keyElem = doc.CreateNode(XmlNodeType.Element, "SSHKey", null);

                if (this.SSHKey != null && this.SSHKey.Length > 0)
                {
                    keyElem.InnerText = this.SSHKey;
                }

                doc.DocumentElement.AppendChild(keyElem);
                return doc.InnerXml;
            }
        }
    }
}