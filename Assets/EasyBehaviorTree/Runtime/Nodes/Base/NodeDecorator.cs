using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

namespace EasyBehaviorTree
{
    [Serializable]
    public abstract class NodeDecorator : NodeParent
    {

#if UNITY_EDITOR

        public override bool Validate(out string error)
        {
            error = null;
            if (base.Validate(out error))
            {
                if (Children.Count != 1)
                {
                    error = "Decorator more than one child node";
                }

            }
            return error == null;
        }
#endif

        protected NodeBase Child
        {
            get
            {
                return Children[0];
            }
        }
    }
}