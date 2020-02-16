using System;


namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public class NodeActionLog : NodeAction
    {
        [NodeParam]
        public readonly string logStr;

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
