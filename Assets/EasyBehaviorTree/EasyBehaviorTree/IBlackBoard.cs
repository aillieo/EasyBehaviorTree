using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System;

namespace EasyBehaviorTree
{
    public interface IBlackBoard
    {
        T Get<T>(string key);

        void Set<T>(string key, T value);

        event Action<string> OnValueChanged;

    }

}
