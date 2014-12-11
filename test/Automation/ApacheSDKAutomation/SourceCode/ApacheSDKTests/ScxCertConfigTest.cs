//-----------------------------------------------------------------------
// <copyright file="ScxCertConfigTest.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>a-brmill</author>
// <description></description>
// <history>1/16/2009 10:34:27 AM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.SDK.SDKTests
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using Infra.Frmwrk;

    /// <summary>
    /// Description for ScxCertConfigTest.
    /// </summary>
    public class ScxCertConfigTest : ISetup, IRun
    {
        #region Private Fields

        /// <summary>
        /// MCF context
        /// </summary>
        private static IContext testContext = null;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ScxCertConfigTest class.
        /// </summary>
        public ScxCertConfigTest()
        {
        }

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Methods

        #region Private Methods

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Check the SSL certificate chain for host as certificate issuer
        /// </summary>
        /// <param name="sender">An object that contains state information for this validation</param>
        /// <param name="certificate">The certificate used to authenticate the remote party</param>
        /// <param name="chain">The chain of certificate authorities associated with the remote certificate</param>
        /// <param name="sslPolicyErrors">One or more errors associated with the remote certificate</param>
        /// <returns>A Boolean value that determines whether the specified certificate is accepted for authentication</returns>
        /// <remarks>The certificate chain needs to be checked "manually."
        /// ICertificatePolicy will not give the information that is required for
        /// this test, as we have our own certificate chain.</remarks>
        public static bool CertificateCheckCallback(
            object sender,
            System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            System.Security.Cryptography.X509Certificates.X509Chain chain,
            System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            testContext.Trc("CheckSslCert sslPolicyErrors=" +
                Enum.GetName(typeof(System.Net.Security.SslPolicyErrors), sslPolicyErrors));
            testContext.Alw("CheckSslCert Issuer =" + certificate.Issuer.ToString());
            testContext.Alw("CheckSslCert Subject=" + certificate.Subject.ToString());

            if (certificate.Issuer.ToString().Contains(System.Environment.MachineName))
            {
                testContext.Alw("CheckSslCert: Issuer is the host machine");
                return true;
            }
            else
            {
                testContext.Alw("CheckSslCert: Issuer is NOT the host machine");
                return false;
            }
        }

        /// <summary>
        /// Initialize from the variation setup
        /// </summary>
        /// <param name="ctx">MCF context</param>
        public void Setup(IContext ctx)
        {
            testContext = ctx;
        }

        /// <summary>
        /// Default Run entry point
        /// </summary>
        /// <param name="ctx">MCF context</param>
        public void Run(IContext ctx)
        {
        }

        /// <summary>
        /// Check the signing authority of a ceritificate
        /// </summary>
        /// <param name="ctx">MCF context</param>
        /// <remarks>The host name must be present in the "issuer" field, and
        /// not be present in the "subject" field</remarks>
        public void CheckCertSignature(IContext ctx)
        {
            string certFileName = ctx.FncRecords.GetValue("CertFileName").Trim();
            X509Certificate testCert = new X509Certificate();

            testCert.Import(certFileName);
            ctx.Alw("Issuer=" + testCert.Issuer.ToString() + "; subject=" + testCert.Subject.ToString());
            if (testCert.Issuer.ToString().Contains(System.Environment.MachineName) == false)
            {
                throw new Exception("Cert issuer = " + testCert.Issuer.ToString() + "; machine name = " + System.Environment.MachineName);
            }
        }

        /// <summary>
        /// Copy a file from a Posix system
        /// </summary>
        /// <param name="ctx">MCF context</param>
        public void CopyFromPosix(IContext ctx)
        {
            Scx.Test.Common.PosixCopy cpPosix = new Scx.Test.Common.PosixCopy(ctx);
            cpPosix.CopyFrom();
        }

        /// <summary>
        /// Copy a file to a Posix system
        /// </summary>
        /// <param name="ctx">MCF context</param>
        public void CopyToPosix(IContext ctx)
        {
            Scx.Test.Common.PosixCopy cpPosix = new Scx.Test.Common.PosixCopy(ctx);
            try
            {
                cpPosix.CopyTo();
            }
            catch (Exception e)
            {
                throw new VarFail("CopyToPosix failed", e);
            }
        }

        /// <summary>
        /// Execute a command on a Posix host
        /// </summary>
        /// <param name="ctx">MCF context</param>
        public void RunOnPosix(IContext ctx)
        {
            Scx.Test.Common.RunPosixCmd posixShell = new Scx.Test.Common.RunPosixCmd(ctx);
            try
            {
                posixShell.RunCmd();
            }
            catch (Exception e)
            {
                throw new VarFail("RunPosixCmd failed", e);
            }
        }

        /// <summary>
        /// Execute a Windows command
        /// </summary>
        /// <param name="ctx">MCF context</param>
        /// <remarks>WorkingDirectory, FileName, Arguments</remarks>
        public void ShellExec(IContext ctx)
        {
            Scx.Test.Common.RunWinCmd winCmd = new Scx.Test.Common.RunWinCmd(ctx);
            try
            {
                winCmd.RunCmd();
            }
            catch (Exception e)
            {
                throw new VarFail("ShellExec failed", e);
            }
        }

        /// <summary>
        /// Check a certificate via SSL connection to Posix machine
        /// </summary>
        /// <param name="ctx">MCF context</param>
        public void CheckSslCert(IContext ctx)
        {
            System.Net.Sockets.TcpClient client = null;
            System.Net.Security.SslStream sslStream = null;

            string ipaddr = Scx.Test.Common.CollectionAccessor.GetValue("IPAddr", ctx);

            // Set up ServicePointManager for program control of certificate check
            try
            {
                client = new System.Net.Sockets.TcpClient(ipaddr, 1270); // Connect to OpMgr agent port
                sslStream = new System.Net.Security.SslStream(
                    client.GetStream(),
                    false,
                    new System.Net.Security.RemoteCertificateValidationCallback(CertificateCheckCallback),
                    null);

                // Check the server's certificate
                sslStream.AuthenticateAsClient(ipaddr);
            }
            catch (System.Security.Authentication.AuthenticationException aex)
            {
                throw new VarFail("Signed certificate failure", aex);
            }
            catch (Exception ex)
            {
                throw new VarAbort("CheckSslCert", ex);
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
