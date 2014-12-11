// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUnixComputerOperatingSystem.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;

    public interface IUnixComputerOperatingSystem
    {
        #region Properties

        /// <summary>
        /// Gets the name of the computer that matches this Operating System instance.
        /// </summary>
        string ComputerName { get; }

        /// <summary>
        /// Gets the managed object representation of the Operating System instance.
        /// </summary>
        IManagedObject ManagedObject { get; }

        /// <summary>
        /// Gets the name of the operating system.
        /// </summary>
        string Platform { get; }

        /// <summary>
        /// Gets the version of the operating system.
        /// </summary>
        string Version { get; }

        #endregion
    }
}