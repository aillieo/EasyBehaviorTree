using System;


namespace AillieoUtils.EasyBehaviorTree
{

    [Serializable]
    public class NodeConditionRandom : NodeCondition
    {
        [NodeParam]
        public float passProbability;

        public override void Cleanup(BehaviorTreeVisitor behaviorTreeVisitor)
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

        protected override bool CheckCondition(BehaviorTreeVisitor behaviorTreeVisitor, float deltaTime)
        {
            return behaviorTreeVisitor.random.NextDouble() < passProbability;
        }

    }
}
