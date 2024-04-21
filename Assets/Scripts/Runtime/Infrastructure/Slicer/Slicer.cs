using System.Collections.Generic;
using Runtime.Infrastructure.SlicableObjects;
using Runtime.Infrastructure.Slicer.SliceServices;

namespace Runtime.Infrastructure.Slicer
{
    public sealed class Slicer : ISlicer
    {
        private readonly Dictionary<SlicableObjectType, ISliceService> _sliceServices;

        public Slicer(Dictionary<SlicableObjectType, ISliceService> sliceServices)
        {
            _sliceServices = sliceServices;
        }

        public bool TrySliceObject(SlicableObjectView slicableObjectView)
        {
            if (_sliceServices.TryGetValue(slicableObjectView.SlicableObjectType, out ISliceService sliceService))
            {
                return sliceService.TrySlice(slicableObjectView);
            }

            return false;
        }
    }
}