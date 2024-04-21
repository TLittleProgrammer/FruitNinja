using System;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Combo
{
    public interface IComboService : ITickable
    {
        public event Action<int> ComboEnded; 
        bool IsActive { get; }
        void AddCombo();
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

        public bool IsActive => _comboIsActive;

        public void Tick()
        {
            _currentTime += Time.deltaTime;

            if (_currentTime >= _comboData.DelayComboDestroy)
            {
                if (_comboIsActive)
                {
                    if (_comboCounter != 1)
                    {
                        ComboEnded?.Invoke(_comboCounter);
                    }
                }
                
                _comboCounter = 0;
                _comboIsActive = false;
                _currentTime = 0f;
            }
        }

        public void AddCombo()
        {
            _comboCounter++;
            _comboIsActive = true;
            _currentTime = 0f;
        }
    }
}