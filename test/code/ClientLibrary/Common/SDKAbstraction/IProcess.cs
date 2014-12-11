// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProcess.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using System.Collections.Generic;

    public interface IProcess
    {
        /// <summary>
        /// Gets the process name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the process handle.
        /// </summary>
        string Handle { get; }

        /// <summary>
        /// Gets the process module path.
        /// </summary>
        string ModulePath { get; }

        /// <summary>
        /// Gets the list of process parameters.
        /// </summary>
        IList<string> Parameters { get; }
    }
}