﻿// Copyright © 2016 Jeroen Stemerdink.
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
namespace EPi.Libraries.Logging.NLog
{
    using System;

    using EPiServer.Logging;

    using global::NLog;

    using ILogger = EPiServer.Logging.ILogger;
    using LogManager = global::NLog.LogManager;

    /// <summary>
    ///     Class NLogger.
    /// </summary>
    public class NLogger : ILogger
    {
        /// <summary>
        ///     The logger
        /// </summary>
        private readonly Logger logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="NLogger" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public NLogger(string name)
        {
            this.logger = LogManager.GetLogger(name: name);
        }

        /// <summary>
        ///     Determines whether logging at the specified level is enabled.
        /// </summary>
        /// <param name="level">The level to check.</param>
        /// <returns><c>true</c> if logging on the provided level is enabled; otherwise <c>false</c></returns>
        public bool IsEnabled(Level level)
        {
            return this.IsEnabled(MapLevel(level: level));
        }

        /// <summary>
        ///     Determines whether the specified level is enabled.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <returns><c>true</c> if the specified level is enabled; otherwise, <c>false</c>.</returns>
        public bool IsEnabled(LogLevel level)
        {
            return this.logger.IsEnabled(level: level);
        }

        /// <summary>
        /// Logs the provided <paramref name="state"/> with the specified level.
        /// </summary>
        /// <typeparam name="TState">The type of the state object.</typeparam><typeparam name="TException">The type of the exception.</typeparam><param name="level">The criticality level of the log message.</param><param name="state">The state that should be logged.</param><param name="exception">The exception that should be logged.</param><param name="messageFormatter">The message formatter used to write the state to the log provider.</param><param name="boundaryType">The type at the boundary of the logging framework facing the code using the logging.</param>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public void Log<TState, TException>(
            Level level, 
            TState state, 
            TException exception, 
            Func<TState, TException, string> messageFormatter, 
            Type boundaryType) where TException : Exception
        {
            if (messageFormatter == null)
            {
                return;
            }

            LogLevel mappedLevel = MapLevel(level: level);

            if (this.IsEnabled(level: mappedLevel))
            {
                this.logger.Log(level: mappedLevel, message: messageFormatter(arg1: state, arg2: exception), argument: exception);
            }
        }

        /// <summary>
        ///     Maps the EPiServer level to the NLoge level.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <returns>LogLevel.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">level</exception>
        private static LogLevel MapLevel(Level level)
        {
            switch (level)
            {
                case Level.Trace:
                    return LogLevel.Trace;
                case Level.Debug:
                    return LogLevel.Debug;
                case Level.Information:
                    return LogLevel.Info;
                case Level.Warning:
                    return LogLevel.Warn;
                case Level.Error:
                    return LogLevel.Error;
                case Level.Critical:
                    return LogLevel.Fatal;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level));
            }
        }
    }
}