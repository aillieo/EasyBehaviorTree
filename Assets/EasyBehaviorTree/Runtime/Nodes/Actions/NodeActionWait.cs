using System.Collections;
using System.Collections.Generic;
using EasyBehaviorTree;
using System;

[Serializable]
public class NodeActionWait : NodeAction
{

    public float time { get; set; }

    private float timer;

    public override void Destroy()
    {

    }

    public override void Init()
    {
        base.Init();
        timer = 0f;
    }

    protected override BTState ExecuteTask(float deltaTime)
    {
        timer += deltaTime;
        if(timer > time)
        {
            timer = 0;
            return BTState.Success;
        }

        return BTState.Running;
    }
}
