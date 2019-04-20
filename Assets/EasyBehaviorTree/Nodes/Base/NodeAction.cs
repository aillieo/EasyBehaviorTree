using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

namespace EasyBehaviorTree
{

    [Serializable]
    public abstract class NodeAction : NodeDecoratee
    {

        protected abstract BTState ExecuteTask();


        public override BTState Update()
        {
            return ExecuteTask();
        }
    }
}
