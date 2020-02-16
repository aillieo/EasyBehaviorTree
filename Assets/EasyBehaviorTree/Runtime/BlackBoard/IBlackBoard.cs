namespace AillieoUtils.EasyBehaviorTree
{
    public interface IBlackBoard
    {
        IBlackBoardData this[string key]
        {
            get; set;
        }

        bool HasValue(string key);

        bool Remove(string key);

        void CleanUp();
    }
}
