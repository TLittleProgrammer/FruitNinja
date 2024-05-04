using System;
using Cysharp.Threading.Tasks;
using Runtime.Infrastructure.StateMachine;
using Runtime.Infrastructure.StateMachine.States;
using UnityEngine;

namespace Runtime.Infrastructure.Timer
{
    public sealed class Stopwatch : IStopwatchable
    {
        private IGameStateMachine _gameStateMachine;
        public event Action<int> Ticked;
        public event Action TickEnded;

        private float _time = 0f;
        private ITimeProvider _timeProvider;

        public async UniTask AsyncInitialize(IGameStateMachine payload, ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
            _gameStateMachine = payload;
            await UniTask.CompletedTask;
        }

        public void Tick()
        {
            if (_time <= 0f || _gameStateMachine.CurrentState is PauseState)
                return;

            _time -= _timeProvider.DeltaTime;
            Ticked?.Invoke((int)_time);

            if (_time <= 0f)
            {
                TickEnded?.Invoke();
            }
        }

        public void Notch(int time)
        {
            _time = time;
        }
    }
}