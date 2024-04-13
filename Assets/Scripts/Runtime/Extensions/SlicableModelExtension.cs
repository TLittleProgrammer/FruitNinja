using Runtime.Infrastructure.SlicableObjects.Movement;
using UnityEngine;

namespace Runtime.Extensions
{
    public static class SlicableModelExtension
    {
        public static SlicableModel CreateCopy(this SlicableModel slicableModel, Vector2 position)
        {
            SlicableModelParams modelParams = slicableModel.GetParams();
            
            return new(modelParams.SpeedX, modelParams.SpeedY, modelParams.Direction, position, modelParams.CurrentAngle);
        }
    }
}