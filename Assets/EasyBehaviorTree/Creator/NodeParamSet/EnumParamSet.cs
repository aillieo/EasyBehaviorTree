using System;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AillieoUtils.EasyBehaviorTree.Creator
{

    [Serializable]
    public class EnumParam : NodeParam<int>
    {
        public string enumType;
    }

    [Serializable]
    public class EnumParamSet : NodeParamSet<EnumParam, int>
    {

#if UNITY_EDITOR

        public override bool TrySetFieldForType(FieldInfo field, NodeBase node)
        {
            if (field.FieldType.IsEnum)
            {
                if(GetIndexOfKey(field.Name) < 0)
                {
                    AddDefaultValue(field.Name, "enumType", field.FieldType.FullName);
                }
                field.SetValue(node, Enum.ToObject(field.FieldType, GetValueForKey(field.Name)));
                return true;
            }
            return false;
        }

        public override SerializedProperty TryGetSerializedParam(SerializedProperty nodeParamSet, FieldInfo field)
        {
            if (field.FieldType.IsEnum)
            {
                string fieldName = field.Name;
                var index = GetIndexOfKey(fieldName);
                if(index < 0)
                {
                    index = AddDefaultValue(fieldName, "enumType", field.FieldType.FullName);
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
