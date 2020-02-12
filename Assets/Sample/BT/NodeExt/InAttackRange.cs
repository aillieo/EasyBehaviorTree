using UnityEngine;
using AillieoUtils.EasyBehaviorTree;
using System;

[Serializable]
public class InAttackRange : NodeCondition
{
    public override void Cleanup()
    {

    }

    protected override bool CheckCondition(float deltaTime)
    {
        Hero self = behaviorTree.blackBoard["self"] as Hero;
        Hero target = behaviorTree.blackBoard["target"] as Hero;
        if(self == null || target == null || !target.alive)
        {
            return false;
        }
        float distance = Vector3.Distance(self.transform.position, target.transform.position);
        return distance < self.attackRange;
    }
}
