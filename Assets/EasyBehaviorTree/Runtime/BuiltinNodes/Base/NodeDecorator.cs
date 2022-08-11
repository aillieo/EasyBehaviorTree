using System;

namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public abstract class NodeDecorator : NodeParent
    {
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

        protected NodeBase Child
        {
            get
            {
                return Children[0];
            }
        }
    }
}
