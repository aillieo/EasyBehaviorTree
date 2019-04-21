using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System;

namespace EasyBehaviorTree
{
    [Serializable]
    public class BehaviorTree
    {
        public NodeBase root;

        public BlackBoard blackBoard;

        public void Init()
        {
            this.blackBoard = new BlackBoard();
            NodeBase.Init(root,this);
        }

        public void Tick()
        {
            BTState ret = NodeBase.TickNode(root);
            Debug.Log("tree ret = " + ret);
        }

        public string DumpTree()
        {
            StringBuilder sb = new StringBuilder();
            root.DumpNode(sb,0);
            return sb.ToString();
        }
    }
}
