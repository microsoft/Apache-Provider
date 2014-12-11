//-----------------------------------------------------------------------
// <copyright file="ISigningTaskFactory.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    /// <summary>
    /// This interface documents the protocol for creating a new SigningTask.
    /// </summary>
    public interface ISigningTaskFactory
    {
        /// <summary>
        /// Create a new SigningTask.
        /// </summary>
        /// <returns>the new task</returns>
        SigningTask CreateSigningTask();
    }
}
