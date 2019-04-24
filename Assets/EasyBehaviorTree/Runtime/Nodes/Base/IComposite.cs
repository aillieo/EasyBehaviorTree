using System.Collections;
using System.Collections.Generic;
using System;

namespace EasyBehaviorTree
{
    public interface IComposite
    {
        IList<NodeBase> Children { get; }

#if UNITY_EDITOR

        void AddChild(NodeBase node);

#endif

    }
}
