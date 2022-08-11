using System;

namespace AillieoUtils.EasyBehaviorTree
{
    public interface IBTTask
    {
        void Init(BehaviorTreeVisitor behaviorTreeVisitor);

        BTState ExecuteTask(BehaviorTreeVisitor behaviorTreeVisitor, float deltaTime);

        void Cleanup(BehaviorTreeVisitor behaviorTreeVisitor);
    }

    [Serializable]
    public abstract class NodeAction : NodeBase
    {
        public abstract IBTTask CreateBTTask(BehaviorTreeVisitor behaviorTreeVisitor);

        public override void Init(BehaviorTreeVisitor behaviorTreeVisitor)
        {
            base.Init(behaviorTreeVisitor);
            IBTTask task = behaviorTreeVisitor.GetTask(this);
            task.Init(behaviorTreeVisitor);
        }

        public override BTState Update(BehaviorTreeVisitor behaviorTreeVisitor, float deltaTime)
        {
            IBTTask task = behaviorTreeVisitor.GetTask(this);
            return task.ExecuteTask(behaviorTreeVisitor, deltaTime);
        }

        public override void Cleanup(BehaviorTreeVisitor behaviorTreeVisitor)
        {
            IBTTask task = behaviorTreeVisitor.GetTask(this);
            task.Cleanup(behaviorTreeVisitor);
        }
    }
}
