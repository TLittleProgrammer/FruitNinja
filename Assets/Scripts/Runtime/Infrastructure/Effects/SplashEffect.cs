using System;
using Runtime.Infrastructure.StateMachine;
using Runtime.Infrastructure.StateMachine.States;
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
        private ITimeProvider _timeProvider;

        [Inject]
        private void Construct(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        private void Update()
        {
            _particleSystemMainModule.simulationSpeed = _timeProvider.TimeScale;
        }

        private void OnUpdatedState(IExitableState state)
        {
            if (state is PauseState)
            {
                _particleSystemMainModule.simulationSpeed = 0f;
            }
            else
            {
                _particleSystemMainModule.simulationSpeed = 1f;
            }
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

        public class Pool : MonoMemoryPool<SplashEffect>
        {
        }
    }
}