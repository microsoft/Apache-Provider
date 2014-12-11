// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidIpAddressRangeException.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System;

    [Serializable]
    public class InvalidIpAddressRangeException : Exception
    {
        public InvalidIpAddressRangeException(string validationError)
            : base(validationError)
        {
        }
    }
}
