using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

namespace EasyBehaviorTree
{

    public interface INode
    {
        void Init();
        void Reset();
        BTState Update(float deltaTime);
        void OnEnter();
        void OnExit();
        void Cleanup();
#if UNITY_EDITOR
        bool Validate(out string error);
#endif
    }
}
