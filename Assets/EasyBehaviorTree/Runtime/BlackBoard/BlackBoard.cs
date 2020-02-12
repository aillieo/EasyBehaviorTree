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
                if (dict.ContainsKey(key))
                {
                    return dict[key];
                }
                return default;
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
    }
}
