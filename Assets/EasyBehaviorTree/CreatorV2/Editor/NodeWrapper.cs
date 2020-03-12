using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using AillieoUtils.EasyGraph;

namespace AillieoUtils.EasyBehaviorTree.Creator
{
    public class NodeWrapper : INodeDataWrapper
    {
        public NodeBase node { get; private set; }

        public NodeWrapper(NodeBase node)
        {
            this.node = node;
        }

        public Vector2 Size => new Vector2(150, 80);

        public void OnDetailGUI(Rect rect)
        {
            GUI.Box(rect, GUIContent.none);
            GUILayout.Label(node.GetType().Name);
            var fields = ReflectionUtils.GetNodeParamFields(node.GetType());
            foreach (var f in fields)
            {
                GUILayout.Label(f.Name);
                ParamValueDrawer.Draw(f,node);
            }
        }

        public void OnGUI(Rect rect)
        {
            GUI.Box(rect, GUIContent.none, new GUIStyle("window"));
            EditorGUI.LabelField(rect, new GUIContent(this.node.GetType().Name));
        }
    }
}
