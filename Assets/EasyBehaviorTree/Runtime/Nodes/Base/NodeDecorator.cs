using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

namespace EasyBehaviorTree
{
    [Serializable]
    public abstract class NodeDecorator : NodeDecoratee, IDecorator
    {

        public NodeDecoratee Child
        {
            get;
#if UNITY_EDITOR
            set;
#endif
        }

#if UNITY_EDITOR

        public override bool Validate(out string error)
        {
            error = null;
            if (Child == null)
            {
                error = "Dont have a child";
                return false;
            }
            return true;
        }
#endif


        public override void Init()
        {
            base.Init();

            if (Child != null)
            {
                InitNode(Child, this.behaviorTree);
            }
        }


        public override void Reset()
        {
            base.Reset();

            if (Child != null)
            {
                ResetNode(Child);
            }
        }


        public override void DumpNode(StringBuilder stringBuilder, bool withBriefInfo, int level = 0)
        {
            base.DumpNode(stringBuilder, withBriefInfo, level);

            if (Child != null)
            {
                Child.DumpNode(stringBuilder, withBriefInfo, level + 1);
            }
        }
    }
}