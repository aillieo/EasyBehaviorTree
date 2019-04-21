using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

namespace EasyBehaviorTree
{

    [Serializable]
    public abstract class NodeBase : INode
    {

        public BehaviorTree behaviorTree;

        public string name;

        public virtual void DumpNode(StringBuilder stringBuilder, int level = 0)
        {
            if (stringBuilder == null)
            {
                return;
            }

            stringBuilder.Append(new string('-',level));
            stringBuilder.Append(ToString());
            stringBuilder.AppendLine();
        }

        public override string ToString()
        {
            return string.Format("{0}({1})",name, GetType().Name);
        }

        public static void Init(NodeBase node, BehaviorTree behaviorTree)
        {
            node.behaviorTree = behaviorTree;
            node.Init();
        }

        public static BTState TickNode(NodeBase node)
        {
            node.OnEnter();
            BTState ret = node.Update();
            node.OnExit();
            return ret;
        }

        public virtual void Init()
        {
            Debug.Log(ToString() + " : Init");
        }

        public virtual void OnEnter() {
            Debug.Log(ToString() +  " : OnEnter");
        }
        public virtual void OnExit() {
            Debug.Log(ToString() + " : OnExit");
        }

        public abstract BTState Update();
        public abstract void Destroy();
    }

}
