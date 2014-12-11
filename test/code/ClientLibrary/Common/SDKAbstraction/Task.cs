//-----------------------------------------------------------------------
// <copyright file="Task.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Security;
    using Microsoft.EnterpriseManagement.Common;
    using Microsoft.EnterpriseManagement.Configuration;
    using Runtime = Microsoft.EnterpriseManagement.Runtime;

    /// <summary>
    /// Represents a management pack task.
    /// </summary>
    public class Task
    {
        /// <summary>
        /// Handle for tracing.
        /// </summary>
        private static TraceSource trace = new TraceSource("Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction");

        /// <summary>
        /// Name of task as found in MP.
        /// </summary>
        private string taskName;

        /// <summary>
        /// Parameter overrides.
        /// </summary>
        private Dictionary<string, string> parameterOverrides = new Dictionary<string, string>();

        /// <summary>
        /// Secure parameter overrides.
        /// </summary>
        private Dictionary<string, SecureString> secureOverrides = new Dictionary<string, SecureString>();

        /// <summary>
        /// Initializes a new instance of the Task class.
        /// </summary>
        /// <param name="taskName">Name of task defined in Management Pack.</param>
        protected Task(string taskName)
        {
            this.taskName = taskName;
        }

        /// <summary>
        /// Changes the name of the task which will be invoked.
        /// </summary>
        /// <param name="taskName">Name of task defined in Management Pack.</param>
        protected void SetTaskName(string taskName)
        {
            this.taskName = taskName;
        }

        /// <summary>
        /// Used by subclasses to set an override for the task.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="value">Parameter value.</param>
        protected void OverrideParameter(string name, string value)
        {
            this.parameterOverrides[name] = value;
        }

        /// <summary>
        /// Used by subclasses to set an override for the task using the data protection API.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="value">Parameter value.</param>
        protected void OverrideParameter(string name, SecureString value)
        {
            this.secureOverrides[name] = value;
        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="managementGroupConnection">ManagementGroupConnection to execute on.</param>
        /// <param name="target">The task will have this managed object as target.</param>
        /// <returns>The results of the task execution as an xml string.</returns>
        protected string DoExecute(IManagementGroupConnection managementGroupConnection, IManagedObject target)
        {
            ManagementPackTask managementPackTask = this.GetMpTask(managementGroupConnection);
            Runtime.TaskConfiguration taskConfig = managementGroupConnection.TaskConfigurationFactory.CreateTaskConfiguration(managementPackTask, this.parameterOverrides, this.secureOverrides);
            EnterpriseManagementObject opsMgrTarget = target.OpsMgrObject;
            trace.TraceEvent(TraceEventType.Information, 5, "Invoking task {0}.", this.taskName);
            Runtime.TaskResult result = managementGroupConnection.TaskRuntime.ExecuteTask(opsMgrTarget, managementPackTask, taskConfig)[0];
            trace.TraceEvent(TraceEventType.Information, 6, "Task {0} completed.", this.taskName);
            ITaskInvocationResult invocationResult = managementGroupConnection.TaskResultFactory.CreateTaskInvocationResult(result);

            CheckForExecutionError(invocationResult);
            return invocationResult.Output;
        }

        /// <summary>
        /// Verifies that an execution error did not occur.
        /// </summary>
        /// <param name="invocationResult">Result from task execution.</param>
        private static void CheckForExecutionError(ITaskInvocationResult invocationResult)
        {
            if (Runtime.TaskStatus.Succeeded != invocationResult.Status)
            {
                int errorCode = 0;
                if (invocationResult.ErrorCode.HasValue)
                {
                    errorCode = invocationResult.ErrorCode.Value;
                }

                throw new TaskInvocationException(errorCode, invocationResult.ErrorMessage);
            }
        }

        /// <summary>
        /// Retrieves the ManagementPackTask from the OpsMgr SDK.
        /// </summary>
        /// <param name="managementGroupConnection">ManagementGroupConnection where task lives.</param>
        /// <returns>ManagementPackTask representing this task.</returns>
        private ManagementPackTask GetMpTask(IManagementGroupConnection managementGroupConnection)
        {
            string criteriaString = string.Format(CultureInfo.InvariantCulture, "Name = '{0}'", this.taskName);
            ManagementPackTaskCriteria criteria = new ManagementPackTaskCriteria(criteriaString);

            trace.TraceEvent(TraceEventType.Information, 7, "Requesting task instances from SDK using query criteria: {0}", criteria.Criteria);

            IList<ManagementPackTask> tasks = managementGroupConnection.TaskConfigManagement.GetTasks(criteria);
            
            trace.TraceEvent(TraceEventType.Information, 8, "Number of task instances returned: {0}", tasks.Count);

            if (1 != tasks.Count)
            {
                throw new TaskNotFoundException(this.taskName);
            }

            ManagementPackTask managementPackTask = tasks[0];
            return managementPackTask;
        }
    }
}
