using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EasyBehaviorTree
{
    [Serializable]
    public class NodeSelector : NodeComposite
    {

        private int curIndex;

        public override void Init()
        {
            base.Init();
            curIndex = 0;
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
 
            while (curIndex < nodeCount)
            {
                var node = Children[curIndex];
                var ret = TickNode(node, deltaTime);
                switch (ret)
                {
                case BTState.Success:
                    lastState = BTState.Success;
                    return BTState.Success;
                case BTState.Running:
                    return BTState.Running;
                case BTState.Failure:
                    ++curIndex;
                    continue;
                }

            }

            lastState = BTState.Failure;
            return BTState.Failure;
        }
    }
}
