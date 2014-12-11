//-----------------------------------------------------------------------
// <copyright file="SSHTaskResult.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Xml;

    /// <summary>
    /// Encapsulates the result of an SSH task invocation.
    /// </summary>
    public class SSHTaskResult : IRemoteCmdTaskResult
    {
        /// <summary>
        /// Build a SSHTaskResult for a task that never even began execution (unable to retrive maintainence account, for example).
        /// </summary>
        /// <param name="exitCode">Exit code to return (non-zero would indicate a failure</param>
        /// <param name="exceptionMessage">Exception message to include in the task result</param>
        public SSHTaskResult(int exitCode, string exceptionMessage)
        {
            this.ExitCode = exitCode;
            this.ExceptionMessage = exceptionMessage;
            this.StdOut = this.StdErr = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the SSHTaskResult class.
        /// </summary>
        /// <param name="xmlData">XML string containing the task output.</param>
        ////  
        //  The following is example XML for an SSH request where the request and
        //  the remote command were both successful:
        //  
        //      <DataItem type=\"Microsoft.SSH.SSHCommandData\" time=\"2010-05-20T10:18:09.8587592-07:00\" sourceHealthServiceId=\"A60DA4C8-1308-E006-1C36-28A431963039\">
        //      <SSHCommandData>
        //      <stdout>/tmp/scx-root/GetOSVersion.sh</stdout>
        //      <stderr></stderr>
        //      <returnCode>0</returnCode>
        //      </SSHCommandData>
        //      </DataItem>
        //  
        //  The example demonstrates an SSH request where the request was a
        //  success, but the remote command failed:
        //  
        //      <DataItem type=\"Microsoft.SSH.SSHCommandData\" time=\"2010-05-20T10:18:09.8587592-07:00\" sourceHealthServiceId=\"A60DA4C8-1308-E006-1C36-28A431963039\">
        //      <SSHCommandData>
        //      <stdout>rm:&#32;cannot&#32;remove&#32;`/tmp/scx-root/GetOSVersion.sh&apos;:&#32;Permission&#32;denied&#10;mkdir:&#32;cannot&#32;create&#32;directory&#32;`/tmp/scx-root/&apos;:&#32;File&#32;exists</stdout>
        //      <stderr></stderr>
        //      <returnCode>1</returnCode>
        //      </SSHCommandData>
        //      </DataItem>
        //
        //     Note: The <stdout> tag above contains the following unencoded
        //           inner text, with a single LF sequence separating the two
        //           lines and no trailing line ending:
        //
        //               rm: cannot remove `/tmp/scx-root/GetOSVersion.sh': Permission denied
        //               mkdir: cannot create directory `/tmp/scx-root/': File exists
        //  
        //  The example demonstrates an exception during the SSH request:
        //  
        //      <DataItem type=\"Microsoft.SSH.SSHCommandData\" time=\"2010-05-20T10:18:09.8587592-07:00\" sourceHealthServiceId=\"A60DA4C8-1308-E006-1C36-28A431963039\">
        //      <SSHCommandData>
        //      <stdout></stdout>
        //      <stderr></stderr>
        //      <exception returnCode=\"-1073479095\">An&#32;exception&#32;(-1073479095)&#32;caused&#32;the&#32;SSH&#32;command&#32;to&#32;fail&#32;-&#32;Bad&#32;passphrase&#13;&#10;</exception>
        //      </SSHCommandData>
        //      </DataItem>
        //
        //     Note: The <exception> tag above contains the following unencoded
        //           inner text, with a trailing CR LF line ending:
        //
        //               An exception (-1073479095) caused the SSH command to fail - Bad passphrase
        ////   
        public SSHTaskResult(string xmlData)
        {
            const int TRACE_ID = 300;
            this.ExceptionMessage = string.Empty;

            Trace.TraceEvent(TraceEventType.Information, TRACE_ID, "Entering SSHTaskResult constructor");
            
             if (String.IsNullOrWhiteSpace(xmlData))
             {
                 Trace.TraceEvent(TraceEventType.Critical, TRACE_ID, "Throwing ArgumentNullException from SSHTaskResult; missing xmlData.");

                 throw new ArgumentNullException("xmlData", Strings.SshResponseIsNullOrEmpty);
             }

             Trace.TraceEvent(TraceEventType.Information, TRACE_ID, "Loading xmlData.");
            
             XmlDocument xmlResult = new XmlDocument();
             xmlResult.LoadXml(xmlData);

             Trace.TraceEvent(TraceEventType.Information, TRACE_ID, "Extracting values from xmlData.");
            
             XmlNode parentNode = xmlResult.SelectSingleNode("/DataItem/SSHCommandData");
             if (parentNode == null)
             {
                 string message = String.Format(CultureInfo.CurrentCulture, Strings.SSHTaskResponseIsNotExpectedXML, xmlData);

                 Trace.TraceEvent(TraceEventType.Critical, TRACE_ID, "Throwing InvalidSSHTaskResponseException from SSHTaskResult: {0}", message);
            
                 throw new InvalidSSHTaskResponseException(message);
             }

             Trace.TraceEvent(TraceEventType.Information, TRACE_ID, "Extracting values from xmlData:\n" + xmlData);

            string nodeName = "stdout";
            string nodeValue = string.Empty;
            XmlNode childNode = parentNode.SelectSingleNode(nodeName);
            if (childNode != null)
            {
                nodeValue = childNode.InnerText;
            }

            Trace.TraceEvent(TraceEventType.Information, TRACE_ID, "Extracted '{0}': \"{1}\"", nodeName, nodeValue);
            this.StdOut = nodeValue;

            nodeName = "stderr";
            nodeValue = string.Empty;
            childNode = parentNode.SelectSingleNode(nodeName);
            if (childNode != null)
            {
                nodeValue = childNode.InnerText;
            }

            Trace.TraceEvent(TraceEventType.Information, TRACE_ID, "Extracted '{0}': \"{1}\"", nodeName, nodeValue);
            this.StdErr = nodeValue;

            nodeName = "returnCode";
            string returnCodeText = null;
            XmlNode returnCodeNode = parentNode.SelectSingleNode(nodeName);

            nodeName = "exception";
            string exceptionText = null;
            XmlNode exceptionNode = parentNode.SelectSingleNode(nodeName);

            // <exception> and <returnCode> are mutually exclusive;
            // however, at least one element MUST be in the response
            if (((exceptionNode != null) && (returnCodeNode != null)) ||
                ((exceptionNode == null) && (returnCodeNode == null)))
            {
                string message = String.Format(CultureInfo.CurrentCulture, Strings.SSHTaskResponseIsInvalid, xmlData);

                Trace.TraceEvent(TraceEventType.Critical, TRACE_ID, "Throwing InvalidSSHTaskResponseException from SSHTaskResult: " + message);

                throw new InvalidSSHTaskResponseException(message);
            }

            if (returnCodeNode != null)
            {
                returnCodeText = returnCodeNode.InnerText;
                Trace.TraceEvent(TraceEventType.Information, TRACE_ID, "Extracted '{0}': \"{1}\"", nodeName, returnCodeText);

                this.ExitCode = int.Parse(returnCodeText);
            }
            else
            {
                exceptionText = exceptionNode.InnerText;
                Trace.TraceEvent(TraceEventType.Information, TRACE_ID, "Extracted '{0}': \"{1}\"", nodeName, exceptionText);

                if (String.IsNullOrWhiteSpace(exceptionText))
                {
                    exceptionText = "(No additional information available.)";
                }

                this.ExceptionMessage = exceptionText;

                // assume there won't be a returnCode attribute
                this.ExitCode = 1;

                if (exceptionNode.Attributes != null)
                {
                    nodeName = "returnCode";
                    returnCodeNode = exceptionNode.Attributes[nodeName];
                    if (returnCodeNode != null)
                    {
                        returnCodeText = returnCodeNode.InnerText;
                        Trace.TraceEvent(TraceEventType.Information, TRACE_ID, "Extracted '{0}': \"{1}\"", nodeName, returnCodeText);

                        this.ExitCode = int.Parse(returnCodeText);
                    }
                }
            }

            Trace.TraceEvent(TraceEventType.Information, TRACE_ID, "Leaving SSHTaskResult (" + returnCodeText + ").");
        }

        /// <summary>
        /// Gets the exception message from task invocation.
        /// </summary>
        public string ExceptionMessage { get; private set; }

        /// <summary>
        /// Gets the exit code of task invocation.
        /// </summary>
        public int ExitCode { get; private set; }

        /// <summary>
        /// Gets the standard out output from the remote execution.
        /// </summary>
        public string StdOut { get; private set; }

        /// <summary>
        /// Gets the standard error output from the remote execution.
        /// </summary>
        public string StdErr { get; private set; }

        /// <summary>
        /// Handle for tracing.
        /// </summary>
        private static readonly TraceSource Trace =
            new TraceSource("Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions");
    }
}
