using UnityEngine;
using AillieoUtils.EasyBehaviorTree;

public class Hero : MonoBehaviour, IBlackBoardData
{
    private BehaviorTree behaviorTree;

    public float speed = 1.0f;
    public float attackRange = 3.0f;
    public int hp = 500;
    private int hpCurrent;

    public bool alive { get { return hpCurrent >= 0; } }

    public void Restart()
    {
        hpCurrent = hp;

        string fullPath = Application.dataPath + "/Sample/BT/BT_Hero.bt";
        behaviorTree = BehaviorTree.LoadBehaviorTree(fullPath);

        if(behaviorTree != null)
        {
            behaviorTree.Restart();
            behaviorTree.blackBoard["self"] = this;

            behaviorTree.OnBehaviorTreeCompleted += (bt,st) => bt.Restart();
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
