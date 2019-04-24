using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyBehaviorTree.Creator
{

    [Serializable]
    public class IntParam : NodeParam<int>
    {
    }

    [Serializable]
    public class IntParamSet : NodeParamSet<IntParam, int>
    {
    }
}
