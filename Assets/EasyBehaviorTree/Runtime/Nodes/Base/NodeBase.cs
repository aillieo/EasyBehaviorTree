using System;
using System.Collections.Generic;
using System.Text;

namespace AillieoUtils.EasyBehaviorTree
{

    [Serializable]
    public abstract class NodeBase
    {

        protected BehaviorTree behaviorTree;

        public string name;

        [NonSerialized]
        public ParamInfo[] paramInfo;

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

        internal int ExtractParamInfo()
        {
            if (paramInfo == null)
            {
                List<ParamInfo> paramInfo = new List<ParamInfo>();

                var fields = ReflectionUtils.GetNodeParamFields(this.GetType());

                foreach (var field in fields)
                {
                    paramInfo.Add(new ParamInfo
                    {
                        name = field.Name,
                        type = field.FieldType,
                        serializedValue = ParamInfoProcessor.Save(field.FieldType,field.GetValue(this))
                    });
                }
                this.paramInfo = paramInfo.ToArray();
            }
            return paramInfo.Length;
        }

        private NodeInfo ExtractNodeInfo(int level)
        {
            ExtractParamInfo();
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

            node.nodeState = NodeState.Ready;
            node.Reset();
        }

        public virtual void OnTreeCleanUp()
        {
            Cleanup();
        }

        public virtual void Init()
        {
        }

        public virtual void Reset()
        {
        }

        public virtual void OnEnter()
        {
        }

        public virtual void OnExit()
        {
        }

        public abstract BTState Update(float deltaTime);
        public abstract void Cleanup();

        public virtual bool Validate(out string error)
        {
            error = null;
            return true;
        }
    }

}
