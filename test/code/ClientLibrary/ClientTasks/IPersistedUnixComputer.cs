//-----------------------------------------------------------------------
// <copyright file="IPersistedUnixComputer.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using Common.SDKAbstraction;

    /// <summary>
    /// Represents a unix computer that has been retrieved from the OpsMgr database.
    /// </summary>
    public interface IPersistedUnixComputer : IUnixComputer
    {
        /// <summary>
        /// Save changes to the database.
        /// </summary>
        void Update();

        void UpdateMAPCache();

        /// <summary>
        /// Unmanages the unix computer represented by this object.
        /// </summary>
        void Unmanage();

        IManagedObject ManagementActionPoint { get; set; }

        /// <summary>
        /// Type of unix computer. Eg. Microsoft.Linux.SLES.10.Computer
        /// </summary>
        string UnixComputerType { get; }

        /// <summary>
        /// Gets the managed object representation of the unix computer.
        /// </summary>
        IManagedObject ManagedObject { get; }

        /// <summary>
        /// Gets the string that identifies the platform of this computer in the Management Pack.
        /// E.g. if the UnixComputerType is "Microsoft.Linux.SLES.10.Computer", the platform identifier is
        /// "Microsoft.Linux.SLES.10".
        /// </summary>
        string ManagementPackPlatformIdentifier { get; }
    }
}