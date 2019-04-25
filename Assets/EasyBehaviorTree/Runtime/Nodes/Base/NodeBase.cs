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

        public string fullName;

        public string briefInfo;

        protected NodeState nodeState { get; private set; } = NodeState.Raw;

        public virtual void DumpNode(StringBuilder stringBuilder, bool withBriefInfo, int level = 0)
        {
            if (stringBuilder == null)
            {
                return;
            }

            stringBuilder.Append(new string('-', level));
            stringBuilder.Append(fullName);
            if(withBriefInfo)
            {
                stringBuilder.Append(" ");
                stringBuilder.Append(briefInfo);
            }
            stringBuilder.AppendLine();
        }

        public static void InitNode(NodeBase node, BehaviorTree behaviorTree)
        {
            if(node.nodeState == NodeState.Raw)
            {
                node.nodeState = NodeState.Ready;
                node.behaviorTree = behaviorTree;
                node.Init();
            }
            else
            {
                behaviorTree.logger.Warning("Trying to init node more than once! " + node.ToString());
            }
        }

        public static BTState TickNode(NodeBase node, float deltaTime)
        {
            if (node.nodeState == NodeState.Raw)
            {
                throw new Exception("Trying to tick a raw node!");
            }

            if (node.nodeState == NodeState.Visited)
            {
                throw new Exception("Trying to tick a visited node!");
            }

            // first time
            if (node.nodeState == NodeState.Ready)
            {
                node.OnEnter();
                node.nodeState = NodeState.Visiting;
            }

            // regular tick
            BTState ret = node.Update(deltaTime);
            if (ret == BTState.Running)
            {
                return ret;
            }

            // last time
            node.OnExit();
            node.nodeState = NodeState.Visited;
            return ret;
        }

        public static void ResetNode(NodeBase node)
        {
            if(node.nodeState == NodeState.Raw)
            {
                node.behaviorTree.logger.Warning("Should Init a node before Reset");
                node.Init();
            }

            node.nodeState = NodeState.Ready;
            node.Reset();
        }

        public virtual void Init()
        {
            behaviorTree.logger.Debug(ToString() + " : Init");
        }

        public virtual void Reset()
        {
            behaviorTree.logger.Debug(ToString() + " : Reset");
        }

        public virtual void OnEnter()
        {
            behaviorTree.logger.Debug(ToString() + " : OnEnter");
        }

        public virtual void OnExit()
        {
            behaviorTree.logger.Debug(ToString() + " : OnExit");
        }

        public abstract BTState Update(float deltaTime);
        public abstract void Cleanup();

#if UNITY_EDITOR
        public abstract bool Validate(out string error);
#endif
    }

}
