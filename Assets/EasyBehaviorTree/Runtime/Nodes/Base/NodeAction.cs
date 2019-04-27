using System.Collections;
using System.Collections.Generic;
using System;

namespace EasyBehaviorTree
{

    [Serializable]
    public abstract class NodeAction : NodeBase
    {

        protected abstract BTState ExecuteTask(float deltaTime);


        public override BTState Update(float deltaTime)
        {
            return ExecuteTask(deltaTime);
        }
    }
}
