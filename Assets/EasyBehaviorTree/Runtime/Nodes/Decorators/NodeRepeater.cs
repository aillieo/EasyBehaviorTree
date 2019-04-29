using System.Collections;
using System.Collections.Generic;
using System;

namespace EasyBehaviorTree
{
    [Serializable]
    public class NodeRepeater : NodeDecorator
    {
        [NodeParam]
        public int repeatTimes { get; set; }

        [NodeParam]
        public bool ignoringFailure { get; set; }

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

            bool needReset = false;

            while (finishedTimes < repeatTimes)
            {
                if(needReset)
                {
                    needReset = false;
                    ResetNode(Child);
                }
                
                var ret = TickNode(Child, deltaTime);
                switch (ret)
                {
                    case BTState.Success:
                        ++finishedTimes;
                        needReset = true;
                        continue;
                    case BTState.Running:
                        return BTState.Running;
                    case BTState.Failure:
                        if (ignoringFailure)
                        {
                            ++finishedTimes;
                            needReset = true;
                            continue;
                        }
                        else
                        {
                            return BTState.Failure;
                        }
                }
            }
            return BTState.Success;
        }
    }
}
