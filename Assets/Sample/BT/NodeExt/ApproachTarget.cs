using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyBehaviorTree;
using UnityEngine.SceneManagement;
using System;

[Serializable]
public class ApproachTarget : NodeAction
{
    public override void Cleanup()
    {

    }

    protected override BTState ExecuteTask(float deltaTime)
    {
        Hero self = behaviorTree.blackBoard["self"] as Hero;
        Hero target = behaviorTree.blackBoard["target"] as Hero;

        Vector3 dir = target.transform.position - self.transform.position;
        dir.Normalize();
        Vector3 move = dir * self.speed * deltaTime;

        self.transform.position += move;

        return BTState.Success;
    }
}
