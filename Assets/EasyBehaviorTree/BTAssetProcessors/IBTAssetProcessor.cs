namespace AillieoUtils.EasyBehaviorTree
{
    public interface IBTAssetProcessor<T>
    {
        BehaviorTree Load(T asset);
        T Save(BehaviorTree behaviorTree);
    }
}
