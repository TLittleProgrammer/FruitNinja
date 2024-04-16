using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Mouse
{
    public sealed class MouseManager : IAsyncInitializable<Camera>, ITickable
    {
        private Camera _camera;
        private Vector2 _previousMousePosition;
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
            CheckMouseButtonDown();
            CheckMouseButtonUp();

            if (_canCheckMousePositionDelta)
            {
                Vector2 currentMousePosition = GetMousePositionInWorldCoordinates();

                _canSlice = Vector2.Distance(currentMousePosition, _previousMousePosition) >= Constants.Game.MinRequiredDistanceBetweenMousePositions;
                _previousMousePosition = currentMousePosition;
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
                _canCheckMousePositionDelta = true;
            }
        }
    }
}