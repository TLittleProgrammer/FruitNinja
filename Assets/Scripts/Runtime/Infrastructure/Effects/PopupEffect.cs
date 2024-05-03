using System;
using DG.Tweening;
using Runtime.Infrastructure.StateMachine;
using Runtime.Infrastructure.StateMachine.States;

namespace Runtime.Infrastructure.Effects
{
    public abstract class PopupEffect : AddictableFromScale
    {
        private IGameStateMachine _gameStateMachine;
        
        public void Initialize(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        private void Update()
        {
            if ((_gameStateMachine.CurrentState as IExitableState) is PauseState or LooseState)
            {
                Sequence.Pause();
            }
            else
            {
                Sequence.Play();
            }
        }

        private void OnEnable()
        {
            TimeProvider.TimeScaleChanged += OnTimeScaleChanged;
            
            if (_gameStateMachine is null)
            {
                return;
            }
            
            _gameStateMachine.UpdatedState += OnUpdatedState;
        }

        private void OnDisable()
        {
            TimeProvider.TimeScaleChanged -= OnTimeScaleChanged;
            
            if (_gameStateMachine is null)
            {
                return;
            }
            
            _gameStateMachine.UpdatedState -= OnUpdatedState;
        }

        private void OnUpdatedState(IExitableState state)
        {
            if (state is PauseState or LooseState)
            {
                Sequence.Pause();
            }
            else
            {
                Sequence.Play();
            }
        }
    }
}