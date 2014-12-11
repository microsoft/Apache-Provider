//-----------------------------------------------------------------------
// <copyright file="TaskConfigurationFactory.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Security;
    using Microsoft.EnterpriseManagement.Common;
    using Microsoft.EnterpriseManagement.Configuration;
    using Runtime = Microsoft.EnterpriseManagement.Runtime;

    /// <summary>
    /// The TaskConfigurationFactory can produce a TaskConfiguration (OM SDK) given a ManagementPackTask and a dictionary of overrides.
    /// </summary>
    internal class TaskConfigurationFactory : ITaskConfigurationFactory
    {
        /// <summary>
        /// Handle for tracing.
        /// </summary>
        private static TraceSource trace = new TraceSource("Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction");

        /// <summary>
        /// Creates a task configuration for the task.
        /// </summary>
        /// <param name="task">Task to create configuration for.</param>
        /// <param name="parameterOverrides">Parameters to override for the task.</param>
        /// <param name="secureOverrides">Secure parameters to override for the task.</param>
        /// <returns>A TaskConfiguration that can be used in the task invocation.</returns>
        public Runtime.TaskConfiguration CreateTaskConfiguration(ManagementPackTask task, Dictionary<string, string> parameterOverrides, Dictionary<string, SecureString> secureOverrides)
        {
            Dictionary<string, ManagementPackOverrideableParameter> parameterMap = this.CreateParameterMap(task);
            Runtime.TaskConfiguration config = new Runtime.TaskConfiguration();
            OverrideParameters(parameterOverrides, secureOverrides, parameterMap, config);
            return config;
        }

        #region Virtual methods for overriding in testcode
        
        /// <summary>
        /// Overrideable method that gets the name from a ManagementPackOverrideableParameter.
        /// </summary>
        /// <param name="parameter">Parameter to get name from.</param>
        /// <returns>Name of parameter.</returns>
        protected virtual string GetParameterName(ManagementPackOverrideableParameter parameter)
        {
            return parameter.Name;
        }

        /// <summary>
        /// Overrideable method that gets a list of all the overrideable parameters of a task.
        /// </summary>
        /// <param name="task">Task to get parameters for.</param>
        /// <returns>List of all the overrideable parameters of the task.</returns>
        protected virtual IList<ManagementPackOverrideableParameter> GetTaskParameters(ManagementPackTask task)
        {
            return task.GetOverrideableParameters();
        }

        #endregion Virtual methods for overriding in testcode

        /// <summary>
        /// Overrides all parameters.
        /// </summary>
        /// <param name="overrides">Parameters that are to be overridden.</param>
        /// <param name="secureOverrides">Secure parameters that are to be overridden.</param>
        /// <param name="parameterMap">Map of all overrideable parameters defined in task.</param>
        /// <param name="config">Task configuration where overrides are stored.</param>
        private static void OverrideParameters(
            Dictionary<string, string> overrides,
            Dictionary<string, SecureString> secureOverrides,
            Dictionary<string, ManagementPackOverrideableParameter> parameterMap,
            Runtime.TaskConfiguration config)
        {
            foreach (var parameterOverride in overrides)
            {
                if (parameterMap.ContainsKey(parameterOverride.Key))
                {
                    trace.TraceEvent(TraceEventType.Information, 9, "Overriding task parameter '{0}' with value '{1}'.", parameterOverride.Key, parameterOverride.Value);
                    config.Overrides.Add(new Pair<ManagementPackOverrideableParameter, string>(parameterMap[parameterOverride.Key], parameterOverride.Value));
                }
            }

            foreach (var parameterOverride in secureOverrides)
            {
                if (parameterMap.ContainsKey(parameterOverride.Key))
                {
                    trace.TraceEvent(TraceEventType.Information, 10, "Overriding task parameter '{0}' with value secret value.", parameterOverride.Key);
                    config.Overrides.Add(CreateSecureParameterOverride(parameterMap[parameterOverride.Key], parameterOverride.Value));
                }
            }
        }

        /// <summary>
        /// Creates a parameter - value pair from a parameter and a secure string value.
        /// </summary>
        /// <param name="parameter">Parameter to override.</param>
        /// <param name="value">Value as a secure string.</param>
        /// <returns>A parameter override pair ready to insert into the configuration.</returns>
        private static Pair<ManagementPackOverrideableParameter, string> CreateSecureParameterOverride(ManagementPackOverrideableParameter parameter, SecureString value)
        {
            IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(value);
            try
            {
                return new Pair<ManagementPackOverrideableParameter, string>(parameter, System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr));
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
            }
        }

        /// <summary>
        /// Creates a map of all overrideable parameters in task.
        /// </summary>
        /// <param name="task">Task which defines the parameters.</param>
        /// <returns>Dictionary that maps parameter name to parameter.</returns>
        private Dictionary<string, ManagementPackOverrideableParameter> CreateParameterMap(ManagementPackTask task)
        {
            var parameterMap = new Dictionary<string, ManagementPackOverrideableParameter>();
            foreach (var parameter in this.GetTaskParameters(task))
            {
                parameterMap.Add(this.GetParameterName(parameter), parameter);
            }

            return parameterMap;
        }
    }
}