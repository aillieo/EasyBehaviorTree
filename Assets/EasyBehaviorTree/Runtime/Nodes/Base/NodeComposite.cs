using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

namespace EasyBehaviorTree
{
    [Serializable]
    public abstract class NodeComposite : NodeDecoratee, IComposite
    {

        protected BTState? lastState = null;

        private List<NodeBase> mChildren = new List<NodeBase>();

        public IList<NodeBase> Children => mChildren.AsReadOnly();

#if UNITY_EDITOR

        public void AddChild(NodeBase node)
        {
            mChildren.Add(node);
        }

        public override bool Validate(out string error)
        {
            error = null;
            if (Children.Count == 0)
            {
                error = "Not enough child nodes";
                return false;
            }
            return true;
        }
#endif

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

            lastState = null;

            foreach (var n in mChildren)
            {
                ResetNode(n);
            }
        }


        public override void DumpNode(StringBuilder stringBuilder, bool withBriefInfo, int level = 0)
        {
            base.DumpNode(stringBuilder, withBriefInfo, level);

            if (null == mChildren)
            {
                return;
            }

            foreach (var node in mChildren)
            {
                node.DumpNode(stringBuilder, withBriefInfo, level + 1);
            }
        }



    }
}
