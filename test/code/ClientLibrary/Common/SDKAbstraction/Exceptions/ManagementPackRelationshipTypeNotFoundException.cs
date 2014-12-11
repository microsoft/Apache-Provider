//-----------------------------------------------------------------------
// <copyright file="ManagementPackRelationshipTypeNotFoundException.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Indicates that an attempt to find a management pack relationship type in the SDK failed.
    /// </summary>
    [Serializable]
    public class ManagementPackRelationshipTypeNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the ManagementPackRelationshipTypeNotFoundException class.
        /// </summary>
        /// <param name="typeName">Name of relationship type that could not be found.</param>
        public ManagementPackRelationshipTypeNotFoundException(string typeName)
            : base(string.Format(CultureInfo.CurrentCulture, Strings.RelationshipTypeNotFoundMessage, typeName))
        {
        }
    }
}
