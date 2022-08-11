using System;
using System.Globalization;
using System.Reflection;
using UnityEngine;
using AillieoUtils.EasyBehaviorTree;

public class Hero : MonoBehaviour, IBlackboardData
{
    private BehaviorTree behaviorTree;

    public float speed = 1.0f;
    public float attackRange = 3.0f;
    public int hp = 500;
    private int hpCurrent;

    public bool alive { get { return hpCurrent >= 0; } }

    private static BehaviorTree btProtoType;

    public void Restart()
    {
        hpCurrent = hp;

        if (btProtoType == null)
        {
            string fullPath = Application.dataPath + "/Sample/BT/BT_Hero.bt";
            btProtoType = BytesAssetProcessor.LoadBehaviorTree(fullPath);
        }

        behaviorTree = new BehaviorTree(btProtoType);

        if (behaviorTree != null)
        {
            behaviorTree.Init();
            behaviorTree.Restart();
            behaviorTree.blackboard["self"] = this;

            behaviorTree.OnBehaviorTreeCompleted += (bt, st) =>
            {
                bt.Restart();
                behaviorTree.blackboard["self"] = this;
            };
        }
    }

    public void OnDamage(int damage)
    {
        hpCurrent -= damage;
        if(!alive)
        {
            behaviorTree.CleanUp();
            behaviorTree = null;
            GameManager.Instance.RemoveHero(this);
        }
    }

    void Update()
    {
        if(behaviorTree != null && alive)
        {
            behaviorTree.Tick(Time.deltaTime);
        }
    }
}
