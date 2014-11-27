//-----------------------------------------------------------------------
// <copyright file="WSManQuery.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>a-brmill</author>
// <description></description>
// <history>3/31/2009 10:32:07 AM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.WSMAN.Common
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Xml;
    using Infra.Frmwrk;
    using WSManAutomation;

    /// <summary>
    /// The WSManQuery class encapsulates methods for performing Web 
    /// Service Management operations on a Posix host.
    /// </summary>
    public class WSManQuery
    {
        #region Private Fields

        /// <summary>
        /// The same part of XML schema namespace
        /// </summary>
        private const string Xmlns = "_INPUT xmlns:p=\"http://schemas.microsoft.com/wbem/wscim/1/cim-schema/2/";

        /// <summary>
        /// Host name (or IP address) of the Posix system
        /// </summary>
        private string hostname;

        /// <summary>
        /// Valid user name for the Posix system
        /// </summary>
        private string username;

        /// <summary>
        /// Valid password for the Posix system
        /// </summary>
        private string password;

        /// <summary>
        /// WSMAN service port number, default 1270
        /// </summary>
        private uint port = 1270;

        /// <summary>
        /// Current MCF context;
        /// </summary>
        private IContext mcfContext;

        /// <summary>
        /// XML Schema prefix
        /// </summary>
        private string xmlSchemaPrefix = "http://schemas.microsoft.com/wbem/wscim/1/cim-schema/2/";

        /// <summary>
        /// XML Schema suffix
        /// </summary>
        private string xmlSchemaSuffix = "?__cimnamespace=root/apache";

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the WSManQuery class.
        /// </summary>
        /// <param name="mcfContext">Current MCF context</param>
        /// <param name="hostName">Name or IP address of the Posix system</param>
        /// <param name="userName">Valid user name for the Posix system</param>
        /// <param name="password">Password for the username</param>
        public WSManQuery(IContext mcfContext, string hostName, string userName, string password)
        {
            this.mcfContext = mcfContext;
            this.hostname = hostName;
            this.username = userName;
            this.password = password;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the value of Hostname
        /// </summary>
        public string Hostname
        {
            get { return this.hostname; }
            set { this.hostname = value; }
        }

        /// <summary>
        /// Gets or sets the value of Username
        /// </summary>
        public string Username
        {
            get { return this.username; }
            set { this.username = value; }
        }

        /// <summary>
        /// Gets or sets the value of Password
        /// </summary>
        public string Password
        {
            get { return this.password; }
            set { this.password = value; }
        }

        /// <summary>
        /// Gets or sets the value of Port
        /// </summary>
        public uint Port
        {
            get { return this.port; }
            set { this.port = value; }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Execute a WSMAN query against a Posix system in the root/scx namespace with default returnType.
        /// </summary>
        /// <param name="queryXml">Results of WSMAN query in XML format</param>
        /// <param name="queryClass">Class in root/scx namespace to query</param>
        public void EnumerateScx(out List<string> queryXml, string queryClass)
        {
            this.EnumerateScx(out queryXml, queryClass, string.Empty);
        }

        /// <summary>
        /// Execute a WSMAN query against a Posix system in the root/scx namespace.
        /// </summary>
        /// <param name="queryXml">Results of WSMAN query in XML format</param>
        /// <param name="queryClass">Class in root/scx namespace to query</param>
        /// <param name="returnType">The returnType of winrm enumerate</param>>
        public void EnumerateScx(out List<string> queryXml, string queryClass, string returnType)
        {
            queryXml = new List<string>();
            WSManClass webSvcMan = new WSManClass();
            int enumFlag;
            switch (returnType)
            {
                case "object": enumFlag = webSvcMan.EnumerationFlagReturnObject(); 
                    break;
                case "epr": enumFlag = webSvcMan.EnumerationFlagReturnEPR(); 
                    break;
                case "objectAndepr": enumFlag = webSvcMan.EnumerationFlagReturnObjectAndEPR(); 
                    break;
                default: enumFlag = webSvcMan.EnumerationFlagReturnObject(); 
                    break;
            }

            try
            {
                IWSManSession session = this.GetWSMANSession(queryClass);

                // Construct XML namespace URI
                string strResourceLocator = string.Concat(this.xmlSchemaPrefix, queryClass, this.xmlSchemaSuffix);
                IWSManResourceLocator resourceLocator = (IWSManResourceLocator)webSvcMan.CreateResourceLocator(strResourceLocator);
                IWSManEnumerator enumerator = (IWSManEnumerator)session.Enumerate(resourceLocator, string.Empty, string.Empty, enumFlag);

                while (enumerator.AtEndOfStream == false)
                {
                    queryXml.Add(enumerator.ReadItem());
                }
            }
            catch (COMException ce)
            {
                this.HandleWSMANException(ce, queryClass, webSvcMan);
            }
        }

        /// <summary>
        /// Execute a WSMAN query against a Posix system in the root/scx namespace.
        /// </summary>
        /// <param name="queryXml">Results of WSMAN query in XML format</param>
        /// <param name="queryClass">Class in root/scx namespace to query</param>
        /// <param name="keyValuePair">A key/value pair for the resource locator string.</param>
        public void GetScx(out string queryXml, string queryClass, string keyValuePair)
        {
            queryXml = string.Empty;
            WSManClass webSvcMan = new WSManClass();

            int getFlag = 0;

            try
            {
                IWSManSession session = this.GetWSMANSession(queryClass);

                // Construct XML namespace URI
                string strResourceLocator = string.Concat(this.xmlSchemaPrefix, queryClass, this.xmlSchemaSuffix, "+", keyValuePair);
                IWSManResourceLocator resourceLocator = (IWSManResourceLocator)webSvcMan.CreateResourceLocator(strResourceLocator);

                string getOutput = session.Get(resourceLocator, getFlag);
                queryXml = getOutput;
                this.mcfContext.Trc("output of get " + getOutput);
            }
            catch (COMException ce)
            {
                this.HandleWSMANException(ce, queryClass, webSvcMan);
            }
        }

        /// <summary>
        /// Execute a WSMAN invoke against a Posix system in the root/scx namespace.
        /// </summary>
        /// <param name="result">Output parameter containing the XML-encoded result of the invocation.</param>
        /// <param name="queryClass">Query class (provider) to use, e.g. SCX_OperatingSystem</param>
        /// <param name="invokeMethod">The invoke method, e.g. ExecuteCommand</param>
        /// <param name="parameters">The WSMAN parameter string</param>
        public void InvokeScx(out string result, string queryClass, string invokeMethod, string parameters)
        {
            result = string.Empty;
            WSManClass webSvcMan = new WSManClass();

            try
            {
                IWSManSession session = this.GetWSMANSession(queryClass);

                // Construct XML namespace URI
                string strResourceLocator = string.Concat(this.xmlSchemaPrefix, queryClass, this.xmlSchemaSuffix);
                IWSManResourceLocator resourceLocator = (IWSManResourceLocator)webSvcMan.CreateResourceLocator(strResourceLocator);

                result = session.Invoke(invokeMethod, resourceLocator, parameters, 0);
            }
            catch (COMException ce)
            {
                this.HandleWSMANException(ce, queryClass, webSvcMan);
            }
        }

        /// <summary>
        /// Helper method to run a posix style command on the remote client using WSMAN, handling normal error conditions following a command
        /// execution.
        /// </summary>
        /// <param name="result">Output parameter containing the contents of stdout</param>
        /// <param name="queryClass">Query class (provider) to use, e.g. SCX_OperatingSystem</param>
        /// <param name="invokeMethod">The invoke method, e.g. ExecuteCommand</param>
        /// <param name="parameters">The WSMAN parameter string</param>
        public void InvokeCommand(out string result, string queryClass, string invokeMethod, string parameters)
        {
            result = string.Empty;
            string resultRawXml = string.Empty;

            this.InvokeScx(out resultRawXml, queryClass, invokeMethod, parameters);

            XmlDocument resultXml = new XmlDocument();
            resultXml.LoadXml(resultRawXml);
            XmlElement resultXmlRoot = resultXml.DocumentElement;

            XmlNodeList nodeList = resultXmlRoot.GetElementsByTagName("n1:ReturnCode");

            //For OMI agent: the result xml contains tag "p:*" , not "n1:*". 
            //Modified by v-jeyin 6/18
            if (nodeList == null || nodeList.Count == 0)
            {
                nodeList = resultXmlRoot.GetElementsByTagName("p:ReturnCode");
            }

            if (nodeList.Count > 0)
            {
                string returnCode = nodeList[0].InnerText.Trim();

                if (returnCode != "0")
                {
                    this.mcfContext.Trc("InvokeScx: returnCode = " + returnCode);
                    throw new Exception("InvokeScx: returnCode = " + returnCode);
                }
            }

            nodeList = resultXmlRoot.GetElementsByTagName("n1:StdErr");

            //For OMI agent: the result xml contains tag "p:*" , not "n1:*". 
            //Modified by v-jeyin 6/18
            if (nodeList == null || nodeList.Count == 0)
            {
                nodeList = resultXmlRoot.GetElementsByTagName("p:StdErr");
            }

            if (nodeList.Count > 0)
            {
                string stdErr = nodeList[0].InnerText.Trim();
                if (stdErr.Contains("Permission denied"))
                {
                    this.mcfContext.Trc("InvokeScx: stderr contains 'Permission denied'");
                    throw new Exception("InvokeScx: stderr contains 'Permission denied'");
                }
            }

            nodeList = resultXmlRoot.GetElementsByTagName("n1:StdOut");
            //For OMI agent: the result xml contains tag "p:*" , not "n1:*". 
            //Modified by v-jeyin 6/18
            if (nodeList == null || nodeList.Count == 0)
            {
                nodeList = resultXmlRoot.GetElementsByTagName("p:StdOut");
            }

            if (nodeList.Count > 0)
            {
                result = nodeList[0].InnerText.Trim();
                this.mcfContext.Trc("InvokeScx: result = " + result);
            }
        }

        /// <summary>
        /// Run the WSMAN invocation for executing a a given command on the remote client.
        /// </summary>
        /// <param name="result">Output parameter containing the contents of stdout</param>
        /// <param name="command">Command to run on the remote client</param>
        /// <param name="timeout">Timeout in seconds for the execution</param>
        /// <param name="elevationType">Use sudo to execute command</param>
        public void InvokeExecuteCommand(out string result, string command, string timeout, string elevationType)
        {
            string parameter = elevationType != null ? "<p:elevationType>" + elevationType + "</p:elevationType>" : string.Empty;

            string parameters = string.Concat(
                "<p:ExecuteCommand",
                Xmlns + "SCX_OperatingSystem.xsd\">",
                "<p:command>",
                command,
                "</p:command><p:timeout>",
                timeout,
                "</p:timeout>" + parameter + "</p:ExecuteCommand_INPUT>");

            this.InvokeCommand(out result, "SCX_OperatingSystem", "ExecuteCommand", parameters);
        }

        /// <summary>
        /// Run the WSMAN invocation for executing a a given shell command on the remote client.
        /// </summary>
        /// <param name="result">Output parameter containing the contents of stdout</param>
        /// <param name="command">Command to run on the remote client</param>
        /// <param name="timeout">Timeout in seconds for the execution</param>
        /// <param name="elevationType">Use sudo to execute command</param>
        public void InvokeExecuteShellCommand(out string result, string command, string timeout, string elevationType)
        {
            string parameter = elevationType != null ? "<p:elevationType>" + elevationType + "</p:elevationType>" : string.Empty;

            string parameters = string.Concat(
                 "<p:ExecuteShellCommand",
                 Xmlns + "SCX_OperatingSystem.xsd\">",
                 "<p:command>",
                 command,
                 "</p:command><p:timeout>",
                 timeout,
                "</p:timeout>" + parameter + "</p:ExecuteShellCommand_INPUT>");

            this.InvokeCommand(out result, "SCX_OperatingSystem", "ExecuteShellCommand", parameters);
        }

        /// <summary>
        /// Run the WSMAN invocation for executing a given script on the remote client
        /// </summary>
        /// <param name="result">Output parameter containing the contents of stdout</param>
        /// <param name="script">Script to run on the remote client</param>
        /// <param name="timeout">Timeout in seconds for the execution</param>
        /// <param name="arguments">The arguments of an wsman command</param>
        /// <param name="elevationType">Use sudo to execute command</param>
        public void InvokeExecuteScript(out string result, string script, string timeout, string arguments, string elevationType)
        {
            string parameter = elevationType != null ? "<p:elevationType>" + elevationType + "</p:elevationType>" : string.Empty;

            string parameters = string.Concat(
                "<p:ExecuteScript",
                Xmlns + "SCX_OperatingSystem.xsd\">",
                "<p:script>",
                script,
                "</p:script><p:arguments>",
                arguments,
                "</p:arguments><p:timeout>",
                timeout,
                "</p:timeout>" + parameter + "</p:ExecuteScript_INPUT>");

            this.InvokeCommand(out result, "SCX_OperatingSystem", "ExecuteScript", parameters);
        }

        /// <summary>
        /// Run the WSMAN invocation to retrieve a list of processes using the most resources
        /// </summary>
        /// <param name="result">Output parameter for the plaintext result of the invocation</param>
        /// <param name="resource">The resource type, for example 'CPUTime'</param>
        /// <param name="count">The length of the list of processes to retrieve</param>
        public void InvokeTopResourceConsumers(out string result, string resource, string count)
        {
            string parameters = string.Concat(
                "<p:TopResourceConsumers",
                Xmlns + "SCX_UnixProcess.xsd\">",
                "<p:resource>",
                resource,
                "</p:resource><p:count>",
                count,
                "</p:count></p:TopResourceConsumers_INPUT>");

            this.InvokeCommand(out result, "SCX_UnixProcess", "TopResourceConsumers", parameters);
        }

        /// <summary>
        /// Run the WSMAN invocation to retrieve matched log file rows
        /// </summary>
        /// <param name="results">Output parameter for an array of strings, each containing a plaintext row from the logfiel (stripped of XML tags)</param>
        /// <param name="fileName">Full path to the log file being polled</param>
        /// <param name="regexp">A regular expression with which to examine the log file</param>
        /// <param name="qid">The 'qid'.  This is the hostname of the server generating the WSMAN, and is usually 'localhost'</param>
        /// <param name="elevationType">Use sudo to execute command</param>
        public void InvokeGetMatchedRows(out string[] results, string fileName, string regexp, string qid, string elevationType)
        {
            results = new string[0];
            int matchesFound = 0;
            string parameter = elevationType != null ? "<p:elevationType>" + elevationType + "</p:elevationType>" : string.Empty;

            string parameters = string.Concat(
                "<p:GetMatchedRows",
                Xmlns + "SCX_LogFile.xsd\">",
                "<p:filename>",
                fileName,
                "</p:filename><p:regexps>",
                regexp,
                "</p:regexps><p:qid>",
                qid,
                "</p:qid>" + parameter + "</p:GetMatchedRows_INPUT>");

            string resultRawXml = string.Empty;

            try
            {
                this.InvokeScx(out resultRawXml, "SCX_LogFile", "GetMatchedRows", parameters);
            }
            catch (Exception e)
            {
                this.mcfContext.Trc("InvokeGetMatchedRows failed: " + e.Message);
                return;
            }

            XmlDocument resultXml = new XmlDocument();
            resultXml.LoadXml(resultRawXml);
            XmlElement resultXmlRoot = resultXml.DocumentElement;

            /* * 
             * Modified by v-jeyin 6/20 
             * Fix test issue for case which related GetMatchedRows(42572, 42565 , 42578,40606,40609,40611) 
             * This is a bug 542383 which is by design 
             * It returens SCX_LogFile_OUTPUT and alue is always 0 in it for Omi agent, but for Pegusas agent , it returns GetMatchedRows_OUTPUT  

                and should have rows counts in result 
             * */
            if (resultXmlRoot.Name.Equals("p:SCX_LogFile_OUTPUT"))
            {
                results = new string[1];
                results[0] = "p:SCX_LogFile_OUTPUT";
                this.mcfContext.Trc("This is omi agent.");
                return;
            }

            XmlNodeList nodeList = resultXmlRoot.GetElementsByTagName("n1:ReturnValue");
            if (nodeList.Count > 0)
            {
                string returnValue = nodeList[0].InnerText.Trim();

                if (returnValue == "0")
                {
                    return;
                }
            }
            else
            {
                return;
            }

            nodeList = resultXmlRoot.GetElementsByTagName("n1:rows");
            matchesFound = nodeList.Count;

            if (matchesFound > 0)
            {
                int rowOffset = 0;

                // If a regular expression is invalid, the error message is contained inline in the row list
                if (nodeList[0].InnerText.Trim().StartsWith("InvalidRegexp"))
                {
                    this.mcfContext.Trc("InvokeGetMatchedRows: Invalid Regular expression");
                    return;
                }

                // if the first row matches this, it is not 'real' data but rather an indication that
                // there are more rows which were matched by the regexps but not returned by the invoke.
                // Therefore, this row has to be suppressed from the results of this method.
                if (nodeList[0].InnerText.Trim().StartsWith("MoreRowsAvailable"))
                {
                    matchesFound--;
                    rowOffset = 1;
                }

                this.mcfContext.Trc(string.Format("InvokeGetMatchedRows: {0} matches found", matchesFound));

                results = new string[matchesFound];

                for (int i = 0; i < matchesFound; i++)
                {
                    string result = nodeList[i + rowOffset].InnerText.Trim();

                    // For unknown reasons, sometimes row results start with a spurious '0;'. 
                    if (result.StartsWith("0;"))
                    {
                        result = result.Substring(2).Trim();
                    }

                    results[i] = result;
                }
            }
            else
            {
                this.mcfContext.Trc("InvokeGetMatchedRows: no matches found");
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Create a WSMAN session with the remote client based the query class (provider) and information in the constructor.
        /// </summary>
        /// <param name="queryClass">The query class</param>
        /// <returns>Returns an interface class to WSMAN</returns>
        private IWSManSession GetWSMANSession(string queryClass)
        {
            WSManClass webSvcMan = new WSManClass();
            IWSManSession session = null;

            IWSManConnectionOptions cnctOptions = (IWSManConnectionOptions)webSvcMan.CreateConnectionOptions();
            cnctOptions.UserName = this.username;
            cnctOptions.Password = this.password;

            int sessionFlags = webSvcMan.SessionFlagUseBasic() + webSvcMan.SessionFlagCredUsernamePassword()
                        + webSvcMan.SessionFlagSkipCACheck() + webSvcMan.SessionFlagSkipCNCheck()
                        + webSvcMan.SessionFlagUTF8()
                        + webSvcMan.SessionFlagSkipRevocationCheck(); // Add for winrm 2.0, remove for winrm 1.1.
            try
            {
                string sessionHost = string.Concat("https://", this.hostname, ":", this.port.ToString());
                session = (IWSManSession)webSvcMan.CreateSession(sessionHost, sessionFlags, cnctOptions);
            }
            catch (COMException ce)
            {
                this.HandleWSMANException(ce, queryClass, webSvcMan);
            }

            return session;
        }

        /// <summary>
        /// Handle a WSMAN Exception
        /// </summary>
        /// <param name="ce">The exception class being handled</param>
        /// <param name="queryClass">The query class</param>
        /// <param name="webSvcMan">The WSMAN interface class</param>
        private void HandleWSMANException(COMException ce, string queryClass, WSManClass webSvcMan)
        {
            if (this.mcfContext != null)
            {
                this.mcfContext.Trc("hostname=" + this.hostname);
                this.mcfContext.Trc("username=" + this.username);
                this.mcfContext.Trc("password=" + this.password);
                this.mcfContext.Trc("queryClass=" + queryClass);
                this.mcfContext.Trc("Error: " + ce.Message);
                this.mcfContext.Trc("WSMan: " + webSvcMan.Error);
                throw new VarAbort("WSMAN query failed", ce);
            }
            else
            {
                throw ce;
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}
