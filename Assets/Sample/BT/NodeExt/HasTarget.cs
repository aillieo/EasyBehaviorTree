using AillieoUtils.EasyBehaviorTree;
using System;

[Serializable]
public class HasTarget : NodeCondition
{
    public override void Cleanup()
    {

    }

    protected override bool CheckCondition(float deltaTime)
    {
        Hero target = behaviorTree.blackBoard.SafeGet("target") as Hero;
        return target != null && target.alive;
    }
}
