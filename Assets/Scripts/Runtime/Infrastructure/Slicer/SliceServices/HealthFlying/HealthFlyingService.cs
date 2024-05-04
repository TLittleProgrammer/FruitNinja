using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Runtime.Extensions;
using Runtime.Infrastructure.Game;
using Runtime.Infrastructure.Timer;
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
        private readonly ITimeProvider _timeProvider;

        private GameScreen _gameScreen;
        private List<FlyingHealth> _healthList;
        private int _previousHealth;
        
        public int HealthCounter => _healthList.Count;

        public HealthFlyingService(
            FlyingHealthView.Pool healthViewPool,
            FlyingHealthViewStaticData flyingHealthViewStaticData,
            GameParameters gameParameters,
            ITimeProvider timeProvider)
        {
            _healthViewPool = healthViewPool;
            _flyingHealthViewStaticData = flyingHealthViewStaticData;
            _gameParameters = gameParameters;
            _timeProvider = timeProvider;
            _healthList = new();
            
            
            _previousHealth = gameParameters.MaxHealth;
            gameParameters.HealthChanged += OnHealthChanged;
            timeProvider.TimeScaleChanged += OnTimeScaleChanged;
        }

        public async UniTask AsyncInitialize(GameScreen payload)
        {
            _gameScreen = payload;

            await UniTask.CompletedTask;
        }

        public void Fly(Vector2 slicedPosition, Vector3 transformLocalScale)
        {
            if (_gameParameters.CurrentHealthIsMax || _healthList.Count + _gameParameters.Health >= _gameParameters.MaxHealth)
            {
                return;
            }
            
            FlyingHealthView flyingHealthView = _healthViewPool.InactiveItems.GetInactiveObject();
            flyingHealthView.RectTransform.position = slicedPosition;
            flyingHealthView.Image.SetNativeSize();
            var imageRectRect = flyingHealthView.ImageRect.rect;
            imageRectRect.size = transformLocalScale;

            var targetPosition = CalculateTargetPosition();
            //TODO этим должна заниматься фабрика
            var flyingHealth = CreateHealth(flyingHealthView, targetPosition);
            flyingHealth.MoveSequence.timeScale = _timeProvider.TimeScale;
            flyingHealth.ScaleSequence.timeScale = _timeProvider.TimeScale;
            
            
            
            _healthList.Add(flyingHealth);
        }

        private void OnTimeScaleChanged(float timeScale)
        {
            foreach (FlyingHealth flyingHealth in _healthList)
            {
                flyingHealth.MoveSequence.timeScale = timeScale;
                flyingHealth.ScaleSequence.timeScale = timeScale;
            }
        }

        private Vector2 CalculateTargetPosition()
        {
            Vector2 targetPosition = Vector2.zero;

            for (int i = _gameParameters.Health; i < _gameParameters.MaxHealth; i++)
            {
                Vector2 position = _gameScreen.HeartViews[i].RectTransform.position;

                if (_healthList.All(x => x.TargetPosition != position))
                {
                    targetPosition = position;
                    break;
                }
            }

            return targetPosition;
        }

        private FlyingHealth CreateHealth(FlyingHealthView flyingHealthView, Vector2 targetPosition)
        {
            FlyingHealth flyingHealth = new(flyingHealthView, _flyingHealthViewStaticData);
            flyingHealth.FlyTo(targetPosition, () =>
            {
                _gameScreen.AddHealthWithoutAnimation();
                _gameParameters.ChangeHealth(1);

                flyingHealth.FlyingHealthView.transform.localScale = Vector3.zero;
                _healthList.RemoveAt(_healthList.Count - 1);
            });
            return flyingHealth;
        }

        private void OnHealthChanged(int health)
        {
            if (_healthList.Count == 0)
            {
                _previousHealth = health;
                return;
            }

            if (_previousHealth == 0 && health == 0)
            {
                _healthList[0].DestroyView(() =>
                {
                    _healthList.RemoveAt(0);
                });
                return;
            }
            
            if (_previousHealth > health)
            {
                int index = _gameParameters.Health;
                Vector2 targetPosition = _gameScreen.HeartViews[index].RectTransform.position;

                for (int i = 0; i < _healthList.Count; i++)
                {
                    Vector2 previousTargetPosition = _healthList[i].TargetPosition;
                    _healthList[i].ChangeTargetPosition(targetPosition);

                    targetPosition = previousTargetPosition;
                }
            }
            
            _previousHealth = health;
        }
    }
}