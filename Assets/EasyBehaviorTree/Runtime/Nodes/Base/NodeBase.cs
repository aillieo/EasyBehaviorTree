using System.Collections;
using System.Collections.Generic;
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

            stringBuilder.Append(new string('-', level));
            stringBuilder.Append(ToString());
            stringBuilder.AppendLine();
        }

        public override string ToString()
        {
            return string.Format("{0}({1})", name, GetType().Name);
        }

        public static void Init(NodeBase node, BehaviorTree behaviorTree)
        {
            node.behaviorTree = behaviorTree;
            node.Init();
        }

        public static BTState TickNode(NodeBase node, float deltaTime)
        {
            node.OnEnter();
            BTState ret = node.Update(deltaTime);
            node.OnExit();
            return ret;
        }

        public virtual void Init()
        {
            behaviorTree.Log(ToString() + " : Init");
        }

        public virtual void OnEnter()
        {
            behaviorTree.Log(ToString() + " : OnEnter");
        }

        public virtual void OnExit()
        {
            behaviorTree.Log(ToString() + " : OnExit");
        }

        public abstract BTState Update(float deltaTime);
        public abstract void Destroy();
    }

}
