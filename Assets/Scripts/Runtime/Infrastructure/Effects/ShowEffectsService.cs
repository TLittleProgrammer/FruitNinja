using System.Linq;
using Runtime.Extensions;
using Runtime.Infrastructure.Combo;
using Runtime.Infrastructure.Mouse;
using Runtime.Infrastructure.SlicableObjects;
using UnityEngine;

namespace Runtime.Infrastructure.Effects
{
    public interface IShowEffectsService
    {
        void ShowEffects(Vector2 position, Sprite sprite);
        void ShowScoreEffect(Vector2 position, int score);
    }

    public sealed class ShowEffectsService : IShowEffectsService
    {
        private readonly BlotEffect.Pool _blotEffectPool;
        private readonly SplashEffect.Pool _splashEffectPool;
        private readonly ScoreEffect.Pool _scoreEffectPool;
        private readonly ComboView.Pool _comboViewPool;
        private readonly SlicableVisualContainer _slicableVisualContainer;
        private readonly MouseManager _mouseManager;
        private readonly IComboViewPositionCorrecter _comboViewPositionCorrecter;

        private Vector2 _lastSlicedPosition;
        private int _lastScore;

        public ShowEffectsService(
                BlotEffect.Pool blotEffectPool,
                SplashEffect.Pool splashEffectPool,
                ScoreEffect.Pool scoreEffectPool,
                ComboView.Pool comboViewPool,
                SlicableVisualContainer slicableVisualContainer,
                MouseManager mouseManager,
                IComboService comboService,
                IComboViewPositionCorrecter comboViewPositionCorrecter
            )
        {
            _blotEffectPool = blotEffectPool;
            _splashEffectPool = splashEffectPool;
            _scoreEffectPool = scoreEffectPool;
            _comboViewPool = comboViewPool;
            _slicableVisualContainer = slicableVisualContainer;
            _mouseManager = mouseManager;
            _comboViewPositionCorrecter = comboViewPositionCorrecter;

            comboService.ComboEnded += OnComboEnded;
        }

        public void ShowEffects(Vector2 position, Sprite sprite)
        {
            _lastSlicedPosition = position;
            AddBlotEffect(position, sprite);
            AddSplashEffect(position, sprite.name);
        }

        public void ShowScoreEffect(Vector2 slicableObjectViewPosition, int score)
        {
            ScoreEffect scoreEffect = _scoreEffectPool.InactiveItems.GetInactiveObject();
            Vector2 screenPosition = _mouseManager.GetScreenPosition(slicableObjectViewPosition);

            scoreEffect.PlayEffect(screenPosition, score);
        }

        private void AddBlotEffect(Vector2 targetPosition, Sprite sprite)
        {
            Sprite blotSprite = _slicableVisualContainer.GetRandomBlot(sprite.name);

            if (blotSprite is not null)
            {
                BlotEffect blotEffect = _blotEffectPool.InactiveItems.First(_ => !_.gameObject.activeInHierarchy);
                
                blotEffect.Animate(targetPosition, blotSprite, () =>
                {
                    blotEffect.enabled = false;
                });
            }
        }

        private void AddSplashEffect(Vector3 transformPosition, string spriteName)
        {
            SplashEffect splashEffect = _splashEffectPool.InactiveItems.GetInactiveObject();

            Color color = _slicableVisualContainer.GetSplashColorBySpriteName(spriteName);
            splashEffect.PlayEffect(transformPosition, color);
        }

        private void OnComboEnded(int comboCounter)
        {
            ComboView comboView = _comboViewPool.InactiveItems.GetInactiveObject();

            _comboViewPositionCorrecter.CorrectPosition(comboView);
            
            Vector2 targetPosition = _mouseManager.GetScreenPosition(_lastSlicedPosition);
            comboView.ShowCombo(targetPosition, comboCounter);
        }
    }
}