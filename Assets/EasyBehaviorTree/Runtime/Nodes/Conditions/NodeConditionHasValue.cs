using System;


namespace AillieoUtils.EasyBehaviorTree
{

    [Serializable]
    public class NodeConditionHasValue : NodeCondition
    {
        [NodeParam]
        private string key;

        public override void Cleanup()
        {

        }


#if UNITY_EDITOR
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
#endif

        protected override bool CheckCondition(float deltaTime)
        {
            return behaviorTree.blackBoard.HasValue(key);
        }
    }
}