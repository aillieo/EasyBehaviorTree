using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EasyBehaviorTree
{
    [Serializable]
    public abstract class NodeCondition : NodeDecoratee
    {

        protected abstract bool CheckCondition();

        public override BTState Update()
        {
            if(CheckCondition())
            {
                return BTState.Success;
            }
            else
            {
                return BTState.Failure;
            }
        }
    }
}
