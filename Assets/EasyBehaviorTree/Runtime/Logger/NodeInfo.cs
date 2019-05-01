using System.Collections;
using System.Collections.Generic;
using System;

namespace EasyBehaviorTree
{
    public struct NodeInfo
    {
        public string name;
        public Type type;
        public string[] paramInfo;

        public NodeState nodeState;

        public int level;
    }
}
