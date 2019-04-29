using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EasyBehaviorTree
{
    [Serializable]
    public class BlackBoard : IBlackBoard
    {
        public Dictionary<string, IBlackBoardData> dict = new Dictionary<string, IBlackBoardData>();

        public IBlackBoardData this[string key]
        {
            get
            {
                if (!dict.ContainsKey(key))
                {
                    dict.Add(key, default(IBlackBoardData));
                }
                return dict[key];
            }
            set
            {
                dict[key] = value;
            }
        }
    }
}
