using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AillieoUtils.EasyBehaviorTree.Creator
{
    public interface INodeParamSet
    {
#if UNITY_EDITOR
        bool TrySetFieldForType(FieldInfo field, NodeBase node);
        SerializedProperty TryGetSerializedParam(SerializedProperty nodeParamSet, FieldInfo field);
#endif
    }

    [Serializable]
    public class NodeParamSet<R,T> : INodeParamSet where R: NodeParam<T>,new()
    {
        [NonSerialized]
        Dictionary<string, int> dict = new Dictionary<string, int>();

        [SerializeField]
        R[] nodeParams = Array.Empty<R>();

        public T GetValueForKey(string key)
        {
            EnsureDict();
            return nodeParams[dict[key]].value;
        }

        public void SetValueForKey(string key, T value)
        {
            EnsureDict();
            nodeParams[dict[key]].value = value;
        }

        private void EnsureDict()
        {
            if (dict.Count == 0 && nodeParams.Length != 0)
            {
                for (int i = 0, len = nodeParams.Length; i < len; ++i)
                {
                    dict[nodeParams[i].key] = i;
                }
            }
        }

        protected int AddDefaultValue(string key, params object[] args)
        {
            int len = nodeParams.Length;
            Array.Resize(ref nodeParams, len + 1);
            R nodeParam = new R();
            nodeParam.key = key;
            nodeParam.value = default;
            for(int i = 0, argCount = args.Length; i< argCount; i+=2)
            {
                string fieldName = args[i] as string;
                if (fieldName != null)
                {
                    FieldInfo fi = typeof(R).GetField(fieldName, BindingFlags.Instance | BindingFlags.Public);
                    if(fi != null)
                    {
                        fi.SetValue(nodeParam, args[i + 1]);
                    }
                }
            }
            nodeParams[len] = nodeParam;
            dict.Add(key, len);
            return 0;
        }


#if UNITY_EDITOR

        protected int GetIndexOfKey(string key)
        {
            EnsureDict();
            if (!dict.ContainsKey(key))
            {
                return -1;
            }
            return dict[key];
        }

        public virtual bool TrySetFieldForType(FieldInfo field, NodeBase node)
        {
            if (field.FieldType == typeof(T))
            {
                field.SetValue(node, GetValueForKey(field.Name));
                return true;
            }
            return false;
        }

        public virtual SerializedProperty TryGetSerializedParam(SerializedProperty nodeParamSet, FieldInfo field)
        {
            if (field.FieldType == typeof(T))
            {
                string fieldName = field.Name;
                var index = GetIndexOfKey(fieldName);
                if(index < 0)
                {
                    index = AddDefaultValue(fieldName);
                }
                var array = nodeParamSet.FindPropertyRelative("nodeParams");
                if (array.arraySize <= index)
                {
                    nodeParamSet.serializedObject.Update();
                }
                var param = array.GetArrayElementAtIndex(index);
                return param;
            }
            return null;
        }
#endif

    }
}
