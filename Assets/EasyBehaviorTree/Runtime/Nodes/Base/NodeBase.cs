using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AillieoUtils.EasyBehaviorTree
{

    [Serializable]
    public abstract class NodeBase
    {

        protected BehaviorTree behaviorTree;

        public string name;

        private ParamInfo[] paramInfo;

        protected internal NodeState nodeState { get; private set; } = NodeState.Raw;

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
            if(paramInfo == null)
            {
                paramInfo = ReflectionUtils.GetNodeParamFields(this.GetType()).Select(f => {
                    return new ParamInfo()
                    {
                        name = f.Name,
                        type = f.FieldType,
                        value = f.GetValue(this)
                    };
                }).ToArray();
            }
            return new NodeInfo()
            {
                name = this.name,
                type = this.GetType(),
                paramInfo = this.paramInfo,
                nodeState = this.nodeState,
                level = level,
            };
        }

        internal static void InitNode(NodeBase node, BehaviorTree behaviorTree)
        {
            if(node.nodeState == NodeState.Raw)
            {
                node.nodeState = NodeState.Ready;
                node.behaviorTree = behaviorTree;
                node.Init();
            }
        }

        internal static BTState NodeTick(NodeBase node, float deltaTime)
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
            node.behaviorTree.logger.Debug(node.ToString() + " : " + ret);
            if (ret == BTState.Running)
            {
                return ret;
            }

            // last time
            node.OnExit();
            node.nodeState = NodeState.Visited;
            return ret;
        }

        internal static void ResetNode(NodeBase node)
        {
            if(node.nodeState == NodeState.Raw)
            {
                node.behaviorTree.logger.Warning("Should Init a node before Reset");
                node.Init();
            }

            if (node.nodeState == NodeState.Visiting)
            {
                node.behaviorTree.logger.Warning("Reset a running node may lead to unexpected result");
            }

            node.nodeState = NodeState.Ready;
            node.Reset();
        }

        internal virtual void OnTreeCleanUp()
        {
            Cleanup();
        }

        protected internal static void ForceExit(NodeBase node)
        {
            if(node.nodeState == NodeState.Visiting)
            {
                node.OnExit();
                node.nodeState = NodeState.Visited;
            }
        }

        // invoked when tree init (only once in tree life cycle)
        protected virtual void Init()
        {
        }

        protected virtual void Reset()
        {
        }

        protected virtual void OnEnter()
        {
        }

        protected virtual void OnExit()
        {
        }

        // invoked every tick
        protected abstract BTState Update(float deltaTime);

        // invoked when tree cleanup (only once in tree life cycle)
        protected abstract void Cleanup();

        protected internal virtual bool Validate(out string error)
        {
            error = null;
            return true;
        }
    }

}
