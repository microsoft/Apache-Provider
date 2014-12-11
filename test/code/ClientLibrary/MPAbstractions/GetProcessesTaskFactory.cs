// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetProcessesTaskFactory.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
//   Default implementation of the IGetProcessesTaskFactory interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System.ComponentModel;

    /// <summary>
    /// Default implementation of the IGetProcessesTaskFactory interface.
    /// </summary>
    public class GetProcessesTaskFactory : IGetProcessesTaskFactory
    {
        /// <summary>
        /// Creates an instance of the GetProcessesTask for a given ProcessTemplateType
        /// </summary>
        /// <param name="type">optional parameter enum type</param>
        /// <returns>Instance of the GetProcessesTask.</returns>
        public IGetProcessesTask CreateTask(ProcessTemplateType type = ProcessTemplateType.ServiceTemplate)
        {
            switch (type)
            {
                case ProcessTemplateType.ServiceTemplate: return new GetProcessesTask();
                case ProcessTemplateType.ProcessTemplate: return new GetProcessTemplateTask();
                default: throw new InvalidEnumArgumentException("type", (int)type, typeof(ProcessTemplateType));
            }
        }
    }
}