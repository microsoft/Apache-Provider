// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidSshKeyException.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
//   Defines the InvalidSshKeyException type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Exceptions
{
    using System;
    using System.Globalization;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Properties;

    [Serializable]
    public class InvalidSshKeyException : Exception
    {
        public enum ValidationFault
        {
            FileNotFound,
            UnsupportedVersion,
            UnsupportedFormatSshV1Rsa,
            UnsupportedFormatOpenSsh,
            UnsupportedFormatCommercialSsh,
            UnrecognizedFormat,
            MaxKeyLengthExceeded, 
            ValidationError
        }


        public InvalidSshKeyException(ValidationFault fault)
            : base(Format(fault))
        {
        }


        public InvalidSshKeyException(ValidationFault fault, Exception inner)
            : base(Format(fault), inner)
        {
        }


        public InvalidSshKeyException(int lineNumber)
            : base(Format(ValidationFault.ValidationError, lineNumber))
        {
        }


        public InvalidSshKeyException(int lineNumber, Exception inner)
            : base(Format(ValidationFault.ValidationError, lineNumber), inner)
        {
        }


        private static string Format(ValidationFault fault, params object[] args)
        {
            switch (fault)
            {
                case ValidationFault.UnsupportedVersion:
                    return String.Format(CultureInfo.CurrentCulture, Resources.SshKeyValidation_UnsupportedVersion, args);

                case ValidationFault.UnsupportedFormatSshV1Rsa:
                    return String.Format(CultureInfo.CurrentCulture, Resources.SshKeyValidation_UnsupportedFormat_SshV1Rsa, args);

                case ValidationFault.UnsupportedFormatOpenSsh:
                    return String.Format(CultureInfo.CurrentCulture, Resources.SshKeyValidation_UnsupportedFormat_OpenSsh, args);

                case ValidationFault.UnsupportedFormatCommercialSsh:
                    return String.Format(CultureInfo.CurrentCulture, Resources.SshKeyValidation_UnsupportedFormat_CommercialSsh, args);

                case ValidationFault.UnrecognizedFormat:
                    return String.Format(CultureInfo.CurrentCulture, Resources.SshKeyValidation_UnrecognizedFormat, args);
                
                case ValidationFault.MaxKeyLengthExceeded:
                    return String.Format(CultureInfo.CurrentCulture, Resources.SshKeyValidation_MaxKeyLengthExceeded, args);

                default:
                    return String.Format(CultureInfo.CurrentCulture, Resources.SshKeyValidation_ValidationError, args);
            }
        }
    }
}