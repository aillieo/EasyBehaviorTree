using System.Collections;
using System.Collections.Generic;
using System;

namespace EasyBehaviorTree
{
    [Serializable]
    public abstract class NodeCondition : NodeDecoratee
    {

        protected abstract bool CheckCondition(float deltaTime);

        public override BTState Update(float deltaTime)
        {
            if(CheckCondition(deltaTime))
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
