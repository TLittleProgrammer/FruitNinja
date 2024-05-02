using System.Collections.Generic;
using System.Timers;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Runtime.Infrastructure.SlicableObjects;
using Runtime.Infrastructure.SlicableObjects.MonoBehaviours;
using Runtime.Infrastructure.SlicableObjects.Movement;
using Runtime.Infrastructure.StateMachine;
using Runtime.Infrastructure.StateMachine.States;
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

        private List<Transform> _magnets = new();
        private Dictionary<Transform, Sequence> _sequenceMapping = new();

        public MagnetSliceService(
            SlicableMovementService slicableMovementService,
            MagnetSettings magnetSettings,
            IGameStateMachine gameStateMachine)
        {
            _slicableMovementService = slicableMovementService;
            _magnetSettings = magnetSettings;
            _gameStateMachine = gameStateMachine;
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

        public bool TrySlice(SlicableObjectView slicableObjectView)
        {
            _slicableMovementService.RemoveFromMapping(slicableObjectView.transform);
            
            AddMagnet(slicableObjectView);

            return true;
        }

        private async void AddMagnet(SlicableObjectView slicableObjectView)
        {
            if (!_magnets.Contains(slicableObjectView.transform))
            {
                Sequence sequence = DOTween.Sequence();
                
                _magnets.Add(slicableObjectView.transform);
                _sequenceMapping.Add(slicableObjectView.transform, sequence);
                
                await sequence.Append(
                        slicableObjectView
                            .transform
                            .DOScale(slicableObjectView.transform.localScale + Vector3.one * 0.25f, 0.5f)
                            .SetLoops((int)_magnetSettings.Duration * 2, LoopType.Yoyo)
                );
                
                await sequence.Append(
                    slicableObjectView
                        .transform
                        .DOScale(0f, 0.25f)
                        .OnComplete(() =>
                        {
                            _sequenceMapping.Remove(slicableObjectView.transform);
                            _magnets.Remove(slicableObjectView.transform);
                            slicableObjectView.gameObject.SetActive(false);
                            sequence.Kill();
                        })
                    );
            }
        }

        public void StopSequences()
        {
            foreach (Sequence sequence in _sequenceMapping.Values)
            {
                sequence.Pause();
            }
        }
        
        public void PlaySequences()
        {
            foreach (Sequence sequence in _sequenceMapping.Values)
            {
                sequence.Play();
            }
        }
    }
}