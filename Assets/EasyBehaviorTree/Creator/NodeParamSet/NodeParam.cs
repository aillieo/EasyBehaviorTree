using System;
using UnityEngine;
#if UNITY_EDITOR

#endif

namespace AillieoUtils.EasyBehaviorTree.Creator
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
