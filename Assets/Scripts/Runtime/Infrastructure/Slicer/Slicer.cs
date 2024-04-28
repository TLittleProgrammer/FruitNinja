using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Runtime.Infrastructure.SlicableObjects;
using Runtime.Infrastructure.Slicer.SliceServices;

namespace Runtime.Infrastructure.Slicer
{
    public sealed class Slicer : ISlicer
    {
        private Dictionary<SlicableObjectType, ISliceService> _sliceServices;

        public Slicer()
        {
            
        }
        
        public Slicer(Dictionary<SlicableObjectType, ISliceService> sliceServices)
        {
            _sliceServices = sliceServices;
        }

        public async UniTask AsyncInitialize(Dictionary<SlicableObjectType, ISliceService> payload)
        {
            _sliceServices = payload;

            await UniTask.CompletedTask;
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