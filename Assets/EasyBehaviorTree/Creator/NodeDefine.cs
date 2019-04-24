using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Linq;

namespace EasyBehaviorTree.Creator
{
    [DisallowMultipleComponent]
    public class NodeDefine : MonoBehaviour
    {
        [HideInInspector]
        public string nodeFullName;
        [HideInInspector]
        public string assemblyName;

        [HideInInspector]
        public string displayName;


        // =============================================================================================================================
        [HideInInspector][SerializeField]
        public StringParamSet stringParamSet = new StringParamSet();
        [HideInInspector][SerializeField]
        public FloatParamSet floatParamSet = new FloatParamSet();
        [HideInInspector][SerializeField]
        public IntParamSet intParamSet = new IntParamSet();
        [HideInInspector][SerializeField]
        public BoolParamSet boolParamSet = new BoolParamSet();
        [HideInInspector][SerializeField]
        public EnumParamSet enumParamSet = new EnumParamSet();
        // =============================================================================================================================


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
                        node.name = displayName;
                        var properties = t.GetProperties();
                        foreach (var property in properties)
                        {
                            // =============================================================================================================================
                            stringParamSet.TrySetPropertyForType(property, node);
                            floatParamSet.TrySetPropertyForType(property, node);
                            intParamSet.TrySetPropertyForType(property, node);
                            boolParamSet.TrySetPropertyForType(property, node);
                            enumParamSet.TrySetPropertyForType(property, node);
                            // =============================================================================================================================
                        }
                    }

                    return node;
                }
            }
            return null;
        }
    }
}
