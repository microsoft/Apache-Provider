//-----------------------------------------------------------------------
// <copyright file="IUpdateAgentTaskFactory.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks;

    /// <summary>
    /// Abstract factory for IUpdateAgentTask.
    /// </summary>
    public interface IUpdateAgentTaskFactory
    {
        /// <summary>
        /// Factory method for IUpdateAgentTask.
        /// </summary>
        /// <param name="unixComputer">
        /// The unix Computer to create a task for.
        /// </param>
        /// <returns>
        /// A new IUpdateAgentTask.
        /// </returns>
        IUpdateAgentTask CreateTask(IPersistedUnixComputer unixComputer);
    }
}
