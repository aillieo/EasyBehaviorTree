using AillieoUtils.EasyGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AillieoUtils.EasyBehaviorTree.Creator
{
    public class NodeCreators : INodeDataCreators<NodeWrapper>
    {
        public NodeDataCreatorEntry<NodeWrapper>[] GetCreatorEntries()
        {
            return new NodeDataCreatorEntry<NodeWrapper>[] {

                new NodeDataCreatorEntry<NodeWrapper>("NodeComposite/NodeSequence", () => new NodeWrapper(new NodeSequence()), -1),
                new NodeDataCreatorEntry<NodeWrapper>("NodeComposite/NodeParallel", () => new NodeWrapper(new NodeParallel()), -1),
                new NodeDataCreatorEntry<NodeWrapper>("NodeComposite/NodeSelector", () => new NodeWrapper(new NodeSelector()), -1),

                new NodeDataCreatorEntry<NodeWrapper>("NodeAction/NodeActionLog", () => new NodeWrapper(new NodeActionLog()), -1),
                new NodeDataCreatorEntry<NodeWrapper>("NodeAction/NodeActionWait", () => new NodeWrapper(new NodeActionWait()), -1),

                new NodeDataCreatorEntry<NodeWrapper>("NodeCondition/NodeConditionHasSharedValue", () => new NodeWrapper(new NodeConditionHasSharedValue()), -1),
                new NodeDataCreatorEntry<NodeWrapper>("NodeCondition/NodeConditionHasValue", () => new NodeWrapper(new NodeConditionHasValue()), -1),
                new NodeDataCreatorEntry<NodeWrapper>("NodeCondition/NodeConditionRandom", () => new NodeWrapper(new NodeConditionRandom()), -1),
                
                new NodeDataCreatorEntry<NodeWrapper>("NodeDecorator/NodeInverter", () => new NodeWrapper(new NodeInverter()), -1),
                new NodeDataCreatorEntry<NodeWrapper>("NodeDecorator/NodeRepeater", () => new NodeWrapper(new NodeRepeater()), -1),
                new NodeDataCreatorEntry<NodeWrapper>("NodeDecorator/NodeReturnFailure", () => new NodeWrapper(new NodeReturnFailure()), -1),
                new NodeDataCreatorEntry<NodeWrapper>("NodeDecorator/NodeReturnSuccess", () => new NodeWrapper(new NodeReturnSuccess()), -1),
                new NodeDataCreatorEntry<NodeWrapper>("NodeDecorator/NodeUntilFailure", () => new NodeWrapper(new NodeUntilFailure()), -1),
                new NodeDataCreatorEntry<NodeWrapper>("NodeDecorator/NodeUntilSuccess", () => new NodeWrapper(new NodeUntilSuccess()), -1),
            };
        }
    }
}
