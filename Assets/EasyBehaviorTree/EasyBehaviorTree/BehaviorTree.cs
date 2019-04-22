using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

namespace EasyBehaviorTree
{
    [Serializable]
    public class BehaviorTree
    {
        public NodeBase root;

        public BlackBoard blackBoard;

        public Random random;

        public bool enableLog = false;

        public void Init()
        {
            this.blackBoard = new BlackBoard();
            this.random = new Random(0);
            NodeBase.Init(root,this);
        }

        public void Tick(float deltaTime)
        {
            BTState ret = NodeBase.TickNode(root,deltaTime);
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
