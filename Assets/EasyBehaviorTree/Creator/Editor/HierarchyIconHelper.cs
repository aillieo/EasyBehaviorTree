using System;
using UnityEngine;
using UnityEditor;

namespace EasyBehaviorTree.Creator
{

    [InitializeOnLoad]
    public class HierarchyIconHelper
    {
        static HierarchyIconHelper()
        {
            hiearchyItemCallback = new EditorApplication.HierarchyWindowItemCallback(HierarchyIconHelper.DrawHierarchyIcon);
            EditorApplication.hierarchyWindowItemOnGUI = (EditorApplication.HierarchyWindowItemCallback)Delegate.Combine(
                EditorApplication.hierarchyWindowItemOnGUI,
                hiearchyItemCallback);
        }

        private static readonly EditorApplication.HierarchyWindowItemCallback hiearchyItemCallback;

        private static Texture2D hierarchyNodeIcon;
        private static Texture2D HierarchyNodeIcon
        {
            get
            {
                if (hierarchyNodeIcon == null)
                {
                    hierarchyNodeIcon = Base64ToTexture2D(TextureAssets.HierarchyNodeIcon);
                }
                return hierarchyNodeIcon;
            }
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

        private static Texture2D Base64ToTexture2D(string base64)
        {
            Texture2D tex = new Texture2D(1, 1);
            tex.LoadImage(Convert.FromBase64String(base64));
            tex.Apply();
            return tex;
        }
    }
}
