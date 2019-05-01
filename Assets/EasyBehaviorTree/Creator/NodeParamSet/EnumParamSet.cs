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
        internal override void TrySetPropertyForType(PropertyInfo property, NodeBase node)
        {
            if (property.PropertyType.IsEnum)
            {
                property.SetValue(node, Enum.ToObject(property.PropertyType, this[property.Name]));
            }
        }

        public override void TryDrawPropertyForType(PropertyInfo property, SerializedProperty serializedProperty)
        {
            if (property.PropertyType.IsEnum)
            {
                string propertyName = property.Name;
                GUILayout.BeginVertical("Box");
                var paramValue = GetSerializedValue(serializedProperty, propertyName);
                if (!Enum.IsDefined(property.PropertyType, paramValue.intValue))
                {
                    paramValue.intValue = (int)Enum.GetValues(property.PropertyType).GetValue(0);
                }
                paramValue.intValue = EditorGUILayout.Popup(new GUIContent(propertyName), paramValue.intValue, property.PropertyType.GetEnumNames());
                GUILayout.EndVertical();
            }
        }

#endif
    }
}
