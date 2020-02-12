using AillieoUtils.EasyBehaviorTree;
using System;

[Serializable]
[NodeIcon("Assets/Sample/BT/NodeExt/AttackTarget.png")]
public class AttackTarget : NodeAction
{
    public override void Cleanup()
    {

    }

    protected override BTState ExecuteTask(float deltaTime)
    {
        Hero self = behaviorTree.blackBoard["self"] as Hero;
        Hero target = behaviorTree.blackBoard["target"] as Hero;

        Bullet bullet = GameManager.Instance.GetBullet(self.transform);
        bullet.SetTarget(target.transform.position);

        return BTState.Success;
    }
}
