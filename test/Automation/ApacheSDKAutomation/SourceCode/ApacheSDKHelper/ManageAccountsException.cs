//-----------------------------------------------------------------------
// <copyright file="ManageAccountsException.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>a-srinia</author>
// <description></description>
// <history>3/25/2009 5:05:51 PM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Apache.SDK.ApacheSDKHelper
{
    using System;
    
    /// <summary>
    /// Exception class for ManageAccounts class
    /// </summary>
    public class ManageAccountsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the ManageAccountsException class
        /// </summary>
        /// <param name="message">Exception message</param>
        public ManageAccountsException(string message)
            : base(message)
        {
        }
    }
}
