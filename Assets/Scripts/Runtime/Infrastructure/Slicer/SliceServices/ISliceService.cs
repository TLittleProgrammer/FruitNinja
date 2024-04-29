using Runtime.Infrastructure.SlicableObjects;
using Runtime.Infrastructure.SlicableObjects.MonoBehaviours;

namespace Runtime.Infrastructure.Slicer.SliceServices
{
    public interface ISliceService
    {
        bool TrySlice(SlicableObjectView slicableObjectView);
    }
}