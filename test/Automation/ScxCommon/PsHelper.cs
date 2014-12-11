//-----------------------------------------------------------------------
// <copyright file="CmdletHelper.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>zhyao</author>
// <update>Author: Sunil. Added logging feature"</update>
//-----------------------------------------------------------------------

namespace Scx.Test.Common
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Management.Automation;
    using System.Management.Automation.Runspaces;
    using System.Runtime.Serialization;
    using System.Text.RegularExpressions;

    /// <summary>
    ///     If the verification of the pipeline status or the output fail, throw this exception.
    /// </summary>
    [Serializable]
    public class VerificationException : Exception
    {
        public VerificationException()
        {
        }

        public VerificationException(string reason) :
            base(reason)
        {
        }

        public VerificationException(string reason, Exception exception) :
            base(reason, exception)
        {
        }

        protected VerificationException(SerializationInfo info, StreamingContext context):
            base(info, context)
        {
        }
    }

    /// <summary>
    ///     Runs PowerShell script locally or remotely using WinRM.  The methods in this class is intended to be used
    ///     by MCF directly.
    /// </summary>
    public class PsHelper : IDisposable
    {
        /// <summary>
        ///     Default WinRM port on the remote host if no port is specified in Setup().
        /// </summary>
        private const int DefaultWinRMPort = 5985;
        private ILogger logger = new ConsoleLogger();
        private static Runspace runspace;
        private RunspaceConnectionInfo connectionInfo;
        private PipelineStateInfo lastPipelineStateInfo;
        private string exceptionMessage;
        private string errorMessage;
        private List<Dictionary<string, string>> lastPowerShellReturnValues;

        #region Aliases for Setup

        // Note: externally visible method should have no default parameters.

        public void Setup()
        {
            SetupInternal();
        }

        public void Setup(
            string hostName)
        {
            SetupInternal(hostName);
        }

        public void Setup(
            string hostName,
            string userName,
            string password)
        {
            SetupInternal(hostName, userName, password);
        }

        public void Setup(
            string hostName,
            string userName,
            string password,
            int portNumber,
            bool useSsl)
        {
            SetupInternal(hostName, userName, password, portNumber, useSsl);
        }

        #endregion

        public List<Dictionary<string, string>> LastPowerShellReturnData
        {
            get { return lastPowerShellReturnValues; }
        }

        /// <summary>
        /// Default logger is the <see cref="ConsoleLogger"/>.
        /// </summary>
        public ILogger Logger { set { logger = value; } }

        public string ExceptionMessage { get { return exceptionMessage; } }

        /// <summary>
        ///     Runs one or more lines of PowerShell scripts and saves the pipeline state as well as the return values
        /// </summary>
        /// <param name="scripts">PowerShell scripts</param>
        public void Run(
            string[] scripts)
        {
            exceptionMessage = string.Empty;
            errorMessage = string.Empty;
            var pipeline = runspace.CreatePipeline();

            PrintScript(LogLevel.Info, scripts);

            foreach (var script in scripts)
            {
                pipeline.Commands.AddScript(script);
            }

            Collection<PSObject> objs = null;
            try
            {
                objs = pipeline.Invoke();
            }
            catch (RuntimeException rex)
            {
                exceptionMessage = rex.ToString();
                logger.Write(LogLevel.Error,"**** Exception in Invoking the pipeline ****");
                logger.Write(LogLevel.Error, "Exception Message:" + exceptionMessage);
            }

            foreach (object obj in pipeline.Error.ReadToEnd())
            {
                errorMessage += obj.ToString();
            }            

            lastPipelineStateInfo = pipeline.PipelineStateInfo;
            logger.Write(LogLevel.Info, "Last pipeline State info: {0} - {1} ", 
                Convert.ToInt32(lastPipelineStateInfo.State), 
                lastPipelineStateInfo.State);
            lastPowerShellReturnValues = ConvertPowerShellObjects(objs);
        }

        /// <summary>
        ///     Checks the pipeline state information against the given pattern using Regex.
        /// </summary>
        /// <param name="state">Regex pattern of the pipeline state</param>
        /// <param name="reason">If the state is not "completed", the reason why it is not.</param>
        public void CheckPipeline(
            string state,
            string reason)
        {
            if (!Regex.Match(
                lastPipelineStateInfo.State.ToString(),
                state,
                RegexOptions.CultureInvariant | RegexOptions.IgnoreCase).Success)
            {
                throw new VerificationException(string.Format(
                    CultureInfo.InvariantCulture,
                    "State: expect='{0}' actual='{1}",
                    lastPipelineStateInfo.State,
                    state));
            }

            string expectedReason = string.Empty;
            if (lastPipelineStateInfo.Reason != null)
            {
                expectedReason = lastPipelineStateInfo.Reason.Message;
            }

            if (!Regex.Match(
                expectedReason,
                reason,
                RegexOptions.CultureInvariant | RegexOptions.IgnoreCase).Success)
            {
                throw new VerificationException(string.Format(
                    CultureInfo.InvariantCulture,
                    "Reason: expect='{0}' actual='{1}'",
                    expectedReason,
                    reason));
            }
        }

        /// <summary>
        ///     Checks the last output of the PowerShell scripting against the given key value pair. The Index is an string.
        /// </summary>
        /// <param name="index">index of PSObject being checked.</param>
        /// <param name="key"></param>
        /// <param name="value">Expected value (regex pattern)</param>
        public void CheckOutput(
            int index,
            string key,
            string value)
        {
            if (lastPowerShellReturnValues.Count <= index)
            {
                throw new VerificationException(string.Format(
                    CultureInfo.InvariantCulture,
                    "Index out of range: Count={0} index={1}",
                    lastPowerShellReturnValues.Count,
                    index));
            }

            if (lastPowerShellReturnValues[index].ContainsKey(key))
            {
                string actualValue = lastPowerShellReturnValues[index][key];

                if (!Regex.Match(actualValue, value, RegexOptions.IgnoreCase).Success)
                {
                    throw new VerificationException(string.Format(
                        CultureInfo.InvariantCulture,
                        "Index {0} Key '{1}': expect='{2}' actual='{3}'",
                        index,
                        key,
                        value,
                        actualValue));
                }

                logger.Write("Expected and actual values matched for key {0}", key);
            }
            else
            {
                throw new VerificationException(string.Format(
                    CultureInfo.InvariantCulture,
                    "Key '{0}' does not exist in the result",
                    key));
            }
        }

        /// <summary>
        ///     Checks the last output of the PowerShell scripting against the given key values pair.
        /// </summary>
        /// <param name="indexString">indexs of PSObject being checked. It's an int array..</param>
        /// <param name="key"></param>
        /// <param name="values">Expected values (regex pattern)</param>
        public void CheckOutput(
            string indexString,
            string key,
            string[] values)
        {
            if (string.IsNullOrEmpty(indexString) || values == null)
            {
                throw new ArgumentNullException("indexs or values");
            }

            string[] indexStrings = indexString.Split(',');
            int[] indexs = new int[indexStrings.Length];
            for (int index = 0; index < indexStrings.Length; index++)
            {
                if (!int.TryParse(indexStrings[index], out indexs[index]))
                {
                    throw new ApplicationException(string.Format("'{0}' should be an integer!", indexStrings[index]));
                }
            }
            if (indexs.Length != values.Length)
            {
                throw new ApplicationException("Indexs and values should have same count.");
            }

            if (lastPowerShellReturnValues.Count <= indexs.Max())
            {
                throw new VerificationException(string.Format(
                    CultureInfo.InvariantCulture,
                    "Index out of range: Count={0} index={1}",
                    lastPowerShellReturnValues.Count,
                    indexs.Max()));
            }

            for (int index = 0; index < indexs.Length; index++)
            {
                CheckOutput(indexs[index], key, values[index]);
            }
        }

        /// <summary>
        ///     Checks the last output of the PowerShell scripting for the given key-value pair 
        ///     and returns the index of the object.  The order of returned objects returned from 
        ///     PowerShell is not always same every time it's run.  This method overcomes that
        ///     shortcoming from previous CheckOutput method
        /// </summary>
        /// <param name="key">Dictionary key property</param>
        /// <param name="value">Expected value (regex pattern)</param>
        /// <returns>Index of first location for found key-value pair</returns>
        public int CheckOutput(
            string key,
            string value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("key or value");
            }

            if (LastPowerShellReturnData.Count <= 0)
            {
                throw new VerificationException(string.Format(
                    CultureInfo.InvariantCulture,
                    "Last PowerShell returned objects don't exist!"));
            }

            for (int index = 0; index < LastPowerShellReturnData.Count; index++)
            {
                if (LastPowerShellReturnData[index].ContainsKey(key))
                {
                    string actualValue = LastPowerShellReturnData[index][key];
                    if (Regex.Match(actualValue, value, RegexOptions.IgnoreCase).Success)
                    {
                        logger.Write("Expected and actual values matched for key '{0}' in index {1}", key, index);
                        return index;
                    }
                }
            }

            throw new VerificationException(string.Format(
                CultureInfo.InvariantCulture,
                "Key '{0}' with Value '{1}' not found in any of the returned PowerShell objects",
                key,
                value));
        }
        
        /// <summary>
        ///     Checks the last output of the PowerShell scripting for the given key-value pair 
        ///     and returns the index of the objects.  The order of returned objects returned from 
        ///     PowerShell is not always same every time it's run.  This method overcomes that
        ///     shortcoming from previous CheckOutput method
        /// </summary>
        /// <param name="key">Dictionary key property</param>
        /// <param name="values">Expected values (regex pattern)</param>
        /// <returns>Indexs string of first location for found key-value pair</returns>
        public string CheckOutput(
            string key,
            string[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }
            if (LastPowerShellReturnData.Count <= 0)
            {
                throw new VerificationException(string.Format(
                    CultureInfo.InvariantCulture,
                    "Last PowerShell returned objects don't exist!"));
            }

            return string.Join(",", values.Select(value => CheckOutput(key, value).ToString()).ToArray());
        }

        /// <summary>
        ///     Checks the error message of last PowerShell scripting for the given error message
        /// </summary>
        /// <param name="expectedErrorMessage">The expected error message</param>
        public void CheckErrorMessage(string expectedErrorMessage)
        {
            if (!Regex.Match(errorMessage, expectedErrorMessage, RegexOptions.IgnoreCase).Success)
            {
                throw new VerificationException(string.Format(
                       CultureInfo.InvariantCulture,
                       "Actual error not include expected: actual='{0}' expect='{1}'",
                       errorMessage, expectedErrorMessage));
            }

            logger.Write("Expected error message '{0}' returned!", expectedErrorMessage);
        }

        /// <summary>
        ///     Checks the exception message of last PowerShell scripting for the given exception message
        /// </summary>
        /// <param name="expectedExceptionMessage">The expected exception message</param>
        public void CheckExceptionMessage(string expectedExceptionMessage)
        {
            if (!Regex.Match(exceptionMessage, expectedExceptionMessage, RegexOptions.IgnoreCase).Success)
            {
                throw new VerificationException(string.Format(
                       CultureInfo.InvariantCulture,
                       "Actual exception not include expected: actual='{0}' expect='{1}'",
                       exceptionMessage, expectedExceptionMessage));
            }

            logger.Write("Expected exception message '{0}' returned!", expectedExceptionMessage);
        }

        /// <summary>
        ///     Checks the amount of PowerShell objects returned.
        /// </summary>
        /// <param name="expectedAmount">Expected amount of objects returned</param>
        public void CheckResultsCount(int expectedAmount)
        {
            if (LastPowerShellReturnData.Count == expectedAmount)
            {
                logger.Write("Expected amount of objects returned: {0}", expectedAmount);
            }
            else
            {
                throw new VerificationException(string.Format(
                    CultureInfo.InvariantCulture,
                    "Mismatched amount of returned objects: expected='{0}' actual='{1}'",
                    expectedAmount, LastPowerShellReturnData.Count));
            }
        }

        /// <summary>
        ///     Prints the last pipeline info and scripting output for preparing the verification.
        /// </summary>
        public void DebugPrint()
        {
            if (lastPipelineStateInfo != null)
            {
                logger.Write(
                    "*** Pipeline state='{0}' reason='{1}'",
                    lastPipelineStateInfo.State,
                    lastPipelineStateInfo.Reason == null ? "" : lastPipelineStateInfo.Reason.Message);
            }
            else
            {
                logger.Write("*** No pipeline status");
            }

            if (lastPowerShellReturnValues != null)
            {
                for (int index = 0; index < lastPowerShellReturnValues.Count; index++)
                {
                    foreach (KeyValuePair<string, string> kvp in lastPowerShellReturnValues[index])
                    {
                        logger.Write(
                            "*** index={0} '{1}' : '{2}'",
                            index,
                            kvp.Key,
                            kvp.Value);
                    }
                }
            }
            else
            {
                logger.Write("*** Last return value not found");
            }
        }

        /// <summary>
        ///     Disposes the object and closes the PowerShell runspace
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Setup the PowerShell runspace locally or remotely.
        /// </summary>
        /// <param name="hostName">empty for local host</param>
        /// <param name="userName">domain prefixed username, e.g. "REDMOND\scxsvc", empty for using the credential of the current user</param>
        /// <param name="password">password for the user</param>
        /// <param name="portNumber">0 if use the default port</param>
        /// <param name="useSsl">whether to use SSL transportation</param>
        private void SetupInternal(
            string hostName = null,
            string userName = null,
            string password = null,
            int portNumber = 0,
            bool useSsl = true)
        {
            if (hostName == null)
            {
                // Scripting on the local host.
                connectionInfo = null;

                runspace = RunspaceFactory.CreateRunspace();
                runspace.Open();

                return;
            }

            var securePassword = new System.Security.SecureString();
            using (securePassword)
            {
                foreach (char c in password)
                {
                    securePassword.AppendChar(c);
                }

                if (portNumber <= 0)
                {
                    // No port number is specified, use default port and http.

                    connectionInfo = new WSManConnectionInfo(
                        new Uri(string.Format(
                            CultureInfo.InvariantCulture,
                            "http://{0}:{1}/wsman",
                            hostName,
                            DefaultWinRMPort)),
                        "http://schemas.microsoft.com/powershell/Microsoft.PowerShell",
                        new PSCredential(
                            userName,
                            securePassword));
                }
                else
                {
                    connectionInfo = new WSManConnectionInfo(
                        useSsl,
                        hostName,
                        portNumber,
                        "wsman",
                        "http://schemas.microsoft.com/powershell/Microsoft.PowerShell",
                        new PSCredential(
                            userName,
                            securePassword));
                }
            }

            // Note: change this if necessary
            connectionInfo.AuthenticationMechanism = AuthenticationMechanism.Default;

            runspace = RunspaceFactory.CreateRunspace(connectionInfo);
            runspace.Open();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && runspace != null)
            {
                runspace.Close();
                runspace = null;
            }
        }

        /// <summary>
        ///     Converts the Collection of PSObject to the list of dictionary of key value pair so that the return
        ///     values can be checked easily.
        /// </summary>
        /// <param name="psObjects"></param>
        /// <returns></returns>
        private static List<Dictionary<string, string>> ConvertPowerShellObjects(
            IEnumerable<PSObject> psObjects)
        {
            var data = new List<Dictionary<string, string>>();

            if (psObjects == null)
                return data;

            foreach (PSObject obj in psObjects)
            {
                var dict = new Dictionary<string, string>();

                foreach (PSPropertyInfo prop in obj.Properties)
                {
                    try
                    {
                        if (prop.Value != null)
                        {
                            dict[prop.Name] = prop.Value.ToString();
                        }
                    }
                    catch (GetValueException e)
                    {
                        // Impossible to get the value.  Just store the Key.
                        dict[prop.Name] = e.Message;
                    }
                }

                data.Add(dict);
            }

            return data;
        }

        /// <summary>
        /// Prints all cmdlets used during any operation.
        /// </summary>
        /// <param name="loglvl">Level of details to print to the log</param>
        /// <param name="scripts">List of lines to add to the log</param>
        public void PrintScript(LogLevel loglvl, string[] scripts)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Environment.NewLine);
            foreach (string script in scripts)
            {
                sb.AppendLine(script);
            }
            sb.AppendLine(Environment.NewLine);
            logger.Write(loglvl, sb.ToString());
        }

        /// <summary>
        /// Get OM Servers In the Management Group
        /// </summary>
        /// <param name="sdkConnection">connection server to sdk</param>
        /// <returns>String contains all the om server name in the management group</returns>
        public string GetOMServersInManagementGroup(string sdkConnection)
        {
            string[] scripts = new string[1] { string.Format("Get-SCOMManagementServer -ComputerName {0}", sdkConnection) };
            this.Run(scripts);
            if (LastPowerShellReturnData.Count <= 0)
            {
                throw new VerificationException(string.Format(
                    CultureInfo.InvariantCulture,
                    "Last PowerShell returned objects don't exist!"));
            }
            string key = "Name"; //index of the server name in the last power shell return data
            if (!lastPowerShellReturnValues[0].ContainsKey(key))
            {
                throw new KeyNotFoundException(string.Format("Key {0} is not contained in last powershell returned objects!", key));
            }

            String[] powerShellReturnValues = new String[lastPowerShellReturnValues.Count];
            for (int index = 0; index < lastPowerShellReturnValues.Count; index++)
            {
                powerShellReturnValues[index] = lastPowerShellReturnValues[index][key].ToString();
            }
            String output = String.Join(",", powerShellReturnValues);

            return output;
        }
    }
}
