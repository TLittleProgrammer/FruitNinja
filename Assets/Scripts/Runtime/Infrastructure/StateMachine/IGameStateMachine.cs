using Runtime.Infrastructure.StateMachine.States;

namespace Runtime.Infrastructure.StateMachine
{
    public interface IGameStateMachine
    {
        IState CurrentState { get; } 
        void Enter<TState>() where TState : class, IState;
    }
}