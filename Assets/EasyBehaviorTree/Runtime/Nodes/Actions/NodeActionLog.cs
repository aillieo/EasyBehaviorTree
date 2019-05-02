using System.Collections;
using System.Collections.Generic;
using EasyBehaviorTree;
using System;


namespace EasyBehaviorTree
{
    [Serializable]
    public class NodeActionLog : NodeAction
    {

        [NodeParam]
        private string logStr;

        public override void Cleanup()
        {

        }

        public override bool Validate(out string error)
        {
            error = null;
            return !string.IsNullOrEmpty(logStr);
        }

        protected override BTState ExecuteTask(float deltaTime)
        {
            behaviorTree.logger.Log(logStr);
            return BTState.Success;
        }
    }
}