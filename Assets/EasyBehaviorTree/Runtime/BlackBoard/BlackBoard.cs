using System;
using System.Collections.Generic;

namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public class BlackBoard : IBlackBoard
    {
        private Dictionary<string, IBlackBoardData> dict = new Dictionary<string, IBlackBoardData>();

        public IBlackBoardData this[string key]
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

        public IBlackBoardData SafeGet(string key, IBlackBoardData fallback = default)
        {
            IBlackBoardData ret = default;
            if (this.dict.TryGetValue(key, out ret))
            {
                return ret;
            }

            return fallback;
        }
    }
}
