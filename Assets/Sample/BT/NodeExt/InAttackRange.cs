using UnityEngine;
using AillieoUtils.EasyBehaviorTree;
using System;

[Serializable]
public class InAttackRange : NodeCondition
{
    public override void Cleanup(BehaviorTreeVisitor behaviorTreeVisitor)
    {

    }

    protected override bool CheckCondition(BehaviorTreeVisitor behaviorTreeVisitor, float deltaTime)
    {
        Hero self = behaviorTreeVisitor.blackboard["self"] as Hero;
        Hero target = behaviorTreeVisitor.blackboard["target"] as Hero;
        if(self == null || target == null || !target.alive)
        {
            return false;
        }
        float distance = Vector3.Distance(self.transform.position, target.transform.position);
        return distance < self.attackRange;
    }
}
