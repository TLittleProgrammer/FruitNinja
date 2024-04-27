using Runtime.Infrastructure.SlicableObjects;

namespace Runtime.Infrastructure.Slicer.SliceServices.Helpers
{
    public interface ICreateDummiesService
    {
        void AddDummies(SlicableObjectView slicableObjectView);
        void AddDummies(SlicableObjectView slicableObjectView, string firstSpriteName, string secondSpriteName);
    }
}