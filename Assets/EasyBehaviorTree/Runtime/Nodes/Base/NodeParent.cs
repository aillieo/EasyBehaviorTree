using System.Collections.Generic;
using System;
using System.Text;

namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public abstract class NodeParent : NodeBase
    {
        private List<NodeBase> mChildren = new List<NodeBase>();

        internal IList<NodeBase> Children => mChildren;

        internal void AddChild(NodeBase node)
        {
            mChildren.Add(node);
        }

        protected internal override bool Validate(out string error)
        {
            error = null;
            if (Children.Count == 0)
            {
                error = "Have no children";
                return false;
            }
            return true;
        }

        protected override void Init()
        {
            base.Init();

            foreach (var n in mChildren)
            {
                InitNode(n, this.behaviorTree);
            }
        }


        protected override void Reset()
        {
            base.Reset();

            foreach (var n in mChildren)
            {
                ResetNode(n);
            }
        }

        protected override void OnExit()
        {
            foreach (var n in mChildren)
            {
                if(n.nodeState == NodeState.Visiting)
                {
                    ForceExit(n);
                }
            }

            base.OnExit();
        }

        public override void DumpNode(StringBuilder stringBuilder, INodeInfoFormatter formatter, int level = 0)
        {
            base.DumpNode(stringBuilder, formatter, level);

            foreach (var node in mChildren)
            {
                node.DumpNode(stringBuilder, formatter, level + 1);
            }
        }

        internal override void OnTreeCleanUp()
        {
            base.OnTreeCleanUp();

            foreach (var node in mChildren)
            {
                node.OnTreeCleanUp();
            }
        }

    }
}
