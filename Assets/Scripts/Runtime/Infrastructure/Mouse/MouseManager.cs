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
            if (_stop || _camera is null)
            {
                _canSlice = false;
                return;
            }

            if (_canCheckMousePositionDelta)
            {
                Vector2 currentMousePosition = GetMousePositionInWorldCoordinates();

                float speed = Vector2.Distance(currentMousePosition, _previousMousePosition) / Time.deltaTime;
                _canSlice = speed >= Constants.Game.MinRequiredSliceSpeed;
                
                _previousMousePositionForOther = _previousMousePosition;
                _previousMousePosition = currentMousePosition;
            }
            else
            {
                _canSlice = false;
            }
            
            CheckMouseButtonDown();
            CheckMouseButtonUp();
        }

        public void SetStopValue(bool value)
        {
            _stop = value;
        }
        
        public void SetCannotMouseCheckPosition()
        {
            _canCheckMousePositionDelta = false;
        }

        public Vector2 GetMousePositionInWorldCoordinates()
        {
            return _camera.ScreenToWorldPoint(Input.mousePosition);
        }
        
        public Vector2 GetMouseNormalizedDirection()
        {
            return (GetMousePositionInWorldCoordinates() - _previousMousePositionForOther).normalized;
        }

        public Vector2 GetPreviousMousePosition()
        {
            return _previousMousePositionForOther;
        }

        public Vector2 GetScreenPosition(Vector3 position)
        {
            return _camera.WorldToScreenPoint(position);
        }
        
        public Vector2 GetViewportPosition(Vector3 position)
        {
            return _camera.WorldToViewportPoint(position);
        }
        
        public Vector2 GetScreenPositionByViewport(Vector2 position)
        {
            return _camera.ViewportToScreenPoint(position);
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