using UnityEditor;

namespace AillieoUtils.EasyBehaviorTree.Creator
{
    public class ParamValueDrawer_Int : IParamValueDrawer<int>
    {
        public int Draw(int oldValue)
        {
            return EditorGUILayout.IntField(oldValue);
        }
    }

    public class ParamValueDrawer_Long : IParamValueDrawer<long>
    {
        public long Draw(long oldValue)
        {
            return EditorGUILayout.LongField(oldValue);
        }
    }

    public class ParamValueDrawer_Boolean : IParamValueDrawer<bool>
    {
        public bool Draw(bool oldValue)
        {
            return EditorGUILayout.Toggle(oldValue);
        }
    }

    public class ParamValueDrawer_Float : IParamValueDrawer<float>
    {
        public float Draw(float oldValue)
        {
            return EditorGUILayout.FloatField(oldValue);
        }
    }

    public class ParamValueDrawer_Double : IParamValueDrawer<double>
    {
        public double Draw(double oldValue)
        {
            return EditorGUILayout.DoubleField(oldValue);
        }
    }

    public class ParamValueDrawer_String : IParamValueDrawer<string>
    {
        public string Draw(string oldValue)
        {
            return EditorGUILayout.TextField(oldValue);
        }
    }

}
