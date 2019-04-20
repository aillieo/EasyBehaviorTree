using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Linq;

namespace EasyBehaviorTree
{
    [DisallowMultipleComponent]
    public class NodeDefine : MonoBehaviour
    {
        [HideInInspector]
        public string nodeFullName;
        [HideInInspector]
        public string assemblyName;

#if UNITY_EDITOR
        [HideInInspector]
        [SerializeField]
        public StringParamSet stringParamSet = new StringParamSet();
#endif

        public NodeBase CreateNode()
        {
            Assembly ass = Assembly.Load(assemblyName);
            if(ass != null)
            {
                Type t = ass.GetType(nodeFullName);
                if (t != null)
                {
                    NodeBase node = Activator.CreateInstance(t) as NodeBase;
                    if(node != null)
                    {
                        var properties = t.GetProperties();
                        foreach(var p in properties)
                        {
                            if(p.PropertyType == typeof(string))
                            {
                                p.SetValue(node, stringParamSet[p.Name]);
                            }
                        }
                    }

                    return node;
                }
            }
            return null;
        }
    }
}
