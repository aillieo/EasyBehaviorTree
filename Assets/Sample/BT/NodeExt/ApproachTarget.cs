using UnityEngine;
using AillieoUtils.EasyBehaviorTree;
using System;

[Serializable]
[NodeIcon("Assets/Sample/BT/NodeExt/ApproachTarget.png")]
public class ApproachTarget : NodeAction
{
    public override IBTTask CreateBTTask(BehaviorTreeVisitor behaviorTreeVisitor)
    {
        return new ApproachTargetTask();
    }

    public class ApproachTargetTask : IBTTask
    {
        public void Init(BehaviorTreeVisitor behaviorTreeVisitor)
        {
        }

        public BTState ExecuteTask(BehaviorTreeVisitor behaviorTreeVisitor, float deltaTime)
        {
            Hero self = behaviorTreeVisitor.blackboard["self"] as Hero;
            Hero target = behaviorTreeVisitor.blackboard["target"] as Hero;
            if (self == null || target == null)
            {
                return BTState.Failure;
            }
            Vector3 dir = target.transform.position - self.transform.position;
            if (dir.sqrMagnitude > 0)
            {
                self.transform.forward = dir;
            }

            dir.Normalize();
            Vector3 move = dir * self.speed * deltaTime;

            self.transform.position += move;

            return BTState.Success;
        }

        public void Cleanup(BehaviorTreeVisitor behaviorTreeVisitor)
        {
        }
    }
}
