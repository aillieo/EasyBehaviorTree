using System;

namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public class NodeInvert : NodeDecorator
    {
        public override void Cleanup(BehaviorTreeVisitor behaviorTreeVisitor)
        {

        }

        public override BTState Update(BehaviorTreeVisitor behaviorTreeVisitor, float deltaTime)
        {
            var ret = NodeTick(Child, behaviorTreeVisitor, deltaTime);
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
