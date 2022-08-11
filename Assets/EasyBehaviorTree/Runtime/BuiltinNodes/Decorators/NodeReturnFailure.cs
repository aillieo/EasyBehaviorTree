using System;

namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public class NodeReturnFailure : NodeDecorator
    {
        public override void Cleanup(BehaviorTreeVisitor behaviorTreeVisitor)
        {

        }

        public override BTState Update(BehaviorTreeVisitor behaviorTreeVisitor, float deltaTime)
        {
            var ret = NodeTick(Child, behaviorTreeVisitor, deltaTime);
            if(ret == BTState.Running)
            {
                return ret;
            }
            return BTState.Failure;
        }
    }
}
