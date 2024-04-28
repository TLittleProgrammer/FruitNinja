using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Runtime.Infrastructure.StateMachine.States;
using IState = Runtime.Infrastructure.StateMachine.States.IState;

namespace Runtime.Infrastructure.StateMachine
{
    public sealed class GameStateMachine : IGameStateMachine
    {
        private Dictionary<Type, IExitableState> _states;

        private IExitableState _activeState;
        
        public GameStateMachine(IEnumerable<IExitableState> states)
        {
            _states = states
                .ToDictionary(x => x.GetType(), x => x);
        }

        public async UniTask AsyncInitialize(IEnumerable<IExitableState> payload)
        {
            _states = payload
                .ToDictionary(x => x.GetType(), x => x);

            await UniTask.CompletedTask;
        }

        public IState CurrentState => _activeState as IState;

        public void Enter<TState>() where TState : class, IState
        {
            TState state = ChangeState<TState>();
            
            state.Enter();
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();

            TState state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState
        {
            return _states[typeof(TState)] as TState;
        }
    }
}