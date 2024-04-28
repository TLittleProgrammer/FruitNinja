using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Effects
{
    [RequireComponent(typeof(ParticleSystem))]
    public sealed class BombEffect : MonoBehaviour
    {
        private ParticleSystem _particleSystem;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public void Play()
        {
            gameObject.SetActive(true);
            _particleSystem.Play();
        }
        
        public class Pool : MonoMemoryPool<BombEffect>
        {
        }
    }
}