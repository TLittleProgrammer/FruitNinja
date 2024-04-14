using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Mouse
{
    public class MouseManager : IAsyncInitializable<Camera>, ITickable
    {
        private Camera _camera;

        private Vector2 _previousMousePosition;
        //TODO магическое число, вынести в настройки
        private float _minRequiredDistance = 0.001f;
        private bool _canSlice;
        private bool _canCheckMousePositionDelta;

        public async UniTask AsyncInitialize(Camera camera)
        {
            _camera = camera;
            _previousMousePosition = GetMousePositionInWorldCoordinates();
            
            await UniTask.CompletedTask;
        }

        public bool CanSlice => _canSlice;

        public void Tick()
        {
            //TODO Костылек. Подумать как можно инициализивать до вызова тиков
            if (_camera is null)
                return;

            CheckMouseButtonDown();
            CheckMouseButtonUp();

            if (_canCheckMousePositionDelta)
            {
                Vector2 currentMousePosition = GetMousePositionInWorldCoordinates();

                _canSlice = Vector2.Distance(currentMousePosition, _previousMousePosition) >= _minRequiredDistance;
                _previousMousePosition = currentMousePosition;
            }
        }

        private void CheckMouseButtonUp()
        {
            //TODO магическое число, вынести в настройки
            if (Input.GetMouseButtonUp(0))
            {
                _canCheckMousePositionDelta = false;
                _canSlice = false;
            }
        }

        private void CheckMouseButtonDown()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _canCheckMousePositionDelta = true;
            }
        }

        public Vector2 GetMousePositionInWorldCoordinates()
        {
            return _camera.ScreenToWorldPoint(Input.mousePosition);
        }

        public Vector2 GetMouseNormalizedDirection()
        {
            return (GetMousePositionInWorldCoordinates() - _previousMousePosition).normalized;
        }
    }
}