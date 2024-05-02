using System;
using System.Collections.Generic;
using Runtime.Infrastructure.StateMachine.States;

namespace Runtime.Infrastructure.StateMachine
{
    public interface IGameStateMachine : IAsyncInitializable<IEnumerable<IExitableState>>
    {
        event Action<IExitableState> UpdatedState;
        IState CurrentState { get; } 
        IState PreviousState { get; }
        void Enter<TState>() where TState : class, IState;
        void ReturnPreviousState();
    }
}