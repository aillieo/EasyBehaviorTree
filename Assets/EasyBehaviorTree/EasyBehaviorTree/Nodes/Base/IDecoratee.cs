using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

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
