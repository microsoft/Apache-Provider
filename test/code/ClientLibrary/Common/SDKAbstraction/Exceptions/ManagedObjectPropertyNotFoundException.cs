//-----------------------------------------------------------------------
// <copyright file="ManagedObjectPropertyNotFoundException.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Exception thrown when a requested managed object property could not be found.
    /// </summary>
    [Serializable]
    public class ManagedObjectPropertyNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the ManagedObjectPropertyNotFoundException class.
        /// </summary>
        /// <param name="name">Name of requested property object.</param>
        public ManagedObjectPropertyNotFoundException(string name)
            : base(string.Format(CultureInfo.CurrentCulture, Strings.ManagedObjectPropertyNotFoundMessage, name))
        {
        }
    }
}