using Runtime.Infrastructure.SlicableObjects.Movement.Animation;
using Runtime.Infrastructure.SlicableObjects.Movement.MoveLogic;
using Runtime.Infrastructure.SlicableObjects.Spawner;
using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects.Movement
{
    public class SlicableModel
    {
        private IMovementObject _movementObject;
        private IModelAnimation _modelAnimation;

        public SlicableModel(Transform movementTransform, float speedX, float speedY, Vector2 direction, IModelAnimation modelAnimation, SideType sideType = SideType.None)
        {
            _movementObject = new MovementObjectService(movementTransform, speedX, speedY, direction, sideType);
            _modelAnimation = modelAnimation;
        }

        public Quaternion Rotation => Quaternion.Euler(0f, 0f, _modelAnimation.Rotation);
        public Vector2 Position =>_movementObject.Position;

        public void Tick()
        {
            _movementObject.SimulateMovement();
            _modelAnimation.SimulateAnimation();
        }

        public void AddDirection(Vector2 direction)
        {
            _movementObject.Direction = direction;
        }

        public SlicableModelParams GetParams()
        {
            return new(_movementObject.Direction, _movementObject.SpeedX, _movementObject.SpeedY, _modelAnimation);
        }
    }

    public record SlicableModelParams
    {
        public Vector2 Direction;
        
        public float SpeedX;
        public float SpeedY;
        public IModelAnimation ModelAnimation;

        public SlicableModelParams(Vector2 direction, float speedX, float speedY, IModelAnimation modelAnimation)
        {
            Direction = direction;
            SpeedX = speedX;
            SpeedY = speedY;
        }
    }
}