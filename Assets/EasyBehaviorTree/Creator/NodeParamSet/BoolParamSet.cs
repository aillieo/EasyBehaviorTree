using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyBehaviorTree.Creator
{

    [Serializable]
    public class BoolParam : NodeParam<bool>
    {
    }

    [Serializable]
    public class BoolParamSet : NodeParamSet<BoolParam, bool>
    {
    }
}
