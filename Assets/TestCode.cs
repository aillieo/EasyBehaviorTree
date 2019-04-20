using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyBehaviorTree;

public class TestCode : MonoBehaviour
{
    void Start()
    {
        BehaviorTree behaviorTree = null;

        string fullPath = Application.dataPath + "/BT_GameObject.bt";
        AillieoUtils.SerializeHelper.DeserializeBytesToData(fullPath, out behaviorTree);

        behaviorTree.Init();
        behaviorTree.Tick();
    }

}
