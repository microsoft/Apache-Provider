//-----------------------------------------------------------------------
// <copyright file="InstallableAgents.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.Utilities;

    /// <summary>
    /// Represents a collection of installable agents.
    /// </summary>
    public class InstallableAgents : IInstallableAgents
    {
        /// <summary>
        /// List of all available agents.
        /// </summary>
        private IList<InstallableAgent> agents = new List<InstallableAgent>();

        /// <summary>
        /// Initializes a new instance of the InstallableAgents class.
        /// </summary>
        /// <param name="resultXml">Xml with results from the task invocation containing data about available installable agents.</param>
        public InstallableAgents(string resultXml)
        {
            traceSource.TraceInformation(@"InstallableAgents: XML returned from MS is [{0}]", resultXml);

            string supportedAgentsInnerXml = GetEnumerationPayload(resultXml);

            XmlDocument supportedAgentsDocument = new XmlDocument();
            supportedAgentsDocument.LoadXml(supportedAgentsInnerXml);

            traceSource.TraceInformation(@"InstallableAgents: XML returned from MS is [{0}]", resultXml);

            this.DiscoveryScriptFolder = supportedAgentsDocument.SelectSingleNode("/SupportedAgents/DiscoveryScript").Attributes["Directory"].Value;
            Debug.Assert("GetOSVersion.sh".Equals(supportedAgentsDocument.SelectSingleNode("/SupportedAgents/DiscoveryScript").Attributes["Name"].Value));
            foreach (XmlNode agentNode in supportedAgentsDocument.SelectNodes("/SupportedAgents/Kit"))
            {
                this.agents.Add(new InstallableAgent(agentNode.Attributes["Directory"].Value, agentNode.Attributes["Name"].Value));
            }
        }

        /// <summary>
        /// Peels off the OpsMgr headers from the XML returned by the PS EnumerateAgents task and returns the string 
        /// representation of the data.
        /// </summary>
        /// <param name="resultXml">XML returned from the task</param>
        /// <returns>String with the payload</returns>
        /// The data typically looks like
        ///     <DataItem type="Microsoft.Windows.SerializedObjectData" time="2011-10-13T00:21:33.073298Z" 
        ///           sourceHealthServiceId="b5d5d34a-5f17-182c-94bc-8b182f3a7559">
        ///     <SerializationSettings Type="OpsMgrSerialization" Depth="3" />
        ///       <Description> 
        ///          the rawKitList goes here
        ///       </Description>
        ///     </SerializationSettings>
        ///  </DataItem>
        private string GetEnumerationPayload(string resultXml)
        {
            XmlDocument parsedResult = new XmlDocument();

            parsedResult.LoadXml(resultXml);
            string supportedAgentsInnerText = parsedResult.SelectSingleNode("DataItem[@type=\"Microsoft.Windows.SerializedObjectData\"]/Description").InnerText;

            return supportedAgentsInnerText;
        }

        /// <summary>
        /// Gets the folder where the DiscoveryScript resides.
        /// </summary>
        public string DiscoveryScriptFolder { get; private set; }

        /// <summary>
        /// Trace Source for this class
        /// </summary>
        private static readonly TraceSource traceSource = new TraceSource("Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions");

        /// <summary>
        /// Gets the installation path for a particular supported agent.
        /// </summary>
        /// <param name="agent">Supported agent data for which to retrieve installation path.</param>
        /// <returns>Installable agent data for the supported agent.</returns>
        public IInstallableAgent GetInstallableAgentFor(ISupportedAgent agent)
        {
            var matchingAgents = from installableAgent in this.agents
                                 where installableAgent.IsSupportedBy(agent)
                                 select installableAgent;

            var maxagentList = from maxagent in matchingAgents
                               where
                                   maxagent.AgentVersion ==
                                   matchingAgents.Max(matchingAgent => matchingAgent.AgentVersion)
                               select maxagent;

            if (maxagentList.Any())
            {
                return maxagentList.First<IInstallableAgent>();
            }

            throw new NoMatchingInstallableAgentException();
        }

        /// <summary>
        /// Represents a single installable agent.
        /// </summary>
        private class InstallableAgent : IInstallableAgent
        {
            private string filename;

            /// <summary>
            /// Initializes a new instance of the InstallableAgent class.
            /// </summary>
            /// <param name="agentFolder">Path to installable agent.</param>
            /// <param name="agentFilename">Filename of agent file.</param>
            public InstallableAgent(string agentFolder, string agentFilename)
            {
                this.Path = agentFolder + "\\" + agentFilename;
                this.filename = agentFilename;
                this.ParseAgentVersion();
            }

            private void ParseAgentVersion()
            {
                Regex versionRegex = new Regex(@"^scx-(?<version>\d+\.\d+\.\d+-\d+)");
                Match match = versionRegex.Match(this.filename);
                if (match.Success)
                {
                    this.AgentVersion = new UnixAgentVersion(match.Result("${version}"));
                }
            }

            /// <summary>
            /// Determines if this installable agent is supported by the supplied supported agent.
            /// </summary>
            /// <param name="supportedAgent">Supported agent to match with.</param>
            /// <returns>True if the supported agent supports this installable agent.</returns>
            public bool IsSupportedBy(ISupportedAgent supportedAgent)
            {
                return this.IsSupportedPlatform(supportedAgent) && this.IsSupportedAgentVersion(supportedAgent);
            }

            private bool IsSupportedAgentVersion(ISupportedAgent supportedAgent)
            {
                return this.AgentVersion >= supportedAgent.AgentVersion;
            }

            private bool IsSupportedPlatform(ISupportedAgent supportedAgent)
            {
                string platformIdentifier = string.Format(
                    @".{0}.{1}.{2}",
                    supportedAgent.OS,
                    supportedAgent.KitNameOSVersion,
                    supportedAgent.KitNameArchitecture);
                return this.filename.Contains(platformIdentifier.ToLowerInvariant());
            }

            public string Path { get; private set; }

            public UnixAgentVersion AgentVersion { get; private set; }

        }
    }
}
