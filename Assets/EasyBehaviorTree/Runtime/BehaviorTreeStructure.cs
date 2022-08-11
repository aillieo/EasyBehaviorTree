using System.Text;
using System;
using Random = System.Random;

namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public sealed class BehaviorTreeStructure
    {
        internal NodeBase root { get; private set; }

        public bool Validate(out string error, out NodeBase errorNode)
        {
            return ValidateNodeAndChildren(root, out error, out errorNode);
        }

        private static bool ValidateNodeAndChildren(NodeBase node, out string error, out NodeBase errorNode)
        {
            errorNode = node;
            if(!node.Validate(out error))
            {
                return false;
            }

            NodeParent nodeParent = node as NodeParent;
            if(nodeParent != null)
            {
                foreach (var child in nodeParent.Children)
                {
                    if (!ValidateNodeAndChildren(child, out error, out errorNode))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        internal BehaviorTreeStructure(NodeBase root)
        {
            this.root = root;
        }

        public string DumpTree(INodeInfoFormatter formatter = null)
        {
            StringBuilder sb = new StringBuilder();
            if (formatter == null)
            {
                formatter = new DefaultFormatter();
            }
            root.DumpNode(sb, formatter, 0);
            return sb.ToString();
        }
    }
}
