﻿using UnityEngine;
using Zenject;
using Vector3 = UnityEngine.Vector3;

namespace Runtime.Infrastructure.SlicableObjects.MonoBehaviours
{
    public class SlicableObjectView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _mainSprite;
        [SerializeField] private SpriteRenderer _shadowSprite;
        [SerializeField] private Collider2D _collider2D;
        [SerializeField] private ParticleSystem _mimikParticles;
        [SerializeField] private ParticleSystem _magnetParticles;

        private bool _isMimik = false;

        public SlicableObjectType SlicableObjectType;

        public SpriteRenderer MainSprite   => _mainSprite;
        public SpriteRenderer ShadowSprite => _shadowSprite;
        public Collider2D Collider2D => _collider2D;
        public ParticleSystem MimikParticles => _mimikParticles;
        public ParticleSystem MagnetParticles => _magnetParticles;
        public bool IsDamagable { get; set; } = true;

        private void OnDisable()
        {
            IsDamagable = true;
        }

        public bool IsMimik
        {
            get => _isMimik;
            set
            {
                _isMimik = value;

                if (_isMimik)
                {
                    _mimikParticles.gameObject.SetActive(true);
                    _mimikParticles.Play();
                }
                else
                {
                    _mimikParticles.gameObject.SetActive(false);
                    _mimikParticles.Stop();
                }
            }
        }

        public class Pool : MonoMemoryPool<Vector3, SlicableObjectView>
        {
        }
    }
}