using System;

namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public abstract class NodePrecondition : NodeDecorator
    {
        protected abstract bool CheckPrecondition(BehaviorTreeVisitor behaviorTreeVisitor, float deltaTime);

        public override BTState Update(BehaviorTreeVisitor behaviorTreeVisitor, float deltaTime)
        {
            if (CheckPrecondition(behaviorTreeVisitor, deltaTime))
            {
                return NodeTick(Child, behaviorTreeVisitor, deltaTime);
            }
            else
            {
                return BTState.Failure;
            }
        }
    }
}
