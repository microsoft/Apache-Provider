//-----------------------------------------------------------------------
// <copyright file="HostResolutionTask.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System.Diagnostics;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;

    /// <summary>
    /// Represents the Host Resolution Task found in the Unix library MP.
    /// </summary>
    public class HostResolutionTask : Task, IHostResolutionTask
    {
        /// <summary>
        /// Handle for tracing.
        /// </summary>
        private static TraceSource trace = new TraceSource("Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions");

        /// <summary>
        /// Initializes a new instance of the HostResolutionTask class.
        /// </summary>
        public HostResolutionTask()
            : base("Microsoft.Unix.HostEntryResolution.Task")
        {
        }

        /// <summary>
        /// Gets or sets the NameOrIPToResolve overrideable parameter of the task.
        /// </summary>
        public string NameOrIPToResolve { get; set; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="group">ManagementGroup to execute on.</param>
        /// <param name="managementActionPoint">The mangement action point to execute on.</param>
        /// <returns>The results of the host resolution.</returns>
        public HostResolutionTaskResult Execute(IManagementGroupConnection group, IManagedObject managementActionPoint)
        {
            this.OverrideParameter("NameOrIPToResolve", this.NameOrIPToResolve);
            trace.TraceEvent(TraceEventType.Information, 23, "Executing Host Resolution task for name or IP '{0}'.", this.NameOrIPToResolve);
            string result = DoExecute(group, managementActionPoint);
            trace.TraceEvent(TraceEventType.Information, 24, "Done executing Host Resolution task for name or IP '{0}'.", this.NameOrIPToResolve);
            return new HostResolutionTaskResult(result, this.NameOrIPToResolve);
        }
    }
}
