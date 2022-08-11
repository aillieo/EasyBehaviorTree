using AillieoUtils.EasyBehaviorTree;
using System;
using UnityEngine;

[Serializable]
[NodeIcon("Assets/Sample/BT/NodeExt/AttackTarget.png")]
public class AttackTarget : NodeAction
{
    public override IBTTask CreateBTTask(BehaviorTreeVisitor behaviorTreeVisitor)
    {
        return new AttackTargetTask();
    }

    public override void Reset(BehaviorTreeVisitor behaviorTreeVisitor)
    {
        base.Reset(behaviorTreeVisitor);
    }

    public class AttackTargetTask : IBTTask
    {
        public void Init(BehaviorTreeVisitor behaviorTreeVisitor)
        {
        }

        public BTState ExecuteTask(BehaviorTreeVisitor behaviorTreeVisitor, float deltaTime)
        {
            Hero self = behaviorTreeVisitor.blackboard["self"] as Hero;
            Hero target = behaviorTreeVisitor.blackboard["target"] as Hero;

            Vector3 dir = target.transform.position - self.transform.position;
            if (dir.sqrMagnitude > 0)
            {
                self.transform.forward = dir;
            }

            Bullet bullet = GameManager.Instance.GetBullet(self.transform);
            bullet.SetTarget(target.transform.position);

            return BTState.Success;
        }

        public void Cleanup(BehaviorTreeVisitor behaviorTreeVisitor)
        {
        }
    }
}
