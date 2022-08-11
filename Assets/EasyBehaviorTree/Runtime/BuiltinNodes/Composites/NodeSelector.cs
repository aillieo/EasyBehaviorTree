using System;

namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public class NodeSelector : NodeComposite
    {
        public override void Cleanup(BehaviorTreeVisitor behaviorTreeVisitor)
        {

        }

        public override BTState Update(BehaviorTreeVisitor behaviorTreeVisitor, float deltaTime)
        {
            BTState[] states = behaviorTreeVisitor.GetChildrenState(this);
            int nodeCount = Children.Count;
            int curIndex = 0;
            while (curIndex < nodeCount)
            {
                if (states[curIndex] != BTState.Running)
                {
                    curIndex++;
                    continue;
                }

                var node = Children[curIndex];
                var ret = NodeTick(node, behaviorTreeVisitor, deltaTime);
                states[curIndex] = ret;
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
