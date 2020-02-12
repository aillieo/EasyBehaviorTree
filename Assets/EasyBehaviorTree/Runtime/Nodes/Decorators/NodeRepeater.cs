using System;

namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public class NodeRepeater : NodeDecorator
    {
        [NodeParam]
        private int repeatTimes;

        [NodeParam]
        private bool ignoringFailure;

        [NodeParam]
        private bool oncePerTick;

        private int finishedTimes;

        public override void Reset()
        {
            base.Reset();
            finishedTimes = 0;
        }

        public override void Cleanup()
        {

        }

#if UNITY_EDITOR
        public override bool Validate(out string error)
        {
            error = null;
            if(repeatTimes < 0)
            {
                error = "Invalid repeatTimes";
            }
            return error == null;
        }
#endif

        public override BTState Update(float deltaTime)
        {
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
                    ResetNode(Child);
                    if(oncePerTick)
                    {
                        return BTState.Running;
                    }
                }

                var ret = NodeTick(Child, deltaTime);
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
            }
            return BTState.Success;
        }
    }
}
