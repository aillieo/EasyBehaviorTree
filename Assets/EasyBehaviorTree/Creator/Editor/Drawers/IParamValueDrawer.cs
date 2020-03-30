namespace AillieoUtils.EasyBehaviorTree.Creator
{
    public interface IParamValueDrawer<T>
    {
        T Draw(T oldValue);
    }
}
