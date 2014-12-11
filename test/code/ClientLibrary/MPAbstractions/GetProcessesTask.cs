// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetProcessesTask.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
//   Represents the GetProcesses task found in the Unix library MP.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;
    using System.Diagnostics;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;

    /// <summary>
    /// Represents the GetProcesses task found in the Unix library MP.
    /// </summary>
    public class GetProcessesTask : Task, IGetProcessesTask
    {
        /// <summary>
        /// Handle for tracing.
        /// </summary>
        private static TraceSource trace = new TraceSource("Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions");

        public GetProcessesTask()
            : base("Microsoft.Unix.GetProcesses.Task")
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
                throw new ArgumentException(Strings.GetProcessesTask_Execute_TargetSystem_must_be_set_before_executing_the_task);
            }
            
            this.OverrideParameter("TargetSystem", this.TargetSystem);
            trace.TraceEvent(TraceEventType.Information, 23, "Executing GetProcesses task for computer '{0}'.", this.TargetSystem);
            string result = DoExecute(managementGroupConnection, unixComputer);
            trace.TraceEvent(TraceEventType.Information, 24, "Done executing GetProcesses task for computer '{0}'.", this.TargetSystem);
            return new GetProcessesTaskResult(result);
        }
    }
}