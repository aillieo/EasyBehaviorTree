using System.Collections.Generic;
using System;

namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public class NodeParallel : NodeComposite
    {

        public enum ParallelStrategy
        {
            FailCertain,
            PassCertain,
        }

        [NodeParam]
        public ParallelStrategy strategy;

        [NodeParam]
        public int targetCount;

        public override void Reset(BehaviorTreeVisitor behaviorTreeVisitor)
        {
            base.Reset(behaviorTreeVisitor);
        }

        public override void Cleanup(BehaviorTreeVisitor behaviorTreeVisitor)
        {
        }

        public override bool Validate(out string error)
        {
            bool ret = base.Validate(out error);
            if(ret)
            {
                if (targetCount > Children.Count)
                {
                    error = "Not enough child nodes";
                }
            }
            return error == null;
        }

        public override BTState Update(BehaviorTreeVisitor behaviorTreeVisitor, float deltaTime)
        {
            BTState[] states = behaviorTreeVisitor.GetChildrenState(this);
            int nodeCount = Children.Count;
            bool changed = false;
            for(int i = 0; i < nodeCount; ++i)
            {
                if (states[i] != BTState.Running)
                {
                    continue;
                }

                var ret = NodeTick(Children[i], behaviorTreeVisitor, deltaTime);
                if (ret != BTState.Running)
                {
                    states[i] = ret;
                    changed = true;
                }
            }

            if (changed)
            {
                return CheckTarget(states);
            }

            return BTState.Running;
        }

        private BTState CheckTarget(BTState[] childrenState)
        {
            int pass = 0, fail = 0, rest = 0;
            foreach(var state in childrenState)
            {
                if(state == BTState.Success)
                {
                    pass++;
                }
                else if (state == BTState.Failure)
                {
                    fail++;
                }
                else
                {
                    rest++;
                }
            }

            switch(strategy)
            {
                case ParallelStrategy.PassCertain:
                    if(pass >= targetCount)
                    {
                        return BTState.Success;
                    }
                    else if(rest == 0)
                    {
                        return BTState.Failure;
                    }
                    break;
                case ParallelStrategy.FailCertain:
                    if (fail >= targetCount)
                    {
                        return BTState.Failure;
                    }
                    else if (rest == 0)
                    {
                        return BTState.Success;
                    }
                    break;
                default:
                    break;
            }

            return BTState.Running;
        }
    }
}
