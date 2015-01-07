//-----------------------------------------------------------------------
// <copyright file="TasksHelper.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>a-brdust</author>
// <description></description>
// <history>4/3/2009 11:41:33 AM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Apache.SDK.ApacheSDKHelper
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Microsoft.EnterpriseManagement.Common;
    using Microsoft.EnterpriseManagement.Configuration;
    using Microsoft.EnterpriseManagement.Monitoring;
    using Scx.Test.Common;

    /// <summary>
    /// Allows running OM SDK tasks
    /// </summary>
    public class TasksHelper
    {
        #region Private Fields

        /// <summary>
        /// Contains information about the local OM Server installation
        /// </summary>
        private OMInfo info;

        /// <summary>
        /// Logger function
        /// </summary>
        private ScxLogDelegate logger = ScxMethods.ScxNullLogDelegate;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TasksHelper class.
        /// </summary>
        /// <param name="logger">The log delegate method</param>
        /// <param name="info">Contains information about the local OM Server installation</param>
        public TasksHelper(ScxLogDelegate logger, OMInfo info)
        {
            this.info = info;
            this.logger = logger;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Sets the log delegate method
        /// </summary>
        public ScxLogDelegate Logger
        {
            set
            {
                this.logger = value;
            }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Generate the MonitoringTaskConfiguration for a given monitoring task, overriden according to overrideParams
        /// </summary>
        /// <param name="task">The task to create a MonitoringTaskConfiguration for</param>
        /// <param name="overrideParams">An array of name/value pairs matching the name of a ManagementPackOverrideableParamater with a value</param>
        /// <returns>A completed monitoring task configuration</returns>
        public Microsoft.EnterpriseManagement.Runtime.TaskConfiguration GetMonitoringTaskConfiguration(ManagementPackTask task, Pair<string, string>[] overrideParams)
        {  
            // Use the default task configuration.
            Microsoft.EnterpriseManagement.Runtime.TaskConfiguration monitorTaskConfig = 
                new Microsoft.EnterpriseManagement.Runtime.TaskConfiguration();

            IList<ManagementPackOverrideableParameter> parameters = task.GetOverrideableParameters();
            foreach (Pair<string, string> overrideParam in overrideParams)
            {
                foreach (ManagementPackOverrideableParameter parameter in parameters)
                {
                    if (parameter.Name == overrideParam.First)
                    {
                        monitorTaskConfig.Overrides.Add(
                            new Pair<ManagementPackOverrideableParameter, string>(parameter, overrideParam.Second));
                    }
                }
            }

            return monitorTaskConfig;
        }

        /// <summary>
        /// Get a monitoring task based on its name, for example, Microsoft.Linux.RHEL.5.Agent.Install.Task
        /// </summary>
        /// <param name="taskName">Task name to retrieve</param>
        /// <returns>The MonitoringTask object corresponding to the task name.</returns>
        public ManagementPackTask GetMonitoringTask(string taskName)
        {
            string query = string.Format("Name = '{0}'", taskName);

            ManagementPackTaskCriteria taskCriteria = new ManagementPackTaskCriteria(query);
            IList<ManagementPackTask> tasks =
                this.info.LocalManagementGroup.TaskConfiguration.GetTasks(taskCriteria);
            ManagementPackTask task = null;

            if (tasks.Count == 1)
            {
                task = tasks[0];
            }
            else
            {
                throw new InvalidOperationException("Error! Expected one task with: " + query);
            }

            return task;
        }

        /// <summary>
        /// Get a monitoring class by its name, for example Microsoft.SystemCenter.ManagementServer
        /// </summary>
        /// <param name="monitoringClassName">Monitoring class name to retrieve</param>
        /// <returns>Monitoring class corresponding to the class name</returns>
        public ManagementPackClass GetMonitoringClass(string monitoringClassName)
        {
            ManagementPackClassCriteria criteria = new ManagementPackClassCriteria(string.Format("Name = '{0}'", monitoringClassName));

            IList<ManagementPackClass> classes = this.info.LocalManagementGroup.EntityTypes.GetClasses(criteria);
            if (classes.Count != 1)
            {
                throw new InvalidOperationException(string.Format("{0} monitoring classes found matching {1} (expected 1)", classes.Count, monitoringClassName));
            }

            return classes[0];
        }

        /// <summary>
        /// Return a list of targets matching the given monitoring class and display name, against a task may be executed
        /// </summary>
        /// <param name="monitoringClass">The monitoring class, for example Microsoft.SystemCenter.ManagementServer</param>
        /// <param name="displayName">Display name of the monitoring objects to retrieve</param>
        /// <returns>A list of the monitoring objects in the management group matching the given monitoring class and display name</returns>
        public List<MonitoringObject> GetTaskTargetList(ManagementPackClass monitoringClass, string displayName)
        {
            // Create a MonitoringObject list containing a specific agent (the target of the task).
            List<MonitoringObject> targets = new List<MonitoringObject>();

            MonitoringObjectCriteria targetCriteria =
                new MonitoringObjectCriteria(string.Format("DisplayName = '{0}'", displayName), monitoringClass);
            targets.AddRange(this.info.LocalManagementGroup.EntityObjects.GetObjectReader<MonitoringObject>(targetCriteria, ObjectQueryOptions.Default));
            if (targets.Count != 1)
            {
                throw new InvalidOperationException(string.Format("{0} monitoring objects found matching {1} (expected 1)", targets.Count, this.info.OMServer));
            }

            return targets;
        }

        /// <summary>
        /// Execute a monitoring task
        /// </summary>
        /// <param name="targetList">List of target MonitoringObject, for example the computer object representing the remote client</param>
        /// <param name="task">The MonitoringTask, for example Microsoft.Linux.RHEL.5.Agent.Install.Task</param>
        /// <param name="config">The MonitoringTaskConfiguration object, containing any task overrides</param>
        /// <returns>A collection of monitoring task results</returns>
        public IList<Microsoft.EnterpriseManagement.Runtime.TaskResult> RunTask(
            List<MonitoringObject> targetList,
            ManagementPackTask task,
            Microsoft.EnterpriseManagement.Runtime.TaskConfiguration config)
        {
            return this.RunTask(targetList, task, config, true);
        }

        /// <summary>
        /// Execute a monitoring task
        /// This method be used to run some specified task, when there no task result output, e.g. output be null, you can use this method
        /// </summary>
        /// <param name="targetList">List of target MonitoringObject, for example the computer object representing the remote client</param>
        /// <param name="task">The MonitoringTask, for example Microsoft.Linux.RHEL.5.Agent.Install.Task</param>
        /// <param name="config">The MonitoringTaskConfiguration object, containing any task overrides</param>
        /// <param name="verifyTaskResult">Verify task result, true if the task Outup isn't null; otherwise, false.</param>
        /// <returns>A collection of monitoring task results</returns>
        public IList<Microsoft.EnterpriseManagement.Runtime.TaskResult> RunTask(
            List<MonitoringObject> targetList, 
            ManagementPackTask task, 
            Microsoft.EnterpriseManagement.Runtime.TaskConfiguration config, 
            bool verifyTaskResult = true)
        {
            // 30 seconds.
            int threadSleepTime = 30 * 1000;
            int maxTries = 6;
            bool success = false;

            // Run the task.
            this.logger(string.Format("Starting task {0} on the following target: {1}", task.Name, targetList[0].DisplayName));

            IList<Microsoft.EnterpriseManagement.Runtime.TaskResult> results = new List<Microsoft.EnterpriseManagement.Runtime.TaskResult>();

            for (int tries = 0; !success && tries < maxTries; tries++)
            {
                System.Threading.Thread.Sleep(threadSleepTime);

                results = this.info.LocalManagementGroup.TaskRuntime.ExecuteTask<MonitoringObject>(targetList, task, config);

                if (results.Count == 1 &&
                    results[0].Status == Microsoft.EnterpriseManagement.Runtime.TaskStatus.Succeeded)
                {
                    success = true;
                    if (verifyTaskResult)
                    {
                        success &= !results[0].Output.Contains("WSManFault");
                    }
                }

                this.logger(string.Format("RunTask: tries={0}, success={1}", tries, success));
            }

            if (results.Count == 0)
            {
                throw new InvalidOperationException("Failed to return any results from task: " + task.Name);
            }

            for (int i = 0; i < results.Count; i++)
            {
                if (results[i].Status != Microsoft.EnterpriseManagement.Runtime.TaskStatus.Succeeded)
                {
                    string taskOutput = verifyTaskResult ? results[i].Output : string.Empty;
                    throw new InvalidOperationException(string.Format("Task result[{0}] does not indicate success: {1}.  Output: {2}", i, task.Name, taskOutput));
                }
            }

            foreach (Microsoft.EnterpriseManagement.Runtime.TaskResult result in results)
            {
                string taskOutput = verifyTaskResult ? result.Output : string.Empty;
                this.logger("RunTask result: " + taskOutput);
            }

            return results;
        }

        /// <summary>
        /// Execute a monitoring task which should failed
        /// This method be used to run some specified task, when there no task result output, e.g. output be null, you can use this method
        /// </summary>
        /// <param name="targetList">List of target MonitoringObject, for example the computer object representing the remote client</param>
        /// <param name="task">The MonitoringTask, for example Microsoft.Linux.RHEL.5.Agent.Install.Task</param>
        /// <param name="config">The MonitoringTaskConfiguration object, containing any task overrides</param>
        /// <param name="failedTask">Expected that the task failed.</param>
        /// <returns>A collection of monitoring task results</returns>
        public IList<Microsoft.EnterpriseManagement.Runtime.TaskResult> RunFailedTask(
            MonitoringObject computerObject,
            string taskName,
            bool failedTask = true)
        {
            ManagementPackTask task = this.GetMonitoringTask(taskName);
            // Use the default task configuration.
            Microsoft.EnterpriseManagement.Runtime.TaskConfiguration config = new Microsoft.EnterpriseManagement.Runtime.TaskConfiguration();

            // Set the target
            List<MonitoringObject> targets = new List<MonitoringObject>();
            targets.Add(computerObject);

            // 30 seconds.
            int threadSleepTime = 30 * 1000;
            int maxTries = 3;
            bool success = false;

            // Run the task.
            this.logger(string.Format("Starting task {0} on the following target: {1}", task.Name, targets[0].DisplayName));

            IList<Microsoft.EnterpriseManagement.Runtime.TaskResult> results = new List<Microsoft.EnterpriseManagement.Runtime.TaskResult>();

            for (int tries = 0; !success && tries < maxTries; tries++)
            {
                System.Threading.Thread.Sleep(threadSleepTime);

                results = this.info.LocalManagementGroup.TaskRuntime.ExecuteTask<MonitoringObject>(targets, task, config);

                if (results.Count == 1)
                {
                    if (failedTask)
                    {
                        if (results[0].Status == Microsoft.EnterpriseManagement.Runtime.TaskStatus.Failed)
                        {
                            success = true;
                            return results;
                        }
                    }
                    else
                    { 
                        if (results[0].Status == Microsoft.EnterpriseManagement.Runtime.TaskStatus.Succeeded)
                        {
                            return results;
                        } 
                    }
                }

                this.logger(string.Format("RunTask: tries={0}, success={1}", tries, success));
            }

            if (results.Count == 0)
            {
                throw new InvalidOperationException("Failed to return any results from task: " + task.Name);
            }

            return results;
        }

        /// <summary>
        /// Runs a given task name, for example Microsoft.Linux.RHEL.5.Agent.Install.Task
        /// </summary>
        /// <param name="computerObject">The computer object to run the task against</param>
        /// <param name="taskName">Task name, for example Microsoft.Linux.RHEL.5.Agent.Install.Task</param>
        /// <returns>A collection of the task results.</returns>
        public IList<Microsoft.EnterpriseManagement.Runtime.TaskResult> RunTask(
           MonitoringObject computerObject,
           string taskName)
        {
            // Get the task.
            ManagementPackTask task = this.GetMonitoringTask(taskName);

            // Run the task.
            return this.RunTask(computerObject, task);
        }

        /// <summary>
        /// Runs a given task name, for example Microsoft.Linux.RHEL.5.Agent.Install.Task
        /// </summary>
        /// <param name="computerObject">The computer object to run the task against</param>
        /// <param name="task">Monitoring task</param>
        /// <returns>A collection of the task results</returns>
        public IList<Microsoft.EnterpriseManagement.Runtime.TaskResult> RunTask(
           MonitoringObject computerObject,
           ManagementPackTask task)
        {
            // Use the default task configuration.
            Microsoft.EnterpriseManagement.Runtime.TaskConfiguration config = new Microsoft.EnterpriseManagement.Runtime.TaskConfiguration();

            // Set the target
            List<MonitoringObject> targets = new List<MonitoringObject>();
            targets.Add(computerObject);

            return this.RunTask(targets, task, config);
        }

        /// <summary>
        /// Run consumer task and return the result
        /// </summary>
        /// <param name="computerObject">The monitored computer object</param>
        /// <param name="taskName">Consumer task name in management pack</param>
        /// <returns>return task result</returns>
        public IList<Microsoft.EnterpriseManagement.Runtime.TaskResult> RunConsumerTask(
            MonitoringObject computerObject,
            string taskName)
        {
            return this.RunTask(computerObject, taskName);
        }

        #endregion Public Methods

        #region Private Methods

        #endregion Private Methods

        #endregion Methods
    }
}
