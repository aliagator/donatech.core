using System;
namespace Donatech.Core.Utils
{
    public static class LoggerUtils
    {
        public static void AddCustomLog(this ILogger logger, string classPrefix,
            string message, Exception? exception = null)
        {
            logger.LogInformation($"{classPrefix} => {message}.\n{exception?.Message}");

            if (exception != null)
                logger.LogError(exception, $"{classPrefix} => {message}.");
        }

        public static void AddCustomLog(this ILogger logger, string classPrefix, string methodPrefix,
            string message, Exception? exception = null)
        {
            logger.LogInformation($"{classPrefix}.{methodPrefix} => {message}.\n{exception?.Message}");

            if (exception != null)
                logger.LogError(exception, $"{classPrefix}.{methodPrefix} => {message}.");
        }
    }
}