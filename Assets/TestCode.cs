using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyBehaviorTree;

public class TestCode : MonoBehaviour
{

    BehaviorTree behaviorTree = null;

    void Start()
    {

        string fullPath = Application.dataPath + "/BT_GameObject.bt";
        behaviorTree = BehaviorTree.LoadBehaviorTree(fullPath);


        // behaviorTree.OnBehaviorTreeCompleted += (bt,st) => bt.Restart();


        behaviorTree.Restart();

        /*
        var ts = NodeParamSetCollector.CollectNodeParamSetTypes();
        foreach (var t in ts)
        {
            var args = t.BaseType.GetGenericArguments();
            foreach (var a in args)
            {
                Debug.LogError(a.FullName);
            }
        }
        */

    }

    void Update()
    {
        behaviorTree.Tick(Time.deltaTime);
    }
}
