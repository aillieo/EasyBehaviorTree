namespace AillieoUtils.EasyBehaviorTree
{
    public interface IParamValueProcessor<T>
    {
        T Load(string serializedValue);
        string Save(T value);
    }
}
