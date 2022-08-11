using System;
using System.Collections.Generic;

namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public class Blackboard
    {
        private Dictionary<string, object> dict = new Dictionary<string, object>();

        public object this[string key]
        {
            get
            {
                return dict[key];
            }
            set
            {
                dict[key] = value;
            }
        }

        public void CleanUp()
        {
            dict.Clear();
        }

        public bool HasValue(string key)
        {
            return dict.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return this.dict.Remove(key);
        }

        public object SafeGet(string key, object fallback = default)
        {
            object ret = default;
            if (this.dict.TryGetValue(key, out ret))
            {
                return ret;
            }

            return fallback;
        }
    }
}
