using System;
using System.Collections.Generic;
using System.Text;

namespace AillieoUtils.EasyBehaviorTree
{

    [Serializable]
    public abstract class NodeBase
    {

        protected BehaviorTreeStructure behaviorTreeStructure;
        
        private string name;
        
        public string nodeName
        {
            get => name;
#if UNITY_EDITOR
            set => name = value;
#endif
        }

        [NonSerialized]
        public ParamInfo[] paramInfo;

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

        public override string ToString()
        {
            return $"{name}({GetType().Name})";
        }

        internal static void InitNode(NodeBase node, BehaviorTreeVisitor behaviorTreeVisitor)
        {
            if (node.nodeState == NodeState.Raw)
            {
                node.nodeState = NodeState.Ready;
                node.Init(behaviorTreeVisitor);
            }
        }

        internal static BTState NodeTick(NodeBase node, BehaviorTreeVisitor behaviorTreeVisitor, float deltaTime)
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
                // node.OnEnter();
                node.nodeState = NodeState.Visiting;
            }

            // regular tick
            BTState ret = node.Update(behaviorTreeVisitor, deltaTime);
            
            behaviorTreeVisitor.logger.Debug(node.ToString() + " : " + ret);
            if (ret == BTState.Running)
            {
                return ret;
            }

            // last time
            // node.OnExit();
            node.nodeState = NodeState.Visited;
            NodeBase.ResetNode(node, behaviorTreeVisitor);
            return ret;
        }

        internal static void ResetNode(NodeBase node, BehaviorTreeVisitor behaviorTreeVisitor)
        {
            if(node.nodeState == NodeState.Raw)
            {
                behaviorTreeVisitor.logger.Warning("Should Init a node before Reset");
                node.Init(behaviorTreeVisitor);
            }

            node.nodeState = NodeState.Ready;
            node.Reset(behaviorTreeVisitor);
        }

        public virtual void OnTreeCleanUp(BehaviorTreeVisitor behaviorTreeVisitor)
        {
            Cleanup(behaviorTreeVisitor);
        }

        public virtual void Init(BehaviorTreeVisitor behaviorTreeVisitor)
        {
        }

        public virtual void Reset(BehaviorTreeVisitor behaviorTreeVisitor)
        {
        }

        // public virtual void OnEnter()
        // {
        // }

        // public virtual void OnExit()
        // {
        // }

        public abstract BTState Update(BehaviorTreeVisitor behaviorTreeVisitor, float deltaTime);
        public abstract void Cleanup(BehaviorTreeVisitor behaviorTreeVisitor);

        public virtual bool Validate(out string error)
        {
            error = null;
            return true;
        }
    }

}
