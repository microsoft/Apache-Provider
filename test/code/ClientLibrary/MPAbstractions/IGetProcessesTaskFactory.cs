// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGetProcessesTaskFactory.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    /// <summary>
    /// Abstract factory for GetProcessesTask.
    /// </summary>
    public interface IGetProcessesTaskFactory
    {
        #region Public Methods

        /// <summary>
        /// Creates a new instance of a GetProcessesTask.
        /// </summary>
        /// <param name="type">enum value of ProcessTemplateType</param>
        /// <returns>
        /// A new instance of a GetProcessesTask or GetProcessTemplateTask, depending upon whether the input ProcessTemplateType is ServiceTemplate or ProcessTemplate.
        /// </returns>
        IGetProcessesTask CreateTask(ProcessTemplateType type = ProcessTemplateType.ServiceTemplate);

        #endregion
    }
}