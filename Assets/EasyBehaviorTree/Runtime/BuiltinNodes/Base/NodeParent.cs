using System.Collections.Generic;
using System;
using System.Text;

namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public abstract class NodeParent : NodeBase
    {
        private List<NodeBase> mChildren = new List<NodeBase>();

        public IList<NodeBase> Children => mChildren.AsReadOnly();

        internal void AddChild(NodeBase node)
        {
            mChildren.Add(node);
        }

        public override bool Validate(out string error)
        {
            error = null;
            if (Children.Count == 0)
            {
                error = "Have no children";
                return false;
            }
            return true;
        }

        public override void Init(BehaviorTreeVisitor behaviorTreeVisitor)
        {
            base.Init(behaviorTreeVisitor);

            foreach (var n in mChildren)
            {
                InitNode(n, behaviorTreeVisitor);
            }

            BTState[] states = behaviorTreeVisitor.GetChildrenState(this);
            for (int i = 0; i < states.Length; ++ i)
            {
                states[i] = BTState.Running;
            }
        }

        public override void Reset(BehaviorTreeVisitor behaviorTreeVisitor)
        {
            base.Reset(behaviorTreeVisitor);

            foreach (var n in mChildren)
            {
                ResetNode(n, behaviorTreeVisitor);
            }

            BTState[] states = behaviorTreeVisitor.GetChildrenState(this);
            for (int i = 0; i < states.Length; ++i)
            {
                states[i] = BTState.Running;
            }
        }

        protected BTState[] GetChildrenState(BehaviorTreeVisitor behaviorTreeVisitor)
        {
            return behaviorTreeVisitor.GetChildrenState(this);
        }
        
        public override void DumpNode(StringBuilder stringBuilder, INodeInfoFormatter formatter, int level = 0)
        {
            base.DumpNode(stringBuilder, formatter, level);

            foreach (var node in mChildren)
            {
                node.DumpNode(stringBuilder, formatter, level + 1);
            }
        }

        public override void OnTreeCleanUp(BehaviorTreeVisitor behaviorTreeVisitor)
        {
            base.OnTreeCleanUp(behaviorTreeVisitor);

            foreach (var node in mChildren)
            {
                node.OnTreeCleanUp(behaviorTreeVisitor);
            }
        }

    }
}
