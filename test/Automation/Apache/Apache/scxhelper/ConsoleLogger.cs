//-----------------------------------------------------------------------
// <copyright file="ConsoleLogger.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>sunilmu</author>
// <description></description>
// <history>3/7/2011 8:47:33 AM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Common
{
    /// <summary>
    /// This class can be used as the default class for logging where MCF logger is not available.
    /// This logs ALL messages to the console.
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        /// <summary>
        /// Gets or sets the verbosity. 
        /// </summary>
        public LogLevel Verbosity
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
                throw new System.NotImplementedException();
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the Initialize class.
        /// </summary>
        /// <param name="deviceName">This is the deviceName</param>
        public void Initialize(string deviceName)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Initializes a new instance of the Shutdown class for shoutdown the machine.
        /// </summary>
        public void Shutdown()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// The function used to write a file. 
        /// </summary>
        /// <param name="logLevel">The param is logLevel</param>
        /// <param name="format">The param is format</param>
        /// <param name="args">The param is args</param>
        public void Write(LogLevel logLevel, string format, params object[] args)
        {
            System.Console.WriteLine(string.Format(format, args));
        }

        /// <summary>
        /// This function can call the Write function.
        /// </summary>
        /// <param name="format">This param is format</param>
        /// <param name="args">This param is args</param>
        public void Write(string format, params object[] args)
        {
            this.Write(LogLevel.Info, format, args);
        }

        /// <summary>
        /// Initializes a new instance of the Context class.
        /// </summary>
        /// <param name="format">The param format</param>
        /// <param name="args">The param args</param>
        public void Context(string format, params object[] args)
        {
            throw new System.NotImplementedException();
        }
    }
}
