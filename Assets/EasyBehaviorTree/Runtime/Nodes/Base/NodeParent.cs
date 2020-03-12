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

        public override void Init()
        {
            base.Init();

            foreach (var n in mChildren)
            {
                InitNode(n, this.behaviorTree);
            }
        }


        public override void Reset()
        {
            base.Reset();

            foreach (var n in mChildren)
            {
                ResetNode(n);
            }
        }


        public override void DumpNode(StringBuilder stringBuilder, INodeInfoFormatter formatter, int level = 0)
        {
            base.DumpNode(stringBuilder, formatter, level);

            foreach (var node in mChildren)
            {
                node.DumpNode(stringBuilder, formatter, level + 1);
            }
        }

        public override void OnTreeCleanUp()
        {
            base.OnTreeCleanUp();

            foreach (var node in mChildren)
            {
                node.OnTreeCleanUp();
            }
        }

    }
}
