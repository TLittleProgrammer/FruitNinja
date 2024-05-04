using System.Collections.Generic;
using Runtime.Infrastructure.Mouse;
using Runtime.Infrastructure.SlicableObjects.MonoBehaviours;
using Runtime.Infrastructure.SlicableObjects.Services;
using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects.CollisionDetector
{
    public class CollisionDetector : ICollisionDetector<Collider2D, SlicableObjectView>
    {
        private readonly MouseManager _mouseManager;
        private readonly IIntermediateMousePositionsService _intermediateMousePositionsService;
        private readonly Slicer.Slicer _slicer;
        private readonly ISlicableObjectCounterOnMap _slicableObjectCounterOnMap;
        private readonly IMimikService _mimikService;

        private MappingColliderAndViewToList _colliders;

        public CollisionDetector(
            MouseManager mouseManager,
            IIntermediateMousePositionsService intermediateMousePositionsService,
            Slicer.Slicer slicer,
            ISlicableObjectCounterOnMap slicableObjectCounterOnMap,
            IMimikService mimikService
        )
        {
            _mouseManager = mouseManager;
            _intermediateMousePositionsService = intermediateMousePositionsService;
            _slicer = slicer;
            _slicableObjectCounterOnMap = slicableObjectCounterOnMap;
            _mimikService = mimikService;
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
                    float distance = Vector2.Distance(turpleArray[i].Item2.transform.position, mousePosition);
                    if (distance <= 0.75f)
                    {
                        if (TrySlice(turpleArray, ref i) is false)
                        {
                            return;
                        }
                        
                        break;
                    }
                }
            }
        }

        private bool TrySlice((Collider2D, SlicableObjectView)[] turpleArray, ref int i)
        {
            if (_slicer.TrySliceObject(turpleArray[i].Item2))
            {
                _slicableObjectCounterOnMap.RemoveType(turpleArray[i].Item2.SlicableObjectType);
                _colliders.RemoveItemWithCollider(turpleArray[i].Item1);
                _mimikService.RemoveMimik(turpleArray[i].Item2);

                return true;
            }

            return false;
        }

        public void AddCollider(Collider2D collider2D, SlicableObjectView slicableObjectView)
        {
            _slicableObjectCounterOnMap.AddType(slicableObjectView.SlicableObjectType);
            _colliders.Add((collider2D, slicableObjectView));
        }

        public void RemoveCollider(Collider2D collider)
        {
            _colliders.RemoveItemWithCollider(collider);
        }

        public void RemoveAllBoostColliders()
        {
            for (int i = 0; i < _colliders.Count; i++)
            {
                if (_colliders[i].Item2.SlicableObjectType is not SlicableObjectType.Simple)
                {
                    _colliders.RemoveAt(i);
                    i--;
                }
            }
        }

        public void SetNotDamagable()
        {
            for (int i = 0; i < _colliders.Count; i++)
            {
                _colliders[i].Item2.IsDamagable = false;
            }
        }
    }

    public sealed class MappingColliderAndViewToList : List<(Collider2D, SlicableObjectView)>
    {
        public void RemoveItemWithCollider(Collider2D collider2D)
        {
            (Collider2D, SlicableObjectView)[] arr = ToArray();
            
            foreach ((Collider2D, SlicableObjectView) turple in arr)
            {
                if (collider2D.Equals(turple.Item1))
                {
                    Remove(turple);
                    break;
                }
            }
        }
    }
}