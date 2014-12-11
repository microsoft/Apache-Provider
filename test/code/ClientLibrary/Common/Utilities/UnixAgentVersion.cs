// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnixAgentVersion.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
//   Provides some extension methods enabling System.Version to understand (some? all?)
//   SCX Version numbers ("1.2.3-555")
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.Utilities
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;

    /// <summary>
    ///     Handles SCX Agent Version numbers ("1.2.3-555")
    /// </summary>
    public class UnixAgentVersion : IComparable
    {
        private Version versionRepresentation;

        /// <summary>
        /// Parses an SCX Version String and converts it into a UnixAgentVersion
        /// </summary>
        /// <param name="agentVersionStr">
        /// The SCX Version string is of the form "x.y.z-w"
        /// </param>
        public UnixAgentVersion(string agentVersionStr)
        {
            if (string.IsNullOrEmpty(agentVersionStr))
            {
                throw new ArgumentNullException("agentVersionStr");
            }

            var expression = new Regex(@"^[0-9]+\.[0-9]+\.[0-9]+-[0-9]+$");
            if (!expression.IsMatch(agentVersionStr))
            {
                throw new FormatException();
            }

            this.versionRepresentation = new Version(agentVersionStr.Replace('-', '.'));
        }

        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "{0}.{1}.{2}-{3}",
                this.versionRepresentation.Major,
                this.versionRepresentation.Minor,
                this.versionRepresentation.Build,
                this.versionRepresentation.Revision);
        }

        public static bool TryParse(string verstring, out UnixAgentVersion unixAgentVersion)
        {
            try
            {
                unixAgentVersion = new UnixAgentVersion(verstring);
                return true;
            }
            catch (Exception)
            {
                unixAgentVersion = null;
                return false;
            }
        }

        public static bool operator >(UnixAgentVersion lhs, UnixAgentVersion rhs)
        {
            ThrowIfParametersNotNull(lhs, rhs);
            return lhs.versionRepresentation > rhs.versionRepresentation;
        }

        public static bool operator >=(UnixAgentVersion lhs, UnixAgentVersion rhs)
        {
            ThrowIfParametersNotNull(lhs, rhs);
            return lhs.versionRepresentation >= rhs.versionRepresentation;
        }

        public static bool operator <(UnixAgentVersion lhs, UnixAgentVersion rhs)
        {
            ThrowIfParametersNotNull(lhs, rhs);
            return lhs.versionRepresentation < rhs.versionRepresentation;
        }

        public static bool operator <=(UnixAgentVersion lhs, UnixAgentVersion rhs)
        {
            ThrowIfParametersNotNull(lhs, rhs);
            return lhs.versionRepresentation <= rhs.versionRepresentation;
        }

        public static bool operator ==(UnixAgentVersion lhs, UnixAgentVersion rhs)
        {
            return ReferenceEquals(lhs, rhs) ||
                   (!ReferenceEquals(null, lhs) && lhs.Equals(rhs));
        }

        public static bool operator !=(UnixAgentVersion lhs, UnixAgentVersion rhs)
        {
            return !(lhs == rhs);
        }

        public bool Equals(UnixAgentVersion other)
        {
            if (ReferenceEquals(null, other))
            {
                // if the parameter is null, this isn't equal
                return false;
            }
            
            if (ReferenceEquals(this, other))
            {
                // if the parameter is this, this is equal to itself
                return true;
            }

            // in all other cases, use the underlying implementation
            return Equals(other.versionRepresentation, this.versionRepresentation);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj) ||
                (GetType() != obj.GetType()))
            {
                // if the parameter is null or it isn't a UnixAgentVersion, this isn't equal
                return false;
            }

            // return the class type specific result
            return this.Equals(obj as UnixAgentVersion);
        }

        public override int GetHashCode()
        {
            return this.versionRepresentation.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            var unixAgentVersion = obj as UnixAgentVersion;

            if (unixAgentVersion == null)
            {
                throw new ArgumentNullException(@"obj");
            }

            return this.versionRepresentation.CompareTo(unixAgentVersion.versionRepresentation);
        }

        private static void ThrowIfParametersNotNull(UnixAgentVersion lhs, UnixAgentVersion rhs)
        {
            if (lhs == null)
            {
                throw new ArgumentNullException(@"lhs");
            }

            if (rhs == null)
            {
                throw new ArgumentNullException(@"rhs");
            }
        }
    }
}
