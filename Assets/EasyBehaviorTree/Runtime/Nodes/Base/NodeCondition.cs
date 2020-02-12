using System;

namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public abstract class NodeCondition : NodeBase
    {

        protected abstract bool CheckCondition(float deltaTime);

        public override BTState Update(float deltaTime)
        {
            if(CheckCondition(deltaTime))
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
