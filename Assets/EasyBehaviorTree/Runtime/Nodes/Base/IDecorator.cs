using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

namespace EasyBehaviorTree
{
    public interface IDecorator
    {
        NodeDecoratee Child
        {
            get;
#if UNITY_EDITOR
            set;
#endif
        }
    }
}
