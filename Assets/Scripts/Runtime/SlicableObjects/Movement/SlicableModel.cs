using UnityEngine;

namespace Runtime.SlicableObjects.Movement
{
    public class SlicableModel
    {
        private readonly Vector2 _direction;
        private readonly float _gravity;

        private float _speedX;
        private float _speedY;
        private float _currentAngle;
        private float _rotateSpeed;

        public SlicableModel(float speedX, float speedY, Vector2 direction, Vector3 position)
        {
            _speedX = speedX;
            _speedY = speedY;
            _direction = direction;
            Position = position;
            
            _gravity = Physics.gravity.y / 2f;

            //TODO Потом перенести в настройки маркеров
            _currentAngle = Random.Range(0, 360f);
            _rotateSpeed  = Random.Range(20f, 50f) * GetRotateDirection();
        }

        public SlicableModel(float speedX, float speedY, Vector2 direction, Vector3 position, float angle)
        {
            _speedX = speedX;
            _speedY = speedY;
            _direction = direction;
            Position = position;
            
            _gravity = Physics.gravity.y / 2f;

            _currentAngle = angle;
            _rotateSpeed  = Random.Range(20f, 50f) * GetRotateDirection();
        }
        
        public Vector2 Position { get; private set; }

        public Quaternion Rotation => Quaternion.Euler(0f, 0f, _currentAngle);

        public void SimulateMoving()
        {
            float deltaTime = Time.deltaTime;
            
            Position += new Vector2(_speedX, _speedY) * deltaTime * _direction;

            _speedY += _gravity * deltaTime;
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