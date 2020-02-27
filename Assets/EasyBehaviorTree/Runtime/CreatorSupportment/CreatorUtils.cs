using System;

namespace AillieoUtils.EasyBehaviorTree
{
#if UNITY_EDITOR
    public static class CreatorUtils
#else
    internal static class CreatorUtils
#endif
    {
        public static BehaviorTree NewBehaviorTree(NodeBase root)
        {
            return new BehaviorTree(root);
        }

        public static void AddChild(NodeParent parent, NodeBase child)
        {
            if (parent == null || child == null)
            {
                return;
            }
            parent.AddChild(child);
        }

        public static bool RemoveChild(NodeParent parent, NodeBase child)
        {
            if (parent == null || child == null)
            {
                return false;
            }
            return parent.Children.Remove(child);
        }

        public static void RemoveAllChildren(NodeParent parent)
        {
            if (parent == null)
            {
                return;
            }
            parent.Children.Clear();
        }

        public static void VisitChildren(NodeParent parent, Action<int,NodeBase> visitFunc)
        {
            if (parent != null)
            {
                int i = 0;
                foreach (var child in parent.Children)
                {
                    visitFunc(i++, child);
                }
            }
        }

        public static NodeBase GetRoot(BehaviorTree behaviorTree)
        {
            return behaviorTree.root;
        }
    }
}
