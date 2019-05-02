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
        internal override void TrySetFieldForType(FieldInfo field, NodeBase node)
        {
            if (field.FieldType.IsEnum)
            {
                field.SetValue(node, Enum.ToObject(field.FieldType, this[field.Name]));
            }
        }

        public override void TryDrawFieldForType(FieldInfo field, SerializedProperty serializedProperty)
        {
            if (field.FieldType.IsEnum)
            {
                string fieldName = field.Name;
                GUILayout.BeginVertical("Box");
                var paramValue = GetSerializedValue(serializedProperty, fieldName);
                if (!Enum.IsDefined(field.FieldType, paramValue.intValue))
                {
                    paramValue.intValue = (int)Enum.GetValues(field.FieldType).GetValue(0);
                }
                paramValue.intValue = EditorGUILayout.Popup(new GUIContent(fieldName), paramValue.intValue, field.FieldType.GetEnumNames());
                GUILayout.EndVertical();
            }
        }

#endif
    }
}
