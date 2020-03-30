using System;

namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public class NodeUntilSuccess : NodeDecorator
    {
        protected override void Cleanup()
        {

        }

        protected override BTState Update(float deltaTime)
        {
            var ret = NodeTick(Child, deltaTime);
            switch (ret)
            {
            case BTState.Success:
                return BTState.Success;
            case BTState.Running:
                break;
            case BTState.Failure:
                ResetNode(Child);
                break;
            }

            return BTState.Running;
        }
    }
}
