using Runtime.Infrastructure.SlicableObjects;

namespace Runtime.Infrastructure.Slicer.SliceServices
{
    public sealed class HealthSlicer : ISliceService
    {
        public bool TrySlice(SlicableObjectView slicableObjectView)
        {
            return true;
        }
    }
}