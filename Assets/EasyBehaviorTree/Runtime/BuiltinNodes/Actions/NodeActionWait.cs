using System;

namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public class NodeActionWait : NodeAction
    {
        [NodeParam]
        public float time;

        public override bool Validate(out string error)
        {
            error = null;
            if (time < 0)
            {
                error = "Invalid time";
            }
            return error == null;
        }

        public override IBTTask CreateBTTask(BehaviorTreeVisitor behaviorTreeVisitor)
        {
            return new WaitTask(time);
        }
        
        public class WaitTask : IBTTask
        {
            private readonly float time;
            private float timer;

            public BTState ExecuteTask(BehaviorTreeVisitor behaviorTreeVisitor, float deltaTime)
            {
                timer += deltaTime;
                if (timer > time)
                {
                    timer = 0;
                    return BTState.Success;
                }

                return BTState.Running;
            }

            public void Init(BehaviorTreeVisitor behaviorTreeVisitor)
            {
                timer = 0f;
            }

            public void Cleanup(BehaviorTreeVisitor behaviorTreeVisitor)
            {
                timer = 0f;
            }

            public WaitTask(float time)
            {
                this.time = time;
            }
        }
    }
}
