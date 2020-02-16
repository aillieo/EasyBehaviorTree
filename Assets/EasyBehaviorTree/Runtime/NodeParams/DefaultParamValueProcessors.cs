namespace AillieoUtils.EasyBehaviorTree
{

    public class ParamValueProcessor_Int : IParamValueProcessor<int>
    {
        public int Load(string serializedValue)
        {
            return int.Parse(serializedValue);
        }

        public string Save(int value)
        {
            return value.ToString();
        }
    }

    public class ParamValueProcessor_Long : IParamValueProcessor<long>
    {
        public long Load(string serializedValue)
        {
            return long.Parse(serializedValue);
        }

        public string Save(long value)
        {
            return value.ToString();
        }
    }

    public class ParamValueProcessor_Boolean : IParamValueProcessor<bool>
    {
        public bool Load(string serializedValue)
        {
            return serializedValue == "True";
        }

        public string Save(bool value)
        {
            return value.ToString();
        }
    }

    public class ParamValueProcessor_Float : IParamValueProcessor<float>
    {
        public float Load(string serializedValue)
        {
            return float.Parse(serializedValue);
        }

        public string Save(float value)
        {
            return value.ToString();
        }
    }

    public class ParamValueProcessor_Double : IParamValueProcessor<double>
    {
        public double Load(string serializedValue)
        {
            return double.Parse(serializedValue);
        }

        public string Save(double value)
        {
            return value.ToString();
        }
    }

    public class ParamValueProcessor_String : IParamValueProcessor<string>
    {
        public string Load(string serializedValue)
        {
            return serializedValue;
        }

        public string Save(string value)
        {
            return value;
        }
    }

}
