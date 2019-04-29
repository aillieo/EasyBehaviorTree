using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EasyBehaviorTree
{
    public interface INodeInfoFormatter
    {
        string FormatNodeInfo(NodeInfo nodeInfo);

        void FormatNodeInfo(NodeInfo nodeInfo, StringBuilder stringBuilder);
    }

}