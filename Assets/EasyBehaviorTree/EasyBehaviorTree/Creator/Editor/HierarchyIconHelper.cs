using System;
using UnityEngine;
using UnityEditor;

namespace EasyBehaviorTree
{

    [InitializeOnLoad]
    public class HierarchyIconHelper
    {
        private static readonly EditorApplication.HierarchyWindowItemCallback hiearchyItemCallback;

        private static Texture2D hierarchyNodeIcon;
        private static Texture2D HierarchyNodeIcon
        {
            get
            {
                if (hierarchyNodeIcon == null)
                {
                    hierarchyNodeIcon = AillieoUtils.ImageUtils.Base64ToTexture2D(TextureAssets.HierarchyNodeIcon);
                }
                return hierarchyNodeIcon;
            }
        }

        static HierarchyIconHelper()
        {
            hiearchyItemCallback = new EditorApplication.HierarchyWindowItemCallback(HierarchyIconHelper.DrawHierarchyIcon);
            EditorApplication.hierarchyWindowItemOnGUI = (EditorApplication.HierarchyWindowItemCallback)Delegate.Combine(
                EditorApplication.hierarchyWindowItemOnGUI,
                hiearchyItemCallback);
        }

        private static void DrawHierarchyIcon(int instanceID, Rect selectionRect)
        {
            GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (gameObject != null && gameObject.GetComponent<NodeDefine>() != null)
            {
                Rect rect = new Rect(selectionRect.x + selectionRect.width - 16f, selectionRect.y, 16f, 16f);
                GUI.DrawTexture(rect, HierarchyNodeIcon);
            }
        }
    }
}
