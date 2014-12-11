// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDiscoveryScope.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
//   Defines the IDiscoveryScope type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    [CLSCompliant(false)]
    public interface IDiscoveryScope : IEnumerable<IPHostEntry>, ICloneable
    {
        string SpecificationString { get; set; }

        [CLSCompliant(false)]
        ushort SshPort { get; set; }
    }
}