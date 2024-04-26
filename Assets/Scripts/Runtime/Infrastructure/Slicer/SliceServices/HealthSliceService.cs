using Runtime.Infrastructure.Effects;
using Runtime.Infrastructure.Mouse;
using Runtime.Infrastructure.SlicableObjects;
using Runtime.Infrastructure.SlicableObjects.Movement;
using Runtime.Infrastructure.Slicer.SliceServices.HealthFlying;
using Runtime.Infrastructure.Slicer.SliceServices.Helpers;
using UnityEngine;

namespace Runtime.Infrastructure.Slicer.SliceServices
{
    public sealed class HealthSliceService : ISliceService
    {
        private readonly SlicableMovementService _slicableMovementService;
        private readonly ICreateDummiesService _createDummiesService;
        private readonly IHealthFlyingService _healthFlyingService;
        private readonly MouseManager _manager;
        private readonly IShowEffectsService _showEffectsService;

        public HealthSliceService(
            SlicableMovementService slicableMovementService,
            ICreateDummiesService createDummiesService,
            IHealthFlyingService healthFlyingService,
            MouseManager manager,
            IShowEffectsService showEffectsService)
        {
            _slicableMovementService = slicableMovementService;
            _createDummiesService = createDummiesService;
            _healthFlyingService = healthFlyingService;
            _manager = manager;
            _showEffectsService = showEffectsService;
        }
        
        public bool TrySlice(SlicableObjectView slicableObjectView)
        {
            _createDummiesService.AddDummies(slicableObjectView);
            RemoveSlicableObjectFromMapping(slicableObjectView);

            Vector2 screenPosition = _manager.GetScreenPosition(slicableObjectView.transform.position);
            
            _healthFlyingService.Fly(screenPosition);
            _showEffectsService.PlayHeartSplash(screenPosition);
            return true;
        }
        
        //TODO дубляж кода в SimpleSliceService. Вынести в абстрактный класс
        private void RemoveSlicableObjectFromMapping(SlicableObjectView slicableObjectView)
        {
            _slicableMovementService.RemoveFromMapping(slicableObjectView.transform);
            slicableObjectView.gameObject.SetActive(false);
        }
    }
}