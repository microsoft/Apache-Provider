//-----------------------------------------------------------------------
// <copyright file="ManagementPackTypeNotFoundException.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Indicates that a requested management pack type could not be found in the database.
    /// </summary>
    [Serializable]
    public class ManagementPackTypeNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the ManagementPackTypeNotFoundException class.
        /// </summary>
        /// <param name="typeName">Management pack type not found.</param>
        public ManagementPackTypeNotFoundException(string typeName)
            : base(string.Format(CultureInfo.CurrentCulture, Strings.MPTypeNotFoundMessage, typeName))
        {
        }
    }
}
