//-----------------------------------------------------------------------
// <copyright file="Logging.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>zhyao</author>
//-----------------------------------------------------------------------

namespace Scx.Test.Common
{
    /// <summary>
    ///     Log level used in the logging interface.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// The enum Fatal of the LogLevel.
        /// </summary>
        Fatal = 0x10 << 1,

        /// <summary>
        /// The enum Error of the LogLevel.
        /// </summary>
        Error = 0x10 << 2,

        /// <summary>
        /// The enum Warning of the LogLevel.
        /// </summary>
        Warning = 0x10 << 3,

        /// <summary>
        /// The enum Info of the LogLevel.
        /// </summary>
        Info = 0x10 << 4,

        /// <summary>
        /// The enum Debug of the LogLevel.
        /// </summary>
        Debug = 0x10 << 5,
    }

    /// <summary>
    ///     Generic logging support class.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        ///    Gets or sets Verbosity or log level of the current logging.  Smaller value means more important and should be logged.
        /// </summary>
        LogLevel Verbosity
        {
            get;
            set;
        }

        /// <summary>
        ///     Initializes the logging device.  Note that multiple devices may be specified as logging destination depending on
        ///     the concrete implementation of this interface.
        /// </summary>
        /// <param name="deviceName">
        ///     name of the underlying logging device.  (For instance, file name for file system based logging)
        /// </param>
        void Initialize(string deviceName);

        /// <summary>
        ///     Closes the logging device and stops all logging afterwards (exception will be raised).
        /// </summary>
        void Shutdown();

        /// <summary>
        ///     Writes a log message.
        /// </summary>
        /// <param name="logLevel">log level of the message</param>
        /// <param name="format">format in string of the message</param>
        /// <param name="args">object list used by the format</param>
        void Write(LogLevel logLevel, string format, params object[] args);

        /// <summary>
        ///     Writes a log message which logLevel == LogLevel.Info
        /// </summary>
        /// <param name="format">This is the format</param>
        /// <param name="args">This is the args</param>
        void Write(string format, params object[] args);

        /// <summary>
        ///     (Optional) Starts a new context and ends the old one.  This is ignored if the underlying logging device des not
        ///     support context based logging.
        /// </summary>
        /// <param name="format">format in string of the context name</param>
        /// <param name="args">object list used by the format</param>
        void Context(string format, params object[] args);
    }
}