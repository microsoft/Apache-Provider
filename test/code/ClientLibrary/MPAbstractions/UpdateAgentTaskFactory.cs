//-----------------------------------------------------------------------
// <copyright file="UpdateAgentTaskFactory.cs" company="Microsoft">
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
    /// Factory class for UpdateAgentTask
    /// </summary>
    public class UpdateAgentTaskFactory : IUpdateAgentTaskFactory
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
        public IUpdateAgentTask CreateTask(IPersistedUnixComputer unixComputer)
        {
            return new UpdateAgentTask(unixComputer);
        }
    }
}
