using Runtime.Infrastructure.Timer;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Effects
{
    [RequireComponent(typeof(ParticleSystem))]
    public sealed class SplashEffect : MonoBehaviour
    {
        private ParticleSystem _particleSystem;
        private ParticleSystem.MainModule _particleSystemMainModule;

        [Inject]
        private void Construct(ITimeProvider timeProvider)
        {
            timeProvider.TimeScaleChanged += OnTimeScaleChanged;
        }

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _particleSystemMainModule = _particleSystem.main;
        }

        private void OnTimeScaleChanged(float timeScale)
        {
            _particleSystemMainModule.simulationSpeed = timeScale;
        }

        public void PlayEffect(Vector3 position, Color color)
        {
            ChangeParticleColor(color);

            transform.position = position;
            gameObject.SetActive(true);
            
            _particleSystem.Play();
        }

        private void ChangeParticleColor(Color color)
        {
            _particleSystemMainModule.startColor = color;
        }
        
        public class Pool : MonoMemoryPool<Vector3, SplashEffect>
        {
        }
    }
}