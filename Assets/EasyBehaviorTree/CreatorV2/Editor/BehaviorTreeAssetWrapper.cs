using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using AillieoUtils.EasyGraph;

namespace AillieoUtils.EasyBehaviorTree.Creator
{
    public class BehaviorTreeAssetWrapper : ScriptableObject, IGraphAsset<NodeWrapper>
    {
        public BehaviorTree behaviorTree { get; set; }

        public bool GraphToAsset(Vector2 canvasSize, IList<NodeDataWithPosition<NodeWrapper>> nodesToSave, IList<RouteDataWithNodeIndex> routesToSave)
        {
            foreach (var n in nodesToSave)
            {
                CreatorUtils.RemoveAllChildren(n.nodeData.node as NodeParent);
            }

            foreach (var route in routesToSave)
            {
                CreatorUtils.AddChild(
                    nodesToSave[route.fromIndex].nodeData.node as NodeParent,
                    nodesToSave[route.toIndex].nodeData.node
                );
            }

            NodeBase root = nodesToSave[0].nodeData.node;
            this.behaviorTree = CreatorUtils.NewBehaviorTree(root);
            return true;
        }

        public bool AssetToGraph(out Vector2 canvasSize, out IList<NodeDataWithPosition<NodeWrapper>> nodesLoaded, out IList<RouteDataWithNodeIndex> routesLoaded)
        {
            canvasSize = new Vector2(1920,1080);
            nodesLoaded = null;
            routesLoaded = null;
            if (behaviorTree == null)
            {
                return false;
            }

            Dictionary<NodeBase,int> nodeIndexCache = new Dictionary<NodeBase,int>();
            var record = new List<NodeDataWithPosition<NodeWrapper>>();
            var routes = new List<RouteDataWithNodeIndex>();

            NodeBase root = CreatorUtils.GetRoot(behaviorTree);
            RecordNode(root, 0, 0, nodeIndexCache, record, routes);

            nodesLoaded = record;
            routesLoaded = routes;
            return true;
        }

        private void RecordNode(
            NodeBase node,
            int depth,
            int index,
            Dictionary<NodeBase,int> nodeIndexCache,
            List<NodeDataWithPosition<NodeWrapper>> graphNodes,
            List<RouteDataWithNodeIndex> graphRoutes)
        {
            graphNodes.Add(new NodeDataWithPosition<NodeWrapper>(new Vector2(depth*160,index*90), new NodeWrapper(node)));
            nodeIndexCache.Add(node, graphNodes.Count - 1);
            CreatorUtils.VisitChildren(node as NodeParent,
                (idx,child) =>
                {
                    RecordNode(child, depth + 1, index + idx, nodeIndexCache, graphNodes, graphRoutes);
                    graphRoutes.Add(new RouteDataWithNodeIndex(nodeIndexCache[node],nodeIndexCache[child]));
                });
        }
    }
}
