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

        private string filePath = "Assets/Sample/BT/BT_Hero.asset";

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
                BehaviorTreeAssetWrapper asset = BehaviorTreeAssetWrapper.CreateInstance<BehaviorTreeAssetWrapper>();
                graph.Save(asset);
                AssetDatabase.CreateAsset(asset, filePath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("Load"))
            {
                BehaviorTreeAssetWrapper asset = AssetDatabase.LoadAssetAtPath<BehaviorTreeAssetWrapper>(filePath);
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
                Graph<BehaviorTreeAssetWrapper, NodeWrapper> newGraph = ImportAndLoad(BTAssetType.Bytes);
                if(newGraph != null)
                {
                    graph = newGraph;
                }
            }

            if (GUILayout.Button("Import Xml"))
            {
                Graph<BehaviorTreeAssetWrapper, NodeWrapper> newGraph = ImportAndLoad(BTAssetType.XML);
                if (newGraph != null)
                {
                    graph = newGraph;
                }
            }

            GUILayout.EndVertical();
            GUILayout.BeginVertical("box");

            GUILayout.Label("Export:");

            if (GUILayout.Button("Export Bytes"))
            {
                ExportFromGraph(graph, BTAssetType.Bytes);
            }

            if (GUILayout.Button("Export Xml"))
            {
                ExportFromGraph(graph, BTAssetType.XML);
            }

            if (GUILayout.Button("Export Builder.cs"))
            {
                ExportFromGraph(graph, BTAssetType.CSBuilder);
            }

            GUILayout.EndVertical();

            GUILayout.Space(4);

            GUILayout.EndArea();
        }

        private string GetFileExt(BTAssetType assetType)
        {
            switch(assetType)
            {
                case BTAssetType.Bytes:
                    return "bytes";
                case BTAssetType.XML:
                    return "xml";
                case BTAssetType.CSBuilder:
                    return "cs";
            }
            return string.Empty;
        }

        private bool ExportFromGraph(Graph<BehaviorTreeAssetWrapper, NodeWrapper> graph, BTAssetType assetType)
        {
            BehaviorTreeAssetWrapper asset = new BehaviorTreeAssetWrapper();
            graph.Save(asset);
            BehaviorTree behaviorTree = asset.behaviorTree;
            return ExportBehaviorTree(assetType, behaviorTree);
        }

        private Graph<BehaviorTreeAssetWrapper, NodeWrapper> ImportAndLoad(BTAssetType assetType)
        {
            BehaviorTree behaviorTree = ImportBehaviorTree(assetType);
            BehaviorTreeAssetWrapper asset = new BehaviorTreeAssetWrapper();
            asset.behaviorTree = behaviorTree;
            Graph<BehaviorTreeAssetWrapper, NodeWrapper> newGraph = null;
            if (Graph<BehaviorTreeAssetWrapper, NodeWrapper>.Load(asset, out newGraph))
            {
                return newGraph;
            }
            return null;
        }

        private bool ExportBehaviorTree(BTAssetType assetType, BehaviorTree behaviorTree)
        {
            string filePath = EditorUtility.SaveFilePanel("存到哪里", ".", "bt", GetFileExt(assetType));
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
            string filePath = EditorUtility.OpenFilePanel("where to load?", ".", GetFileExt(assetType));
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

