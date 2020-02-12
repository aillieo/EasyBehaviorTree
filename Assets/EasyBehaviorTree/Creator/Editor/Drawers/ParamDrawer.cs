using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Reflection;

namespace AillieoUtils.EasyBehaviorTree.Creator
{

    enum ParamType
    {
        Unknown,
        Normal,
        Array,
        Enum,
    }

    [CustomPropertyDrawer(typeof(NodeParamObject),true)]
    public class ParamDrawer : PropertyDrawer
    {

        ParamType paramType = ParamType.Unknown;
        ReorderableList reorderableList;

        void EnsureParamType()
        {
            if (paramType == ParamType.Unknown)
            {
                Type fieldType = fieldInfo.FieldType;
                Type elementType = fieldType.GetElementType();
                FieldInfo valueField = elementType.GetField("value", BindingFlags.Public | BindingFlags.Instance);
                Type valueType = valueField.FieldType;

                if (valueType.IsArray)
                {
                    paramType = ParamType.Array;
                }
                else if (elementType.GetField("enumType",BindingFlags.Instance|BindingFlags.Public) != null)
                {
                    paramType = ParamType.Enum;
                }
                else
                {
                    paramType = ParamType.Normal;
                }
            }
        }


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EnsureParamType();

            switch (paramType)
            {
                case ParamType.Unknown:
                    Debug.LogError("Unknown type");
                    break;
                case ParamType.Normal:
                    EditorGUI.PropertyField(position, property.FindPropertyRelative("value"), new GUIContent(property.FindPropertyRelative("key").stringValue));
                    break;
                case ParamType.Array:
                    if (reorderableList == null)
                    {
                        CreateReorderableList(property);
                    }
                    reorderableList.DoList(position);
                    break;
                case ParamType.Enum:
                    Type fieldType = fieldInfo.FieldType;
                    Type elementType = fieldType.GetElementType();
                    string enumTypeName = property.FindPropertyRelative("enumType").stringValue;
                    Type enumType = Type.GetType(enumTypeName);
                    var paramValue = property.FindPropertyRelative("value");
                    FieldInfo field = elementType.GetField("value", BindingFlags.Public | BindingFlags.Instance);
                    if (!Enum.IsDefined(enumType, paramValue.intValue))
                    {
                        paramValue.intValue = (int)Enum.GetValues(field.FieldType).GetValue(0);
                    }
                    paramValue.intValue = EditorGUI.Popup(position,property.FindPropertyRelative("key").stringValue, paramValue.intValue, enumType.GetEnumNames());
                    break;
            }
        }
        
        void CreateReorderableList(SerializedProperty serializedProperty)
        {
            reorderableList = new ReorderableList(serializedProperty.serializedObject, serializedProperty.FindPropertyRelative("value"));
            reorderableList.drawHeaderCallback += rect => GUI.Label(rect, serializedProperty.FindPropertyRelative("key").stringValue);
            reorderableList.elementHeight = EditorGUIUtility.singleLineHeight;
            reorderableList.drawElementCallback += (rect, index, isActive, isFocused) => {
                DrawOneProperty(serializedProperty.FindPropertyRelative("value"), rect, index, isActive, isFocused);
            };
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            EnsureParamType();
            switch (paramType)
            {
                case ParamType.Unknown:
                    return 0;
                case ParamType.Normal:
                    return base.GetPropertyHeight(property, label);
                case ParamType.Array:
                    SerializedProperty valueProperty = property.FindPropertyRelative("value");
                    int arraySize = valueProperty.arraySize;
                    float height = EditorGUIUtility.singleLineHeight * 3;
                    if(arraySize > 0)
                    {
                        height += EditorGUI.GetPropertyHeight(valueProperty.GetArrayElementAtIndex(0)) * arraySize;
                    }
                    else
                    {
                        height += EditorGUIUtility.singleLineHeight;
                    }
                    return height;
                case ParamType.Enum:
                    return EditorGUIUtility.singleLineHeight;
                default:
                    return base.GetPropertyHeight(property, label);
            }
        }

        private void DrawOneProperty(SerializedProperty serializedProperty, Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = serializedProperty.GetArrayElementAtIndex(index);

            var halfWidth = rect.width / 2;
            var rectLeft = new Rect(rect.x, rect.y, halfWidth, rect.height);
            EditorGUI.LabelField(rectLeft, index.ToString());
            var rectRight = new Rect(rect.x + halfWidth, rect.y, halfWidth, rect.height);
            EditorGUI.PropertyField(rectRight, element, GUIContent.none);
        }
    }
}

