using System.Text;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AillieoUtils.EasyBehaviorTree
{
    public sealed class BehaviorTreeVisitor
    {
        private Stack<NodeBase> executingStack;
        
        private ConditionalWeakTable<NodeParent, BTState[]> childrenStateCache = new ConditionalWeakTable<NodeParent, BTState[]>();
        private ConditionalWeakTable<NodeAction, IBTTask> actionTaskCache = new ConditionalWeakTable<NodeAction, IBTTask>();

        private bool treeInited = false;

        public Blackboard blackboard { get; private set; }

        private static Blackboard mSharedBlackboard = new Blackboard();

        public Blackboard sharedBlackboard
        {
            get { return mSharedBlackboard; }
        }

        internal NodeBase root { get; private set; }

        internal BehaviorTreeVisitor(NodeBase root)
        {
            this.root = root;
        }

        public bool debugLogging
        {
            get
            {
                return (logger.filter & LogLevel.Debug) > 0;
            }
            set
            {
                if (value)
                {
                    logger.filter |= LogLevel.Debug;
                }
                else
                {
                    logger.filter &= LogLevel.NonDebug;
                }
            }
        }

        public bool isRunning { get; private set; } = false;

        [NonSerialized]
        private DefaultLogger mLogger;

        public DefaultLogger logger
        {
            get
            {
                if (mLogger == null)
                {
                    mLogger = new DefaultLogger();
                    mLogger.filter = LogLevel.NonDebug;
                }
                return mLogger;
            }
            set
            {
                mLogger = value;
            }
        }

        public Random random { get; private set; }

        internal void Init(BehaviorTree behaviorTree)
        {
            if (isRunning || treeInited)
            {
                return;
            }

            NodeBase.InitNode(root, this);

            this.treeInited = true;
        }

        private void ResetNodes()
        {
            NodeBase.ResetNode(root, this);
        }

        public void Restart()
        {
            if (isRunning)
            {
                return;
            }

            if (!treeInited)
            {
                throw new InvalidOperationException();
            }

            if (executingStack == null)
            {
                executingStack = new Stack<NodeBase>();
            }
            else
            {
                executingStack.Clear();
            }

            if (blackboard == null)
            {
                blackboard = new Blackboard();
            }
            else
            {
                blackboard.CleanUp();
            }

            if (random == null)
            {
                this.random = new Random(DateTime.Now.Second);
            }

            ResetNodes();
            
            //if(OnBehaviorTreeStarted != null)
            //{
            //    OnBehaviorTreeStarted.Invoke(this);
            //}

            isRunning = true;
        }

        public BTState Tick(float deltaTime)
        {
            if (!isRunning)
            {
                throw new InvalidOperationException();
            }
            
            BTState ret = NodeBase.NodeTick(root, this, deltaTime);

            if (ret != BTState.Running)
            {
                isRunning = false;
            }

            return ret;
        }

        internal BTState[] GetChildrenState(NodeParent nodeParent)
        {
            if (!childrenStateCache.TryGetValue(nodeParent, out BTState[] states))
            {
                states = new BTState[nodeParent.Children.Count];
                childrenStateCache.Add(nodeParent, states);
            }

            return states;
        }

        internal IBTTask GetTask(NodeAction nodeAction)
        {
            if (!actionTaskCache.TryGetValue(nodeAction, out IBTTask task))
            {
                task = nodeAction.CreateBTTask(this);
                actionTaskCache.Add(nodeAction, task);
            }

            return task;
        }

        public void CleanUp()
        {
            this.blackboard.CleanUp();

            root.OnTreeCleanUp(this);
        }
    }
}
