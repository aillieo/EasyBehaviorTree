using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyBehaviorTree
{

    [Serializable]
    public class FloatParam: NodeParam<float>
    {
    }

    [Serializable]
    public class FloatParamSet : NodeParamSet<FloatParam, float>
    {
    }
}
