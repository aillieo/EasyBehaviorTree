using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using AillieoUtils.EasyGraph;
using System;
using System.Linq;

namespace AillieoUtils.EasyBehaviorTree.Creator
{
    public class SerializedBehaviorTree : ScriptableObject, IGraphAsset<NodeWrapper>
    {

        [SerializeField]
        private Vector2 serializedCanvasSize = new Vector2(1920, 1080);

        [SerializeField]
        private SerializedNode[] serializedNodes;

        [SerializeField]
        private SerializedRoute[] serializedRoutes;


        public BehaviorTree LoadFromSerializedAsset()
        {
            NodeBase[] runtimeNodes = serializedNodes.Select(n => {
                NodeBase node = Activator.CreateInstance(ReflectionUtils.GetType(n.nodeType)) as NodeBase;
                node.name = n.name;
                CreatorUtils.ApplyParamInfo(node, n.param.Select(p => p.LoadFromSerializedInfo()).ToArray());
                return node;
            }).ToArray();
            foreach (var route in serializedRoutes)
            {
                CreatorUtils.AddChild(
                    runtimeNodes[route.fromIndex] as NodeParent,
                    runtimeNodes[route.toIndex]
                );
            }
            return CreatorUtils.NewBehaviorTree(runtimeNodes[0]);
        }

        public void SerializeBehaviorTree(BehaviorTree behaviorTree)
        {
            Dictionary<NodeBase, int> nodeIndexCache = new Dictionary<NodeBase, int>();
            NodeBase root = CreatorUtils.GetRoot(behaviorTree);
            List<SerializedNode> sNodes = new List<SerializedNode>();
            List<SerializedRoute> sRoutes = new List<SerializedRoute>();
            RecordNode(root, 0, 0, nodeIndexCache, sNodes, sRoutes);
            this.serializedNodes = sNodes.ToArray();
            this.serializedRoutes = sRoutes.ToArray();
        }

        public bool GraphToAsset(Vector2 canvasSize, IList<NodeDataWithPosition<NodeWrapper>> nodesToSave, IList<RouteDataWithNodeIndex> routesToSave)
        {
            serializedCanvasSize = canvasSize;
            serializedNodes = nodesToSave.Select(n => {
                return new SerializedNode() {
                    canvasPos = n.position,
                    name = n.nodeData.name,
                    nodeType = n.nodeData.nodeType,
                    param = n.nodeData.param.Select( p => new SerializedParamInfo(p)).ToArray(),
                };
            }).ToArray();
            serializedRoutes = routesToSave.Select(r => {
                return new SerializedRoute()
                {
                    fromIndex = r.fromIndex,
                    toIndex = r.toIndex,
                };
            }).ToArray();
            return true;
        }

        public bool AssetToGraph(out Vector2 canvasSize, out IList<NodeDataWithPosition<NodeWrapper>> nodesLoaded, out IList<RouteDataWithNodeIndex> routesLoaded)
        {
            canvasSize = serializedCanvasSize;
            nodesLoaded = serializedNodes.Select(n => {
                return new NodeDataWithPosition<NodeWrapper>(n.canvasPos, new NodeWrapper()
                {
                    name = n.name,
                    nodeType = n.nodeType,
                    param = n.param.Select(p => p.LoadFromSerializedInfo()).ToArray()
                });
            }).ToArray();
            routesLoaded = serializedRoutes.Select(n => {
                return new RouteDataWithNodeIndex(n.fromIndex, n.toIndex);
            }).ToArray();

            return true;
        }

        private void RecordNode(
            NodeBase node,
            int depth,
            int index,
            Dictionary<NodeBase, int> nodeIndexCache,
            List<SerializedNode> sNodes,
            List<SerializedRoute> sRoutes)
        {
            sNodes.Add(new SerializedNode() {
                canvasPos = new Vector2(depth * 160, index * 90),
                name = node.name,
                nodeType = node.GetType().FullName,
                param = CreatorUtils.ExtractParamInfo(node).Select(p => new SerializedParamInfo(p)).ToArray()
            });
            nodeIndexCache.Add(node, sNodes.Count - 1);

            CreatorUtils.VisitChildren(node as NodeParent,
                (idx,child) =>
                {
                    RecordNode(child, depth + 1, index + idx, nodeIndexCache, sNodes, sRoutes);
                    sRoutes.Add(new SerializedRoute() {
                        fromIndex = nodeIndexCache[node],
                        toIndex = nodeIndexCache[child]
                    });
                });
        }

    }


    [Serializable]
    public class SerializedNode
    {
        public Vector2 canvasPos;
        public string name;
        public string nodeType;
        public SerializedParamInfo[] param;
    }

    [Serializable]
    public class SerializedRoute
    {
        public int fromIndex;
        public int toIndex;
    }

    [Serializable]
    public class SerializedParamInfo
    {
        public string name;
        public string paramType;
        public string serializedValue;

        public ParamInfo LoadFromSerializedInfo()
        {
            Type t = ReflectionUtils.GetType(this.paramType);
            return new ParamInfo()
            {
                name = this.name,
                type = t,
                value = ParamInfoProcessor.Load(t, this.serializedValue)
            };
        }

        public SerializedParamInfo(ParamInfo param)
        {
            this.name = param.name;
            this.paramType = param.type.FullName;
            this.serializedValue = ParamInfoProcessor.Save(param.type, param.value);
        }
    }

}
