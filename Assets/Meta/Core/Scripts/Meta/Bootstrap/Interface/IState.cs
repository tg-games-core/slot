namespace Core.Bootstrap.States
{
    public interface IState: IExitableState
    {
        void Enter();
    }
}