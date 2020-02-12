using System.Collections.Generic;

namespace AillieoUtils.EasyBehaviorTree
{
    public interface IParent
    {
        IList<NodeBase> Children { get; }

#if UNITY_EDITOR

        void AddChild(NodeBase node);

#endif
    }
}
