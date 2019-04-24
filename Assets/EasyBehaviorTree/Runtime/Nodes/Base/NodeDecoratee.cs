using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

namespace EasyBehaviorTree
{

    [Serializable]
    public abstract class NodeDecoratee : NodeBase, IDecoratee
    {
        public NodeDecorator decorator
        {
            get;
#if UNITY_EDITOR
            set;
#endif
        }

    }
}
