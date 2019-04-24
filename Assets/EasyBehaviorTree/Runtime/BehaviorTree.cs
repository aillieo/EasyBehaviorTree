using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

namespace EasyBehaviorTree
{
    [Serializable]
    public class BehaviorTree
    {
        public event Action<BehaviorTree> OnBehaviorTreeEnd;

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

        public void Init()
        {
            if (isRunning)
            {
                return;
            }
            this.blackBoard = new BlackBoard();
            this.random = new Random(DateTime.Now.Second);
            NodeBase.Init(root,this);
        }

        public void Restart()
        {
            if(isRunning)
            {
                return;
            }

            Init();

            isRunning = true;
        }

        public void Tick(float deltaTime)
        {
            if(!isRunning)
            {
                return;
            }

            BTState ret = NodeBase.TickNode(root,deltaTime);

            if (ret != BTState.Running)
            {
                isRunning = false;

                if(OnBehaviorTreeEnd != null)
                {
                    OnBehaviorTreeEnd.Invoke(this);
                }
            }

            logger.Debug("tree ret = " + ret);
        }

        
        public string DumpTree()
        {
            StringBuilder sb = new StringBuilder();
            root.DumpNode(sb,0);
            return sb.ToString();
        }
    }
}
