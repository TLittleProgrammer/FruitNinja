using Cysharp.Threading.Tasks;
using Runtime.Infrastructure.Mouse;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Trail
{
    public class TrailMoveService : IAsyncInitializable<TrailView>, ITickable
    {
        private readonly MouseManager _mouseManager;
        private TrailView _trailView;

        public TrailMoveService(MouseManager mouseManager)
        {
            _mouseManager = mouseManager;
        }

        public async UniTask AsyncInitialize(TrailView trailViewTransform)
        {
            _trailView = trailViewTransform;
            
            await UniTask.CompletedTask;
        }

        public void Tick()
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 targetPosition = _mouseManager.GetMousePositionInWorldCoordinates();
                
                _trailView.transform.position = new Vector3(targetPosition.x, targetPosition.y, 0f);
            }
            
            CheckMouseButtonDown();
        }
        
        private void CheckMouseButtonDown()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _trailView.TrailRenderer.Clear();
            }
        }
    }
}