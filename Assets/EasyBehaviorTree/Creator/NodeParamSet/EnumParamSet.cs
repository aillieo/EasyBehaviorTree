using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EasyBehaviorTree.Creator
{

    [Serializable]
    public class EnumParam : NodeParam<int>
    {
    }

    [Serializable]
    public class EnumParamSet : NodeParamSet<EnumParam, int>
    {

#if UNITY_EDITOR
        public void SetPropertiesForType(PropertyInfo[] properties, NodeBase node)
        {
            foreach (var p in properties)
            {
                if (p.PropertyType.IsEnum)
                {
                    p.SetValue(node, Enum.ToObject(p.PropertyType, this[p.Name]));
                }
            }
        }

        public void DrawPropertiesForType(PropertyInfo[] properties, SerializedProperty serializedProperty)
        {
            foreach (var p in properties)
            {
                if (p.PropertyType.IsEnum)
                {
                    string propertyName = p.Name;
                    GUILayout.BeginVertical("Box");
                    var paramValue = GetSerializedValue(serializedProperty, propertyName);
                    if (!Enum.IsDefined(p.PropertyType, paramValue.intValue))
                    {
                        paramValue.intValue = (int)Enum.GetValues(p.PropertyType).GetValue(0);
                    }
                    paramValue.intValue = EditorGUILayout.Popup(new GUIContent(propertyName), paramValue.intValue, p.PropertyType.GetEnumNames());
                    GUILayout.EndVertical();
                }
            }
        }

#endif
    }
}
