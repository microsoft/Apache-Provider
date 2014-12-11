// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidCredentialException.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Exceptions
{
    using System;

    [Serializable]
    public class InvalidCredentialException : Exception
    {
        public InvalidCredentialException(string validationError)
            : base(validationError)
        {
        }
    }
}

