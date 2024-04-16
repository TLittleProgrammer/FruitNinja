using Runtime.Infrastructure.SlicableObjects.Movement.Animation;
using Runtime.Infrastructure.SlicableObjects.Movement.MoveLogic;
using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects.Movement
{
    public class SlicableModel
    {
        private readonly float _velocityX;
        private readonly float _velocityY;
        private readonly float _angle;

        private IMovementObject _movementObject;
        private IModelAnimation _modelAnimation;

        public SlicableModel(Transform movementTransform, float velocityX, float velocityY, float angle, IModelAnimation modelAnimation)
        {
            _movementObject = new MovementObjectService(movementTransform, velocityX, velocityY, angle);
            _velocityX = velocityX;
            _velocityY = velocityY;
            _angle = angle;
            _modelAnimation = modelAnimation;
        }

        public Quaternion Rotation => Quaternion.Euler(0f, 0f, _modelAnimation.Rotation);
        public Vector2 Position => _movementObject.Position;

        public void Tick()
        {
            _movementObject.SimulateMovement();
            _modelAnimation.SimulateAnimation();
        }

        public SlicableModelParams GetParams()
        {
            return new(_velocityX, _angle, _modelAnimation);
        }
    }

    public record SlicableModelParams
    {
        public float VelocityX;
        public float VelocityY;
        public float Angle;
        public IModelAnimation ModelAnimation;

        public SlicableModelParams(float velocityX, float angle, IModelAnimation modelAnimation)
        {
            VelocityX = velocityX;
            Angle = angle;
            ModelAnimation = modelAnimation;
        }
    }
}