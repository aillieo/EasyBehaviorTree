using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyBehaviorTree;

public class Hero : MonoBehaviour, IBlackBoardData
{
    private BehaviorTree behaviorTree;

    public float speed = 1.0f;
    public float attackRange = 3.0f;

    void Start()
    {
        string fullPath = Application.dataPath + "/Sample/BT/BT_Hero.bt";
        behaviorTree = BehaviorTree.LoadBehaviorTree(fullPath);

        if(behaviorTree != null)
        {
            behaviorTree.Restart();
            behaviorTree.blackBoard["self"] = this;

            behaviorTree.OnBehaviorTreeCompleted += (bt,st) => bt.Restart();
        }
    }

    void Update()
    {
        if(behaviorTree != null)
        {
            behaviorTree.Tick(Time.deltaTime);
        }
    }
}