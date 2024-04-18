using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Mouse
{
    public sealed class MouseManager : IAsyncInitializable<Camera>, ITickable
    {
        private Camera _camera;
        private Vector2 _previousMousePosition;
        private Vector2 _previousMousePositionForOther;
        private bool _stop;
        private bool _canSlice;
        private bool _canCheckMousePositionDelta;

        public async UniTask AsyncInitialize(Camera camera)
        {
            _camera = camera;
            _previousMousePosition = GetMousePositionInWorldCoordinates();
            _stop = false;
            
            await UniTask.CompletedTask;
        }

        public bool CanSlice => _canSlice && _stop is false;

        public void Tick()
        {
            if (_stop)
                return;

            if (_canCheckMousePositionDelta)
            {
                Vector2 currentMousePosition = GetMousePositionInWorldCoordinates();

                float speed = Vector2.Distance(currentMousePosition, _previousMousePosition) / Time.deltaTime;
                _canSlice = speed >= Constants.Game.MinRequiredSliceSpeed;
                
                _previousMousePositionForOther = _previousMousePosition;
                _previousMousePosition = currentMousePosition;
            }
            
            CheckMouseButtonDown();
            CheckMouseButtonUp();
        }

        public void SetStopValue(bool value)
        {
            _stop = value;
        }

        public Vector2 GetMousePositionInWorldCoordinates()
        {
            return _camera.ScreenToWorldPoint(Input.mousePosition);
        }
        
        public Vector2 GetMouseNormalizedDirection()
        {
            return (GetMousePositionInWorldCoordinates() - _previousMousePosition).normalized;
        }

        public Vector2 GetPreviousMousePosition()
        {
            return _previousMousePositionForOther;
        }

        private void CheckMouseButtonUp()
        {
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
                _previousMousePosition = GetMousePositionInWorldCoordinates();
                _previousMousePositionForOther = _previousMousePosition;
                
                _canCheckMousePositionDelta = true;
                _canSlice = false;
            }
        }
    }
}