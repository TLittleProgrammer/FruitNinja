using Runtime.Infrastructure.SlicableObjects;

namespace Runtime.Infrastructure.Slicer
{
    public interface ISlicer
    {
        bool TrySliceObject(SlicableObjectView slicableObjectView);
    }
}