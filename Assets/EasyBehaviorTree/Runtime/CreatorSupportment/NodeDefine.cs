using UnityEngine;
using System;

namespace AillieoUtils.EasyBehaviorTree.Creator
{
    public class NodeDefine : NodeDefineBase
    {
        [SerializeField][HideInInspector]
        private string nodeFullName;

        [SerializeField][HideInInspector]
        private string displayName;
        [SerializeField][HideInInspector]
        private string nodeDescription;
        [SerializeField][HideInInspector]
        private string nodeParams;

        public override Type GetNodeType()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                Type t = assembly.GetType(nodeFullName);
                if (t != null)
                {
                    return t;
                }
            }

            return null;
        }

        public override NodeBase CreateNode()
        {
            Type t = GetNodeType();
            if (t != null)
            {
                NodeBase node = default;
                if (!string.IsNullOrWhiteSpace(nodeParams))
                {
                    node = JsonUtility.FromJson(nodeParams, t) as NodeBase;
                }

                if (node == null)
                {
                    node = Activator.CreateInstance(t) as NodeBase;
                }

                if (node != null)
                {
                    node.nodeName = displayName;
                }

                return node;
            }

            return null;
        }
    }
}
