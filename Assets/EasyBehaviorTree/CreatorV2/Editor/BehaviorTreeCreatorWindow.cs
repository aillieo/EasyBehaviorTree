using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AillieoUtils.EasyGraph;
using UnityEditor;
using AillieoUtils;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AillieoUtils.EasyBehaviorTree.Creator
{
    public class BehaviorTreeCreatorWindow : EditorWindow
    {
        [MenuItem("Window/BehaviorTreeCreatorWindow")]
        private static void OpenWindow()
        {
            EditorWindow.GetWindow<BehaviorTreeCreatorWindow>("Easy Graph Window");
        }

        private Graph<BehaviorTreeAssetWrapper,NodeWrapper> graph = new Graph<BehaviorTreeAssetWrapper,NodeWrapper>(new Vector2(1280f, 640f));

        private string filePath = "Assets/Sample/BT/BT_Hero.bt";

        private void OnGUI()
        {
            float singleLineHeight = EditorGUIUtility.singleLineHeight;
            Vector2 detailSize = new Vector2(160, 200);
            Rect viewRect = this.position;
            viewRect.position = Vector2.zero;
            viewRect.width -= (detailSize.x + 2);

            // main
            graph.OnGUI(viewRect);

            // right side
            Rect right = viewRect.SetWidth(detailSize.x);
            right.position = Vector2.zero;
            right = right.OffsetX(position.width - detailSize.x);
            GUILayout.BeginArea(right);

            // detail
            graph.OnGUIDetail(new Rect(Vector2.zero,Vector2.one * 200f));
            GUILayout.Space(200f);

            GUILayout.FlexibleSpace();

            // save&load

            GUILayout.BeginVertical("box");

            GUILayout.Label("Editor Asset:");

            GUILayout.Label("Asset path:");

            filePath = GUILayout.TextField(filePath);

            if (GUILayout.Button("Save"))
            {
                BehaviorTreeAssetWrapper asset = new BehaviorTreeAssetWrapper();
                graph.Save(asset);
                BehaviorTree behaviorTree = asset.behaviorTree;
                BytesAssetProcessor.SaveBehaviorTree(behaviorTree,Application.dataPath + "/../" + filePath);
            }

            if (GUILayout.Button("Load"))
            {
                BehaviorTree behaviorTree = BytesAssetProcessor.LoadBehaviorTree(Application.dataPath + "/../" + filePath);
                BehaviorTreeAssetWrapper asset = new BehaviorTreeAssetWrapper();
                asset.behaviorTree = behaviorTree;
                Graph<BehaviorTreeAssetWrapper,NodeWrapper> newGraph = null;
                if (Graph<BehaviorTreeAssetWrapper,NodeWrapper>.Load(asset, out newGraph))
                {
                    graph = newGraph;
                }
            }

            GUILayout.EndVertical();
            GUILayout.BeginVertical("box");

            GUILayout.Label("Import:");

            if (GUILayout.Button("Import Bytes"))
            {

            }

            if (GUILayout.Button("Import Xml"))
            {

            }

            GUILayout.EndVertical();
            GUILayout.BeginVertical("box");

            GUILayout.Label("Export:");

            if (GUILayout.Button("Export Bytes"))
            {
                
            }

            if (GUILayout.Button("Export Xml"))
            {

            }

            if (GUILayout.Button("Export Builder.cs"))
            {

            }

            GUILayout.EndVertical();

            GUILayout.Space(4);

            GUILayout.EndArea();
        }

        private bool ExportBehaviorTree(BTAssetType assetType, BehaviorTree behaviorTree)
        {
            string filePath = EditorUtility.SaveFilePanel("存到哪里", ".", "bt", "bytes");
            if (File.Exists(filePath))
            {
                Debug.LogError("文件已存在 即将覆盖");
            }

            switch (assetType)
            {
                case BTAssetType.Bytes:
                    return BytesAssetProcessor.SaveBehaviorTree(behaviorTree, filePath);
                case BTAssetType.XML:
                    return XmlAssetProcessor.SaveBehaviorTree(behaviorTree, filePath);
                case BTAssetType.CSBuilder:
                    Debug.LogError("BTAssetType.CSBuilder");
                    break;
                default:
                    break;
            }
            return false;
        }

        private BehaviorTree ImportBehaviorTree(BTAssetType assetType)
        {
            string filePath = EditorUtility.OpenFilePanel("where to load?", ".", "bytes");
            if (!File.Exists(filePath))
            {
                Debug.LogError("文件不存在");
                return null;
            }

            switch (assetType)
            {
                case BTAssetType.Bytes:
                    return BytesAssetProcessor.LoadBehaviorTree(filePath);
                case BTAssetType.XML:
                    return XmlAssetProcessor.LoadBehaviorTree(filePath);
                default:
                    break;
            }

            return null;
        }

    }
}

