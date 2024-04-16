using System.Collections.Generic;
using Runtime.Infrastructure.Mouse;
using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects.CollisionDetector
{
    public class CollisionDetector : ICollisionDetector<Collider2D, SlicableObjectView>
    {
        private readonly MouseManager _mouseManager;
        private readonly IIntermediateMousePositionsService _intermediateMousePositionsService;
        private readonly Slicer _slicer;

        private MappingColliderAndViewToList _colliders;

        public CollisionDetector(
            MouseManager mouseManager,
            IIntermediateMousePositionsService intermediateMousePositionsService,
            Slicer slicer
            )
        {
            _mouseManager = mouseManager;
            _intermediateMousePositionsService = intermediateMousePositionsService;
            _slicer = slicer;
            _colliders = new();
        }

        public void LateTick()
        {
            if (_mouseManager.CanSlice)
            {
                Vector2 previousMousePosition = _mouseManager.GetPreviousMousePosition();
                Vector2 currentMousePosition = _mouseManager.GetMousePositionInWorldCoordinates();

                Vector2[] mousePositions = _intermediateMousePositionsService.GetIntermediateMousePositions(previousMousePosition, currentMousePosition, Time.deltaTime);

                GoThrowAllCollidersAndMousePositions(mousePositions);
            }
        }

        private void GoThrowAllCollidersAndMousePositions(Vector2[] mousePositions)
        {
            (Collider2D, SlicableObjectView)[] turpleArray = _colliders.ToArray();
            
            for (int i = 0; i < turpleArray.Length; i++)
            {
                foreach (Vector2 mousePosition in mousePositions)
                {
                    if (turpleArray[i].Item1.OverlapPoint(mousePosition))
                    {
                        _slicer.SliceObject(turpleArray[i].Item2);
                        
                        _colliders.Remove(turpleArray[i]);
                        i--;
                        
                        break;
                    }
                }
            }
        }

        public void AddCollider(Collider2D collider2D, SlicableObjectView slicableObjectView)
        {
            _colliders.Add((collider2D, slicableObjectView));
        }

        public void RemoveCollider(Collider2D collider)
        {
            _colliders.RemoveItemWithCollider(collider);
        }
    }

    public sealed class MappingColliderAndViewToList : List<(Collider2D, SlicableObjectView)>
    {
        public void RemoveItemWithView(SlicableObjectView slicableObjectView)
        {
            foreach ((Collider2D, SlicableObjectView) turple in this)
            {
                if (slicableObjectView.Equals(turple.Item2))
                {
                    Remove(turple);
                }
            }
        }
        
        public void RemoveItemWithCollider(Collider2D collider2D)
        {
            foreach ((Collider2D, SlicableObjectView) turple in this)
            {
                if (collider2D.Equals(turple.Item1))
                {
                    Remove(turple);
                }
            }
        }
    }
}