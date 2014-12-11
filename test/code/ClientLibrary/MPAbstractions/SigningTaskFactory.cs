//-----------------------------------------------------------------------
// <copyright file="SigningTaskFactory.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// This class implements the default behavior for creating new SigningTask objects.
    /// </summary>
    public class SigningTaskFactory : ISigningTaskFactory
    {
        /// <summary>
        /// Create a new SigningTask.
        /// </summary>
        /// <returns>the new task</returns>
        public SigningTask CreateSigningTask()
        {
            return new SigningTask();
        }
    }
}
