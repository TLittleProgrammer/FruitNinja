using Runtime.Infrastructure.StateMachine.States;

namespace Runtime.Infrastructure.StateMachine
{
    public interface IGameStateMachine
    {
        void Enter<TState>() where TState : class, IState;
    }
}