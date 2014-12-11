//-----------------------------------------------------------------------
// <copyright file="IPersistableUnixComputer.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;

    /// <summary>
    /// Represents a new UnixComputer that is not yet (but can be) persisted in the OpsMgr database.
    /// </summary>
    public interface IPersistableUnixComputer : IUnixComputer
    {
        /// <summary>
        /// Save this computer instance to the database.
        /// </summary>
        void Persist();

        /// <summary>
        /// Get the managed object.
        /// </summary>
        /// <returns>
        /// Return the managed object.
        /// </returns>
        IManagedObject GetManagedObject();
    }
}