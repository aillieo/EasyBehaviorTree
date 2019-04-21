using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

namespace EasyBehaviorTree
{

    [Serializable]
    public abstract class NodeDecoratee : NodeBase, IDecoratee
    {

        protected List<NodeDecorator> mDecorators = new List<NodeDecorator>();

        public IList<NodeDecorator> Decorators => mDecorators;



#if UNITY_EDITOR
        public void AddDecorator(NodeDecorator decorator)
        {
            mDecorators.Add(decorator);
        }
#endif

    }
}
