using System.Collections;
using System.Collections.Generic;
using System.Text;
#if UNITY_EDITOR
using UnityEngine;
#endif

namespace EasyBehaviorTree
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
            for (int i = 0, len = nodeInfo.briefInfo.Length; i < len; i += 2)
            {
                if(i != 0)
                {
                    stringBuilder.Append(',');
                }
                stringBuilder.AppendFormat("{0}={1}",nodeInfo.briefInfo[i],nodeInfo.briefInfo[i+1]);
            }
            stringBuilder.Append('}');
        }
    }
}