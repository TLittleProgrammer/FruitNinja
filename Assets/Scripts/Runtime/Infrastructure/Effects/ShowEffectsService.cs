using System.Linq;
using Runtime.Extensions;
using Runtime.Infrastructure.Mouse;
using Runtime.Infrastructure.SlicableObjects;
using UnityEngine;

namespace Runtime.Infrastructure.Effects
{
    public interface IShowEffectsService
    {
        void ShowEffects(Vector2 position, Sprite sprite, int score);
    }

    public sealed class ShowEffectsService : IShowEffectsService
    {
        private readonly BlotEffect.Pool _blotEffectPool;
        private readonly SplashEffect.Pool _splashEffectPool;
        private readonly ScoreEffect.Pool _scoreEffectPool;
        private readonly SlicableVisualContainer _slicableVisualContainer;
        private readonly MouseManager _mouseManager;

        public ShowEffectsService(
                BlotEffect.Pool blotEffectPool,
                SplashEffect.Pool splashEffectPool,
                ScoreEffect.Pool scoreEffectPool,
                SlicableVisualContainer slicableVisualContainer,
                MouseManager mouseManager
            )
        {
            _blotEffectPool = blotEffectPool;
            _splashEffectPool = splashEffectPool;
            _scoreEffectPool = scoreEffectPool;
            _slicableVisualContainer = slicableVisualContainer;
            _mouseManager = mouseManager;
        }
        
        public void ShowEffects(Vector2 position, Sprite sprite, int score)
        {
            AddBlotEffect(position, sprite);
            AddSplashEffect(position, sprite.name);
            AddScoreEffect(position, score);
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
        
        private void AddScoreEffect(Vector3 slicableObjectViewPosition, int score)
        {
            ScoreEffect scoreEffect = _scoreEffectPool.InactiveItems.GetInactiveObject();
            Vector2 screenPosition = _mouseManager.GetScreenPosition(slicableObjectViewPosition);

            scoreEffect.PlayEffect(screenPosition, score);
        }
    }
}