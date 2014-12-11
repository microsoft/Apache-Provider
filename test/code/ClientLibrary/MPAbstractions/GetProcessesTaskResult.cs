// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetProcessesTaskResult.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Xml;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions.Exceptions;

    using Process = Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction.Process;

    /// <summary>
    /// Results of getting the processes for a unix computer.
    /// </summary>
    public class GetProcessesTaskResult : IGetProcessesTaskResult
    {
        #region Constants and Fields

        /// <summary>
        /// Handle for tracing.
        /// </summary>
        private static readonly TraceSource trace =
            new TraceSource("Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions");

        private readonly List<IProcess> processes = new List<IProcess>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the GetProcessesTaskResult class by parsing the result string as XML.
        /// </summary>
        /// <param name="result">XML output of the get processes task.</param>
        public GetProcessesTaskResult(string result)
        {
            trace.TraceEvent(TraceEventType.Information, 13, "Parsing GetProcesses task output xml.");

            var xmlResult = new XmlDocument();

            try
            {
                xmlResult.LoadXml(result);
            }
            catch (Exception)
            {
                throw new InvalidGetProcessesTaskResponseException(result);
            }

            if (!this.TryParseProcessData(xmlResult))
            {
                throw new InvalidGetProcessesTaskResponseException(result);
            }

            trace.TraceEvent(TraceEventType.Information, 14, "Done parsing GetProcesses task output xml.");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the list of processes that were returned from the computer.
        /// </summary>
        public List<IProcess> Processes
        {
            get
            {
                return this.processes;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Attempts to parse the result data as a successful get processes.
        /// </summary>
        /// <param name="xmlResult">XML output of the get processes task.</param>
        /// <returns>true if data could be parsed as a successful get processes.</returns>
        private bool TryParseProcessData(XmlDocument xmlResult)
        {
            XmlNode wsManDataNode =
                xmlResult.SelectSingleNode(
                    "DataItem[@type=\"Microsoft.SystemCenter.WSManagement.WSManData\"]/WsManData");

            if (wsManDataNode != null)
            {
                XmlNodeList xmlNodeList = wsManDataNode.SelectNodes("./*[local-name()='SCX_UnixProcess']");

                if (xmlNodeList != null)
                {
                    foreach (XmlNode node in xmlNodeList)
                    {
                        XmlNode nameNode = node.SelectSingleNode("./*[local-name()='Name']");
                        string name = nameNode != null ? nameNode.InnerText : string.Empty;

                        XmlNode handleNode = node.SelectSingleNode("./*[local-name()='Handle']");
                        string handle = handleNode != null ? handleNode.InnerText : string.Empty;

                        XmlNode modulePathNode = node.SelectSingleNode("./*[local-name()='ModulePath']");
                        string modulePath = modulePathNode != null ? modulePathNode.InnerText : string.Empty;

                        XmlNodeList parametersNodes = node.SelectNodes("./*[local-name()='Parameters']");

                        var parameters = new List<string>();

                        if (parametersNodes != null)
                        {
                            parameters.AddRange(from XmlNode parametersNode in parametersNodes select parametersNode.InnerText);
                        }

                        this.processes.Add(new Process(name, handle, modulePath, parameters));
                    }

                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}