using Runtime.Constants;
using Runtime.Extensions;
using Runtime.Infrastructure.SlicableObjects.Spawner;
using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects.Movement
{
    public class SlicableModel
    {
        private readonly Vector2 _direction;

        private float _speedX;
        private float _speedY;
        private float _currentAngle;
        private float _rotateSpeed;
        private float _speedYFormulaConstantValue;
        private float _time;
        private float _speedYMultiplier = 1f;

        public SlicableModel(float speedX, float speedY, Vector2 direction, Vector3 position, SideType sideType)
        {
            _speedX = speedX;
            _speedY = speedY;
            _direction = direction;
            Position = position;

            //TODO Потом перенести в настройки маркеров
            _currentAngle = Random.Range(0, 360f);
            _rotateSpeed  = Random.Range(20f, 50f) * GetRotateDirection();

            Initialize(direction, speedY, sideType);
        }

        public SlicableModel(float speedX, float speedY, Vector2 direction, Vector3 position, float angle, SideType sideType = SideType.None)
        {
            _speedX = speedX;
            _speedY = speedY;
            _direction = direction;
            Position = position;

            _currentAngle = angle;
            _rotateSpeed  = Random.Range(20f, 50f) * GetRotateDirection();
            
            Initialize(direction, speedY, sideType);
        }

        private void Initialize(Vector2 direction, float speed, SideType sideType)
        {
            float directionAngle = direction.GetAngleBetweenVectorAndHorizontalAxis();

            directionAngle += sideType.GetAngle();
            
            float sinus = Mathf.Abs(Mathf.Sin(directionAngle.ConvertToRadians()));

            _speedYFormulaConstantValue = speed * sinus;

            if (direction.y < 0f)
            {
                _speedYMultiplier = -1f;
            }
        }

        public Vector2 Position { get; private set; }

        public Quaternion Rotation => Quaternion.Euler(0f, 0f, _currentAngle);
        
        public void SimulateMoving()
        {
            float deltaTime = Time.deltaTime;

            _time += deltaTime;

            _speedY = _speedYFormulaConstantValue + World.Gravity * _time * _speedYMultiplier;
            
            Position += new Vector2(_speedX, _speedY) * deltaTime * _direction;
        }

        public void SimulateRotating()
        {
            if (Abs(_currentAngle) >= 360f)
            {
                _currentAngle = 0f;
            }

            _currentAngle += _rotateSpeed * Time.deltaTime;
        }

        private int GetRotateDirection()
        {
            return Random.Range(-1, 1) == -1 ? -1 : 1;
        }

        private float Abs(float value)
        {
            return value < 0 ? -value : value;
        }

        public SlicableModelParams GetParams()
        {
            return new(_direction, _speedX, _speedY, _currentAngle);
        }
    }

    public record SlicableModelParams
    {
        public Vector2 Direction;

        public float SpeedX;
        public float SpeedY;
        public float CurrentAngle;

        public SlicableModelParams(Vector2 direction, float speedX, float speedY, float currentAngle)
        {
            Direction = direction;
            SpeedX = speedX;
            SpeedY = speedY;
            CurrentAngle = currentAngle;
        }
    }
}