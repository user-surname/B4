using NLog;
using System;

namespace B4.Shared.Logger
{
    public static class LoggerManager
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public static void Info(string message)
        {
            logger.Info(message);
        }

        public static void Warn(string message)
        {
            logger.Warn(message);
        }

        public static void Error(string message, Exception? ex = null)
        {
            logger.Error(ex, message);
        }

        public static void Debug(string message)
        {
            logger.Debug(message);
        }
    }
}