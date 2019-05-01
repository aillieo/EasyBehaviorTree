using System.Collections;
using System.Collections.Generic;
using System;

namespace EasyBehaviorTree
{
    [Serializable]
    public class NodeReturnSuccess : NodeDecorator
    {
        public override void Cleanup()
        {

        }

        public override BTState Update(float deltaTime)
        {
            var ret = TickNode(Child, deltaTime);
            if(ret == BTState.Running)
            {
                return ret;
            }
            return BTState.Success;
        }
    }
}
