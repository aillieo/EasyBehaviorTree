using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

namespace EasyBehaviorTree
{
    public interface IBlackBoard
    {
        IBlackBoardData this[string key]
        {
            get; set;
        }
    }
}
