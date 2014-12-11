// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGetProcessesTaskResult.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.MPAbstractions
{
    using System.Collections.Generic;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction;

    public interface IGetProcessesTaskResult
    {
        #region Properties

        /// <summary>
        /// Gets the list of processes that were returned from the computer.
        /// </summary>
        List<IProcess> Processes { get; }

        #endregion
    }
}