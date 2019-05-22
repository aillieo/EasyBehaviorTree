using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

namespace EasyBehaviorTree.Creator
{

    [Serializable]
    public class NodeParamObject
    {
    }


    [Serializable]
    public class NodeParam<T>: NodeParamObject
    {
        [SerializeField]
        public string key;

        [SerializeField]
        public T value;

        public virtual Type GetValueType()
        {
            return typeof(T);
        }
    }
}
