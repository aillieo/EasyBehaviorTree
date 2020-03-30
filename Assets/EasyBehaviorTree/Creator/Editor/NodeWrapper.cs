using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using AillieoUtils.EasyGraph;

namespace AillieoUtils.EasyBehaviorTree.Creator
{
    public class NodeWrapper : INodeDataWrapper
    {
        public string name;
        public string nodeType;
        public ParamInfo[] param;

        public NodeWrapper(NodeBase node = null)
        {
            if (node == null)
            {
                return;
            }

            this.name = node.name;
            this.nodeType = node.GetType().FullName;
            CreatorUtils.ExtractParamInfo(node);
            this.param = CreatorUtils.ExtractParamInfo(node);
        }

        public Vector2 Size => new Vector2(150, 80);

        public void OnDetailGUI(Rect rect)
        {
            GUI.Box(rect, GUIContent.none);
            GUILayout.Label(nodeType);

            if(param != null && param.Length > 0)
            {
                foreach (var p in param)
                {
                    GUILayout.Label(p.name);
                    ParamValueDrawer.Draw(p);
                }
            }
        }

        public void OnGUI(Rect rect)
        {
            GUI.Box(rect, GUIContent.none, new GUIStyle("window"));
            EditorGUI.LabelField(rect, new GUIContent(this.nodeType));
        }


        public NodeBase ToNodeBase(bool includeChildren = false)
        {
            Type tNode = ReflectionUtils.GetType(this.nodeType);

            if (tNode == null)
            {
                Debug.LogError(this.nodeType);
                return null;
            }

            NodeBase node = Activator.CreateInstance(tNode) as NodeBase;

            if (node == null)
            {
                return null;
            }
            node.name = this.name;
            CreatorUtils.ApplyParamInfo(node, this.param);

            if (!includeChildren)
            {
                return node;
            }

            return node;
        }

    }
}
