// ----------------------------------------------------------------------------------------------------
// <copyright file="SshDiscoveryTaskResult.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Xml;
    using System.Xml.Schema;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions.Properties;

    /// <summary>
    ///     Result of script-based (SSH) discovery.
    /// </summary>
    public class SshDiscoveryTaskResult : IOSInformation
    {
        #region Constants and Fields

        /// <summary>
        /// Set of schemas used for validating incoming xml.
        /// </summary>
        private static readonly Lazy<XmlSchemaSet> Schemas = new Lazy<XmlSchemaSet>(LoadSchemas);

        /// <summary>
        /// discovery result xsd schema
        /// </summary>
        private const string DiscoSchema = @"<?xml version='1.0' encoding='utf-8' standalone='yes'?>
                    <xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' elementFormDefault='qualified'>
                        <xs:element name='DiscoveredOS'>
                            <xs:complexType>
                                <!--
                                  The <all> indicator specifies that the child elements can appear
                                  in any order, and that each child element must only occur 1 time.
                                  Using minOccurs sets the required/optional elements.
                                  -->
                                <xs:all>
                                    <xs:element minOccurs='0' name='Hostname' type='xs:string'/>
                                    <xs:element minOccurs='1' name='OSName' type='xs:string'/>
                                    <xs:element minOccurs='0' name='OSAlias' type='xs:string'/>
                                    <xs:element minOccurs='0' name='Version' type='xs:string'/>
                                    <xs:element minOccurs='0' name='Arch' type='xs:string'/>
                                    <xs:element minOccurs='0' name='IsLinux' type='xs:boolean'/>
                                </xs:all>
                            </xs:complexType>
                        </xs:element>
                    </xs:schema>";


        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SshDiscoveryTaskResult class.
        /// </summary>
        /// <remarks>
        /// If <paramref name="stdout "/>is <see langword="null"/>, empty (including all
        /// whitespace), or otherwise invalid/unexpected XML, exceptions are thrown.
        /// </remarks>
        /// <param name="stdout">The entire text returned from the standard output
        /// of an SSH invocation of the GetOSVersion.sh script.</param>
        public SshDiscoveryTaskResult(string stdout)
        {
            const int TRACE_ID = 310;

            Trace.TraceEvent(TraceEventType.Information, TRACE_ID, "Entering SshDiscoveryTaskResult constructor");

            if (String.IsNullOrWhiteSpace(stdout))
            {
                Trace.TraceEvent(
                    TraceEventType.Critical, TRACE_ID, "Throwing ArgumentNullException from SshDiscoveryTaskResult; missing stdout.");

                throw new ArgumentNullException("stdout", Strings.SshResponseIsNullOrEmpty);
            }

            // Strip off stuff before and after the quoted raw XML (from SUDO elevation)
            //
            // Before our RegEx substitution (using SUDO elevation), we may have something like:
            //
            //    Last login: Fri Feb  4 01:08:12 2011 from scxomd-ws7-08.scx.com
            //
            //    su root -c 'sh /tmp/scx-$USER/GetOSVersion.sh; EC=$?; rm -rf /tmp/scx-$USER; exit $EC'
            //    Sun Microsystems Inc.   SunOS 5.10      Generic January 2005
            //    -bash-3.00$ su root -c 'sh /tmp/scx-$USER/GetOSVersion.sh; EC=$?; rm -rf /tmp/scx-$USER exit $EC'
            //    Password: 
            //    <DiscoveredOS><Hostname>scxcrd-sol10-01</Hostname><OSName>SunOS</OSName><OSAlias>Solaris</OSAlias><Version>5.10</Version><Arch>i386</Arch><IsLinux>false</IsLinux></DiscoveredOS>
            //    -bash-3.00$ exit
            //    logout
            //
            // After our RegEx substitution, we'll end up with all of the "junk" in stdout removed, like this:
            //
            //    <DiscoveredOS><Hostname>scxcrd-sol10-01</Hostname><OSName>SunOS</OSName><OSAlias>Solaris</OSAlias><Version>5.10</Version><Arch>i386</Arch><IsLinux>false</IsLinux></DiscoveredOS>
            //
            // In this way, the MSXML parser doesn't have problems parsing <DiscoveredOS>...</DiscoveredOS>.
            //
            // Note: In the case where the GetOSVersion script cannot determine
            // the OSName, it returns "Unknown" and no other nodes.  For example:
            //
            //    <DiscoveredOS><OSName>Unknown</OSName></DiscoveredOS>
            Trace.TraceEvent(TraceEventType.Verbose, TRACE_ID, "Processing raw stdout:\n*****\n{0}\n*****\n", stdout);

            const string PATTERN = ".*(<DiscoveredOS>.*</DiscoveredOS>).*";
            Match m = Regex.Match(stdout, PATTERN, RegexOptions.Singleline);
            if (m.Success == false)
            {
                Trace.TraceEvent(
                    TraceEventType.Critical,
                    TRACE_ID,
                    "Throwing InvalidSSHDiscoveryTaskResponseException from SshDiscoveryTaskResult; stdout does not appear to contain the embeded DiscoveredOS XML.");

                throw new InvalidSSHDiscoveryTaskResponseException(stdout);
            }

            string discoveredOsXml = Regex.Replace(stdout, PATTERN, "$1", RegexOptions.Singleline);

            Trace.TraceEvent(TraceEventType.Verbose, TRACE_ID, "Processing embeded XML:\n*****\n{0}\n*****\n", discoveredOsXml);

            XmlDocument doc = LoadAndValidateXml(discoveredOsXml);

            Trace.TraceEvent(TraceEventType.Information, TRACE_ID, "Extracting values from the XML.");

            string nodeName = "OSName";
            XmlNode childNode = doc.SelectSingleNode("/DiscoveredOS/" + nodeName);
            if (childNode == null)
            {
                Trace.TraceEvent(
                    TraceEventType.Critical,
                    TRACE_ID,
                    "Throwing InvalidSSHDiscoveryTaskResponseException from SshDiscoveryTaskResult; the embeded DiscoveredOS XML must contain an <OSName> node.");

                throw new InvalidSSHDiscoveryTaskResponseException(doc.InnerText);
            }

            this.Name = childNode.InnerText;

            Trace.TraceEvent(TraceEventType.Information, TRACE_ID, "Extracted '{0}': \"{1}\"", nodeName, this.Name);

            bool expectMore = true;
            if (String.Compare(this.Name, "unknown", true) == 0)
            {
                // in the case where the discovery worked but the OSName is
                // unknown, the fact that there is no additional discovery
                // information is expected
                expectMore = false;
            }

            nodeName = "OSAlias";
            childNode = doc.SelectSingleNode("/DiscoveredOS/" + nodeName);
            if (childNode != null)
            {
                this.Alias = childNode.InnerText;

                Trace.TraceEvent(TraceEventType.Information, TRACE_ID, "Extracted '{0}': \"{1}\"", nodeName, this.Alias);
            }
            else if (expectMore)
            {
                Trace.TraceEvent(TraceEventType.Warning, TRACE_ID, "Expected <{0}> node.", nodeName);
            }

            nodeName = "Version";
            childNode = doc.SelectSingleNode("/DiscoveredOS/" + nodeName);
            if (childNode != null)
            {
                this.Version = childNode.InnerText;

                Trace.TraceEvent(TraceEventType.Information, TRACE_ID, "Extracted '{0}': \"{1}\"", nodeName, this.Version);
            }
            else if (expectMore)
            {
                Trace.TraceEvent(TraceEventType.Warning, TRACE_ID, "Expected <{0}> node.", nodeName);
            }

            nodeName = "Arch";
            childNode = doc.SelectSingleNode("/DiscoveredOS/" + nodeName);
            if (childNode != null)
            {
                this.Architecture = childNode.InnerText;

                Trace.TraceEvent(TraceEventType.Information, TRACE_ID, "Extracted '{0}': \"{1}\"", nodeName, this.Architecture);
            }
            else if (expectMore)
            {
                Trace.TraceEvent(TraceEventType.Warning, TRACE_ID, "Expected <{0}> node.", nodeName);
            }

            nodeName = "IsLinux";
            childNode = doc.SelectSingleNode("/DiscoveredOS/" + nodeName);
            if (childNode != null)
            {
                bool isLinux;

                if (bool.TryParse(childNode.InnerText, out isLinux) == false)
                {
                    Trace.TraceEvent(TraceEventType.Error, TRACE_ID, "The <{0}> node contained invalid boolean text.", nodeName);
                    isLinux = false;
                }

                this.IsLinux = isLinux;

                Trace.TraceEvent(TraceEventType.Information, TRACE_ID, "Extracted '{0}': \"{1}\"", nodeName, this.IsLinux);
            }
            else if (expectMore)
            {
                Trace.TraceEvent(TraceEventType.Warning, TRACE_ID, "Expected <{0}> node.", nodeName);
            }

            Trace.TraceEvent(TraceEventType.Information, TRACE_ID, "Leaving SshDiscoveryTaskResult constructor.");
        }

        #endregion Constructors

        /// <summary>
        ///     Gets a value indicating whether the OS is of the Linux constellation of OS.
        /// </summary>
        /// <remarks>
        ///     For some reason, the deployment step needs to know this as it does something
        ///     differently for Linux vs. (all) other Unix systems.
        /// </remarks>
        public bool IsLinux { get; private set; }

        /// <summary>
        /// Gets the architecture as reported by uname.
        /// </summary>
        public string Architecture { get; private set; }

        /// <summary>
        /// Gets the Operating System version.
        /// </summary>
        public string Version { get; private set; }

        /// <summary>
        /// Gets the short Operating System name. E.g. SLES
        /// </summary>
        public string Alias { get; private set; }

        /// <summary>
        /// Gets the Operating System Name.
        /// E.g. AIX, HP-UX, Red Hat Enterprise Linux Server, SUSE Linux Enterprise Server.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Loads and validates the xml text pulled from the standard output of
        /// an SSH invocation of the GetOSVersion.sh script.
        /// </summary>
        /// <param name="discoveredOsXml">A string containing the DiscoveredOS xml.</param>
        /// <returns>A parsed xml document.</returns>
        private static XmlDocument LoadAndValidateXml(string discoveredOsXml)
        {
            const int TRACE_ID = 3110;

            Trace.TraceEvent(TraceEventType.Information, TRACE_ID, "Entering SshDiscoveryTaskResult::LoadAndValidateXml");

            XmlDocument doc = new XmlDocument();

            try
            {
                doc.LoadXml(discoveredOsXml);

                Trace.TraceEvent(TraceEventType.Information, TRACE_ID, "Successfully loaded the XML");
            }
            catch (XmlException ex)
            {
                Trace.TraceEvent(
                    TraceEventType.Critical,
                    TRACE_ID,
                    "Throwing InvalidSSHDiscoveryTaskResponseException from LoadAndValidateXml; failed to load the XML.");

                throw new InvalidSSHDiscoveryTaskResponseException(discoveredOsXml, ex);
            }

            doc.Schemas.Add(Schemas.Value);
            doc.Validate(new ValidationEventHandler((src, ev) =>
                {
                    switch (ev.Severity)
                    {
                        case XmlSeverityType.Error:
                            Trace.TraceEvent(
                                TraceEventType.Critical,
                                TRACE_ID,
                                "Throwing InvalidSSHDiscoveryTaskResponseException from LoadAndValidateXml; failed to validate the XML.");

                            throw new InvalidSSHDiscoveryTaskResponseException(doc.InnerXml, ev.Exception);

                        case XmlSeverityType.Warning:
                            Trace.TraceEvent(
                                TraceEventType.Warning,
                                TRACE_ID,
                                "The XML validation found the following: {0}",
                                ev.Message);
                            break;
                    }
                }));

            Trace.TraceEvent(TraceEventType.Information, TRACE_ID, "Leaving SshDiscoveryTaskResult::LoadAndValidateXml");

            return doc;
        }

        /// <summary>
        /// This static method of SshDiscoveryTaskResult loads schema or set of schemas used
        /// during script-based ssh discovery.
        /// </summary>
        /// <remarks>
        /// While it is harmless (if not wasteful) to invoke this method more than once, it
        /// is intended to be used only by the lazy-initialization of the property
        /// equivalent.
        /// </remarks>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchemaSet">XmlSchemaSet </see>representing
        /// the loaded and compiled collection of XSD for this task.
        /// </returns>
        private static XmlSchemaSet LoadSchemas()
        {
            const int TRACE_ID = 3100;

            Trace.TraceEvent(TraceEventType.Information, TRACE_ID, "Entering SshDiscoveryTaskResult::LoadSchemas");

            XmlSchemaSet schemas;

            using (var stringReader = new StringReader(DiscoSchema))
            {
                XmlReader rdr = XmlReader.Create(stringReader);

                schemas = new XmlSchemaSet();
                schemas.Add(string.Empty, rdr);
            }

            Trace.TraceEvent(TraceEventType.Information, TRACE_ID, "Leaving SshDiscoveryTaskResult::LoadSchemas");

            return schemas;
        }

        /// <summary>
        /// Handle for tracing.
        /// </summary>
        private static readonly TraceSource Trace =
            new TraceSource("Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions");
    }
}
