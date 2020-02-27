using System;

namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public class NodeSelector : NodeComposite
    {

        private int curIndex;

        public override void Reset()
        {
            base.Reset();
            curIndex = 0;
        }

        public override void Cleanup()
        {

        }

        public override BTState Update(float deltaTime)
        {
            int nodeCount = Children.Count;

            while (curIndex < nodeCount)
            {
                var node = Children[curIndex];
                var ret = NodeTick(node, deltaTime);
                switch (ret)
                {
                case BTState.Success:
                    return BTState.Success;
                case BTState.Running:
                    return BTState.Running;
                case BTState.Failure:
                    ++curIndex;
                    continue;
                }

            }

            return BTState.Failure;
        }
    }
}
