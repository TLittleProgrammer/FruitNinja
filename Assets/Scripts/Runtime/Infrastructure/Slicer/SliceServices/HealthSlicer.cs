using Runtime.Infrastructure.SlicableObjects;

namespace Runtime.Infrastructure.Slicer.SliceServices
{
    public sealed class HealthSlicer : ISlicer
    {
        public bool TrySliceObject(SlicableObjectView slicableObjectView)
        {
            return true;
        }
    }
}