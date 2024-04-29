using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Timer
{
    //TODO Подумать над именем класса
    public class Timer : ITickable
    {
        private List<TimerData> _timers = new();

        public void Tick()
        {
            for (int i = 0; i < _timers.Count; i++)
            {
                TimerData timerData = _timers[i];

                timerData.CurrentTime -= Time.deltaTime;

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
        public Action TimeEnded;
    }
}