using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Runtime.Extensions;
using Runtime.Infrastructure.Game;
using Runtime.StaticData.UI;
using Runtime.UI.Screens;
using UnityEngine;

namespace Runtime.Infrastructure.Slicer.SliceServices.HealthFlying
{
    public sealed class HealthFlyingService : IHealthFlyingService
    {
        private readonly FlyingHealthView.Pool _healthViewPool;
        private readonly FlyingHealthViewStaticData _flyingHealthViewStaticData;
        private readonly GameParameters _gameParameters;

        private GameScreen _gameScreen;
        private List<FlyingHealth> _healthList;
        private int _previousHealth;

        public HealthFlyingService(
            FlyingHealthView.Pool healthViewPool,
            FlyingHealthViewStaticData flyingHealthViewStaticData,
            GameParameters gameParameters)
        {
            _healthViewPool = healthViewPool;
            _flyingHealthViewStaticData = flyingHealthViewStaticData;
            _gameParameters = gameParameters;
            _healthList = new();
            
            
            _previousHealth = gameParameters.MaxHealth;
            gameParameters.HealthChanged += OnHealthChanged;
        }

        public async UniTask AsyncInitialize(GameScreen payload)
        {
            _gameScreen = payload;

            await UniTask.CompletedTask;
        }

        public void Fly(Vector2 slicedPosition)
        {
            if (_gameParameters.CurrentHealthIsMax || _healthList.Count + _gameParameters.Health >= _gameParameters.MaxHealth)
            {
                return;
            }

            FlyingHealthView flyingHealthView = _healthViewPool.InactiveItems.GetInactiveObject();
            Vector2 targetPosition = _gameScreen.GetLastLostHealthPosition(_healthList.Count);

            flyingHealthView.RectTransform.position = slicedPosition;
            
            FlyingHealth flyingHealth = new(flyingHealthView, _flyingHealthViewStaticData);
            flyingHealth.FlyTo(targetPosition, () =>
            {
                _gameScreen.AddHealthWithoutAnimation();
                _gameParameters.ChangeHealth(1);

                flyingHealth.FlyingHealthView.transform.localScale = Vector3.zero;
                _healthList.RemoveAt(_healthList.Count - 1);
            });
            
            _healthList.Add(flyingHealth);
        }

        private void OnHealthChanged(int health)
        {
            if (_healthList.Count == 0)
                return;

            if (health + _healthList.Count == 0)
            {
                foreach (FlyingHealth flyingHealth in _healthList)
                {
                    flyingHealth.DestroyView();
                }

                return;
            }
            
            if (_previousHealth > health)
            {
                Vector2 targetPosition = _gameScreen.GetLastLostHealthPosition(0);

                for (int i = _healthList.Count - 1; i >= 0; i--)
                {
                    Vector2 previousTargetPosition = _healthList[i].TargetPosition;
                    _healthList[i].ChangeTargetPosition(targetPosition);

                    targetPosition = previousTargetPosition;
                }
            }
            else
            {
                _previousHealth = health;
            }
        }
    }
}