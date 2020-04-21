using System;

namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public class NodeInverter : NodeDecorator
    {
        protected override void Cleanup()
        {

        }

        protected override BTState Update(float deltaTime)
        {
            var ret = NodeTick(Child, deltaTime);
            return Invert(ret);
        }

        private BTState Invert(BTState state)
        {
            switch (state)
            {
                case BTState.Failure:
                    return BTState.Success;
                case BTState.Success:
                    return BTState.Failure;
                default:
                    return BTState.Running;
            }
        }
    }
}
