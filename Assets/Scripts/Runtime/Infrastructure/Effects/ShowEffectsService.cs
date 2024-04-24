using System;
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
        void ShowSplash(Vector2 position, string spriteName);
        void ShowBlots(Vector2 position, string spriteName);
        void ShowScore(Vector2 position, int score);
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

        public void ShowSplash(Vector2 position, string spriteName)
        {
            _lastSlicedPosition = position;
            SplashEffect splashEffect;

            try
            {
                splashEffect = _splashEffectPool.InactiveItems.GetInactiveObject();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            Color color = _slicableVisualContainer.GetSplashColorBySpriteName(spriteName);
            splashEffect.PlayEffect(position, color);
        }

        public void ShowBlots(Vector2 position, string spriteName)
        {
            _lastSlicedPosition = position;
            Sprite blotSprite = _slicableVisualContainer.GetRandomBlot(spriteName);

            if (blotSprite is not null)
            {
                BlotEffect blotEffect = _blotEffectPool.InactiveItems.First(_ => !_.gameObject.activeInHierarchy);
                
                blotEffect.Animate(position, blotSprite, () =>
                {
                    blotEffect.enabled = false;
                });
            }
        }

        public void ShowScore(Vector2 slicableObjectViewPosition, int score)
        {
            _lastSlicedPosition = slicableObjectViewPosition;
            ScoreEffect scoreEffect = _scoreEffectPool.InactiveItems.GetInactiveObject();
            Vector2 screenPosition = _mouseManager.GetScreenPosition(slicableObjectViewPosition);

            scoreEffect.PlayEffect(screenPosition, score);
        }

        private void OnComboEnded(int comboCounter)
        {
            ComboView comboView = _comboViewPool.InactiveItems.GetInactiveObject();
            
            comboView.ShowCombo(comboCounter);
            _comboViewPositionCorrecter.CorrectPosition(comboView, _lastSlicedPosition);
        }
    }
}