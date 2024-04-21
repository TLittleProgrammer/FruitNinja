using Runtime.Infrastructure.SlicableObjects;

namespace Runtime.Infrastructure.Slicer.SliceServices
{
    public sealed class BrickSliceService : ISliceService
    {

        public void Slice()
        {
            //_lastSlicedPosition = slicableObjectView.transform.position;
            //_trailMoveService.SetCannotMove();
            //_mouseManager.SetCannotMouseCheckPosition();
            //_showEffectsService.ShowSplash(_lastSlicedPosition, slicableObjectView.MainSprite.sprite);
        }

        public bool TrySlice(SlicableObjectView slicableObjectView)
        {
            return false;
        }
    }
}