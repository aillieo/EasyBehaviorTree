using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyBehaviorTree
{
    [Serializable]
    public class NodeParam<T>
    {
        public string key;
        public T value;
    }

    [Serializable]
    public class NodeParamSet<R,T> where R: NodeParam<T>,new()
    {
        [NonSerialized]
        Dictionary<string, int> dict = new Dictionary<string, int>();

        [SerializeField]
        R[] nodeParams = Array.Empty<R>();

        public T this[string key]
        {
            get
            {
                if (dict.Count == 0 && nodeParams.Length != 0)
                {
                    InitDict();
                }
                if (!dict.ContainsKey(key))
                {
                    AddDefaultValue(key);
                }

                return nodeParams[dict[key]].value;
            }

            set
            {
                int len = nodeParams.Length;
                int index = len;
                if(dict.ContainsKey(key))
                {
                    index = dict[key];
                }
                if(index == len)
                {
                    Array.Resize(ref nodeParams, len + 1);
                }

                dict[key] = index;
                nodeParams[index] = new R
                {
                    key = key,
                    value = value
                };

            }
        }

        private void AddDefaultValue(string key)
        {
            int len = nodeParams.Length;
            Array.Resize(ref nodeParams, len + 1);
            nodeParams[len] = new R
            {
                key = key,
                value = default(T)
            };
            dict.Add(key, len);
        }

        public int GetIndexOfKey(string key)
        {
            if (dict.Count == 0 && nodeParams.Length != 0)
            {
                InitDict();
            }
            if (!dict.ContainsKey(key))
            {
                AddDefaultValue(key);
            }
            return dict[key];
        }

        private void InitDict()
        {
            for (int i = 0, len = nodeParams.Length; i < len; ++i)
            {
                dict[nodeParams[i].key] = i;
            }
        }
    }

}
