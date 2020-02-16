using System.Collections.Generic;

namespace AillieoUtils.EasyBehaviorTree
{
    public class BehaviorTreeBuilder
    {
        private Stack<NodeParent> stack = new Stack<NodeParent>();

        public BehaviorTreeBuilder AddAction(NodeAction nodeAction)
        {
            this.stack.Peek().AddChild(nodeAction);
            return this;
        }

        public BehaviorTreeBuilder AddCondition(NodeCondition nodeCondition)
        {
            this.stack.Peek().AddChild(nodeCondition);
            return this;
        }

        public BehaviorTreeBuilder AddComposite(NodeComposite nodeComposite)
        {
            if (this.stack.Count > 0)
            {
                this.stack.Peek().AddChild(nodeComposite);
            }
            this.stack.Push(nodeComposite);
            return this;
        }

        public BehaviorTreeBuilder AddDecorator(NodeDecorator nodeDecorator)
        {
            if (this.stack.Count > 0)
            {
                this.stack.Peek().AddChild(nodeDecorator);
            }
            this.stack.Push(nodeDecorator);
            return this;
        }

        public BehaviorTreeBuilder EndCurrent()
        {
            this.stack.Pop();
            return this;
        }

        public BehaviorTree Build(out string err, out NodeBase node)
        {
            while (this.stack.Count > 1)
            {
                this.EndCurrent();
            }

            BehaviorTree behaviorTree = CreatorUtils.NewBehaviorTree(stack.Pop());

            if (behaviorTree.Validate(out err, out node))
            {
                return behaviorTree;
            }
            else
            {
                DefaultLogger logger = new DefaultLogger();
                logger.Error(err);
                return null;
            }
        }
    }
}
