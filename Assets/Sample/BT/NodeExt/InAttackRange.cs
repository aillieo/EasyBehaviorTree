using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyBehaviorTree;
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
        float distance = Vector3.Distance(self.transform.position, target.transform.position);
        
        return distance < self.attackRange;
    }
}
