using System.Collections;
using System.Collections.Generic;
using System;

namespace EasyBehaviorTree
{
    public interface IDecoratee
    {
        NodeDecorator decorator
        {
            get;
#if UNITY_EDITOR
            set;
#endif
        }
    }
}
