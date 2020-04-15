using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace AillieoUtils.EasyBehaviorTree
{
    public class CSBuilderAssetProcessor : IBTAssetProcessor<string>
    {
        private static CSBuilderAssetProcessor instance;

        public static string SaveBehaviorTree(BehaviorTree behaviorTree)
        {
            if (instance == null)
            {
                instance = new CSBuilderAssetProcessor();
            }
            return instance.Save(behaviorTree);
        }

        public BehaviorTree Load(string asset)
        {
            throw new NotImplementedException();
        }

        public string Save(BehaviorTree behaviorTree)
        {
            StringBuilder sb = new StringBuilder();
            NodeBase root = CreatorUtils.GetRoot(behaviorTree);
            Append(sb, root);
            sb.Append(new string(' ', 4));
            string className = root.name;
            if(string.IsNullOrWhiteSpace(className))
            {
                className = "DefaultBehaviorTreeBuilder";
            }
            return string.Format(BUILDER_TEMPLATE, className, sb.ToString());
        }

        private void Append(StringBuilder stringBuilder, NodeBase node, int depth = 1)
        {
            stringBuilder.Append(new string(' ', 4 * depth + 8));
            string nodeType = node.GetType().FullName;
            stringBuilder.AppendLine($".Add(new {nodeType}())");
            CreatorUtils.VisitChildren(node as NodeParent, (index, child) => {
                Append(stringBuilder, child, depth + 1);
                if (depth > 1)
                {
                    stringBuilder.Append(new string(' ', 4 * (depth + 1) + 8));
                    stringBuilder.AppendLine(".EndCurrent()");
                }
            });
        }

        private static readonly string BUILDER_TEMPLATE =
@"using AillieoUtils.EasyBehaviorTree;
public class {0} {{
    public static BehaviorTree Build(out string err, out NodeBase node)
    {{
        return new BehaviorTreeBuilder()
{1}            .Build(out err, out node);
    }}
}}
";

    }
}
