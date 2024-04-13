using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Mouse
{
    public class MouseMoveService : IAsyncInitializable<Trail, Camera>, ITickable
    {
        private Trail _trail;
        private Camera _camera;

        public async UniTask AsyncInitialize(Trail trailTransform, Camera camera)
        {
            _camera = camera;
            _trail = trailTransform;
            
            await UniTask.CompletedTask;
        }

        public void Tick()
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 targetPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                
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