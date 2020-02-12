using System;

namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public class NodeUntilFailure : NodeDecorator
    {
        public override void Cleanup()
        {

        }

        public override BTState Update(float deltaTime)
        {
            var ret = NodeTick(Child, deltaTime);
            if(ret == BTState.Failure)
            {
                return BTState.Success;
            }
            return BTState.Running;
        }
    }
}
