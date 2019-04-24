using System;
using System.Collections;
using System.Collections.Generic;

namespace EasyBehaviorTree
{
    public abstract class BaseLogger : ILogger
    {
        public LogLevel filter { get; set; } = LogLevel.Any;

        public event Action<LogLevel, object> OnLog;

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
                if (OnLog != null)
                {
                    OnLog(logLevel, message);
                }
            }
        }

        protected abstract void Log(LogLevel logLevel, object message);

    }

}
