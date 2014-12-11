// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetProcessTemplateTask.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
//   Represents the GetTemplateProcess found in the Microsoft.Unix.Process.Library MP.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;
    using System.Diagnostics;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;

    /// <summary>
    /// Represents the GetProcessTemplateTask found in the Microsoft.Unix.Process.Library MP.
    /// </summary>
    public class GetProcessTemplateTask : Task, IGetProcessesTask
    {
        /// <summary>
        /// Handle for tracing.
        /// </summary>
        private static TraceSource trace = new TraceSource("Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions");

        public GetProcessTemplateTask()
            : base("Microsoft.Unix.ProcessTemplate.GetProcesses.Task")
        {
        }

        /// <summary>
        /// Gets or sets the TargetSystem overrideable parameter of the task.
        /// </summary>
        public string TargetSystem { get; set; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="managementGroupConnection">
        /// The ManagementGroupConnection on which to execute task.
        /// </param>
        /// <param name="unixComputer">The mangement action point to execute on.</param>
        /// <returns>
        /// Process task execution result.
        /// </returns>
        public IGetProcessesTaskResult Execute(IManagementGroupConnection managementGroupConnection, IManagedObject unixComputer)
        {
            if (managementGroupConnection == null)
            {
                throw new ArgumentNullException("managementGroupConnection");
            }

            if (unixComputer == null)
            {
                throw new ArgumentNullException("unixComputer");
            }

            if (string.IsNullOrWhiteSpace(this.TargetSystem))
            {
                throw new ArgumentException(Strings.GetProcessTemplateTask_Execute_Target_system_must_be_set_before_executing); 
            }
            
            this.OverrideParameter("TargetSystem", this.TargetSystem);
            trace.TraceEvent(TraceEventType.Information, 23, "Executing GetProcessTemplateTask for computer '{0}'.", this.TargetSystem);
            string result = DoExecute(managementGroupConnection, unixComputer);
            trace.TraceEvent(TraceEventType.Information, 24, "Done executing GetProcessTemplateTask for computer '{0}'.", this.TargetSystem);
            return new GetProcessesTaskResult(result);
        }
    }
}