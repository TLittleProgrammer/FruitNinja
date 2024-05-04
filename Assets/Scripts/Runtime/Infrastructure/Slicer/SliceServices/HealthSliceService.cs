using Runtime.Infrastructure.Effects;
using Runtime.Infrastructure.Mouse;
using Runtime.Infrastructure.SlicableObjects;
using Runtime.Infrastructure.SlicableObjects.MonoBehaviours;
using Runtime.Infrastructure.SlicableObjects.Movement;
using Runtime.Infrastructure.Slicer.SliceServices.HealthFlying;
using Runtime.Infrastructure.Slicer.SliceServices.Helpers;
using UnityEngine;

namespace Runtime.Infrastructure.Slicer.SliceServices
{
    public sealed class HealthSliceService : SliceService
    {
        private readonly ICreateDummiesService _createDummiesService;
        private readonly IHealthFlyingService _healthFlyingService;
        private readonly MouseManager _manager;
        private readonly IShowEffectsService _showEffectsService;

        public HealthSliceService(
            SlicableMovementService slicableMovementService,
            ICreateDummiesService createDummiesService,
            IHealthFlyingService healthFlyingService,
            MouseManager manager,
            IShowEffectsService showEffectsService) : base(slicableMovementService)
        {
            _createDummiesService = createDummiesService;
            _healthFlyingService = healthFlyingService;
            _manager = manager;
            _showEffectsService = showEffectsService;
        }
        
        public override bool TrySlice(SlicableObjectView slicableObjectView)
        {
            _createDummiesService.AddDummies(slicableObjectView);
            RemoveSlicableObjectFromMapping(slicableObjectView);

            Vector2 screenPosition = _manager.GetScreenPosition(slicableObjectView.transform.position);
            
            _healthFlyingService.Fly(screenPosition, slicableObjectView.transform.localScale);
            _showEffectsService.PlayHeartSplash(screenPosition, SlicableObjectType.Health);
            return true;
        }
    }
}