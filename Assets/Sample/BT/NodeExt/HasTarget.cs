using AillieoUtils.EasyBehaviorTree;
using System;

[Serializable]
public class HasTarget : NodeCondition
{
    public override void Cleanup(BehaviorTreeVisitor behaviorTreeVisitor)
    {

    }

    protected override bool CheckCondition(BehaviorTreeVisitor behaviorTreeVisitor, float deltaTime)
    {
        Hero target = behaviorTreeVisitor.blackboard.SafeGet("target") as Hero;
        return target != null && target.alive;
    }
}
