using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyBehaviorTree;
using UnityEngine.SceneManagement;
using System;

[Serializable]
public class FindTarget : NodeAction
{
    public override void Cleanup()
    {

    }

    protected override BTState ExecuteTask(float deltaTime)
    {
        Hero self = behaviorTree.blackBoard["self"] as Hero;
        Hero target = null;
        float sqrDis = float.MaxValue;
        foreach (var hero in GameManager.Instance.GetHeroes())
        {
            if (hero != null && hero.alive && hero != self)
            {
                float newSqrDis = (hero.transform.position - self.transform.position).sqrMagnitude;
                if(newSqrDis < sqrDis)
                {
                    sqrDis = newSqrDis;
                    target = hero;
                }
            }
        }

        if(target != null)
        {
            behaviorTree.blackBoard["target"] = target;
            Vector3 dir = target.transform.position - self.transform.position;
            if(dir.sqrMagnitude >0)
            {
                self.transform.forward = dir;
            }
            return BTState.Success;
        }
        return BTState.Failure;
    }
}
