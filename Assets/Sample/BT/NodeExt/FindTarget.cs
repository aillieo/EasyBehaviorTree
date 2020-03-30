using UnityEngine;
using AillieoUtils.EasyBehaviorTree;
using System;

[Serializable]
[NodeIcon("Assets/Sample/BT/NodeExt/FindTarget.png")]
public class FindTarget : NodeAction
{
    protected override void Cleanup()
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
            return BTState.Success;
        }
        return BTState.Failure;
    }
}
