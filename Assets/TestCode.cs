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
        AillieoUtils.SerializeHelper.DeserializeBytesToData(fullPath, out behaviorTree);

        behaviorTree.Init();
        
    }

    void Update()
    {
        behaviorTree.Tick(Time.deltaTime);
    }
}
