﻿using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Runtime.Infrastructure.StateMachine;
using Runtime.Infrastructure.StateMachine.States;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Timer
{
    //TODO Подумать над именем класса
    public class Timer : ITickable, IAsyncInitializable<IGameStateMachine, ITimeProvider>
    {
        private IGameStateMachine _gameStateMachine;
        private List<TimerData> _timers = new();
        private ITimeProvider _timeProvider;

        public async UniTask AsyncInitialize(IGameStateMachine payload, ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
            _gameStateMachine = payload;
            await UniTask.CompletedTask;
        }

        public void Tick()
        {
            if (_gameStateMachine is null || _gameStateMachine.CurrentState is PauseState or LooseState)
            {
                return;
            }
            
            for (int i = 0; i < _timers.Count; i++)
            {
                TimerData timerData = _timers[i];

                timerData.CurrentTime -= _timeProvider.DeltaTime;

                if (timerData.IsAnimated is false && timerData.CurrentTime <= timerData.SpecialIntervalBeforeShit)
                {
                    timerData.ChangeScale.Invoke();
                    timerData.IsAnimated = true;
                }
                
                if (timerData.CurrentTime <= 0f)
                {
                    timerData.CurrentTime = timerData.InitialTime;
                    timerData.TimeEnded.Invoke();
                }
            }
        }

        public void AddTimerData(TimerData timerData)
        {
            _timers.Add(timerData);
        }

        public void RemoveTimeData(TimerData timerData)
        {
            _timers.Remove(timerData);
        }
    }

    public class TimerData
    {
        public float InitialTime;
        public float CurrentTime;
        public float SpecialIntervalBeforeShit;
        public Action TimeEnded;
        public Action ChangeScale;
        public bool IsAnimated = false;
    }
}