using System;


namespace AillieoUtils.EasyBehaviorTree
{

    [Serializable]
    public class NodeConditionHasSharedValue : NodeCondition
    {
        [NodeParam]
        public string key;

        public override void Cleanup(BehaviorTreeVisitor behaviorTreeVisitor)
        {

        }

        public override bool Validate(out string error)
        {
            if (!base.Validate(out error))
            {
                return false;
            }

            if (string.IsNullOrEmpty(key))
            {
                error = "Invalid key";
            }
            return error == null;
        }

        protected override bool CheckCondition(BehaviorTreeVisitor behaviorTreeVisitor, float deltaTime)
        {
            return behaviorTreeVisitor.sharedBlackboard.HasValue(key);
        }
    }
}

