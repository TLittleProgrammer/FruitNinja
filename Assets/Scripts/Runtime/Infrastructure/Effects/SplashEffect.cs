using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Effects
{
    [RequireComponent(typeof(ParticleSystem))]
    public sealed class SplashEffect : MonoBehaviour
    {
        private ParticleSystem _particleSystem;
        
        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        public void PlayEffect(Vector3 position, Color color)
        {
            ParticleSystem.MainModule particleSystemMain = _particleSystem.main;
            particleSystemMain.startColor = color;
            
            transform.position = position;
            
            gameObject.SetActive(true);
            _particleSystem.Play();
        }

        private void Reset(Vector3 startPosition)
        {
            transform.position = startPosition;
        }
        
        public class Pool : MonoMemoryPool<Vector3, SplashEffect>
        {
            protected override void Reinitialize(Vector3 position, SplashEffect splash)
            {
                splash.Reset(position);
            }
        }
    }
}