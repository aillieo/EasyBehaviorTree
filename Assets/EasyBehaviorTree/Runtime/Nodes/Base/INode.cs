using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

namespace EasyBehaviorTree
{

    public interface INode
    {
        void Init();
        BTState Update(float deltaTime);
        void OnEnter();
        void OnExit();
        void Destroy();
    }
}
