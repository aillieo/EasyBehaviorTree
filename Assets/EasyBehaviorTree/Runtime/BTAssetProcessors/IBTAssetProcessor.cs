namespace AillieoUtils.EasyBehaviorTree
{
    public interface IBTAssetProcessor
    {
        BehaviorTree Load(string filepath);
        bool Save(BehaviorTree behaviorTree, string filepath);
    }
}
