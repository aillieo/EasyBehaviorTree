using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace EasyBehaviorTree.Creator
{
    public class BehaviorTreeCreator
    {

        private Dictionary<NodeDefine, NodeBase> nodeCreateInfo = new Dictionary<NodeDefine, NodeBase>();

        [MenuItem("Assets/EasyBehaviorTree/CreateTreeAsset", false, 0)]
        [MenuItem("GameObject/EasyBehaviorTree/CreateTreeAsset", false, 0)]
        private static BehaviorTree CreateTreeAsset()
        {
            GameObject go = Selection.activeGameObject;
            BehaviorTreeCreator creator = new BehaviorTreeCreator();
            return creator.CreateTree(go);
        }

        [MenuItem("Assets/EasyBehaviorTree/CreateTreeAsset", true, 0)]
        [MenuItem("GameObject/EasyBehaviorTree/CreateTreeAsset", true, 0)]
        private static bool CreateTreeAssetValidateFunction()
        {
            if (Selection.objects.Length != 1)
            {
                return false;
            }

            GameObject go = Selection.activeGameObject;
            if (null == go)
            {
                return false;
            }

            NodeDefine  nodeDefine = go.GetComponent<NodeDefine>();
            if(nodeDefine == null)
            {
                return false;
            }

            return true;
        }

        private BehaviorTree CreateTree(GameObject go)
        {
            BehaviorTree behaviorTree = new BehaviorTree();
            NodeBase rootNode = GameObjectToNode(go);
            if (rootNode == null)
            {
                Debug.LogError("create root failed");
                return null;
            }

            behaviorTree.root = rootNode;

            ProcessChildrenForTrans(go.transform, behaviorTree.root);

            if (Validate())
            {
                string fullPath = GetFullPathForTree(go);
                SaveBehaviorTree(behaviorTree, fullPath);
                Debug.Log("Created successfully!\n" + fullPath + "\n" + behaviorTree.DumpTree());
                return behaviorTree;
            }
            else
            {
                return null;
            }
        }

        private void ProcessChildrenForTrans(Transform parentTrans, NodeBase parentNode)
        {
            foreach (Transform t in parentTrans)
            {
                var node = GameObjectToNode(t.gameObject);
                if (node == null)
                {
                    continue;
                }

                NodeParent parent = parentNode as NodeParent;
                if(parent != null)
                {
                    parent.AddChild(node);
                    ProcessChildrenForTrans(t, node);
                    continue;
                }
            }
        }

        private static string GetFullPathForTree(GameObject prefab)
        {
            string prefabPath = AssetDatabase.GetAssetPath(prefab);
            string path = Path.GetDirectoryName(prefabPath);
            return string.Format("{0}/../{1}/{2}.bt", Application.dataPath, path,prefab.name);
        }

        private NodeBase GameObjectToNode(GameObject go)
        {
            if(go != null)
            {
                NodeDefine nodeDefine = go.GetComponent<NodeDefine>();
                if (nodeDefine != null)
                {
                    NodeBase node = nodeDefine.CreateNode();
                    if (node != null)
                    {
                        nodeCreateInfo.Add(nodeDefine, node);
                    }
                    return node;
                }
            }
            return null;
        }

        private static bool SaveBehaviorTree(BehaviorTree behaviorTree, string filename)
        {
            using (Stream stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, behaviorTree);
                stream.Close();
                AssetDatabase.Refresh();
                return true;
            }
        }

        private bool Validate()
        {
            bool valid = true;
            string error;
            foreach(var pair in nodeCreateInfo)
            {
                error = null;
                if(!pair.Value.Validate(out error))
                {
                    valid = false;
                    Debug.LogError(string.Format("{0}: {1}({2})", error,pair.Value.name,pair.Key.gameObject.name), pair.Key.gameObject);
                }
            }

            nodeCreateInfo.Clear();
            return valid;
        }
    }
}
