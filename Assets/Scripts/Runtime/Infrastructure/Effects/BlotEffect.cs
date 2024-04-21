using System;
using DG.Tweening;
using Runtime.Infrastructure.SlicableObjects;
using Runtime.StaticData.Animations;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Runtime.Infrastructure.Effects
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class BlotEffect : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private BlotEffectSettings _blotEffectSettings;
        private SliceableObjectSpriteRendererOrderService _orderService;

        [Inject]
        private void Construct(BlotEffectSettings blotEffectSettings, SliceableObjectSpriteRendererOrderService orderService)
        {
            _orderService = orderService;
            _blotEffectSettings = blotEffectSettings;
        }
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        public void Animate(Vector2 position, Sprite sprite, Action animationEnded = null)
        {
            SetPosition(position);
            SetNewBlotSprite(sprite);
            
            _orderService.UpdateOrderInLayer(_spriteRenderer);
            gameObject.SetActive(true);

            GoAnimate(() =>
            {
                gameObject.SetActive(false);
                animationEnded?.Invoke();
            });
        }

        private void GoAnimate(Action animationEnded)
        {
            _spriteRenderer
                .DOColor(Color.clear, _blotEffectSettings.Duration)
                .SetDelay(GetRandomValue(_blotEffectSettings.MinDelay, _blotEffectSettings.MaxDelay))
                .OnComplete(() =>
                {
                    animationEnded?.Invoke();
                });
        }

        private void SetNewBlotSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
            _spriteRenderer.color = Color.white;
        }

        private void SetPosition(Vector2 position)
        {
            transform.position = new Vector3(position.x, position.y, 0f);
            transform.localScale = Vector3.one * GetRandomValue(_blotEffectSettings.MinScale, _blotEffectSettings.MaxScale);
            transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360));
        }

        private float GetRandomValue(float leftValue, float rightValue)
        {
            return Random.Range(leftValue, rightValue);
        }

        public class Pool : MonoMemoryPool<Vector3, BlotEffect>
        {
        }
    }
}