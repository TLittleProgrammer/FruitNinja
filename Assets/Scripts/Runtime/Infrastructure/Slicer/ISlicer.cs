using System.Collections.Generic;
using Runtime.Infrastructure.SlicableObjects;
using Runtime.Infrastructure.SlicableObjects.MonoBehaviours;
using Runtime.Infrastructure.Slicer.SliceServices;

namespace Runtime.Infrastructure.Slicer
{
    public interface ISlicer : IAsyncInitializable<Dictionary<SlicableObjectType, ISliceService>>
    {
        bool TrySliceObject(SlicableObjectView slicableObjectView);
    }
}