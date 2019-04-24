using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyBehaviorTree;
using AillieoUtils;

public class TestCode : MonoBehaviour
{

    BehaviorTree behaviorTree = null;

    void Start()
    {

        string fullPath = Application.dataPath + "/BT_GameObject.bt";
        Utils.DeserializeBytesToData(fullPath, out behaviorTree);


        // behaviorTree.OnBehaviorTreeEnd += (bt) => bt.Restart();


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
