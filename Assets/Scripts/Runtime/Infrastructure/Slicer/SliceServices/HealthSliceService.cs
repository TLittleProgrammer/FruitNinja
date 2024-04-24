using Runtime.Infrastructure.SlicableObjects;
using Runtime.Infrastructure.SlicableObjects.Movement;
using Runtime.Infrastructure.Slicer.SliceServices.Helpers;

namespace Runtime.Infrastructure.Slicer.SliceServices
{
    public sealed class HealthSliceService : ISliceService
    {
        private readonly SlicableMovementService _slicableMovementService;
        private readonly ICreateDummiesService _createDummiesService;

        public HealthSliceService(SlicableMovementService slicableMovementService, ICreateDummiesService createDummiesService)
        {
            _slicableMovementService = slicableMovementService;
            _createDummiesService = createDummiesService;
        }
        
        public bool TrySlice(SlicableObjectView slicableObjectView)
        {
            _createDummiesService.AddDummies(slicableObjectView);
            RemoveSlicableObjectFromMapping(slicableObjectView);
            
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