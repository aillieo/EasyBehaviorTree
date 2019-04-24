using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace EasyBehaviorTree
{
    [Serializable]
    public class BehaviorTree
    {
        public event Action<BehaviorTree> OnBehaviorTreeStarted;
        public event Action<BehaviorTree, BTState> OnBehaviorTreeCompleted;

        public NodeBase root;

        public BlackBoard blackBoard { get; private set; }

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
#else
        private BehaviorTree()
        { }
#endif

        public void Init()
        {
            if (isRunning)
            {
                return;
            }
            this.blackBoard = new BlackBoard();
            this.random = new Random(DateTime.Now.Second);
            NodeBase.Init(root, this);
        }

        public void Restart()
        {
            if (isRunning)
            {
                return;
            }

            Init();

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


        public string DumpTree()
        {
            StringBuilder sb = new StringBuilder();
            root.DumpNode(sb, 0);
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
    }
}
