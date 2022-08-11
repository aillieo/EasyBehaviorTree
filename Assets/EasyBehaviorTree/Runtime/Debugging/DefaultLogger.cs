using System;

namespace AillieoUtils
{
    [Flags]
    public enum LogLevel
    {
        None = 0,
        Debug = 1,
        Log = 2,
        Warning = 4,
        Error = 8,
        NonDebug = 14,
        Any = 15,
    }

    public class DefaultLogger
    {
        public LogLevel filter { get; set; } = LogLevel.Any;

        public void Debug(object message)
        {
            LogWithFilter(LogLevel.Debug, message);
        }

        public void Log(object message)
        {
            LogWithFilter(LogLevel.Log, message);
        }

        public void Warning(object message)
        {
            LogWithFilter(LogLevel.Warning, message);
        }

        public void Error(object message)
        {
            LogWithFilter(LogLevel.Error, message);
        }

        private void LogWithFilter(LogLevel logLevel, object message)
        {
            if ((this.filter & logLevel) > 0)
            {
                Log(logLevel, message);
            }
        }

        protected void Log(LogLevel logLevel, object message)
        {
            string log = $"[{UnityEngine.Time.frameCount}]{message}";
            switch (logLevel)
            {
            case LogLevel.Warning:
                UnityEngine.Debug.LogWarning(log);
                break;
            case LogLevel.Error:
                UnityEngine.Debug.LogError(log);
                break;
            default:
                UnityEngine.Debug.Log(log);
                break;
            }
        }
    }
}
