using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Mouse
{
    public class MouseMoveService : IAsyncInitializable<Trail>, ITickable
    {
        private readonly MouseManager _mouseManager;
        
        private Trail _trail;

        public MouseMoveService(MouseManager mouseManager)
        {
            _mouseManager = mouseManager;
        }

        public async UniTask AsyncInitialize(Trail trailTransform)
        {
            _trail = trailTransform;
            
            await UniTask.CompletedTask;
        }

        public void Tick()
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 targetPosition = _mouseManager.GetMousePositionInWorldCoordinates();
                
                _trail.transform.position = new Vector3(targetPosition.x, targetPosition.y, 0f);
            }
            
            CheckMouseButtonDown();
            CheckMouseButtonUp();
        }

        private void CheckMouseButtonUp()
        {
            if (Input.GetMouseButtonUp(0))
            {
                _trail.SpriteRenderer.enabled = false;
                _trail.TrailRenderer.emitting = false;
            }
        }

        private void CheckMouseButtonDown()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _trail.SpriteRenderer.enabled = true;
                _trail.TrailRenderer.emitting = true;
            }
        }
    }
}