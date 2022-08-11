using System.Text;

namespace AillieoUtils.EasyBehaviorTree
{
    public interface INodeInfoFormatter
    {
        string FormatNodeInfo(NodeInfo nodeInfo);

        void FormatNodeInfo(NodeInfo nodeInfo, StringBuilder stringBuilder);
    }

}