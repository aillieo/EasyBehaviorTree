using System;


namespace AillieoUtils.EasyBehaviorTree
{

    [Serializable]
    public class NodeConditionRandom : NodeCondition
    {
        [NodeParam]
        public readonly float passProbability;

        public override void Cleanup()
        {

        }

        public override bool Validate(out string error)
        {
            if(!base.Validate(out error))
            {
                return false;
            }

            if(passProbability > 1f || passProbability < 0f)
            {
                error = "Invalid passProbability";
            }
            return error == null;
        }

        protected override bool CheckCondition(float deltaTime)
        {
            return behaviorTree.random.NextDouble() < passProbability;
        }

    }
}
