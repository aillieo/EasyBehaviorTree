using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EasyBehaviorTree
{
    [Serializable]
    public class NodeSequence : NodeComposite
    {

        public override void Destroy()
        {

        }


        public override BTState Update()
        {
            BTState ret = BTState.Success;
            foreach (var node in Children)
            {
                if (BTState.Failure == TickNode(node))
                {
                    ret = BTState.Failure;
                    break;
                }
            }

            return ret;

        }
    }
}
