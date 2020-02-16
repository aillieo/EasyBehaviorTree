using UnityEngine;
using AillieoUtils.EasyBehaviorTree;
using System;

[Serializable]
[NodeIcon("Assets/Sample/BT/NodeExt/ApproachTarget.png")]
public class ApproachTarget : NodeAction
{
    public override void Cleanup()
    {

    }

    protected override BTState ExecuteTask(float deltaTime)
    {
        Hero self = behaviorTree.blackBoard["self"] as Hero;
        Hero target = behaviorTree.blackBoard["target"] as Hero;
        if (self == null || target == null)
        {
            return BTState.Failure;
        }
        Vector3 dir = target.transform.position - self.transform.position;
        if(dir.sqrMagnitude >0)
        {
            self.transform.forward = dir;
        }

        dir.Normalize();
        Vector3 move = dir * self.speed * deltaTime;

        self.transform.position += move;

        return BTState.Success;
    }
}
