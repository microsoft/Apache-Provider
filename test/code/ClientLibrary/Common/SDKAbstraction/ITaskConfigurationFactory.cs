//-----------------------------------------------------------------------
// <copyright file="ITaskConfigurationFactory.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using System.Collections.Generic;
    using System.Security;
    using Microsoft.EnterpriseManagement.Configuration;
    using Runtime = Microsoft.EnterpriseManagement.Runtime;

    /// <summary>
    /// Creates task configurations given a task and the parameters to override.
    /// </summary>
    public interface ITaskConfigurationFactory
    {
        /// <summary>
        /// Creates a task configuration for the task.
        /// </summary>
        /// <param name="task">Task to create configuration for.</param>
        /// <param name="parameterOverrides">Parameters to override for the task.</param>
        /// <param name="secureOverrides">Secure parameters to override for the task.</param>
        /// <returns>A TaskConfiguration that can be used in the task invocation.</returns>
        Runtime.TaskConfiguration CreateTaskConfiguration(ManagementPackTask task, Dictionary<string, string> parameterOverrides, Dictionary<string, SecureString> secureOverrides);
    }
}
