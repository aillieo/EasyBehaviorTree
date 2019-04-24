using System.Collections;
using System.Collections.Generic;
using System;

namespace EasyBehaviorTree
{

    public interface IDecoratee
    {
        IList<NodeDecorator> Decorators { get;}

#if UNITY_EDITOR
        void AddDecorator(NodeDecorator decorator);
#endif

    }
}
