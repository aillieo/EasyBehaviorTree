using System;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace EasyBehaviorTree
{
    public static class NodeParamSetCollector
    {

        public static Type[] CollectNodeParamSetTypes()
        {
            Type[] nodeParamSetTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes().Where(
                t => IsNodeParamSetType(t))).ToArray();
            return nodeParamSetTypes;
        }

        public static bool IsNodeParamSetType(Type t)
        {
            if (t.BaseType == null)
            {
                return false;
            }
            if (!t.BaseType.IsGenericType)
            {
                return false;
            }
            if (t.BaseType.GetGenericTypeDefinition() == typeof(EasyBehaviorTree.NodeParamSet<,>))
            {
                return true;
            }
            return false;
        }

    }
}
