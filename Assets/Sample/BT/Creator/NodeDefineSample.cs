using UnityEngine;
using System;
using AillieoUtils.EasyBehaviorTree.Creator;
using AillieoUtils.EasyBehaviorTree;

public class NodeDefineSample : NodeDefineBase
{
    [SerializeField]
    [HideInInspector]
    private string nodeFullName;

    [SerializeField]
    [HideInInspector]
    private string displayName;
    [SerializeField]
    [HideInInspector]
    private string nodeDescription;
    
    public override Type GetNodeType()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            Type t = assembly.GetType(nodeFullName);
            if (t != null)
            {
                return t;
            }
        }
        return null;
    }

    public override NodeBase CreateNode()
    {
        Type t = GetNodeType();
        if (t != null)
        {
            NodeBase node = Activator.CreateInstance(t) as NodeBase;
            if (node != null)
            {
                node.nodeName = displayName;
            }

            PostProcessNode(ref node);

            return node;
        }

        return null;
    }

    private void PostProcessNode(ref NodeBase node)
    {
        switch (node)
        {
            case ApproachTarget approachTarget:
                break;
            case AttackTarget attackTarget:
                break;
            case FindTarget findTarget:
                break;
            case HasTarget hasTarget:
                break;
            case InAttackRange inAttackRange:
                break;
        }
    }
}
