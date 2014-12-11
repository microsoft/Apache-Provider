// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Process.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
//   Defines the Process type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the Process type.
    /// </summary>
    public class Process : IProcess
    {
        /// <summary>
        /// Initializes a new instance of the Process class.
        /// </summary>
        /// <param name="name">
        /// The name of the process
        /// </param>
        /// <param name="handle">
        /// The handle of the process.
        /// </param>
        /// <param name="modulePath">
        /// The module path of the process.
        /// </param>
        /// <param name="parameters">
        /// The parameters of the process.
        /// </param>
        public Process(string name, string handle, string modulePath, IList<string> parameters)
        {
            Name = name;
            Handle = handle;
            ModulePath = modulePath;
            Parameters = parameters;
        }

        /// <summary>
        /// Gets the process name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the process handle.
        /// </summary>
        public string Handle { get; private set; }

        /// <summary>
        /// Gets the process module path.
        /// </summary>
        public string ModulePath { get; private set; }

        /// <summary>
        /// Gets the process parameters.
        /// </summary>
        public IList<string> Parameters { get; private set; }
    }
}