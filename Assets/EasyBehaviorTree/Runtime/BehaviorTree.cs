using System.Text;
using System;
using Random = System.Random;

namespace AillieoUtils.EasyBehaviorTree
{
    [Serializable]
    public sealed class BehaviorTree
    {
        public event Action<BehaviorTree> OnBehaviorTreeStarted;
        public event Action<BehaviorTree, BTState> OnBehaviorTreeCompleted;

        private bool treeInited = false;

        internal NodeBase root { get; private set; }

        public BlackBoard blackBoard { get; private set; }

        public static BlackBoard sharedBlackBoard { get; private set; } = new BlackBoard();

        public Random random { get; private set; }

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
        private BaseLogger mLogger;

        public BaseLogger logger
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

        public bool Validate(out string error, out NodeBase errorNode)
        {
            return ValidateNodeAndChildren(root, out error, out errorNode);
        }

        private static bool ValidateNodeAndChildren(NodeBase node, out string error, out NodeBase errorNode)
        {
            errorNode = node;
            if(!node.Validate(out error))
            {
                return false;
            }

            NodeParent nodeParent = node as NodeParent;
            if(nodeParent != null)
            {
                foreach (var child in nodeParent.Children)
                {
                    if (!ValidateNodeAndChildren(child, out error, out errorNode))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        internal BehaviorTree(NodeBase root)
        {
            this.root = root;
        }

        private void Init()
        {
            if (isRunning)
            {
                return;
            }

            this.blackBoard = new BlackBoard();
            this.random = new Random(DateTime.Now.Second);

            NodeBase.InitNode(root, this);
        }

        private void ResetNodes()
        {
            NodeBase.ResetNode(root);
        }

        public void Restart()
        {
            if (isRunning)
            {
                return;
            }

            if(!treeInited)
            {
                Init();
                treeInited = true;
            }

            ResetNodes();

            if(OnBehaviorTreeStarted != null)
            {
                OnBehaviorTreeStarted.Invoke(this);
            }

            isRunning = true;
        }

        public void Tick(float deltaTime)
        {
            if (!isRunning)
            {
                return;
            }

            BTState ret = NodeBase.NodeTick(root, deltaTime);

            if (ret != BTState.Running)
            {
                isRunning = false;

                logger.Debug("Tree complete : " + ret);

                if (OnBehaviorTreeCompleted != null)
                {
                    OnBehaviorTreeCompleted.Invoke(this, ret);
                }
            }
        }

        public string DumpTree(INodeInfoFormatter formatter = null)
        {
            StringBuilder sb = new StringBuilder();
            if (formatter == null)
            {
                formatter = new DefaultFormatter();
            }
            root.DumpNode(sb, formatter, 0);
            return sb.ToString();
        }

        public void CleanUp()
        {
            root.OnTreeCleanUp();
            this.blackBoard.CleanUp();
        }
    }
}
