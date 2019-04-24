using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

namespace EasyBehaviorTree
{
    public interface IDecorator
    {
        IDecoratee Target
        {
            get;
#if UNITY_EDITOR
            set;
#endif
        }


        BTState PreUpdate(float deltaTime);
        BTState PostUpdate(float deltaTime);

    }
}
