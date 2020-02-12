using System;
using System.Text;

namespace AillieoUtils.EasyBehaviorTree
{

    [Serializable]
    public abstract class NodeBase : INode
    {

        protected BehaviorTree behaviorTree;

        public string name;

        public string[] paramInfo;

        protected NodeState nodeState { get; private set; } = NodeState.Raw;

        public virtual void DumpNode(StringBuilder stringBuilder, INodeInfoFormatter formatter, int level = 0)
        {
            if (stringBuilder == null)
            {
                return;
            }

            if (formatter == null)
            {
                return;
            }

            stringBuilder.AppendLine(formatter.FormatNodeInfo(ExtractNodeInfo(level)));

        }

        private NodeInfo ExtractNodeInfo(int level)
        {
            return new NodeInfo()
            {
                name = this.name,
                type = this.GetType(),
                paramInfo = this.paramInfo,
                nodeState = this.nodeState,
                level = level,
            };
        }

        public static void InitNode(NodeBase node, BehaviorTree behaviorTree)
        {
            if(node.nodeState == NodeState.Raw)
            {
                node.nodeState = NodeState.Ready;
                node.behaviorTree = behaviorTree;
                node.Init();
            }
        }

        public static BTState NodeTick(NodeBase node, float deltaTime)
        {
            if (node.nodeState == NodeState.Raw)
            {
                throw new Exception("A raw node can not tick!");
            }

            if (node.nodeState == NodeState.Visited)
            {
                throw new Exception("A visited node can not tick!");
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

        public virtual void OnTreeCleanUp()
        {
            Cleanup();
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
        public virtual bool Validate(out string error)
        {
            error = null;
            return true;
        }
#endif
    }

}
