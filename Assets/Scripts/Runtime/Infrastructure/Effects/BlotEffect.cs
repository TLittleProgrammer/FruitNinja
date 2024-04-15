using System;
using DG.Tweening;
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

        [Inject]
        private void Construct(BlotEffectSettings blotEffectSettings)
        {
            _blotEffectSettings = blotEffectSettings;
        }
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        public void Animate(Vector2 position, Sprite sprite, Action animationEnded = null)
        {
            gameObject.SetActive(true);
            
            transform.position   = new Vector3(position.x, position.y, 0f);
            transform.localScale = Vector3.one * GetRandomValue(_blotEffectSettings.MinScale, _blotEffectSettings.MaxScale);
            transform.rotation   = Quaternion.Euler(0f, 0f, Random.Range(0, 360));
            
            _spriteRenderer.sprite = sprite;

            _spriteRenderer.color = Color.white;
            _spriteRenderer
                .DOColor(Color.clear, _blotEffectSettings.Duration)
                .SetDelay(GetRandomValue(_blotEffectSettings.MinDelay, _blotEffectSettings.MaxDelay))
                .OnComplete(() =>
                {
                    animationEnded?.Invoke();
                });
        }
        
        private void Reset(Vector3 startPosition)
        {
            transform.position = startPosition;
        }

        private float GetRandomValue(float leftValue, float rightValue)
        {
            return Random.Range(leftValue, rightValue);
        }

        public class Pool : MonoMemoryPool<Vector3, BlotEffect>
        {
            protected override void Reinitialize(Vector3 position, BlotEffect blotEffect)
            {
                blotEffect.Reset(position);
            }
        }
    }
}