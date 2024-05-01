using System.Collections.Generic;
using System.Timers;
using Cysharp.Threading.Tasks;
using Runtime.Infrastructure.SlicableObjects.MonoBehaviours;
using Runtime.Infrastructure.SlicableObjects.Movement;
using Runtime.StaticData.Boosts;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Slicer.SliceServices
{
    public sealed class MagnetSliceService : ISliceService, ITickable
    {
        private readonly SlicableMovementService _slicableMovementService;
        private readonly MagnetSettings _magnetSettings;

        private List<Transform> _magnets = new();

        public MagnetSliceService(
            SlicableMovementService slicableMovementService,
            MagnetSettings magnetSettings)
        {
            _slicableMovementService = slicableMovementService;
            _magnetSettings = magnetSettings;
        }
        
        public void Tick()
        {
            foreach (Transform magnet in _magnets)
            {
                foreach (SlicableModel slicableModel in _slicableMovementService.SlicableModels)
                {
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
                _magnets.Add(slicableObjectView.transform);

                await UniTask.Delay((int)(_magnetSettings.Duration * 1000));
                
                _magnets.Remove(slicableObjectView.transform);
                slicableObjectView.gameObject.SetActive(false);
            }
        }
    }
}