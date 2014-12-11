//-----------------------------------------------------------------------
// <copyright file="TaskNotFoundException.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Exception thrown when a requested task can not be found in the database.
    /// </summary>
    [Serializable]
    public class TaskNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the TaskNotFoundException class.
        /// </summary>
        /// <param name="taskName">Name of the task that was requested.</param>
        public TaskNotFoundException(string taskName)
            : base(string.Format(CultureInfo.CurrentCulture, Strings.TaskNotFoundMessage, taskName))
        {
        }
    }
}
