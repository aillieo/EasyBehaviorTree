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

        public bool enableLog { get; set; } = false;

        public bool isRunning { get; private set; } = false;

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

            Log("tree ret = " + ret);
        }

        public void Log(string message, Action<string> logAction = null)
        {
            if(!enableLog)
            {
                return;
            }

            if (logAction == null)
            {
                logAction = UnityEngine.Debug.Log;
            }

            logAction(message);
        }

        public string DumpTree()
        {
            StringBuilder sb = new StringBuilder();
            root.DumpNode(sb,0);
            return sb.ToString();
        }
    }
}
