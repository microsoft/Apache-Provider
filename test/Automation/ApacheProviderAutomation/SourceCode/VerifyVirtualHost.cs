//-----------------------------------------------------------------------
// <copyright file="VerifyVirtualHost.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>v-litin</author>
// <description></description>
// <history>12/01/2014 3:50:44 PM: Created</history>
//--  

namespace Scx.Test.Apache.Provider
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
    class VerifyVirtualHost : ISetup, IRun, IVerify, ICleanup
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
        /// The return type of winrm enumerate.
        /// </summary>
        private string returnType;

        /// <summary>
        /// All Shell scripts location.
        /// </summary>
        private string scriptsLocation = System.Environment.CurrentDirectory;

        /// <summary>
        /// Creating SSL Certification script name.
        /// </summary>
        private string sslCertificationScriptName = "createSSLCertification.sh";

        /// <summary>
        /// Creating ports script name.
        /// </summary>
        private string portScriptName = "createPortforApache.sh";

        /// <summary>
        /// Reverting ports script name.
        /// </summary>
        private string revertScriptName = "revertapacheconf.sh";

        /// <summary>
        /// XML fragment returned by query
        /// </summary>
        private List<string> queryXmlResult;

        /// <summary>
        /// ArrayList for cache the process check list.
        /// </summary>
        private ArrayList alreadyExistProcess = new ArrayList();

        /// <summary>
        /// Time to wait before attempting retry of query after a failed query
        /// </summary>
        private TimeSpan queryRetryInterval = new TimeSpan(0, 0, 20);

        /// <summary>
        /// Create ports command.
        /// </summary>
        private string actionCmd;

        /// <summary>
        /// Clean up ports command.
        /// </summary>
        private string postCmd;

        /// <summary>
        /// Apache helper.
        /// </summary>
        private ApacheHelper apacheHelper;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the VerifyEnumerate class.
        /// </summary>
        public VerifyVirtualHost()
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

            this.apacheHelper = new ApacheHelper(mcfContext.Trc, this.hostname, this.username, this.password);

            this.actionCmd = mcfContext.Records.GetValue("ActionCmd");
            if (!(String.IsNullOrEmpty(this.actionCmd)))
            {
                PosixCopy copyToHost = new PosixCopy(this.hostname, this.username, this.password);
                copyToHost.CopyTo(scriptsLocation + "/" + sslCertificationScriptName, "/tmp/" + sslCertificationScriptName);
                copyToHost.CopyTo(scriptsLocation + "/" + portScriptName, "/tmp/" + portScriptName);
                copyToHost.CopyTo(scriptsLocation + "/" + revertScriptName, "/tmp/" + revertScriptName);
                apacheHelper.RunCmd(this.actionCmd);
            }

            this.queryClass = varContext.CustomID;

            if (String.IsNullOrEmpty(this.queryClass))
            {
                throw new VarAbort("cid field not specified, specify query class in cid");
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

                XmlNodeList nameNodeList = root.GetElementsByTagName("p:InstanceID");
                string xmlDocumentName = nameNodeList.Count > 0 ? nameNodeList[0].InnerText : "unknown";

                mcfContext.Trc("Processing new XML document: " + xmlDocumentName);
                
                string portRecordValue = mcfContext.Records.GetValue("p:InstanceID");
                System.Text.RegularExpressions.Regex criteriaPort = new Regex(portRecordValue);

                if (!criteriaPort.IsMatch(nameNodeList[0].InnerText))
                {
                    continue;
                }
                requiredInstanceFound = true;
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
            }

            if (requiredInstanceFound == false)
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
            this.postCmd = mcfContext.Records.GetValue("PostCmd");
            if (!(String.IsNullOrEmpty(this.postCmd)))
            {
                apacheHelper.RunCmd(this.postCmd);
                apacheHelper.RunCmd("rm -rf /tmp/" + sslCertificationScriptName);
                apacheHelper.RunCmd("rm -rf /tmp/" + portScriptName);
                apacheHelper.RunCmd("rm -rf /tmp/" + revertScriptName);
            }           
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Generic wait method for use to allow the state of the installed agent to stabilize
        /// </summary>
        /// <param name="ctx">Current MCF context</param>
        private void Wait(IContext ctx)
        {
            ctx.Trc(string.Format("Waiting for {0}...", this.queryRetryInterval));
            System.Threading.Thread.Sleep(this.queryRetryInterval);
        }

        #endregion Private Methods

        #endregion Methods
    }
}
