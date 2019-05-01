using System.Collections;
using System.Collections.Generic;
using System;

namespace EasyBehaviorTree
{
    [Serializable]
    public class NodeUntilSuccess : NodeDecorator
    {
        public override void Cleanup()
        {

        }

        public override BTState Update(float deltaTime)
        {
            var ret = TickNode(Child, deltaTime);
            if(ret == BTState.Success)
            {
                return BTState.Success;
            }
            return BTState.Running;
        }
    }
}
