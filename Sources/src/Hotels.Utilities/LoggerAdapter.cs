using System;
using Microsoft.Extensions.Logging;

namespace Hotels.Utilities
{
    public class LoggerAdapter<T> : ILoggerAdapter<T>
    {
        private readonly ILogger<T> _logger;

        public LoggerAdapter(ILogger<T> logger)
        {
            _logger = logger;
        }


        public void LogInFo(string message)
        {
            _logger.LogInformation(message);
        }

        public void LogWarn(Exception exception, string message, params object[] args)
        {
            _logger.LogWarning(exception, message, args);
        }

        public void LogError(Exception exception, string message, params object[] args)
        {
            _logger.LogError(exception, message, args);
        }
    }
}