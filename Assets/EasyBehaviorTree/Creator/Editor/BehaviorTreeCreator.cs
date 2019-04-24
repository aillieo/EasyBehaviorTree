using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.IO;

namespace EasyBehaviorTree.Creator
{
    public class BehaviorTreeCreator
    {

        [MenuItem("Assets/EasyBehaviorTree/CreateTreeAsset", false, 0)]
        public static BehaviorTree CreateTreeAsset()
        {
            GameObject go = Selection.activeGameObject;
            if (null == go)
            {
                return null;
            }

            Transform root = go.transform;

            BehaviorTree behaviorTree = new BehaviorTree();
            NodeBase rootNode = GameObjectToNode(root.gameObject);
            if(rootNode == null)
            {
                Debug.LogError("create root failed");
                return null;
            }

            behaviorTree.root = rootNode;

            ProcessChildrenForTrans(root, behaviorTree.root);

            string fullPath = GetFullPathForTree(go);
            AillieoUtils.Utils.SerializeDataToBytes(behaviorTree, fullPath);

            Debug.Log("Created successfully!\n" + fullPath + "\n" + behaviorTree.DumpTree());

            return behaviorTree;

        }


        static void ProcessChildrenForTrans(Transform parentTrans, NodeBase parentNode)
        {
            foreach (Transform t in parentTrans)
            {
                var node = GameObjectToNode(t.gameObject);
                if (node == null)
                {
                    continue;
                }

                NodeComposite composite = parentNode as NodeComposite;
                if(composite != null)
                {
                    composite.AddChild(node);
                    ProcessChildrenForTrans(t, node);
                }

            }
        }

        static string GetFullPathForTree(GameObject prefab)
        {
            string prefabPath = AssetDatabase.GetAssetPath(prefab);
            string path = Path.GetDirectoryName(prefabPath);
            return string.Format("{0}/../{1}/{2}.bt", Application.dataPath, path,prefab.name);
        }

        static NodeBase GameObjectToNode(GameObject go)
        {
            NodeDefine nodeDefine = go.GetComponent<NodeDefine>();
            if (nodeDefine != null)
            {
                return nodeDefine.CreateNode();
            }

            return null;
        }
    }

}
