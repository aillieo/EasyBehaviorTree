using System.Text;
using System;

namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public sealed class BehaviorTree
    {
        public event Action<BehaviorTree> OnBehaviorTreeStarted;
        public event Action<BehaviorTree, BTState> OnBehaviorTreeCompleted;

        internal BehaviorTreeStructure behaviorTreeStructure;
        [field:NonSerialized]
        internal BehaviorTreeVisitor behaviorTreeVisitor { get; set; }
        
        internal NodeBase root { get; private set; }

        public Blackboard blackboard
        {
            get
            {
                return behaviorTreeVisitor.blackboard;
            }
        }

        public Blackboard sharedBlackboard
        {
            get
            {
                return behaviorTreeVisitor.sharedBlackboard;
            }
        }

        public bool debugLogging
        {
            get
            {
                return behaviorTreeVisitor.debugLogging;
            }
            set
            {
                behaviorTreeVisitor.debugLogging = value;
            }
        }

        public DefaultLogger logger
        {
            get
            {
                return behaviorTreeVisitor.logger;
            }
        }

        public bool Validate(out string error, out NodeBase errorNode)
        {
            return this.behaviorTreeStructure.Validate(out error, out errorNode);
        }

        internal BehaviorTree(NodeBase root)
        {
            this.behaviorTreeStructure = new BehaviorTreeStructure(root);
            this.behaviorTreeVisitor = new BehaviorTreeVisitor(root);
        }

        internal BehaviorTree(BehaviorTreeStructure behaviorTreeStructure)
        {
            this.behaviorTreeStructure = behaviorTreeStructure;
            this.behaviorTreeVisitor = new BehaviorTreeVisitor(behaviorTreeStructure.root);
        }
        
        public BehaviorTree(BehaviorTree behaviorTree)
            : this(behaviorTree.behaviorTreeStructure)
        {
        }

        public void Init()
        {
            if (behaviorTreeVisitor.isRunning)
            {
                return;
            }
            
            behaviorTreeVisitor.Init(this);
        }

        public void Restart()
        {
            this.behaviorTreeVisitor.Restart();
        }

        public void Tick(float deltaTime)
        {
            BTState ret = behaviorTreeVisitor.Tick(deltaTime);

            if (ret != BTState.Running)
            {
                logger.Debug("Tree complete : " + ret);

                if (OnBehaviorTreeCompleted != null)
                {
                    OnBehaviorTreeCompleted.Invoke(this, ret);
                }
            }
        }

        public string DumpTree(INodeInfoFormatter formatter = null)
        {
            return behaviorTreeStructure.DumpTree(formatter);
        }

        public void CleanUp()
        {
            behaviorTreeVisitor.CleanUp();
        }
    }
}
