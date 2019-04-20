using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

namespace EasyBehaviorTree
{

    public interface INode
    {
        void Init();
        BTState Update();
        void OnEnter();
        void OnExit();
        void Destroy();
    }
}
