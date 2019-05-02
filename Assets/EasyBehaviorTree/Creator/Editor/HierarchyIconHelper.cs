using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace EasyBehaviorTree.Creator
{

    [InitializeOnLoad]
    public static class HierarchyIconHelper
    {
        static HierarchyIconHelper()
        {
            hiearchyItemCallback = new EditorApplication.HierarchyWindowItemCallback(HierarchyIconHelper.DrawHierarchyIcon);
            EditorApplication.hierarchyWindowItemOnGUI = (EditorApplication.HierarchyWindowItemCallback)Delegate.Combine(
                EditorApplication.hierarchyWindowItemOnGUI,
                hiearchyItemCallback);
        }

        private static readonly EditorApplication.HierarchyWindowItemCallback hiearchyItemCallback;

        private static Dictionary<string, Texture2D> cachedTexture = new Dictionary<string, Texture2D>();

        private static void DrawHierarchyIcon(int instanceID, Rect selectionRect)
        {
            GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (gameObject != null)
            {
                NodeDefine node = gameObject.GetComponent<NodeDefine>();
                if (node == null)
                {
                    return;
                }

                Type type = node.GetNodeType();
                if (type != null)
                {
                    // draw custom icon
                    var nodeIconAttributes = type.GetCustomAttributes(typeof(NodeIconAttribute), true);
                    if (nodeIconAttributes.Length > 0)
                    {
                        var attribute = nodeIconAttributes[0] as NodeIconAttribute;
                        string filePath = attribute.iconPath;
                        if (!cachedTexture.ContainsKey(filePath))
                        {
                            cachedTexture.Add(filePath, AssetDatabase.LoadAssetAtPath<Texture2D>(filePath));
                        }
                        Texture2D tex = cachedTexture[filePath];
                        if (tex != null)
                        {
                            DrawIcon(instanceID, selectionRect, tex);
                            return;
                        }
                    }

                    // draw preset icons
                    if(type.IsSubclassOf(typeof(NodeAction)))
                    {
                        DrawIcon(instanceID, selectionRect, TextureAssets.TextureDict[PresetNodeIcons.NodeAction]);
                        return;
                    }
                    if (type.IsSubclassOf(typeof(NodeCondition)))
                    {
                        DrawIcon(instanceID, selectionRect, TextureAssets.TextureDict[PresetNodeIcons.NodeCondition]);
                        return;
                    }
                    if (type.IsSubclassOf(typeof(NodeDecorator)))
                    {
                        DrawIcon(instanceID, selectionRect, TextureAssets.TextureDict[PresetNodeIcons.NodeDecorator]);
                        return;
                    }

                    if (type == typeof(NodeSequence))
                    {
                        DrawIcon(instanceID, selectionRect, TextureAssets.TextureDict[PresetNodeIcons.NodeSequence]);
                        return;
                    }
                    if (type == typeof(NodeSelector))
                    {
                        DrawIcon(instanceID, selectionRect, TextureAssets.TextureDict[PresetNodeIcons.NodeSelector]);
                        return;
                    }
                    if (type == typeof(NodeParallel))
                    {
                        DrawIcon(instanceID, selectionRect, TextureAssets.TextureDict[PresetNodeIcons.NodeParallel]);
                        return;
                    }

                }



                // default
                DrawIcon(instanceID, selectionRect, TextureAssets.TextureDict[PresetNodeIcons.NodeBase]);
            }
        }


        private static void DrawIcon(int instanceID, Rect selectionRect, Texture texture)
        {
            Rect rect = new Rect(selectionRect.x + selectionRect.width - 16f, selectionRect.y, 16f, 16f);
            GUI.DrawTexture(rect, texture);
        }

    }
}
