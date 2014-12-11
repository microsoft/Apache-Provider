//-----------------------------------------------------------------------
// <copyright file="UnixComputerNotFoundException.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Thrown when a managed Unix computer can't be found.
    /// </summary>
    [Serializable]
    public class UnixComputerNotFoundException : Exception
    {
        public UnixComputerNotFoundException(string computername, Exception innerException)
            : base(string.Format(CultureInfo.CurrentCulture, Strings.UnixComputerNotFound, computername), innerException)
        {
        }
    }
}