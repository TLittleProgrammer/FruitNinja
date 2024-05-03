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
        void PlayHeartSplash(Vector2 position);
        void PlayBombEffect(Vector2 position);
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
        private readonly BombEffect.Pool _bombEffectPool;

        private Vector2 _lastSlicedPosition;
        private int _lastScore;
        private HeartSplash.Pool _heartSplashPool;

        public ShowEffectsService(
                BombEffect.Pool bombEffectPool,
                BlotEffect.Pool blotEffectPool,
                SplashEffect.Pool splashEffectPool,
                ScoreEffect.Pool scoreEffectPool,
                ComboView.Pool comboViewPool,
                HeartSplash.Pool heartSplashPool,
                SlicableVisualContainer slicableVisualContainer,
                MouseManager mouseManager,
                IComboService comboService,
                IComboViewPositionCorrecter comboViewPositionCorrecter
            )
        {
            _bombEffectPool = bombEffectPool;
            _heartSplashPool = heartSplashPool;
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
            SplashEffect splashEffect = _splashEffectPool.InactiveItems.GetInactiveObject();


            Color color = _slicableVisualContainer.GetSplashColorBySpriteName(spriteName);
            splashEffect.PlayEffect(position, color);
        }

        public void ShowBlots(Vector2 position, string spriteName)
        {
            _lastSlicedPosition = position;
            Sprite blotSprite = _slicableVisualContainer.GetRandomBlot(spriteName);

            if (blotSprite is not null)
            {
                BlotEffect blotEffect = _blotEffectPool.InactiveItems.GetInactiveObject();
                
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

        public void PlayHeartSplash(Vector2 position)
        {
            HeartSplash heartSplash = _heartSplashPool.InactiveItems.GetInactiveObject();
            
            heartSplash.Play(position);
        }

        public void PlayBombEffect(Vector2 position)
        {
            BombEffect bombEffect = _bombEffectPool.InactiveItems.GetInactiveObject();
            bombEffect.SetPosition(position);
            bombEffect.Play();
        }

        private void OnComboEnded(int comboCounter)
        {
            ComboView comboView = _comboViewPool.InactiveItems.GetInactiveObject();
            
            comboView.ShowCombo(comboCounter);
            _comboViewPositionCorrecter.CorrectPosition(comboView, _lastSlicedPosition);
        }
    }
}