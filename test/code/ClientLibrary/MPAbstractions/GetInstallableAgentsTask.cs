//-----------------------------------------------------------------------
// <copyright file="GetInstallableAgentsTask.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System.Diagnostics;

    using Microsoft.EnterpriseManagement.Common;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions.Properties;

    /// <summary>
    /// Task abstraction for retrieving data about all the agent kits available for installation.
    /// </summary>
    public class GetInstallableAgentsTask : Task
    {
        /// <summary>
        /// Handle for tracing.
        /// </summary>
        private static TraceSource trace = new TraceSource("Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions");

        /// <summary>
        /// Initializes a new instance of the GetInstallableAgentsTask class.
        /// </summary>
        public GetInstallableAgentsTask()
            : base("Microsoft.Unix.EnumerateAvailableAgents.Task")
        {
        }

        /// <summary>
        /// Executes the task on a particular ManagementGroupConnection using targetHost as target.
        /// </summary>
        /// <param name="group">Management group on which to execute the task.</param>
        /// <param name="managementActionPoint">The management action point on which to execute the task.</param>
        /// <returns>Results of the task.</returns>
        public virtual InstallableAgents Execute(IManagementGroupConnection group, IManagedObject managementActionPoint)
        {
            try
            {
                trace.TraceEvent(TraceEventType.Information, 33, "Executing EnumerateAvailableAgents task.");
                string result = DoExecute(group, managementActionPoint);
                trace.TraceEvent(TraceEventType.Information, 34, "Done executing EnumerateAvailableAgents task.");
                return new InstallableAgents(result);
            }
            catch (LocationObjectNotFoundException)
            {
                throw new TaskInvocationException(Resources.GetInstallableAgentsTask_Execute_Unable_to_enumerate_Installable_agent_types);
            }
        }
    }
}
