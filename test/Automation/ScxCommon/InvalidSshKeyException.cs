// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidSshKeyException.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
//   Defines the InvalidSshKeyException type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Scx.Test.Common
{
    using System;
    using System.Globalization;
 
    [Serializable]

    /// <summary>
    /// Initializes a new instance of the Exception class.
    /// </summary>
    public class InvalidSshKeyException : Exception
    {     
        /// <summary>
        /// Initializes a new instance of the InvalidSshKeyException class.
        /// </summary>
        /// <param name="fault">ValidationFaul object</param>
        public InvalidSshKeyException(ValidationFault fault)
            : base(Format(fault))
        {
        }

        /// <summary>
        /// Initializes a new instance of the InvalidSshKeyException class.
        /// </summary>
        /// <param name="fault">ValidationFaul object</param>
        /// <param name="inner">Inner Exception object</param>
        public InvalidSshKeyException(ValidationFault fault, Exception inner)
            : base(Format(fault), inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the InvalidSshKeyException class.
        /// </summary>
        /// <param name="lineNumber">Line number which error exists</param>
        public InvalidSshKeyException(int lineNumber)
            : base(Format(ValidationFault.ValidationError, lineNumber))
        {
        }

        /// <summary>
        /// Initializes a new instance of the InvalidSshKeyException class.
        /// </summary>
        /// <param name="lineNumber">Line number which error exists</param>
        /// <param name="inner">Inner Exception object</param>
        public InvalidSshKeyException(int lineNumber, Exception inner)
            : base(Format(ValidationFault.ValidationError, lineNumber), inner)
        {
        }

        /// <summary>
        /// Type of ValidationFault
        /// </summary>
        public enum ValidationFault
        {
            /// <summary>
            /// The enum FileNotFound of ValidationFault.
            /// </summary>
            FileNotFound,

            /// <summary>
            /// The enum UnsupportedVersion of ValidationFault.
            /// </summary>
            UnsupportedVersion,

            /// <summary>
            /// The enum UnsupportedFormatSshV1Rsa of ValidationFault.
            /// </summary>
            UnsupportedFormatSshV1Rsa,

            /// <summary>
            /// The enum UnsupportedFormatOpenSsh of ValidationFault.
            /// </summary>
            UnsupportedFormatOpenSsh,

            /// <summary>
            /// The enum UnsupportedFormatCommercialSsh of ValidationFault.
            /// </summary>
            UnsupportedFormatCommercialSsh,

            /// <summary>
            /// The enum UnrecognizedFormat of ValidationFault.
            /// </summary>
            UnrecognizedFormat,

            /// <summary>
            /// The enum ValidationError of ValidationFault.
            /// </summary>
            ValidationError
        }

        /// <summary>
        /// Get formated error string
        /// </summary>
        /// <param name="fault">ValidationFaul object</param>
        /// <param name="args">Passed in args</param>
        /// <returns>Return the value</returns>>
        private static string Format(ValidationFault fault, params object[] args)
        {
            switch (fault)
            {
                case ValidationFault.UnsupportedVersion:
                    return String.Format(CultureInfo.CurrentCulture, "The SSH key is not a supported version.", args);

                case ValidationFault.UnsupportedFormatSshV1Rsa:
                    return String.Format(CultureInfo.CurrentCulture, "The SSH key was created using SSH version 1. The keys have known vulnerabilities and are not supported.", args);

                case ValidationFault.UnsupportedFormatOpenSsh:
                    return String.Format(
                        CultureInfo.CurrentCulture,
                        "The SSH key was created using the OpenSSH implementation and is not supported.  The key must be converted into a supported format before it can be used.",
                        args);

                case ValidationFault.UnsupportedFormatCommercialSsh:
                    return String.Format(
                        CultureInfo.CurrentCulture,
                        "The SSH key was created by an unsupported SSH implementation. The key must be converted into a supported format before it can be used.",
                        args);

                case ValidationFault.UnrecognizedFormat:
                    return String.Format(CultureInfo.CurrentCulture, "The data passed does not appear to be an SSH key.", args);

                default:
                    return String.Format(CultureInfo.CurrentCulture, "The SSH key contains invalid data at line {0}.", args);
            }
        }
    }
}