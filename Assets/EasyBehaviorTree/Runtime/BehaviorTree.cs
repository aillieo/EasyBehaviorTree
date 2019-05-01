using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace EasyBehaviorTree
{
    [Serializable]
    public sealed class BehaviorTree
    {
        public event Action<BehaviorTree> OnBehaviorTreeStarted;
        public event Action<BehaviorTree, BTState> OnBehaviorTreeCompleted;

        private bool treeInited = false;

        public NodeBase root { get;
#if UNITY_EDITOR
            set;
#endif
        }

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

#if UNITY_EDITOR
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

#else
        private BehaviorTree()
        { }
#endif

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

            BTState ret = NodeBase.TickNode(root, deltaTime);

            if (ret != BTState.Running)
            {
                isRunning = false;

                if (OnBehaviorTreeCompleted != null)
                {
                    OnBehaviorTreeCompleted.Invoke(this, ret);
                }
            }

            logger.Debug("tree ret = " + ret);
        }


        public string DumpTree(bool withBriefInfo = false)
        {
            StringBuilder sb = new StringBuilder();
            root.DumpNode(sb, new DefaultFormatter(), 0);
            return sb.ToString();
        }

        public static BehaviorTree LoadBehaviorTree(string filename)
        {
            if (!File.Exists(filename))
            {
                return null;
            }

            BehaviorTree behaviorTree = null;
            using (Stream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                behaviorTree = formatter.Deserialize(stream) as BehaviorTree;
                stream.Close();
            }
            return behaviorTree;
        }

        public void CleanUp()
        {
            root.OnTreeCleanUp();
            this.blackBoard.CleanUp();
        }
    }
}
