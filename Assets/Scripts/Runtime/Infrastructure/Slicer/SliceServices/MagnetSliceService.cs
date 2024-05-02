using System.Collections.Generic;
using DG.Tweening;
using Runtime.Infrastructure.SlicableObjects.MonoBehaviours;
using Runtime.Infrastructure.SlicableObjects.Movement;
using Runtime.Infrastructure.StateMachine;
using Runtime.Infrastructure.StateMachine.States;
using Runtime.Infrastructure.Timer;
using Runtime.StaticData.Boosts;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Slicer.SliceServices
{
    public sealed class MagnetSliceService : ISliceService, ITickable
    {
        private readonly SlicableMovementService _slicableMovementService;
        private readonly MagnetSettings _magnetSettings;
        private readonly IGameStateMachine _gameStateMachine;

        private List<SlicableObjectView> _magnetsViews = new();
        private List<Transform> _magnets = new();
        private Dictionary<Transform, Sequence> _sequenceMapping = new();
        private ITimeProvider _timeProvider;

        public MagnetSliceService(
            SlicableMovementService slicableMovementService,
            MagnetSettings magnetSettings,
            IGameStateMachine gameStateMachine,
            ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
            _slicableMovementService = slicableMovementService;
            _magnetSettings = magnetSettings;
            _gameStateMachine = gameStateMachine;

            timeProvider.TimeScaleChanged += OnTimeScaleChanged;
        }

        public void Tick()
        {
            if (_gameStateMachine.CurrentState is PauseState or LooseState)
                return;
            
            foreach (Transform magnet in _magnets)
            {
                foreach (SlicableModel slicableModel in _slicableMovementService.SlicableModels)
                {
                    if (!_magnetSettings.AvailableMagnetTypes.Contains(slicableModel.Type))
                    {
                        continue;
                    }
                    
                    float distance = Vector2.Distance(slicableModel.Position, magnet.position);

                    if (distance <= _magnetSettings.Distance)
                    {
                        float addForce = Mathf.Lerp(0f, _magnetSettings.Force, distance / _magnetSettings.Distance);

                        Vector2 offset = addForce * ((Vector2)magnet.position - slicableModel.Position).normalized;

                        slicableModel.AddMagnetOffset(offset);
                    }
                }
            }
        }

        private void OnTimeScaleChanged(float timeScale)
        {
            foreach (SlicableObjectView views in _magnetsViews)
            {
                var main = views.MagnetParticles.main;
                main.simulationSpeed = timeScale;
            }
        }

        public void ResetAll()
        {
            foreach ((Transform view, Sequence sequence) in _sequenceMapping)
            {
                sequence.Kill();
                view
                    .transform
                    .DOScale(0f, 0.25f)
                    .OnComplete(() =>
                    {
                        _sequenceMapping.Remove(view.transform);
                        _magnets.Remove(view.transform);
                        view.gameObject.SetActive(false);
                    });
            }
        }

        public bool TrySlice(SlicableObjectView slicableObjectView)
        {
            _slicableMovementService.RemoveFromMapping(slicableObjectView.transform);
            
            AddMagnet(slicableObjectView);

            return true;
        }

        private void AddMagnet(SlicableObjectView slicableObjectView)
        {
            if (!_magnets.Contains(slicableObjectView.transform))
            {
                Sequence sequence = DOTween.Sequence();
                slicableObjectView.MagnetParticles.Play();
                _magnetsViews.Add(slicableObjectView);
                ParticleSystem.ShapeModule shapeModule = slicableObjectView.MagnetParticles.shape;
                shapeModule.radius = _magnetSettings.Distance;
                
                _magnets.Add(slicableObjectView.transform);
                _sequenceMapping.Add(slicableObjectView.transform, sequence);
                
                sequence.Append(
                        slicableObjectView
                            .transform
                            .DOScale(slicableObjectView.transform.localScale + Vector3.one * 0.25f, 0.5f)
                            .SetLoops((int)_magnetSettings.Duration * 2, LoopType.Yoyo)
                );
                
                sequence.Append(
                    slicableObjectView
                        .transform
                        .DOScale(0f, 0.25f)
                        .OnComplete(() =>
                        {
                            _sequenceMapping.Remove(slicableObjectView.transform);
                            _magnets.Remove(slicableObjectView.transform);

                            _magnetsViews.Remove(slicableObjectView);
                            
                            slicableObjectView.MagnetParticles.Stop();
                            slicableObjectView.gameObject.SetActive(false);
                            sequence.Kill();
                        })
                    );
            }
        }

        public void StopSequences()
        {
            foreach (SlicableObjectView magnetsView in _magnetsViews)
            {
                var magnetParticlesMain = magnetsView.MagnetParticles.main;
                magnetParticlesMain.simulationSpeed = 0f;
            }
            foreach (Sequence sequence in _sequenceMapping.Values)
            {
                sequence.Pause();
            }
        }
        
        public void PlaySequences()
        {
            foreach (SlicableObjectView magnetsView in _magnetsViews)
            {
                var magnetParticlesMain = magnetsView.MagnetParticles.main;
                magnetParticlesMain.simulationSpeed = _timeProvider.TimeScale;
            }
            foreach (Sequence sequence in _sequenceMapping.Values)
            {
                sequence.Play();
            }
        }
    }
}