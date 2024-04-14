using Runtime.Infrastructure.SlicableObjects.Movement;
using Runtime.Infrastructure.SlicableObjects.Movement.Animation;
using UnityEngine;

namespace Runtime.Extensions
{
    public static class SlicableModelExtension
    {
        public static SlicableModel CreateCopy(this SlicableModel slicableModel, Transform targetTransform, Transform shadowTransform)
        {
            SlicableModelParams modelParams = slicableModel.GetParams();

            IModelAnimation modelAnimation = GetModelAnimation(modelParams.ModelAnimation, targetTransform, shadowTransform, 0f);
            
            return new(targetTransform, modelParams.VelocityX, modelParams.VelocityY, modelParams.Angle, modelAnimation);
        }

        private static IModelAnimation GetModelAnimation(IModelAnimation modelAnimation, Transform targetTransform, Transform shadowTransform, float angle)
        {
            if (modelAnimation is SimpleRotateAnimation)
            {
                return new SimpleRotateAnimation(targetTransform, angle);
            }
            
            if (modelAnimation is ScaleAnimation)
            {
                return new ScaleAnimation(targetTransform, shadowTransform);
            }

            return new MixedAnimation(targetTransform, shadowTransform, angle);
        }
    }
}