//-----------------------------------------------------------------------
// <copyright file="VerifyEnumerate.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>a-brmill</author>
// <description></description>
// <history>3/25/2009 2:27:44 PM: Created</history>
//-----------------------------------------------------------------------

namespace Apache
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Xml;
    using Infra.Frmwrk;
    using Scx.Test.Common;

    /// <summary>
    /// <para>The VerifyEnumerate test verifies the basic information for an
    /// enumeration query.  The fields are verified against a regular
    /// expression from the MCF variation records.</para>
    /// <para>Specify the class to query in the Custom ID (cid) field
    /// of the variation.</para>
    /// </summary>
    public class VerifyEnumerate : ISetup, IRun, IVerify, ICleanup
    {
        #region Private Fields

        /// <summary>
        /// Name of Posix host
        /// </summary>
        private string hostname;

        /// <summary>
        /// IP Adress of the POSIX host
        /// </summary>
        private string ipaddress;

        /// <summary>
        /// User name for Posix host
        /// </summary>
        private string username;

        /// <summary>
        /// Password for Posix host
        /// </summary>
        private string password;

        /// <summary>
        /// Query class
        /// </summary>
        /// <remarks>E.g., SCX_Agent</remarks>
        private string queryClass;

        /// <summary>
        /// The soft link name
        /// </summary>
        private string softLinkName;

        /// <summary>
        /// The location of the process
        /// </summary>
        private string processLocation;

        /// <summary>
        /// The parameters for the command to run
        /// </summary>
        private string parameters;

        /// <summary>
        /// The return type of winrm enumerate.
        /// </summary>
        private string returnType;

        /// <summary>
        /// Class use to run posix command
        /// </summary>
        private RunPosixCmd rPC;

        /// <summary>
        /// Whether one or more instances of a matching object is required.  If true, then, the test
        /// passes if a matching object is found.  Not all objects must match, but at least one must.
        /// This allows a more specific regular expression to be used for the required instance.
        /// </summary>
        private bool requiredInstance = false;

        /// <summary>
        /// XML fragment returned by query
        /// </summary>
        private List<string> queryXmlResult;

        /// <summary>
        /// XML fragment returned by query the SCX_FileSystemStatisticalInformation
        /// </summary>
        private List<string> fileSytemQueryXmlResult;

        /// <summary>
        /// ArrayList for cache the process check list.
        /// </summary>
        private ArrayList alreadyExistProcess = new ArrayList();

        /// <summary>
        /// Time to wait before attempting retry of query after a failed query
        /// </summary>
        private TimeSpan queryRetryInterval = new TimeSpan(0, 0, 20);

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the VerifyEnumerate class.
        /// </summary>
        public VerifyEnumerate()
        {
            this.queryXmlResult = new List<string>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the value of UserName
        /// </summary>
        public string UserName
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
        /// Gets or sets the value of HostName
        /// </summary>
        public string HostName
        {
            get { return this.hostname; }
            set { this.hostname = value; }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Implements MCF Setup interface.
        /// This function will access the Posix host for the query, and store the results for verification.
        /// </summary>
        /// <param name="mcfContext">MCF context</param>
        public void Setup(IContext mcfContext)
        {
            mcfContext.Trc("Setup entered");

            IVarContext varContext = (IVarContext)mcfContext;

            this.hostname = mcfContext.ParentContext.Records.GetValue("hostname");
            if (String.IsNullOrEmpty(this.hostname))
            {
                throw new VarAbort("hostname not specified");
            }

            this.username = mcfContext.ParentContext.Records.GetValue("username");
            if (String.IsNullOrEmpty(this.username))
            {
                throw new VarAbort("username not specified");
            }

            this.password = mcfContext.ParentContext.Records.GetValue("password");
            if (String.IsNullOrEmpty(this.password))
            {
                throw new VarAbort("password not specified");
            }

            this.queryClass = varContext.CustomID;
            if (String.IsNullOrEmpty(this.queryClass))
            {
                throw new VarAbort("cid field not specified, specify query class in cid");
            }

            if (mcfContext.Records.HasKey("requiredinstance") &&
               mcfContext.Records.GetValue("requiredinstance") == "true")
            {
                this.requiredInstance = true;
            }

            this.ipaddress = new ClientInfo().GetHostIPv4Address(this.hostname);

            mcfContext.Trc(string.Format("Initialized enumeration test against {0}, ipaddr={1} for enumeration query {2}", this.hostname, this.ipaddress, this.queryClass));
            Scx.Test.WSMAN.Common.WSManQuery posixQuery = new Scx.Test.WSMAN.Common.WSManQuery(mcfContext, this.hostname, this.username, this.password);

            int maxTries = 3;
            bool success = false;

            for (int tries = 0; !success && tries < maxTries; tries++)
            {
                try
                {
                    // Query result from provider SCX_FileSystemStatisticalInformation
                    if (mcfContext.Records.HasKey("VerifyFileSystem") &&
                        mcfContext.Records.HasKey("FileSystemQueryClass"))
                    {
                        this.fileSytemQueryXmlResult = new List<string>();
                        string fileSytemQueryClass = mcfContext.Records.GetValue("FileSystemQueryClass");
                        posixQuery.EnumerateScx(out this.fileSytemQueryXmlResult, fileSytemQueryClass);
                    }

                    // If the recordkey haskey 'returnType' do the function EnumerateScx.
                    if (mcfContext.Records.HasKey("returnType"))
                    {
                        this.returnType = mcfContext.Records.GetValue("returnType");
                        posixQuery.EnumerateScx(out this.queryXmlResult, this.queryClass, this.returnType);
                    }
                    else
                    {
                        posixQuery.EnumerateScx(out this.queryXmlResult, this.queryClass);
                    }

                    if (this.queryXmlResult != null && this.queryXmlResult.Count > 0)
                    {
                        success = true;
                    }
                }
                catch (Exception e)
                {
                    mcfContext.Trc("WSMAN Query failed: " + e.Message);
                    success = false;
                }

                if (!success)
                {
                    this.Wait(mcfContext);
                }

                if (!success)
                {
                    throw new VarAbort(string.Format("Client does not respond to WSMAN requests after {0} attempts", maxTries));
                }
            }
        }

        /// <summary>
        /// Implements MCF Run interface
        /// </summary>
        /// <param name="mcfContext">MCF context</param>
        /// <remarks><para>The records will have a name-value pair, with the
        /// enumeration item as the name and the C# regex as its value.
        /// Use multiple entries for multiple checks.</para>
        /// <para>If a record-value pair debugxml-true is present, the
        /// node will be pretty-printed to STDOUT.</para></remarks>
        public void Run(IContext mcfContext)
        {
            mcfContext.Trc("Run entered");
            string debugRecord = null;
            bool requiredInstanceFound = false;
            bool printDebug = false;
            string[] recordKeys = mcfContext.Records.GetKeys();

            // Check records for DebugXML record flag from mcf command line. For example: MCF.exe /m:%VarMap%.xml /debugxml:true
            try
            {
                debugRecord = mcfContext.Framework.GetValue("debugxml");
            }
            catch
            {
                mcfContext.Trc("The value of \'debugxml\' is null.");
            }

            if (string.IsNullOrEmpty(debugRecord) == false)
            {
                printDebug = bool.Parse(debugRecord);
            }

            // Enumerate through context variation records and execute record values as regular expressions
            foreach (string wsmanQuery in this.queryXmlResult)
            {
                XmlDocument queryXmlDoc = new XmlDocument();
                queryXmlDoc.LoadXml(wsmanQuery);
                XmlElement root = queryXmlDoc.DocumentElement;
                bool matchingInstance = true;

                // Pretty-print XML
                if (printDebug == true)
                {
                    XmlTextWriter xmlWriter = new XmlTextWriter(Console.Out);
                    xmlWriter.Formatting = Formatting.Indented;
                    queryXmlDoc.WriteContentTo(xmlWriter);
                    xmlWriter.Flush();
                    Console.WriteLine();
                }

                XmlNodeList nameNodeList = root.GetElementsByTagName("p:Name");
                string xmlDocumentName = nameNodeList.Count > 0 ? nameNodeList[0].InnerText : "unknown";
                mcfContext.Trc("Processing new XML document: " + xmlDocumentName);

                // Test entries in MCF variation map records agains fields returned by WSMAN
                // The WSMAN fields will be start with 'p:','wsa:','wsman:'.
                // WSMAN XML query may return 'parameter' nodes.  These are ignored.
                foreach (string recordKey in recordKeys)
                {
                    // If we want to get the inner text of node which has node name wsman:Selector, we need 2 properties.
                    // so we has 2 properties in such recordkey. Once we read the recordkey, we get 2 properties by splitting it with ','.
                    // So that we need a string array to save those 2 properties.
                    string[] recordKeyForEPR = new string[2];
                    string recordValue = mcfContext.Records.GetValue(recordKey);
                    XmlNodeList nodes = root.GetElementsByTagName(recordKey);

                    // If the recordkey contains "wsman:Selector",we try to get the nodes by the tagname.
                    if (recordKey.Contains("wsman:Selector"))
                    {
                        recordKeyForEPR = recordKey.Split(',');
                        nodes = root.GetElementsByTagName(recordKeyForEPR[0]);
                    }

                    if (recordValue == "_IP_ADDRESS__")
                    {
                        recordValue = this.ipaddress + @"|$^";
                    }

                    if (nodes != null)
                    {
                        // A matching recordKey result in 1 or more XML nodes from search
                        if (nodes.Count > 0)
                        {
                            foreach (XmlNode node in nodes)
                            {
                                System.Text.RegularExpressions.Regex criteria = new Regex(recordValue);
                                if (criteria != null)
                                {
                                    if (recordKeyForEPR[0] != null)
                                    {
                                        if (node.Attributes[0].Value.Equals(recordKeyForEPR[1]))
                                        {
                                            if (criteria.IsMatch(node.InnerText))
                                            {
                                                mcfContext.Trc(recordKey + " passed criteria check");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (criteria.IsMatch(node.InnerText) == true)
                                        {
                                            mcfContext.Trc(recordKey + " passed criteria check");
                                        }
                                        else
                                        {
                                            matchingInstance = false;
                                            mcfContext.Err(string.Format("{0}.{1} with value '{2}' failed criteria check, McfRecord='{3}', regex='{4}'", this.queryClass, node.LocalName, node.InnerText, recordKey, recordValue));
                                            mcfContext.Err("Outer XML=" + node.OuterXml);
                                        }
                                    }
                                }
                                else
                                {
                                    mcfContext.Err("Error: test " + recordKey + "Invalid Regex '" + recordValue + "'");
                                }
                            }
                        }
                        else
                        {
                            // Non-query record-value pairs are ignored
                            if ((recordKey.StartsWith("p:") == true) || (recordKey.StartsWith("wsa:") == true) || (recordKey.StartsWith("wsman:") == true))
                            {
                                matchingInstance = false;
                                mcfContext.Err("Expected to find " + recordKey + " in WSMAN query");
                            }
                        }
                    }
                    else
                    {
                        mcfContext.Alw("'nodes' == null for " + recordKey);
                    }
                }

                if (this.requiredInstance)
                {
                    if (matchingInstance)
                    {
                        requiredInstanceFound = true;
                        break;
                    }
                }
                else
                {
                    if (!matchingInstance)
                    {
                        throw new VarAbort("Values in WSMAN query failed criteria check");
                    }
                }
            }

            if (this.requiredInstance &&
                requiredInstanceFound == false)
            {
                throw new VarAbort("Required instance of WSMAN object not found");
            }
        }

        /// <summary>
        /// Implements MCF Verify interface
        /// </summary>
        /// <param name="mcfContext">MCF context</param>
        public void Verify(IContext mcfContext)
        {
            mcfContext.Trc("Verify entered");
        }

        /// <summary>
        /// Implements MCF Cleanup interface
        /// </summary>
        /// <param name="mcfContext">MCF context</param>
        public void Cleanup(IContext mcfContext)
        {
            mcfContext.Trc("Cleanup entered");
            if (!string.IsNullOrEmpty(this.softLinkName))
            {
                this.RunCommand(string.Format("rm {0}", this.softLinkName));
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Waiting for the process is ready for query.
        /// </summary>
        /// <param name="ctx">MCF Context</param>
        /// <param name="processName">Current process name</param>
        /// <param name="hostname">Current UNIX/Linux machine name</param>
        private static void WaitForProcessReady(IContext ctx, string processName, string hostname)
        {
            RunPosixCmd runPosixCmd = new RunPosixCmd(hostname, "root", "OpsMgr2007R2");

            int index = 1;
            int tryTimes = 15;
            bool isProcessCreated = false;

            while (index < tryTimes)
            {
                string query = "ps -ef|grep " + processName + "|grep -v \"grep\"|awk \'{print $2}\'";
                ctx.Alw(string.Format("Executing commnad {0} to query process ids for {1}...", query, processName));
                runPosixCmd.RunCmd(query);
                string processIDs = runPosixCmd.StdOut.Trim();

                // Kill the process if it is already running
                if (string.IsNullOrEmpty(processIDs))
                {
                    ctx.Alw(string.Format("Can't find process {0} after trying {1}/{2}", processName, index, tryTimes));
                    Thread.Sleep(1000);
                }
                else
                {
                    ctx.Alw(string.Format("Find process {0} after trying {1}/{2}.", processName, index, tryTimes));
                    isProcessCreated = true;
                    break;
                }

                index++;
            }

            if (!isProcessCreated)
            {
                throw new VarFail(string.Format("Failed to create process \"{0}\".", processName));
            }
        }

        /// <summary>
        /// Kill the process with its name
        /// </summary>
        /// <param name="ctx">MCF context</param>
        /// <param name="processName">Current process name</param>
        /// <param name="hostname">Current UNIX/Linux machine's hostname</param>
        private static void KillProcess(IContext ctx, string processName, string hostname)
        {
            RunPosixCmd runPosixCmd = new RunPosixCmd(hostname, "root", "OpsMgr2007R2");
            string query = "ps -ef|grep " + processName + "|grep -v \"grep\"|awk \'{print $2}\'";
            ctx.Alw(string.Format("Executing commnad {0} to query process ids for {1}...", query, processName));
            runPosixCmd.RunCmd(query);
            string processIDs = runPosixCmd.StdOut.Trim();

            // Kill the process if it is already running
            if (!string.IsNullOrEmpty(processIDs))
            {
                string[] processIDList = processIDs.Split(new char[] { ' ', ',', '\n' });
                foreach (string processID in processIDList)
                {
                    try
                    {
                        ctx.Alw(string.Format("Find the process ID {0} of process {1}, kill this process...", processID, processName));
                        runPosixCmd.RunCmd(string.Format("kill {0}", processID));
                    }
                    catch (Exception e)
                    {
                        ctx.Alw(string.Format("Fail to kill process {0} with error '{1}'", processID, e.Message));
                    }
                }
            }
            else
            {
                ctx.Alw(string.Format("No process exists for {0}", processName));
            }
        }

        /// <summary>
        /// Generic wait method for use to allow the state of the installed agent to stabilize
        /// </summary>
        /// <param name="ctx">Current MCF context</param>
        private void Wait(IContext ctx)
        {
            ctx.Trc(string.Format("Waiting for {0}...", this.queryRetryInterval));
            System.Threading.Thread.Sleep(this.queryRetryInterval);
        }

        /// <summary>
        /// Create soft linik for sleep process
        /// </summary>
        /// <param name="ctx">MCF context interface</param>
        /// <param name="processLocation">The process location in UNIX/Linux machine</param>
        /// <param name="softLinkName">The soft link name</param>
        /// <returns>Return the command executed result</returns>
        private string CreateSoftLink(IContext ctx, string processLocation, string softLinkName)
        {
            string cmd = string.Format("ln -s {0} {1}", processLocation, softLinkName);
            string result = this.RunCommand(cmd);
            return result;
        }

        /// <summary>
        /// Overrride method StartProcess for calling by thread
        /// </summary>
        /// <param name="process">The process name</param>
        private void StartProcess(string process)
        {
            string cmd = string.Format("{0} {1}", process, this.parameters);
            this.rPC = new RunPosixCmd(this.hostname, "root", "OpsMgr2007R2");
            this.rPC.RunCmdInBackGround(cmd);
        }

        /// <summary>
        /// Run remote command
        /// </summary>
        /// <param name="cmd">The command need to be executed</param>
        /// <returns>Command execute result</returns>
        private string RunCommand(string cmd)
        {
            string result = string.Empty;
            this.rPC = new RunPosixCmd(this.hostname, "root", "OpsMgr2007R2");
            try
            {
                this.rPC.RunCmd(cmd);
                result = string.Format("Command executed with output \"{0}\" or Error \"{1}\"", this.rPC.StdOut, this.rPC.StdErr);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("An exception \"{0}\" occured when executing command \"{1}\"", e.Message + ":" + this.rPC.StdOut + ":" + this.rPC.StdErr, cmd));
            }

            return result;
        }

        /// <summary>
        /// Method to verify if the get the expected parameters
        /// </summary>
        /// <param name="ctx">MCF context</param>
        /// <param name="posixQuery">Remove command execute interface</param>
        /// <param name="process">Current process name</param>
        /// <param name="hostname">UNIX/Linux machine name</param>
        private void VerifyProcessAvaliable(IContext ctx, Scx.Test.WSMAN.Common.WSManQuery posixQuery, string process, string hostname)
        {
            int index = 1;

            // Each platform has different time to get the return of the process. so we need more tries to get return.
            int tryTimes = 30;
            string parameter = string.Empty;
            string processName = process.Split('/')[process.Split('/').Length - 1].Trim();

            while (index < tryTimes)
            {
                try
                {
                    posixQuery.EnumerateScx(out this.queryXmlResult, this.queryClass);
                }
                catch (Exception e)
                {
                    ctx.Trc("WSMAN Query failed: " + e.Message);
                }

                foreach (string wsmanQuery in this.queryXmlResult)
                {
                    XmlDocument queryXmlDoc = new XmlDocument();
                    queryXmlDoc.LoadXml(wsmanQuery);
                    XmlElement root = queryXmlDoc.DocumentElement;

                    if (root == null)
                    {
                        throw new VarFail("Current WSMan query should not be null.");
                    }

                    XmlNodeList nodes = root.GetElementsByTagName("p:Parameters");
                    XmlNodeList nameNodeList = root.GetElementsByTagName("p:Name");
                    string xmlDocumentName = nameNodeList.Count > 0 ? nameNodeList[0].InnerText : "unknown";

                    if (xmlDocumentName.Equals(processName))
                    {
                        if (nodes.Count > 0)
                        {
                            parameter = nodes.Cast<XmlNode>().Aggregate(parameter, (current, node) => current + (node.InnerText + " "));
                        }

                        ctx.Trc("Parameter : " + parameter);
                    }
                }

                parameter = parameter.Trim();

                if (!string.IsNullOrEmpty(parameter))
                {
                    ctx.Alw(string.Format("Get the expected parameters {0} for process {1} .", parameter, processName));
                    goto end;
                }
                else
                {
                    ctx.Alw(string.Format("Can't get correct parameters for process {0} after trying {1}/{2}, current parameter is \"{3}\"", processName, index, tryTimes, parameter));
                    Thread.Sleep(5000);
                }

                index++;
            }
        end: ctx.Trc("Process is found");
        }

        #endregion Private Methods

        #endregion Methods
    }
}
