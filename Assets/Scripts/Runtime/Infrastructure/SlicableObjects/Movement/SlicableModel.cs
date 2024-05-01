using Runtime.Infrastructure.SlicableObjects.Movement.Animation;
using Runtime.Infrastructure.SlicableObjects.Movement.MoveLogic;
using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects.Movement
{
    public class SlicableModel
    {
        public readonly SlicableObjectType Type;
        private float _velocityX;
        private float _velocityY;
        private float _angle;

        private IMovementObjectService _movementObjectService;
        private IModelAnimation _modelAnimation;

        public SlicableModel(SlicableObjectType type,Transform movementTransform, float velocityX, float velocityY, float angle, IModelAnimation modelAnimation)
        {
            _movementObjectService = new MovementObjectService(movementTransform, velocityX, velocityY, angle);
            Type = type;
            _velocityX = velocityX;
            _velocityY = velocityY;
            _angle = angle;
            _modelAnimation = modelAnimation;
        }

        public Quaternion Rotation => Quaternion.Euler(0f, 0f, _modelAnimation.Rotation);
        public Vector2 Position => _movementObjectService.Position;

        public void Tick()
        {
            _movementObjectService.SimulateMovement();
            _modelAnimation.SimulateAnimation();
        }

        public void ResetMovementObjectService(float velocityX, float velocityY, float angle, Vector3 startPosition)
        {
            _velocityX = velocityX;
            _velocityY = velocityY;
            _angle = angle;

            _movementObjectService.Reset(velocityX, velocityX, angle, startPosition);
        }

        public void AddMagnetOffset(Vector2 x)
        {
            _movementObjectService.AddSpeed(x);
        }

        public SlicableModelParams GetParams()
        {
            return new(Type, _velocityX, _velocityY, _angle, _modelAnimation);
        }
    }

    public record SlicableModelParams
    {
        public SlicableObjectType Type;
        public float VelocityX;
        public float VelocityY;
        public float Angle;
        public IModelAnimation ModelAnimation;

        public SlicableModelParams(SlicableObjectType type, float velocityX, float velocityY, float angle, IModelAnimation modelAnimation)
        {
            Type = type;
            VelocityX = velocityX;
            VelocityY = velocityY;
            Angle = angle;
            ModelAnimation = modelAnimation;
        }
    }
}