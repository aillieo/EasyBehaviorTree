using System;

namespace AillieoUtils.EasyBehaviorTree
{
    public struct NodeInfo
    {
        public string name;
        public Type type;
        public ParamInfo[] paramInfo;

        public NodeState nodeState;

        public int level;
    }
}
