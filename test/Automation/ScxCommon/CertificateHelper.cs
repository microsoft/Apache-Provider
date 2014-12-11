//-----------------------------------------------------------------------
// <copyright file="CertificateHelper.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>a-brdust</author>
// <description></description>
// <history>1/20/2009 3:50:59 PM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Common
{
    using System;
    using System.IO;
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// Retrieve certificate information about a monitored system.
    /// </summary>
    public class CertificateHelper
    {
        #region Private Fields

        /// <summary>
        /// host name of the remote system
        /// </summary>
        private string hostName;

        /// <summary>
        /// privileged user name on the remote system
        /// </summary>
        private string userName;

        /// <summary>
        /// password of the privileged user on the remote system
        /// </summary>
        private string password;

        /// <summary>
        /// Helper class to run POSIX commands on the remote system
        /// </summary>
        private RunPosixCmd posixCmd;

        /// <summary>
        /// Helper class to copy files between the local and remote systems
        /// </summary>
        private PosixCopy posixCpy;

        /// <summary>
        /// path to the local certificate cache
        /// </summary>
        private string certCache = "certCache";

        /// <summary>
        /// path to the client certificate cache
        /// </summary>
        private string clientCertCache = "/etc/opt/microsoft/scx/ssl/";

        /// <summary>
        /// Logger delegate method
        /// </summary>
        private ScxLogDelegate logger = ScxMethods.ScxNullLogDelegate;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the CertificateHelper class.
        /// </summary>
        /// <param name="logger">Log delegate method</param>
        /// <param name="hostName">Name of Posix host</param>
        /// <param name="userName">Valid user on Posix host</param>
        /// <param name="password">Password for user</param>
        public CertificateHelper(ScxLogDelegate logger, string hostName, string userName, string password)
        {
            this.logger = logger;
            this.hostName = hostName;
            this.userName = userName;
            this.password = password;

            this.certCache = Path.Combine(System.Environment.CurrentDirectory, this.certCache);
            this.logger("certCache set to: " + this.certCache);

            if (!Directory.Exists(this.certCache))
            {
                Directory.CreateDirectory(this.certCache);
            }

            this.posixCmd = new RunPosixCmd(hostName, userName, password);

            this.posixCpy = new PosixCopy(hostName, userName, password);
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        /// <summary>
        /// Retrieve certificate information object from the SSL certificate chain
        /// </summary>
        /// <param name="sender">An object that contains state information for this validation</param>
        /// <param name="certificate">The certificate used to authenticate the remote party</param>
        /// <param name="chain">The chain of certificate authorities associated with the remote certificate</param>
        /// <param name="sslPolicyErrors">One or more errors associated with the remote certificate</param>
        /// <returns>A Boolean value that determines whether the specified certificate is accepted for authentication</returns>
        public bool CertificateCheckCallback(
            object sender,
            System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            System.Security.Cryptography.X509Certificates.X509Chain chain,
            System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            this.logger("CheckSslCert sslPolicyErrors=" +
                Enum.GetName(typeof(System.Net.Security.SslPolicyErrors), sslPolicyErrors));
            this.logger("CheckSslCert Issuer =" + certificate.Issuer.ToString());
            this.logger("CheckSslCert Subject=" + certificate.Subject.ToString());

            if (certificate.Issuer.ToString().Contains(System.Environment.MachineName))
            {
                this.logger("CheckSslCert: Issuer is the host machine");
                return true;
            }
            else
            {
                this.logger("CheckSslCert: Issuer is NOT the host machine");
                return false;
            }
        }

        /// <summary>
        /// Check the signing authority of a ceritificate
        /// </summary>
        /// <returns>The imported certificate from the remote host</returns>
        public X509Certificate RetrieveCertificate()
        {
            string certFileName = "scx.pem";
            string certFileLocalPath = Path.Combine(this.certCache, certFileName);
            string certFileRemotePath = string.Concat(this.clientCertCache, "/", certFileName);

            this.logger(string.Format("Copy certificate from {0}:{1} to {2} on local system", this.hostName, certFileRemotePath, certFileLocalPath));
            this.posixCpy.CopyFrom(certFileRemotePath, certFileLocalPath);

            X509Certificate cert = new X509Certificate();

            cert.Import(certFileLocalPath);

            this.logger(string.Format("Imported cert: issuer={0}, subject={1}", cert.Issuer.ToString(), cert.Issuer.ToString()));

            return cert;
        }

        /// <summary>
        /// Check a certificate via SSL connection to Posix machine
        /// </summary>
        /// <param name="ipaddr">IP address of the client machine</param>
        public void CheckSslCert(string ipaddr)
        {
            System.Net.Sockets.TcpClient client = null;
            System.Net.Security.SslStream sslStream = null;

            // Set up ServicePointManager for program control of certificate check
            try
            {
                client = new System.Net.Sockets.TcpClient(ipaddr, 1270); // Connect to OpMgr agent port
                sslStream = new System.Net.Security.SslStream(
                    client.GetStream(),
                    false,
                    new System.Net.Security.RemoteCertificateValidationCallback(this.CertificateCheckCallback),
                    null);

                // Check the server's certificate
                sslStream.AuthenticateAsClient(ipaddr);
            }
            catch (System.Security.Authentication.AuthenticationException aex)
            {
                throw new Exception("Signed certificate failure", aex);
            }
            catch (Exception ex)
            {
                throw new Exception("CheckSslCert", ex);
            }

            if (sslStream != null)
            {
                sslStream.Close();
            }

            if (client != null)
            {
                client.Close();
            }
        }

        #endregion Public Methods

        #endregion Methods
    }
}
