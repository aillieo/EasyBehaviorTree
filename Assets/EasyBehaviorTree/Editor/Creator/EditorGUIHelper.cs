using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace AillieoUtils.EasyBehaviorTree.Creator
{
    public static class EditorGUIHelper
    {
        private static readonly HashSet<Type> typeIntegerSet = new HashSet<Type>()
        {
            typeof(sbyte), typeof(byte),
            typeof(short), typeof(ushort),
            typeof(int), typeof(uint),
            typeof(long), typeof(ulong),
        };

        private static readonly HashSet<Type> typeFloatSet = new HashSet<Type>()
        {
            typeof(float), typeof(double),
        };

        private static readonly Dictionary<Type, SerializedPropertyType> st2sptMappings = new Dictionary<Type, SerializedPropertyType>()
        {
            { typeof(bool), SerializedPropertyType.Boolean },
            { typeof(char), SerializedPropertyType.Character },
            { typeof(string), SerializedPropertyType.String },

            { typeof(Color), SerializedPropertyType.Color },
            { typeof(LayerMask), SerializedPropertyType.LayerMask },

            { typeof(Vector2), SerializedPropertyType.Vector2 },
            { typeof(Vector3), SerializedPropertyType.Vector3 },
            { typeof(Vector4), SerializedPropertyType.Vector4 },

            { typeof(Vector2Int), SerializedPropertyType.Vector2Int },
            { typeof(Vector3Int), SerializedPropertyType.Vector3Int },

            { typeof(Bounds), SerializedPropertyType.Bounds },
            { typeof(Quaternion), SerializedPropertyType.Quaternion },
            { typeof(Rect), SerializedPropertyType.Rect },

            { typeof(BoundsInt), SerializedPropertyType.BoundsInt },
            { typeof(RectInt), SerializedPropertyType.RectInt },

            { typeof(AnimationCurve), SerializedPropertyType.AnimationCurve },
            { typeof(Gradient), SerializedPropertyType.Gradient },
        };

        public static Type SerializedPropertyTypeToSystemType(SerializedPropertyType serializedPropertyType)
        {
            throw new NotImplementedException();
        }

        public static SerializedPropertyType SystemTypeToSerializedPropertyType(Type type)
        {
            if (typeIntegerSet.Contains(type))
            {
                return SerializedPropertyType.Integer;
            }

            if (typeFloatSet.Contains(type))
            {
                return SerializedPropertyType.Float;
            }

            if (type.IsEnum)
            {
                return SerializedPropertyType.Enum;
            }

            if (typeof(UnityEngine.Object).IsAssignableFrom(type))
            {
                return SerializedPropertyType.ObjectReference;
            }

            if (st2sptMappings.TryGetValue(type, out SerializedPropertyType serializedPropertyType))
            {
                return serializedPropertyType;
            }

            Debug.LogError(type.Name);

            return SerializedPropertyType.Generic;
        }

        public static bool DrawField(object target, FieldInfo fieldInfo)
        {
            SerializedPropertyType serializedPropertyType = SystemTypeToSerializedPropertyType(fieldInfo.FieldType);
            object value = fieldInfo.GetValue(target);
            EditorGUI.BeginChangeCheck();
            object newValue = DrawFieldInternal(serializedPropertyType, fieldInfo.Name, value);

            if (EditorGUI.EndChangeCheck())
            {
                fieldInfo.SetValue(target, newValue);
                return true;
            }

            return false;
        }

        private static object DrawFieldInternal(SerializedPropertyType serializedPropertyType, string label, object value)
        {
            switch (serializedPropertyType)
            {
                case SerializedPropertyType.Generic:
                    break;
                case SerializedPropertyType.Integer:
                    return EditorGUILayout.IntField(label, (int)value);
                case SerializedPropertyType.Boolean:
                    return EditorGUILayout.Toggle(label, (bool)value);
                case SerializedPropertyType.Float:
                    return EditorGUILayout.FloatField(label, (float)value);
                case SerializedPropertyType.String:
                    return EditorGUILayout.TextField(label, (string)value);
                case SerializedPropertyType.Color:
                    break;
                case SerializedPropertyType.ObjectReference:
                    break;
                case SerializedPropertyType.LayerMask:
                    break;
                case SerializedPropertyType.Enum:
                    break;
                case SerializedPropertyType.Vector2:
                    break;
                case SerializedPropertyType.Vector3:
                    break;
                case SerializedPropertyType.Vector4:
                    break;
                case SerializedPropertyType.Rect:
                    break;
                case SerializedPropertyType.ArraySize:
                    break;
                case SerializedPropertyType.Character:
                    break;
                case SerializedPropertyType.AnimationCurve:
                    break;
                case SerializedPropertyType.Bounds:
                    break;
                case SerializedPropertyType.Gradient:
                    break;
                case SerializedPropertyType.Quaternion:
                    break;
                case SerializedPropertyType.ExposedReference:
                    break;
                case SerializedPropertyType.FixedBufferSize:
                    break;
                case SerializedPropertyType.Vector2Int:
                    break;
                case SerializedPropertyType.Vector3Int:
                    break;
                case SerializedPropertyType.RectInt:
                    break;
                case SerializedPropertyType.BoundsInt:
                    break;
                case SerializedPropertyType.ManagedReference:
                    break;
                default:
                    throw new IndexOutOfRangeException();
            }

            throw new NotImplementedException();
        }
    }
}
