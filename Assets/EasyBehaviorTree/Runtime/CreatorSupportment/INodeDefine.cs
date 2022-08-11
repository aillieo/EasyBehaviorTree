using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

namespace AillieoUtils.EasyBehaviorTree.Creator
{
    public interface INodeDefine
    {
        Type GetNodeType();
        NodeBase CreateNode();
        INodeDefine[] GetChildren();
    }
}
