using Runtime.Infrastructure.SlicableObjects;
using Runtime.Infrastructure.SlicableObjects.MonoBehaviours;

namespace Runtime.Infrastructure.Slicer.SliceServices.Helpers
{
    public interface ICreateDummiesService
    {
        void AddDummies(SlicableObjectView slicableObjectView);
        void AddDummies(SlicableObjectView slicableObjectView, string firstSpriteName, string secondSpriteName);
    }
}