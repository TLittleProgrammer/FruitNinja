using Runtime.Infrastructure.SlicableObjects.Movement;
using Runtime.Infrastructure.SlicableObjects.Movement.Animation;
using Runtime.Infrastructure.Timer;
using UnityEngine;

namespace Runtime.Extensions
{
    public static class SlicableModelExtension
    {
        public static SlicableModel CreateCopy(this SlicableModel slicableModel, Transform targetTransform, Transform shadowTransform, ITimeProvider timeProvider)
        {
            SlicableModelParams modelParams = slicableModel.GetParams();
            IModelAnimation modelAnimation  = GetModelAnimation(modelParams.ModelAnimation, targetTransform, shadowTransform, 0f, timeProvider);
            
            return new(modelParams.Type, targetTransform, modelParams.VelocityX, modelParams.VelocityY, modelParams.Angle, modelAnimation, timeProvider);
        }

        private static IModelAnimation GetModelAnimation(IModelAnimation modelAnimation, Transform targetTransform, Transform shadowTransform, float angle, ITimeProvider timeProvider)
        {
            if (modelAnimation is SimpleRotateAnimation)
            {
                return new SimpleRotateAnimation(targetTransform, angle, timeProvider);
            }
            
            if (modelAnimation is ScaleAnimation)
            {
                return new ScaleAnimation(targetTransform, shadowTransform, timeProvider);
            }

            return new MixedAnimation(targetTransform, shadowTransform, angle, timeProvider);
        }
    }
}