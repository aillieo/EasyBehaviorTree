using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EasyBehaviorTree
{
    [Serializable]
    public class NodeSequence : NodeComposite
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
            int nodeCount = Children.Count;
            if(curIndex >= nodeCount)
            {
                curIndex = 0;
            }

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
                        curIndex = 0;
                        return BTState.Failure;
                }

            }

            return BTState.Success;
        }
    }
}
