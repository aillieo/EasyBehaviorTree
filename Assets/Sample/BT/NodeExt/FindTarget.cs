using UnityEngine;
using AillieoUtils.EasyBehaviorTree;
using System;

[Serializable]
[NodeIcon("Assets/Sample/BT/NodeExt/FindTarget.png")]
public class FindTarget : NodeAction
{
    public override IBTTask CreateBTTask(BehaviorTreeVisitor behaviorTreeVisitor)
    {
        return new FindTargetTask();
    }

    public class FindTargetTask : IBTTask
    {
        public void Cleanup(BehaviorTreeVisitor behaviorTreeVisitor)
        {
        }

        public void Init(BehaviorTreeVisitor behaviorTreeVisitor)
        {
        }

        public BTState ExecuteTask(BehaviorTreeVisitor behaviorTreeVisitor, float deltaTime)
        {
            Hero self = behaviorTreeVisitor.blackboard["self"] as Hero;
            Hero target = null;
            float sqrDis = float.MaxValue;
            foreach (var hero in GameManager.Instance.GetHeroes())
            {
                if (hero != null && hero.alive && hero != self)
                {
                    float newSqrDis = (hero.transform.position - self.transform.position).sqrMagnitude;
                    if (newSqrDis < sqrDis)
                    {
                        sqrDis = newSqrDis;
                        target = hero;
                    }
                }
            }

            if (target != null)
            {
                behaviorTreeVisitor.blackboard["target"] = target;
                return BTState.Success;
            }
            return BTState.Failure;
        }
    }
}
