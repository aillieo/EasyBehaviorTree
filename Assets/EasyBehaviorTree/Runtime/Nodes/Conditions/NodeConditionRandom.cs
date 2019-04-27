using System.Collections;
using System.Collections.Generic;
using EasyBehaviorTree;
using System;


namespace EasyBehaviorTree
{

    [Serializable]
    public class NodeConditionRandom : NodeCondition
    {
        [NodeParam]
        public float passProbability { get; set; }

        public override void Cleanup()
        {

        }


#if UNITY_EDITOR
        public override bool Validate(out string error)
        {
            error = null;
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