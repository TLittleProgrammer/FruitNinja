using System.Collections.Generic;
using Runtime.Infrastructure.StateMachine.States;

namespace Runtime.Infrastructure.StateMachine
{
    public interface IGameStateMachine : IAsyncInitializable<IEnumerable<IExitableState>>
    {
        IState CurrentState { get; } 
        void Enter<TState>() where TState : class, IState;
    }
}