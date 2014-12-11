//-----------------------------------------------------------------------
// <copyright file="UniqueDerivedTypeNotFoundException.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using System;
    using System.Globalization;

    /// <summary>
    /// This exception is thrown when we try to get the most derived type of a managed object and there is not exactly one match.
    /// </summary>
    [Serializable]
    public class UniqueDerivedTypeNotFoundException : Exception
    {
        public UniqueDerivedTypeNotFoundException(string className, int numberOfDerivedTypes)
            : base(string.Format(CultureInfo.CurrentCulture, Strings.UniqueDerivedTypeNotFoundMessage, numberOfDerivedTypes, className))
        {
        }
    }
}