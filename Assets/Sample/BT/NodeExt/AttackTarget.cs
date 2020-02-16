using AillieoUtils.EasyBehaviorTree;
using System;
using UnityEngine;

[Serializable]
[NodeIcon("Assets/Sample/BT/NodeExt/AttackTarget.png")]
public class AttackTarget : NodeAction
{
    public override void Cleanup()
    {

    }

    protected override BTState ExecuteTask(float deltaTime)
    {
        Hero self = behaviorTree.blackBoard["self"] as Hero;
        Hero target = behaviorTree.blackBoard["target"] as Hero;

        Vector3 dir = target.transform.position - self.transform.position;
        if(dir.sqrMagnitude >0)
        {
            self.transform.forward = dir;
        }

        Bullet bullet = GameManager.Instance.GetBullet(self.transform);
        bullet.SetTarget(target.transform.position);

        return BTState.Success;
    }
}
