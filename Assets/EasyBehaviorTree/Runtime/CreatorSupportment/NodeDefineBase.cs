using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace AillieoUtils.EasyBehaviorTree.Creator
{
    [DisallowMultipleComponent]
    public abstract class NodeDefineBase : MonoBehaviour, INodeDefine
    {
        public abstract Type GetNodeType();

        public bool IsRoot()
        {
            return transform.parent == null;
        }

        public abstract NodeBase CreateNode();

        public INodeDefine[] GetChildren()
        {
            List<INodeDefine> children = new List<INodeDefine>();
            IEnumerator enumerator = this.transform.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Transform trans = enumerator.Current as Transform;
                if (trans != null)
                {
                    INodeDefine nodeDefine = trans.GetComponent<NodeDefineBase>();
                    if (nodeDefine != null)
                    {
                        children.Add(nodeDefine);
                    }
                }
            }

            return children.ToArray();
        }
    }
}
