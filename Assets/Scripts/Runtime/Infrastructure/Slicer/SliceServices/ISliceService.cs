using Runtime.Infrastructure.SlicableObjects;

namespace Runtime.Infrastructure.Slicer.SliceServices
{
    public interface ISliceService
    {
        bool TrySlice(SlicableObjectView slicableObjectView);
    }
}