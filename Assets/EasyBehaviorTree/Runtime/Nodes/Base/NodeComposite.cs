using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

namespace EasyBehaviorTree
{
    [Serializable]
    public abstract class NodeComposite : NodeParent
    {

        protected BTState? lastState = null;

        public override void Reset()
        {
            base.Reset();

            lastState = null;
        }

    }
}
