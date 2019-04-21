using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyBehaviorTree;
using System;

[Serializable]
public class NodeActionLog : NodeAction
{

    public string logStr { get; set; }

    public override void Destroy()
    {

    }

    public override void Init()
    {
        base.Init();
    }

    protected override BTState ExecuteTask()
    {
        if(string.IsNullOrEmpty(logStr))
        {
            return BTState.Failure;
        }
        Debug.Log(logStr);
        return BTState.Success;
    }
}

