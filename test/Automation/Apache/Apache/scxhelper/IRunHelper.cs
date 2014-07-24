//-----------------------------------------------------------------------
// <copyright file="IRunHelper.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>a-brmill</author>
// <description></description>
// <history>1/20/2009 9:22:56 AM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Common
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Description for IRunHelper.
    /// </summary>
    public interface IRunHelper
    {
        #region Private Fields

        #endregion Private Fields

        #region Constructors

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the name of the executable file
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// Gets or sets the arguments passed to file
        /// </summary>
        string Arguments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the expected code is pass (0) or not
        /// </summary>
        bool ExpectToPass { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore the exit code
        /// </summary>
        bool IgnoreExit { get; set; }

        /// <summary>
        /// Gets or sets the time out (in seconds)
        /// </summary>
        int TimeOut { get; set; }

        /// <summary>
        /// Gets the text returned on the stdout stream
        /// </summary>
        string StdOut { get; }

        /// <summary>
        /// Gets the text returned on the stderr stream (not implemented)
        /// </summary>
        string StdErr { get; }

        #endregion Properties

        #region Methods

        #region Private Methods

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Run the command
        /// </summary>
        void RunCmd();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        //// static void RunCmd(IContext ctx);

        #endregion Public Methods

        #endregion Methods
    }
}
