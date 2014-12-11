//-----------------------------------------------------------------------
// <copyright file="OperatingSystemVersion.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Compares Unix OS versions. We could not use the System.Version class since it doesn't handle as many different 
    /// formats as we need to support.
    /// Examples of supported formats:
    /// 4
    /// 10.1
    /// 6.1.0.0
    /// B.11.23
    /// When compared, both versions have to be of the same format. Anything else doesn't make sense.
    /// </summary>
    public class OperatingSystemVersion
    {
        /// <summary>
        /// Version string decomposed into integer pieces.
        /// </summary>
        private IList<int> version = new List<int>();

        /// <summary>
        /// Initializes a new instance of the OperatingSystemVersion class.
        /// </summary>
        /// <param name="versionString">String representation of the version.</param>
        public OperatingSystemVersion(string versionString)
        {
            var stringParts = versionString.Split('.');
            foreach (string part in stringParts)
            {
                int versionPart;
                if (!int.TryParse(part, out versionPart))
                {
                    versionPart = Convert.ToInt32(part[0]);
                }

                this.version.Add(versionPart);
            }
        }

        /// <summary>
        /// Determines if current instance is older than instance supplied as argument.
        /// </summary>
        /// <param name="other">Version to compare to.</param>
        /// <returns>True if this instance is older than other instance.</returns>
        public bool IsOlderThan(OperatingSystemVersion other)
        {
            var versionPart = this.version.GetEnumerator();
            var otherVersionPart = other.version.GetEnumerator();

            while (versionPart.MoveNext() && otherVersionPart.MoveNext())
            {
                if (versionPart.Current < otherVersionPart.Current)
                {
                    return true;
                }
                else if (versionPart.Current > otherVersionPart.Current)
                {
                    return false;
                }
            }

            return false;
        }
    }
}
