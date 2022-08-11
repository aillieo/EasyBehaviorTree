using System;

namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public class NodeUntilFailure : NodeDecorator
    {
        public override void Cleanup(BehaviorTreeVisitor behaviorTreeVisitor)
        {

        }

        public override BTState Update(BehaviorTreeVisitor behaviorTreeVisitor, float deltaTime)
        {
            var ret = NodeTick(Child, behaviorTreeVisitor, deltaTime);
            switch (ret)
            {
                case BTState.Success:
                    ResetNode(Child, behaviorTreeVisitor);
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
