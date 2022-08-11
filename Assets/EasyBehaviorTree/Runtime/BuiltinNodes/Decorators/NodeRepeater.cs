using System;
using System.Globalization;

namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public class NodeRepeater : NodeDecorator
    {
        [NodeParam]
        public int repeatTimes;

        [NodeParam]
        public bool ignoringFailure;

        [NodeParam]
        public bool oncePerTick;

        public override void Reset(BehaviorTreeVisitor behaviorTreeVisitor)
        {
            base.Reset(behaviorTreeVisitor);
            string key = this.GetHashCode().ToString(CultureInfo.InvariantCulture);
            behaviorTreeVisitor.blackboard.Remove(key);
        }

        public override void Cleanup(BehaviorTreeVisitor behaviorTreeVisitor)
        {

        }

        public override bool Validate(out string error)
        {
            error = null;
            if(repeatTimes < 0)
            {
                error = "Invalid repeatTimes";
            }
            return error == null;
        }

        public override BTState Update(BehaviorTreeVisitor behaviorTreeVisitor, float deltaTime)
        {
            string key = this.GetHashCode().ToString(CultureInfo.InvariantCulture);
            int finishedTimes = (int)behaviorTreeVisitor.blackboard.SafeGet(key, 1);

            if (finishedTimes == 0)
            {
                ResetNode(Child, behaviorTreeVisitor);
            }

            if (finishedTimes >= repeatTimes)
            {
                return BTState.Success;
            }

            bool firstTimeInLoop = true;

            while (finishedTimes < repeatTimes)
            {
                if(firstTimeInLoop)
                {
                    firstTimeInLoop = false;
                }
                else
                {
                    UnityEngine.Assertions.Assert.AreNotEqual(Child.nodeState, NodeState.Visiting);
                    //ResetNode(Child, behaviorTreeVisitor);
                    if(oncePerTick)
                    {
                        return BTState.Running;
                    }
                }

                var ret = NodeTick(Child, behaviorTreeVisitor, deltaTime);
                switch (ret)
                {
                    case BTState.Success:
                        break;
                    case BTState.Running:
                        return BTState.Running;
                    case BTState.Failure:
                        if (!ignoringFailure)
                        {
                            return BTState.Failure;
                        }
                        break;
                }

                ++finishedTimes;
                behaviorTreeVisitor.blackboard[key] = finishedTimes;
            }

            return BTState.Success;
        }
    }
}
