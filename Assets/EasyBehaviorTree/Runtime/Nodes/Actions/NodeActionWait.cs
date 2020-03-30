using System;

namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public class NodeActionWait : NodeAction
    {
        [NodeParam]
        public readonly float time;

        private float timer;

        protected override void Cleanup()
        {

        }

        protected internal override bool Validate(out string error)
        {
            error = null;
            if (time < 0)
            {
                error = "Invalid time";
            }
            return error == null;
        }

        protected override void Reset()
        {
            base.Reset();
            timer = 0f;
        }

        protected override BTState ExecuteTask(float deltaTime)
        {
            timer += deltaTime;
            if (timer > time)
            {
                timer = 0;
                return BTState.Success;
            }

            return BTState.Running;
        }
    }

}
