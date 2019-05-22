using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Linq;
using System.Text;

namespace EasyBehaviorTree.Creator
{

    struct NodeParamSetAndName
    {
        public INodeParamSet set;
        public string name;
    }


    [DisallowMultipleComponent]
    public partial class NodeDefine : MonoBehaviour
    {
        [SerializeField][HideInInspector]
        private string nodeFullName;
        [SerializeField][HideInInspector]
        private string assemblyName;

        [SerializeField][HideInInspector]
        private string displayName;
        [SerializeField][HideInInspector]
        private string nodeDescription;

        Action ensureCachedMappingsImpl;

        private Dictionary<Type, NodeParamSetAndName> cachedMappings;

        public static FieldInfo[] GetNodeParamFields(Type type)
        {
            return type.GetFields(BindingFlags.Public| BindingFlags.NonPublic|BindingFlags.Instance)
            .Where(fi => fi.GetCustomAttribute<NodeParamAttribute>(false) != null).ToArray();
        }

        public Type GetNodeType()
        {
            Assembly ass = Assembly.Load(assemblyName);
            if (ass != null)
            {
                Type t = ass.GetType(nodeFullName);
                return t;
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
                    List<string> paramInfo = new List<string>();

                    var fields = GetNodeParamFields(t);

                    ensureCachedMappingsImpl?.Invoke();

                    foreach (var field in fields)
                    {
                        Type type = field.FieldType;
                        if (type.IsEnum)
                        {
                            type = typeof(Enum);
                        }
                        cachedMappings[type].set.TrySetFieldForType(field, node);
                        paramInfo.Add(field.Name);
                        string value = Convert.ToString(field.GetValue(node));
                        paramInfo.Add(value != null ? value : string.Empty);
                    }
                    node.paramInfo = paramInfo.ToArray();
                }
                return node;
            }
            return null;
        }

        public void TryDrawFields(FieldInfo[] fields, SerializedObject serializedObject)
        {
            ensureCachedMappingsImpl?.Invoke();
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
    }
}
