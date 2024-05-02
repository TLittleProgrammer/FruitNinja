using System;
using UnityEngine;

namespace Runtime.Infrastructure.Timer
{
    public interface ITimeProvider
    {
        event Action<float> TimeScaleChanged;
        float DeltaTime { get; }

        void SetScale(float scale);
    }

    public sealed class TimeProvider : ITimeProvider
    {
        private float _timeScale = 1f;

        public event Action<float> TimeScaleChanged;
        
        public float DeltaTime => Time.deltaTime * _timeScale;
        public void SetScale(float scale)
        {
            _timeScale = scale;
            
            TimeScaleChanged?.Invoke(_timeScale);
        }
    }
}