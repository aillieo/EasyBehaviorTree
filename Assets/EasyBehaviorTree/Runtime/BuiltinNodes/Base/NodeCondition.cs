using System;

namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public abstract class NodeCondition : NodeBase
    {

        protected abstract bool CheckCondition(BehaviorTreeVisitor behaviorTreeVisitor, float deltaTime);

        public override BTState Update(BehaviorTreeVisitor behaviorTreeVisitor, float deltaTime)
        {
            if(CheckCondition(behaviorTreeVisitor, deltaTime))
            {
                return BTState.Success;
            }
            else
            {
                return BTState.Failure;
            }
        }
    }
}
