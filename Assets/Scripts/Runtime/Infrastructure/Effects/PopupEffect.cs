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
            if ((_gameStateMachine.CurrentState as IExitableState) is PauseState)
            {
                    Sequence.Pause();
            }
            else
            {
                Sequence.timeScale = TimeProvider.TimeScale;
                Sequence.Play();
            }
        }

        private void OnEnable()
        {
            if (_gameStateMachine is null)
            {
                return;
            }
            
            _gameStateMachine.UpdatedState += OnUpdatedState;
        }

        private void OnDisable()
        {
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