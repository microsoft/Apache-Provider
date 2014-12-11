//-----------------------------------------------------------------------
// <copyright file="CmdletHelper.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>sunilmu</author>
// <description></description>
// <history>3/7/2011 8:47:33 AM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Common
{
    using System;
    using System.Collections;
    using System.Text;
    using System.Security;

    /// <summary>
    /// Class that exposes some of the commonly called groups of cmdlets for easy reuse. For other 
    /// complex operations, use PsHelper to call the script directly from the varmap.
    /// </summary>
    public class CmdletHelper
    {
        /// <summary>
        /// OM's default resource pool name
        /// </summary>
        private const string DefaultResourcePool = "All Management Servers Resource Pool";

        private ILogger logger = new ConsoleLogger();

        public ILogger Logger { set { logger = value; } }
        
        /// <summary>
        /// Install the agent using the Install-SCXAgent cmdlet.
        /// TODO:$sshcred and $cred is set in Varmap. 
        /// </summary>
        /// <param name="psHelper">Powershell calls helper object</param>
        /// <param name="clientHostname">Linux/Unix clent system</param>
        /// <param name="omServer">OM Server</param>
        /// <param name="credential">Credentilas to use for the command</param>
        /// <param name="resourcePool">ResourcePool into which to add the agent</param>
        public void InstallScxAgent(
            PsHelper psHelper, 
            string clientHostname,          
            string omServer = "",
            string credential = "",
            string resourcePool = "")
        {
            logger.Write("Creating script for invoking Invoke-SCXAgent");

            ArrayList scripts = new ArrayList
                        {
                            "Sleep -Seconds 40",
                            "$pool=Get-SCOMResourcePool",
                             string.Format(
                                "$res=Invoke-SCXDiscovery -Name {0} -WsManCredential $cred -SshCredential $sshcred -ResourcePool $pool",
                                clientHostname),
                            "Install-SCXAgent -DiscoveryResult $res -Verbose"
                        };

            if (!string.IsNullOrEmpty(resourcePool))
            {
                scripts[1] = string.Format("{0} -Name \"{1}\"", scripts[1], resourcePool);
            }
            else
            {
                logger.Write("No resource pool specified. Getting the default resource pool");
                scripts[1] = string.Format("{0} -Name \"{1}\"", scripts[1], DefaultResourcePool);
            }

            if(!string.IsNullOrEmpty(omServer))
            {
                scripts[2] = string.Format("{0} -ComputerName {1}", scripts[2], omServer);
                scripts[3] = string.Format("{0} -ComputerName {1}", scripts[3], omServer);
            }

            if (!string.IsNullOrEmpty(credential))
            {
                scripts[2] = string.Format("{0} -Credential {1}", scripts[2], credential);
                scripts[3] = string.Format("{0} -Credential {1}", scripts[3], credential);
            }

            psHelper.Run(scripts.ToArray(typeof(string)) as string[]);
        }

        /// <summary>
        /// Installs using Uninstall-SCXAgent cmdlet.
        /// TODO: $sshcred is set in varmap. Use sdk to monitor, instead of sleep.
        /// </summary>
        /// <param name="psHelper">Powershell calls helper object</param>
        /// <param name="clientHostname">Linux/Unix clent system</param>
        /// <param name="omServer">OM Server</param>
        /// <param name="credential">Credentilas to use for the command</param>
        public void UninstallScxAgent(
            PsHelper psHelper,
            string clientHostname,
            string omServer = "",
            string credential = "")
        { 
            logger.Write("Creating script for invoking Uninstall-SCXAgent");
            ArrayList scripts = new ArrayList
                        {
                            "Sleep -Seconds 40",
                            string.Format("$agent = Get-SCXAgent -Name {0}", clientHostname),
                            "Uninstall-SCXAgent -Agent $agent -SshCredential $sshcred",
                        };

            if (!string.IsNullOrEmpty(omServer))
            {
                scripts[1] = string.Format("{0} -ComputerName {1}", scripts[1], omServer);
                scripts[2] = string.Format("{0} -ComputerName {1}", scripts[2], omServer);
            }

            if (!string.IsNullOrEmpty(credential))
            {
                scripts[1] = string.Format("{0} -Credential {1}", scripts[1], credential);
                scripts[2] = string.Format("{0} -Credential {1}", scripts[2], credential);
            }

            psHelper.Run(scripts.ToArray(typeof(string)) as string[]);
        }

        /// <summary>
        /// Invokes the Update-SCXAgent cmdlet.
        /// TODO: $sshcred and $cred are set in varmap. Might want to change that
        /// </summary>
        /// <param name="psHelper">Powershell calls helper object</param>
        /// <param name="clientHostname">Linux/Unix clent system</param>
        /// <param name="omServer">OM Server</param>
        /// <param name="credential">Credentilas to use for the command</param>
        public void UpdateScxAgent(
            PsHelper psHelper,
            string clientHostname,           
            string omServer = "",
            string credential = "")
        {
            logger.Write("Creating script for invoking Update-SCXAgent");
            ArrayList scripts = new ArrayList
                        {
                            "Sleep -Seconds 40",
                            string.Format("$agent = Get-SCXAgent -Name {0}", clientHostname),
                            "Update-SCXAgent -Verbose -Agent $agent -WsManCredential $cred -SshCredential $sshcred"
                        };

            if (!string.IsNullOrEmpty(omServer))
            {
                scripts[1] = string.Format("{0} -ComputerName {1}", scripts[1], omServer);
                scripts[2] = string.Format("{0} -ComputerName {1}", scripts[2], omServer);
            }

            if (!string.IsNullOrEmpty(credential))
            {
                scripts[1] = string.Format("{0} -Credential {1}", scripts[1], credential);
                scripts[2] = string.Format("{0} -Credential {1}", scripts[2], credential);
            }

            psHelper.Run(scripts.ToArray(typeof(string)) as string[]);
        }

        /// <summary>
        /// Removes the agent using Remove-SCXAgent cmdlet.
        /// </summary>
        /// <param name="psHelper">Powershell calls helper object</param>
        /// <param name="clientHostname">Linux/Unix clent system</param>
        /// <param name="omServer">OM Server</param>
        /// <param name="credential">Credentilas to use for the command</param>
        public void RemoveScxAgent(
            PsHelper psHelper,
            string clientHostname,
            string omServer = "",
            string credential = "")
        {
            logger.Write("Creating script for invoking Update-SCXAgent");
            ArrayList scripts = new ArrayList
                        {
                            string.Format("$agent = Get-SCXAgent -Name {0}", clientHostname),
                            "Remove-SCXAgent -Verbose -Agent $agent"
                        };


            if (!string.IsNullOrEmpty(omServer))
            {
                scripts[0] = string.Format("{0} -ComputerName {1}", scripts[0], omServer);
                scripts[1] = string.Format("{0} -ComputerName {1}", scripts[1], omServer);
            }

            if (!string.IsNullOrEmpty(credential))
            {
                scripts[0] = string.Format("{0} -Credential {1}", scripts[0], credential);
                scripts[1] = string.Format("{0} -Credential {1}", scripts[1], credential);
            }

            psHelper.Run(scripts.ToArray(typeof(string)) as string[]);
        }

        /// <summary>
        /// Invoke Get-ScxAgent cmdlet
        /// </summary>
        /// <param name="psHelper">Powershell calls helper object</param>
        /// <param name="clientHostname">Linux/Unix clent system</param>
        /// <param name="guid">GIUD of the agent</param>
        /// <param name="resourcePool">ResourcePool the agent belongs to</param>
        /// <param name="omServer">OM Server</param>
        /// <param name="credential">Credentilas to use for the command</param>
        public void GetScxAgent(
            PsHelper psHelper,
            string clientHostname,
            string guid = "",
            string resourcePool = "",
            string omServer = "",
            string credential = "")
        {
            logger.Write("Creating script for invoking Get-SCXAgent");
            StringBuilder script = new StringBuilder("Get-SCXAgent");

            if (!string.IsNullOrEmpty(clientHostname))
            {
                script.AppendFormat(" -Name {0}", clientHostname);
            }

            if (!string.IsNullOrEmpty(guid))
            {
                script.AppendFormat(" -ID {0}", script, guid);
            }

            if (!string.IsNullOrEmpty(resourcePool))
            {
                script.AppendFormat(" -ResourcePool {0}", resourcePool);
            }

            if (!string.IsNullOrEmpty(omServer))
            {
                script.AppendFormat(" -ComputerName {0}", omServer);
            }

            if (!string.IsNullOrEmpty(credential))
            {
                script.AppendFormat(" -Credential {0}", credential);
            }

            ArrayList scripts = new ArrayList {script.ToString() };

            psHelper.Run(scripts.ToArray(typeof(string)) as string[]);
        }

        /// <summary>
        /// Create Ssh Credential object
        /// </summary>
        /// <param name="psHelper">Powershell calls helper object</param>
        /// <param name="credVariable">Ssh Credential object's variable name</param>
        /// <param name="userName">Account username to use for the cmdlet call</param>
        /// <param name="password">Account password to use for the cmdlet call</param>
        /// <param name="sshKey">SSH Key file</param>
        /// <param name="elevationType">Elevation type</param>
        /// <param name="suPassword">Password for SU elevation</param>
        public void SetSshCredential(
          PsHelper psHelper,
          string credVariable="",
          string userName = "",
          string passphrase = "",
          string sshKey = "",
          string elevationType = "",
          string suPassword = "")
        {
            logger.Write("Create Ssh Credential object");
            ArrayList scripts = new ArrayList
                        {
                            string.Format("${0} = New-Object Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core.CredentialSet", credVariable),
                            "$posix = New-Object Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core.PosixHostCredential",
                            "$posix.Usage = 2",
                            string.Format("$posix.PrincipalName  = \"{0}\"", userName)
                        };

            if (!String.IsNullOrEmpty(passphrase))
            {
                scripts.Add(string.Format("$posix.Passphrase = ConvertTo-SecureString \"{0}\" -AsPlainText -Force", passphrase));
            }

            if (!String.IsNullOrEmpty(sshKey))
            {
                scripts.Add(string.Format("$posix.KeyFile = \"{0}\"", sshKey));
                scripts.Add("$posix.ReadAndValidateSshKey()");
            }

            if (elevationType.Equals("sudo"))
            {
                scripts.Add("$posix.Usage = 16");
            }

            scripts.Add(string.Format("${0}.Add($posix)", credVariable));
            
            if (elevationType.Equals("su"))
            {
                scripts.Add("$sucred = New-Object Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core.PosixHostCredential");
                scripts.Add("$sucred.Usage = 32");
                scripts.Add("$sucred.PrincipalName = \"root\"");
                scripts.Add(string.Format("$sucred.Passphrase = ConvertTo-SecureString \"{0}\" -AsPlainText -Force", suPassword));
                scripts.Add(string.Format("${0}.Add($sucred)", credVariable));
            }

            psHelper.Run(scripts.ToArray(typeof(string)) as string[]);
        }
    }
}
