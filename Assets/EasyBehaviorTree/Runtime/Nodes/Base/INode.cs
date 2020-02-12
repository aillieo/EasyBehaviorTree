namespace AillieoUtils.EasyBehaviorTree
{

    public interface INode
    {
        void Init();
        void Reset();
        BTState Update(float deltaTime);
        void OnEnter();
        void OnExit();
        void Cleanup();
#if UNITY_EDITOR
        bool Validate(out string error);
#endif
    }
}
