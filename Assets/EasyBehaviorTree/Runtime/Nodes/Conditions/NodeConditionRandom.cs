using System;


namespace AillieoUtils.EasyBehaviorTree
{

    [Serializable]
    public class NodeConditionRandom : NodeCondition
    {
        [NodeParam]
        private float passProbability;

        public override void Cleanup()
        {

        }


#if UNITY_EDITOR
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
#endif

        protected override bool CheckCondition(float deltaTime)
        {
            return behaviorTree.random.NextDouble() < passProbability;
        }

    }
}