using System;

namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public class NodeUntilSuccess : NodeDecorator
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
                return BTState.Success;
            case BTState.Running:
                break;
            case BTState.Failure:
                ResetNode(Child, behaviorTreeVisitor);
                break;
            }

            return BTState.Running;
        }
    }
}
