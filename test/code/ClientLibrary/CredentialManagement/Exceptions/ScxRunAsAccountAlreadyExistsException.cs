// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidCredentialException.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Exceptions
{
    using System;


    [Serializable]
    public class ScxRunAsAccountAlreadyExistsException : Exception
    {
        public ScxRunAsAccountAlreadyExistsException(string message)
            : base(message)
        {
        }

        public ScxRunAsAccountAlreadyExistsException(string message, Exception innnerException)
            : base(message, innnerException)
        {
        }
    }
}

