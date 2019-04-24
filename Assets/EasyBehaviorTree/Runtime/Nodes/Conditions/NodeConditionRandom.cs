using System.Collections;
using System.Collections.Generic;
using EasyBehaviorTree;
using System;

[Serializable]
public class NodeConditionRandom : NodeCondition
{
    public float passProbability { get; set; }

    public override void Destroy()
    {

    }

    public override void Init()
    {
        base.Init();
    }

    protected override bool CheckCondition(float deltaTime)
    {
        return behaviorTree.random.NextDouble() < passProbability;
    }

}
