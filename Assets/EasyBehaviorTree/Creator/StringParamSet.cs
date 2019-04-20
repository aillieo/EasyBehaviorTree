using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyBehaviorTree
{
    [Serializable]
    struct StringParam
    {
        public string key;
        public string value;
    }

    [Serializable]
    public class StringParamSet
    {
        [NonSerialized]
        Dictionary<string, int> dict = new Dictionary<string, int>();

        [SerializeField]
        StringParam[] stringParams = Array.Empty<StringParam>();

        public string this[string key]
        {
            get
            {
                if (dict.Count == 0 && stringParams.Length != 0)
                {
                    InitDict();
                }
                if (!dict.ContainsKey(key))
                {
                    AddDefaultValue(key);
                }

                return stringParams[dict[key]].value;
            }

            set
            {
                int len = stringParams.Length;
                int index = len;
                if(dict.ContainsKey(key))
                {
                    index = dict[key];
                }
                if(index == len)
                {
                    Array.Resize(ref stringParams, len + 1);
                }

                dict[key] = index;
                stringParams[index] = new StringParam
                {
                    key = key,
                    value = value
                };

            }
        }

        private void AddDefaultValue(string key)
        {
            int len = stringParams.Length;
            Array.Resize(ref stringParams, len + 1);
            stringParams[len] = new StringParam
            {
                key = key,
                value = string.Empty
            };
            dict.Add(key, len);
        }

        public int GetIndexOfKey(string key)
        {
            if (dict.Count == 0 && stringParams.Length != 0)
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
            for (int i = 0, len = stringParams.Length; i < len; ++i)
            {
                dict[stringParams[i].key] = i;
            }
        }
    }
}
