using System;

namespace AillieoUtils.EasyBehaviorTree
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
