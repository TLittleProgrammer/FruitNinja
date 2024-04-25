using Runtime.Infrastructure.Mouse;
using Runtime.Infrastructure.SlicableObjects;
using Runtime.Infrastructure.SlicableObjects.Movement;
using Runtime.Infrastructure.Slicer.SliceServices.HealthFlying;
using Runtime.Infrastructure.Slicer.SliceServices.Helpers;

namespace Runtime.Infrastructure.Slicer.SliceServices
{
    public sealed class HealthSliceService : ISliceService
    {
        private readonly SlicableMovementService _slicableMovementService;
        private readonly ICreateDummiesService _createDummiesService;
        private readonly IHealthFlyingService _healthFlyingService;
        private readonly MouseManager _manager;

        public HealthSliceService(
            SlicableMovementService slicableMovementService,
            ICreateDummiesService createDummiesService,
            IHealthFlyingService healthFlyingService,
            MouseManager manager)
        {
            _slicableMovementService = slicableMovementService;
            _createDummiesService = createDummiesService;
            _healthFlyingService = healthFlyingService;
            _manager = manager;
        }
        
        public bool TrySlice(SlicableObjectView slicableObjectView)
        {
            _createDummiesService.AddDummies(slicableObjectView);
            RemoveSlicableObjectFromMapping(slicableObjectView);
            
            _healthFlyingService.Fly(_manager.GetScreenPosition(slicableObjectView.transform.position));
            
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