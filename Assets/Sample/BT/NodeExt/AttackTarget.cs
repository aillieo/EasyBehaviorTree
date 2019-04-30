using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyBehaviorTree;
using UnityEngine.SceneManagement;
using System;

[Serializable]
public class AttackTarget : NodeAction
{
    public override void Cleanup()
    {

    }

    protected override BTState ExecuteTask(float deltaTime)
    {
        Hero self = behaviorTree.blackBoard["self"] as Hero;
        Hero target = behaviorTree.blackBoard["target"] as Hero;

        Bullet bullet = GameManager.Instance.GetBullet(self.transform);
        bullet.SetTarget(target.transform.position);

        return BTState.Success;
    }
}
