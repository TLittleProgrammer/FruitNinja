using Runtime.Infrastructure.SlicableObjects;
using Runtime.Infrastructure.SlicableObjects.Movement;

namespace Runtime.Infrastructure.Slicer.SliceServices
{
    public abstract class SliceService : ISliceService
    {
        private readonly SlicableMovementService _slicableMovementService;

        public SliceService(SlicableMovementService slicableMovementService)
        {
            _slicableMovementService = slicableMovementService;
        }
        
        public abstract bool TrySlice(SlicableObjectView slicableObjectView);
        
        protected void RemoveSlicableObjectFromMapping(SlicableObjectView slicableObjectView)
        {
            _slicableMovementService.RemoveFromMapping(slicableObjectView.transform);
            slicableObjectView.gameObject.SetActive(false);
        }
    }
}