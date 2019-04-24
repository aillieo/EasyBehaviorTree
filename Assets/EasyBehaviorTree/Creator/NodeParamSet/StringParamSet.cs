using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyBehaviorTree.Creator
{

    [Serializable]
    public class StringParam: NodeParam<string>
    {
    }

    [Serializable]
    public class StringParamSet : NodeParamSet<StringParam,string>
    {
    }
}
