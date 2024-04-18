namespace Runtime.Infrastructure.StateMachine.States
{
    public interface IState : IExitableState
    {
        void Enter();
    }
}