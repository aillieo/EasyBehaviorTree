using System;

namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public class NodeReturnFailure : NodeDecorator
    {
        public override void Cleanup()
        {

        }

        public override BTState Update(float deltaTime)
        {
            var ret = NodeTick(Child, deltaTime);
            if(ret == BTState.Running)
            {
                return ret;
            }
            return BTState.Failure;
        }
    }
}
