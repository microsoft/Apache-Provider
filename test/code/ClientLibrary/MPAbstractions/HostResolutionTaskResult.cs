//-----------------------------------------------------------------------
// <copyright file="HostResolutionTaskResult.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System.Net;
    using System.Xml;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks;

    /// <summary>
    /// Results of a host resolution.
    /// </summary>
    public class HostResolutionTaskResult
    {
        /// <summary>
        /// Initializes a new instance of the HostResolutionTaskResult class by parsing the result string as XML.
        /// </summary>
        /// <param name="result">XML output of the host resolution task.</param>
        /// <param name="targetName">The provided target host name.</param>
        public HostResolutionTaskResult(string result, string targetName)
        {
            XmlDocument xmlResult = new XmlDocument();
            xmlResult.LoadXml(result);

            if (!this.TryParseHostData(xmlResult, targetName))
            {
                this.ThrowOnErrorResult(xmlResult);
                throw new InvalidHostResolutionTaskResponseException(result);
            }
        }

        /// <summary>
        /// Gets the host resolution data retrieved by the task.
        /// </summary>
        public IPHostEntry HostEntry { get; private set; }

        /// <summary>
        /// Attempts to parse the result data as a successful host resolution.
        /// </summary>
        /// <param name="xmlResult">XML output of the host resolution task.</param>
        /// <param name="targetName">The provided taget host name.</param>
        /// <returns>true if data could be parsed as a successful host resolution.</returns>
        private bool TryParseHostData(XmlDocument xmlResult, string targetName)
        {
            if ("0" != xmlResult.SelectSingleNode("/DataItem/StatusCode").InnerText)
            {
                return false;
            }

            this.HostEntry = new IPHostEntry();

            this.HostEntry.HostName = xmlResult.SelectSingleNode("/DataItem/NetworkName").InnerText;
            var ipAdressText = xmlResult.SelectSingleNode("/DataItem/IPAddress").InnerText;
            IPAddress address = IPAddress.Parse(ipAdressText);
            IPAddress ip;

            if (!IPAddress.TryParse(targetName, out ip))
            {
                var errorMessage = string.Format(Strings.DnsMismatchLookupException, targetName, ipAdressText, this.HostEntry.HostName);

                // if A.scx.com does not match B or B.scx.com
                if (!(this.HostEntry.HostName).ToLower().StartsWith(targetName.ToLower()))
                {
                    throw new DnsLookupMismatchException(errorMessage);
                }

                // If AA.scx.com does not match A
                var hostNameSplits = (this.HostEntry.HostName).ToLower().Split('.');
                var targetSplits = targetName.ToLower().Split('.');

                if (targetSplits.Length == 1 && hostNameSplits[0] != targetSplits[0])
                {
                    throw new DnsLookupMismatchException(errorMessage);
                }

                // If A.scx.com.cn does not match A.scx.com
                if (targetSplits.Length != 1 && hostNameSplits.Length != targetSplits.Length)
                {
                    throw new DnsLookupMismatchException(errorMessage);
                }

                // If A.scx.comn does not match A.scx.com
                if (hostNameSplits.Length == targetSplits.Length)
                {
                    for(int i = 0; i < hostNameSplits.Length; i++)
                    {
                        if (hostNameSplits[i] != targetSplits[i])
                        {
                            throw new DnsLookupMismatchException(errorMessage);
                        }
                    }
                }
            }

            this.HostEntry.AddressList = new IPAddress[] { address };
            return true;
        }

        /// <summary>
        /// If result is successfully parsed as an error result, this method will throw the data as an exception.
        /// </summary>
        /// <param name="xmlResult">XML to parse for error result.</param>
        private void ThrowOnErrorResult(XmlDocument xmlResult)
        {
            var messageNode = xmlResult.SelectSingleNode("/DataItem/StatusMessage");
            if (null != messageNode)
            {
                throw new HostResolutionFailedException(messageNode.InnerText);
            }
        }
    }
}
