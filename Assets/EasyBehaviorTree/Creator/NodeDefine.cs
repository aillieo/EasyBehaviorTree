using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Linq;

namespace AillieoUtils.EasyBehaviorTree.Creator
{

    struct NodeParamSetAndName
    {
        public INodeParamSet set;
        public string name;
    }


    [DisallowMultipleComponent]
    public partial class NodeDefine : MonoBehaviour
    {
#if UNITY_EDITOR

        [SerializeField][HideInInspector]
        private string nodeFullName;

        [SerializeField][HideInInspector]
        private string displayName;
        [SerializeField][HideInInspector]
        private string nodeDescription;

        private Dictionary<Type, NodeParamSetAndName> cachedMappings;

        public Type GetNodeType()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                Type t = assembly.GetType(nodeFullName);
                if (t != null)
                {
                    return t;
                }
            }
            return null;
        }

        public bool IsRoot()
        {
            return transform.parent == null;
        }


        public NodeBase CreateNode()
        {
            Type t = GetNodeType();
            if (t != null)
            {
                NodeBase node = Activator.CreateInstance(t) as NodeBase;
                if (node != null)
                {
                    node.name = displayName;

                    var fields = ReflectionUtils.GetNodeParamFields(t);

                    foreach (var field in fields)
                    {
                        Type type = field.FieldType;
                        if (type.IsEnum)
                        {
                            type = typeof(Enum);
                        }
                        cachedMappings[type].set.TrySetFieldForType(field, node);
                    }
                }
                return node;
            }
            return null;
        }

        public void TryDrawFields(FieldInfo[] fields, SerializedObject serializedObject)
        {
            foreach (var field in fields)
            {
                Type type = field.FieldType;
                if(type.IsEnum)
                {
                    type = typeof(Enum);
                }
                NodeParamSetAndName nodeParamSetAndName = cachedMappings[type];
                SerializedProperty nodeParamSet = serializedObject.FindProperty(nodeParamSetAndName.name);
                SerializedProperty param = nodeParamSetAndName.set.TryGetSerializedParam(nodeParamSet, field);
                if (param != null)
                {
                    EditorGUILayout.PropertyField(param);
                }
            }
        }

#endif
    }
}
