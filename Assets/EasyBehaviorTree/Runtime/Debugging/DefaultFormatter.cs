using System.Text;

namespace AillieoUtils.EasyBehaviorTree
{
    public class DefaultFormatter : INodeInfoFormatter
    {
        public string FormatNodeInfo(NodeInfo nodeInfo)
        {
            StringBuilder stringBuilder = new StringBuilder();
            FormatNodeInfo(nodeInfo, stringBuilder);
            return stringBuilder.ToString();
        }

        public void FormatNodeInfo(NodeInfo nodeInfo, StringBuilder stringBuilder)
        {
            stringBuilder.Append(new string('-', nodeInfo.level));
            stringBuilder.AppendFormat("{0}({1})", nodeInfo.name, nodeInfo.type.ToString());
            stringBuilder.Append(" ");
            stringBuilder.Append('{');
            for (int i = 0, len = nodeInfo.paramInfo.Length; i < len; i += 2)
            {
                if(i != 0)
                {
                    stringBuilder.Append(',');
                }
                stringBuilder.AppendFormat("{0}={1}",nodeInfo.paramInfo[i].name,nodeInfo.paramInfo[i].serializedValue);
            }
            stringBuilder.Append('}');
        }
    }
}
