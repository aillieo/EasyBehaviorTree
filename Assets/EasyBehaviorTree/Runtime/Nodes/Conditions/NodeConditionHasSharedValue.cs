using System;


namespace AillieoUtils.EasyBehaviorTree
{

    [Serializable]
    public class NodeConditionHasSharedValue : NodeCondition
    {
        [NodeParam]
        public readonly string key;

        public override void Cleanup()
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

        protected override bool CheckCondition(float deltaTime)
        {
            return BehaviorTree.sharedBlackBoard.HasValue(key);
        }
    }
}
