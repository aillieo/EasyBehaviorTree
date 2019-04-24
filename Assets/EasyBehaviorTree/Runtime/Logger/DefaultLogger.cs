using System;
using System.Collections;
using System.Collections.Generic;

namespace EasyBehaviorTree
{
    public class DefaultLogger : BaseLogger
    {
        protected override void Log(LogLevel logLevel, object message)
        {
#if UNITY_EDITOR
            switch (logLevel)
            {
            case LogLevel.Warning:
                UnityEngine.Debug.LogWarning(message);
                break;
            case LogLevel.Error:
                UnityEngine.Debug.LogError(message);
                break;
            default:
                UnityEngine.Debug.Log(message);
                break;
            }
#else
            System.Console.WriteLine(message.ToString());
#endif
        }
    }
}
