using System.Collections;
using System.Collections.Generic;
using System;

namespace EasyBehaviorTree
{
    [Serializable]
    public class NodeParallel : NodeComposite
    {

        public enum ParallelStrategy
        {
            FailCertain,
            PassCertain,
        }

        public ParallelStrategy strategy { get; set; }
        public int targetCount { get; set; }


        private int failCount;
        private int passCount;

        public override void Init()
        {
            base.Init();
            if (targetCount > Children.Count)
            {
                behaviorTree.logger.Error("Not enough child nodes");
            }

            failCount = 0;
            passCount = 0;
        }

        public override void Destroy()
        {

        }

        public override BTState Update(float deltaTime)
        {
            if (lastState != null)
            {
                return lastState.Value;
            }

            int nodeCount = Children.Count;


            /*
             * 
            while (curIndex < nodeCount)
            {
                var node = Children[curIndex];
                var ret = TickNode(node, deltaTime);
                switch (ret)
                {
                    case BTState.Success:
                        ++curIndex;
                        continue;
                    case BTState.Running:
                        return BTState.Running;
                    case BTState.Failure:
                        lastState = BTState.Failure;
                        return BTState.Failure;
                }

            }

            */
            lastState = BTState.Success;
            return BTState.Success;
        }
    }
}
