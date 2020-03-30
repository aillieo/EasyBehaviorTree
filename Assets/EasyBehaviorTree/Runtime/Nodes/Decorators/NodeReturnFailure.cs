using System;

namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public class NodeReturnFailure : NodeDecorator
    {
        protected override void Cleanup()
        {

        }

        protected override BTState Update(float deltaTime)
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
