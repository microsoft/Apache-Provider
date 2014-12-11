//-----------------------------------------------------------------------
// <copyright file="RunPosixCmd.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>a-brmill</author>
// <description></description>
// <history>1/20/2009 3:50:59 PM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Common
{
    using System;
    using sshcomLib; // This is the Microsoft SSL library

    /// <summary>
    /// Run a command on a Posix system via SSH connection
    /// </summary>
    public class RunPosixCmd : IRunHelper, IDisposable
    {
        #region Private Fields

        /// <summary>
        /// SSH connection to Posix host
        /// </summary>
        private sshcomLib.scxsshClass ssh;

        /// <summary>
        /// Name of Posix host
        /// </summary>
        private string hostName = string.Empty;

        /// <summary>
        /// Port number of SSH
        /// </summary>
        private int port = 22;

        /// <summary>
        /// Valid user name
        /// </summary>
        private string userName = string.Empty;

        /// <summary>
        /// Password for user name
        /// </summary>
        private string password = string.Empty;

        /// <summary>
        /// Followup password used in case of prompt, for example, when using su.
        /// </summary>
        private string followupPassword = string.Empty;

        /// <summary>
        /// Full path to executable file
        /// </summary>
        private string fileName = string.Empty;

        /// <summary>
        /// Arguments passed to executable file
        /// </summary>
        private string arguments = string.Empty;

        /// <summary>
        /// Is command expected to pass
        /// </summary>
        private bool expectToPass = true;

        /// <summary>
        /// Should the exit value be ignored
        /// </summary>
        private bool ignoreExit;

        /// <summary>
        /// Timeout for command, in milliseconds
        /// </summary>
        private int timeOut;

        /// <summary>
        /// Return value from command
        /// </summary>
        private uint exitCode;

        /// <summary>
        /// StdOut result from executing command
        /// </summary>
        private string stdout = string.Empty;

        /// <summary>
        /// StdErr result from executing command
        /// </summary>
        private string stderr = string.Empty;

        /// <summary>
        /// Delegate method for executing a command via SSH
        /// </summary>
        private PosixExecCmd posixExecCmd;

        /// <summary>
        /// Contains exception if an exception was thrown in the worker thread
        /// </summary>
        private Exception threadException;

        /// <summary>
        /// Log Delegate to allow writing using a log mechanism provided by the user.
        /// </summary>
        private LogDelegate logger = NullLogDelegate;

        /// <summary>
        /// The project does not disposed
        /// </summary>
        private bool alreadyDisposed = false;

        #endregion protected Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the RunPosixCmd class. Used for inheritance and direct VarMap usage.
        /// </summary>
        public RunPosixCmd()
        {
        }

        /// <summary>
        /// Initializes a new instance of the RunPosixCmd class.
        /// </summary>
        /// <param name="hostName">Name of Posix host</param>
        /// <param name="userName">Valid user on Posix host</param>
        /// <param name="password">Password for user</param>
        public RunPosixCmd(string hostName, string userName, string password)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                throw new ArgumentNullException("HostName");
            }

            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException("UserName");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("Password");
            }

            this.hostName = hostName;
            this.userName = userName;
            this.password = password;
            this.NewSshConnection();
        }

        /// <summary>
        /// Finalizes an instance of the RunPosixCmd class
        /// </summary>
        ~RunPosixCmd()
        {
            this.Dispose(true);
        }

        #endregion Constructors

        #region Delegates

        /// <summary>
        /// Delegate allowing output to the log file without having to accept an external class instance such as IContext.
        /// </summary>
        /// <param name="logMsg">Log message</param>
        public delegate void LogDelegate(string logMsg);

        /// <summary>
        /// Delegate method for executing a command via SSH
        /// </summary>
        /// <param name="command">Command to be executed</param>
        /// <param name="result">Standard output from the command</param>
        /// <returns>Command return value, 0-255</returns>
        protected delegate uint PosixExecCmd(string command, out string result);

        #endregion Delegates

        #region Properties

        /// <summary>
        /// Gets or sets the HostName property
        /// </summary>
        public string HostName
        {
            get { return this.hostName; }
            set { this.hostName = value; }
        }

        /// <summary>
        /// Gets or sets the UserName property
        /// </summary>
        public string UserName
        {
            get { return this.userName; }
            set { this.userName = value; }
        }

        /// <summary>
        /// Gets or sets the Password property
        /// </summary>
        public string Password
        {
            get { return this.password; }
            set { this.password = value; }
        }

        /// <summary>
        /// Gets or sets the FileName property
        /// </summary>
        /// <remarks>This is the full path to the executable</remarks>
        public string FileName
        {
            get { return this.fileName; }
            set { this.fileName = value; }
        }

        /// <summary>
        /// Gets or sets the Arguments property
        /// </summary>
        /// <remarks>This is appended to the FileName property</remarks>
        public string Arguments
        {
            get { return this.arguments; }
            set { this.arguments = value; }
        }

        /// <summary>
        /// Gets or sets the TimeOut property
        /// </summary>
        /// <remarks>Optional value specifies time to wait before aborting operation, in seconds</remarks>
        public int TimeOut
        {
            get { return this.timeOut; }
            set { this.timeOut = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the command is expected to pass
        /// </summary>
        /// <remarks>Optional value</remarks>
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
        /// Gets the command's standard output
        /// </summary>
        public string StdOut
        {
            get { return this.stdout; }
        }

        /// <summary>
        /// Gets the command's standard error output (not used)
        /// </summary>
        /// <remarks>Provided for interface compatibility, and is never set.
        /// Stderr stream is not available under the sshlib.dll COM library.</remarks>
        public string StdErr
        {
            get { return this.stderr; }
        }

        /// <summary>
        /// Gets the ExitCode
        /// </summary>
        public uint ExitCode
        {
            get { return this.exitCode; }
        }

        /// <summary>
        /// Gets or sets the followup password
        /// </summary>
        public string FollowupPassword
        {
            get { return this.followupPassword; }
            set { this.followupPassword = value; }
        }

        /// <summary>
        /// Gets or sets the ssh property
        /// </summary>
        public sshcomLib.scxsshClass Ssh
        {
            get { return this.ssh; }
            set { this.ssh = value; }
        }

        /// <summary>
        /// Gets or sets the log delegate.
        /// </summary>
        public LogDelegate Logger
        {
            get { return this.logger; }
            set { this.logger = value; }
        }

        /// <summary>
        /// Sets a value indicating whether to use ShellCommand2 (true) or ExecuteCommand2 (false, default)
        /// </summary>
        /// <remarks>Normally RunPosixCmd is invoked to start a binary
        /// executable on the Posix system.  However the command may be
        /// executed using the login shell (sh, bash, etc.) by setting
        /// this true.</remarks>
        public bool UseShellCommand
        {
            set
            {
                if (value == true)
                {
                    this.posixExecCmd = this.ssh.ShellCommand2;
                }
                else
                {
                    this.posixExecCmd = this.ssh.ExecuteCommand2;
                }
            }
        }

        #endregion Properties

        #region Static Methods

        /// <summary>
        /// Trivial log delegate function which does nothing.
        /// </summary>
        /// <param name="logMsg">Log message</param>
        public static void NullLogDelegate(string logMsg)
        {
            // do nothing
        }

        #endregion Static Methods

        #region Methods

        #region Public Methods

        /// <summary>
        /// Execute a command on a remote Posix machine using ExecuteCommand
        /// </summary>
        /// <remarks>When there is a problem with a SSH command, a
        /// very generic exception is thrown by the COM library.  This
        /// does not indicate that a significant bug-type error occured.  Check
        /// whether the host is accessible by DNS name and IP, whether you are
        /// using an executable binary or shell command, if the full path is
        /// specified, etc.</remarks>
        public void RunCmd()
        {
            bool threadCompleted = true;

            if (string.IsNullOrEmpty(this.fileName))
            {
                throw new ArgumentNullException("FileName property is null or empty");
            }

            // Create thread for SSH command to control timeout
            System.Threading.Thread posixThread = new System.Threading.Thread(this.ExecuteSshCommand);
            posixThread.Start();

            if (this.timeOut != 0)
            {
                threadCompleted = posixThread.Join(this.timeOut);
            }
            else
            {
                posixThread.Join();
            }

            // The threadException will be set independently of the Join()
            if (this.threadException != null)
            {
                throw new ApplicationException("SSH command failure", this.threadException);
            }

            // Thread completed normally
            if (threadCompleted == true)
            {
                bool testSucceeded;
                string message;

                this.AnalyzeCmdResults(out testSucceeded, out message);
                this.logger("Command returned: " + this.stdout);
                this.logger(message);

                if (testSucceeded == false)
                {
                    throw new ApplicationException(message);
                }
            }
            else
            {
                throw new ApplicationException("RunPosixCmd '" + this.fileName + "' timed out");
            }
        }

        /// <summary>
        /// Execute a command on a remote Posix machine using ExecuteCommand
        /// </summary>
        /// <param name="isCipher">Check cipher cases</param>
        /// <remarks>When there is a problem with a SSH command, a
        /// very generic exception is thrown by the COM library.  This
        /// does not indicate that a significant bug-type error occured.  Check
        /// whether the host is accessible by DNS name and IP, whether you are
        /// using an executable binary or shell command, if the full path is
        /// specified, etc.</remarks>
        public void RunCmd(bool isCipher)
        {
            bool threadCompleted = true;

            if (string.IsNullOrEmpty(this.fileName))
            {
                throw new ArgumentNullException("FileName property is null or empty");
            }

            // Create thread for SSH command to control timeout
            System.Threading.Thread posixThread = new System.Threading.Thread(this.ExecuteSshCommand);
            posixThread.Start();

            if (this.timeOut != 0)
            {
                threadCompleted = posixThread.Join(this.timeOut);
            }
            else
            {
                posixThread.Join();
            }

            // The threadException will be set independently of the Join()
            if (this.threadException != null)
            {
                throw new ApplicationException("SSH command failure", this.threadException);
            }

            // Thread completed normally
            if (threadCompleted == true)
            {
                bool testSucceeded;
                string message;

                this.AnalyzeCmdResults(out testSucceeded, out message, isCipher);
                this.logger("Command returned: " + this.stdout);
                this.logger(message);

                if (testSucceeded == false)
                {
                    throw new ApplicationException(message);
                }
            }
            else
            {
                throw new ApplicationException("RunPosixCmd '" + this.fileName + "' timed out");
            }
        }

        /// <summary>
        /// Execute a command on a remote Posix machine using ExecuteCommand without analyzing command results
        /// </summary>
        /// <remarks>When there is a problem with a SSH command, a
        /// very generic exception is thrown by the COM library.  This
        /// does not indicate that a significant bug-type error occured.  Check
        /// whether the host is accessible by DNS name and IP, whether you are
        /// using an executable binary or shell command, if the full path is
        /// specified, etc.</remarks>
        public void RunCmdWithoutAnalysis()
        {
            bool threadCompleted = true;

            if (string.IsNullOrEmpty(this.fileName))
            {
                throw new ArgumentNullException("FileName property is null or empty");
            }

            // Create thread for SSH command to control timeout
            System.Threading.Thread posixThread = new System.Threading.Thread(this.ExecuteSshCommand);
            posixThread.Start();

            if (this.timeOut != 0)
            {
                threadCompleted = posixThread.Join(this.timeOut);
            }
            else
            {
                posixThread.Join();
            }

            // The threadException will be set independently of the Join()
            if (this.threadException != null)
            {
                throw new ApplicationException("SSH command failure", this.threadException);
            }

            // Thread completed normally
            if (threadCompleted == true)
            {
                this.logger("Command returned: " + this.stdout);
            }
            else
            {
                throw new ApplicationException("RunPosixCmd '" + this.fileName + "' timed out");
            }
        }

        /// <summary>
        /// Execute a command on a remote Posix machine using ExecuteCommand in background. 
        /// </summary>
        /// <remarks>When there is a problem with a SSH command, a
        /// very generic exception is thrown by the COM library.  This
        /// does not indicate that a significant bug-type error occured.  Check
        /// whether the host is accessible by DNS name and IP, whether you are
        /// using an executable binary or shell command, if the full path is
        /// specified, etc.</remarks>
        public void RunCmdInBackGround()
        {
            if (string.IsNullOrEmpty(this.fileName))
            {
                throw new ArgumentNullException("FileName property is null or empty");
            }

            // Create thread for SSH command and run it in background
            System.Threading.Thread posixThread = new System.Threading.Thread(this.ExecuteSshCommand) { IsBackground = true };
            posixThread.Start();
        }

        /// <summary>
        /// Cipher cases when execute a command on a remote Posix machine using ExecuteCommand
        /// </summary>
        /// <param name="fullCommand">The full command string containing path to the command and its arguments</param>
        /// <remarks>with this method there is no need to set FileName or Arguments as these are extracted from 'fullCommand'
        /// </remarks>
        public void RunCipherCmd(string fullCommand)
        {
            this.Arguments = string.Empty;
            this.FileName = fullCommand;
            this.RunCmd(true);
        }

        /// <summary>
        /// Execute a command on a remote Posix machine using ExecuteCommand
        /// </summary>
        /// <param name="fullCommand">The full command string containing path to the command and its arguments</param>
        /// <remarks>with this method there is no need to set FileName or Arguments as these are extracted from 'fullCommand'
        /// </remarks>
        public void RunCmd(string fullCommand)
        {
            this.Arguments = string.Empty;
            this.FileName = fullCommand;
            this.RunCmd();
        }

        /// <summary>
        /// Execute a command on a remote Posix machine using ExecuteCommand in back ground
        /// </summary>
        /// <param name="fullCommand">The full command string containing path to the command and its arguments</param>
        /// <remarks>with this method there is no need to set FileName or Arguments as these are extracted from 'fullCommand'
        /// </remarks>
        public void RunCmdInBackGround(string fullCommand)
        {
            this.Arguments = string.Empty;
            this.FileName = fullCommand;
            this.RunCmdInBackGround();
        }

        /// <summary>
        /// Execute a command on a remote Posix machine, making retry attempts if the first one fails.
        /// </summary>
        /// <param name="fullCommand">The full command string containing path to the command and its arguments</param>
        /// <param name="maxRetries">Maximum number of times to run the command in case the first attempts fail.</param>
        public void RunCmd(string fullCommand, int maxRetries)
        {
            string errorMessage = string.Empty;
            bool success = false;

            for (int tries = 0; tries < maxRetries && !success; tries++)
            {
                try
                {
                    this.RunCmd(fullCommand);
                    success = true;
                }
                catch (Exception ex)
                {
                    success = false;
                    errorMessage = ex.Message;
                }

                if (!success)
                {
                    this.logger(string.Format("PosixCmd({0}) failed after {1}/{2} attempts: {3}", fullCommand, tries, maxRetries, errorMessage));
                    System.Threading.Thread.Sleep(new TimeSpan(0, 0, 30));
                }
            }

            if (!success)
            {
                throw new Exception(string.Format("PosixCmd({0}) failed after {1} attempts: {2}", fullCommand, maxRetries, errorMessage));
            }
        }

        /// <summary>
        /// Open a shell on the remote system.  This is necessary in case the command being run requires a second, interative
        /// password entry (for example, using the sudo command)
        /// </summary>
        public void OpenShell()
        {
            uint retValue = this.ssh.OpenShell();
            if (retValue != 0)
            {
                throw new Exception(string.Format("ssh.OpenShell failed: return code = {0}", retValue));
            }
        }

        /// <summary>
        /// Run command in a shell, optionally entering a followup password when prompted (for example, in reply to su or sudo)
        /// </summary>
        /// <param name="cmd">The full command, including file and arguments</param>
        public void RunShellCommand(string cmd)
        {
            bool threadCompleted = true;

            this.fileName = cmd;

            this.ssh.SetTimeout((uint)this.timeOut);
            this.ssh.SetShellOutputTimeout((uint)this.timeOut);

            // Create thread for SSH command to control timeout
            System.Threading.Thread posixThread = new System.Threading.Thread(this.ExecuteSshShellCommand);
            posixThread.Start();

            if (this.timeOut != 0)
            {
                threadCompleted = posixThread.Join(this.timeOut);
            }
            else
            {
                posixThread.Join();
            }

            // The threadException will be set independently of the Join()
            if (this.threadException != null)
            {
                throw new ApplicationException("SSH shell command failure", this.threadException);
            }

            // Thread completed normally
            if (threadCompleted == true)
            {
                bool testSucceeded;
                string message;

                this.AnalyzeCmdResults(out testSucceeded, out message);
                this.logger("Command returned: " + this.stdout);
                this.logger(message);

                if (testSucceeded == false)
                {
                    throw new ApplicationException(message);
                }
            }
            else
            {
                throw new ApplicationException("RunPosixCmd '" + this.fileName + "' timed out");
            }
        }

        /// <summary>
        /// Set the log delegate to allow logging using a mechanism provided by the user
        /// </summary>
        /// <param name="logDlg">Logging delegate method</param>
        public void SetLogDelegate(LogDelegate logDlg)
        {
            this.logger = logDlg;
        }

        /// <summary>
        /// The function for dispose.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Analyze the command's return value according to whether it was expected to pass or fail
        /// </summary>
        /// <param name="succeeded">set to true if the command succeeded according to expectations</param>
        /// <param name="usrMsg">text containing user message</param>
        /// <param name="isCipher">Check cipher cases</param>
        /// <remarks>Success is based on test expectation, not the actual result.</remarks>
        public void AnalyzeCmdResults(out bool succeeded, out string usrMsg, bool isCipher = false)
        {
            succeeded = false;
            usrMsg = string.Format("RunPosixCmd '{0}' returned {1}", this.fileName, this.exitCode.ToString());

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
                    if (isCipher)
                    {
                        if (this.stdout.Contains("Cipher is (NONE)") || this.stdout.Contains("Secure Renegotiation IS NOT supported"))
                        {
                            succeeded = true;
                            usrMsg += ", cipher case expected to pass, has passed";
                        }
                    }
                    else
                    {
                        usrMsg += ", expected to pass, has failed";
                    }
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

        #endregion Public Methods

        #region protected Methods

        /// <summary>
        /// Initializes a new instance of the Dispose class to dispose the project.
        /// </summary>
        /// <param name="isDisposing">The bool string to sign the status of dispose</param>
        protected virtual void Dispose(bool isDisposing)
        {
            if (this.alreadyDisposed)
            {
                return;
            }

            if (isDisposing)
            {
                if (this.ssh != null)
                {
                    this.ssh.Shutdown();
                }
            }

            this.alreadyDisposed = true;
        }

        /// <summary>
        /// Initialize a new SSH connection
        /// </summary>
        /// <returns>returns whether the connection completed successfully</returns>
        protected bool NewSshConnection()
        {
            uint connectValue;

            try
            {
                this.ssh = new scxsshClass();
                /*returns uint*/
                connectValue = this.ssh.ConnectWithPassword(this.hostName, this.port, this.userName, this.password);
                this.posixExecCmd = this.ssh.ExecuteCommand2;
            }
            catch (Exception e)
            {
                throw new Exception("Unable to connect to host", e);
            }

            return connectValue == 0;
        }

        /// <summary>
        /// Run a seperate thread to execute a command over SSH.
        /// </summary>
        /// <remarks>This (hopefully) provides timeout control over remote process</remarks>
        protected void ExecuteSshCommand()
        {
            this.stdout = string.Empty;
            this.stderr = string.Empty;
            this.threadException = null;

            try
            {
                if (string.IsNullOrEmpty(this.arguments))
                {
                    this.exitCode = this.posixExecCmd(this.fileName, out this.stdout);
                }
                else
                {
                    this.exitCode = this.posixExecCmd(this.fileName + " " + this.arguments, out this.stdout);
                }
            }
            catch (Exception e)
            {
                this.threadException = e;
            }
        }

        /// <summary>
        /// Run a seperate thread to execute a shell command over SSH.
        /// </summary>
        protected void ExecuteSshShellCommand()
        {
            this.stdout = string.Empty;
            this.stderr = string.Empty;
            this.threadException = null;

            try
            {
                if (!string.IsNullOrEmpty(this.followupPassword))
                {
                    string[] followupPasswords = this.followupPassword.Split(new char[] { ';' });

                    this.ssh.SetShellOutputTimeout(5000);

                    this.exitCode = this.ssh.ShellCommand2(this.fileName, out this.stdout);

                    for (int passwordIndex = 0;
                        passwordIndex < followupPasswords.Length && this.exitCode == 0;
                        passwordIndex++)
                    {
                        string stdOutBuffer = string.Empty;
                        System.Threading.Thread.Sleep(1000);
                        this.exitCode = this.ssh.ShellCommand2(followupPasswords[passwordIndex], out stdOutBuffer);
                        this.stdout += stdOutBuffer;
                    }
                }
                else
                {
                    this.exitCode = this.ssh.ShellCommand2(this.fileName, out this.stdout);
                }
            }
            catch (Exception e)
            {
                this.threadException = e;
            }
        }

        #endregion protected Methods

        #endregion Methods
    }
}
