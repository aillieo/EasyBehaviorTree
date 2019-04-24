using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EasyBehaviorTree.Creator
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


        private void InitDict()
        {
            for (int i = 0, len = nodeParams.Length; i < len; ++i)
            {
                dict[nodeParams[i].key] = i;
            }
        }


#if UNITY_EDITOR

        protected int GetIndexOfKey(string key)
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


        protected SerializedProperty GetSerializedValue(SerializedProperty nodeParamSet, string propertyName)
        {
            var array = nodeParamSet.FindPropertyRelative("nodeParams");
            var index = GetIndexOfKey(propertyName);
            if (array.arraySize <= index)
            {
                nodeParamSet.serializedObject.Update();
            }
            var param = array.GetArrayElementAtIndex(index);
            var paramValue = param.FindPropertyRelative("value");
            return paramValue;
        }


        public virtual void TrySetPropertyForType(PropertyInfo property, NodeBase node)
        {
            if (property.PropertyType == typeof(T))
            {
                property.SetValue(node, this[property.Name]);
            }
        }


        public virtual void TryDrawPropertyForType(PropertyInfo property, SerializedProperty serializedProperty)
        {
            if (property.PropertyType == typeof(T))
            {
                string propertyName = property.Name;
                GUILayout.BeginVertical("Box");
                var paramValue = GetSerializedValue(serializedProperty, propertyName);
                EditorGUILayout.PropertyField(paramValue, new GUIContent(propertyName));
                GUILayout.EndVertical();
            }
        }

#endif

    }
}
