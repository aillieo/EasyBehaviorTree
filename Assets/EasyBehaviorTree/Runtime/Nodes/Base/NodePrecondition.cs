using System;

namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public abstract class NodePrecondition : NodeDecorator
    {
        protected abstract bool CheckPrecondition(float deltaTime);

        public override BTState Update(float deltaTime)
        {
            if(CheckPrecondition(deltaTime))
            {
                return NodeTick(Child, deltaTime);
            }
            else
            {
                return BTState.Failure;
            }
        }
    }
}
