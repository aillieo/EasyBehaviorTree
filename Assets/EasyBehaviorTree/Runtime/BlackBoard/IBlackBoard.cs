namespace AillieoUtils.EasyBehaviorTree
{
    public interface IBlackBoard
    {
        IBlackBoardData this[string key]
        {
            get; set;
        }

        bool HasValue(string key);

        void CleanUp();
    }
}
