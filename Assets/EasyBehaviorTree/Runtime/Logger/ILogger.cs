using System;
using System.Collections;
using System.Collections.Generic;

namespace EasyBehaviorTree
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

    public interface ILogger
    {
        LogLevel filter { get; set; }

        event Action<LogLevel,object> OnLog;

        void Debug(object message);
        void Log(object message);
        void Warning(object message);
        void Error(object message);
    }
}
