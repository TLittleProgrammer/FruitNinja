using Runtime.Infrastructure.SlicableObjects;
using UnityEngine;

namespace Runtime.Infrastructure.Slicer.SliceServices.Helpers
{
    public interface ICreateDummiesService
    {
        void AddDummies(SlicableObjectView slicableObjectView, Sprite sprite, Sprite slicableObjectSprite);
    }
}