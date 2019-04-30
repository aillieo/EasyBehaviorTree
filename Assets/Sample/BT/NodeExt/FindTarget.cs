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
        Scene scene = SceneManager.GetActiveScene();
        GameObject[] roots = scene.GetRootGameObjects();
        Hero self = behaviorTree.blackBoard["self"] as Hero;
        foreach (var go in roots)
        {
            Hero[] heroes = go.GetComponentsInChildren<Hero>();
            foreach(var h in heroes)
            {
                if(h != self)
                {
                    behaviorTree.blackBoard["target"] = h;
                    return BTState.Success;
                }
            }

        }
        return BTState.Failure;
    }
}
