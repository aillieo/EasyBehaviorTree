using System;


namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public class NodeActionLog : NodeAction
    {
        [NodeParam]
        private string logStr;

        public override void Cleanup()
        {

        }

#if UNITY_EDITOR
        public override bool Validate(out string error)
        {
            error = null;
            return !string.IsNullOrEmpty(logStr);
        }

#endif

        protected override BTState ExecuteTask(float deltaTime)
        {
            behaviorTree.logger.Log(logStr);
            return BTState.Success;
        }
    }
}
