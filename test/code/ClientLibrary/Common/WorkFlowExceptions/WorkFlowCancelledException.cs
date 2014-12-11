//-----------------------------------------------------------------------
// <copyright file="WorkFlowCancelledException.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.WorkFlowExceptions
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Exception thrown when an executing workflow is cancelled.
    /// </summary>
    [Serializable]
    public class WorkFlowCancelledException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the WorkFlowCancelledException class.
        /// </summary>
        public WorkFlowCancelledException()
            : base(string.Format(CultureInfo.CurrentCulture, Strings.WorkFlowCancelledMessage))
        {
        }
    }
}
