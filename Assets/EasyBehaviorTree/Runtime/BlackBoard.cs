using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EasyBehaviorTree
{
    [Serializable]
    public class BlackBoard : IBlackBoard
    {
        public Dictionary<string, object> objDict = new Dictionary<string, object>();

        public event Action<string> OnValueChanged;

        public T Get<T>(string key)
        {
            if(objDict.ContainsKey(key))
            {
                return (T)objDict[key];

            }
            return default(T);
        }


        public void Set<T>(string key, T value)
        {
            objDict[key] = value;
            OnValueChanged.Invoke(key);
        }


    }

}
