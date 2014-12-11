//-----------------------------------------------------------------------
// <copyright file="RunWinCmd.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>a-brmill</author>
// <description></description>
// <history>1/20/2009 3:51:18 PM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Common
{
    using System;
    using System.Linq;
    using System.Management;
    using Microsoft.Win32;

    /// <summary>
    /// Description for RunWinCmd.
    /// </summary>
    public class RunWinCmd : IRunHelper
    {
        #region Private Fields

        /// <summary>
        /// Full path to location of executable
        /// </summary>
        private string workingDirectory = string.Empty;

        /// <summary>
        /// Name of executable
        /// </summary>
        private string fileName = string.Empty;

        /// <summary>
        /// Command line arguments for executable
        /// </summary>
        private string arguments = string.Empty;

        /// <summary>
        /// Is command expected to pass
        /// </summary>
        private bool expectToPass = true; // optional

        /// <summary>
        /// Ignore exit value
        /// </summary>
        private bool ignoreExit; // optional

        /// <summary>
        /// Timeout in milliseconds, 0 = no timeout
        /// </summary>
        private int timeOut; // optional

        /// <summary>
        /// StdOut result from executing command
        /// </summary>
        /// <todo>Implement in new execute function</todo>
        private string stdout = string.Empty;

        /// <summary>
        /// StdErr result from executing command
        /// </summary>
        /// <todo>Implement in new execute function</todo>
        private string stderr = string.Empty;

        /// <summary>
        /// Logger function
        /// </summary>
        private ScxLogDelegate logger = ScxMethods.ScxNullLogDelegate;

        /// <summary>
        /// Exit code from command
        /// </summary>
        private int exitCode;

        #endregion protected Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the RunWinCmd class for MCF usage.
        /// </summary>
        public RunWinCmd()
        {
        }

        /// <summary>
        /// Initializes a new instance of the RunWinCmd class.
        /// </summary>
        /// <param name="workingDirectory">Directory containing executable</param>
        /// <param name="fileName">Name of executable file</param>
        /// <param name="arguments">Command line arguments for executable</param>
        public RunWinCmd(string workingDirectory, string fileName, string arguments)
        {
            if (string.IsNullOrEmpty(workingDirectory))
            {
                throw new ArgumentException("WorkingDirectory");
            }

            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("FileName");
            }

            if (string.IsNullOrEmpty(arguments))
            {
                throw new ArgumentException("Arguments");
            }

            this.workingDirectory = workingDirectory;
            this.fileName = fileName;
            this.arguments = arguments;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the WorkingDirectory property, specifies location of executable
        /// </summary>
        public string WorkingDirectory
        {
            get { return this.workingDirectory; }
            set { this.workingDirectory = value; }
        }

        /// <summary>
        /// Gets or sets the FileName property, specifies name of executable
        /// </summary>
        public string FileName
        {
            get { return this.fileName; }
            set { this.fileName = value; }
        }

        /// <summary>
        /// Gets or sets the Arguments property, specifies arguments passed to executable
        /// </summary>
        public string Arguments
        {
            get { return this.arguments; }
            set { this.arguments = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the process is expected to exit successfully
        /// </summary>
        public bool ExpectToPass
        {
            get { return this.expectToPass; }
            set { this.expectToPass = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the command exit value should be ignored
        /// </summary>
        public bool IgnoreExit
        {
            get { return this.ignoreExit; }
            set { this.ignoreExit = value; }
        }

        /// <summary>
        /// Gets or sets the TimeOut property
        /// </summary>
        public int TimeOut
        {
            get { return this.timeOut; }
            set { this.timeOut = value; }
        }

        /// <summary>
        /// Gets the StdOut property
        /// </summary>
        /// <remarks>Not available with shell execution</remarks>
        /// <todo>Impelement new Execute function to populate this property</todo>
        public string StdOut
        {
            get { return this.stdout; }
        }

        /// <summary>
        /// Gets the StdErr property
        /// </summary>
        /// <remarks>Not available with shell execution</remarks>
        /// <todo>Impelement new Execute function to populate this property</todo>
        public string StdErr
        {
            get { return this.stderr; }
        }

        #endregion Properties

        #region Methods

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

        #region Public Methods

        /// <summary>
        /// Execute a program via the Windows command shell
        /// </summary>
        public void RunCmd()
        {
            bool cmdSucceeded;
            string userMessage;

            try
            {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.WorkingDirectory = this.workingDirectory;
                p.StartInfo.FileName = this.fileName;
                p.StartInfo.Arguments = this.arguments;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.RedirectStandardError = false;  // UseShellExecute and RedirectStandardError are mutually exclusive
                p.StartInfo.RedirectStandardOutput = false;
                p.EnableRaisingEvents = true;

                if (p.Start() == false)
                {
                    this.logger("Unable to start " + this.fileName + " " + this.arguments);
                    throw new ApplicationException("Unable to start " + this.fileName + " " + this.arguments);
                }

                if (this.timeOut != 0)
                {
                    p.WaitForExit(this.timeOut);
                }
                else
                {
                    p.WaitForExit();
                }

                this.exitCode = p.ExitCode;
                this.AnalyzeCmdResults(out cmdSucceeded, out userMessage);
                this.logger(userMessage);

                if (cmdSucceeded == false)
                {
                    throw new ApplicationException(userMessage);
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException("Unable to start " + this.fileName + " " + this.arguments, e);
            }
        }

        /// <summary>
        /// Run WMI Process on local|remote host.
        /// </summary>
        /// <param name="hostname">Hostname, both local and remote host supported</param>
        /// <param name="username">Username for WMI Connection</param>
        /// <param name="password">Password for WMI Connection</param>
        /// <returns> Return value check is not implemented yet. Return immediately after execute WMI process.</returns>
        public ManagementBaseObject RunWMICmd(string hostname, string username, string password)
        {
            ConnectionOptions connectionOptions = new ConnectionOptions();
            connectionOptions.Username = username;
            connectionOptions.Password = password;

            // Credentials is required for remote connection, but not for local connection.
            ManagementScope managementScope;
            if (-1 != hostname.IndexOf(Environment.MachineName, StringComparison.InvariantCultureIgnoreCase))
            {
                managementScope = new ManagementScope(string.Format(@"\\{0}\root\cimv2", hostname));
            }
            else
            {
                managementScope = new ManagementScope(string.Format(@"\\{0}\root\cimv2", hostname), connectionOptions);
            }

            managementScope.Connect();
            
            // Run WMI Process
            ManagementPath managementPath = new ManagementPath("Win32_Process");
            ObjectGetOptions objectGetOptions = new ObjectGetOptions();
            ManagementBaseObject outParams = null;
            using (ManagementClass processClass = new ManagementClass(managementScope, managementPath, objectGetOptions))
            {
                ManagementBaseObject inParams = processClass.GetMethodParameters("Create");
                inParams["CommandLine"] = string.Format("{0} {1}", this.FileName, this.Arguments);
                try
                {
                    outParams = processClass.InvokeMethod("Create", inParams, null);
                }
                catch (Exception ex)
                {
                    throw new Exception("WMI failed to run process : " + inParams["CommandLine"], ex);
                }
            }

            return outParams;
        }

        /// <summary>
        /// Run WMI process on local|remote host and wait for exit.
        /// </summary>
        /// <param name="hostname">Hostname, both local and remote host supported</param>
        /// <param name="username">Username for WMI Connection</param>
        /// <param name="password">Password for WMI Connection</param>
        /// <param name="milliSecondsTimeOut">Max wait time, -1 means to wait until process exits</param>
        public void RunWMICmd(string hostname, string username, string password, long milliSecondsTimeOut)
        {
            ConnectionOptions connectionOptions = new ConnectionOptions();
            connectionOptions.Username = username;
            connectionOptions.Password = password;

            // Credentials is required for remote connection, but not for local connection.
            ManagementScope managementScope;
            if (-1 != hostname.IndexOf(Environment.MachineName, StringComparison.InvariantCultureIgnoreCase))
            {
                managementScope = new ManagementScope(string.Format(@"\\{0}\root\cimv2", hostname));
            }
            else
            {
                managementScope = new ManagementScope(string.Format(@"\\{0}\root\cimv2", hostname), connectionOptions);
            }

            managementScope.Connect();
            
            // Build WMI connection
            ManagementPath managementPath = new ManagementPath("Win32_Process");
            ObjectGetOptions objectGetOptions = new ObjectGetOptions();
            using (ManagementClass processClass = new ManagementClass(managementScope, managementPath, objectGetOptions))
            {
                ManagementBaseObject inParams = processClass.GetMethodParameters("Create");
                inParams["CommandLine"] = string.Format("{0} {1}", this.FileName, this.Arguments);
                if (!string.IsNullOrEmpty(this.workingDirectory))
                {
                    inParams["CurrentDirectory"] = this.workingDirectory;
                }

                ManagementBaseObject outParams = null;
                try
                {
                    // Run WMI process
                    this.logger("Running command:" + inParams["CommandLine"]);
                    outParams = processClass.InvokeMethod("Create", inParams, null);
                    if (outParams["returnValue"].ToString() == "0")
                    {
                        // Check the process status
                        ManagementObjectSearcher searcher = null;
                        long count = 0;
                        WqlObjectQuery wqlObjectQuery = new WqlObjectQuery("select * from Win32_Process where ProcessID = " + outParams["processId"].ToString());
                        do
                        {
                            if (searcher != null)
                            {
                                System.Threading.Thread.Sleep(500);
                            }

                            searcher = new ManagementObjectSearcher(managementScope, wqlObjectQuery);
                        }
                        while ((searcher.Get().Count > 0) && ((500 * count++ <= milliSecondsTimeOut) || (milliSecondsTimeOut == -1)));
                    }
                    else
                    {
                        throw new Exception("Command didn't start properly.");
                    } 
                }
                catch (Exception ex)
                {
                    throw new Exception("WMI failed to run process : " + inParams["CommandLine"], ex);
                }
            }
        }

        /// <summary>
        /// Get temp folder path from registy in remote machine.
        /// </summary>
        /// <param name="hostname">Hostname, both local and remote host supported</param>
        /// <returns> Return temp folder path.</returns>
        public string GetTempPathFromRegistry(string hostname)
        {
            string tempPath = null;
            string tempKeyPath = @"SYSTEM\CurrentControlSet\Control\Session Manager\Environment";
            RegistryKey environmentKey = Registry.Users;

            try
            {
                // Open HKEY_LOCAL_MACHINE path of registy in specified remote machine
                environmentKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, hostname);

                // Open registy of the remote machine with specified path.
                RegistryKey tempKey = environmentKey.OpenSubKey(tempKeyPath);
                tempPath = tempKey.GetValue("TEMP").ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Could not get remote system's TEMP environment value"), ex);
            }
            finally
            {
                environmentKey.Close();
            }

            return tempPath;
        }

        /// <summary>
        /// Set the log delegate to allow logging using a mechanism provided by the user
        /// </summary>
        /// <param name="logDlg">Logging delegate method</param>
        public void SetLogDelegate(ScxLogDelegate logDlg)
        {
            this.logger = logDlg;
        }

        #endregion Public Methods

        #region protected Methods

        /// <summary>
        /// Analyze the command's return value according to whether it was expected to pass or fail
        /// </summary>
        /// <param name="succeeded">Set to true if the command succeeded</param>
        /// <param name="usrMsg">User message to be displayed to the user</param>
        /// <remarks>Success is based on test expectation, not the actual result.</remarks>
        protected void AnalyzeCmdResults(out bool succeeded, out string usrMsg)
        {
            succeeded = false;
            usrMsg = "RunWinCmd '" + this.fileName + " " + this.arguments + "' returned " + this.exitCode.ToString();

            if (this.ignoreExit == true)
            {
                succeeded = true;
                return;
            }

            if (this.expectToPass)
            {
                if (this.exitCode == 0)
                {
                    usrMsg += ", expected to pass, has passed";
                    succeeded = true;
                }
                else
                {
                    usrMsg += ", expected to pass, has failed";
                }
            }
            else
            {
                if (this.exitCode == 0)
                {
                    usrMsg += ", expected to fail, has passed";
                }
                else
                {
                    usrMsg += ", expected to fail, has failed";
                    succeeded = true;
                }
            }
        }

        #endregion protected Methods

        #endregion Methods
    }
}
