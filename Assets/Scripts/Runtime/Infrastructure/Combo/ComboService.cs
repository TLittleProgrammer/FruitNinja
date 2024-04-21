using System;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Combo
{
    public interface IComboService : ITickable
    {
        public event Action<int> ComboEnded; 
        bool IsActive { get; }
        void AddCombo(Transform slicableObjectTransform);
    }
    
    public sealed class ComboService : IComboService
    {
        public event Action<int> ComboEnded;
        
        private readonly ComboData _comboData;

        private float _currentTime;
        private int _comboCounter;
        private bool _comboIsActive;

        public ComboService(ComboData comboData)
        {
            _comboData = comboData;
            _currentTime = 0f;
            _comboCounter = 0;
            _comboIsActive = false;
        }

        public bool IsActive { get; }

        public void Tick()
        {
            _currentTime += Time.deltaTime;

            if (_currentTime >= _comboData.DelayComboDestroy)
            {
                _comboCounter = 1;
                _comboIsActive = false;
                _currentTime = 0f;

                ComboEnded?.Invoke(_comboCounter);
            }
        }

        public void AddCombo(Transform slicableObjectTransform)
        {
            _comboCounter++;
            _comboIsActive = true;
            _currentTime = 0f;
        }
    }
}