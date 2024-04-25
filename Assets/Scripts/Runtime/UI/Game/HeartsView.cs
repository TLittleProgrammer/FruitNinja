using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Runtime.Infrastructure;
using Runtime.Infrastructure.Factories;
using Runtime.Infrastructure.Game;
using UnityEngine;
using Zenject;

namespace Runtime.UI.Game
{
    public class HeartsView : MonoBehaviour, IAsyncInitializable<IUIFactory>
    {
        [SerializeField] private List<HeartView> _heartViews;

        private const string PathToHeartView = "Prefabs/UI/HealthView_01";
        private int _initialHealthCount;
        private int _lastHealth;
        private bool _canAnimate;
        private bool _addHealthWithoutAnimation = false;
        private Queue<int> _healthChangeQueue;

        [Inject]
        private void Construct(GameParameters gameParameters)
        {
            gameParameters.HealthChanged += OnChangedHealth;
            _initialHealthCount = gameParameters.Health;
            _lastHealth = _initialHealthCount;
            _canAnimate = true;
            _healthChangeQueue = new();
        }
        
        public async UniTask AsyncInitialize(IUIFactory uiFactory)
        {
            int heartsCounter = _initialHealthCount;

            for (int i = 0; i < heartsCounter; i++)
            {
                HeartView heartView = await uiFactory.LoadUIObjectByPath<HeartView>(PathToHeartView, transform);
                
                CorrectView(heartView);

                _heartViews.Add(heartView);
            }
        }

        public Vector2 GetLastLostHealthPosition(int index)
        {
            return _heartViews[^(_lastHealth + 1 + index)].RectTransform.position;
        }

        public void AddHealthWithoutAnimation()
        {
            _addHealthWithoutAnimation = true;
        }

        private void OnChangedHealth(int health)
        {
            _healthChangeQueue.Enqueue(health);
            
            GoAnimate();
        }

        private async void GoAnimate()
        {
            if (_canAnimate is false)
                return;

            _canAnimate = false;

            while (_healthChangeQueue.Count != 0)
            {
                int targetHealth = _healthChangeQueue.Dequeue();
                int offset = _lastHealth > targetHealth ? -1 : 1;
            
                await ChangeCurrentHealthToTargetHealth(targetHealth, offset);
            }

            _canAnimate = true;
        }

        private async UniTask ChangeCurrentHealthToTargetHealth(int targetHealth, int offset)
        {
            while (_lastHealth != targetHealth)
            {
                if (offset == -1)
                {
                    await _heartViews[^_lastHealth].AnimateGetDamage();
                }
                else
                {
                    if (_addHealthWithoutAnimation)
                    {
                        _addHealthWithoutAnimation = false;
                        _heartViews[^(_lastHealth + 1)].HeartTransform.localScale = Vector3.one;
                        _lastHealth++;
                        continue;
                    }

                    await _heartViews[^(_lastHealth + 1)].AnimateGetHealth();
                }

                _lastHealth += offset;
            }
        }

        private void CorrectView(HeartView heartView)
        {
            heartView.transform.localScale = Vector3.one;
        }
    }
}