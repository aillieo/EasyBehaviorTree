using System;

namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public class NodeUntilFailure : NodeDecorator
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
                    ResetNode(Child);
                    break;
                case BTState.Running:
                    break;
                case BTState.Failure:
                    return BTState.Success;
            }

            return BTState.Running;
        }
    }
}
