// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidScopeSpecificationException.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
//   Defines the InvalidScopeSpecificationException type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System;

    [Serializable]
    public class InvalidScopeSpecificationException : Exception
    {
        public InvalidScopeSpecificationException(string validationError) : base(validationError)
        {
        }
    }
}