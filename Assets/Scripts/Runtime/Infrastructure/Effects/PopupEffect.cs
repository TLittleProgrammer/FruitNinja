using DG.Tweening;
using Runtime.Infrastructure.StateMachine;
using Runtime.Infrastructure.StateMachine.States;
using UnityEngine;

namespace Runtime.Infrastructure.Effects
{
    public abstract class PopupEffect : MonoBehaviour
    {
        public Sequence Sequence;
        private IGameStateMachine _gameStateMachine;
        
        public void Initialize(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        private void OnEnable()
        {
            if (_gameStateMachine is null)
            {
                return;
            }
            
            _gameStateMachine.UpdatedState += OnUpdatedState;

            if ((_gameStateMachine.CurrentState as IExitableState) is PauseState or LooseState)
            {
                Sequence.Pause();
            }
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