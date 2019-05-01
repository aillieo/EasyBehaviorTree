using System.Collections;
using System.Collections.Generic;
using EasyBehaviorTree;
using System;


namespace EasyBehaviorTree
{

    [Serializable]
    public class NodeConditionHasSharedValue : NodeCondition
    {
        [NodeParam]
        public string key { get; set; }

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
            return BehaviorTree.sharedBlackBoard.HasValue(key);
        }
    }
}