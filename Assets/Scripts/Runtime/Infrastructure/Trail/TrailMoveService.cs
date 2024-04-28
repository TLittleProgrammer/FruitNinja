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

        private bool _canTrail;
        private bool _canMove;

        public TrailMoveService(MouseManager mouseManager)
        {
            _mouseManager = mouseManager;
            _canTrail = true;
        }

        public async UniTask AsyncInitialize(TrailView trailViewTransform)
        {
            _trailView = trailViewTransform;
            TrailClear();
            
            await UniTask.CompletedTask;
        }

        public void Tick()
        {
            if (_trailView is null)
                return;
            
            if (_canTrail && Input.GetMouseButton(0))
            {
                if (_canMove)
                {
                    Vector3 targetPosition = _mouseManager.GetMousePositionInWorldCoordinates();
                
                    _trailView.transform.position = new Vector3(targetPosition.x, targetPosition.y, 0f);
                    
                    _trailView.TrailRenderer.enabled = true;
                }
            }
            
            CheckMouseButtonDown();
        }

        public void SetCanTrail(bool value)
        {
            _canTrail = value;
        }

        public void SetCannotMove()
        {
            _canMove = false;
        }

        private void CheckMouseButtonDown()
        {
            if (Input.GetMouseButtonDown(0))
            {
                TrailClear();
                _canMove = true;
            }
        }

        private void TrailClear()
        {
            _trailView.TrailRenderer.Clear();
        }
    }
}