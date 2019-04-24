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

        protected List<NodeBase> mChildren = new List<NodeBase>();

        public IList<NodeBase> Children => mChildren.AsReadOnly();

#if UNITY_EDITOR

        public void AddChild(NodeBase node)
        {
            mChildren.Add(node);
        }
#endif

        public override void Init()
        {
            base.Init();

            lastState = null;

            foreach(var n in mChildren)
            {
                Init(n,this.behaviorTree);
            }
        }


        public override void DumpNode(StringBuilder stringBuilder, int level = 0)
        {
            if (stringBuilder == null)
            {
                return;
            }

            stringBuilder.Append(new string('-', level));
            stringBuilder.Append(ToString());
            stringBuilder.AppendLine();

            if (null == mChildren)
            {
                return;
            }

            foreach (var node in mChildren)
            {
                node.DumpNode(stringBuilder, level + 1);
            }
        }



    }
}
