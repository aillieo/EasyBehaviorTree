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
            parent.AddChild(child);
        }

    }
}
