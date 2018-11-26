using System;

namespace Hotels.Utilities
{
    public interface ILoggerAdapter<T>
    {
        void LogInFo(string message);
        void LogWarn(Exception exception, string message, params object[] args);
        void LogError(Exception exception, string message, params object[] args);
    }
}