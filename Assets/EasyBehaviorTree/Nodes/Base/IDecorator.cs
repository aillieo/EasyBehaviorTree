using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

namespace EasyBehaviorTree
{
    public interface IDecorator
    {
        IDecoratee Target { get;
#if UNITY_EDITOR
            set;
#endif
        }


        BTState PreUpdate();
        BTState PostUpdate();

    }
}
