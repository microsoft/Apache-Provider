//-----------------------------------------------------------------------
// <copyright file="ScxMethods.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>a-brdust</author>
// <description></description>
// <history>5/13/2009 11:09:42 AM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Common
{
    /// <summary>
    /// Wrapper class to hold static methods and delegates
    /// </summary>
    public class ScxMethods
    {
        /// <summary>
        /// Null log delegate: acts as a placeholder for a logging functionality.
        /// </summary>
        /// <param name="logMsg">Log message (will be ingnored)</param>
        public static void ScxNullLogDelegate(string logMsg)
        {
            // do nothing
        }

        /// <summary>
        /// Null context value delegate: acts as a placeholder for retrieving an MCF context value
        /// </summary>
        /// <param name="contextKey">MCF Context</param>
        /// <returns>null string</returns>
        public static string ScxNullContextValueDelegateDelegate(string contextKey)
        {
            return null;
        }
    }
}
