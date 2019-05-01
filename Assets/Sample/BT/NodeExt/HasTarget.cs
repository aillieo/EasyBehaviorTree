using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyBehaviorTree;
using System;

[Serializable]
public class HasTarget : NodeCondition
{
    public override void Cleanup()
    {

    }

    protected override bool CheckCondition(float deltaTime)
    {
        Hero target = behaviorTree.blackBoard["target"] as Hero;
        return target != null && target.alive;
    }
}
