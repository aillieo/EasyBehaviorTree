using System;


namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public class NodeActionLog : NodeAction
    {
        [NodeParam]
        public string logStr;

        public override bool Validate(out string error)
        {
            error = null;
            return !string.IsNullOrEmpty(logStr);
        }

        public override IBTTask CreateBTTask(BehaviorTreeVisitor behaviorTreeVisitor)
        {
            return new LogTask(logStr);
        }

        public class LogTask : IBTTask
        {
            private string logStr;

            public BTState ExecuteTask(BehaviorTreeVisitor behaviorTreeVisitor, float deltaTime)
            {
                behaviorTreeVisitor.logger.Log(logStr);
                return BTState.Success;
            }

            public void Init(BehaviorTreeVisitor behaviorTreeVisitor)
            {
            }

            public void Cleanup(BehaviorTreeVisitor behaviorTreeVisitor)
            {
            }

            public LogTask(string logStr)
            {
                this.logStr = logStr;
            }
        }
    }
}
