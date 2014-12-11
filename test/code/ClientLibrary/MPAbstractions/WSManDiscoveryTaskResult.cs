//-----------------------------------------------------------------------
// <copyright file="WSManDiscoveryTaskResult.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;
    using System.Diagnostics;
    using System.Xml;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks;

    /// <summary>
    /// Represents the results of a WSManDiscoveryTask invocation.
    /// </summary>
    public class WSManDiscoveryTaskResult : IWSManDiscoveryTaskResult
    {
        /// <summary>
        /// Handle for tracing.
        /// </summary>
        private static TraceSource trace = new TraceSource("Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions");

        /// <summary>
        /// Initializes a new instance of the WSManDiscoveryTaskResult class.
        /// </summary>
        /// <param name="result">XML with result to wrap.</param>
        /// <param name="targetSystem">Target FQDN.</param>
        public WSManDiscoveryTaskResult(string result, string targetSystem)
        {
            trace.TraceEvent(TraceEventType.Information, 13, "Parsing wsman discovery task output xml.");

            XmlDocument outputXml = new XmlDocument();
            
            try
            {
                outputXml.LoadXml(result);
            }
            catch (XmlException ex)
            {
                throw new InvalidWSManTaskResponseException(result, ex);
            }

            if (!this.TryParseSCXAgentData(outputXml))
            {
                ThrowOnWSManFault(outputXml, targetSystem);
                throw new InvalidWSManTaskResponseException(result);
            }

            trace.TraceEvent(TraceEventType.Information, 14, "Done parsing wsman discovery task output xml.");
        }

        /// <summary>
        /// Gets the Operating System Information reported by the task.
        /// </summary>
        public IOSInformation OSInfo { get; private set; }

        /// <summary>
        /// Gets any agent version discovered.
        /// </summary>
        public string AgentVersion { get; private set; }
        
        /// <summary>
        /// Gets the value of a named sub node.
        /// </summary>
        /// <param name="node">Node to search from.</param>
        /// <param name="name">Name of sub node.</param>
        /// <returns>Inner text of sub node.</returns>
        private static string GetSubNodeValue(XmlNode node, string name)
        {
            return node.SelectSingleNode("./*[local-name()='" + name + "']").InnerText;
        }

        /// <summary>
        /// Try to parse the output xml as a failed WSMan result.
        /// On success it will throw an exception based on the Error and ErrorMessage properties.
        /// </summary>
        /// <param name="outputXml">Task output as xml document.</param>
        /// <param name="targetName">Target FQDN.</param>
        private static void ThrowOnWSManFault(XmlDocument outputXml, string targetName)
        {
            trace.TraceEvent(TraceEventType.Information, 18, "Trying to parse wsman discovery task output as WSManFault data.");
            
            XmlNode wsmanFault = outputXml.SelectSingleNode("//*[local-name()='WSManFault']");
            if (wsmanFault == null)
            {
                trace.TraceEvent(TraceEventType.Error, 19, "Failed to parse wsman discovery task output as WSManFault data.");
                return;
            }

            trace.TraceEvent(TraceEventType.Information, 20, "Successfully parsed wsman discovery task output as WSManFault data.");

            throw CreateException(wsmanFault, targetName);
        }

        /// <summary>
        /// Creates an exception from a wsman fault node.
        /// </summary>
        /// <param name="wsmanFault">WSMan fault node.</param>
        /// <param name="targetName">Target FQDN.</param>
        /// <returns>A new exception.</returns>
        private static Exception CreateException(XmlNode wsmanFault, string targetName)
        {
            string errorMessage = GetErrorMessage(wsmanFault);
            string errorCode = GetErrorCode(wsmanFault);
        
            switch (errorCode)
            {
                case "5":
                    return new WSManAuthenticationErrorException(Strings.WsmanAccessDeniedException);
                case "2150858975":
                    return new WinRMBasicAuthDisabledException(Strings.WinRMBasicAuthDisableException);
                case "2150859193":
                    return new WSManResolutionErrorException(errorMessage);
                case "2150858770":
                    return new WSManNoAgentException(errorMessage);
                case "2150859046":
                    return new WSManHostUnreachableException(errorMessage);
                case "12175":
                    var errorString = String.Format(errorMessage + "\r\n" + Strings.WSManSSLError, targetName);
                    return new CertificateErrorException(errorString);
                default:
                    return new WSManUnknownErrorException(errorMessage);
            }
        }

        /// <summary>
        /// Parses out the error message from a wsman fault xml node.
        /// </summary>
        /// <param name="wsmanFault">XML node to parse.</param>
        /// <returns>Error message.</returns>
        private static string GetErrorMessage(XmlNode wsmanFault)
        {
            XmlNode errorMessageNode = wsmanFault.SelectSingleNode("./*[local-name()='Message']");
            return errorMessageNode.InnerText;
        }

        /// <summary>
        /// Retrieves the error code from a wsman fault node.
        /// </summary>
        /// <param name="wsmanFault">WSMan fault node from which to parse the error code.</param>
        /// <returns>The error code as a string.</returns>
        private static string GetErrorCode(XmlNode wsmanFault)
        {
            XmlAttribute wsmanFaultCodeAttribute = wsmanFault.Attributes["Code"];
            return wsmanFaultCodeAttribute.Value;
        }

        /// <summary>
        /// Try to parse the output xml as a successful SCX_Agent instance.
        /// On success it will fill in all the data properties related to successful discovery.
        /// </summary>
        /// <param name="outputXml">Task output as xml document.</param>
        /// <returns>True if successful.</returns>
        private bool TryParseSCXAgentData(XmlDocument outputXml)
        {
            trace.TraceEvent(TraceEventType.Information, 15, "Trying to parse wsman discovery task output as SCX_Agent data.");
            XmlNode scxAgent = outputXml.SelectSingleNode("//*[local-name()='SCX_Agent']");
            if (null == scxAgent)
            {
                trace.TraceEvent(TraceEventType.Information, 16, "Failed to parse wsman discovery task output as SCX_Agent data. Most likely a failed discovery result.");
                return false;
            }

            this.OSInfo = new WSManOSInformation(scxAgent);

            this.AgentVersion = GetSubNodeValue(scxAgent, "VersionString");

            trace.TraceEvent(TraceEventType.Information, 17, "Successfully parsed wsman discovery task output as SCX_Agent data.");

            return true;
        }

        /// <summary>
        /// Parses OS information from WSMan Agent XML.
        /// </summary>
        private class WSManOSInformation : IOSInformation
        {
            /// <summary>
            /// Initializes a new instance of the WSManOSInformation class.
            /// </summary>
            /// <param name="agentNode">XML node from task output.</param>
            public WSManOSInformation(XmlNode agentNode)
            {
                this.Name = GetSubNodeValue(agentNode, "OSName");
                this.Version = GetSubNodeValue(agentNode, "OSVersion");
                this.Architecture = GetSubNodeValue(agentNode, "UnameArchitecture");
                this.Alias = GetSubNodeValue(agentNode, "OSAlias");
                this.IsLinux = String.Compare(@"Linux", GetSubNodeValue(agentNode, "OSType"), true) == 0;
            }

            /// <summary>
            /// Gets the Operating System Name.
            /// E.g. AIX, HP-UX, Red Hat Enterprise Linux Server, SUSE Linux Enterprise Server.
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// Gets the Operating System version.
            /// </summary>
            public string Version { get; private set; }

            /// <summary>
            /// Gets the architecture as reported by uname.
            /// </summary>
            public string Architecture { get; private set; }

            /// <summary>
            /// Gets the short Operating System name. E.g. SLES
            /// </summary>
            public string Alias { get; private set; }

            /// <summary>
            /// Gets a value indicating whether the OS is of the Linux constellation of OS.
            /// </summary>
            /// <remarks>
            /// The deployment step needs to know this as it invokes a different task depending
            /// on this flag.
            /// <para></para>
            /// <para>WS-Man discovery does not provide this information but it can be deducted from the Type field.
            /// Even if this value were to be wrong, it would be OK in this case as this value is only significant during
            /// deployment, where WS-Man won't succeed anyway. Providing the correct value is a matter of clarity for end user.</para>
            /// </remarks>
            public bool IsLinux { get; private set; }
        }
    }
}
