using System;

namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public class NodeUntilSuccess : NodeDecorator
    {
        public override void Cleanup()
        {

        }

        public override BTState Update(float deltaTime)
        {
            var ret = NodeTick(Child, deltaTime);
            if(ret == BTState.Success)
            {
                return BTState.Success;
            }
            return BTState.Running;
        }
    }
}
